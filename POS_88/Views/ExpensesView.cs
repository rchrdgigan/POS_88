using POS_88.Presenters;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ClosedXML.Excel;


namespace POS_88.Views
{
    public partial class ExpensesView : Form, IExpensesView
    {
        private readonly SkeletonLoader usersSkeletonLoader;
        private readonly SkeletonLoader totalusersSkeletonLoader;

        private SkeletonLoader CreateSkeletonLoader(Control target, int? width = null)
        {
            SkeletonLoader loader = new SkeletonLoader();
            loader.Anchor = target.Anchor;
            loader.Bounds = new Rectangle(target.Left, target.Top, width ?? target.Width, target.Height);
            loader.Visible = false;
            target.Parent.Controls.Add(loader);
            loader.BringToFront();
            return loader;
        }
        public ExpensesView()
        {
            InitializeComponent();

            usersSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totalusersSkeletonLoader = CreateSkeletonLoader(lblTotalExpense, 100);

            btnAddNew.Click += (sender, args) =>
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

                    using (AddOrEditExpensesView addView = new AddOrEditExpensesView())
                    {
                        new AddOrEditExpensesPresenter(
                            addView,
                            null,
                            null,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addView.StartPosition = FormStartPosition.CenterParent;
                        //addView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                    }
                }
            };

            btnEdit.Click += (sender, args) =>
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an expens before editing.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (AddOrEditExpensesView editView = new AddOrEditExpensesView())
                    {
                        new AddOrEditExpensesPresenter(
                            editView,
                            null,
                            this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString(),
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        editView.StartPosition = FormStartPosition.CenterParent;
                        //editView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(editView, modalOwner);
                    }
                }
            };

            btnPrint.Click += (sender, args) => {
                ArrayList paramArrayList = new ArrayList();
                ArrayList paramValueArrayList = new ArrayList();
                System.DateTime date1 = this.dtpFrom.Value.Date;
                System.DateTime date2 = this.dtpTo.Value.Date;
                paramArrayList.Add((object)"@From");
                paramValueArrayList.Add((object)date1);
                paramArrayList.Add((object)"@To");
                paramValueArrayList.Add((object)date2);
                ReportViewerView reportView = new ReportViewerView(
                    "expenses.rpt",
                    paramArrayList,
                    paramValueArrayList);
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
            };


            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (sender, args) => DeleteEvent?.Invoke(this, EventArgs.Empty);
            btnExportExcel.Click += (sender, args) =>
            {
                DataTable table = dataGridViewX1.DataSource as DataTable;
                BindingSource bindingSource = dataGridViewX1.DataSource as BindingSource;
                if (table == null && bindingSource != null)
                    table = bindingSource.DataSource as DataTable;

                if (table == null || table.Rows.Count == 0)
                {
                    MessageBox.Show("There is no expenses record to export.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "Excel workbook|*.xlsx",
                    Title = "Export Expenses",
                    FileName = "Expenses.xlsx"
                })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(table, "Expenses");
                            workbook.SaveAs(dialog.FileName);
                        }
                        MessageBox.Show("Expenses exported successfully.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(dialog.FileName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            btnClearFilter.Click += (sender, args) =>
            {
                ClearEvent?.Invoke(this, EventArgs.Empty);
            };

            dataGridViewX1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public string SelectedExpense
        {
            get
            {
                if (dataGridViewX1 == null || dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells.Count == 0)
                    return string.Empty;

                var value = dataGridViewX1.CurrentRow.Cells[0].Value;
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }

        public string From
        {
            get { return dtpFrom.Value.ToString("d", CultureInfo.CurrentCulture); }
            set
            {
                DateTime date;
                if (DateTime.TryParse(value, out date))
                    dtpFrom.Value = date;
            }
        }

        public string SearchParms
        {
            get { return txtSearchParam.Text; }
            set { txtSearchParam.Text = value; }
        }

        public string To
        {
            get { return dtpTo.Value.ToString("d", CultureInfo.CurrentCulture); }
            set
            {
                DateTime date;
                if (DateTime.TryParse(value, out date))
                    dtpTo.Value = date;
            }
        }

        public string TotalExpenses
        {
            get { return lblTotalExpense.Text; }
            set { lblTotalExpense.Text = value; }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SearchEvent;
        public event EventHandler DeleteEvent;

        public void SetExpensesListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }


        private static ExpensesView instance;
        public static ExpensesView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new ExpensesView();
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

        public void ClearFilters()
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            this.SearchParms = string.Empty;
        }

        public void ShowExpensesListLoading()
        {
            SetLoading(dataGridViewX1, usersSkeletonLoader, true);
            SetLoading(lblTotalExpense, totalusersSkeletonLoader, true);
        }

        public void HideExpensesListLoading()
        {
            SetLoading(dataGridViewX1, usersSkeletonLoader, false);
            SetLoading(lblTotalExpense, totalusersSkeletonLoader, false);
        }
        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }
    }
}
