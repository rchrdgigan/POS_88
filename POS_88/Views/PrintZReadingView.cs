using POS_88.Database;
using POS_88.Persistence;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class PrintZReadingView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private string _storename;
        private string _address;
        private string _tinno;
        private string _contactno;
        private bool _isPrint;
        private bool _iswithLogo;
        private DateTime _dtNow;
        private DateTime _printTime;
        private spGetZReading_Result _z;

        // NOTE: recordDoc and printPreviewControl1 are declared in the Designer file.

        public PrintZReadingView()
        {
            this.InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Fuchsia;
            TransparencyKey = Color.Fuchsia;
            StartPosition = FormStartPosition.CenterParent;

            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._isPrint = false;

            btnEscOrCancel.Click += delegate { this.Close(); };
            btnEnterOrPrint.Click += delegate { this.DoPrint(); };
        }

        private void PrintZReadingView_Load(object sender, EventArgs e)
        {
            this._dtNow = DateTime.Now.Date;
            this._printTime = DateTime.Now;

            if (!this.PrepareReceipt())
                this.BeginInvoke(new Action(this.Close));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            if (keyData == Keys.Return)
            {
                this.DoPrint();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DoPrint()
        {
            // Set the flag first so Enter + button click can't print twice.
            if (this._isPrint || this.recordDoc == null)
                return;
            this._isPrint = true;

            this.recordDoc.Print();
            this.Close();
        }

        private bool PrepareReceipt()
        {
            // Load settings (null-safe)
            setting setting = this._unitOfWork.Setting.Find(xy => xy.settingsid == 1).FirstOrDefault();
            if (setting == null)
            {
                MessageBox.Show("Store settings not found.", "Z-Reading",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            this._storename = setting.storename;
            this._address = setting.address;
            this._contactno = setting.contactno;
            this._tinno = setting.tinno;
            this._iswithLogo = setting.withLogo.GetValueOrDefault();

            // Load report data ONCE (not inside the paint handler)
            this._z = this._unitOfWork.ItemSales.GetZReadingReport(this._dtNow).FirstOrDefault()
                      ?? new spGetZReading_Result();

            // Clean up any previous document
            if (this.recordDoc != null)
            {
                if (this.printPreviewControl1.Document == this.recordDoc)
                    this.printPreviewControl1.Document = null;
                this.recordDoc.PrintPage -= this.PrintReceiptPageRGM;
                this.recordDoc.Dispose();
            }

            this.recordDoc = new PrintDocument();
            this.recordDoc.DocumentName = "Z-Reading";
            this.recordDoc.PrintPage += this.PrintReceiptPageRGM;

            PrinterSettings printerSettings = new PrinterSettings();
            if (!string.IsNullOrWhiteSpace(setting.printername))
                printerSettings.PrinterName = setting.printername;
            this.recordDoc.PrinterSettings = printerSettings;

            if (this.printPreviewControl1 != null)
            {
                this.printPreviewControl1.Document = this.recordDoc;
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.UseAntiAlias = true;
                this.printPreviewControl1.Zoom = 1.5;
                this.printPreviewControl1.StartPage = 0;
                this.printPreviewControl1.InvalidatePreview();
                this.printPreviewControl1.Refresh();
            }
            return true;
        }

        private static string Fmt(decimal? value)
        {
            return (value ?? 0m).ToString("N2");
        }

        // Draws a label (left) and value (right) on one row; returns the y of the next row.
        private float DrawRow(Graphics g, Font font, Brush brush,
            StringFormat left, StringFormat right,
            string label, string value, float y, float gapAfter)
        {
            const float x = 2f;
            const float labelWidth = 95f;
            const float valueWidth = 90f;

            g.DrawString(label, font, brush, new RectangleF(x, y, labelWidth, 0f), left);
            g.DrawString(value, font, brush, new RectangleF(x + labelWidth, y, valueWidth, 0f), right);
            return y + g.MeasureString(value, font).Height + gapAfter;
        }

        private void PrintReceiptPageRGM(object sender, PrintPageEventArgs e)
        {
            // Draw only; all data was loaded beforehand.
            const float x = 2f;
            const float lineEnd = 185f;
            Graphics g = e.Graphics;
            float y = 5f;

            using (Font font1 = new Font("Tahoma", 8f, FontStyle.Bold))
            using (Font font2 = new Font("Tahoma", 7f, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center })
            using (StringFormat near = new StringFormat { Alignment = StringAlignment.Near })
            using (StringFormat far = new StringFormat { Alignment = StringAlignment.Far })
            using (Pen pen = new Pen(Color.Black, 1f) { DashStyle = DashStyle.Dash })
            {
                // Logo
                if (this._iswithLogo)
                {
                    string logoPath = Path.Combine(Application.StartupPath, "receiptlogo.jpg");
                    if (File.Exists(logoPath))
                    {
                        using (Image image = Image.FromFile(logoPath))
                        {
                            g.DrawImage(image, (e.PageBounds.Width - image.Width) / 2f, y, image.Width, image.Height);
                        }
                        y += 48f;
                    }
                }

                // Header
                g.DrawString(this._storename, font1, brush, new RectangleF(x, y, lineEnd, 0f), center);
                y += g.MeasureString(this._storename, font1).Height;

                g.DrawString(this._address, font2, brush, new RectangleF(x, y, lineEnd, 0f), center);
                y += g.MeasureString(this._address, font2).Height;

                string tin = "TIN No.: " + this._tinno;
                g.DrawString(tin, font2, brush, new RectangleF(x, y, lineEnd, 0f), center);
                y += g.MeasureString(tin, font2).Height;

                string title = "Z-Reading Report";
                g.DrawString(title, font1, brush, new RectangleF(x, y, lineEnd, 0f), center);
                y += g.MeasureString(title, font1).Height + 10f;

                g.DrawLine(pen, new PointF(x, y), new PointF(lineEnd, y));
                y += 10f;

                // Date / time (fixed at load so preview and print match)
                string date = "DATE: " + this._dtNow.ToString("M/d/yyyy");
                string time = "TIME : " + this._printTime.ToString("hh:mm tt");
                g.DrawString(date, font2, brush, new RectangleF(x, y, lineEnd, 0f), near);
                g.DrawString(time, font2, brush, new RectangleF(x, y, lineEnd, 0f), far);
                y += g.MeasureString(time, font2).Height + 12f;

                g.DrawLine(pen, new PointF(x, y), new PointF(lineEnd, y));
                y += 8f;

                // Sales figures
                y = DrawRow(g, font2, brush, near, far, "GROSS SALES", Fmt(_z.GROSSSALES), y, 12f);
                y = DrawRow(g, font2, brush, near, far, "Regular Discount", Fmt(_z.REGDISCOUNT), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "Senior Discount", Fmt(_z.SCDISCOUNT), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "PWD Discount", Fmt(_z.PWDDISCOUNT), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "NET SALES", Fmt(_z.NETSALES), y, 12f);
                y = DrawRow(g, font2, brush, near, far, "VAT Sales", Fmt(_z.VATSALES), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "12% VAT", Fmt(_z.VATAMT), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "Non-VAT Sales", Fmt(_z.NONVATSALES), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "VAT-Exempt Sales", Fmt(_z.VATEXEMSALES), y, 0f);
                y = DrawRow(g, font2, brush, near, far, "Zero-Rated Sales", Fmt(_z.ZERORATEDSALES), y, 8f);

                g.DrawLine(pen, new PointF(x, y), new PointF(lineEnd, y));
                y += 8f;

                // Counts
                string txCount = (_z.NOOFTRANSACTIONS ?? 0).ToString("N0");
                y = DrawRow(g, font2, brush, near, far, "No. of Transactions", txCount, y, 0f);
                y = DrawRow(g, font2, brush, near, far, "Total Quantity", Fmt(_z.NOOFITEMS), y, 8f);

                g.DrawLine(pen, new PointF(x, y), new PointF(lineEnd, y));
                y += 8f;

                // Footer
                g.DrawString("Z-Reading Report End", font1, brush, new RectangleF(x, y, lineEnd, 0f), center);
            }

            e.HasMorePages = false;
        }
    }
}