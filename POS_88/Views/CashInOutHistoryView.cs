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
    public partial class CashInOutHistoryView : Form
    {

        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        private int _zNo;


        public CashInOutHistoryView(int zno)
        {
            InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
            this._zNo = zno;
            if (dataGridViewX1 != null)
            {
                dataGridViewX1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void CashInOutHistoryView_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = (object)this._unitOfWork.CashInOut.GetCashInOutHistory(this._zNo);
        }

    }
}
