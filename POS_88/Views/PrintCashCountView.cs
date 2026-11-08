using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;

namespace POS_88.Views
{
    public partial class PrintCashCountView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private string _storename;
        private string _address;
        private string _tinno;
        private string _contactno;
        private bool _isPrint;
        private bool _iswithLogo;
        private int _cashCountID;

        public PrintCashCountView(int cashcountid)
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Fuchsia;
            TransparencyKey = Color.Fuchsia;
            StartPosition = FormStartPosition.CenterParent;

            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._cashCountID = cashcountid;
            this._isPrint = false;
            this.KeyDown += PrintCashCountView_KeyDown;
            this.FormClosed += PrintCashCountView_FormClosed;
            btnEnterOrPrint.Click += btnEnterOrPrint_Click;
            btnEscOrCancel.Click += btnEscOrCancel_Click;
        }

        private void PrintCashCountView_Load(object sender, EventArgs e)
        {
            this.recordDoc = new PrintDocument();
            this.PrintReceiptForTransaction();
        }


        private bool HasPhysicalPrinter(PrintDocument doc)
        {
            return doc != null && !IsVirtualPrinter(doc.PrinterSettings.PrinterName);
        }

        private static bool IsVirtualPrinter(string name)
        {
            return !string.IsNullOrEmpty(name) &&
                   (name.IndexOf("Fax", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("xps", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("onenote", StringComparison.OrdinalIgnoreCase) >= 0);
        }
      

        public void PrintReceiptForTransaction()
        {
            this.recordDoc.DocumentName = "Z-Reading";
            this.recordDoc.PrintPage += new PrintPageEventHandler(this.PrintReceiptPageRGM);
            PrinterSettings printerSettings = new PrinterSettings();
            setting setting = this._unitOfWork.Setting.Find(xy => xy.settingsid == 1).FirstOrDefault<setting>();
            this._storename = setting.storename;
            this._address = setting.address;
            this._contactno = setting.contactno;
            this._tinno = setting.tinno;
            this._iswithLogo = setting.withLogo.GetValueOrDefault();
            printerSettings.PrinterName = setting.printername;
            this.recordDoc.PrinterSettings = printerSettings;
       
            if (ppc != null)
            {
                ppc.Document = recordDoc;
                ppc.AutoZoom = false;
                ppc.UseAntiAlias = true;
                ppc.Zoom = 1.5;
                ppc.StartPage = 0;
                ppc.InvalidatePreview();
                ppc.Refresh();
            }
        }

        private static string Fmt(Decimal? value)
        {
            return value.GetValueOrDefault().ToString("N");
        }

        private static string Fmt(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("M/d/yyyy hh:mm tt") : null;
        }

        private void PrintReceiptPageRGM(object sender, PrintPageEventArgs e)
        {
            float x = 2f;
            float y = 5f;
            float pageW = 185f;
            Graphics g = e.Graphics;

            using (Font fontBold = new Font("Tahoma", 8f, FontStyle.Bold))
            using (Font fontReg = new Font("Tahoma", 7f, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Pen pen = new Pen(Color.Black, 1f) { DashStyle = DashStyle.Dash })
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center })
            using (StringFormat near = new StringFormat { Alignment = StringAlignment.Near })
            using (StringFormat far = new StringFormat { Alignment = StringAlignment.Far })
            {
                // Local helpers (C# 6 compatible: lambdas instead of local functions)
                Func<string, Font, StringFormat, float, float> centered = (text, font, fmt, yy) =>
                {
                    g.DrawString(text, font, brush, new RectangleF(x, yy, pageW, 0f), fmt);
                    return yy + g.MeasureString(text, font).Height;
                };

                Action<float> line = yy => g.DrawLine(pen, new PointF(x, yy), new PointF(pageW, yy));

                // Two-column row: label on left, amount on right
                Func<string, string, float, float, float, float> row = (label, value, yy, labelW, valueW) =>
                {
                    g.DrawString(label, fontReg, brush, new RectangleF(x, yy, labelW, 0f), near);
                    g.DrawString(value, fontReg, brush, new RectangleF(x + labelW, yy, valueW, 0f), far);
                    return yy + g.MeasureString(value, fontReg).Height;
                };

                // ---------- Header ----------
                if (this._iswithLogo)
                {
                    using (Image image = Image.FromFile(Application.StartupPath + "\\receiptlogo.jpg"))
                    {
                        g.DrawImage(image, (e.PageBounds.Width - image.Width) / 2, y, image.Width, image.Height);
                    }
                    y += 48f;
                }

                y = centered(this._storename, fontBold, center, y);
                y = centered(this._address, fontReg, center, y);
                y = centered("TIN No.: " + this._tinno, fontReg, center, y);
                y = centered("Cash Count Report", fontBold, center, y) + 10f;
                line(y);

                cashcount cc = this._unitOfWork.CashCount.Find(yy => yy.SqNo == this._cashCountID).FirstOrDefault<cashcount>();

                y += 10f;
                g.DrawString("Cashier: " + cc.UserNm, fontReg, brush, new RectangleF(x, y, pageW, 0f), near);
                y += g.MeasureString("X", fontReg).Height;
                g.DrawString("Cash Count #: " + cc.SqNo.ToString(), fontReg, brush, new RectangleF(x, y, pageW, 0f), near);
                y += g.MeasureString("X", fontReg).Height;
                g.DrawString("Start Shift: " + Fmt(cc.TimeIn), fontReg, brush, new RectangleF(x, y, pageW, 0f), near);
                y += g.MeasureString("X", fontReg).Height;
                g.DrawString("End Shift: " + Fmt(cc.TimeoOut), fontReg, brush, new RectangleF(x, y, pageW, 0f), near);
                y += g.MeasureString("X", fontReg).Height + 8f;
                line(y);

                // ---------- Denomination table ----------
                y += 4f;
                float w1 = 70f, w2 = 35f, w3 = 80f;
                g.DrawString("CASH", fontReg, brush, new RectangleF(x, y, w1, 0f), near);
                g.DrawString("QTY", fontReg, brush, new RectangleF(x + w1, y, w2, 0f), center);
                g.DrawString("TOTAL", fontReg, brush, new RectangleF(x + w1 + w2, y, w3, 0f), far);
                y += g.MeasureString("TOTAL", fontReg).Height + 4f;
                line(y);
                y += 8f;

                var denominations = new[]
                {
                    new { Label = "1,000.00",    Qty = cc.OneThousand.ToString(),    Amt = cc.OneThousandAmt },
                    new { Label = "500.00",      Qty = cc.FiveHundred.ToString(),    Amt = cc.FiveHundredAmt },
                    new { Label = "200.00",      Qty = cc.TwoHundred.ToString(),     Amt = cc.TwoHundredAmt },
                    new { Label = "100.00",      Qty = cc.OneHundred.ToString(),     Amt = cc.OneHundredAmt },
                    new { Label = "50.00",       Qty = cc.Fifty.ToString(),          Amt = cc.FiftyAmt },
                    new { Label = "20.00 Bills", Qty = cc.Twenty.ToString(),         Amt = cc.TwentyAmt },
                    new { Label = "20.00 Coins", Qty = cc.TwentyCoins.ToString(),    Amt = cc.TwentyCoinsAmt },
                    new { Label = "10.00",       Qty = cc.TenCoins.ToString(),       Amt = cc.TenCoinsAmt },
                    new { Label = "5.00",        Qty = cc.FiveCoins.ToString(),      Amt = cc.FiveCoinsAmt },
                    new { Label = "1.00",        Qty = cc.OneCoins.ToString(),       Amt = cc.OneCoinsAmt },
                    new { Label = "0.25",        Qty = cc.TwentyFiveCents.ToString(),Amt = cc.TwentyFiveCentsAmt },
                    new { Label = "0.10",        Qty = cc.TenCents.ToString(),       Amt = cc.TenCentsAmt },
                    new { Label = "0.05",        Qty = cc.FiveCents.ToString(),      Amt = cc.FiveCentsAmt },
                };

                foreach (var d in denominations)
                {
                    string amt = Fmt(d.Amt);
                    g.DrawString(d.Label, fontReg, brush, new RectangleF(x, y, w1, 0f), near);
                    g.DrawString(d.Qty, fontReg, brush, new RectangleF(x + w1, y, w2, 0f), center);
                    g.DrawString(amt, fontReg, brush, new RectangleF(x + w1 + w2, y, w3, 0f), far);
                    y += g.MeasureString(amt, fontReg).Height;
                }

                y += 8f;
                line(y);

                // ---------- Cash Count Summary ----------
                y += 8f;
                y = centered("Cash Count Summary", fontBold, center, y);
                y = row("Cash Count", Fmt(cc.CashCountAmt), y, w1 + w2, w3);
                y = row("Opening Amount", Fmt(cc.OpeningAmt), y, w1 + w2, w3);
                Decimal cashOnSales = Convert.ToDecimal(cc.CashCountAmt) - Convert.ToDecimal(cc.OpeningAmt);
                y = row("Cash on Sales", cashOnSales.ToString("N"), y, w1 + w2, w3);

                spGetZReadingByZno_Result z = this._unitOfWork.ItemSales.GetZReadingReportByZno(this._cashCountID).FirstOrDefault<spGetZReadingByZno_Result>();

                y += 12f;
                line(y);

                // ---------- Sales Report ----------
                float lw = 95f, vw = 90f;
                y = centered("Sales Report", fontBold, center, y) + 8f;
                y = row("GROSS SALES", Fmt(z.GROSSSALES), y, lw, vw) + 12f;
                y = row("Regular Discount", Fmt(z.REGDISCOUNT), y, lw, vw);
                y = row("Senior Discount", Fmt(z.SCDISCOUNT), y, lw, vw);
                y = row("PWD Discount", Fmt(z.PWDDISCOUNT), y, lw, vw);
                y = row("NET SALES", Fmt(z.NETSALES), y, lw, vw) + 12f;
                y = row("VAT Sales", Fmt(z.VATSALES), y, lw, vw);
                y = row("12% VAT", Fmt(z.VATAMT), y, lw, vw);
                y = row("Non-VAT Sales", Fmt(z.NONVATSALES), y, lw, vw);
                y = row("VAT-Exempt Sales", Fmt(z.VATEXEMSALES), y, lw, vw);
                y = row("Zero-Rated Sales", Fmt(z.ZERORATEDSALES), y, lw, vw) + 8f;
                line(y);

                y += 8f;
                string trans = z.NOOFTRANSACTIONS.GetValueOrDefault().ToString("N");
                y = row("No. of Transactions", trans, y, lw, vw);
                y = row("Total Quantity", Fmt(z.NOOFITEMS), y, lw, vw) + 8f;
                line(y);

                // ---------- Summary ----------
                y += 8f;
                y = centered("Summary", fontBold, center, y) + 8f;
                y = row("OPENING AMOUNT", Fmt(cc.OpeningAmt), y, lw, vw);
                y = row("NET SALES", Fmt(z.NETSALES), y, lw, vw);

                var cashIn = this._unitOfWork.CashInOut.GetCashInOut(this._cashCountID)
                    .Where(xy => xy.cashType == "CASH IN").FirstOrDefault();
                Decimal cashInAmt = cashIn != null ? Convert.ToDecimal(cashIn.amount) : 0M;
                y = row("CASH IN", cashInAmt.ToString("N"), y, lw, vw);

                Decimal totalCash = Convert.ToDecimal(cc.OpeningAmt) + Convert.ToDecimal(z.NETSALES) + cashInAmt;
                y = row("TOTAL CASH", totalCash.ToString("N"), y, lw, vw);
                y = row("CASH COUNT", Fmt(cc.GrandTotal), y, lw, vw);

                var cashOut = this._unitOfWork.CashInOut.GetCashInOut(this._cashCountID)
                    .Where(xy => xy.cashType == "CASH OUT").FirstOrDefault();
                Decimal cashOutAmt = cashOut != null ? Convert.ToDecimal(cashOut.amount) : 0M;
                y = row("CASH OUT", cashOutAmt.ToString("N"), y, lw, vw);

                Decimal shortOver = Convert.ToDecimal(cc.GrandTotal) + cashOutAmt - totalCash;
                y = row("SHORT/OVER", shortOver.ToString("N"), y, lw, vw);
            }
        }

        // ------------------------------------------------------------------
        // Form behaviour
        // ------------------------------------------------------------------

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                btnEscOrCancel_Click(this, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.Return && !_isPrint)
            {
                btnEnterOrPrint_Click(this, EventArgs.Empty);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnEnterOrPrint_Click(object sender, EventArgs e)
        {
            if (!HasPhysicalPrinter(recordDoc))
            {
                //MessageBox.Show("No receipt printer found. Please set a printer in Settings.", "Print Receipt",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                recordDoc.Print();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isPrint = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnEscOrCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void PrintCashCountView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnEscOrCancel_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnEnterOrPrint_Click(sender, e);
                e.Handled = true;
            }
        }


        private void PrintCashCountView_FormClosed(object sender, FormClosedEventArgs e)
        {
            ppc.Document = null;
            if (recordDoc != null)
                recordDoc.Dispose();
            if (recordDoc != null)
                recordDoc.Dispose();
            _unitOfWork.Dispose();
        }

       
    }
}
