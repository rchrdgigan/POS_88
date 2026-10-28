using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Business;
using System.Collections;

namespace POS_88.Presenters
{
    class AddOrEditPurchaseOrderPresenter
    {
        private readonly IAddOrEditPurchaseOrderView view;
        private readonly POSImplementation posImplementation;
        private readonly string userName;
        private readonly string orderID;
        private readonly string saveType;
        private readonly Action afterSave;

        private const string itemCodeDesc = "ITEM CODE & DESCRIPTION";
        private const string itemBarcode = "BARCODE";
        private const string stIN = "STOCKS TRANSFER IN";
        private const string stOut = "STOCKS TRANSFER OUT";
        private const string Title = "Purchase Order";

        public AddOrEditPurchaseOrderPresenter(
            IAddOrEditPurchaseOrderView view,
            string userName,
            string orderID,
            Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.orderID = orderID;

            this.saveType = string.IsNullOrWhiteSpace(orderID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();

            this.view.SaveEvent += SaveItem;
            this.view.SearchSupplierEvent += SearchSupplier;
            this.view.SearchItemsEvent += SearchItems;
            this.view.AddItemEvent += AddItem;
            this.view.PrintEvent += PrintOrders;
            this.view.ReceivedEvent += MarkReceived;

            PopulateLookups();
            view.SearchType = itemCodeDesc;
            view.SetEditMode(saveType != "INSERT");

            if (saveType != "INSERT")
                LoadOrder();
        }

        private void MarkReceived(object sender, EventArgs e)
        {
            int currentOrderId = Int32.Parse(orderID);
            if (currentOrderId == 0)
            {
                MessageBox.Show("Please save the purchase order first.", Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to mark order as received?", "Verify",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            bool updateStocks = MessageBox.Show("Do you want to add/deduct stocks to inventory?", "Verify",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                Order order = unitOfWork.Order.Find(x => x.OrderID == currentOrderId).FirstOrDefault();
                if (order == null)
                {
                    MessageBox.Show("The purchase order could not be found.", Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (order.isProcessed == "YES")
                {
                    MessageBox.Show("This order has already been received.", Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    view.IsReceived();
                    return;
                }

                order.OrderReceived = DateTime.Now;
                order.isProcessed = "YES";

                string error = posImplementation.SaveOrder(order, "UPDATE");
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (updateStocks)
                {
                    var details = unitOfWork.OrderDetail.Find(x => x.OrderId == currentOrderId).ToList();
                    foreach (OrderDetail detail in details)
                    {
                        decimal qty = Convert.ToDecimal(detail.Qty);

                        if (order.OrderType == stIN)
                        {
                            unitOfWork.Inventory.UpdateStocks("ADD", detail.ItemCode, qty);
                            unitOfWork.StockHistory.LogStocksMovement("ADD", "Stocks Transfer In",
                                detail.OrderId, 0, detail.ItemCode, qty, userName);
                        }
                        else
                        {
                            unitOfWork.Inventory.UpdateQty(detail.ItemCode, qty);
                            unitOfWork.StockHistory.LogStocksMovement("DEDUCT", "Stocks Transfer Out",
                                detail.OrderId, 0, detail.ItemCode, qty, userName);
                        }
                    }
                    unitOfWork.Complete();   // remove if your repository methods already commit themselves
                }

                view.IsReceived();           // disables Save/Received buttons
                afterSave?.Invoke();         // refresh the order list

                MessageBox.Show("Order has been marked as received.", Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ShowError(ex); }
            finally { unitOfWork.Dispose(); }
        }

        private void PrintOrders(object sender, EventArgs e)
        {
            ArrayList paramArrayList = new ArrayList();
            ArrayList paramValueArrayList = new ArrayList();
            paramArrayList.Add((object)"@OrderID");
            paramValueArrayList.Add((object)this.orderID);
            ReportViewerView reportView = new ReportViewerView(
                "Orders.rpt", paramArrayList, paramValueArrayList);
            try
            {
                new ReportViewerPresenter(reportView).LoadReport();
                reportView.Show();
            }
            catch (Exception exception)
            {
                reportView.Dispose();
                MessageBox.Show(exception.Message, "Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void PopulateLookups()
        {
            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                var suppliers = unitOfWork.Supplier.GetAll().OrderBy(s => s.suppliername).ToList();
                view.SetSupplierList(new BindingSource { DataSource = suppliers });
                view.SetMovementTypeList(new List<string> { "", stIN, stOut });
                view.SetSearchByList(new List<string> { itemCodeDesc, itemBarcode });
            }
            catch (Exception ex) { ShowError(ex); }
            finally { unitOfWork.Dispose(); }
        }

        private void SearchSupplier(object sender, EventArgs e)
        {
            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                string suppId = view.SuppId;
                supplier supplier = unitOfWork.Supplier.Find(x => x.supplierid == suppId).FirstOrDefault();
                if (supplier != null)
                {
                    view.Address = supplier.address;
                    view.ContactNumber = supplier.contactno;
                }
            }
            catch (Exception ex) { ShowError(ex); }
            finally { unitOfWork.Dispose(); }
        }

        private void SearchItems(object sender, EventArgs e)
        {
            try
            {
                string text = view.SearchItems;
                if (string.IsNullOrEmpty(text) || view.SearchType == itemBarcode)
                {
                    view.SetSearchResults(new List<ItemLookup>(), false);
                    return;
                }

                var results = posImplementation.SearchItem("ITEM CODE AND NAME", text)
                    .Select(r => new ItemLookup { Code = r.ITEM_CODE, Name = r.NAME })
                    .ToList();

                view.SetSearchResults(results, true);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void AddItem(object sender, ItemEventArgs e)
        {
            string code = (e.ItemCode ?? string.Empty).Trim();
            if (code == string.Empty) return;

            // already in the grid -> just bump the quantity
            if (view.TryIncrementItem(code, 1M)) return;

            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                inventory item = unitOfWork.Inventory.Find(x => x.itemcode == code).FirstOrDefault();
                if (item == null)
                {
                    MessageBox.Show("Item \"" + code + "\" was not found.", Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal cost = item.cost ?? 0M;
                view.AddOrderDetail(item.itemcode, item.itemname, item.quantity, 1M, cost, cost);
                view.SelectLastOrderDetail();
            }
            catch (Exception ex) { ShowError(ex); }
            finally { unitOfWork.Dispose(); }
        }

        private void LoadOrder()
        {
            if (string.IsNullOrWhiteSpace(orderID)) return;

            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                Order order = null;
                int parsedId;
                if (int.TryParse(orderID, out parsedId))
                    order = unitOfWork.Order.Find(o => o.OrderID == parsedId).FirstOrDefault();

                if (order == null)
                    order = unitOfWork.Order.Find(o => o.OrderNo == orderID).FirstOrDefault();

                if (order == null)
                {
                    MessageBox.Show("The selected purchase order could not be found.", Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.Close();
                    return;
                }

                view.Supplier = order.SupplierId;
                view.OrderNo = order.OrderNo;
                view.OrderDate = order.OrderDate ?? DateTime.Today;
                view.MovementType = order.OrderType;
                view.Remarks = order.Remarks;

                if (order.OrderType == stOut)
                    view.StatusReceived("Transfer Stocks");
                if (order.isProcessed == "YES")
                    view.IsReceived();

                int realOrderId = order.OrderID;   // use the loaded order's ID, not the parsed text
                var details = unitOfWork.OrderDetail.Find(x => x.OrderId == realOrderId).ToList();
                foreach (OrderDetail detail in details)
                {
                    string code = detail.ItemCode;
                    inventory inv = unitOfWork.Inventory.Find(x => x.itemcode == code).FirstOrDefault();
                    view.AddOrderDetail(
                        detail.ItemCode,
                        inv != null ? inv.itemname : "(item not found)",
                        inv != null ? inv.quantity : (decimal?)null,
                        detail.Qty, detail.price, detail.total);
                }
                view.SelectLastOrderDetail();
            }
            catch (Exception ex)
            {
                ShowError(ex);
                view.Close();
            }
            finally { unitOfWork.Dispose(); }
        }

        private void SaveItem(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.Supplier))
            {
                MessageBox.Show("Please select a supplier.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(view.OrderNo))
            {
                MessageBox.Show("Please enter an order number.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(view.MovementType))
            {
                MessageBox.Show("Please select a movement type.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lines = view.GetOrderLines();
            if (lines.Count == 0)
            {
                MessageBox.Show("Please add at least one item.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                Order order = null;
                int parsedId;
                if (saveType == "UPDATE" && int.TryParse(orderID, out parsedId))
                    order = unitOfWork.Order.Find(o => o.OrderID == parsedId).FirstOrDefault();

                if (order == null)
                    order = new Order();

                string orderNo = view.OrderNo.Trim();

                order.SupplierId = view.Supplier.Trim();
                order.OrderNo = orderNo;
                order.OrderDate = view.OrderDate;
                order.OrderType = view.MovementType.Trim();
                order.Remarks = view.Remarks;

                if (saveType == "INSERT")
                {
                    order.createdBy = userName;
                    order.createdDt = DateTime.Now;
                    order.isProcessed = "NO";
                }

                var error = posImplementation.SaveOrder(order, saveType);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ---- save the order lines ----
                var saved = unitOfWork.Order.Find(o => o.OrderNo == orderNo).FirstOrDefault();
                if (saved == null)
                {
                    MessageBox.Show("The order was saved but could not be reloaded to save its items.",
                        Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int savedId = saved.OrderID;

                foreach (OrderDetail old in unitOfWork.OrderDetail.Find(x => x.OrderId == savedId).ToList())
                    unitOfWork.OrderDetail.Remove(old);

                foreach (OrderLine line in lines)
                {
                    unitOfWork.OrderDetail.Add(new OrderDetail
                    {
                        OrderId = savedId,
                        ItemCode = line.ItemCode,
                        Qty = line.Qty,
                        price = line.Price,
                        total = line.Total
                    });
                }
                unitOfWork.Complete();

                MessageBox.Show("Purchase order has been saved successfully.", Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                afterSave?.Invoke();

                if (saveType == "INSERT")
                    view.ClearForm();
                else
                    view.Close();
            }
            catch (Exception ex) { ShowError(ex); }
            finally { unitOfWork.Dispose(); }
        }
    }
}