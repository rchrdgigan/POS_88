namespace POS_88.Views
{
    partial class ReportViewerView
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
            this.myCrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // myCrystalReportViewer
            // 
            this.myCrystalReportViewer.ActiveViewIndex = -1;
            this.myCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myCrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.myCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myCrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.myCrystalReportViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.myCrystalReportViewer.Name = "myCrystalReportViewer";
            this.myCrystalReportViewer.Size = new System.Drawing.Size(1144, 786);
            this.myCrystalReportViewer.TabIndex = 0;
            this.myCrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.myCrystalReportViewer.ToolPanelWidth = 233;
            // 
            // ReportViewerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 786);
            this.Controls.Add(this.myCrystalReportViewer);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ReportViewerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer myCrystalReportViewer;
    }
}