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
using POS_88.Properties;
using System.Collections;

namespace POS_88.Views
{
    public partial class CustomerDisplayView : Form
    {
        private CashierView frmCash;
        public Decimal totalrcp;
        private UnitOfWorkPOS _unitOfWork;

        public CustomerDisplayView()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
        }

        public CustomerDisplayView(CashierView frm)
        {
            this.InitializeComponent();
            this.frmCash = frm;
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
        }

        public void Refresh(
            string refreshType,
            string itemcode,
            string description,
            Decimal price,
            Decimal qty,
            Decimal total,
            string pricetype)
        {
            switch (refreshType)
            {
            case "ADD":
                this.dataGrid.Rows.Add((object) itemcode, (object) description, (object) price, (object) qty, (object) total, (object) pricetype);
                this.dataGrid.Rows[this.dataGrid.Rows.Count - 1].Selected = true;
                break;
            case "UPDATE":
                foreach (DataGridViewRow row in (IEnumerable) this.dataGrid.Rows)
                {
                if (Convert.ToString(row.Cells["ITEMCODE"].Value) == itemcode && Convert.ToString(row.Cells["TYPEOFPRICE"].Value) == pricetype)
                {
                    if (description != "")
                    row.Cells["DESCRIPTION"].Value = (object) description;
                    row.Cells["PRICE"].Value = (object) price;
                    row.Cells["QUANTITY"].Value = (object) qty;
                    row.Cells["TOTAL"].Value = (object) total;
                    row.Selected = true;
                }
                }
                break;
            }
        }

        public void RemoveItem(string itemcode, string pricetype)
        {
            for (int index = 0; index < this.dataGrid.Rows.Count; ++index)
            {
                if (string.Equals(this.dataGrid[0, index].Value as string, itemcode) && string.Equals(this.dataGrid[5, index].Value as string, pricetype))
                {
                    this.dataGrid.Rows.RemoveAt(index);
                    --index;
                }
            }
        }

        public void ClearAll()
        {
            this.dataGrid.Rows.Clear();
            this.lblPayment.Text = "₱ 0.00";
            this.lblChange.Text = "₱ 0.00";
        }

        public void RefreshTotal(Decimal subtotal, Decimal discount, Decimal total)
        {
            this.lblSubtotal.Text = "₱ " + subtotal.ToString("N");
            this.lblDiscount.Text = "₱ " + discount.ToString("N");
            this.lblTotal.Text = "₱ " + total.ToString("N");
        }

        public void RefreshPayment(Decimal payment, Decimal changeamt)
        {
            this.lblPayment.Text = "₱ " + payment.ToString("N");
            this.lblChange.Text = "₱ " + changeamt.ToString("N");
        }

        private void CustomerDisplayView_Load(object sender, EventArgs e)
        {
            setting setting = this._unitOfWork.Setting.Find(xy => xy.settingsid == 1).FirstOrDefault<setting>();
            this.lblStoreNm.Text = setting.storename;
            this.lblAddress.Text = setting.address;
            this.dataGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGrid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGrid.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGrid.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
