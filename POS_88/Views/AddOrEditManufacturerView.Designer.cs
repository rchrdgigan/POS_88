namespace POS_88.Views
{
    partial class AddOrEditManufacturerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOrEditManufacturerView));
            this.bunifuCards1 = new Bunifu.Framework.UI.BunifuCards();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bunifuTxtName = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.bunifuTxtManufacturerID = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.lblNameReq = new System.Windows.Forms.Label();
            this.lblManufacturerReq = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bunifuCards2 = new Bunifu.Framework.UI.BunifuCards();
            this.btnClear = new System.Windows.Forms.Button();
            this.bunifuCards3 = new Bunifu.Framework.UI.BunifuCards();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblModalTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bunifuCards1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.bunifuCards2.SuspendLayout();
            this.bunifuCards3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuCards1
            // 
            this.bunifuCards1.BackColor = System.Drawing.Color.White;
            this.bunifuCards1.BorderRadius = 20;
            this.bunifuCards1.BottomSahddow = true;
            this.bunifuCards1.color = System.Drawing.Color.White;
            this.bunifuCards1.Controls.Add(this.panel3);
            this.bunifuCards1.LeftSahddow = false;
            this.bunifuCards1.Location = new System.Drawing.Point(4, 47);
            this.bunifuCards1.Name = "bunifuCards1";
            this.bunifuCards1.RightSahddow = true;
            this.bunifuCards1.ShadowDepth = 20;
            this.bunifuCards1.Size = new System.Drawing.Size(360, 234);
            this.bunifuCards1.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.bunifuTxtName);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.bunifuTxtManufacturerID);
            this.panel3.Controls.Add(this.lblNameReq);
            this.panel3.Controls.Add(this.lblManufacturerReq);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.bunifuCards2);
            this.panel3.Controls.Add(this.bunifuCards3);
            this.panel3.Location = new System.Drawing.Point(13, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(344, 215);
            this.panel3.TabIndex = 22;
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
            this.bunifuTxtName.Location = new System.Drawing.Point(6, 103);
            this.bunifuTxtName.Name = "bunifuTxtName";
            this.bunifuTxtName.Size = new System.Drawing.Size(319, 35);
            this.bunifuTxtName.TabIndex = 35;
            this.bunifuTxtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Manufacturer ID";
            // 
            // bunifuTxtManufacturerID
            // 
            this.bunifuTxtManufacturerID.BackColor = System.Drawing.Color.White;
            this.bunifuTxtManufacturerID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtManufacturerID.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtManufacturerID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtManufacturerID.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtManufacturerID.HintText = "Enter manufacturer id";
            this.bunifuTxtManufacturerID.isPassword = false;
            this.bunifuTxtManufacturerID.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtManufacturerID.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtManufacturerID.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtManufacturerID.LineThickness = 3;
            this.bunifuTxtManufacturerID.Location = new System.Drawing.Point(6, 27);
            this.bunifuTxtManufacturerID.Name = "bunifuTxtManufacturerID";
            this.bunifuTxtManufacturerID.Size = new System.Drawing.Size(319, 35);
            this.bunifuTxtManufacturerID.TabIndex = 4;
            this.bunifuTxtManufacturerID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblNameReq
            // 
            this.lblNameReq.AutoSize = true;
            this.lblNameReq.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameReq.ForeColor = System.Drawing.Color.Red;
            this.lblNameReq.Location = new System.Drawing.Point(227, 141);
            this.lblNameReq.Name = "lblNameReq";
            this.lblNameReq.Size = new System.Drawing.Size(98, 16);
            this.lblNameReq.TabIndex = 9;
            this.lblNameReq.Text = "Name is required";
            this.lblNameReq.Visible = false;
            // 
            // lblManufacturerReq
            // 
            this.lblManufacturerReq.AutoSize = true;
            this.lblManufacturerReq.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManufacturerReq.ForeColor = System.Drawing.Color.Red;
            this.lblManufacturerReq.Location = new System.Drawing.Point(171, 65);
            this.lblManufacturerReq.Name = "lblManufacturerReq";
            this.lblManufacturerReq.Size = new System.Drawing.Size(154, 16);
            this.lblManufacturerReq.TabIndex = 8;
            this.lblManufacturerReq.Text = "Manufacturer ID is required";
            this.lblManufacturerReq.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-3, 81);
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
            this.bunifuCards2.Location = new System.Drawing.Point(5, 163);
            this.bunifuCards2.Name = "bunifuCards2";
            this.bunifuCards2.RightSahddow = true;
            this.bunifuCards2.ShadowDepth = 20;
            this.bunifuCards2.Size = new System.Drawing.Size(140, 45);
            this.bunifuCards2.TabIndex = 24;
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
            // bunifuCards3
            // 
            this.bunifuCards3.BackColor = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.BorderRadius = 20;
            this.bunifuCards3.BottomSahddow = true;
            this.bunifuCards3.color = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.Controls.Add(this.btnSave);
            this.bunifuCards3.LeftSahddow = false;
            this.bunifuCards3.Location = new System.Drawing.Point(185, 163);
            this.bunifuCards3.Name = "bunifuCards3";
            this.bunifuCards3.RightSahddow = true;
            this.bunifuCards3.ShadowDepth = 20;
            this.bunifuCards3.Size = new System.Drawing.Size(140, 45);
            this.bunifuCards3.TabIndex = 25;
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // lblModalTitle
            // 
            this.lblModalTitle.AutoSize = true;
            this.lblModalTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalTitle.ForeColor = System.Drawing.Color.White;
            this.lblModalTitle.Location = new System.Drawing.Point(12, 11);
            this.lblModalTitle.Name = "lblModalTitle";
            this.lblModalTitle.Size = new System.Drawing.Size(201, 19);
            this.lblModalTitle.TabIndex = 5;
            this.lblModalTitle.Text = "Add or Edit Manufacturer";
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
            this.panel1.TabIndex = 29;
            // 
            // AddOrEditManufacturerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 284);
            this.Controls.Add(this.bunifuCards1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AddOrEditManufacturerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOrEditManufacturerView";
            this.bunifuCards1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.bunifuCards2.ResumeLayout(false);
            this.bunifuCards3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCards bunifuCards1;
        private System.Windows.Forms.Panel panel3;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtName;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtManufacturerID;
        private System.Windows.Forms.Label lblNameReq;
        private System.Windows.Forms.Label lblManufacturerReq;
        private System.Windows.Forms.Label label2;
        private Bunifu.Framework.UI.BunifuCards bunifuCards2;
        private System.Windows.Forms.Button btnClear;
        private Bunifu.Framework.UI.BunifuCards bunifuCards3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblModalTitle;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
    }
}