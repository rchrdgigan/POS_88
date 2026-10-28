using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using ClosedXML.Excel;
using System.Diagnostics;

using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Presenters
{
    public class PurchaseOrderPresenter
    {
        private readonly IPurchaseOrderView view;
        private readonly string authenticatedUserName;

        private POSImplementation _posImplement;
        private BindingSource purchaseOrderBindingSource;

        public PurchaseOrderPresenter(IPurchaseOrderView view)
            : this(view, string.Empty)
        {
        }

        public PurchaseOrderPresenter(IPurchaseOrderView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;

            this._posImplement = new POSImplementation();
            this.purchaseOrderBindingSource = new BindingSource();

            this.view.SearchEvent += SearchPurchaseOrder;
            this.view.ExportExcelEvent += this.ExportExcel;
            this.view.ClearFiltersEvent += this.ClearFilters;

            view.AuthenticatedUserName = authenticatedUserName;
            view.SetPurchaseOrderListBindingSource(this.purchaseOrderBindingSource);
            view.Show();
            this.LoadPurchaseOrder();
        }

        private void ExportExcel(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Save to Excel";
            int num1 = (int)saveFileDialog.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            string text = view.SearchParam;
            DataTable dataTable1 = new DataTable();
            SearchRequestOrders request = new SearchRequestOrders()
            {
                from = this.view.DateOrderFrom,
                to = this.view.DateOrderTo,
                SearchParam = this.view.SearchParam
            };
            request.SearchParam = this.view.SearchParam;
            DataTable dataTable2 = this._posImplement.SearchOrders(request);
            string fileName = saveFileDialog.FileName;
            if (fileName != "")
            {
                using (XLWorkbook xlWorkbook = new XLWorkbook())
                {
                    xlWorkbook.Worksheets.Add(dataTable2, "OrdersReport");
                    xlWorkbook.SaveAs(fileName);
                }
                int num2 = (int)MessageBox.Show("Report Successfully Exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Process.Start(fileName);
            }
            Cursor.Current = Cursors.Default;
        }

        private async void SearchPurchaseOrder(object sender, EventArgs e)
        {
            await this.GetPurchaseOrderData(this.view.SearchParam);
        }

        private async void LoadPurchaseOrder()
        {
            this.view.SetIsReceivedList(new List<string> { "", "YES", "NO" });
            await this.GetPurchaseOrderData(this.view.SearchParam);
        }

        public void Reload()
        {
            this.LoadPurchaseOrder();
        }

        public async Task GetPurchaseOrderData(string param)
        {
            this.view.ShowPurchaseOrderLoading();
            try
            {
                string searchParam = string.IsNullOrWhiteSpace(param) ? this.view.SearchParam : param;
                SearchRequestOrders request = new SearchRequestOrders
                {
                    isProcessed = this.view.IsReceived,
                    from = this.view.DateOrderFrom,
                    to = this.view.DateOrderTo,
                    SearchParam = searchParam
                };
                DataTable purchaseOrder = await Task.Run(() => this._posImplement.SearchOrders(request));

                this.purchaseOrderBindingSource.DataSource = purchaseOrder;
                int totalPurchaseOrders = purchaseOrder.Rows.Count;

                decimal totalQty = GetTotal(purchaseOrder, "TOTAL_QTY");
                decimal grandTotal = GetTotal(purchaseOrder, "GRAND_TOTAL");

                view.GrandTotal = "Grand Total: ₱ " + grandTotal.ToString("N");
                view.TotalOrders = "Total Orders: " + totalPurchaseOrders.ToString();
                view.TotalQty = "Total Qty: " + totalQty.ToString();
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HidePurchaseOrderLoading();
            }

            
        }

        private decimal GetTotal(DataTable purchaseOrder, string columnName)
        {
            object total = purchaseOrder.Compute("SUM(" + columnName + ")", string.Empty);
            return total == DBNull.Value ? 0M : Convert.ToDecimal(total);
        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetPurchaseOrderData(string.Empty);
        }
       
    }
}
