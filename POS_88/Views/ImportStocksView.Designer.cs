namespace POS_88.Views
{
    partial class ImportStocksView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportStocksView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblModalTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cBoxImportType = new System.Windows.Forms.ComboBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseExcel = new System.Windows.Forms.Button();
            this.txtSheetName = new System.Windows.Forms.ComboBox();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImportData = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.picBoxNoData = new System.Windows.Forms.PictureBox();
            this.lblNoData = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNoData)).BeginInit();
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
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 50);
            this.panel1.TabIndex = 2;
            // 
            // lblModalTitle
            // 
            this.lblModalTitle.AutoSize = true;
            this.lblModalTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalTitle.ForeColor = System.Drawing.Color.White;
            this.lblModalTitle.Location = new System.Drawing.Point(27, 19);
            this.lblModalTitle.Name = "lblModalTitle";
            this.lblModalTitle.Size = new System.Drawing.Size(148, 19);
            this.lblModalTitle.TabIndex = 5;
            this.lblModalTitle.Text = "Import Item Stocks";
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMinimize.Location = new System.Drawing.Point(981, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(51, 50);
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
            this.btnClose.Location = new System.Drawing.Point(1032, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 50);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Step 2- Browse Excel (Inventory Report)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Step 1 - Select Import Type";
            // 
            // cBoxImportType
            // 
            this.cBoxImportType.FormattingEnabled = true;
            this.cBoxImportType.Location = new System.Drawing.Point(13, 153);
            this.cBoxImportType.Name = "cBoxImportType";
            this.cBoxImportType.Size = new System.Drawing.Size(219, 25);
            this.cBoxImportType.TabIndex = 11;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(11, 267);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(221, 22);
            this.txtFilePath.TabIndex = 12;
            // 
            // btnBrowseExcel
            // 
            this.btnBrowseExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseExcel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBrowseExcel.FlatAppearance.BorderSize = 0;
            this.btnBrowseExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseExcel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseExcel.ForeColor = System.Drawing.Color.White;
            this.btnBrowseExcel.Location = new System.Drawing.Point(13, 295);
            this.btnBrowseExcel.Name = "btnBrowseExcel";
            this.btnBrowseExcel.Size = new System.Drawing.Size(219, 32);
            this.btnBrowseExcel.TabIndex = 22;
            this.btnBrowseExcel.Text = "Choose file...";
            this.btnBrowseExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseExcel.UseVisualStyleBackColor = false;
            // 
            // txtSheetName
            // 
            this.txtSheetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSheetName.Enabled = false;
            this.txtSheetName.Location = new System.Drawing.Point(11, 430);
            this.txtSheetName.Name = "txtSheetName";
            this.txtSheetName.Size = new System.Drawing.Size(221, 25);
            this.txtSheetName.TabIndex = 23;
            this.txtSheetName.Visible = false;
            // 
            // btnLoadData
            // 
            this.btnLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadData.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLoadData.FlatAppearance.BorderSize = 0;
            this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadData.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadData.ForeColor = System.Drawing.Color.White;
            this.btnLoadData.Location = new System.Drawing.Point(11, 458);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(221, 32);
            this.btnLoadData.TabIndex = 24;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadData.UseVisualStyleBackColor = false;
            this.btnLoadData.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 375);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 16);
            this.label1.TabIndex = 25;
            this.label1.Text = "Step 3 - Load Data Sheet";
            this.label1.Visible = false;
            // 
            // btnImportData
            // 
            this.btnImportData.BackColor = System.Drawing.Color.SeaGreen;
            this.btnImportData.FlatAppearance.BorderSize = 0;
            this.btnImportData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportData.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportData.ForeColor = System.Drawing.Color.White;
            this.btnImportData.Image = ((System.Drawing.Image)(resources.GetObject("btnImportData.Image")));
            this.btnImportData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportData.Location = new System.Drawing.Point(13, 654);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(219, 55);
            this.btnImportData.TabIndex = 26;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportData.UseVisualStyleBackColor = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.ForeColor = System.Drawing.SystemColors.Highlight;
            this.chkSelectAll.Location = new System.Drawing.Point(261, 66);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(81, 20);
            this.chkSelectAll.TabIndex = 27;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.Visible = false;
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewX1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.HighlightSelectedColumnHeaders = false;
            this.dataGridViewX1.Location = new System.Drawing.Point(251, 91);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.Size = new System.Drawing.Size(820, 618);
            this.dataGridViewX1.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 30);
            this.label2.TabIndex = 29;
            this.label2.Text = "Choose the type of data you want\r\nto import.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "Select your Excel file to import";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 401);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "Load the sheet from your file";
            this.label6.Visible = false;
            // 
            // picBoxNoData
            // 
            this.picBoxNoData.BackColor = System.Drawing.Color.White;
            this.picBoxNoData.Image = ((System.Drawing.Image)(resources.GetObject("picBoxNoData.Image")));
            this.picBoxNoData.Location = new System.Drawing.Point(487, 271);
            this.picBoxNoData.Name = "picBoxNoData";
            this.picBoxNoData.Size = new System.Drawing.Size(345, 259);
            this.picBoxNoData.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxNoData.TabIndex = 32;
            this.picBoxNoData.TabStop = false;
            // 
            // lblNoData
            // 
            this.lblNoData.AutoSize = true;
            this.lblNoData.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoData.ForeColor = System.Drawing.Color.Red;
            this.lblNoData.Location = new System.Drawing.Point(967, 66);
            this.lblNoData.Name = "lblNoData";
            this.lblNoData.Size = new System.Drawing.Size(104, 16);
            this.lblNoData.TabIndex = 33;
            this.lblNoData.Text = "No data loaded!";
            // 
            // ImportStocksView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(227)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(1083, 721);
            this.Controls.Add(this.lblNoData);
            this.Controls.Add(this.picBoxNoData);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnImportData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.txtSheetName);
            this.Controls.Add(this.btnBrowseExcel);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.cBoxImportType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ImportStocksView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImportStocksView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNoData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblModalTitle;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBoxImportType;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseExcel;
        private System.Windows.Forms.ComboBox txtSheetName;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picBoxNoData;
        private System.Windows.Forms.Label lblNoData;
    }
}