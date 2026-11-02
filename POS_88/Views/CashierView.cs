using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using POS_88.Database;    // inventory
using POS_88.Presenters;

/*  Mapping used:
 *  Tender Cash button  -> iconButton1        Z-Reading button -> btnVoidSales
 *  Stocks button       -> iconButton2        Total due label  -> lblAmountDue
 *  dataGrid has no columns in the designer, so InitializeGrid() creates them in code.
 */
namespace POS_88.Views
{
    /// <summary>Passive view: raises events and renders what the presenter hands it. No business logic.</summary>
    public partial class CashierView : Form, ICashierView, ICustomerFacingDisplay
    {
        private readonly CashierPresenter _presenter;
        private readonly Timer _searchTimer = new Timer { Interval = 250 };
        private CustomerDisplayView _frmCustDisplay = new CustomerDisplayView();
        private SerialPort _sp;
        private bool _poleEnabled;
        private string _poleTop, _poleBottom, _itemSel;
        private readonly string _userName;
        public string userNm;

        public CashierView() : this("") { }

        public CashierView(string userName)
        {
            _userName = userName;
            userNm = userName;
            InitializeComponent();
            ApplyTheme();
            InitializeSearchList();
            InitializeGrid();
            cboSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSearchBy.SelectedIndexChanged += (s, e) => txtSearch.Focus();
            label42.Visible = btnReturnSales.Visible;   // stray "Crt + 7" label belongs to the hidden Return button

            btnClose.Click += (s, e) => Close();
            btnMinimize.Click += (s, e) => WindowState = FormWindowState.Minimized;
            FormClosed += (s, e) => { if (_poleEnabled) _sp.Close(); };

            // price type buttons
            btnRetail.Click += NavButton_Click;
            btnWholesale.Click += NavButton_Click;
            btnPromo.Click += NavButton_Click;

            // search
            txtSearch.TextChanged += (s, e) => { _searchTimer.Stop(); _searchTimer.Start(); };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); SearchTextChanged?.Invoke(this, EventArgs.Empty); };
            txtSearch.KeyDown += txtSearch_KeyDown;
            txtSearch.MouseHover += (s, e) => txtSearch.Focus();
            listViewItems.MouseHover += (s, e) => listViewItems.Focus();
            listViewItems.SelectedIndexChanged += (s, e) =>
            {
                if (listViewItems.FocusedItem != null) _itemSel = listViewItems.FocusedItem.Name;
            };
            listViewItems.Click += (s, e) =>
            {
                if (listViewItems.SelectedItems.Count > 0)
                    SearchResultChosen?.Invoke(this, listViewItems.SelectedItems[0].Name);
            };
            listViewItems.KeyDown += listViewItems_KeyDown;

            // cart actions
            btEditItem.Click += (s, e) => Raise(EditItemRequested);
            btCancel.Click += (s, e) => Raise(RemoveRequested);
            btDiscount.Click += (s, e) => Raise(DiscountRequested);
            btQuantity.Click += (s, e) => Raise(QuantityRequested);
            btnNew.Click += (s, e) => Raise(NewSaleRequested);
            iconButton1.Click += (s, e) => Raise(TenderRequested);
            btnReturnSales.Click += (s, e) => Raise(ReturnRequested);

            // dialogs
            btnCashCount.Click += (s, e) => Raise(DialogRequested, CashierDialog.CashCount);
            btnRecentSales.Click += (s, e) => Raise(DialogRequested, CashierDialog.RecentSales);
            btnReprint.Click += (s, e) => Raise(DialogRequested, CashierDialog.Reprint);
            btnVoidSales.Click += (s, e) => Raise(DialogRequested, CashierDialog.ZReading);
            btnPickItem.Click += (s, e) => Raise(DialogRequested, CashierDialog.PickItem);
            iconButton2.Click += (s, e) => Raise(DialogRequested, CashierDialog.Stocks);
            btnCustomerDisplay.Click += (s, e) => Raise(DialogRequested, CashierDialog.CustomerDisplay);

            _presenter = new CashierPresenter(this, this);
        }

        private void InitializeSearchList()
        {
            // drop-down that opens right under the search box (inside panelLeft)
            listViewItems.View = View.Details;
            listViewItems.FullRowSelect = true;
            listViewItems.HoverSelection = true;
            listViewItems.HeaderStyle = ColumnHeaderStyle.None;
            listViewItems.BorderStyle = BorderStyle.FixedSingle;
            listViewItems.Font = new Font("Century Gothic", 11f);
            listViewItems.Columns.Add("Name");
            listViewItems.Columns.Add("Price");
            listViewItems.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listViewItems.Location = new Point(bunifuCards16.Left, bunifuCards16.Bottom);
            listViewItems.Size = new Size(bunifuCards16.Width, 300);
            listViewItems.Visible = false;
            listViewItems.BringToFront();
        }

        private void InitializeGrid()
        {
            dataGrid.AutoGenerateColumns = false;
            dataGrid.Columns.Clear();
            AddColumn("ITEMCODE", "ITEM CODE", true, false);
            AddColumn("DESCRIPTION", "DESCRIPTION", true, false);
            AddColumn("PRICE", "PRICE", true, true);
            AddColumn("QUANTITY", "QTY", true, true);
            AddColumn("TOTAL", "TOTAL", true, true);
            AddColumn("DISCOUNT", "DISCOUNT", true, false);
            AddColumn("TOTALDUE", "TOTAL DUE", true, true);
            AddColumn("VATSTATUS", "VAT", true, false);
            AddColumn("TYPEOFPRICE", "PRICE TYPE", true, false);
            AddColumn("NOTE", "NOTE", true, false);
            // hidden data the presenter / dialogs still need
            AddColumn("DISCOUNTAMT", "DISCOUNTAMT", false, true);
            AddColumn("COST", "COST", false, true);
            AddColumn("DISCOUNTTYPE", "DISCOUNTTYPE", false, false);
            AddColumn("PWIDTH", "WIDTH", false, true);
            AddColumn("PHEIGHT", "HEIGHT", false, true);
            AddColumn("CURRENTQTY", "CURRENT QTY", true, false);

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.Columns["DESCRIPTION"].FillWeight = 220;
            dataGrid.AllowUserToResizeRows = false;
        }

        private void AddColumn(string name, string header, bool visible, bool numeric)
        {
            var c = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Visible = visible,
                ReadOnly = true,
                MinimumWidth = 8,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            if (numeric)
            {
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGrid.Columns.Add(c);
        }

        private void ApplyTheme()
        {
            POSTheme.ApplySidebar(panelLogo);
            POSTheme.ApplySidebar(bunifuCardsSidebar);
            POSTheme.ApplyTopbar(panel2);
            POSTheme.ApplyTopbar(panelBottom);

            lblDateTime.ForeColor = POSTheme.Text;
            lblDateTime.BackColor = Color.Transparent;

            btnMinimize.BackColor = POSTheme.Navy;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.UseVisualStyleBackColor = false;

            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        // ===================================================== ICashierView: state

        public string UserName => _userName;

        public string SearchText
        {
            get { return txtSearch.Text; }
            set { txtSearch.Text = value; } 
        }

        public SearchMode SearchMode
        {
            get { return cboSearchBy.Text == "BARCODE" ? SearchMode.Barcode
                     : cboSearchBy.Text == "BARCODE WEIGHING SCALE" ? SearchMode.BarcodeWeighingScale
                     : SearchMode.ItemCodeAndDescription;
            } 
                set {
                cboSearchBy.SelectedIndex = value == SearchMode.ItemCodeAndDescription ? 0
                                           : value == SearchMode.Barcode ? 1 : 2;
            } 
        }

            public int SelectedLineIndex => dataGrid.CurrentRow?.Index ?? -1;

        // ===================================================== ICashierView: events

        public event EventHandler SearchTextChanged;
        public event EventHandler SearchSubmitted;
        public event EventHandler<string> SearchResultChosen;
        public event EventHandler<string> PriceTypeSelected;
        public event EventHandler TogglePriceTypeRequested;
        public event EventHandler ToggleSearchModeRequested;
        public event EventHandler EditItemRequested;
        public event EventHandler RemoveRequested;
        public event EventHandler DiscountRequested;
        public event EventHandler QuantityRequested;
        public event EventHandler NewSaleRequested;
        public event EventHandler TenderRequested;
        public event EventHandler ReturnRequested;
        public event EventHandler<CashierDialog> DialogRequested;

        private void Raise(EventHandler h) => h?.Invoke(this, EventArgs.Empty);
        private void Raise(EventHandler<CashierDialog> h, CashierDialog d) => h?.Invoke(this, d);

        private void NavButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            string type = button == btnRetail ? "RETAIL" : button == btnPromo ? "PROMO" : "WHOLESALE";
            PriceTypeSelected?.Invoke(this, type);   // presenter replies with SetPriceType -> moves the mark
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            bool itemMode = SearchMode == SearchMode.ItemCodeAndDescription;
            if (e.KeyData == Keys.Down && itemMode && listViewItems.Items.Count > 0)
            {
                listViewItems.Focus();
                listViewItems.Items[0].Focused = true;
                listViewItems.Items[0].Selected = true;
            }
            else if (e.KeyData == Keys.Return && itemMode) Raise(SearchSubmitted);
        }

        private void listViewItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return && _itemSel != null) SearchResultChosen?.Invoke(this, _itemSel);
            if (e.KeyData == Keys.Up && listViewItems.Items.Count > 0 && listViewItems.FocusedItem?.Index == 0)
                txtSearch.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F12: Raise(TenderRequested); break;
                case Keys.Return:
                    if (SearchMode != SearchMode.ItemCodeAndDescription)
                    {
                        Raise(SearchSubmitted);
                    }
                    break;
                case Keys.Escape: ClearSearch(); break;
                case Keys.F2:
                case Keys.Delete: Raise(RemoveRequested); break;
                case Keys.F1: Raise(EditItemRequested); break;
                case Keys.F3: Raise(DiscountRequested); break;
                case Keys.F6: Raise(QuantityRequested); break;
                case Keys.F7: Raise(NewSaleRequested); break;
                case Keys.D1 | Keys.Control: Raise(DialogRequested, CashierDialog.CashCount); break;
                case Keys.D2 | Keys.Control: Raise(DialogRequested, CashierDialog.RecentSales); break;
                case Keys.D3 | Keys.Control: Raise(DialogRequested, CashierDialog.Reprint); break;
                case Keys.D4 | Keys.Control: Raise(DialogRequested, CashierDialog.ZReading); break;
                case Keys.D5 | Keys.Control: Raise(DialogRequested, CashierDialog.PickItem); break;
                case Keys.D6 | Keys.Control: Raise(DialogRequested, CashierDialog.CustomerDisplay); break;
                case Keys.D7 | Keys.Control: Raise(DialogRequested, CashierDialog.Stocks); break;
                case Keys.D8 | Keys.Control: Raise(ReturnRequested); break;
                case Keys.S | Keys.Control: Raise(ToggleSearchModeRequested); break;
                case Keys.F | Keys.Control: txtSearch.Focus(); break;
                case Keys.D | Keys.Control: Raise(TogglePriceTypeRequested); break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ===================================================== ICashierView: rendering

        public void BindCart(IList<CartLine> lines, int selectIndex)
        {
            dataGrid.Rows.Clear();
            foreach (var l in lines)
            {
                var row = dataGrid.Rows[dataGrid.Rows.Add()];
                row.Cells["ITEMCODE"].Value = l.ItemCode;
                row.Cells["DESCRIPTION"].Value = l.Description;
                row.Cells["PRICE"].Value = l.Price;
                row.Cells["QUANTITY"].Value = l.Quantity;
                row.Cells["TOTAL"].Value = l.Total;
                row.Cells["DISCOUNT"].Value = l.DiscountText;
                row.Cells["TOTALDUE"].Value = l.TotalDue;
                row.Cells["VATSTATUS"].Value = l.VatStatus;
                row.Cells["DISCOUNTAMT"].Value = l.DiscountAmt;
                row.Cells["COST"].Value = l.Cost;
                row.Cells["TYPEOFPRICE"].Value = l.PriceType;
                row.Cells["DISCOUNTTYPE"].Value = l.DiscountType;
                row.Cells["NOTE"].Value = l.Note;
                row.Cells["PWIDTH"].Value = l.Width;
                row.Cells["PHEIGHT"].Value = l.Height;
                row.Cells["CURRENTQTY"].Value = l.CurrentQty;
            }
            if (dataGrid.Rows.Count == 0) return;
            int i = selectIndex >= 0 && selectIndex < dataGrid.Rows.Count ? selectIndex : dataGrid.Rows.Count - 1;
            dataGrid.Rows[i].Selected = true;
            dataGrid.CurrentCell = dataGrid.Rows[i].Cells[0];
        }

        private static string Peso(decimal v) => "₱" + v.ToString("N");

        public void ShowTotals(CartTotals t)
        {
            lblSubTotal.Text = Peso(t.SubTotal);
            lblSubDiscount.Text = Peso(t.Discount);
            lblAmountDue.Text = Peso(t.AmountDue);
            lblVATSales.Text = Peso(t.VatSales);
            lblVATAmt.Text = Peso(t.VatAmount);
            lblNonVATSales.Text = Peso(t.NonVatSales);
            lblVATExemptSales.Text = Peso(t.VatExemptSales);
            lblZeroRatedSales.Text = Peso(t.ZeroRatedSales);
            lblNoOfItems.Text = t.NoOfItems.ToString("N");
        }

        public void ShowPaymentSummary(decimal cashTendered, decimal change, decimal redeemAmount)
        {
            lblCashTendered.Text = Peso(cashTendered);
            lblChange.Text = Peso(change);
            lblRedeemAmt.Text = Peso(redeemAmount);
        }

        public void ShowSearchResults(IList<SearchResultItem> items, bool visible)
        {
            listViewItems.Items.Clear();
            foreach (var r in items)
            {
                var li = listViewItems.Items.Add(r.ItemCode, r.Name, 0);
                li.SubItems.Add("₱ " + r.Price);
            }
            listViewItems.Visible = visible;
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void SetPriceType(string priceType)
        {
            Button b = priceType == "RETAIL" ? btnRetail : priceType == "PROMO" ? btnPromo : btnWholesale;
            panelMark.Visible = true;
            panelMark.Location = new Point(b.Location.X, b.Location.Y + b.Height - panelMark.Height);
            panelMark.Width = b.Width;
        }

        public void SetEntryEnabled(bool enabled)
        {
            btEditItem.Enabled = enabled;
            btCancel.Enabled = enabled;
            btDiscount.Enabled = enabled;
            btQuantity.Enabled = enabled;
            txtSearch.Enabled = enabled;
            iconButton1.Enabled = enabled;
        }

        public void FocusSearch() => txtSearch.Focus();

        public void ClearSearch()
        {
            txtSearch.Clear();
            txtSearch.Focus();
        }

        // ===================================================== ICashierView: messages & dialogs

        public void ShowMessage(string text, string caption = "") =>
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        public bool Confirm(string text) =>
            MessageBox.Show(text, "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public void ShowItemNotFound(string code) => new ItemNotFoundView(code).ShowDialog();

        // NOTE: these legacy dialogs must accept ICashierHost instead of frmCashier (see reply).
        public void OpenDialog(CashierDialog dialog, int receiptNo)
        {
            Form modalOwner = this.MdiParent ?? this;

            switch (dialog)
            {
                case CashierDialog.ZReading:
                    using (Form backdrop = new Form())
                    {
                        backdrop.FormBorderStyle = FormBorderStyle.None;
                        backdrop.StartPosition = FormStartPosition.Manual;
                        backdrop.ShowInTaskbar = false;
                        backdrop.BackColor = Color.Black;
                        backdrop.Opacity = 0.45;
                        backdrop.Bounds = modalOwner.Bounds;
                        backdrop.Show(modalOwner);
                        backdrop.BringToFront();

                        using (LoginView loginView = new LoginView(_userName, this, "Z-READING"))
                        {
                            loginView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(loginView, modalOwner);
                        }
                    }
                    break;
                
                //case CashierDialog.RecentSales: new frmCashierReport(_userName).ShowDialog(); break;


                case CashierDialog.Reprint:
                    using (Form backdrop = new Form())
                    {
                        backdrop.FormBorderStyle = FormBorderStyle.None;
                        backdrop.StartPosition = FormStartPosition.Manual;
                        backdrop.ShowInTaskbar = false;
                        backdrop.BackColor = Color.Black;
                        backdrop.Opacity = 0.45;
                        backdrop.Bounds = modalOwner.Bounds;
                        backdrop.Show(modalOwner);
                        backdrop.BringToFront();

                        using (PrintConfirmView loginView = new PrintConfirmView(receiptNo, "PURCHASED", 1))
                        {
                            loginView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(loginView, modalOwner);
                        }
                    }
                    break;
                case CashierDialog.CashCount:
                    
                    using (Form backdrop = new Form())
                    {
                        backdrop.FormBorderStyle = FormBorderStyle.None;
                        backdrop.StartPosition = FormStartPosition.Manual;
                        backdrop.ShowInTaskbar = false; 
                        backdrop.BackColor = Color.Black;
                        backdrop.Opacity = 0.45;
                        backdrop.Bounds = modalOwner.Bounds;
                        backdrop.Show(modalOwner);
                        backdrop.BringToFront();

                        using (CashierCashCountView loginView = new CashierCashCountView(_userName))
                        {
                            loginView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(loginView, modalOwner);
                        }
                    }
                    break;
                //case CashierDialog.Stocks: new frmInventory(_userName, "").ShowDialog(); break;
                //case CashierDialog.PickItem: new frmPickItem(this).ShowDialog(); break;
                //case CashierDialog.VoidSales: new frmVoidSales(this).ShowDialog(); break;
                case CashierDialog.CustomerDisplay:
                    _frmCustDisplay = new CustomerDisplayView();
                    _frmCustDisplay.Show();
                    break;
            }
        }

        public void ShowDiscountDialog(string itemCode, string priceType)
        {
            Form modalOwner = this.MdiParent ?? this;

            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = modalOwner.Bounds;
                backdrop.Show(modalOwner);
                backdrop.BringToFront();
                using (DiscountView discView = new DiscountView(this, itemCode, priceType))
                {
                    discView.StartPosition = FormStartPosition.CenterParent;
                    POSTheme.ShowSmoothFadeDialog(discView, modalOwner);
                }
            }
        }

        public void ShowQuantityDialog(CartLine l) => new AdjustQtyView(l.ItemCode, l.Quantity, l.Total, this, l.PriceType, l.Width, l.Height, l.Note).ShowDialog();

        public void ShowVoidItemDialog(CartLine l) { throw new NotImplementedException(); }
        //=> new frmVoidItem(l.ItemCode, l.Description, l.Quantity, l.PriceType, this).ShowDialog();

        public void ShowEditItemDialog(string itemCode, bool isAdmin)
        {
            if (isAdmin)
            {
                Form modalOwner = this.MdiParent ?? this;
                using (Form backdrop = new Form())
                {
                    backdrop.FormBorderStyle = FormBorderStyle.None;
                    backdrop.StartPosition = FormStartPosition.Manual;
                    backdrop.ShowInTaskbar = false;
                    backdrop.BackColor = Color.Black;
                    backdrop.Opacity = 0.45;
                    backdrop.Bounds = modalOwner.Bounds;
                    backdrop.Show(modalOwner);
                    backdrop.BringToFront();

                    using (AddOrEditItemView editItemView = new AddOrEditItemView())
                    {
                        new AddOrEditItemPresenter(
                            editItemView,
                            _userName,
                            itemCode,
                            null); // Optional
                        editItemView.StartPosition = FormStartPosition.CenterParent;
                        POSTheme.ShowSmoothFadeDialog(editItemView, modalOwner);
                    }
                }
            }
            else new LoginView(_userName, itemCode, this, "CASHIER EDIT ITEM").ShowDialog();
        }

        public void ShowPaymentDialog(decimal amountDue) { throw new NotImplementedException(); }
        //=> new frmPayment(this, amountDue).Show();

        public void ShowReceiptDialog(PaymentModel payment) { throw new NotImplementedException(); }
        //=> new frmMessage(this, payment).Show();

        // ===================================================== ICashierHost (what child dialogs call)

        public void ApplyDiscount(string id, string mode, string scope, decimal value, string priceType, string name) =>
            _presenter.ApplyDiscount(id, mode, scope, value, priceType, name);

        public void AdjustQty(string id, decimal qty, decimal price, decimal total, string priceType, string note, decimal w, decimal h) =>
            _presenter.AdjustQty(id, qty, price, total, priceType, note, w, h);

        public void ConfirmPayment(PaymentModel pm) => _presenter.ConfirmPayment(pm);
        public void RemoveCurrentItem(string itemCode, string priceType) => _presenter.RemoveCurrentItem();
        public void UpdateItem(inventory item) => _presenter.UpdateItem(item);
        public void ClearEntries() => _presenter.ClearEntries();
        public void UnlockVoid() => _presenter.UnlockVoid();
        public void F7Key() => _presenter.StartNewSale();
        public void AddItem(string id) => _presenter.AddItem(id);

        // ===================================================== ICustomerFacingDisplay

        public void ConfigurePole(bool enabled, string port, string defaultTop, string defaultBottom)
        {
            _poleEnabled = enabled;
            _poleTop = defaultTop;
            _poleBottom = defaultBottom;
            if (enabled) _sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            ResetPole();
        }

        public void LineAdded(CartLine l)
        {
            PoleTop(l.Description, l.Quantity);
            _frmCustDisplay.Refresh("ADD", l.ItemCode, l.Description, l.Price, l.Quantity, l.TotalDue, l.PriceType);
        }

        public void LineUpdated(CartLine l)
        {
            PoleTop(l.Description, l.Quantity);
            _frmCustDisplay.Refresh("UPDATE", l.ItemCode, "", l.Price, l.Quantity, l.TotalDue, l.PriceType);
        }

        public void LineRemoved(string itemCode, string priceType) => _frmCustDisplay.RemoveItem(itemCode, priceType);

        public void ShowTotals(CartTotals totals, bool writePole)
        {
            if (writePole) PoleBottom("Total: " + totals.AmountDue.ToString("N"));
            _frmCustDisplay.RefreshTotal(totals.SubTotal, totals.Discount, totals.AmountDue);
        }

        public void ShowPayment(decimal paid, decimal change) => _frmCustDisplay.RefreshPayment(paid, change);

        public void Reset()
        {
            _frmCustDisplay.ClearAll();
            ResetPole();
        }

        private static string Clip(string s) => s.Length > 18 ? s.Substring(0, 18) : s;

        private void ResetPole()
        {
            if (!_poleEnabled) return;
            _sp.Open();
            _sp.Write(new byte[] { 12 }, 0, 1);
            _sp.Write(_poleTop);
            _sp.Write(new byte[] { 10, 13 }, 0, 2);
            _sp.Write(_poleBottom);
            _sp.Close();
        }

        private void PoleTop(string text, decimal qty)
        {
            if (!_poleEnabled) return;
            _sp.Open();
            _sp.Write(new byte[] { 12 }, 0, 1);
            _sp.Write(Clip(qty == 1m ? text : qty.ToString("#.##") + "x " + text));
            _sp.Close();
        }

        private void PoleBottom(string text)
        {
            if (!_poleEnabled) return;
            _sp.Open();
            _sp.Write(new byte[] { 10, 13 }, 0, 2);
            _sp.Write(Clip(text));
            _sp.Close();
        }
    }

    
}
