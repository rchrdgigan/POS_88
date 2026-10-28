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
    public partial class AddOrEditCustomersView : Form, IAddOrEditCustomersView
    {
        public AddOrEditCustomersView()
        {
            InitializeComponent();
            ApplyTheme();

            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
            btnSave.Click += (sender, args) =>
            {
                SaveEvent?.Invoke(this, EventArgs.Empty);
            };
            btnClearFilter.Click += (sender, args) => ClearEvent?.Invoke(this, EventArgs.Empty);
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

        public string CAddress
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

        public string CName
        {
            get
            {
                return txtName.Text;
            }

            set
            {
                txtName.Text = value;
            }
        }

        public string ContactNo
        {
            get
            {
                return txtContactNo.Text;
            }

            set
            {
                txtContactNo.Text = value;
            }
        }

        public decimal CreditLimit
        {
            get { return numericCreditLimit.Value; }
            set { numericCreditLimit.Value = value; }
        }

        public string CustomerID
        {
            get
            {
                return txtCustomerID.Text;
            }

            set
            {
                txtCustomerID.Text = value;
            }
        }

        public string IDNo
        {
            get
            {
                return txtIDNo.Text;
            }

            set
            {
                txtIDNo.Text = value;
            }
        }

        public decimal LoyaltyPoints
        {
            get
            {
                return numericLoyaltyPoints.Value;
            }

            set
            {
                numericLoyaltyPoints.Value = value;
            }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SaveEvent;

        public void ClearFields()
        {
            CAddress = string.Empty;
            CName = string.Empty;
            ContactNo = string.Empty;
            IDNo = string.Empty;
            LoyaltyPoints = Convert.ToDecimal("0.00");
            CreditLimit = Convert.ToDecimal("0.00");
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Customer" : "Add New Customer";
            btnSave.Text = isEditMode ? "Update" : "Save";
            if(!isEditMode)
            {
                CustomerID = "Auto Generate...";
            }
        }
    }
}
