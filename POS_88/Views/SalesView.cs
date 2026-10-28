using POS_88.Presenters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace POS_88.Views
{
    public partial class SalesView : Form, ISalesView
    {
        private bool suppressSalesTypeChanged;
        private readonly SkeletonLoader typeSalesSkeletonLoader;
        private readonly SkeletonLoader staffSkeletonLoader;
        private readonly SkeletonLoader statusSkeletonLoader;
        private readonly SkeletonLoader paymentTypeSkeletonLoader;
        private readonly SkeletonLoader dateFromSkeletonLoader;
        private readonly SkeletonLoader dateToSkeletonLoader;
        private readonly SkeletonLoader searchSkeletonLoader;
        private readonly SkeletonLoader salesGridSkeletonLoader;

        public SalesView()
        {
            InitializeComponent();

            typeSalesSkeletonLoader = CreateSkeletonLoader(cbBoxTypeSales);
            staffSkeletonLoader = CreateSkeletonLoader(cbBoxStaff);
            statusSkeletonLoader = CreateSkeletonLoader(cBoxStatus);
            paymentTypeSkeletonLoader = CreateSkeletonLoader(comboBox2);
            dateFromSkeletonLoader = CreateSkeletonLoader(dtpFrom);
            dateToSkeletonLoader = CreateSkeletonLoader(dtpTo);
            searchSkeletonLoader = CreateSkeletonLoader(txtSearchParam);
            salesGridSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);

            cbBoxTypeSales.SelectedIndexChanged += SalesTypeSelectedIndexChanged;
            btnSearch.Click += (sender, args) => OnSearchClick();
            btnClearFilter.Click += (sender, args) => ClearFiltersAndReload();
            //btnPayment.Click += (sender, args) => OnPaymentClick();
            btnVoid.Click += (sender, args) => OnVoidSalesClick();
            btnCompleted.Click += (sender, args) => OnCompleteTransactionClick();
            //btnReprint.Click += (sender, args) => OnReprintClick();
            btnPrint.Click += (sender, args) =>
            {
                string type = cbBoxTypeSales.Text;
                bool isTransaction = type == "Sales Transaction" || type == "Void Sales";

                var names = new ArrayList { "@From", "@To" };
                var values = new ArrayList { dtpFrom.Value.Date, dtpTo.Value.Date };

                if (isTransaction)
                {
                    names.AddRange(new[] { "@CahierID", "@InvoiceType", "@Status" });
                    values.AddRange(new object[]
                    {
                        Convert.ToString(cbBoxStaff.SelectedValue),
                        type == "Sales Transaction" ? "SALES" : "VOID",
                        cBoxStatus.Text
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

            btnPurchaseItem.Click += (sender, args) =>
            {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales item to view Purchase Items.", "Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (PurchaseItemView purchaseItemView = new PurchaseItemView(Convert.ToInt32(receiptNo)))
                    {
                        purchaseItemView.StartPosition = FormStartPosition.CenterParent;
                        //purchaseItemView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(purchaseItemView, modalOwner);
                    }
                }
            };

            btnReprint.Click += (sender, args) =>
            {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales transaction to Re-Print Receipt.", "Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (PrintConfirmView printConfirmView = new PrintConfirmView(Convert.ToInt32(receiptNo)))
                    {
                        try
                        {
                            printConfirmView.StartPosition = FormStartPosition.CenterParent;
                            //printConfirmView.ShowDialog(modalOwner);
                            POSTheme.ShowSmoothFadeDialog(printConfirmView, modalOwner);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                       
                    }
                }
            };

            btnPayment.Click += (sender, args) =>
            {
                string receiptNo = SelectedReceiptNo;
                if (string.IsNullOrWhiteSpace(receiptNo))
                {
                    MessageBox.Show("Select an sales transaction to view Payment Info.", "Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            btnExportExcel.Click += delegate { ExporttoExcel(); };

            if (dataGridViewX2 != null)
            {
                dataGridViewX2.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void ExporttoExcel()
        {
            var cols = dataGridViewX1.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
            var rows = dataGridViewX1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();

            if (cols.Count == 0 || rows.Count == 0)
            {
                MessageBox.Show("There is no sales report to export.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog
            {
                Filter = "Excel workbook|*.xlsx",
                Title = "Export Sales",
                FileName = "Sales.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    UseWaitCursor = true;
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Sales");

                        for (int c = 0; c < cols.Count; c++)
                        {
                            ws.Cell(1, c + 1).Value = cols[c].HeaderText;
                            bool isCode = "ITEM_CODE".Equals(cols[c].DataPropertyName, StringComparison.OrdinalIgnoreCase)
                                       || "ITEM_CODE".Equals(cols[c].HeaderText, StringComparison.OrdinalIgnoreCase);

                            for (int r = 0; r < rows.Count; r++)
                            {
                                object raw = rows[r].Cells[cols[c].Index].Value;
                                object v = raw == null || raw == DBNull.Value ? string.Empty : raw;
                                var cell = ws.Cell(r + 2, c + 1);

                                if (isCode)
                                {
                                    string s = Convert.ToString(v);
                                    cell.Value = s == "" ? "" : "'" + s;
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else cell.Value = v;
                            }
                        }

                        var table = ws.Range(1, 1, rows.Count + 1, cols.Count).CreateTable("SalesTable");
                        table.Theme = XLTableTheme.TableStyleMedium2;
                        ws.SheetView.FreezeRows(1);
                        ws.Columns().AdjustToContents();
                        wb.SaveAs(dlg.FileName);
                    }
                    MessageBox.Show("Sales report exported successfully.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not export sales report. " + ex.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { UseWaitCursor = false; }
            }
        }

        public string SelectedReceiptNo
        {
            get
            {
                if (dataGridViewX1 == null || dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells.Count == 0)
                    return string.Empty;

                var value = dataGridViewX1.CurrentRow.Cells[0].Value;
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }


        public event EventHandler SearchRequested;
        public event EventHandler ReloadRequested;
        public event EventHandler PaymentRequested;
        public event EventHandler VoidSalesRequested;
        public event EventHandler CompleteTransactionRequested;
        public event EventHandler ReprintRequested;

        public string SearchText
        {
            get { return txtSearchParam.Text; }
            set { txtSearchParam.Text = value ?? string.Empty; }
        }

        public string TypeOfSales
        {
            get { return cbBoxTypeSales.Text; }
            set { cbBoxTypeSales.Text = value ?? string.Empty; }
        }

        public string PaymentType
        {
            get { return comboBox2.Text; }
            set { comboBox2.Text = value ?? string.Empty; }
        }

        public string PaymentStatus
        {
            get { return cBoxStatus.Text; }
            set { cBoxStatus.Text = value ?? string.Empty; }
        }

        public string StaffName
        {
            get { return cbBoxStaff.Text; }
            set { cbBoxStaff.Text = value ?? string.Empty; }
        }

        public string StaffId
        {
            get { return Convert.ToString(cbBoxStaff.SelectedValue); }
        }

        public DateTime DateFrom
        {
            get { return dtpFrom.Value; }
            set { dtpFrom.Value = value; }
        }

        public DateTime DateTo
        {
            get { return dtpTo.Value; }
            set { dtpTo.Value = value; }
        }

        public DataTable SalesGridData
        {
            get { return dataGridViewX1.DataSource as DataTable; }
            set { dataGridViewX1.DataSource = value; }
        }

        public DataTable SalesGridSummaryData
        {
            get { return dataGridViewX2.DataSource as DataTable; }
            set { dataGridViewX2.DataSource = value; }
        }

        public DataGridView SalesGrid
        {
            get { return dataGridViewX1; }
        }

        public DataGridView SalesGridSummary
        {
            get { return dataGridViewX2; }
        }

        public string TotalRevenue
        {
            get { return lblTotalRevenue.Text; }
            set { lblTotalRevenue.Text = value ?? string.Empty; }
        }

        public string AmountPaid
        {
            get { return lblAmountPaid.Text; }
            set { lblAmountPaid.Text = value ?? string.Empty; }
        }

        public string Balance
        {
            get { return lblBalance.Text; }
            set { lblBalance.Text = value ?? string.Empty; }
        }

        public string GrossProfit
        {
            get { return lblGrossProfit.Text; }
            set { lblGrossProfit.Text = value ?? string.Empty; }
        }

        public void SetStaffList(BindingSource staff)
        {
            cbBoxStaff.DataSource = staff;
            cbBoxStaff.DisplayMember = "nameuser";
            cbBoxStaff.ValueMember = "usernm";
            cbBoxStaff.SelectedIndex = -1;
        }

        public void SetSalesTypeList(IList<string> salesTypes)
        {
            suppressSalesTypeChanged = true;
            try
            {
                cbBoxTypeSales.DataSource = salesTypes;
                if (salesTypes != null && salesTypes.Contains("Sales Transaction"))
                    cbBoxTypeSales.SelectedItem = "Sales Transaction";
            }
            finally
            {
                suppressSalesTypeChanged = false;
            }

            ApplySalesTypeBehavior();
        }

        public void SetPaymentStatusList(IList<string> statuses)
        {
            cBoxStatus.DataSource = statuses;
        }

        public void SetPaymentTypeList(IList<string> paymentTypes)
        {
            comboBox2.DataSource = paymentTypes;
        }

        public void SetSummaryVisibility(bool showGrid, bool showRevenue, bool showAmountPaid,
            bool showBalance, bool showGrossProfit)
        {
            dataGridViewX2.Visible = showGrid;
            if (!showGrid)
            {
                dataGridViewX1.Height = dataGridViewX1.Parent.ClientSize.Height
                            - dataGridViewX1.Top
                            - dataGridViewX1.Margin.Bottom
                            - 39;
                panel2.Height = 39;
            }
            else
            {
                dataGridViewX1.Height = dataGridViewX1.Parent.ClientSize.Height
                            - dataGridViewX1.Top
                            - dataGridViewX1.Margin.Bottom
                            - 93;
                panel2.Height = 93;
            }
            lblTotalRevenue.Visible = showRevenue;
            lblAmountPaid.Visible = showAmountPaid;
            lblBalance.Visible = showBalance;
            lblGrossProfit.Visible = showGrossProfit;
        }

        private SkeletonLoader CreateSkeletonLoader(Control target)
        {
            SkeletonLoader loader = new SkeletonLoader();
            loader.Anchor = target.Anchor;
            loader.Bounds = new Rectangle(target.Left, target.Top, target.Width, target.Height);
            loader.Visible = false;
            target.Parent.Controls.Add(loader);
            loader.BringToFront();
            return loader;
        }

        private static void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }

        public void ShowFilterLoading()
        {
            SetLoading(cbBoxTypeSales, typeSalesSkeletonLoader, true);
            SetLoading(cbBoxStaff, staffSkeletonLoader, true);
            SetLoading(cBoxStatus, statusSkeletonLoader, true);
            SetLoading(comboBox2, paymentTypeSkeletonLoader, true);
            SetLoading(dtpFrom, dateFromSkeletonLoader, true);
            SetLoading(dtpTo, dateToSkeletonLoader, true);
            SetLoading(txtSearchParam, searchSkeletonLoader, true);
        }

        public void HideFilterLoading()
        {
            SetLoading(cbBoxTypeSales, typeSalesSkeletonLoader, false);
            SetLoading(cbBoxStaff, staffSkeletonLoader, false);
            SetLoading(cBoxStatus, statusSkeletonLoader, false);
            SetLoading(comboBox2, paymentTypeSkeletonLoader, false);
            SetLoading(dtpFrom, dateFromSkeletonLoader, false);
            SetLoading(dtpTo, dateToSkeletonLoader, false);
            SetLoading(txtSearchParam, searchSkeletonLoader, false);
        }

        public void ShowSalesLoading()
        {
            SetLoading(dataGridViewX1, salesGridSkeletonLoader, true);
        }

        public void HideSalesLoading()
        {
            SetLoading(dataGridViewX1, salesGridSkeletonLoader, false);
        }

        public void RefreshData()
        {
            ReloadRequested?.Invoke(this, EventArgs.Empty);
        }

        public void GetSalesData(string searchParam)
        {
            SearchText = searchParam;
            SearchSales();
        }

        public void SearchSales()
        {
            SearchRequested?.Invoke(this, EventArgs.Empty);
        }

        public void LoadSalesData()
        {
            RefreshData();
        }

        public void OnPaymentClick()
        {
            PaymentRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnVoidSalesClick()
        {
            VoidSalesRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnCompleteTransactionClick()
        {
            CompleteTransactionRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnReprintClick()
        {
            ReprintRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnReloadClick()
        {
            RefreshData();
        }

        public void OnSearchClick()
        {
            SearchSales();
        }

        private void ClearFiltersAndReload()
        {
            SearchText = string.Empty;
            suppressSalesTypeChanged = true;
            try
            {
                TypeOfSales = "Sales Transaction";
            }
            finally
            {
                suppressSalesTypeChanged = false;
            }
            ApplySalesTypeBehavior();
            cbBoxStaff.SelectedIndex = -1;
            PaymentStatus = string.Empty;
            PaymentType = "ALL";
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            RefreshData();
        }

        private void SalesTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressSalesTypeChanged)
                return;

            SearchText = string.Empty;
            cbBoxStaff.SelectedIndex = -1;
            PaymentStatus = string.Empty;
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            ApplySalesTypeBehavior();
            SearchSales();
        }

        private void ApplySalesTypeBehavior()
        {
            string salesType = TypeOfSales;
            bool isTransactionType =
                salesType == "Sales Transaction" ||
                salesType == "Payment History" ||
                salesType == "Pending Transaction";
            bool isVoidType = salesType == "Void Sales";
            bool isPendingType = salesType == "Pending Transaction";

            label5.Text = isTransactionType || isVoidType
                ? "Search Receipt No/Customer"
                : "Search Itemcode/Name/Category";

            label8.Enabled = isTransactionType;
            cBoxStatus.Enabled = isTransactionType;
            btnVoid.Enabled = isTransactionType;
            btnPurchaseItem.Enabled = isTransactionType || isVoidType;
            btnPayment.Enabled = isTransactionType;
            btnCompleted.Enabled = isPendingType;
        }

        //Singleton pattern (Open a single form instance)
        private static SalesView instance;
        public static SalesView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new SalesView();
                instance.MdiParent = parentContainer;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
    }
}