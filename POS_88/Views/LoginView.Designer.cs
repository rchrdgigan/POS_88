namespace POS_88.Views
{
    partial class LoginView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblModalTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.bunifuTxtUser = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.bunifuTxtPass = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUsernameRequired = new System.Windows.Forms.Label();
            this.lblPasswordRequired = new System.Windows.Forms.Label();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelButton = new System.Windows.Forms.Panel();
            this.bunifuCards2 = new Bunifu.Framework.UI.BunifuCards();
            this.bunifuCards3 = new Bunifu.Framework.UI.BunifuCards();
            this.panelPass = new System.Windows.Forms.Panel();
            this.panelUser = new System.Windows.Forms.Panel();
            this.bunifuCards1 = new Bunifu.Framework.UI.BunifuCards();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.bunifuCards2.SuspendLayout();
            this.bunifuCards3.SuspendLayout();
            this.panelPass.SuspendLayout();
            this.panelUser.SuspendLayout();
            this.bunifuCards1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblModalTitle);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 50);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // lblModalTitle
            // 
            this.lblModalTitle.AutoSize = true;
            this.lblModalTitle.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalTitle.ForeColor = System.Drawing.Color.White;
            this.lblModalTitle.Location = new System.Drawing.Point(44, 16);
            this.lblModalTitle.Name = "lblModalTitle";
            this.lblModalTitle.Size = new System.Drawing.Size(125, 23);
            this.lblModalTitle.TabIndex = 5;
            this.lblModalTitle.Text = "Admin Login";
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMinimize.Location = new System.Drawing.Point(211, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(50, 50);
            this.btnMinimize.TabIndex = 8;
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
            this.btnClose.Location = new System.Drawing.Point(261, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 50);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // bunifuTxtUser
            // 
            this.bunifuTxtUser.BackColor = System.Drawing.Color.White;
            this.bunifuTxtUser.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtUser.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuTxtUser.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtUser.HintText = "Enter username";
            this.bunifuTxtUser.isPassword = false;
            this.bunifuTxtUser.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtUser.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtUser.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtUser.LineThickness = 4;
            this.bunifuTxtUser.Location = new System.Drawing.Point(0, 22);
            this.bunifuTxtUser.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuTxtUser.Name = "bunifuTxtUser";
            this.bunifuTxtUser.Size = new System.Drawing.Size(270, 43);
            this.bunifuTxtUser.TabIndex = 4;
            this.bunifuTxtUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // bunifuTxtPass
            // 
            this.bunifuTxtPass.BackColor = System.Drawing.Color.White;
            this.bunifuTxtPass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bunifuTxtPass.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuTxtPass.ForeColor = System.Drawing.Color.Black;
            this.bunifuTxtPass.HintForeColor = System.Drawing.Color.Empty;
            this.bunifuTxtPass.HintText = "";
            this.bunifuTxtPass.isPassword = true;
            this.bunifuTxtPass.LineFocusedColor = System.Drawing.Color.Blue;
            this.bunifuTxtPass.LineIdleColor = System.Drawing.Color.Gray;
            this.bunifuTxtPass.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.bunifuTxtPass.LineThickness = 4;
            this.bunifuTxtPass.Location = new System.Drawing.Point(0, 25);
            this.bunifuTxtPass.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuTxtPass.Name = "bunifuTxtPass";
            this.bunifuTxtPass.Size = new System.Drawing.Size(270, 43);
            this.bunifuTxtPass.TabIndex = 5;
            this.bunifuTxtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // lblUsernameRequired
            // 
            this.lblUsernameRequired.AutoSize = true;
            this.lblUsernameRequired.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsernameRequired.ForeColor = System.Drawing.Color.Red;
            this.lblUsernameRequired.Location = new System.Drawing.Point(72, 70);
            this.lblUsernameRequired.Name = "lblUsernameRequired";
            this.lblUsernameRequired.Size = new System.Drawing.Size(119, 16);
            this.lblUsernameRequired.TabIndex = 8;
            this.lblUsernameRequired.Text = "Username is required";
            // 
            // lblPasswordRequired
            // 
            this.lblPasswordRequired.AutoSize = true;
            this.lblPasswordRequired.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordRequired.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordRequired.Location = new System.Drawing.Point(75, 73);
            this.lblPasswordRequired.Name = "lblPasswordRequired";
            this.lblPasswordRequired.Size = new System.Drawing.Size(116, 16);
            this.lblPasswordRequired.TabIndex = 9;
            this.lblPasswordRequired.Text = "Password is required";
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearFilter.FlatAppearance.BorderSize = 0;
            this.btnClearFilter.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearFilter.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearFilter.ForeColor = System.Drawing.Color.White;
            this.btnClearFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFilter.Image")));
            this.btnClearFilter.Location = new System.Drawing.Point(-8, -3);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(130, 50);
            this.btnClearFilter.TabIndex = 6;
            this.btnClearFilter.Text = "Clear";
            this.btnClearFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.BackColor = System.Drawing.Color.SeaGreen;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaGreen;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(0, -3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(130, 50);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "  Login";
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panelButton);
            this.panel2.Controls.Add(this.panelPass);
            this.panel2.Controls.Add(this.panelUser);
            this.panel2.Location = new System.Drawing.Point(15, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(270, 252);
            this.panel2.TabIndex = 22;
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.bunifuCards2);
            this.panelButton.Controls.Add(this.bunifuCards3);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButton.Location = new System.Drawing.Point(0, 204);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(270, 45);
            this.panelButton.TabIndex = 26;
            // 
            // bunifuCards2
            // 
            this.bunifuCards2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuCards2.BorderRadius = 20;
            this.bunifuCards2.BottomSahddow = true;
            this.bunifuCards2.color = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuCards2.Controls.Add(this.btnClearFilter);
            this.bunifuCards2.Dock = System.Windows.Forms.DockStyle.Left;
            this.bunifuCards2.LeftSahddow = false;
            this.bunifuCards2.Location = new System.Drawing.Point(0, 0);
            this.bunifuCards2.Name = "bunifuCards2";
            this.bunifuCards2.RightSahddow = true;
            this.bunifuCards2.ShadowDepth = 20;
            this.bunifuCards2.Size = new System.Drawing.Size(122, 45);
            this.bunifuCards2.TabIndex = 24;
            // 
            // bunifuCards3
            // 
            this.bunifuCards3.BackColor = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.BorderRadius = 20;
            this.bunifuCards3.BottomSahddow = true;
            this.bunifuCards3.color = System.Drawing.Color.SeaGreen;
            this.bunifuCards3.Controls.Add(this.btnLogin);
            this.bunifuCards3.Dock = System.Windows.Forms.DockStyle.Right;
            this.bunifuCards3.LeftSahddow = false;
            this.bunifuCards3.Location = new System.Drawing.Point(140, 0);
            this.bunifuCards3.Name = "bunifuCards3";
            this.bunifuCards3.RightSahddow = true;
            this.bunifuCards3.ShadowDepth = 20;
            this.bunifuCards3.Size = new System.Drawing.Size(130, 45);
            this.bunifuCards3.TabIndex = 25;
            // 
            // panelPass
            // 
            this.panelPass.Controls.Add(this.lblPasswordRequired);
            this.panelPass.Controls.Add(this.label2);
            this.panelPass.Controls.Add(this.bunifuTxtPass);
            this.panelPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPass.Location = new System.Drawing.Point(0, 99);
            this.panelPass.Name = "panelPass";
            this.panelPass.Size = new System.Drawing.Size(270, 105);
            this.panelPass.TabIndex = 21;
            // 
            // panelUser
            // 
            this.panelUser.Controls.Add(this.label1);
            this.panelUser.Controls.Add(this.bunifuTxtUser);
            this.panelUser.Controls.Add(this.lblUsernameRequired);
            this.panelUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUser.Location = new System.Drawing.Point(0, 0);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(270, 99);
            this.panelUser.TabIndex = 20;
            // 
            // bunifuCards1
            // 
            this.bunifuCards1.BackColor = System.Drawing.Color.White;
            this.bunifuCards1.BorderRadius = 50;
            this.bunifuCards1.BottomSahddow = true;
            this.bunifuCards1.color = System.Drawing.Color.White;
            this.bunifuCards1.Controls.Add(this.panel2);
            this.bunifuCards1.LeftSahddow = false;
            this.bunifuCards1.Location = new System.Drawing.Point(5, 56);
            this.bunifuCards1.Name = "bunifuCards1";
            this.bunifuCards1.RightSahddow = true;
            this.bunifuCards1.ShadowDepth = 20;
            this.bunifuCards1.Size = new System.Drawing.Size(300, 279);
            this.bunifuCards1.TabIndex = 23;
            // 
            // LoginView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(227)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(311, 339);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bunifuCards1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panelButton.ResumeLayout(false);
            this.bunifuCards2.ResumeLayout(false);
            this.bunifuCards3.ResumeLayout(false);
            this.panelPass.ResumeLayout(false);
            this.panelPass.PerformLayout();
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            this.bunifuCards1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblModalTitle;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtUser;
        private Bunifu.Framework.UI.BunifuMaterialTextbox bunifuTxtPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUsernameRequired;
        private System.Windows.Forms.Label lblPasswordRequired;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.Framework.UI.BunifuCards bunifuCards1;
        private Bunifu.Framework.UI.BunifuCards bunifuCards2;
        private Bunifu.Framework.UI.BunifuCards bunifuCards3;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Panel panelPass;
        private System.Windows.Forms.Panel panelUser;
    }
}