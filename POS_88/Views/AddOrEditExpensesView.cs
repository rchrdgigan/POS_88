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
    public partial class AddOrEditExpensesView : Form, IAddOrEditExpensesView
    {
        public AddOrEditExpensesView()
        {
            InitializeComponent();
            ApplyTheme();
            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
            btnClearFilter.Click += (sender, args) => ClearEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (sender, args) => SaveEvent?.Invoke(this, EventArgs.Empty);
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

        public string Address
        {
            get
            {
                return txtAddress.Text;
            }

            set
            {
                txtAddress.Text = value;
            }
        }

        public string Amount
        {
            get
            {
                return txtAmount.Text;
            }

            set
            {
                txtAmount.Text = value;
            }
        }

        public string Category
        {
            get
            {
                return txtCategory.Text;
            }

            set
            {
                txtCategory.Text = value;
            }
        }

        public DateTime Date { get { return dtpDate.Value; } set { dtpDate.Value = value; } }

        public string Description
        {
            get
            {
                return txtDesc.Text;
            }

            set
            {
                txtDesc.Text = value;
            }
        }

        public string Payee
        {
            get
            {
                return txtPayee.Text;
            }

            set
            {
                txtPayee.Text = value;
            }
        }

        public string Remarks
        {
            get
            {
                return txtRemarks.Text;
            }

            set
            {
                txtRemarks.Text = value;
            }
        }

        public string TinNo
        {
            get
            {
                return txtTinNo.Text;
            }

            set
            {
                txtTinNo.Text = value;
            }
        }

        public string Vat
        {
            get
            {
                return txtVat.Text;
            }

            set
            {
                txtVat.Text = value;
            }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SaveEvent;

        public void ClearFields()
        {
            Payee = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            Vat = string.Empty;
            Amount = string.Empty;
            TinNo = string.Empty;
            Address = string.Empty;
            Remarks = string.Empty;
            Date = DateTime.Today;
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Item" : "Add New Item";
            btnSave.Text = isEditMode ? "Update" : "Save";
        }
    }
}
