using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class ConfirmPaymentView : Form
    {
        private CashierPaymentView frmPay;
        private ConfirmPaymentView frmConfirm;
        private Decimal paymentAmt;
        private Decimal changeAmt;
        private Decimal pointsRedeem;
        private Decimal pointsAmt;
        private bool isClick;

        public ConfirmPaymentView(
            CashierPaymentView frm,
            Decimal paymentAmt,
            Decimal changeAmt,
            Decimal balance,
            Decimal pointsRedeem,
            Decimal pointsAmt,
            string status)
        {
            InitializeComponent();
            this.frmPay = frm;
            this.paymentAmt = paymentAmt;
            this.changeAmt = changeAmt;
            this.pointsRedeem = pointsRedeem;
            this.pointsAmt = pointsAmt;
            this.lblStatus.Text = status;
            this.lblPayment.Text = paymentAmt.ToString("N");
            this.lblBalance.Text = balance.ToString("N");
            this.lblChange.Text = changeAmt.ToString("N");
            this.frmConfirm = this;
            this.isClick = false;

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Fuchsia;
            this.TransparencyKey = Color.Fuchsia;
            this.StartPosition = FormStartPosition.CenterParent;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Cancel();
                return true;
            }
            if (keyData == Keys.Return)
            {
                Accept();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Accept()
        {
            if (isClick) return;          // ignore double Enter / double click
            isClick = true;
            DialogResult = DialogResult.OK;   // this also closes the dialog
        }

        private void Cancel()
        {
            frmPay.isEnter = false;
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e) { Accept(); }
        private void btnClose_Click(object sender, EventArgs e) { Cancel(); }
        private void Confirmation() => this.frmPay.Confirm();
      

    }
}
