namespace POS_88.Views
{
    partial class PurchaseOrderView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnEditPurchaseOrder = new System.Windows.Forms.Button();
            this.btnAddPurchaseOrder = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBoxIsReceived = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchParam = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnExportExcel);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnEditPurchaseOrder);
            this.panel1.Controls.Add(this.btnAddPurchaseOrder);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1513, 86);
            this.panel1.TabIndex = 14;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
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
            this.btnExportExcel.Location = new System.Drawing.Point(1398, 8);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(106, 60);
            this.btnExportExcel.TabIndex = 6;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExportExcel.UseVisualStyleBackColor = false;
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
            this.btnPrint.Location = new System.Drawing.Point(1315, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(77, 60);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnEditPurchaseOrder
            // 
            this.btnEditPurchaseOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditPurchaseOrder.BackColor = System.Drawing.Color.Navy;
            this.btnEditPurchaseOrder.FlatAppearance.BorderSize = 0;
            this.btnEditPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditPurchaseOrder.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditPurchaseOrder.ForeColor = System.Drawing.Color.White;
            this.btnEditPurchaseOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPurchaseOrder.Image")));
            this.btnEditPurchaseOrder.Location = new System.Drawing.Point(1219, 8);
            this.btnEditPurchaseOrder.Name = "btnEditPurchaseOrder";
            this.btnEditPurchaseOrder.Size = new System.Drawing.Size(90, 60);
            this.btnEditPurchaseOrder.TabIndex = 2;
            this.btnEditPurchaseOrder.Text = "Update/Edit";
            this.btnEditPurchaseOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditPurchaseOrder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditPurchaseOrder.UseVisualStyleBackColor = false;
            // 
            // btnAddPurchaseOrder
            // 
            this.btnAddPurchaseOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPurchaseOrder.BackColor = System.Drawing.Color.YellowGreen;
            this.btnAddPurchaseOrder.FlatAppearance.BorderSize = 0;
            this.btnAddPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPurchaseOrder.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPurchaseOrder.ForeColor = System.Drawing.Color.White;
            this.btnAddPurchaseOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPurchaseOrder.Image")));
            this.btnAddPurchaseOrder.Location = new System.Drawing.Point(1127, 8);
            this.btnAddPurchaseOrder.Name = "btnAddPurchaseOrder";
            this.btnAddPurchaseOrder.Size = new System.Drawing.Size(86, 60);
            this.btnAddPurchaseOrder.TabIndex = 0;
            this.btnAddPurchaseOrder.Text = "New Order";
            this.btnAddPurchaseOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddPurchaseOrder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddPurchaseOrder.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(62, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(545, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Create, manage, and track purchase orders to keep your inventory stocked and orga" +
    "nized.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(59, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(232, 36);
            this.label6.TabIndex = 8;
            this.label6.Text = "Purchase Order";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(194, 121);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(182, 22);
            this.dtpTo.TabIndex = 18;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(15, 121);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(173, 22);
            this.dtpFrom.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(191, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Date of Orders [To]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Date of Orders [From]";
            // 
            // cbBoxIsReceived
            // 
            this.cbBoxIsReceived.FormattingEnabled = true;
            this.cbBoxIsReceived.Location = new System.Drawing.Point(382, 118);
            this.cbBoxIsReceived.Name = "cbBoxIsReceived";
            this.cbBoxIsReceived.Size = new System.Drawing.Size(152, 25);
            this.cbBoxIsReceived.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(379, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Is Received";
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearFilter.FlatAppearance.BorderSize = 0;
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFilter.Image")));
            this.btnClearFilter.Location = new System.Drawing.Point(1450, 95);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(49, 48);
            this.btnClearFilter.TabIndex = 29;
            this.btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(1395, 95);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(49, 48);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearchParam
            // 
            this.txtSearchParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchParam.Location = new System.Drawing.Point(540, 117);
            this.txtSearchParam.Multiline = true;
            this.txtSearchParam.Name = "txtSearchParam";
            this.txtSearchParam.Size = new System.Drawing.Size(847, 26);
            this.txtSearchParam.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(537, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(193, 16);
            this.label5.TabIndex = 30;
            this.label5.Text = "Search Supplier Name/Order ID";
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
            this.dataGridViewX1.Location = new System.Drawing.Point(15, 157);
            this.dataGridViewX1.MultiSelect = false;
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(1484, 843);
            this.dataGridViewX1.TabIndex = 31;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblGrandTotal);
            this.panel2.Controls.Add(this.lblTotalOrders);
            this.panel2.Controls.Add(this.lblTotalQty);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 1006);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1513, 43);
            this.panel2.TabIndex = 41;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGrandTotal.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrandTotal.Location = new System.Drawing.Point(910, 0);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblGrandTotal.Size = new System.Drawing.Size(203, 19);
            this.lblGrandTotal.TabIndex = 38;
            this.lblGrandTotal.Text = "Grand Total:";
            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalOrders.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalOrders.Location = new System.Drawing.Point(1113, 0);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblTotalOrders.Size = new System.Drawing.Size(207, 19);
            this.lblTotalOrders.TabIndex = 35;
            this.lblTotalOrders.Text = "Total Orders: ";
            this.lblTotalOrders.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalQty.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQty.Location = new System.Drawing.Point(1320, 0);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Padding = new System.Windows.Forms.Padding(100, 0, 10, 0);
            this.lblTotalQty.Size = new System.Drawing.Size(193, 19);
            this.lblTotalQty.TabIndex = 37;
            this.lblTotalQty.Text = "Total Qty: ";
            // 
            // PurchaseOrderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1100, 760);
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1513, 1049);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchParam);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBoxIsReceived);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PurchaseOrderView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PurchasingView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnEditPurchaseOrder;
        private System.Windows.Forms.Button btnAddPurchaseOrder;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBoxIsReceived;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchParam;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblTotalOrders;
        private System.Windows.Forms.Label lblTotalQty;
    }
}