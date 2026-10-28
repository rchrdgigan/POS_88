using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class SalesPresenter
    {
        private readonly ISalesView view;
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly POSImplementation posImplementation;
        private readonly string authenticatedUserName;

        public SalesPresenter(ISalesView view)
            : this(view, string.Empty)
        {
        }

        public SalesPresenter(ISalesView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;
            unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            posImplementation = new POSImplementation();

            view.SearchRequested += SearchSales;
            view.ReloadRequested += ReloadSales;
            view.VoidSalesRequested += VoidSale;
            view.CompleteTransactionRequested += CompletePendingSale;
            view.PaymentRequested += ShowPaymentDialog;
            view.ReprintRequested += ShowReprintNotAvailable;

            view.SetSalesTypeList(new[]
            {
                "Sales Transaction",
                "Sales by Items",
                "Payment History",
                "Void Sales",
                "Pending Transaction"
            });
            view.SetPaymentStatusList(new[] { string.Empty, "PAID", "UNPAID" });
            view.SetPaymentTypeList(new[]
            {
                "ALL", "CASH", "CREDIT CARD", "DEBIT CARD", "GCASH",
                "CL", "BANK TRANSFER", "UNIONBANK", "BDO", "CREDIT"
            });

            view.Show();
            LoadSales();
        }

        public void Reload()
        {
            ReloadSales(this, EventArgs.Empty);
        }

        private async void LoadSales()
        {
            view.ShowFilterLoading();
            try
            {
                var staff = await Task.Run(() =>
                    unitOfWork.User.GetAll().OrderBy(user => user.nameuser).ToList());

                view.SetStaffList(new BindingSource { DataSource = staff });
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
            finally
            {
                view.HideFilterLoading();
            }

            await GetSalesData();
        }

        private async void SearchSales(object sender, EventArgs e)
        {
            await GetSalesData();
        }

        private async void ReloadSales(object sender, EventArgs e)
        {
            await GetSalesData();
        }

        private async Task GetSalesData()
        {
            view.ShowSalesLoading();
            try
            {
                SearchRequestSales request = new SearchRequestSales
                {
                    from = view.DateFrom,
                    to = view.DateTo,
                    crtdby = view.StaffId,
                    paymentstatus = view.PaymentStatus,
                    paymenttype = view.PaymentType,
                    SearchParam = view.SearchText,
                    invoicetype = GetInvoiceType(view.TypeOfSales)
                };
                string salesType = view.TypeOfSales;

                DataTable sales = await Task.Run(() => GetSales(request, salesType));
                view.SalesGridData = sales;
                UpdateSummary(sales, salesType);
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
            finally
            {
                view.HideSalesLoading();
            }
        }

        private DataTable GetSales(SearchRequestSales request, string salesType)
        {
            if (salesType == "Payment History")
                return posImplementation.SearchPaymentHistory(request);
            if (salesType == "Sales by Items")
                return posImplementation.SearchItemSales(request);

            return posImplementation.SearchSales(request);
        }

        private static string GetInvoiceType(string salesType)
        {
            switch (salesType)
            {
                case "Sales Transaction":
                case "Payment History":
                case "Sales by Items":
                    return "SALES";
                case "Pending Transaction":
                    return "PENDING";
                case "Void Sales":
                    return "VOID";
                default:
                    throw new InvalidOperationException("Select a valid sales type before searching.");
            }
        }

        private void UpdateSummary(DataTable sales, string salesType)
        {
            if (salesType == "Payment History")
            {
                decimal totalPayment = Sum(sales, "AMOUNT_PAID");
                view.AmountPaid = "Total Payment: ₱ " + totalPayment.ToString("N");
                view.SetSummaryVisibility(false, false, true, false, false);
                return;
            }

            if (salesType == "Sales by Items")
            {
                decimal subtotal = Sum(sales, "SUB_TOTAL");
                decimal discount = Sum(sales, "DISCOUNT");
                decimal totalSales = Sum(sales, "TOTAL_DUE");
                decimal totalCost = Sum(sales, "TOTAL_COST");
                decimal grossProfit = totalSales - totalCost;

                view.TotalRevenue = "Total Sales: ₱ " + totalSales.ToString("N");
                view.AmountPaid = "Cost: ₱ " + totalCost.ToString("N");
                view.GrossProfit = "Gross Profit: ₱ " + grossProfit.ToString("N");
                view.SetSummaryVisibility(true, true, true, false, true);

                DataTable itemSummary = CreateSummaryTable(salesType);
                itemSummary.Rows.Add(
                    subtotal,
                    Sum(sales, "TOTAL_QTY"),
                    subtotal,
                    discount,
                    totalSales,
                    totalCost);
                view.SalesGridSummaryData = itemSummary;
                return;
            }

            decimal transactionSubtotal = Sum(sales, "SUBTOTAL");
            decimal transactionDiscount = Sum(sales, "SUB_DISCOUNT");
            decimal transactionSales = Sum(sales, "AMOUNT_DUE");
            decimal transactionPaid = Sum(sales, "AMOUNT_PAID");
            decimal transactionBalance = Sum(sales, "BALANCE");
            decimal transactionCost = Sum(sales, "COST");

            view.TotalRevenue = "Total Sales: ₱ " + transactionSales.ToString("N");
            view.AmountPaid = "Amount Paid: ₱ " + transactionPaid.ToString("N");
            view.Balance = "Balance: ₱ " + transactionBalance.ToString("N");
            view.GrossProfit = "Gross Profit: ₱ " + (transactionSales - transactionCost).ToString("N");
            view.SetSummaryVisibility(true, true, true, true, true);

            DataTable summary = CreateSummaryTable(salesType);
            summary.Rows.Add(
                transactionSubtotal,
                transactionDiscount,
                transactionSales,
                Sum(sales, "VAT_SALES"),
                Sum(sales, "VAT_AMOUNT"),
                Sum(sales, "NON_VAT_SALES"),
                Sum(sales, "VAT_EXEMPT_SALES"),
                Sum(sales, "ZERO_RATED_SALES"),
                Sum(sales, "NO_OF_ITEMS"),
                transactionCost,
                transactionPaid,
                transactionBalance);
            view.SalesGridSummaryData = summary;
        }

        private static DataTable CreateSummaryTable(string salesType)
        {
            DataTable summary = new DataTable();
            if (salesType == "Sales by Items")
            {
                summary.Columns.Add("SUBTOTAL", typeof(decimal));
                summary.Columns.Add("TOTAL_QTY", typeof(decimal));
                summary.Columns.Add("SUB_TOTAL", typeof(decimal));
                summary.Columns.Add("DISCOUNT", typeof(decimal));
                summary.Columns.Add("TOTAL_DUE", typeof(decimal));
                summary.Columns.Add("TOTAL_COST", typeof(decimal));
                return summary;
            }

            summary.Columns.Add("SUBTOTAL", typeof(decimal));
            summary.Columns.Add("SUB_DISCOUNT", typeof(decimal));
            summary.Columns.Add("AMOUNT_DUE", typeof(decimal));
            summary.Columns.Add("VAT_SALES", typeof(decimal));
            summary.Columns.Add("VAT_AMOUNT", typeof(decimal));
            summary.Columns.Add("NON_VAT_SALES", typeof(decimal));
            summary.Columns.Add("VAT_EXEMPT_SALES", typeof(decimal));
            summary.Columns.Add("ZERO_RATED_SALES", typeof(decimal));
            summary.Columns.Add("NO_OF_ITEMS", typeof(decimal));
            summary.Columns.Add("TOTAL_COST", typeof(decimal));
            summary.Columns.Add("AMOUNT_PAID", typeof(decimal));
            summary.Columns.Add("BALANCE", typeof(decimal));
            return summary;
        }

        private static decimal Sum(DataTable table, string columnName)
        {
            if (table == null || table.Rows.Count == 0 || !table.Columns.Contains(columnName))
                return 0M;

            object value = table.Compute("SUM([" + columnName + "])", string.Empty);
            return value == null || value == DBNull.Value ? 0M : Convert.ToDecimal(value);
        }

        private void VoidSale(object sender, EventArgs e)
        {
            UpdateInvoiceType("VOID", "Void selected sale?", "Sale voided successfully.");
        }

        private void CompletePendingSale(object sender, EventArgs e)
        {
            UpdateInvoiceType("SALES", "Mark selected pending transaction as completed?",
                "Pending transaction completed successfully.");
        }

        private void UpdateInvoiceType(string invoiceType, string confirmation, string successMessage)
        {
            if (invoiceType == "SALES" && view.TypeOfSales != "Pending Transaction")
            {
                MessageBox.Show("Select a pending transaction to complete.", "Sales",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (invoiceType == "VOID" &&
                view.TypeOfSales != "Sales Transaction" &&
                view.TypeOfSales != "Pending Transaction")
            {
                MessageBox.Show("Select a sales or pending transaction to void.", "Sales",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int receiptNumber;
            if (!TryGetSelectedReceipt(out receiptNumber))
                return;

            if (MessageBox.Show(
                confirmation + Environment.NewLine + "Receipt No: " + receiptNumber,
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                unitOfWork.Invoice.UpdateInvoiceType(receiptNumber, invoiceType, authenticatedUserName);
                MessageBox.Show(successMessage, "Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reload();
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }

        private bool TryGetSelectedReceipt(out int receiptNumber)
        {
            receiptNumber = 0;
            DataGridView grid = view.SalesGrid;
            if (grid.CurrentRow == null || grid.CurrentRow.Cells.Count == 0 ||
                grid.CurrentRow.Cells[0].Value == null ||
                !int.TryParse(Convert.ToString(grid.CurrentRow.Cells[0].Value), out receiptNumber))
            {
                MessageBox.Show("Select a valid sales transaction first.", "Sales",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ShowPaymentDialog(object sender, EventArgs e)
        {
            int receiptNumber;
            if (!TryGetSelectedReceipt(out receiptNumber))
                return;

            using (var paymentView = new PaymentView(receiptNumber))
            {
                paymentView.ShowDialog();
            }
        }

        private void ShowReprintNotAvailable(object sender, EventArgs e)
        {
            int receiptNumber;
            if (!TryGetSelectedReceipt(out receiptNumber))
                return;
            MessageBox.Show("Receipt reprinting is not implemented in this sales view yet." + receiptNumber, "Sales",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message, "Sales", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
