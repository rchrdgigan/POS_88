namespace POS_88.Views
{
    partial class SalesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnVoid = new System.Windows.Forms.Button();
            this.btnCompleted = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPurchaseItem = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchParam = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBoxStaff = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBoxTypeSales = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cBoxStatus = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.lblAmountPaid = new System.Windows.Forms.Label();
            this.lblGrossProfit = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewX2 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnExportExcel);
            this.panel1.Controls.Add(this.btnPayment);
            this.panel1.Controls.Add(this.btnReprint);
            this.panel1.Controls.Add(this.btnVoid);
            this.panel1.Controls.Add(this.btnCompleted);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnPurchaseItem);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1608, 91);
            this.panel1.TabIndex = 14;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(227)))), ((int)(((byte)(230)))));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.BackColor = System.Drawing.Color.SeaGreen;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExportExcel.Image")));
            this.btnExportExcel.Location = new System.Drawing.Point(1494, 8);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(104, 60);
            this.btnExportExcel.TabIndex = 6;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // btnPayment
            // 
            this.btnPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayment.BackColor = System.Drawing.Color.SeaGreen;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Image = ((System.Drawing.Image)(resources.GetObject("btnPayment.Image")));
            this.btnPayment.Location = new System.Drawing.Point(1410, 8);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(78, 60);
            this.btnPayment.TabIndex = 5;
            this.btnPayment.Text = "Payment";
            this.btnPayment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPayment.UseVisualStyleBackColor = false;
            // 
            // btnReprint
            // 
            this.btnReprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReprint.BackColor = System.Drawing.Color.Navy;
            this.btnReprint.FlatAppearance.BorderSize = 0;
            this.btnReprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprint.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprint.ForeColor = System.Drawing.Color.White;
            this.btnReprint.Image = ((System.Drawing.Image)(resources.GetObject("btnReprint.Image")));
            this.btnReprint.Location = new System.Drawing.Point(1115, 8);
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.Size = new System.Drawing.Size(108, 60);
            this.btnReprint.TabIndex = 2;
            this.btnReprint.Text = "Re-Print Receipt";
            this.btnReprint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReprint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnReprint.UseVisualStyleBackColor = false;
            // 
            // btnVoid
            // 
            this.btnVoid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVoid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnVoid.FlatAppearance.BorderSize = 0;
            this.btnVoid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoid.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoid.ForeColor = System.Drawing.Color.White;
            this.btnVoid.Image = ((System.Drawing.Image)(resources.GetObject("btnVoid.Image")));
            this.btnVoid.Location = new System.Drawing.Point(1325, 8);
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size(79, 60);
            this.btnVoid.TabIndex = 7;
            this.btnVoid.Text = "Void Sales";
            this.btnVoid.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVoid.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVoid.UseVisualStyleBackColor = false;
            // 
            // btnCompleted
            // 
            this.btnCompleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompleted.BackColor = System.Drawing.Color.Navy;
            this.btnCompleted.FlatAppearance.BorderSize = 0;
            this.btnCompleted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompleted.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompleted.ForeColor = System.Drawing.Color.White;
            this.btnCompleted.Image = ((System.Drawing.Image)(resources.GetObject("btnCompleted.Image")));
            this.btnCompleted.Location = new System.Drawing.Point(1229, 8);
            this.btnCompleted.Name = "btnCompleted";
            this.btnCompleted.Size = new System.Drawing.Size(90, 60);
            this.btnCompleted.TabIndex = 4;
            this.btnCompleted.Text = "Completed";
            this.btnCompleted.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompleted.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCompleted.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Navy;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.Location = new System.Drawing.Point(1035, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(74, 60);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnPurchaseItem
            // 
            this.btnPurchaseItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPurchaseItem.BackColor = System.Drawing.Color.YellowGreen;
            this.btnPurchaseItem.FlatAppearance.BorderSize = 0;
            this.btnPurchaseItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseItem.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPurchaseItem.ForeColor = System.Drawing.Color.White;
            this.btnPurchaseItem.Image = ((System.Drawing.Image)(resources.GetObject("btnPurchaseItem.Image")));
            this.btnPurchaseItem.Location = new System.Drawing.Point(930, 8);
            this.btnPurchaseItem.Name = "btnPurchaseItem";
            this.btnPurchaseItem.Size = new System.Drawing.Size(99, 60);
            this.btnPurchaseItem.TabIndex = 0;
            this.btnPurchaseItem.Text = "Purchase Item";
            this.btnPurchaseItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPurchaseItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPurchaseItem.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(62, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(433, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Manage your sales, transactions, and item voids efficiently in one place.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(59, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 36);
            this.label6.TabIndex = 8;
            this.label6.Text = "Sales";
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearFilter.FlatAppearance.BorderSize = 0;
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFilter.Image")));
            this.btnClearFilter.Location = new System.Drawing.Point(1547, 95);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(49, 48);
            this.btnClearFilter.TabIndex = 26;
            this.btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(1492, 95);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(49, 48);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearchParam
            // 
            this.txtSearchParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchParam.Location = new System.Drawing.Point(832, 117);
            this.txtSearchParam.Multiline = true;
            this.txtSearchParam.Name = "txtSearchParam";
            this.txtSearchParam.Size = new System.Drawing.Size(654, 26);
            this.txtSearchParam.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(829, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Search Receipt No/Customer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(436, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Name of Cashier";
            // 
            // cbBoxStaff
            // 
            this.cbBoxStaff.FormattingEnabled = true;
            this.cbBoxStaff.Location = new System.Drawing.Point(439, 117);
            this.cbBoxStaff.Name = "cbBoxStaff";
            this.cbBoxStaff.Size = new System.Drawing.Size(140, 25);
            this.cbBoxStaff.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(298, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "[To]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(160, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Date of Sales [From]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Type of Sales";
            // 
            // cbBoxTypeSales
            // 
            this.cbBoxTypeSales.FormattingEnabled = true;
            this.cbBoxTypeSales.Location = new System.Drawing.Point(13, 117);
            this.cbBoxTypeSales.Name = "cbBoxTypeSales";
            this.cbBoxTypeSales.Size = new System.Drawing.Size(144, 25);
            this.cbBoxTypeSales.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(582, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "Status";
            // 
            // cBoxStatus
            // 
            this.cBoxStatus.FormattingEnabled = true;
            this.cBoxStatus.Location = new System.Drawing.Point(585, 117);
            this.cBoxStatus.Name = "cBoxStatus";
            this.cBoxStatus.Size = new System.Drawing.Size(80, 25);
            this.cBoxStatus.TabIndex = 27;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(163, 120);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(132, 22);
            this.dtpFrom.TabIndex = 29;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(301, 120);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(132, 22);
            this.dtpTo.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(668, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 16);
            this.label9.TabIndex = 32;
            this.label9.Text = "Payment Type";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(671, 117);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(155, 25);
            this.comboBox2.TabIndex = 31;
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewX1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.HighlightSelectedColumnHeaders = false;
            this.dataGridViewX1.Location = new System.Drawing.Point(15, 158);
            this.dataGridViewX1.MultiSelect = false;
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(1580, 627);
            this.dataGridViewX1.TabIndex = 33;
            // 
            // lblAmountPaid
            // 
            this.lblAmountPaid.AutoSize = true;
            this.lblAmountPaid.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAmountPaid.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountPaid.Location = new System.Drawing.Point(1005, 0);
            this.lblAmountPaid.Name = "lblAmountPaid";
            this.lblAmountPaid.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblAmountPaid.Size = new System.Drawing.Size(214, 19);
            this.lblAmountPaid.TabIndex = 38;
            this.lblAmountPaid.Text = "Amount Paid:";
            // 
            // lblGrossProfit
            // 
            this.lblGrossProfit.AutoSize = true;
            this.lblGrossProfit.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGrossProfit.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossProfit.Location = new System.Drawing.Point(1401, 0);
            this.lblGrossProfit.Name = "lblGrossProfit";
            this.lblGrossProfit.Padding = new System.Windows.Forms.Padding(100, 0, 10, 0);
            this.lblGrossProfit.Size = new System.Drawing.Size(207, 19);
            this.lblGrossProfit.TabIndex = 37;
            this.lblGrossProfit.Text = "Gross Profit: ";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalRevenue.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRevenue.Location = new System.Drawing.Point(809, 0);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblTotalRevenue.Size = new System.Drawing.Size(196, 19);
            this.lblTotalRevenue.TabIndex = 36;
            this.lblTotalRevenue.Text = "Total Sales: ";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBalance.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.Location = new System.Drawing.Point(1219, 0);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblBalance.Size = new System.Drawing.Size(182, 19);
            this.lblBalance.TabIndex = 35;
            this.lblBalance.Text = "Balance: ";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Left;
            this.label14.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label14.Size = new System.Drawing.Size(157, 28);
            this.label14.TabIndex = 34;
            this.label14.Text = "Grand Total";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewX2);
            this.panel2.Controls.Add(this.lblTotalRevenue);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.lblAmountPaid);
            this.panel2.Controls.Add(this.lblBalance);
            this.panel2.Controls.Add(this.lblGrossProfit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 801);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1608, 93);
            this.panel2.TabIndex = 40;
            // 
            // dataGridViewX2
            // 
            this.dataGridViewX2.AllowUserToAddRows = false;
            this.dataGridViewX2.AllowUserToDeleteRows = false;
            this.dataGridViewX2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX2.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewX2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX2.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewX2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX2.Location = new System.Drawing.Point(12, 31);
            this.dataGridViewX2.MultiSelect = false;
            this.dataGridViewX2.Name = "dataGridViewX2";
            this.dataGridViewX2.ReadOnly = true;
            this.dataGridViewX2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX2.Size = new System.Drawing.Size(1580, 52);
            this.dataGridViewX2.TabIndex = 41;
            // 
            // SalesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1100, 760);
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1608, 894);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cBoxStatus);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchParam);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBoxStaff);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBoxTypeSales);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SalesView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SalesView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnVoid;
        private System.Windows.Forms.Button btnCompleted;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPurchaseItem;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchParam;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbBoxStaff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBoxTypeSales;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cBoxStatus;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox2;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Label lblAmountPaid;
        private System.Windows.Forms.Label lblGrossProfit;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX2;
    }
}