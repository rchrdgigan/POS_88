using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using POS_88.Presenters;

namespace POS_88.Views
{
    public partial class StockHistoryView : Form, IStockHistoryView
    {
        private string itemCode;

        public StockHistoryView()
        {
            InitializeComponent();
            ApplyTheme();
            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) =>
            {
                dtpFrom.Value = DateTime.Today;
                dtpTo.Value = DateTime.Today;
                ClearEvent?.Invoke(this, EventArgs.Empty);
            };
            btnExportExcel.Click += (sender, args) => ExportToExcel();
            btnPrint.Click += (sender, args) => PrintStockHistory();

            dataGridViewX1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ApplyTheme()
        {
            // topbar gradients
            lblModalTitle.BackColor = Color.Transparent;
            POSTheme.ApplyTopbar(panel1);

            // button close
            btnMinimize.BackColor = POSTheme.Navy;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

            // button close
            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;
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

        public string ItemCode
        {
            get { return itemCode; }
            set { itemCode = value; }
        }

        public string ItemName
        {
            get { return lblItemName.Text; }
            set { lblItemName.Text = value; }
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

        public void SetStockHistoryListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }

        private void ExportToExcel()
        {
            DataTable table = dataGridViewX1.DataSource as DataTable;
            BindingSource bindingSource = dataGridViewX1.DataSource as BindingSource;
            if (table == null && bindingSource != null)
                table = bindingSource.DataSource as DataTable;

            if (table == null || table.Rows.Count == 0)
            {
                MessageBox.Show("There is no stock history to export.", "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel workbook|*.xlsx",
                Title = "Export Stock History",
                FileName = "StockHistory.xlsx"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        workbook.Worksheets.Add(table, "Stock History");
                        workbook.SaveAs(dialog.FileName);
                    }
                    MessageBox.Show("Stock history exported successfully.", "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintStockHistory()
        {
            ArrayList parameterNames = new ArrayList { "@ItemCode", "@From", "@To" };
            ArrayList parameterValues = new ArrayList
            {
                ItemCode,
                DateTime.Parse(From).Date,
                DateTime.Parse(To).Date
            };
            ReportViewerView reportView = new ReportViewerView("StockHistory.rpt", parameterNames, parameterValues);
            try
            {
                new ReportViewerPresenter(reportView).LoadReport();
                reportView.Show();
            }
            catch (Exception exception)
            {
                reportView.Dispose();
                MessageBox.Show(exception.Message, "Stock History", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SearchEvent;
    }
}
