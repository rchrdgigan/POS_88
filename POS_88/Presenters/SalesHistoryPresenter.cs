using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;
using System.Collections.Generic;


namespace POS_88.Presenters
{
    public class SalesHistoryPresenter
    {
        private readonly ISalesHistoryView view;
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly POSImplementation posImplementation;
        private readonly BindingSource salesHistoryBindingSource;
        private DataTable _pubDT;

        public SalesHistoryPresenter(ISalesHistoryView view, string customerID)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            if (string.IsNullOrWhiteSpace(customerID))
                throw new ArgumentException("An item code is required.", "customerID");

            this.view = view;
            this.view.CustomerID = customerID;
            this.posImplementation = new POSImplementation();
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.salesHistoryBindingSource = new BindingSource();

            this.view.SetSalesHistoryListBindingSource(this.salesHistoryBindingSource);
            this.view.SearchEvent += SearchHistory;
            this.view.ClearEvent += ClearFilter;
            if (LoadData())
                SearchHistory(this, EventArgs.Empty);
        }

        private void ClearFilter(object sender, EventArgs e)
        {
            this.GetSalesData($" WHERE invoice.customerid={view.CustomerID} AND invoicetype='SALES'");
            view.From = System.DateTime.Now;
            view.To = System.DateTime.Now;
        }

        private bool LoadData()
        {
            try
            {
                int _customerID;
                if (!int.TryParse(view.CustomerID, out _customerID))
                {
                    MessageBox.Show("The selected customer id not converting a string to int.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                try
                {
                    customer customer = this.unitOfWork.Customers.Find(x => x.customerid == _customerID).FirstOrDefault();
                    view.CustomerID = Convert.ToString(_customerID);
                    view.CustomerName = customer.customername;
                    view.CustomerContact = customer.contactno;
                    view.ModalTitle = $"Sales History ({customer.customername})";
                    this.GetSalesData($" WHERE invoice.customerid={view.CustomerID} AND invoicetype='SALES'");
                }
                finally
                {
                    unitOfWork.Dispose();
                }

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Sales Customer History", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void GetSalesData(string param)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = this.posImplementation.SearchSaleswithParam(param);
            salesHistoryBindingSource.DataSource = dataTable2;
            this._pubDT = dataTable2;
            List<SummarySales> summarySalesList = new List<SummarySales>();
            SummarySales summarySales = new SummarySales();
            Decimal num1 = 0M;
            Decimal num2 = 0M;
            if (dataTable2.Rows.Count > 0)
            {
                summarySales.SUBTOTAL = Convert.ToDecimal(dataTable2.Compute("SUM(SUBTOTAL)", string.Empty));
                summarySales.SUB_DISCOUNT = Convert.ToDecimal(dataTable2.Compute("SUM(SUB_DISCOUNT)", string.Empty));
                summarySales.AMOUNT_DUE = Convert.ToDecimal(dataTable2.Compute("SUM(AMOUNT_DUE)", string.Empty));
                summarySales.NO_OF_ITEMS = Convert.ToDecimal(dataTable2.Compute("SUM(NO_OF_ITEMS)", string.Empty));
                summarySales.TOTAL_COST = Convert.ToDecimal(dataTable2.Compute("SUM(COST)", string.Empty));
                num2 = Convert.ToDecimal(dataTable2.Compute("SUM(BALANCE)", string.Empty));
                num1 = Convert.ToDecimal(dataTable2.Compute("SUM(AMOUNT_PAID)", string.Empty));
                summarySalesList.Add(summarySales);
            }
            Decimal num3 = summarySales.AMOUNT_DUE - summarySales.TOTAL_COST;
            view.TotalRevenue = "Total Sales: ₱ " + summarySales.AMOUNT_DUE.ToString("N");
            view.AmountPaid = "Amount Paid: ₱ " + num1.ToString("N");
            view.Balance = "Balance: ₱ " + num2.ToString("N");
            view.GrossProfit = "Gross Profit: ₱ " + num3.ToString("N");
        }

        private void SearchHistory(object sender, EventArgs e)
        {
            DateTime from = view.From;
            DateTime to = view.To;
            if (from.Date > to.Date)
            {
                MessageBox.Show("The From date must not be later than the To date.", "Sales Customer History", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.GetSalesData($" WHERE invoice.customerid={view.CustomerID} AND convert(DATE,solddate) BETWEEN '{from.ToString("MM/dd/yyyy")}' AND '{to.ToString("MM/dd/yyyy")}' AND invoicetype='SALES'");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}