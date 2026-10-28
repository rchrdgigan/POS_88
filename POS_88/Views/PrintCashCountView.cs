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

        //private void PrintReceiptPageRGM(object sender, PrintPageEventArgs e)
        //{
        //    float x = 2f;
        //    float y1 = 5f;
        //    float num1 = 185f;
        //    float height1 = 0.0f;
        //    Font font1 = new Font("Tahoma", 8f, FontStyle.Bold);
        //    Font font2 = new Font("Tahoma", 7f, FontStyle.Regular);
        //    SolidBrush solidBrush = new SolidBrush(Color.Black);
        //    StringFormat format1 = new StringFormat();
        //    format1.Alignment = StringAlignment.Center;
        //    StringFormat format2 = new StringFormat();
        //    format2.Alignment = StringAlignment.Near;
        //    StringFormat format3 = new StringFormat();
        //    format3.Alignment = StringAlignment.Far;
        //    if (this._iswithLogo)
        //    {
        //        Image image = Image.FromFile(Application.StartupPath + "\\receiptlogo.jpg");
        //        e.Graphics.DrawImage(image, (float)((e.PageBounds.Width - image.Width) / 2), y1 / 1f, (float)image.Width, (float)image.Height);
        //        y1 += 48f;
        //    }
        //    string storename = this._storename;
        //    e.Graphics.DrawString(storename, font1, (Brush)solidBrush, new RectangleF(x, y1, num1, height1), format1);
        //    double num2 = (double)y1;
        //    SizeF sizeF = e.Graphics.MeasureString(storename, font1);
        //    double height2 = (double)sizeF.Height;
        //    float y2 = (float)(num2 + height2);
        //    string address = this._address;
        //    e.Graphics.DrawString(address, font2, (Brush)solidBrush, new RectangleF(x, y2, num1, height1), format1);
        //    double num3 = (double)y2;
        //    sizeF = e.Graphics.MeasureString(address, font2);
        //    double height3 = (double)sizeF.Height;
        //    float y3 = (float)(num3 + height3);
        //    string str1 = "TIN No.: " + this._tinno;
        //    e.Graphics.DrawString(str1, font2, (Brush)solidBrush, new RectangleF(x, y3, num1, height1), format1);
        //    double num4 = (double)y3;
        //    sizeF = e.Graphics.MeasureString(str1, font2);
        //    double height4 = (double)sizeF.Height;
        //    float y4 = (float)(num4 + height4);
        //    string str2 = "Cash Count Report";
        //    e.Graphics.DrawString(str2, font1, (Brush)solidBrush, new RectangleF(x, y4, num1, height1), format1);
        //    double num5 = (double)y4;
        //    sizeF = e.Graphics.MeasureString(str2, font1);
        //    double height5 = (double)sizeF.Height;
        //    float y5 = (float)(num5 + height5) + 10f;
        //    Pen pen = new Pen(Color.Black, 1f);
        //    pen.DashStyle = DashStyle.Dash;
        //    PointF pt1 = new PointF(x, y5);
        //    PointF pt2 = new PointF(num1, y5);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    cashcount cashcount = this._unitOfWork.CashCount.Find(yy => yy.SqNo == this._cashCountID).FirstOrDefault<cashcount>();
        //    float y6 = y5 + 10f;
        //    string str3 = "Cashier: " + cashcount.UserNm;
        //    e.Graphics.DrawString(str3, font2, (Brush)solidBrush, new RectangleF(x, y6, num1, height1), format2);
        //    double num6 = (double)y6;
        //    sizeF = e.Graphics.MeasureString(str3, font2);
        //    double height6 = (double)sizeF.Height;
        //    float y7 = (float)(num6 + height6);
        //    int num7 = cashcount.SqNo;
        //    string str4 = "Cash Count #: " + num7.ToString();
        //    e.Graphics.DrawString(str4, font2, (Brush)solidBrush, new RectangleF(x, y7, num1, height1), format2);
        //    double num8 = (double)y7;
        //    sizeF = e.Graphics.MeasureString(str4, font2);
        //    double height7 = (double)sizeF.Height;
        //    float y8 = (float)(num8 + height7);
        //    DateTime? nullable1 = cashcount.TimeIn;
        //    DateTime? local1 = nullable1;
        //    DateTime valueOrDefault1;
        //    string str5;
        //    if (!local1.HasValue)
        //    {
        //        str5 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault1 = local1.GetValueOrDefault();
        //        str5 = valueOrDefault1.ToString("M/d/yyyy hh:mm tt");
        //    }
        //    string str6 = "Start Shift: " + str5;
        //    e.Graphics.DrawString(str6, font2, (Brush)solidBrush, new RectangleF(x, y8, num1, height1), format2);
        //    double num9 = (double)y8;
        //    sizeF = e.Graphics.MeasureString(str6, font2);
        //    double height8 = (double)sizeF.Height;
        //    float y9 = (float)(num9 + height8);
        //    nullable1 = cashcount.TimeoOut;
        //    DateTime? local2 = nullable1;
        //    string str7;
        //    if (!local2.HasValue)
        //    {
        //        str7 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault1 = local2.GetValueOrDefault();
        //        str7 = valueOrDefault1.ToString("M/d/yyyy hh:mm tt");
        //    }
        //    string str8 = "End Shift: " + str7;
        //    e.Graphics.DrawString(str8, font2, (Brush)solidBrush, new RectangleF(x, y9, num1, height1), format2);
        //    double num10 = (double)y9;
        //    sizeF = e.Graphics.MeasureString(str8, font2);
        //    double height9 = (double)sizeF.Height;
        //    float y10 = (float)(num10 + height9) + 8f;
        //    pt1 = new PointF(x, y10);
        //    pt2 = new PointF(num1, y10);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float y11 = y10 + 4f;
        //    float width1 = 70f;
        //    float width2 = 35f;
        //    float num11 = 0.0f;
        //    float width3 = 80f;
        //    string s1 = "CASH";
        //    e.Graphics.DrawString(s1, font2, (Brush)solidBrush, new RectangleF(x, y11, width1, height1), format2);
        //    string s2 = "QTY";
        //    e.Graphics.DrawString(s2, font2, (Brush)solidBrush, new RectangleF(x + width1, y11, width2, height1), format1);
        //    string str9 = "TOTAL";
        //    e.Graphics.DrawString(str9, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y11, width3, height1), format3);
        //    double num12 = (double)y11;
        //    sizeF = e.Graphics.MeasureString(str9, font2);
        //    double height10 = (double)sizeF.Height;
        //    float y12 = (float)(num12 + height10) + 4f;
        //    pt1 = new PointF(x, y12);
        //    pt2 = new PointF(num1, y12);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float y13 = y12 + 8f;
        //    string s3 = "1,000.00";
        //    e.Graphics.DrawString(s3, font2, (Brush)solidBrush, new RectangleF(x, y13, width1, height1), format2);
        //    string s4 = cashcount.OneThousand.ToString();
        //    e.Graphics.DrawString(s4, font2, (Brush)solidBrush, new RectangleF(x + width1, y13, width2, height1), format1);
        //    Decimal? oneThousandAmt = cashcount.OneThousandAmt;
        //    Decimal? local3 = oneThousandAmt;
        //    Decimal valueOrDefault2;
        //    string str10;
        //    if (!local3.HasValue)
        //    {
        //        str10 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local3.GetValueOrDefault();
        //        str10 = valueOrDefault2.ToString("N");
        //    }
        //    if (str10 == null)
        //        str10 = "0.00";
        //    string str11 = str10;
        //    e.Graphics.DrawString(str11, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y13, width3, height1), format3);
        //    double num13 = (double)y13;
        //    sizeF = e.Graphics.MeasureString(str11, font2);
        //    double height11 = (double)sizeF.Height;
        //    float y14 = (float)(num13 + height11);
        //    string s5 = "500.00";
        //    e.Graphics.DrawString(s5, font2, (Brush)solidBrush, new RectangleF(x, y14, width1, height1), format2);
        //    string s6 = cashcount.FiveHundred.ToString();
        //    e.Graphics.DrawString(s6, font2, (Brush)solidBrush, new RectangleF(x + width1, y14, width2, height1), format1);
        //    Decimal? fiveHundredAmt = cashcount.FiveHundredAmt;
        //    Decimal? local4 = fiveHundredAmt;
        //    string str12;
        //    if (!local4.HasValue)
        //    {
        //        str12 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local4.GetValueOrDefault();
        //        str12 = valueOrDefault2.ToString("N");
        //    }
        //    if (str12 == null)
        //        str12 = "0.00";
        //    string str13 = str12;
        //    e.Graphics.DrawString(str13, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y14, width3, height1), format3);
        //    double num14 = (double)y14;
        //    sizeF = e.Graphics.MeasureString(str13, font2);
        //    double height12 = (double)sizeF.Height;
        //    float y15 = (float)(num14 + height12);
        //    string s7 = "200.00";
        //    e.Graphics.DrawString(s7, font2, (Brush)solidBrush, new RectangleF(x, y15, width1, height1), format2);
        //    string s8 = cashcount.TwoHundred.ToString();
        //    e.Graphics.DrawString(s8, font2, (Brush)solidBrush, new RectangleF(x + width1, y15, width2, height1), format1);
        //    Decimal? twoHundredAmt = cashcount.TwoHundredAmt;
        //    Decimal? local5 = twoHundredAmt;
        //    string str14;
        //    if (!local5.HasValue)
        //    {
        //        str14 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local5.GetValueOrDefault();
        //        str14 = valueOrDefault2.ToString("N");
        //    }
        //    if (str14 == null)
        //        str14 = "0.00";
        //    string str15 = str14;
        //    e.Graphics.DrawString(str15, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y15, width3, height1), format3);
        //    double num15 = (double)y15;
        //    sizeF = e.Graphics.MeasureString(str15, font2);
        //    double height13 = (double)sizeF.Height;
        //    float y16 = (float)(num15 + height13);
        //    string s9 = "100.00";
        //    e.Graphics.DrawString(s9, font2, (Brush)solidBrush, new RectangleF(x, y16, width1, height1), format2);
        //    string s10 = cashcount.OneHundred.ToString();
        //    e.Graphics.DrawString(s10, font2, (Brush)solidBrush, new RectangleF(x + width1, y16, width2, height1), format1);
        //    Decimal? oneHundredAmt = cashcount.OneHundredAmt;
        //    Decimal? local6 = oneHundredAmt;
        //    string str16;
        //    if (!local6.HasValue)
        //    {
        //        str16 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local6.GetValueOrDefault();
        //        str16 = valueOrDefault2.ToString("N");
        //    }
        //    if (str16 == null)
        //        str16 = "0.00";
        //    string str17 = str16;
        //    e.Graphics.DrawString(str17, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y16, width3, height1), format3);
        //    double num16 = (double)y16;
        //    sizeF = e.Graphics.MeasureString(str17, font2);
        //    double height14 = (double)sizeF.Height;
        //    float y17 = (float)(num16 + height14);
        //    string s11 = "50.00";
        //    e.Graphics.DrawString(s11, font2, (Brush)solidBrush, new RectangleF(x, y17, width1, height1), format2);
        //    string s12 = cashcount.Fifty.ToString();
        //    e.Graphics.DrawString(s12, font2, (Brush)solidBrush, new RectangleF(x + width1, y17, width2, height1), format1);
        //    Decimal? fiftyAmt = cashcount.FiftyAmt;
        //    Decimal? local7 = fiftyAmt;
        //    string str18;
        //    if (!local7.HasValue)
        //    {
        //        str18 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local7.GetValueOrDefault();
        //        str18 = valueOrDefault2.ToString("N");
        //    }
        //    if (str18 == null)
        //        str18 = "0.00";
        //    string str19 = str18;
        //    e.Graphics.DrawString(str19, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y17, width3, height1), format3);
        //    double num17 = (double)y17;
        //    sizeF = e.Graphics.MeasureString(str19, font2);
        //    double height15 = (double)sizeF.Height;
        //    float y18 = (float)(num17 + height15);
        //    string s13 = "20.00 Bills";
        //    e.Graphics.DrawString(s13, font2, (Brush)solidBrush, new RectangleF(x, y18, width1, height1), format2);
        //    string s14 = cashcount.Twenty.ToString();
        //    e.Graphics.DrawString(s14, font2, (Brush)solidBrush, new RectangleF(x + width1, y18, width2, height1), format1);
        //    Decimal? twentyAmt = cashcount.TwentyAmt;
        //    Decimal? local8 = twentyAmt;
        //    string str20;
        //    if (!local8.HasValue)
        //    {
        //        str20 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local8.GetValueOrDefault();
        //        str20 = valueOrDefault2.ToString("N");
        //    }
        //    if (str20 == null)
        //        str20 = "0.00";
        //    string str21 = str20;
        //    e.Graphics.DrawString(str21, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y18, width3, height1), format3);
        //    double num18 = (double)y18;
        //    sizeF = e.Graphics.MeasureString(str21, font2);
        //    double height16 = (double)sizeF.Height;
        //    float y19 = (float)(num18 + height16);
        //    string s15 = "20.00 Coins";
        //    e.Graphics.DrawString(s15, font2, (Brush)solidBrush, new RectangleF(x, y19, width1, height1), format2);
        //    string s16 = cashcount.TwentyCoins.ToString();
        //    e.Graphics.DrawString(s16, font2, (Brush)solidBrush, new RectangleF(x + width1, y19, width2, height1), format1);
        //    Decimal? twentyCoinsAmt = cashcount.TwentyCoinsAmt;
        //    Decimal? local9 = twentyCoinsAmt;
        //    string str22;
        //    if (!local9.HasValue)
        //    {
        //        str22 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local9.GetValueOrDefault();
        //        str22 = valueOrDefault2.ToString("N");
        //    }
        //    if (str22 == null)
        //        str22 = "0.00";
        //    string str23 = str22;
        //    e.Graphics.DrawString(str23, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y19, width3, height1), format3);
        //    double num19 = (double)y19;
        //    sizeF = e.Graphics.MeasureString(str23, font2);
        //    double height17 = (double)sizeF.Height;
        //    float y20 = (float)(num19 + height17);
        //    string s17 = "10.00";
        //    e.Graphics.DrawString(s17, font2, (Brush)solidBrush, new RectangleF(x, y20, width1, height1), format2);
        //    string s18 = cashcount.TenCoins.ToString();
        //    e.Graphics.DrawString(s18, font2, (Brush)solidBrush, new RectangleF(x + width1, y20, width2, height1), format1);
        //    Decimal? tenCoinsAmt = cashcount.TenCoinsAmt;
        //    Decimal? local10 = tenCoinsAmt;
        //    string str24;
        //    if (!local10.HasValue)
        //    {
        //        str24 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local10.GetValueOrDefault();
        //        str24 = valueOrDefault2.ToString("N");
        //    }
        //    if (str24 == null)
        //        str24 = "0.00";
        //    string str25 = str24;
        //    e.Graphics.DrawString(str25, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y20, width3, height1), format3);
        //    double num20 = (double)y20;
        //    sizeF = e.Graphics.MeasureString(str25, font2);
        //    double height18 = (double)sizeF.Height;
        //    float y21 = (float)(num20 + height18);
        //    string s19 = "5.00";
        //    e.Graphics.DrawString(s19, font2, (Brush)solidBrush, new RectangleF(x, y21, width1, height1), format2);
        //    string s20 = cashcount.FiveCoins.ToString();
        //    e.Graphics.DrawString(s20, font2, (Brush)solidBrush, new RectangleF(x + width1, y21, width2, height1), format1);
        //    Decimal? fiveCoinsAmt = cashcount.FiveCoinsAmt;
        //    Decimal? local11 = fiveCoinsAmt;
        //    string str26;
        //    if (!local11.HasValue)
        //    {
        //        str26 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local11.GetValueOrDefault();
        //        str26 = valueOrDefault2.ToString("N");
        //    }
        //    if (str26 == null)
        //        str26 = "0.00";
        //    string str27 = str26;
        //    e.Graphics.DrawString(str27, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y21, width3, height1), format3);
        //    double num21 = (double)y21;
        //    sizeF = e.Graphics.MeasureString(str27, font2);
        //    double height19 = (double)sizeF.Height;
        //    float y22 = (float)(num21 + height19);
        //    string s21 = "1.00";
        //    e.Graphics.DrawString(s21, font2, (Brush)solidBrush, new RectangleF(x, y22, width1, height1), format2);
        //    string s22 = cashcount.OneCoins.ToString();
        //    e.Graphics.DrawString(s22, font2, (Brush)solidBrush, new RectangleF(x + width1, y22, width2, height1), format1);
        //    Decimal? oneCoinsAmt = cashcount.OneCoinsAmt;
        //    Decimal? local12 = oneCoinsAmt;
        //    string str28;
        //    if (!local12.HasValue)
        //    {
        //        str28 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local12.GetValueOrDefault();
        //        str28 = valueOrDefault2.ToString("N");
        //    }
        //    if (str28 == null)
        //        str28 = "0.00";
        //    string str29 = str28;
        //    e.Graphics.DrawString(str29, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y22, width3, height1), format3);
        //    double num22 = (double)y22;
        //    sizeF = e.Graphics.MeasureString(str29, font2);
        //    double height20 = (double)sizeF.Height;
        //    float y23 = (float)(num22 + height20);
        //    string s23 = "0.25";
        //    e.Graphics.DrawString(s23, font2, (Brush)solidBrush, new RectangleF(x, y23, width1, height1), format2);
        //    string s24 = cashcount.TwentyFiveCents.ToString();
        //    e.Graphics.DrawString(s24, font2, (Brush)solidBrush, new RectangleF(x + width1, y23, width2, height1), format1);
        //    Decimal? twentyFiveCentsAmt = cashcount.TwentyFiveCentsAmt;
        //    Decimal? local13 = twentyFiveCentsAmt;
        //    string str30;
        //    if (!local13.HasValue)
        //    {
        //        str30 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local13.GetValueOrDefault();
        //        str30 = valueOrDefault2.ToString("N");
        //    }
        //    if (str30 == null)
        //        str30 = "0.00";
        //    string str31 = str30;
        //    e.Graphics.DrawString(str31, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y23, width3, height1), format3);
        //    double num23 = (double)y23;
        //    sizeF = e.Graphics.MeasureString(str31, font2);
        //    double height21 = (double)sizeF.Height;
        //    float y24 = (float)(num23 + height21);
        //    string s25 = "0.10";
        //    e.Graphics.DrawString(s25, font2, (Brush)solidBrush, new RectangleF(x, y24, width1, height1), format2);
        //    string s26 = cashcount.TenCents.ToString();
        //    e.Graphics.DrawString(s26, font2, (Brush)solidBrush, new RectangleF(x + width1, y24, width2, height1), format1);
        //    Decimal? tenCentsAmt = cashcount.TenCentsAmt;
        //    Decimal? local14 = tenCentsAmt;
        //    string str32;
        //    if (!local14.HasValue)
        //    {
        //        str32 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local14.GetValueOrDefault();
        //        str32 = valueOrDefault2.ToString("N");
        //    }
        //    if (str32 == null)
        //        str32 = "0.00";
        //    string str33 = str32;
        //    e.Graphics.DrawString(str33, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y24, width3, height1), format3);
        //    double num24 = (double)y24;
        //    sizeF = e.Graphics.MeasureString(str33, font2);
        //    double height22 = (double)sizeF.Height;
        //    float y25 = (float)(num24 + height22);
        //    string s27 = "0.05";
        //    e.Graphics.DrawString(s27, font2, (Brush)solidBrush, new RectangleF(x, y25, width1, height1), format2);
        //    string s28 = cashcount.FiveCents.ToString();
        //    e.Graphics.DrawString(s28, font2, (Brush)solidBrush, new RectangleF(x + width1, y25, width2, height1), format1);
        //    Decimal? fiveCentsAmt = cashcount.FiveCentsAmt;
        //    Decimal? local15 = fiveCentsAmt;
        //    string str34;
        //    if (!local15.HasValue)
        //    {
        //        str34 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local15.GetValueOrDefault();
        //        str34 = valueOrDefault2.ToString("N");
        //    }
        //    if (str34 == null)
        //        str34 = "0.00";
        //    string str35 = str34;
        //    e.Graphics.DrawString(str35, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y25, width3, height1), format3);
        //    double num25 = (double)y25;
        //    sizeF = e.Graphics.MeasureString(str35, font2);
        //    double height23 = (double)sizeF.Height;
        //    float y26 = (float)(num25 + height23) + 8f;
        //    pt1 = new PointF(x, y26);
        //    pt2 = new PointF(num1, y26);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float y27 = y26 + 8f;
        //    string str36 = "Cash Count Summary";
        //    e.Graphics.DrawString(str36, font1, (Brush)solidBrush, new RectangleF(x, y27, num1, height1), format1);
        //    double num26 = (double)y27;
        //    sizeF = e.Graphics.MeasureString(str36, font1);
        //    double height24 = (double)sizeF.Height;
        //    float y28 = (float)(num26 + height24);
        //    string s29 = "Cash Count";
        //    e.Graphics.DrawString(s29, font2, (Brush)solidBrush, new RectangleF(x, y28, width1, height1), format2);
        //    Decimal? nullable2 = cashcount.CashCountAmt;
        //    Decimal? local16 = nullable2;
        //    string str37;
        //    if (!local16.HasValue)
        //    {
        //        str37 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local16.GetValueOrDefault();
        //        str37 = valueOrDefault2.ToString("N");
        //    }
        //    if (str37 == null)
        //        str37 = "0.00";
        //    string str38 = str37;
        //    e.Graphics.DrawString(str38, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y28, width3, height1), format3);
        //    double num27 = (double)y28;
        //    sizeF = e.Graphics.MeasureString(str38, font2);
        //    double height25 = (double)sizeF.Height;
        //    float y29 = (float)(num27 + height25);
        //    string s30 = "Opening Amount";
        //    e.Graphics.DrawString(s30, font2, (Brush)solidBrush, new RectangleF(x, y29, width1 + width2, height1), format2);
        //    nullable2 = cashcount.OpeningAmt;
        //    Decimal? local17 = nullable2;
        //    string str39;
        //    if (!local17.HasValue)
        //    {
        //        str39 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local17.GetValueOrDefault();
        //        str39 = valueOrDefault2.ToString("N");
        //    }
        //    if (str39 == null)
        //        str39 = "0.00";
        //    string str40 = str39;
        //    e.Graphics.DrawString(str40, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y29, width3, height1), format3);
        //    double num28 = (double)y29;
        //    sizeF = e.Graphics.MeasureString(str40, font2);
        //    double height26 = (double)sizeF.Height;
        //    float y30 = (float)(num28 + height26);
        //    string s31 = "Cash on Sales";
        //    e.Graphics.DrawString(s31, font2, (Brush)solidBrush, new RectangleF(x, y30, width1, height1), format2);
        //    string str41 = (Convert.ToDecimal((object)cashcount.CashCountAmt) - Convert.ToDecimal((object)cashcount.OpeningAmt)).ToString("N") ?? "0.00";
        //    e.Graphics.DrawString(str41, font2, (Brush)solidBrush, new RectangleF(x + width1 + width2 + num11, y30, width3, height1), format3);
        //    double num29 = (double)y30;
        //    sizeF = e.Graphics.MeasureString(str41, font2);
        //    double height27 = (double)sizeF.Height;
        //    float num30 = (float)(num29 + height27);
        //    spGetZReadingByZno_Result zreadingByZnoResult = this._unitOfWork.ItemSales.GetZReadingReportByZno(this._cashCountID).FirstOrDefault<spGetZReadingByZno_Result>();
        //    float y31 = num30 + 12f;
        //    pt1 = new PointF(x, y31);
        //    pt2 = new PointF(num1, y31);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float width4 = 95f;
        //    float num31 = 0.0f;
        //    float num32 = 0.0f;
        //    float width5 = 90f;
        //    string str42 = "Sales Report";
        //    e.Graphics.DrawString(str42, font1, (Brush)solidBrush, new RectangleF(x, y31, num1, height1), format1);
        //    double num33 = (double)y31;
        //    sizeF = e.Graphics.MeasureString(str42, font1);
        //    double height28 = (double)sizeF.Height;
        //    float y32 = (float)(num33 + height28) + 8f;
        //    string s32 = "GROSS SALES";
        //    e.Graphics.DrawString(s32, font2, (Brush)solidBrush, new RectangleF(x, y32, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.GROSSSALES;
        //    Decimal? local18 = nullable2;
        //    string str43;
        //    if (!local18.HasValue)
        //    {
        //        str43 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local18.GetValueOrDefault();
        //        str43 = valueOrDefault2.ToString("N");
        //    }
        //    if (str43 == null)
        //        str43 = "0.00";
        //    string str44 = str43;
        //    e.Graphics.DrawString(str44, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y32, width5, height1), format3);
        //    double num34 = (double)y32;
        //    sizeF = e.Graphics.MeasureString(str44, font2);
        //    double height29 = (double)sizeF.Height;
        //    float y33 = (float)(num34 + height29) + 12f;
        //    string s33 = "Regular Discount";
        //    e.Graphics.DrawString(s33, font2, (Brush)solidBrush, new RectangleF(x, y33, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.REGDISCOUNT;
        //    Decimal? local19 = nullable2;
        //    string str45;
        //    if (!local19.HasValue)
        //    {
        //        str45 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local19.GetValueOrDefault();
        //        str45 = valueOrDefault2.ToString("N");
        //    }
        //    if (str45 == null)
        //        str45 = "0.00";
        //    string str46 = str45;
        //    e.Graphics.DrawString(str46, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y33, width5, height1), format3);
        //    double num35 = (double)y33;
        //    sizeF = e.Graphics.MeasureString(str46, font2);
        //    double height30 = (double)sizeF.Height;
        //    float y34 = (float)(num35 + height30);
        //    string s34 = "Senior Discount";
        //    e.Graphics.DrawString(s34, font2, (Brush)solidBrush, new RectangleF(x, y34, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.SCDISCOUNT;
        //    Decimal? local20 = nullable2;
        //    string str47;
        //    if (!local20.HasValue)
        //    {
        //        str47 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local20.GetValueOrDefault();
        //        str47 = valueOrDefault2.ToString("N");
        //    }
        //    if (str47 == null)
        //        str47 = "0.00";
        //    string str48 = str47;
        //    e.Graphics.DrawString(str48, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y34, width5, height1), format3);
        //    double num36 = (double)y34;
        //    sizeF = e.Graphics.MeasureString(str48, font2);
        //    double height31 = (double)sizeF.Height;
        //    float y35 = (float)(num36 + height31);
        //    string s35 = "PWD Discount";
        //    e.Graphics.DrawString(s35, font2, (Brush)solidBrush, new RectangleF(x, y35, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.PWDDISCOUNT;
        //    Decimal? local21 = nullable2;
        //    string str49;
        //    if (!local21.HasValue)
        //    {
        //        str49 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local21.GetValueOrDefault();
        //        str49 = valueOrDefault2.ToString("N");
        //    }
        //    if (str49 == null)
        //        str49 = "0.00";
        //    string str50 = str49;
        //    e.Graphics.DrawString(str50, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y35, width5, height1), format3);
        //    double num37 = (double)y35;
        //    sizeF = e.Graphics.MeasureString(str50, font2);
        //    double height32 = (double)sizeF.Height;
        //    float y36 = (float)(num37 + height32);
        //    string s36 = "NET SALES";
        //    e.Graphics.DrawString(s36, font2, (Brush)solidBrush, new RectangleF(x, y36, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.NETSALES;
        //    Decimal? local22 = nullable2;
        //    string str51;
        //    if (!local22.HasValue)
        //    {
        //        str51 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local22.GetValueOrDefault();
        //        str51 = valueOrDefault2.ToString("N");
        //    }
        //    if (str51 == null)
        //        str51 = "0.00";
        //    string str52 = str51;
        //    e.Graphics.DrawString(str52, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y36, width5, height1), format3);
        //    double num38 = (double)y36;
        //    sizeF = e.Graphics.MeasureString(str52, font2);
        //    double height33 = (double)sizeF.Height;
        //    float y37 = (float)(num38 + height33) + 12f;
        //    string s37 = "VAT Sales";
        //    e.Graphics.DrawString(s37, font2, (Brush)solidBrush, new RectangleF(x, y37, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.VATSALES;
        //    Decimal? local23 = nullable2;
        //    string str53;
        //    if (!local23.HasValue)
        //    {
        //        str53 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local23.GetValueOrDefault();
        //        str53 = valueOrDefault2.ToString("N");
        //    }
        //    if (str53 == null)
        //        str53 = "0.00";
        //    string str54 = str53;
        //    e.Graphics.DrawString(str54, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y37, width5, height1), format3);
        //    double num39 = (double)y37;
        //    sizeF = e.Graphics.MeasureString(str54, font2);
        //    double height34 = (double)sizeF.Height;
        //    float y38 = (float)(num39 + height34);
        //    string s38 = "12% VAT";
        //    e.Graphics.DrawString(s38, font2, (Brush)solidBrush, new RectangleF(x, y38, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.VATAMT;
        //    Decimal? local24 = nullable2;
        //    string str55;
        //    if (!local24.HasValue)
        //    {
        //        str55 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local24.GetValueOrDefault();
        //        str55 = valueOrDefault2.ToString("N");
        //    }
        //    if (str55 == null)
        //        str55 = "0.00";
        //    string str56 = str55;
        //    e.Graphics.DrawString(str56, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y38, width5, height1), format3);
        //    double num40 = (double)y38;
        //    sizeF = e.Graphics.MeasureString(str56, font2);
        //    double height35 = (double)sizeF.Height;
        //    float y39 = (float)(num40 + height35);
        //    string s39 = "Non-VAT Sales";
        //    e.Graphics.DrawString(s39, font2, (Brush)solidBrush, new RectangleF(x, y39, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.NONVATSALES;
        //    Decimal? local25 = nullable2;
        //    string str57;
        //    if (!local25.HasValue)
        //    {
        //        str57 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local25.GetValueOrDefault();
        //        str57 = valueOrDefault2.ToString("N");
        //    }
        //    if (str57 == null)
        //        str57 = "0.00";
        //    string str58 = str57;
        //    e.Graphics.DrawString(str58, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y39, width5, height1), format3);
        //    double num41 = (double)y39;
        //    sizeF = e.Graphics.MeasureString(str58, font2);
        //    double height36 = (double)sizeF.Height;
        //    float y40 = (float)(num41 + height36);
        //    string s40 = "VAT-Exempt Sales";
        //    e.Graphics.DrawString(s40, font2, (Brush)solidBrush, new RectangleF(x, y40, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.VATEXEMSALES;
        //    Decimal? local26 = nullable2;
        //    string str59;
        //    if (!local26.HasValue)
        //    {
        //        str59 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local26.GetValueOrDefault();
        //        str59 = valueOrDefault2.ToString("N");
        //    }
        //    if (str59 == null)
        //        str59 = "0.00";
        //    string str60 = str59;
        //    e.Graphics.DrawString(str60, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y40, width5, height1), format3);
        //    double num42 = (double)y40;
        //    sizeF = e.Graphics.MeasureString(str60, font2);
        //    double height37 = (double)sizeF.Height;
        //    float y41 = (float)(num42 + height37);
        //    string s41 = "Zero-Rated Sales";
        //    e.Graphics.DrawString(s41, font2, (Brush)solidBrush, new RectangleF(x, y41, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.ZERORATEDSALES;
        //    Decimal? local27 = nullable2;
        //    string str61;
        //    if (!local27.HasValue)
        //    {
        //        str61 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local27.GetValueOrDefault();
        //        str61 = valueOrDefault2.ToString("N");
        //    }
        //    if (str61 == null)
        //        str61 = "0.00";
        //    string str62 = str61;
        //    e.Graphics.DrawString(str62, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y41, width5, height1), format3);
        //    double num43 = (double)y41;
        //    sizeF = e.Graphics.MeasureString(str62, font2);
        //    double height38 = (double)sizeF.Height;
        //    float y42 = (float)(num43 + height38) + 8f;
        //    pt1 = new PointF(x, y42);
        //    pt2 = new PointF(num1, y42);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float y43 = y42 + 8f;
        //    string s42 = "No. of Transactions";
        //    e.Graphics.DrawString(s42, font2, (Brush)solidBrush, new RectangleF(x, y43, width4, height1), format2);
        //    int? nooftransactions = zreadingByZnoResult.NOOFTRANSACTIONS;
        //    int? local28 = nooftransactions;
        //    string str63;
        //    if (!local28.HasValue)
        //    {
        //        str63 = (string)null;
        //    }
        //    else
        //    {
        //        num7 = local28.GetValueOrDefault();
        //        str63 = num7.ToString("N");
        //    }
        //    if (str63 == null)
        //        str63 = "0.00";
        //    string str64 = str63;
        //    e.Graphics.DrawString(str64, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y43, width5, height1), format3);
        //    double num44 = (double)y43;
        //    sizeF = e.Graphics.MeasureString(str64, font2);
        //    double height39 = (double)sizeF.Height;
        //    float y44 = (float)(num44 + height39);
        //    string s43 = "Total Quantity";
        //    e.Graphics.DrawString(s43, font2, (Brush)solidBrush, new RectangleF(x, y44, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.NOOFITEMS;
        //    Decimal? local29 = nullable2;
        //    string str65;
        //    if (!local29.HasValue)
        //    {
        //        str65 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local29.GetValueOrDefault();
        //        str65 = valueOrDefault2.ToString("N");
        //    }
        //    if (str65 == null)
        //        str65 = "0.00";
        //    string str66 = str65;
        //    e.Graphics.DrawString(str66, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y44, width5, height1), format3);
        //    double num45 = (double)y44;
        //    sizeF = e.Graphics.MeasureString(str66, font2);
        //    double height40 = (double)sizeF.Height;
        //    float y45 = (float)(num45 + height40) + 8f;
        //    pt1 = new PointF(x, y45);
        //    pt2 = new PointF(num1, y45);
        //    e.Graphics.DrawLine(pen, pt1, pt2);
        //    float y46 = y45 + 8f;
        //    string str67 = "Summary";
        //    e.Graphics.DrawString(str67, font1, (Brush)solidBrush, new RectangleF(x, y46, num1, height1), format1);
        //    double num46 = (double)y46;
        //    sizeF = e.Graphics.MeasureString(str67, font1);
        //    double height41 = (double)sizeF.Height;
        //    float y47 = (float)(num46 + height41) + 8f;
        //    string s44 = "OPENING AMOUNT";
        //    e.Graphics.DrawString(s44, font2, (Brush)solidBrush, new RectangleF(x, y47, width4, height1), format2);
        //    nullable2 = cashcount.OpeningAmt;
        //    Decimal? local30 = nullable2;
        //    string str68;
        //    if (!local30.HasValue)
        //    {
        //        str68 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local30.GetValueOrDefault();
        //        str68 = valueOrDefault2.ToString("N");
        //    }
        //    if (str68 == null)
        //        str68 = "0.00";
        //    string str69 = str68;
        //    e.Graphics.DrawString(str69, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y47, width5, height1), format3);
        //    double num47 = (double)y47;
        //    sizeF = e.Graphics.MeasureString(str69, font2);
        //    double height42 = (double)sizeF.Height;
        //    float y48 = (float)(num47 + height42);
        //    string s45 = "NET SALES";
        //    e.Graphics.DrawString(s45, font2, (Brush)solidBrush, new RectangleF(x, y48, width4, height1), format2);
        //    nullable2 = zreadingByZnoResult.NETSALES;
        //    Decimal? local31 = nullable2;
        //    string str70;
        //    if (!local31.HasValue)
        //    {
        //        str70 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local31.GetValueOrDefault();
        //        str70 = valueOrDefault2.ToString("N");
        //    }
        //    if (str70 == null)
        //        str70 = "0.00";
        //    string str71 = str70;
        //    e.Graphics.DrawString(str71, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y48, width5, height1), format3);
        //    double num48 = (double)y48;
        //    sizeF = e.Graphics.MeasureString(str71, font2);
        //    double height43 = (double)sizeF.Height;
        //    float y49 = (float)(num48 + height43);
        //    spGetCashInout_Result getCashInoutResult1 = this._unitOfWork.CashInOut.GetCashInOut(this._cashCountID).Where<spGetCashInout_Result>((Func<spGetCashInout_Result, bool>)(xy => xy.cashType == "CASH IN")).FirstOrDefault<spGetCashInout_Result>();
        //    string s46 = "CASH IN";
        //    e.Graphics.DrawString(s46, font2, (Brush)solidBrush, new RectangleF(x, y49, width4, height1), format2);
        //    Decimal num49 = 0M;
        //    if (getCashInoutResult1 != null)
        //        num49 = Convert.ToDecimal((object)getCashInoutResult1.amount);
        //    string str72 = num49.ToString("N") ?? "0.00";
        //    e.Graphics.DrawString(str72, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y49, width5, height1), format3);
        //    double num50 = (double)y49;
        //    sizeF = e.Graphics.MeasureString(str72, font2);
        //    double height44 = (double)sizeF.Height;
        //    float y50 = (float)(num50 + height44);
        //    Decimal num51 = Convert.ToDecimal((object)cashcount.OpeningAmt) + Convert.ToDecimal((object)zreadingByZnoResult.NETSALES) + num49;
        //    string s47 = "TOTAL CASH";
        //    e.Graphics.DrawString(s47, font2, (Brush)solidBrush, new RectangleF(x, y50, width4, height1), format2);
        //    string str73 = num51.ToString("N") ?? "0.00";
        //    e.Graphics.DrawString(str73, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y50, width5, height1), format3);
        //    double num52 = (double)y50;
        //    sizeF = e.Graphics.MeasureString(str73, font2);
        //    double height45 = (double)sizeF.Height;
        //    float y51 = (float)(num52 + height45);
        //    string s48 = "CASH COUNT";
        //    e.Graphics.DrawString(s48, font2, (Brush)solidBrush, new RectangleF(x, y51, width4, height1), format2);
        //    nullable2 = cashcount.GrandTotal;
        //    Decimal? local32 = nullable2;
        //    string str74;
        //    if (!local32.HasValue)
        //    {
        //        str74 = (string)null;
        //    }
        //    else
        //    {
        //        valueOrDefault2 = local32.GetValueOrDefault();
        //        str74 = valueOrDefault2.ToString("N");
        //    }
        //    if (str74 == null)
        //        str74 = "0.00";
        //    string str75 = str74;
        //    e.Graphics.DrawString(str75, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y51, width5, height1), format3);
        //    double num53 = (double)y51;
        //    sizeF = e.Graphics.MeasureString(str75, font2);
        //    double height46 = (double)sizeF.Height;
        //    float y52 = (float)(num53 + height46);
        //    spGetCashInout_Result getCashInoutResult2 = this._unitOfWork.CashInOut.GetCashInOut(this._cashCountID).Where<spGetCashInout_Result>((Func<spGetCashInout_Result, bool>)(xy => xy.cashType == "CASH OUT")).FirstOrDefault<spGetCashInout_Result>();
        //    string s49 = "CASH OUT";
        //    e.Graphics.DrawString(s49, font2, (Brush)solidBrush, new RectangleF(x, y52, width4, height1), format2);
        //    Decimal num54 = 0M;
        //    if (getCashInoutResult2 != null)
        //        num54 = Convert.ToDecimal((object)getCashInoutResult2.amount);
        //    string str76 = num54.ToString("N") ?? "0.00";
        //    e.Graphics.DrawString(str76, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y52, width5, height1), format3);
        //    double num55 = (double)y52;
        //    sizeF = e.Graphics.MeasureString(str76, font2);
        //    double height47 = (double)sizeF.Height;
        //    float y53 = (float)(num55 + height47);
        //    Decimal num56 = Convert.ToDecimal((object)cashcount.GrandTotal) + num54 - Convert.ToDecimal(num51);
        //    string s50 = "SHORT/OVER";
        //    e.Graphics.DrawString(s50, font2, (Brush)solidBrush, new RectangleF(x, y53, width4, height1), format2);
        //    string str77 = num56.ToString("N") ?? "0.00";
        //    e.Graphics.DrawString(str77, font2, (Brush)solidBrush, new RectangleF(x + width4 + num31 + num32, y53, width5, height1), format3);
        //    double num57 = (double)y53;
        //    sizeF = e.Graphics.MeasureString(str77, font2);
        //    double height48 = (double)sizeF.Height;
        //    float num58 = (float)(num57 + height48);
        //}



    }
}
