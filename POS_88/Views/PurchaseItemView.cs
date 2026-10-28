using POS_88.Presenters;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class PurchaseItemView : Form, IPurchaseItemView
    {
        private readonly PurchaseItemsPresenter purchaseItemsPresenter = new PurchaseItemsPresenter();
        private int _receiptNo;

        public PurchaseItemView() : this(0) { }

        public PurchaseItemView(int receiptNo)
        {
            InitializeComponent();
            ApplyTheme();

            _receiptNo = receiptNo;

            btnMinimize.Click += delegate { WindowState = FormWindowState.Minimized; };
            btnClose.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            btnPrint.Click += btnPrint_Click;

            if (_receiptNo > 0) LoadDetails(_receiptNo);
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

        public int ReceiptNo
        {
            get { return _receiptNo; }
            set
            {
                _receiptNo = value;
                if (_receiptNo > 0) LoadDetails(_receiptNo);
            }
        }

        private static string N(object value)
        {
            return value == null ? "0.00" : ((IFormattable)value).ToString("N", null);
        }

        public void LoadDetails(int receiptNo)
        {
            _receiptNo = receiptNo;

            var t = purchaseItemsPresenter.GetTransaction(receiptNo);
            if (t == null) return;

            string customer = t.CUSTOMER ?? "WALK-IN";

            txtReceiptNo.Text = t.RECEIPT_NO.ToString();
            txtCustomerName.Text = t.CUSTOMER ?? string.Empty;
            txtDate.Text = t.DATE.HasValue ? t.DATE.Value.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty;

            lblPaidOrUnpaid.Text = t.STATUS ?? "PAID";
            lblPaidOrUnpaid.ForeColor = t.STATUS == "PAID" ? Color.SeaGreen : Color.FromArgb(198, 40, 40);

            lblCustomerReceiptNo.Text = string.Format("({0}-{1})", customer, receiptNo);
            Text = string.Format("Purchased Items ({0}-{1})", customer, receiptNo);

            lblSubTotal.Text = N(t.SUBTOTAL);
            lblSubDiscount.Text = N(t.SUB_DISCOUNT);
            lblAmountDue.Text = N(t.AMOUNT_DUE);
            lblBalance.Text = N(t.BALANCE);
            lblVATSales.Text = N(t.VAT_SALES);
            lblVATAmount.Text = N(t.VAT_AMOUNT);
            lblNONVATSales.Text = N(t.NON_VAT_SALES);
            lblVATExempt.Text = N(t.VAT_EXEMPT_SALES);
            lblZeroRated.Text = N(t.ZERO_RATED_SALES);
            lblNoItems.Text = N(t.NO_OF_ITEMS);

            dataGridViewX1.DataSource = purchaseItemsPresenter.GetPurchasedItems(receiptNo).ToList();
            dataGridViewX1.AutoGenerateColumns = true;
            dataGridViewX1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var reportView = new ReportViewerView("Invoice.rpt", new ArrayList { "@ReceiptNo" }, new ArrayList { _receiptNo });
                new ReportViewerPresenter(reportView).LoadReport();
                reportView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchase Items", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}