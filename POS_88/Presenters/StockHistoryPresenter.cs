using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class StockHistoryPresenter
    {
        private readonly IStockHistoryView view;
        private readonly POSImplementation posImplementation;
        private readonly BindingSource stockHistoryBindingSource;

        public StockHistoryPresenter(IStockHistoryView view, string itemCode)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            if (string.IsNullOrWhiteSpace(itemCode))
                throw new ArgumentException("An item code is required.", "itemCode");

            this.view = view;
            this.view.ItemCode = itemCode;
            this.posImplementation = new POSImplementation();
            this.stockHistoryBindingSource = new BindingSource();

            this.view.SetStockHistoryListBindingSource(this.stockHistoryBindingSource);
            this.view.SearchEvent += SearchHistory;
            this.view.ClearEvent += SearchHistory;

            if (LoadItemName())
                SearchHistory(this, EventArgs.Empty);
        }

        private bool LoadItemName()
        {
            try
            {
                UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
                try
                {
                    inventory item = unitOfWork.Inventory
                        .Find(value => value.itemcode == view.ItemCode)
                        .FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The selected item could not be found.", "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        view.ItemName = string.Empty;
                        return false;
                    }

                    view.ItemName = item.itemname;
                }
                finally
                {
                    unitOfWork.Dispose();
                }

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Error);
                view.ItemName = string.Empty;
                return false;
            }
        }

        private void SearchHistory(object sender, EventArgs e)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParse(view.From, out from) || !DateTime.TryParse(view.To, out to))
            {
                MessageBox.Show("Enter a valid stock-history date range.", "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (from.Date > to.Date)
            {
                MessageBox.Show("The From date must not be later than the To date.", "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable history = posImplementation.SearchStocksHistory(new SearchRequestStocksHistory
                {
                    from = from.Date,
                    to = to.Date,
                    itemcode = view.ItemCode
                });
                stockHistoryBindingSource.DataSource = history;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}