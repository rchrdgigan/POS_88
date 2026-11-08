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
    public partial class AdjustQtyView : Form
    {
        private Decimal _width;
        private Decimal _height;
        private string _itemcode;
        private Decimal _qty;
        private Decimal _total;
        private CashierView frmCash;
        private UnitOfWorkPOS _unitOfWork;
        private string _priceType;

        public AdjustQtyView()
        {
            InitializeComponent();
        }

        public AdjustQtyView(
            string id,
            Decimal qty,
            Decimal total,
            CashierView frm,
            string priceType,
            Decimal getwidth,
            Decimal getheight,
            string note)
        {
            this.InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._itemcode = id;
            this._qty = qty;
            this.frmCash = frm;
            this._priceType = priceType;
            this._width = getwidth;
            this._height = getheight;
            this._total = total;
            this.txtNote.Text = note;
            this.txtQty.Focus();

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
            iconButton1.Click += delegate { this.Close(); };
        }


        private void AdjustQtyView_Load(object sender, EventArgs e)
        {
            this.GetData(this._itemcode);
            this.txtQty.Focus();
        }

        private void GetData(string id)
        {
            inventory inventory = this._unitOfWork.Inventory.Find(x => x.itemcode == id).FirstOrDefault();
            this.txtItemCode.Text = inventory.itemcode;
            this.txtName.Text = inventory.itemname;
            if (this._priceType == "RETAIL")
                this.txtPrice.Text = Convert.ToString((object)inventory.price);
            else if (this._priceType == "PROMO")
                this.txtPrice.Text = Convert.ToString((object)inventory.promoprice);
            else
                this.txtPrice.Text = Convert.ToString((object)inventory.wholesaleprice);
            this.txtQty.Text = Convert.ToString(this._qty);
            this.txttotal.Text = this._total.ToString("N");
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            Decimal result;
            if (keyData == Keys.Return && Decimal.TryParse(this.txtQty.Text, out result) && result > 0M)
            {
                Decimal price = Convert.ToDecimal(this.txtPrice.Text);
                Decimal total = Convert.ToDecimal(this.txttotal.Text);
                this.EnterKey(result, price, total);
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EnterKey(Decimal newqty, Decimal price, Decimal total)
        {
            this.frmCash.AdjustQty(this._itemcode, newqty, price, total, this._priceType, this.txtNote.Text, this._width, this._height);
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            Decimal result;
            if (!Decimal.TryParse(this.txtQty.Text, out result) || !(result > 0M))
                return;
            Decimal price = Convert.ToDecimal(this.txtPrice.Text);
            Decimal total = Convert.ToDecimal(this.txttotal.Text);
            this.EnterKey(result, price, total);
            this.Close();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            Decimal result = 0M;
            if (!Decimal.TryParse(this.txtQty.Text, out result))
                return;
            this.txttotal.Text = (Convert.ToDecimal(this.txtPrice.Text) * result).ToString("N");
        }

        private void btnMinus_Click(object sender, EventArgs e) => this.AdjustQty(-1);

        private void AdjustQty(int num)
        {
            if (this.txtQty.Text != "")
            {
                Decimal result;
                Decimal num1;
                if (Decimal.TryParse(this.txtQty.Text, out result))
                {
                    num1 = result + (Decimal)num;
                    if (num1 < 0M)
                        num1 = 0M;
                }
                else
                    num1 = 0M;
                this.txtQty.Text = num1.ToString();
            }
            this.txtQty.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e) => this.AdjustQty(1);

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string str = this.txtQty.Text;
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            this.txtQty.Text = str;
            this.txtQty.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtQty.Clear();
            this.txtQty.Focus();
        }
        private void InpNum(string num)
        {
            this.txtQty.Text += num;
            this.txtQty.Focus();
        }

    }
}
