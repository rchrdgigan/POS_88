using System;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class AddStocksPresenter
    {
        private readonly IAddStocksView view;
        private readonly string itemCode;
        private readonly string userName;
        private readonly Action afterSave;

        public AddStocksPresenter(IAddStocksView view, string itemCode, string userName, Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            if (string.IsNullOrWhiteSpace(itemCode))
                throw new ArgumentException("An item code is required.", "itemCode");

            this.view = view;
            this.itemCode = itemCode;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.afterSave = afterSave;

            this.view.SaveEvent += AddStocks;
            this.view.ClearEvent += ClearFields;

            LoadItem();
        }

        private void LoadItem()
        {
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                inventory item = unitOfWork.Inventory
                    .Find(value => value.itemcode == itemCode).FirstOrDefault();
                if (item == null)
                {
                    MessageBox.Show("The selected item could not be found.", "Add Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.Close();
                    return;
                }

                view.ItemCode = item.itemcode;
                view.ItemName = item.itemname;
                view.CurrentQty = item.quantity.GetValueOrDefault().ToString("N");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Add Stocks", MessageBoxButtons.OK, MessageBoxIcon.Error);
                view.Close();
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void AddStocks(object sender, EventArgs e)
        {
            decimal quantity = view.Quantity;
            if (quantity <= 0M)
            {
                MessageBox.Show("Please enter a quantity greater than zero.", "Add Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
                try
                {
                    unitOfWork.Inventory.UpdateStocks("ADD", itemCode, quantity);
                    unitOfWork.StockHistory.LogStocksMovement(
                        "ADD", "Add Stocks", 0, 0, itemCode, quantity, userName);
                }
                finally
                {
                    unitOfWork.Dispose();
                }

                MessageBox.Show("Stocks have been added successfully.", "Add Stocks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                afterSave?.Invoke();
                view.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Add Stocks", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields(object sender, EventArgs e)
        {
            view.ClearFields();
        }
    }
}