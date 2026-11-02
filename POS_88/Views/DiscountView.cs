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
    public partial class DiscountView : Form
    {

        private CashierView frmCash;
        private string _itemcode;
        private string _pricetype;

        public DiscountView()
        {
            InitializeComponent();
            this.txtEnterAmt.Focus();
        }

        public DiscountView(CashierView frm, string id, string pricetype)
        {
            this.InitializeComponent();
            this.txtEnterAmt.Focus();
            this.frmCash = frm;
            this._itemcode = id;
            this._pricetype = pricetype;
            this.txtEnterAmt.Focus();

            btnSave.Click += btnEnter_Click;
            btnClose.Click += btnCancel_Click;
            btn0.Click += btn0_Click;
            btn1.Click += btn1_Click;
            btn2.Click += btn2_Click;
            btn3.Click += btn3_Click;
            btn4.Click += btn4_Click;
            btn5.Click += btn5_Click;
            btn6.Click += btn6_Click;
            btn7.Click += btn7_Click;
            btn8.Click += btn8_Click;
            btn9.Click += btn9_Click;
            btnBackSpace.Click += btnBackSpace_Click;
        }

        private void PopulateCombos()
        {
            this.cboDiscount.SelectedIndex = 0;
            this.cboApply.SelectedIndex = 0;
            this.txtEnterAmt.Focus();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            decimal _txtEnterAmt;
            if (!Decimal.TryParse(this.txtEnterAmt.Text, out _txtEnterAmt))
            {
                MessageBox.Show("Please enter a valid numeric discount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                
            this.EnterKey();
            this.Close();
        }

        private void EnterKey()
        {
            string discountNm = "";
            if (Convert.ToDecimal(this.txtEnterAmt.Text) > 0M)
                discountNm = this.cboDiscount.Text;
            this.frmCash.ApplyDiscount(this._itemcode, this.cboDiscount.Text, this.cboApply.Text, Convert.ToDecimal(this.txtEnterAmt.Text), this._pricetype, discountNm);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            decimal _txtEnterAmt;

            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == Keys.Return && Decimal.TryParse(this.txtEnterAmt.Text, out _txtEnterAmt))
            {
                this.EnterKey();
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DiscountView_Load(object sender, EventArgs e)
        {
            this.PopulateCombos();
        }

        private void InpNum(string num)
        {
            this.txtEnterAmt.Text += num;
            this.txtEnterAmt.Focus();
        }

        private void btn0_Click(object sender, EventArgs e) => this.InpNum(this.btn0.Text);

        private void btnDot_Click(object sender, EventArgs e) => this.InpNum(this.btnDot.Text);

        private void btn1_Click(object sender, EventArgs e) => this.InpNum(this.btn1.Text);

        private void btn2_Click(object sender, EventArgs e) => this.InpNum(this.btn2.Text);

        private void btn3_Click(object sender, EventArgs e) => this.InpNum(this.btn3.Text);

        private void btn6_Click(object sender, EventArgs e) => this.InpNum(this.btn6.Text);

        private void btn5_Click(object sender, EventArgs e) => this.InpNum(this.btn5.Text);

        private void btn4_Click(object sender, EventArgs e) => this.InpNum(this.btn4.Text);

        private void btn9_Click(object sender, EventArgs e) => this.InpNum(this.btn9.Text);

        private void btn8_Click(object sender, EventArgs e) => this.InpNum(this.btn8.Text);

        private void btn7_Click(object sender, EventArgs e) => this.InpNum(this.btn7.Text);

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.txtEnterAmt.Text;
                this.txtEnterAmt.Text = text.Remove(text.Length - 1);
                this.txtEnterAmt.Focus();
            }
            catch (Exception)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void cboDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDiscount.Text == "PWD(%)" || this.cboDiscount.Text == "SENIOR CITIZEN(%)")
                this.txtEnterAmt.Text = "20";
            else if (this.cboDiscount.Text == "LOYALTY CARD(%)")
                this.txtEnterAmt.Text = "10";
            else
                this.txtEnterAmt.Text = "";
            this.txtEnterAmt.Focus();
        }

        private void cboApply_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtEnterAmt.Focus();
        }
    }
}
