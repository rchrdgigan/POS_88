namespace POS_88.Views
{
    partial class AddOrEditSupplierView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOrEditSupplierView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblModalTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.bunifuTxtName = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.bunifuTxtSupplierID = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.bunifuCards3 = new Bunifu.Framework.UI.BunifuCards();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bunifuTxtContact = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label6 = new System.Windows.Forms.Label();
            this.bunifuTxtAddress = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNameReq = new System.Windows.Forms.Label();
            this.lblSupplierIDReq = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bunifuCards2 = new Bunifu.Framework.UI.BunifuCards();
            this.bunifuCards1 = new Bunifu.Framework.UI.BunifuCards();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.bunifuCards3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.bunifuCards2.SuspendLayout();
            this.bunifuCards1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.panel1.Controls.Add(this.lblModalTitle);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 40);
            this.panel1.TabIndex = 31;
            // 
            // lblModalTitle
            // 
            this.lblModalTitle.AutoSize = true;
            this.lblModalTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalTitle.ForeColor = System.Drawing.Color.White;
            this.lblModalTitle.Location = new System.Drawing.Point(12, 11);
            this.lblModalTitle.Name = "lblModalTitle";
            this.lblModalTitle.Size = new System.Drawing.Size(160, 19);
            this.lblModalTitle.TabIndex = 5;
            this.lblModalTitle.Text = "Add or Edit Supplier";
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMinimize.Location = new System.Drawing.Point(269, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(50, 40);
            this.btnMinimize.TabIndex = 4;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(319, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(-7, -5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(152, 53);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(-5, -5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(152, 53);
            this.btnClear.TabIndex = 20;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // bunifuTxtName
            // 
            this.bunifuTxtName.BackColor = System.Drawing.Color.White;
            this.bunifuTxtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtName.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtName.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtName.HintText = "Enter name";
            this.bunifuTxtName.isPassword = false;
            this.bunifuTxtName.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtName.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtName.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtName.LineThickness = 3;
            this.bunifuTxtName.Location = new System.Drawing.Point(5, 107);
            this.bunifuTxtName.Name = "bunifuTxtName";
            this.bunifuTxtName.Size = new System.Drawing.Size(320, 35);
            this.bunifuTxtName.TabIndex = 35;
            this.bunifuTxtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Supplier ID";
            // 
            // bunifuTxtSupplierID
            // 
            this.bunifuTxtSupplierID.BackColor = System.Drawing.Color.White;
            this.bunifuTxtSupplierID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtSupplierID.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtSupplierID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtSupplierID.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtSupplierID.HintText = "Enter supplier id";
            this.bunifuTxtSupplierID.isPassword = false;
            this.bunifuTxtSupplierID.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtSupplierID.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtSupplierID.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtSupplierID.LineThickness = 3;
            this.bunifuTxtSupplierID.Location = new System.Drawing.Point(5, 27);
            this.bunifuTxtSupplierID.Name = "bunifuTxtSupplierID";
            this.bunifuTxtSupplierID.Size = new System.Drawing.Size(320, 35);
            this.bunifuTxtSupplierID.TabIndex = 4;
            this.bunifuTxtSupplierID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // bunifuCards3
            // 
            this.bunifuCards3.BackColor = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.BorderRadius = 20;
            this.bunifuCards3.BottomSahddow = true;
            this.bunifuCards3.color = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.Controls.Add(this.btnSave);
            this.bunifuCards3.LeftSahddow = false;
            this.bunifuCards3.Location = new System.Drawing.Point(185, 339);
            this.bunifuCards3.Name = "bunifuCards3";
            this.bunifuCards3.RightSahddow = true;
            this.bunifuCards3.ShadowDepth = 20;
            this.bunifuCards3.Size = new System.Drawing.Size(140, 45);
            this.bunifuCards3.TabIndex = 25;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.bunifuTxtContact);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.bunifuTxtAddress);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.bunifuTxtName);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.bunifuTxtSupplierID);
            this.panel3.Controls.Add(this.lblNameReq);
            this.panel3.Controls.Add(this.lblSupplierIDReq);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.bunifuCards2);
            this.panel3.Controls.Add(this.bunifuCards3);
            this.panel3.Location = new System.Drawing.Point(13, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(344, 390);
            this.panel3.TabIndex = 22;
            // 
            // bunifuTxtContact
            // 
            this.bunifuTxtContact.BackColor = System.Drawing.Color.White;
            this.bunifuTxtContact.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtContact.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtContact.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtContact.HintText = "Enter contact";
            this.bunifuTxtContact.isPassword = false;
            this.bunifuTxtContact.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtContact.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtContact.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtContact.LineThickness = 3;
            this.bunifuTxtContact.Location = new System.Drawing.Point(5, 278);
            this.bunifuTxtContact.Name = "bunifuTxtContact";
            this.bunifuTxtContact.Size = new System.Drawing.Size(320, 35);
            this.bunifuTxtContact.TabIndex = 41;
            this.bunifuTxtContact.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(-3, 256);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 16);
            this.label6.TabIndex = 39;
            this.label6.Text = "Contact";
            // 
            // bunifuTxtAddress
            // 
            this.bunifuTxtAddress.BackColor = System.Drawing.Color.White;
            this.bunifuTxtAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtAddress.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtAddress.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtAddress.HintText = "Enter address";
            this.bunifuTxtAddress.isPassword = false;
            this.bunifuTxtAddress.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtAddress.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtAddress.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtAddress.LineThickness = 3;
            this.bunifuTxtAddress.Location = new System.Drawing.Point(5, 193);
            this.bunifuTxtAddress.Name = "bunifuTxtAddress";
            this.bunifuTxtAddress.Size = new System.Drawing.Size(320, 35);
            this.bunifuTxtAddress.TabIndex = 38;
            this.bunifuTxtAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(-4, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 16);
            this.label4.TabIndex = 36;
            this.label4.Text = "Address";
            // 
            // lblNameReq
            // 
            this.lblNameReq.AutoSize = true;
            this.lblNameReq.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameReq.ForeColor = System.Drawing.Color.Red;
            this.lblNameReq.Location = new System.Drawing.Point(227, 147);
            this.lblNameReq.Name = "lblNameReq";
            this.lblNameReq.Size = new System.Drawing.Size(98, 16);
            this.lblNameReq.TabIndex = 9;
            this.lblNameReq.Text = "Name is required";
            this.lblNameReq.Visible = false;
            // 
            // lblSupplierIDReq
            // 
            this.lblSupplierIDReq.AutoSize = true;
            this.lblSupplierIDReq.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierIDReq.ForeColor = System.Drawing.Color.Red;
            this.lblSupplierIDReq.Location = new System.Drawing.Point(203, 67);
            this.lblSupplierIDReq.Name = "lblSupplierIDReq";
            this.lblSupplierIDReq.Size = new System.Drawing.Size(122, 16);
            this.lblSupplierIDReq.TabIndex = 8;
            this.lblSupplierIDReq.Text = "Supplier ID is required";
            this.lblSupplierIDReq.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-3, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name";
            // 
            // bunifuCards2
            // 
            this.bunifuCards2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuCards2.BorderRadius = 20;
            this.bunifuCards2.BottomSahddow = true;
            this.bunifuCards2.color = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuCards2.Controls.Add(this.btnClear);
            this.bunifuCards2.LeftSahddow = false;
            this.bunifuCards2.Location = new System.Drawing.Point(5, 339);
            this.bunifuCards2.Name = "bunifuCards2";
            this.bunifuCards2.RightSahddow = true;
            this.bunifuCards2.ShadowDepth = 20;
            this.bunifuCards2.Size = new System.Drawing.Size(140, 45);
            this.bunifuCards2.TabIndex = 24;
            // 
            // bunifuCards1
            // 
            this.bunifuCards1.BackColor = System.Drawing.Color.White;
            this.bunifuCards1.BorderRadius = 20;
            this.bunifuCards1.BottomSahddow = true;
            this.bunifuCards1.color = System.Drawing.Color.White;
            this.bunifuCards1.Controls.Add(this.panel3);
            this.bunifuCards1.LeftSahddow = false;
            this.bunifuCards1.Location = new System.Drawing.Point(5, 47);
            this.bunifuCards1.Name = "bunifuCards1";
            this.bunifuCards1.RightSahddow = true;
            this.bunifuCards1.ShadowDepth = 20;
            this.bunifuCards1.Size = new System.Drawing.Size(360, 405);
            this.bunifuCards1.TabIndex = 30;
            // 
            // AddOrEditSupplierView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 454);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bunifuCards1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AddOrEditSupplierView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOrEditSupplierView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.bunifuCards3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.bunifuCards2.ResumeLayout(false);
            this.bunifuCards1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblModalTitle;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Bunifu.Framework.UI.BunifuCards bunifuCards1;
        private System.Windows.Forms.Panel panel3;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtContact;
        private System.Windows.Forms.Label label6;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtAddress;
        private System.Windows.Forms.Label label4;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtName;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtSupplierID;
        private System.Windows.Forms.Label lblNameReq;
        private System.Windows.Forms.Label lblSupplierIDReq;
        private System.Windows.Forms.Label label2;
        private Bunifu.Framework.UI.BunifuCards bunifuCards2;
        private System.Windows.Forms.Button btnClear;
        private Bunifu.Framework.UI.BunifuCards bunifuCards3;
        private System.Windows.Forms.Button btnSave;
    }
}