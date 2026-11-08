using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms;

using POS_88.Database;
using POS_88.Persistence;
using POS_88.Presenters;

namespace POS_88.Views
{
    public partial class CashierReportView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        private string _UserNm;
        private string _UserType;

        public CashierReportView()
        {
            InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
        }

        public CashierReportView(string usernm)
        {
            this.InitializeComponent();

            if (dataGridSummary != null)
            {
                dataGridSummary.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }

            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
            this._UserNm = usernm;

            this.btnSearch.Click += delegate { this.GetSalesData(""); };

            this.btnClearFilter.Click += delegate
            {
                this.dtFrom.Value = System.DateTime.Now;
                this.dtTo.Value = System.DateTime.Now;
                this.txtSearch.Clear();
                this.cboStatus.Text = "";
                this.GetSalesData(this.txtSearch.Text);
                this.txtSearch.Focus();
            };

            this.btnExportExcel.Click += delegate { this.ExporttoExcel(); };

            this.btnPurchaseitems.Click += delegate {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales transaction to open Purchase Items.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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

                    using (PurchaseItemView printConfirmView = new PurchaseItemView(Convert.ToInt32(receiptNo)))
                    {
                        try
                        {
                            printConfirmView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(printConfirmView, modalOwner);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error:"+ ex.Message, "Purchase Item",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }
            };

            this.btnReprint.Click += delegate {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales transaction to Re-Print Receipt.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                    using (PrintConfirmView printConfirmView = new PrintConfirmView(Convert.ToInt32(receiptNo), "PURCHASED", 1))
                    {
                        try
                        {
                            printConfirmView.StartPosition = FormStartPosition.CenterParent;
                            POSTheme.ShowSmoothFadeDialog(printConfirmView, modalOwner);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact the system administrator for assistance.", "Print Receipt",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }
            };

            this.btnPrint.Click += delegate
            {
                string userNm = this._UserNm;
                string type = cboTypeOfSales.Text;
                bool isTransaction = type == "Sales Transaction" || type == "Void Sales";

                var names = new ArrayList { "@From", "@To" };
                var values = new ArrayList { dtFrom.Value.Date, dtTo.Value.Date };

                if (isTransaction)
                {
                    names.AddRange(new[] { "@CahierID", "@InvoiceType", "@Status" });
                    values.AddRange(new object[]
                    {
                        userNm,
                        type == "Sales Transaction" ? "SALES" : "VOID",
                        cboStatus.Text
                    });
                }

                var reportView = new ReportViewerView(isTransaction ? "SalesbyTransaction.rpt" : "SalesbyItems.rpt", names, values);
                try
                {
                    new ReportViewerPresenter(reportView).LoadReport();
                    reportView.Show();
                }
                catch (Exception ex)
                {
                    reportView.Dispose();
                    MessageBox.Show(ex.Message, "Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            this.btnPayment.Click += delegate
            {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales transaction to view Payment Info.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                    using (PaymentView paymentView = new PaymentView(Convert.ToInt32(receiptNo)))
                    {
                        paymentView.StartPosition = FormStartPosition.CenterParent;
                        //paymentView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(paymentView, modalOwner);
                    }
                }
            };
        }

        public string SelectedReceiptNo
        {
            get
            {
                if (dataGrid == null || dataGrid.CurrentRow == null || dataGrid.CurrentRow.Cells.Count == 0)
                    return string.Empty;

                var value = dataGrid.CurrentRow.Cells[0].Value;
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }

        private void CashierReportView_Load(object sender, EventArgs e)
        {
            this.PolulateCombo();
            this.GetUserDetails();
            this.GetSalesData("");
        }

        private void GetUserDetails()
        {
            user user = this._unitOfWork.User.Find((Expression<System.Func<user, bool>>)(x => x.usernm == this._UserNm)).FirstOrDefault<user>();
            this.lblCashierNm.Text = user.nameuser;
            this._UserType = user.usertype;
        }

        public void GetSalesData(string param)
        {
            param = this.txtSearch.Text;
            DataTable dataTable1 = new DataTable();
            SearchRequestSales request = new SearchRequestSales();
            request.from = this.dtFrom.Value;
            request.to = this.dtTo.Value;
            request.crtdby = this._UserNm;
            request.paymentstatus = this.cboStatus.Text;
            request.SearchParam = this.txtSearch.Text;
            DataTable dataTable2;
            if (this.cboTypeOfSales.Text == "Sales Transaction")
            {
                request.invoicetype = "SALES";
                dataTable2 = this._posImplement.SearchSales(request);
            }
            else if (this.cboTypeOfSales.Text == "Void Sales")
            {
                request.invoicetype = "VOID";
                dataTable2 = this._posImplement.SearchSales(request);
            }
            else
            {
                request.invoicetype = "SALES";
                dataTable2 = this._posImplement.SearchItemSales(request);
            }
            this.dataGrid.DataSource = (object)dataTable2;
            if (this.cboTypeOfSales.Text == "Sales Transaction" || this.cboTypeOfSales.Text == "Void Sales")
            {
                List<SummarySales> summarySalesList = new List<SummarySales>();
                SummarySales summarySales = new SummarySales();
                Decimal num1 = 0M;
                Decimal num2 = 0M;
                if (dataTable2.Rows.Count > 0)
                {
                    summarySales.SUBTOTAL = Convert.ToDecimal(dataTable2.Compute("SUM(SUBTOTAL)", string.Empty));
                    summarySales.SUB_DISCOUNT = Convert.ToDecimal(dataTable2.Compute("SUM(SUB_DISCOUNT)", string.Empty));
                    summarySales.AMOUNT_DUE = Convert.ToDecimal(dataTable2.Compute("SUM(AMOUNT_DUE)", string.Empty));
                    summarySales.VAT_SALES = Convert.ToDecimal(dataTable2.Compute("SUM(VAT_SALES)", string.Empty));
                    summarySales.VAT_AMOUNT = Convert.ToDecimal(dataTable2.Compute("SUM(VAT_AMOUNT)", string.Empty));
                    summarySales.NON_VAT_SALES = Convert.ToDecimal(dataTable2.Compute("SUM(NON_VAT_SALES)", string.Empty));
                    summarySales.VAT_EXEMPT_SALES = Convert.ToDecimal(dataTable2.Compute("SUM(VAT_EXEMPT_SALES)", string.Empty));
                    summarySales.ZERO_RATED_SALES = Convert.ToDecimal(dataTable2.Compute("SUM(ZERO_RATED_SALES)", string.Empty));
                    summarySales.NO_OF_ITEMS = Convert.ToDecimal(dataTable2.Compute("SUM(NO_OF_ITEMS)", string.Empty));
                    summarySales.TOTAL_COST = Convert.ToDecimal(dataTable2.Compute("SUM(COST)", string.Empty));
                    num2 = Convert.ToDecimal(dataTable2.Compute("SUM(BALANCE)", string.Empty));
                    num1 = Convert.ToDecimal(dataTable2.Compute("SUM(AMOUNT_PAID)", string.Empty));
                    summarySalesList.Add(summarySales);
                    this.dataGridSummary.DataSource = (object)summarySalesList;
                }
                else
                {
                    summarySalesList.Add(summarySales);
                    this.dataGridSummary.DataSource = (object)summarySalesList;
                }
                this.lblBalance.Visible = true;
                this.lblAmountPaid.Visible = true;
                Decimal num3 = summarySales.AMOUNT_DUE - summarySales.TOTAL_COST;
                this.lblTotalRevenue.Text = "Total Sales: ₱ " + summarySales.AMOUNT_DUE.ToString("N");
                this.lblAmountPaid.Text = "Amount Paid: ₱ " + num1.ToString("N");
                this.lblBalance.Text = "Balance: ₱ " + num2.ToString("N");
                this.lblGrossProfit.Text = "Gross Profit: ₱ " + num3.ToString("N");
                if (this._UserType != "ADMIN")
                    this.dataGrid.Columns["COST"].Visible = false;
            }
            else
            {
                List<SummaryItemSales> summaryItemSalesList = new List<SummaryItemSales>();
                SummaryItemSales summaryItemSales = new SummaryItemSales();
                Decimal num4 = 0M;
                if (dataTable2.Rows.Count > 0)
                {
                    summaryItemSales.SUBTOTAL = Convert.ToDecimal(dataTable2.Compute("SUM(SUB_TOTAL)", string.Empty));
                    summaryItemSales.TOTAL_QTY = Convert.ToInt32(dataTable2.Compute("SUM(TOTAL_QTY)", string.Empty));
                    summaryItemSales.SUB_TOTAL = Convert.ToDecimal(dataTable2.Compute("SUM(SUB_TOTAL)", string.Empty));
                    summaryItemSales.DISCOUNT = Convert.ToDecimal(dataTable2.Compute("SUM(DISCOUNT)", string.Empty));
                    summaryItemSales.TOTAL_DUE = Convert.ToDecimal(dataTable2.Compute("SUM(TOTAL_DUE)", string.Empty));
                    num4 = Convert.ToDecimal(dataTable2.Compute("SUM(TOTAL_COST)", string.Empty));
                    summaryItemSalesList.Add(summaryItemSales);
                    this.dataGridSummary.DataSource = (object)summaryItemSalesList;
                }
                else
                {
                    summaryItemSalesList.Add(summaryItemSales);
                    this.dataGridSummary.DataSource = (object)summaryItemSalesList;
                }
                Decimal num5 = summaryItemSales.TOTAL_DUE - num4;
                this.lblBalance.Visible = false;
                this.lblAmountPaid.Text = "Cost: ₱ " + num4.ToString("N");
                this.lblTotalRevenue.Text = "Total Sales: ₱ " + summaryItemSales.TOTAL_DUE.ToString("N");
                this.lblGrossProfit.Text = "Gross Profit: ₱ " + num5.ToString("N");
                if (this._UserType != "ADMIN")
                    this.dataGrid.Columns["TOTAL_COST"].Visible = false;
            }
            if (this._UserType == "ADMIN")
            {
                this.lblAmountPaid.Visible = true;
                this.lblGrossProfit.Visible = true;
            }
            else
            {
                if (this.cboTypeOfSales.Text == "Sales Transaction")
                    this.dataGridSummary.Columns["TOTAL_COST"].Visible = false;
                this.lblGrossProfit.Visible = false;
                this.lblAmountPaid.Visible = false;
            }
        }

        private void PolulateCombo()
        {
            this.cboTypeOfSales.Items.Clear();
            this.cboTypeOfSales.Items.Add((object)"Sales Transaction");
            this.cboTypeOfSales.Items.Add((object)"Sales by Items");
            this.cboTypeOfSales.Items.Add((object)"Void Sales");
            this.cboTypeOfSales.Text = "Sales Transaction";
            this.cboStatus.Items.Clear();
            this.cboStatus.Items.Add((object)"");
            this.cboStatus.Items.Add((object)"PAID");
            this.cboStatus.Items.Add((object)"UNPAID");
            this.dtFrom.Value = System.DateTime.Now;
            this.dtTo.Value = System.DateTime.Now;
            this.cboStatus.Text = "";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return)
                this.GetSalesData("");
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void cboTypeOfSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboTypeOfSales.Text == "Sales Transaction")
            {
                this.lblSearch.Text = "Search Receipt No/Customer";
                this.lblstatus.Enabled = true;
                this.cboStatus.Enabled = true;
                this.btnPurchaseitems.Enabled = true;
            }
            else if (this.cboTypeOfSales.Text == "Void Sales")
            {
                this.lblSearch.Text = "Search Receipt No/Customer";
                this.lblstatus.Enabled = false;
                this.cboStatus.Enabled = false;
                this.btnPurchaseitems.Enabled = true;
            }
            else
            {
                this.lblSearch.Text = "Search Itemcode/Name/Category";
                this.lblstatus.Enabled = false;
                this.cboStatus.Enabled = false;
                this.btnPurchaseitems.Enabled = false;
            }
            this.dtFrom.Value = System.DateTime.Now;
            this.dtTo.Value = System.DateTime.Now;
            this.txtSearch.Clear();
            this.cboStatus.Text = "";
            this.GetSalesData(this.txtSearch.Text);
            this.txtSearch.Focus();
        }

        private void ExporttoExcel()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Save to Excel";
            int num1 = (int)saveFileDialog.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            string text = this.txtSearch.Text;
            DataTable dataTable1 = new DataTable();
            SearchRequestSales request = new SearchRequestSales()
            {
                from = this.dtFrom.Value,
                to = this.dtTo.Value,
                SearchParam = this.txtSearch.Text,
                crtdby = this._UserNm,
                paymentstatus = this.cboStatus.Text
            };
            request.SearchParam = this.txtSearch.Text;
            DataTable dataTable2;
            if (this.cboTypeOfSales.Text == "Sales Transaction")
            {
                request.invoicetype = "SALES";
                dataTable2 = this._posImplement.SearchSales(request);
            }
            else if (this.cboTypeOfSales.Text == "Void Sales")
            {
                request.invoicetype = "VOID";
                dataTable2 = this._posImplement.SearchSales(request);
            }
            else
            {
                request.invoicetype = "SALES";
                dataTable2 = this._posImplement.SearchItemSales(request);
            }
            string fileName = saveFileDialog.FileName;
            if (fileName != "")
            {
                using (XLWorkbook xlWorkbook = new XLWorkbook())
                {
                    xlWorkbook.Worksheets.Add(dataTable2, "SalesReport");
                    xlWorkbook.SaveAs(fileName);
                }
                int num2 = (int)MessageBox.Show("Report Successfully Exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Process.Start(fileName);
            }
            Cursor.Current = Cursors.Default;
        }

    }
}
