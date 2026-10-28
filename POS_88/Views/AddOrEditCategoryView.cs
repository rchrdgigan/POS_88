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
    public partial class AddOrEditCategoryView : Form, IAddOrEditCategoryView
    {
        private bool isAdd { get; set; }
        public AddOrEditCategoryView()
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

        public string CategoryId
        {
            get
            {
                return bunifuTxtCatID.Text;
            }

            set
            {
                bunifuTxtCatID.Text = value;
            }
        }

        public string CatName
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
                CategoryId = string.Empty;
            CatName = string.Empty;

            this.lblCatIDReq.Visible = false;
            this.lblNameReq.Visible = false;
            this.errorProvider.SetError((Control)this.bunifuTxtCatID, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Category" : "Add New Category";
            btnSave.Text = isEditMode ? "Update" : "Save";
            if (isEditMode)
            {
                this.bunifuTxtCatID.Enabled = false;
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
            this.errorProvider.SetError((Control)this.bunifuTxtCatID, "");
            this.errorProvider.SetError((Control)this.bunifuTxtName, "");
            if (this.bunifuTxtCatID.Text == "")
            {
                flag = false;
                this.errorProvider.SetError((Control)this.bunifuTxtCatID, "This field is required");
                this.lblCatIDReq.Visible = true;
            }
            else
                this.lblCatIDReq.Visible = false;

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
    }
}
