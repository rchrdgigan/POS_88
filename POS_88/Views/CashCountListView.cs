using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using ClosedXML.Excel;
using POS_88.Presenters;

namespace POS_88.Views
{
    public partial class CashCountListView : Form, ICashCountListView
    {
        public CashCountListView()
        {
            InitializeComponent();
            ApplyTheme();

            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
            btnExportExcel.Click += (sender, args) => ExportExcelEvent?.Invoke(this, EventArgs.Empty);
            btnPrint.Click += (s, e) => {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an user cash count record to print.", "Cash Count", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    int num = (int)new PrintCashCountView(Convert.ToInt32(this.dataGridViewX1.CurrentRow.Cells["SqNo"].Value.ToString())).ShowDialog();
                }
                catch (Exception)
                {

                    MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            btnCashInOut.Click += (s, e) => {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an user cash count record to view Cash In/Out.", "Cash Count", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int num = (int)new CashInOutHistoryView(Convert.ToInt32(this.dataGridViewX1.CurrentRow.Cells["SqNo"].Value.ToString())).ShowDialog();
            };
            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) =>
            {
                dtpFrom.Value = DateTime.Today;
                dtpTo.Value = DateTime.Today;
                ClearEvent?.Invoke(this, EventArgs.Empty);
            };
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

        public string GrandTotal
        {
            get { return lblGrandTotal.Text; }
            set { lblGrandTotal.Text = value ?? string.Empty; }
        }

        public string OpeningAmount
        {
            get { return lblOpeningAmount.Text; }
            set { lblOpeningAmount.Text = value ?? string.Empty; }
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

        public event EventHandler ClearEvent;
        public event EventHandler SearchEvent;
        public event EventHandler ExportExcelEvent;

        public void SetCashCountListBindingSource(BindingSource list)
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
                MessageBox.Show("There is no cash count list to export.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel workbook|*.xlsx",
                Title = "Export Cash Count List",
                FileName = "CashCountList.xlsx"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        workbook.Worksheets.Add(table, "Cash Count List");
                        workbook.SaveAs(dialog.FileName);
                    }
                    MessageBox.Show("Cash Count List exported successfully.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintCashCountReceipt()
        {
        }
    }
}
