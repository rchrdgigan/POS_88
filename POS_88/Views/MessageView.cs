using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class MessageView : Form
    {
        private CashierView frmcashier;
        private PaymentModel _paymentModel;
        private bool _opening;
        private DateTime _shownAt = DateTime.MaxValue;

        public MessageView()
        {
            InitializeComponent();
        }

        public MessageView(CashierView frm, PaymentModel paymentModel)
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Fuchsia;
            TransparencyKey = Color.Fuchsia;
            StartPosition = FormStartPosition.CenterParent;

            frmcashier = frm;
            _paymentModel = paymentModel;
            KeyPreview = true;

            Shown += (s, e) => _shownAt = DateTime.Now;
            btnOk.Click += (s, e) => btnEnter_Click();
        }

        // print
        private void btnEnter_Click()
        {
            // ignore the Enter still "in flight" from the payment confirm
            if ((DateTime.Now - _shownAt).TotalMilliseconds < 400) return;
            if (_opening) return;          // open the print dialog only once
            _opening = true;

            // Cover the whole cashier screen. Owned by this form so it sits above it.
            var overlay = new LoadingOverlay(frmcashier, "Preparing receipt...");
            overlay.Show(this);
            overlay.Refresh();             // paint it right now
            overlay.BackdropOnly();        // the print dialog will sit on top, so keep just the dim
            Opacity = 0;                   // vanish behind the overlay, no Hide() repaint

            try
            {
                // owner = overlay, so the print dialog appears above it
                using (var print = new PrintConfirmView(_paymentModel.receiptno, "", 1, frmcashier))
                    print.ShowDialog(overlay);
            }
            catch (Exception)
            {
                MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.",
                    "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                overlay.Close();
                overlay.Dispose();
            }
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.Return)
            {
                btnEnter_Click();
                return true;               // don't let btnOk react to the same key
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void MessageView_Load(object sender, EventArgs e)
        {
            lblAmountDue.Text = _paymentModel.amountDue.ToString("N");
            lblReceiptNo.Text = Convert.ToString(_paymentModel.receiptno);
            lblCashTendered.Text = _paymentModel.paymentAmt.ToString("N");
            lblChange.Text = _paymentModel.changeAmt.ToString("N");
            lblTableNo.Text = _paymentModel.tablenm;
            lblBalance.Text = _paymentModel.balance.ToString("N");
            lblStatus.Text = _paymentModel.status;
        }
    }
}