using POS_88.Database;
using POS_88.Persistence;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class PaymentView : Form
    {
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly int receiptNumber;
        private invoice currentInvoice;
        private string userName = Environment.UserName;
        //private CashierReportView cashierReport;

        // Values kept like the old frmEnterPayment
        private decimal amountDue;
        private decimal amtPaid;
        private decimal balance;
        private decimal paymentAmt;
        private decimal changeAmt;
        private decimal newBalance;
        private decimal totalAmtPaid;
        private string status;

        public PaymentView()
            : this(0)
        {
        }

        public PaymentView(string userNm, int receiptno, CashierReportView frm)
            : this(receiptno)
        {
            if (!string.IsNullOrWhiteSpace(userNm))
                userName = userNm;
            //cashierReport = frm;
        }

        public PaymentView(int receiptNumber)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.receiptNumber = receiptNumber;

            btnMinimize.Click += delegate { WindowState = FormWindowState.Minimized; };
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            cBoxPaymentType.Items.Clear();
            cBoxPaymentType.Items.AddRange(new[]
            {
                "CASH",
                "CREDIT CARD",
                "DEBIT CARD",
                "GCASH",
                "CL",
                "BANK TRANSFER",
                "UNIONBANK",
                "BDO",
                "CREDIT"
            });

            txtPayment.TextChanged += delegate { CalculateChange(); };
            txtPayment.Leave += delegate { txtPayment.Text = paymentAmt.ToString("N2"); };
            btnSave.Click += OnSaveClick;

            if (receiptNumber > 0)
            {
                LoadInvoiceDetails();
            }
        }

        private void LoadInvoiceDetails()
        {
            currentInvoice = unitOfWork.Invoice.Find(x => x.receiptno == receiptNumber).FirstOrDefault();
            if (currentInvoice == null)
            {
                MessageBox.Show("The selected receipt was not found.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            customer customer = unitOfWork.Customers.Find(x => x.customerid == currentInvoice.customerid).FirstOrDefault();

            amountDue = currentInvoice.amountdue ?? 0m;
            amtPaid = currentInvoice.amountpaid ?? 0m;
            balance = currentInvoice.balance ?? Math.Max(amountDue - amtPaid, 0m);

            txtReceiptNo.Text = currentInvoice.receiptno.ToString();
            txtCustomerName.Text = customer != null ? customer.customername : "Walk-in";

            DateTime? invoiceDate = currentInvoice.solddate ?? currentInvoice.crtddt;
            txtDate.Text = invoiceDate.HasValue ? invoiceDate.Value.ToString("MM/dd/yyyy HH:mm:ss") : DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            txtAmountDue.Text = amountDue.ToString("N2");
            txtPaid.Text = amtPaid.ToString("N2");
            txtBalance.Text = balance.ToString("N2");
            txtTotalAmountPaid.Text = amtPaid.ToString("N2");
            txtNewBalance.Text = balance.ToString("N2");
            txtChange.Text = "0.00";
            txtPayment.Text = "0.00";          // old form starts at 0.00
            txtRemarks.Text = string.Empty;

            cBoxPaymentType.SelectedIndex = 0;

            bool isPaid = balance <= 0m
                          || string.Equals(currentInvoice.paymentstatus, "PAID", StringComparison.OrdinalIgnoreCase);

            lblPaidOrUnpaid.Text = isPaid ? "PAID" : "UNPAID";
            lblPaidOrUnpaid.ForeColor = isPaid ? Color.FromArgb(0, 141, 85) : Color.FromArgb(198, 40, 40);

            // Old behavior: a fully paid invoice cannot take another payment
            txtPayment.Enabled = !isPaid;
            cBoxPaymentType.Enabled = !isPaid;
            txtRemarks.Enabled = !isPaid;
            btnSave.Enabled = !isPaid;
        }

        private void CalculateChange()
        {
            if (currentInvoice == null)
                return;

            if (!decimal.TryParse(txtPayment.Text, out paymentAmt))
                paymentAmt = 0m;

            changeAmt = paymentAmt - balance;
            newBalance = balance - paymentAmt;

            if (newBalance <= 0m)
            {
                newBalance = 0m;
                status = "PAID";
            }
            else
            {
                status = "UNPAID";
            }

            if (changeAmt < 0m)
                changeAmt = 0m;

            totalAmtPaid = amtPaid + paymentAmt;
            if (totalAmtPaid > amountDue)
                totalAmtPaid = amountDue;

            txtChange.Text = changeAmt.ToString("N2");
            txtNewBalance.Text = newBalance.ToString("N2");
            txtTotalAmountPaid.Text = totalAmtPaid.ToString("N2");
            lblPaidOrUnpaid.Text = status;
            lblPaidOrUnpaid.ForeColor = status == "PAID" ? Color.FromArgb(0, 141, 85) : Color.FromArgb(198, 40, 40);
        }

        private void SavePayment()
        {
            // Invoice (same rules as old SavePayment)
            if (paymentAmt > amountDue)
            {
                currentInvoice.cashtender = paymentAmt;
                currentInvoice.changeamt = changeAmt;
            }
            else
            {
                currentInvoice.cashtender = totalAmtPaid;
            }

            currentInvoice.amountpaid = totalAmtPaid;
            currentInvoice.balance = newBalance;
            currentInvoice.paymentstatus = lblPaidOrUnpaid.Text;
            unitOfWork.Invoice.Update(currentInvoice);

            // Payment history (same rules as old SavePayment)
            paymenthistory history = new paymenthistory();
            history.receiptno = receiptNumber;
            history.paymode = cBoxPaymentType.Text;
            history.paytype = cBoxPaymentType.Text;

            if (paymentAmt >= balance)
            {
                history.amount = balance;       // only record what was actually owed
                history.balance = 0m;
            }
            else
            {
                history.amount = paymentAmt;
                history.balance = newBalance;
            }

            history.cashtender = paymentAmt;
            history.paydate = DateTime.Now;
            history.usernm = userName;
            history.readflag = false;           // old form used false
            history.remarks = txtRemarks.Text;
            unitOfWork.Payment.Add(history);

            unitOfWork.Complete();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (currentInvoice == null)
            {
                MessageBox.Show("No invoice selected for payment.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (paymentAmt <= 0m)
            {
                MessageBox.Show("Please check entered amount", "Payment Not Accepted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Do you want to save payment? ", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                SavePayment();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payment failed: " + ex.Message, "Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Payment successfully saved", "Payment Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //if (cashierReport != null)
            //    cashierReport.GetSalesData("");   // refresh the cashier report grid

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}