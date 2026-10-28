namespace POS_88.Views
{
    partial class PrintConfirmView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintConfirmView));
            this.bunifuCards1 = new Bunifu.Framework.UI.BunifuCards();
            this.btnEscOrCancel = new System.Windows.Forms.Button();
            this.btnEnterOrPrint = new System.Windows.Forms.Button();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.label1 = new System.Windows.Forms.Label();
            this.bunifuCards1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuCards1
            // 
            this.bunifuCards1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(227)))), ((int)(((byte)(230)))));
            this.bunifuCards1.BorderRadius = 50;
            this.bunifuCards1.BottomSahddow = true;
            this.bunifuCards1.color = System.Drawing.Color.Navy;
            this.bunifuCards1.Controls.Add(this.btnEscOrCancel);
            this.bunifuCards1.Controls.Add(this.btnEnterOrPrint);
            this.bunifuCards1.Controls.Add(this.printPreviewControl1);
            this.bunifuCards1.Controls.Add(this.label1);
            this.bunifuCards1.LeftSahddow = false;
            this.bunifuCards1.Location = new System.Drawing.Point(13, 13);
            this.bunifuCards1.Name = "bunifuCards1";
            this.bunifuCards1.RightSahddow = true;
            this.bunifuCards1.ShadowDepth = 20;
            this.bunifuCards1.Size = new System.Drawing.Size(349, 594);
            this.bunifuCards1.TabIndex = 0;
            // 
            // btnEscOrCancel
            // 
            this.btnEscOrCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEscOrCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnEscOrCancel.FlatAppearance.BorderSize = 0;
            this.btnEscOrCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEscOrCancel.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEscOrCancel.ForeColor = System.Drawing.Color.White;
            this.btnEscOrCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnEscOrCancel.Image")));
            this.btnEscOrCancel.Location = new System.Drawing.Point(24, 492);
            this.btnEscOrCancel.Name = "btnEscOrCancel";
            this.btnEscOrCancel.Size = new System.Drawing.Size(91, 74);
            this.btnEscOrCancel.TabIndex = 8;
            this.btnEscOrCancel.Text = "Cancel [ESC]";
            this.btnEscOrCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEscOrCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEscOrCancel.UseVisualStyleBackColor = false;
            // 
            // btnEnterOrPrint
            // 
            this.btnEnterOrPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnterOrPrint.BackColor = System.Drawing.Color.Navy;
            this.btnEnterOrPrint.FlatAppearance.BorderSize = 0;
            this.btnEnterOrPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnterOrPrint.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterOrPrint.ForeColor = System.Drawing.Color.White;
            this.btnEnterOrPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnEnterOrPrint.Image")));
            this.btnEnterOrPrint.Location = new System.Drawing.Point(234, 492);
            this.btnEnterOrPrint.Name = "btnEnterOrPrint";
            this.btnEnterOrPrint.Size = new System.Drawing.Size(87, 74);
            this.btnEnterOrPrint.TabIndex = 4;
            this.btnEnterOrPrint.Text = "Print [Enter]";
            this.btnEnterOrPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEnterOrPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEnterOrPrint.UseVisualStyleBackColor = false;
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printPreviewControl1.Location = new System.Drawing.Point(8, 58);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(333, 417);
            this.printPreviewControl1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Print Receipt?";
            // 
            // PrintConfirmView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(373, 619);
            this.Controls.Add(this.bunifuCards1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PrintConfirmView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintConfirmView";
            this.bunifuCards1.ResumeLayout(false);
            this.bunifuCards1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCards bunifuCards1;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnterOrPrint;
        private System.Windows.Forms.Button btnEscOrCancel;
    }
}