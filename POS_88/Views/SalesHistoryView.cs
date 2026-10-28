using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class SalesHistoryView : Form, ISalesHistoryView
    {
        public SalesHistoryView()
        {
            InitializeComponent();
            ApplyTheme();

            btnMinimize.Click += delegate { WindowState = FormWindowState.Minimized; };
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            btnSearch.Click += delegate { SearchEvent?.Invoke(this, EventArgs.Empty); };
            btnClearFilter.Click += delegate { ClearEvent?.Invoke(this, EventArgs.Empty); };

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


            btnExportExcel.Click += delegate { ExporttoExcel(); };


        }

        private void ExporttoExcel()
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
                Title = "Export Sales History",
                FileName = "SalesReport.xlsx"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        workbook.Worksheets.Add(table, "SalesReport");
                        workbook.SaveAs(dialog.FileName);
                    }
                    MessageBox.Show("Stock history exported successfully.", "Sales History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(dialog.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        public string CustomerContact
        {
            get
            {
                return txtContact.Text;
            }

            set
            {
                txtContact.Text = value;
            }
        }

        public string CustomerID
        {
            get
            {
                return txtCustomerID.Text;
            }

            set
            {
                txtCustomerID.Text = value;
            }
        }

        public string CustomerName
        {
            get
            {
                return txtCustomerName.Text;
            }

            set
            {
                txtCustomerName.Text = value;
            }
        }

        public DateTime From
        {
            get { return dtpFrom.Value; }
            set { dtpFrom.Value = value; }
        }

        public DateTime To
        {
            get { return dtpTo.Value; }
            set { dtpTo.Value = value; }
        }

        public string ModalTitle
        {
            get
            {
                return lblModalTitle.Text;
            }

            set
            {
               lblModalTitle.Text = value;
            }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SearchEvent;

        public void SetSalesHistoryListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }
    }
}
