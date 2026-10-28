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
    public partial class AddOrEditSupplierView : Form, IAddOrEditSupplierView
    {
        private bool isAdd { get; set; }
        public AddOrEditSupplierView()
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
                if (!this.ValidateEntries())
                    return;

                SaveEvent?.Invoke(this, EventArgs.Empty);
            };
            btnClear.Click += (sender, args) => ClearEvent?.Invoke(this, EventArgs.Empty);
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

        private bool ValidateEntries()
        {
            bool flag = true;
            this.errorProvider.SetError((Control)this.bunifuTxtSupplierID, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
            if (this.bunifuTxtSupplierID.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtSupplierID, "This field is required");
                this.lblSupplierIDReq.Visible = true;
            }
            else
                this.lblSupplierIDReq.Visible = false;

            if (this.bunifuTxtName.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtName, "This field is required");
                this.lblNameReq.Visible = true;
            }
            else
                this.lblNameReq.Visible = false;

            return flag;
        }

        public string SupplierAddress
        {
            get
            {
                return bunifuTxtAddress.Text;
            }

            set
            {
                bunifuTxtAddress.Text = value;
            }
        }

        public string SupplierCPNo
        {
            get
            {
                return bunifuTxtContact.Text;
            }

            set
            {
                bunifuTxtContact.Text = value;
            }
        }

        public string SupplierId
        {
            get
            {
                return bunifuTxtSupplierID.Text;
            }

            set
            {
                bunifuTxtSupplierID.Text = value;
            }
        }

        public string SupplierName
        {
            get
            {
                return bunifuTxtName.Text;
            }

            set
            {
                bunifuTxtName.Text = value;
            }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SaveEvent;

        public void ClearFields()
        {
            if (isAdd)
                SupplierId = string.Empty;
            SupplierName = string.Empty;
            SupplierAddress = string.Empty;
            SupplierCPNo = string.Empty;

            this.lblSupplierIDReq.Visible = false;
            this.lblNameReq.Visible = false;
            this.errorProvider.SetError((Control)this.bunifuTxtSupplierID, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Supplier" : "Add New Supplier";
            btnSave.Text = isEditMode ? "Update" : "Save";
            if (isEditMode)
            {
                this.bunifuTxtSupplierID.Enabled = false;
                this.bunifuTxtName.Focus();
                isAdd = false;
            }
            else
            {
                isAdd = true;
            }
        }
    }
}
