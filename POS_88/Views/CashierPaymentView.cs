using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Views
{
    public partial class CashierPaymentView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private UnitOfWorkPOS _unitOfWorkPOS;
        private POSImplementation _posImplement;
        private setting _setting;
        private CashierView frmcash;
        private CashierPaymentView frmpay;
        private Decimal amountDue;
        private Decimal paymentAmt;
        private Decimal changeAmt;
        private Decimal subtotalAmt;
        private Decimal balance;
        private Decimal pointsAmt;
        private Decimal pointsRedeem;
        private string note;
        private string driverName;
        private string status;
        private string tableNo;
        private bool iswithAR;
        private bool isCreditLimit;
        private Decimal _downpercent;
        public bool isEnter;
        public bool _init;

        public CashierPaymentView(CashierView frmcash, Decimal amtdue)
        {
            InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._unitOfWorkPOS = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
            this._setting = new setting();
            this.frmcash = frmcash;
            this.subtotalAmt = amtdue;
            this.iswithAR = false;
            this.isCreditLimit = false;
            this._downpercent = 0M;
            this.lblSubtotal.Text = this.subtotalAmt.ToString("N");
            this.txtEnterAmt.Text = amtdue.ToString("N");
            this.txtStatus.Text = "PAID";
            this.frmpay = this;
            if (this.frmpay.Height >= 605)
                this.frmpay.Height -= 35;
            this.lblWarning.Visible = false;
            this.timer1.Enabled = false;
            this.isEnter = false;
            this._init = true;

            btn0.Click += delegate { this.InpNum(this.btn0.Text); };
            btn1.Click += delegate { this.InpNum(this.btn1.Text); };
            btn2.Click += delegate { this.InpNum(this.btn2.Text); };
            btn3.Click += delegate { this.InpNum(this.btn3.Text); };
            btn4.Click += delegate { this.InpNum(this.btn4.Text); };
            btn5.Click += delegate { this.InpNum(this.btn5.Text); };
            btn6.Click += delegate { this.InpNum(this.btn6.Text); };
            btn7.Click += delegate { this.InpNum(this.btn7.Text); };
            btn8.Click += delegate { this.InpNum(this.btn8.Text); };
            btn9.Click += delegate { this.InpNum(this.btn9.Text); };
            btnDot.Click += delegate { this.InpNum(this.btnDot.Text); };
            btnClear.Click += btnClear_Click;
            btnBackSpace.Click += btnBackSpace_Click;
        }

        private void CashierPaymentView_Load(object sender, EventArgs e)
        {
            this.PopulateCombos();
            this.txtEnterAmt.Focus();
        }

        private void PopulateCombos()
        {
            this.cboCustomer.DataSource = this._unitOfWork.Customers.GetAll().ToList().OrderBy(x => x.customername).ToList();
            this.cboCustomer.DisplayMember = "customername";
            this.cboCustomer.ValueMember = "customerid";
            this.cboCustomer.SelectedValue = (object)1;
            this.cboPaymentType.Text = "CASH";
            this.cboType.Text = "SALES";
            this.cboTable.Items.Clear();
            for (int index = 1; index < 101; ++index)
                this.cboTable.Items.Add((object)index);
            setting setting1 = new setting();
            setting setting2 = this._unitOfWork.Setting.Find(x => x.settingsid == 1).FirstOrDefault();
            this.iswithAR = setting2.withAR.GetValueOrDefault();
            this.isCreditLimit = setting2.enablecreditlimit.GetValueOrDefault();
            this._downpercent = setting2.downpayment;
            this._setting = setting2;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            if (keyData == (Keys.Return | Keys.Alt))
            {
                customer customer = this._unitOfWorkPOS.Customers.Find(x => x.idno == this.txtIdNo.Text).FirstOrDefault();
                if (customer != null)
                {
                    this.cboCustomer.SelectedValue = (object)customer.customerid;
                }
                else
                {
                    MessageBox.Show("Customer not found!\nId No: " + this.txtIdNo.Text, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.txtIdNo.Clear();
                    this.cboCustomer.SelectedValue = (object)1;
                }
                return true;
            }
            if (keyData == Keys.Return)
            {
                this.HitEnter();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HitEnter()
        {
            int customerid;
            if (int.TryParse(this.cboCustomer.SelectedValue.ToString(), out customerid))
            {
                customer customer = this._unitOfWork.Customers.Find(x => x.customerid == customerid).FirstOrDefault();
                if (customer != null)
                {
                    string strParam = $" WHERE invoice.customerid={customer.customerid.ToString()} AND invoicetype='SALES' AND balance>0";
                    DataTable dataTable1 = new DataTable();
                    DataTable dataTable2 = this._posImplement.SearchSaleswithParam(strParam);
                    Decimal num = 0M;
                    if (dataTable2.Rows.Count > 0)
                        num = Convert.ToDecimal(dataTable2.Compute("SUM(BALANCE)", string.Empty));
                    if (this.balance + num > Convert.ToDecimal((object)customer.credit_limit) && customer.customerid != 1 && this.isCreditLimit)
                    {
                        this.timer1.Enabled = true;
                        this.lblWarning.Text = "Customer's credit limit exceeded!";
                        this.lblWarning.Visible = true;
                        if (this.frmpay.Height <= 570)
                            this.frmpay.Height += 35;
                        this.isEnter = false;
                        return;
                    }
                }
            }
            Decimal num1 = this.amountDue * (this._downpercent / 100M);
            this.tableNo = this.cboTable.Text;
            if (this.isEnter)
                return;
            if (this.paymentAmt < this.amountDue - this.pointsAmt && !this.iswithAR)
            {
                this.timer1.Enabled = true;
                this.lblWarning.Text = "Please check entered amount!";
                this.lblWarning.Visible = true;
                if (this.frmpay.Height <= 570)
                    this.frmpay.Height += 35;
                this.isEnter = false;
            }
            else if (Convert.ToInt32(this.cboCustomer.SelectedValue) == 1 && this.pointsAmt > 0M)
            {
                this.timer1.Enabled = true;
                this.lblWarning.Text = "WALK-IN customers cannot redeem points!";
                this.lblWarning.Visible = true;
                if (this.frmpay.Height <= 570)
                    this.frmpay.Height += 35;
                this.isEnter = false;
            }
            else if (Convert.ToString(this.cboCustomer.Text) == "WALK-IN" && this.pointsAmt > 0M)
            {
                this.timer1.Enabled = true;
                this.lblWarning.Text = "WALK-IN customers cannot redeem points!";
                this.lblWarning.Visible = true;
                if (this.frmpay.Height <= 570)
                    this.frmpay.Height += 35;
                this.isEnter = false;
            }
            else if (this.paymentAmt < num1 && this.iswithAR)
            {
                this.timer1.Enabled = true;
                this.lblWarning.Text = $"Downpayment should be atleast {this._downpercent.ToString("0.##")}% (Php{num1.ToString("N")})";
                this.lblWarning.Visible = true;
                if (this.frmpay.Height <= 570)
                    this.frmpay.Height += 35;
                this.isEnter = false;
            }
            else if (this.pointsRedeem > this.GetCustomerPoints())
            {
                this.timer1.Enabled = true;
                this.lblWarning.Text = $"Redeem points exceed the available points ({this.GetCustomerPoints().ToString()})";
                this.lblWarning.Visible = true;
                if (this.frmpay.Height <= 570)
                    this.frmpay.Height += 35;
                this.isEnter = false;
            }
            else
            {
                this.lblWarning.Visible = false;
                this.timer1.Enabled = false;
                if (this.frmpay.Height >= 605)
                    this.frmpay.Height -= 35;
                this.CalculateChange();

                DialogResult result;
                using (var confirm = new ConfirmPaymentView(this, this.paymentAmt, this.changeAmt,
                           this.balance, this.pointsRedeem, this.pointsAmt, this.txtStatus.Text))
                {
                    result = confirm.ShowDialog(this);
                }

                if (result == DialogResult.OK)
                {
                    this.isEnter = true;
                    this.Confirm();                      // saves the invoice; the receipt is only queued
                    this.DialogResult = DialogResult.OK; // closes this payment form
                }
                else
                {
                    this.isEnter = false;                // cancelled, allow Enter again
                }
            }
        }

        private void CalculateChange()
        {
            this.paymentAmt = 0M;
            this.balance = 0M;
            this.pointsRedeem = 0M;
            this.pointsAmt = 0M;
            if (!Decimal.TryParse(this.txtEnterAmt.Text, out this.paymentAmt))
                this.paymentAmt = 0M;
            if (!Decimal.TryParse(this.txtPointsRedeem.Text, out this.pointsRedeem))
                this.pointsRedeem = 0M;
            else if (Decimal.TryParse(this.txtPointsRedeem.Text, out this.pointsRedeem))
                this.pointsAmt = this.pointsRedeem / this._setting.conversionrate;
            if (this.pointsRedeem != 0M && this._setting.conversionrate != 0M)
                this.pointsAmt = this.pointsRedeem / this._setting.conversionrate;
            this.changeAmt = this.paymentAmt - (this.subtotalAmt - this.pointsAmt);
            this.balance = this.subtotalAmt - this.pointsAmt - this.paymentAmt;
            this.amountDue = this.subtotalAmt - this.pointsAmt;
            if (this.balance <= 0M)
            {
                this.balance = 0M;
                this.status = "PAID";
            }
            else
                this.status = "UNPAID";
            this.note = this.txtNote.Text;
            this.driverName = this.txtDriver.Text;
            if (this.changeAmt < 0M)
                this.changeAmt = 0M;
            this.lblPayment.Text = this.paymentAmt.ToString("N");
            this.lblChange.Text = this.changeAmt.ToString("N");
            this.lblBalance.Text = this.balance.ToString("N");
            this.lblReward.Text = this.pointsAmt.ToString("N");
            this.lblSubtotal.Text = this.subtotalAmt.ToString("N");
            this.lblAmountDue.Text = this.amountDue.ToString("N");
            this.txtStatus.Text = this.status;
        }

        public Decimal GetCustomerPoints()
        {
            Decimal customerPoints = 0M;
            if (this.cboCustomer.SelectedValue != null)
            {
                int customerid;
                if (int.TryParse(this.cboCustomer.SelectedValue.ToString(), out customerid))
                {
                    customer customer = this._unitOfWork.Customers.Find(x => x.customerid == customerid).FirstOrDefault();
                    if (customer != null)
                        customerPoints = Convert.ToDecimal((object)customer.loyaltypoints);
                }
            }
            return customerPoints;
        }

        private void txtEnterAmt_TextChanged(object sender, EventArgs e)
        {
            this.CalculateChange();
        }

        private void InpNum(string num)
        {
            this.txtEnterAmt.Text += num;
            this.txtEnterAmt.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtEnterAmt.Clear();
            this.txtEnterAmt.Focus();
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string text = this.txtEnterAmt.Text;
            this.txtEnterAmt.Text = text.Remove(text.Length - 1);
            this.txtEnterAmt.Focus();
        }

        public void Confirm()
        {
            PaymentModel paymentModel = new PaymentModel();
            paymentModel.paymenttype = Convert.ToString(this.cboPaymentType.SelectedItem);
            paymentModel.customerid = Convert.ToInt32(this.cboCustomer.SelectedValue);
            paymentModel.tablenm = Convert.ToString(this.tableNo);
            paymentModel.amountDue = this.amountDue;
            paymentModel.paymentAmt = this.paymentAmt;
            paymentModel.changeAmt = this.changeAmt;
            paymentModel.balance = this.balance;
            paymentModel.pointsRedeem = this.pointsRedeem;
            paymentModel.pointsAmt = this.pointsAmt;
            paymentModel.note = this.note;
            paymentModel.driverName = this.driverName;
            paymentModel.status = this.status;
            paymentModel.invoicetype = Convert.ToString(this.cboType.SelectedItem);
            this.frmcash.ConfirmPayment(paymentModel);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.lblWarning.ForeColor == Color.Red)
                this.lblWarning.ForeColor = Color.White;
            else
                this.lblWarning.ForeColor = Color.Red;
        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblAvailablePoints.Text = this.GetCustomerPoints().ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            this.HitEnter();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.txtIdNo.Clear();
            this.cboCustomer.SelectedValue = (object)1;
        }

        private void txtPointsRedeem_TextChanged(object sender, EventArgs e)
        {
            this.CalculateChange();
        }
    }
}
