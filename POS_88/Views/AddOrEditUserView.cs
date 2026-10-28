using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace POS_88.Views
{
    public partial class AddOrEditUserView : Form, IAddOrEditUserView
    {
        private bool isAdd { get; set; }
        public AddOrEditUserView()
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


        public string ConfirmPassword
        {
            get
            {
                return bunifuTxtConfirmPass.Text;
            }

            set
            {
                bunifuTxtConfirmPass.Text = value;
            }
        }

        public string FName
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

        public string Password
        {
            get
            {
                return bunifuTxtPass.Text;
            }

            set
            {
                bunifuTxtPass.Text = value;
            }
        }

        public string UserName
        {
            get
            {
                return bunifuTxtUsername.Text;
            }

            set
            {
                bunifuTxtUsername.Text = value;
            }
        }

        public string UserType
        {
            get { return cBoxUserType.Text; } set { cBoxUserType.Text = value; }
        }

        public event EventHandler ClearEvent;
        public event EventHandler SaveEvent;

        public void ClearFields()
        {
            ConfirmPassword = string.Empty;
            Password = string.Empty;
            if (isAdd)
                UserName = string.Empty;
            UserType = string.Empty;
            FName = string.Empty;
            
            this.lblUsernameReq.Visible = false;
            this.lblNameReq.Visible = false;
            this.lblUserTypeReq.Visible = false;
            this.lblPassReq.Visible = false;
            this.lblConfirmPassReq.Visible = false;
            this.errorProvider.SetError((Control)this.bunifuTxtUsername, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
            this.errorProvider.SetError((Control)this.cBoxUserType, "");
            this.errorProvider.SetError((Control)this.bunifuTxtPass, "");
            this.errorProvider.SetError((Control)this.bunifuTxtConfirmPass, "");
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit User" : "Add New User";
            btnSave.Text = isEditMode ? "Update" : "Save";
            if (isEditMode)
            {
                this.bunifuTxtUsername.Enabled = false;
                this.bunifuTxtName.Focus();
                isAdd = false;
            }
            else
            {
                isAdd = true;
            }

        }


        private bool ValidateEntries()
        {
            bool flag = true;
            this.errorProvider.SetError((Control)this.bunifuTxtUsername, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
            this.errorProvider.SetError((Control)this.cBoxUserType, "");
            this.errorProvider.SetError((Control)this.bunifuTxtPass, "");
            this.errorProvider.SetError((Control)this.bunifuTxtConfirmPass, "");
            if (this.bunifuTxtUsername.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtUsername, "This field is required");
                this.lblUsernameReq.Visible = true;
            }
            else
                this.lblUsernameReq.Visible = false;

            if (this.bunifuTxtName.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtName, "This field is required");
                this.lblNameReq.Visible = true;
            }
            else
                this.lblNameReq.Visible = false;

            if (this.cBoxUserType.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.cBoxUserType, "This field is required");
                this.lblUserTypeReq.Visible = true;
            }
            else
                this.lblUserTypeReq.Visible = false;

            if (this.bunifuTxtPass.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtPass, "This field is required");
                this.lblPassReq.Visible = true;
            }
            else
                this.lblPassReq.Visible = false;

            if (this.bunifuTxtConfirmPass.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtConfirmPass, "This field is required");
                this.lblConfirmPassReq.Visible = true;
            }
            else
                this.lblConfirmPassReq.Visible = false;

            if (this.bunifuTxtPass.Text != this.bunifuTxtConfirmPass.Text)
            {
                flag = false;
                int num = (int)MessageBox.Show("Password Confirmation Mismatch!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return flag;
        }

    }
}
