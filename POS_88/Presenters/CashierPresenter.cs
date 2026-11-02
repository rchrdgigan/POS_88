using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Presenters
{
    public sealed class CashierPresenter
    {
        private readonly ICashierView _view;
        private readonly ICustomerFacingDisplay _display;
        private readonly POSImplementation _pos = new POSImplementation();
        private readonly List<CartLine> _cart = new List<CartLine>();

        private UnitOfWorkPOS _uow = NewUow();
        private setting _settings;
        private decimal _vatPct;
        private string _priceType = "RETAIL";
        private decimal _redeemAmt;
        private decimal _pointsRedeem;
        private PaymentModel _payment;
        private int _zno;
        private bool _entryEnabled = true;
        private bool _resultsVisible;
        private CartTotals _totals = new CartTotals();

        public int ReceiptNo { get; private set; }

        private static UnitOfWorkPOS NewUow() => new UnitOfWorkPOS(new POSDBEntities());

        public CashierPresenter(ICashierView view, ICustomerFacingDisplay display)
        {
            _view = view;
            _display = display;

            _view.SearchTextChanged += (s, e) => Search();
            _view.SearchSubmitted += (s, e) => OnSearchSubmitted();
            _view.SearchResultChosen += (s, code) => { AddItem(code); _view.ClearSearch(); };
            _view.PriceTypeSelected += (s, p) => SetPriceType(p);
            _view.TogglePriceTypeRequested += (s, e) => SetPriceType(_priceType == "RETAIL" ? "WHOLESALE" : "RETAIL");
            _view.ToggleSearchModeRequested += (s, e) => ToggleSearchMode();
            _view.EditItemRequested += (s, e) => EditSelectedItem();
            _view.RemoveRequested += (s, e) => RemoveSelected();
            _view.DiscountRequested += (s, e) => RequestDiscount();
            _view.QuantityRequested += (s, e) => RequestQuantity();
            _view.NewSaleRequested += (s, e) => { if (_view.Confirm("Do you want to clear all items?")) StartNewSale(); };
            _view.TenderRequested += (s, e) => RequestTender();
            _view.ReturnRequested += (s, e) => ReturnItems();
            _view.DialogRequested += (s, d) => OnDialogRequested(d);

            Initialize();
        }

        private void Initialize()
        {
            _settings = _uow.Setting.Find(x => x.settingsid == 1).FirstOrDefault();
            _display.ConfigurePole(_settings.enablepoledisplay.GetValueOrDefault(), _settings.poleport,
              _settings.defaultTopDisplay, _settings.defaultBottomDisplay);

            var vat = _uow.VATStatus.Find(x => x.vatid == "YES").FirstOrDefault(); // Optional
            _vatPct = Convert.ToDecimal((object)vat.percentage);

            _view.SearchMode = SearchMode.ItemCodeAndDescription;
            SetPriceType("RETAIL");
            Refresh(false);
        }

        // ------------------------------------------------------------ search

        private void Search()
        {
            string text = _view.SearchText;
            if (string.IsNullOrEmpty(text))
            {
                _resultsVisible = false;
                _view.ShowSearchResults(new List<SearchResultItem>(), false);
                return;
            }

            string by = _view.SearchMode == SearchMode.ItemCodeAndDescription ? "ITEM CODE AND NAME" : "BARCODE";
            if (_view.SearchMode == SearchMode.Barcode && text.Contains("*"))
            {
                var parts = text.Split('*');
                if (parts.Length > 1) text = parts[1];
            }

            var items = _pos.SearchItem(by, text).Select(r => new SearchResultItem
            {
                ItemCode = r.ITEM_CODE,
                Name = r.NAME,
                Price = Convert.ToDecimal(_priceType == "RETAIL" ? (object)r.RETAIL_PRICE
                                      : _priceType == "PROMO" ? (object)r.PROMO_PRICE
                                      : (object)r.WHOLESALE_PRICE)
            }).ToList();

            _resultsVisible = items.Count > 0 && by != "BARCODE";
            _view.ShowSearchResults(items, _resultsVisible);
            _lastFirstResult = items.FirstOrDefault()?.ItemCode;
        }
        private string _lastFirstResult;

        private void OnSearchSubmitted()
        {
            string text = _view.SearchText;
            if (string.IsNullOrEmpty(text)) return;

            if (_view.SearchMode == SearchMode.ItemCodeAndDescription)
            {
                if (!_resultsVisible || _lastFirstResult == null) return;
                AddItem(_lastFirstResult);
            }
            else AddItem(text);

            _view.ClearSearch();
        }

        private void ToggleSearchMode()
        {
            if (_view.SearchMode == SearchMode.Barcode) _view.SearchMode = SearchMode.ItemCodeAndDescription;
            else if (_view.SearchMode == SearchMode.ItemCodeAndDescription) _view.SearchMode = SearchMode.Barcode;
            _view.FocusSearch();
        }

        private void SetPriceType(string priceType)
        {
            _priceType = priceType;
            _view.SetPriceType(priceType);
            _view.FocusSearch();
        }

        // ------------------------------------------------------------ cart

        private bool TryParseScan(string raw, out string code, out decimal qty)
        {
            code = raw;
            qty = 1m;
            if (_view.SearchMode == SearchMode.BarcodeWeighingScale)
            {
                int codeFrom = Convert.ToInt32((object)_settings.itemcodefrom);
                int codeLen = Convert.ToInt32((object)_settings.itemcodeto);
                int wFrom = Convert.ToInt32((object)_settings.weightfrom);
                int wLen = Convert.ToInt32((object)_settings.weightto);
                int dec = Convert.ToInt32((object)_settings.nodecimal);
                code = raw.Substring(codeFrom - 1, codeLen);
                qty = Convert.ToDecimal(raw.Substring(wFrom - 1, wLen - dec) + "." + raw.Substring(wFrom + wLen - dec - 1, dec));
            }
            else if (_view.SearchMode == SearchMode.Barcode && raw.Contains("*"))
            {
                var p = raw.Split('*');
                qty = Convert.ToDecimal(p[0]);
                code = p[1];
            }
            return true;
        }

        private static decimal PriceFor(inventory i, string pt) =>
          pt == "RETAIL" ? i.price.GetValueOrDefault()
          : pt == "PROMO" ? i.promoprice.GetValueOrDefault()
          : i.wholesaleprice.GetValueOrDefault();

        public void AddItem(string raw)
        {
            decimal qty;
            string code = raw;
            try
            {
                TryParseScan(raw, out code, out qty);
                _uow = NewUow();
                string c = code;
                var inv = _uow.Inventory.Find(x => x.itemcode == c).FirstOrDefault();

                if (inv == null)
                {
                    if (_view.SearchMode != SearchMode.ItemCodeAndDescription) _view.ShowItemNotFound(code);
                    return;
                }

                var line = _cart.FirstOrDefault(l => l.ItemCode == code && l.PriceType == _priceType);
                if (line != null)
                {
                    line.Quantity += qty;
                    line.Reprice();
                    _display.LineUpdated(line);
                }
                else
                {
                    line = new CartLine
                    {
                        ItemCode = inv.itemcode,
                        Description = inv.itemname,
                        Price = PriceFor(inv, _priceType),
                        Quantity = qty,
                        VatStatus = inv.vatstatus,
                        Cost = Convert.ToDecimal((object)inv.cost),
                        CurrentQty = Convert.ToDecimal((object)inv.quantity),
                        PriceType = _priceType
                    };
                    line.Reprice();
                    _cart.Add(line);
                    _display.LineAdded(line);
                }
                Refresh(true, _cart.IndexOf(line));
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
                if (_view.SearchMode == SearchMode.BarcodeWeighingScale) _view.ShowItemNotFound(code);
            }
        }

        public void ApplyDiscount(string itemCode, string discountMode, string scope, decimal value, string priceType, string discountName)
        {
            foreach (var l in _cart)
            {
                bool match = scope == "APPLY TO ALL ITEMS"
                  || (scope == "APPLY TO SELECTED ITEM" && l.ItemCode == itemCode && l.PriceType == priceType);
                if (!match) continue;
                l.DiscountMode = discountMode;
                l.DiscountValue = value;
                l.DiscountType = discountName;
                l.Rediscount();
                _display.LineUpdated(l);
            }
            Refresh(true, _view.SelectedLineIndex);
        }

        public void AdjustQty(string itemCode, decimal newQty, decimal price, decimal total, string priceType,
                              string note, decimal width, decimal height)
        {
            var l = _cart.FirstOrDefault(x => x.ItemCode == itemCode && x.PriceType == priceType);
            if (l == null) return;
            l.Quantity = newQty;
            l.Price = price;
            l.Total = total;       // dialog may price by area (width x height), so don't recompute
            l.Note = note;
            l.Width = width;
            l.Height = height;
            l.Rediscount();
            _display.LineUpdated(l);
            Refresh(true, _cart.IndexOf(l));
        }

        /// <summary>Called by the item editor after an item's master data changed.</summary>
        public void UpdateItem(inventory item)
        {
            foreach (var l in _cart.Where(x => x.ItemCode == item.itemcode))
            {
                l.Description = item.itemname;
                l.Price = PriceFor(item, l.PriceType);
                l.Cost = Convert.ToDecimal((object)item.cost);
                l.VatStatus = item.vatstatus;
                l.Reprice();
                _display.LineUpdated(l);
            }
            Refresh(true, _view.SelectedLineIndex);
        }

        private CartLine Selected()
        {
            int i = _view.SelectedLineIndex;
            return i >= 0 && i < _cart.Count ? _cart[i] : null;
        }

        private void RemoveSelected()
        {
            var line = Selected();
            if (!_entryEnabled || line == null) return;

            string userNm = _view.UserName;
            var user = _uow.User.Find(x => x.usernm == userNm).FirstOrDefault();
            if (user.usertype == "ADMIN" || user.usertype == "CASHIER")
                RemoveCurrentItem();
            else _view.ShowVoidItemDialog(line);   // supervisor approval, dialog calls RemoveCurrentItem()
        }

        /// <summary>Also called by the void-item dialog once approved.</summary>
        public void RemoveCurrentItem()
        {
            int i = _view.SelectedLineIndex;
            if (i < 0 || i >= _cart.Count) return;
            var l = _cart[i];
            _display.LineRemoved(l.ItemCode, l.PriceType);
            _cart.RemoveAt(i);
            Refresh(true, Math.Min(i, _cart.Count - 1));
        }

        private void RequestDiscount()
        {
            var l = Selected();
            if (_entryEnabled && l != null) _view.ShowDiscountDialog(l.ItemCode, l.PriceType);
        }

        private void RequestQuantity()
        {
            var l = Selected();
            if (_entryEnabled && l != null) _view.ShowQuantityDialog(l);
        }

        private void EditSelectedItem()
        {
            var l = Selected();
            if (!_entryEnabled || l == null) return;
            string userNm = _view.UserName;
            var user = _uow.User.Find(x => x.usernm == userNm).FirstOrDefault();
            _view.ShowEditItemDialog(l.ItemCode, user.usertype == "ADMIN");
        }

        // ------------------------------------------------------------ totals

        private CartTotals ComputeTotals()
        {
            var t = new CartTotals();
            decimal vatable = 0m, due = 0m;
            foreach (var l in _cart)
            {
                t.SubTotal += l.Total;
                t.Discount += l.DiscountAmt;
                t.NoOfItems += l.Quantity;
                t.TotalCost += l.Cost * l.Quantity;
                due += l.TotalDue;
                if (l.VatStatus == "YES") vatable += l.TotalDue;
                else if (l.VatStatus == "NO") t.NonVatSales += l.TotalDue;
                else t.VatExemptSales += l.TotalDue;
            }
            t.VatAmount = Math.Round(vatable * (_vatPct / (100m + _vatPct)), 2);
            t.VatSales = Math.Round(vatable - t.VatAmount, 2);
            t.AmountDue = Math.Round(due - _redeemAmt, 2);
            return t;
        }

        private void Refresh(bool writePole, int selectIndex = -1)
        {
            _totals = ComputeTotals();
            _view.BindCart(_cart, selectIndex);
            _view.ShowTotals(_totals);
            _display.ShowTotals(_totals, writePole);
        }

        // ------------------------------------------------------------ payment

        private void RequestTender()
        {
            if (!_entryEnabled || _cart.Count == 0) return;

            string userNm = _view.UserName;
            var shift = _uow.CashCount.Find(x => x.UserNm == userNm && x.IsClosed == false).FirstOrDefault();
            if (shift == null)
            {
                _view.ShowMessage("Cannot process transaction, Kindly start your shift first!", "Not Allowed");
                return;
            }
            _zno = shift.SqNo;
            _view.ShowPaymentDialog(_totals.AmountDue);
        }

        /// <summary>Called by the payment dialog when the cashier confirms.</summary>
        public void ConfirmPayment(PaymentModel payment)
        {
            _payment = payment;
            _redeemAmt = payment.pointsAmt;
            _pointsRedeem = payment.pointsRedeem;
            _view.ShowPaymentSummary(payment.paymentAmt, payment.changeAmt, payment.pointsAmt);
            _totals = ComputeTotals();
            _view.ShowTotals(_totals);
            SaveInvoice(payment.invoicetype);
            _view.ShowReceiptDialog(payment);
        }

        private void ReturnItems()
        {
            if (!_view.Confirm("Are you sure you want to return items?")) return;
            if (_payment == null)
            {
                _view.ShowMessage("Complete a payment first before returning items.", "Return");
                return;
            }
            SaveInvoice("RETURN");
            _view.OpenDialog(CashierDialog.Reprint, ReceiptNo);
        }

        private void SaveInvoice(string tranType)
        {
            var now = DateTime.Now;
            var pm = _payment;
            var t = _totals;

            int custId = pm.customerid;
            var customer = _uow.Customers.Find(x => x.customerid == custId).FirstOrDefault();

            var inv = new invoice();
            if (ReceiptNo != 0)
            {
                int rno = ReceiptNo;
                inv = _uow.Invoice.Find(x => x.receiptno == rno).FirstOrDefault();
            }

            inv.solddate = now;
            inv.customerid = pm.customerid;
            inv.tableno = pm.tablenm;
            inv.paymenttype = pm.paymenttype;
            inv.cost = t.TotalCost;
            inv.subtotal = t.SubTotal;
            inv.subdiscount = t.Discount;
            inv.amountdue = t.AmountDue;
            inv.vatsales = t.VatSales;
            inv.vatamount = t.VatAmount;
            inv.nonvatsales = t.NonVatSales;
            inv.vatexemptsales = t.VatExemptSales;
            inv.zeroratedsales = t.ZeroRatedSales;
            inv.noofitems = t.NoOfItems;
            inv.pointsRedeem = _pointsRedeem;
            inv.previouspoints = customer.loyaltypoints;
            inv.loyaltyamt = _redeemAmt;
            inv.note = pm.note;
            inv.drivername = pm.driverName;
            inv.cashtender = pm.paymentAmt;
            inv.ZReadNo = _zno;

            // loyalty
            decimal earned = t.AmountDue / _settings.amountspent / _settings.pointsearned;
            inv.earned = earned;
            customer.loyaltypoints = customer.loyaltypoints.HasValue
              ? customer.loyaltypoints + (earned - _pointsRedeem)
              : (decimal?)null;
            inv.currentpoints = customer.loyaltypoints;
            if (customer.customerid == 1)   // walk-in / guest customer never earns points
            {
                customer.loyaltypoints = 0m;
                inv.currentpoints = 0m;
                inv.earned = 0m;
                inv.previouspoints = 0m;
                inv.loyaltyamt = 0m;
            }
            _pos.SaveCustomer(customer, "UPDATE");
            _uow.Customers.Update(customer);

            inv.amountpaid = pm.status == "PAID" ? t.AmountDue : pm.paymentAmt;
            inv.changeamt = pm.changeAmt;
            inv.paymentstatus = pm.status;
            inv.balance = pm.balance;
            inv.invoicetype = tranType;
            inv.crtdby = _view.UserName;
            inv.crtddt = now;

            if (ReceiptNo == 0)
            {
                _uow.Invoice.Add(inv);
                _uow.Complete();
                var r = _uow.Invoice.GetReceiptNo(_view.UserName).FirstOrDefault();
                ReceiptNo = r.receiptno;
            }
            else
            {
                _uow.Invoice.Update(inv);
                _uow.Complete();
                _uow.Inventory.VoidItemQty(ReceiptNo, "", 0);
            }
            pm.receiptno = ReceiptNo;

            foreach (var l in _cart)
            {
                var sale = new itemsale
                {
                    receiptno = ReceiptNo,
                    itemcode = l.ItemCode,
                    description = l.Description,
                    costprice = l.Cost,
                    price = l.Price,
                    quantity = l.Quantity,
                    total = l.Total,
                    discountdesc = l.DiscountText,
                    discountamt = l.DiscountAmt,
                    totaldue = l.TotalDue,
                    vatstatus = l.VatStatus,
                    pricetype = l.PriceType,
                    discounttype = l.DiscountType,
                    note = l.Note
                };
                _uow.ItemSales.Add(sale);
                _uow.Complete();
                if (tranType == "SALES")
                {
                    _uow.Inventory.UpdateQty(sale.itemcode, l.Quantity);
                    _uow.StockHistory.LogStocksMovement("DEDUCT", "Sales", sale.receiptno, inv.customerid,
                      sale.itemcode, sale.quantity, inv.crtdby);
                }
            }

            decimal due = t.AmountDue, cash = pm.paymentAmt;
            var history = new paymenthistory
            {
                receiptno = inv.receiptno,
                paymode = inv.paymenttype,
                amount = cash >= due ? due : cash,
                balance = cash >= due ? 0m : due - cash,
                cashtender = inv.cashtender,
                paydate = inv.solddate,
                usernm = _view.UserName,
                readflag = false,
                remarks = pm.note
            };
            _uow.Payment.Add(history);
            _uow.Complete();

            _display.ShowPayment(pm.paymentAmt, pm.changeAmt);
        }

        // ------------------------------------------------------------ session

        public void StartNewSale()
        {
            _cart.Clear();
            _redeemAmt = 0m;
            _pointsRedeem = 0m;
            _payment = null;
            ReceiptNo = 0;
            _entryEnabled = true;
            _display.Reset();
            _view.SetEntryEnabled(true);
            _view.ShowPaymentSummary(0m, 0m, 0m);
            _view.ClearSearch();
            Refresh(false);
            _view.FocusSearch();
        }

        /// <summary>Lock the screen after payment (called by the receipt dialog).</summary>
        public void ClearEntries()
        {
            _entryEnabled = false;
            _view.ClearSearch();
            _view.SetEntryEnabled(false);
        }

        /// <summary>Supervisor override: re-enable editing of a paid sale.</summary>
        public void UnlockVoid()
        {
            _entryEnabled = true;
            _view.SetEntryEnabled(true);
            _view.ClearSearch();
            _view.FocusSearch();
        }

        private void OnDialogRequested(CashierDialog d)
        {
            if (d == CashierDialog.Reprint && ReceiptNo == 0) return;
            _view.OpenDialog(d, ReceiptNo);
        }
    }

}
