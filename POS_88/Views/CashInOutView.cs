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
    public partial class CashInOutView : Form
    {
        private int _zNo;
        private string _userNm;
        private POSImplementation _posImplement;
        private UnitOfWorkPOS _unitOfWorkPOS;
        private CashierCashCountView _frmCash;

        public CashInOutView()
        {
            InitializeComponent();
        }

        public CashInOutView(int zNo, string userNm, CashierCashCountView frm)
        {
            this.InitializeComponent();
            this._zNo = zNo;
            this._userNm = userNm;
            this._posImplement = new POSImplementation();
            this._unitOfWorkPOS = new UnitOfWorkPOS(new POSDBEntities());
            this._frmCash = frm;
        }

        private void CashInOutView_Load(object sender, EventArgs e)
        {
            this.cboType.SelectedIndex = 0;
            btnClose.Click += delegate { this.Close(); };
            btnSave.Click += delegate {
                EnterOrSave();
            };
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
           if (keyData == Keys.Return)
           {
                EnterOrSave();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EnterOrSave()
        {
            if (MessageBox.Show("Do you want to save cash in/out?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                this._unitOfWorkPOS.CashInOut.Add(new cashinout()
                {
                    cashType = this.cboType.Text,
                    amount = new Decimal?(Convert.ToDecimal(this.txtEnterAmt.Text)),
                    note = this.txtNote.Text,
                    createdBy = this._userNm,
                    ZReadNo = new int?(this._zNo),
                    createdDt = new DateTime?(DateTime.Now)
                });
                this._unitOfWorkPOS.Complete();
                int num = (int)MessageBox.Show("Cash in/out saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.cboType.SelectedItem = (object)0;
                this._frmCash.LoadCashInOut();
                this.txtEnterAmt.Clear();
                this.txtNote.Clear();
            }
            catch (Exception ex)
            {

                int num = (int)MessageBox.Show("Cash in/out saving failed:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
