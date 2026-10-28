using POS_88.Database;
using POS_88.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class PrintConfirmView : Form, IPrintConfirmView
    {
        // Per-printer layout values, taken from the old PrintReceiptPageRGM / PrintReceiptPageMASBATE.
        private sealed class ReceiptLayout
        {
            public bool IsRgm;
            public float Width;
            public float TitleSize;
            public float NameW, QtyW, PriceW, AmtW;      // item columns
            public float SumLabelW, SumValueW;           // summary rows
            public int WrapChars;                        // description longer than this -> numbers go on next line
            public string QtyFormat;
            public StringAlignment PriceHeaderAlign;
            public float LogoOffset;
            public float TimeInset;
        }

        private static readonly ReceiptLayout LayoutRgm = new ReceiptLayout
        {
            IsRgm = true,
            Width = 185f,
            TitleSize = 8f,
            NameW = 70f,
            QtyW = 25f,
            PriceW = 45f,
            AmtW = 45f,
            SumLabelW = 100f,
            SumValueW = 85f,
            WrapChars = 14,
            QtyFormat = "0.###",
            PriceHeaderAlign = StringAlignment.Center,
            LogoOffset = 48f,
            TimeInset = 0f
        };

        private static readonly ReceiptLayout LayoutMasbate = new ReceiptLayout
        {
            IsRgm = false,
            Width = 258f,
            TitleSize = 9f,
            NameW = 95f,
            QtyW = 37f,
            PriceW = 56f,
            AmtW = 60f,
            SumLabelW = 165f,
            SumValueW = 80f,
            WrapChars = 16,
            QtyFormat = "#.##",
            PriceHeaderAlign = StringAlignment.Far,
            LogoOffset = 55f,
            TimeInset = 8f
        };

        private readonly UnitOfWorkPOS _unitOfWork;
        private PrintDocument recordDoc;
        private PrintDocument recordBlank;
        private string _OpenType;
        private string _PrinterNm;
        private int _PrinterType;
        private string _storename;
        private string _footer;
        private string _address;
        private string _tinno;
        private string _contactno;
        private bool _isPrint;
        private bool _iswithLogo;
        private bool _isPointSystem;
        private int _receiptNo;

        private static readonly string[] VirtualPrinterNames =
        {
            "Microsoft Print to PDF",
            "Microsoft XPS Document Writer",
            "OneNote",
            "Send To OneNote",
            "Fax",
            "Adobe PDF",
            "CutePDF",
            "PDFCreator",
            "doPDF",
            "Foxit PDF",
            "Bullzip PDF"
        };

        public PrintConfirmView() : this(0, string.Empty, 1)
        {
        }

        public PrintConfirmView(int receiptNo) : this(receiptNo, string.Empty, 1)
        {
        }

        public PrintConfirmView(int receiptNo, string openType, int printerType)
        {
            if (printerType != 1 && printerType != 2)
                throw new ArgumentOutOfRangeException("printerType", "Printer type must be 1 (RGM) or 2 (MASBATE).");

            InitializeComponent();
            _unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            _receiptNo = receiptNo;
            _OpenType = openType ?? string.Empty;
            _isPrint = false;
            _PrinterNm = string.Empty;
            _PrinterType = printerType;

            btnEscOrCancel.Click += btnEscOrCancel_Click;
            btnEnterOrPrint.Click += btnEnterOrPrint_Click;
            KeyPreview = true;
            KeyDown += PrintConfirmView_KeyDown;
            FormClosed += PrintConfirmView_FormClosed;

            if (_receiptNo > 0)
                LoadDetails(_receiptNo);
        }

        public int PrinterType
        {
            get { return _PrinterType; }
            set
            {
                if (value != 1 && value != 2)
                    throw new ArgumentOutOfRangeException("value", "Printer type must be 1 (RGM) or 2 (MASBATE).");

                _PrinterType = value;
                if (_receiptNo > 0)
                    PrintReceiptForTransaction();
            }
        }

        public int ReceiptNo
        {
            get { return _receiptNo; }
            set
            {
                _receiptNo = value;
                if (_receiptNo > 0)
                    LoadDetails(_receiptNo);
            }
        }

        public void LoadDetails(int receiptNo)
        {
            _receiptNo = receiptNo;
            GetData();
            PrintReceiptForTransaction();
            Text = string.Format("Print Receipt ({0})", _receiptNo);
            label1.Text = string.Format("Print Receipt #{0}?", _receiptNo);
        }

        private void GetData()
        {
            var setting = _unitOfWork.Setting
                .Find(x => x.settingsid == 1)
                .FirstOrDefault();

            if (setting == null)
                return;

            _storename = setting.storename ?? string.Empty;
            _address = setting.address ?? string.Empty;
            _footer = setting.footer ?? string.Empty;
            _tinno = setting.tinno ?? string.Empty;
            _contactno = setting.contactno ?? string.Empty;
            _iswithLogo = setting.withLogo.GetValueOrDefault();
            _isPointSystem = setting.enablepointspayment.GetValueOrDefault();
            _PrinterNm = string.IsNullOrWhiteSpace(setting.printername) ? string.Empty : setting.printername;
        }

        public void PrintReceiptForTransaction()
        {
            if (_receiptNo <= 0)
                return;

            if (recordDoc != null)
            {
                if (printPreviewControl1.Document == recordDoc)
                    printPreviewControl1.Document = null;
                recordDoc.Dispose();
            }
            if (recordBlank != null)
                recordBlank.Dispose();

            recordDoc = new PrintDocument();
            recordBlank = new PrintDocument();
            recordDoc.DocumentName = "Customer Receipt";
            recordBlank.DocumentName = "Opening Cash Drawer";

            // Same as the old code: KITCHEN prints the same receipt layout as normal receipts.
            if (_PrinterType == 1)
                recordDoc.PrintPage += PrintReceiptPageRGM;
            else
                recordDoc.PrintPage += PrintReceiptPageMASBATE;

            recordBlank.PrintPage += PrintReceiptBlank;

            //string printerName = ResolvePrinterName();
            //if (!string.IsNullOrEmpty(printerName))
            //{
            //    var printerSettings = new PrinterSettings();
            //    printerSettings.PrinterName = printerName;
            //    recordDoc.PrinterSettings = printerSettings;
            //    recordBlank.PrinterSettings = printerSettings;
            //}
            string printerName = ResolvePrinterName();
            if (!string.IsNullOrEmpty(printerName))
            {
                var printerSettings = new PrinterSettings { PrinterName = printerName };
                if (printerSettings.IsValid)
                {
                    recordDoc.PrinterSettings = printerSettings;
                    recordBlank.PrinterSettings = printerSettings;
                }
            }

            if (printPreviewControl1 != null)
            {
                printPreviewControl1.Document = recordDoc;
                printPreviewControl1.AutoZoom = false;
                printPreviewControl1.UseAntiAlias = true;
                printPreviewControl1.Zoom = 1.5;
                printPreviewControl1.StartPage = 0;
                printPreviewControl1.InvalidatePreview();
                printPreviewControl1.Refresh();
            }
        }

        // Virtual printers (Microsoft Print to PDF, XPS Document Writer, etc.) pop up a "Save as" dialog.
        //private static bool IsVirtualPrinter(string name)
        //{
        //    return !string.IsNullOrEmpty(name) &&
        //           (name.IndexOf("pdf", StringComparison.OrdinalIgnoreCase) >= 0 ||
        //            name.IndexOf("xps", StringComparison.OrdinalIgnoreCase) >= 0 ||
        //            name.IndexOf("onenote", StringComparison.OrdinalIgnoreCase) >= 0);
        //}
        private static bool IsVirtualPrinter(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return name.IndexOf("XPS Document Writer", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   name.IndexOf("OneNote", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   name.IndexOf("Fax", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // Configured printer -> Windows default printer -> first physical printer. Never a PDF printer.
        private string ResolvePrinterName()
        {
            if (!string.IsNullOrWhiteSpace(_PrinterNm) && !IsVirtualPrinter(_PrinterNm))
                return _PrinterNm;

            string defaultName = new PrinterSettings().PrinterName;
            if (!string.IsNullOrEmpty(defaultName) && !IsVirtualPrinter(defaultName))
                return defaultName;

            foreach (string name in PrinterSettings.InstalledPrinters)
            {
                if (!IsVirtualPrinter(name))
                    return name;
            }

            return _PrinterNm;
        }

        //private bool HasPhysicalPrinter(PrintDocument doc)
        //{
        //    return doc != null && !IsVirtualPrinter(doc.PrinterSettings.PrinterName);
        //}
        private bool HasPhysicalPrinter(PrintDocument doc)
        {
            return doc != null
                && doc.PrinterSettings.IsValid
                && !IsVirtualPrinter(doc.PrinterSettings.PrinterName);
        }

        private void PrintReceiptPageRGM(object sender, PrintPageEventArgs e)
        {
            RenderReceipt(e, LayoutRgm);
        }

        private void PrintReceiptPageMASBATE(object sender, PrintPageEventArgs e)
        {
            RenderReceipt(e, LayoutMasbate);
        }

        private void PrintReceiptBlank(object sender, PrintPageEventArgs e)
        {
            using (var font = new Font("Tahoma", 8f, FontStyle.Bold))
                e.Graphics.DrawString(string.Empty, font, Brushes.Black, new RectangleF(2f, 5f, 185f, 0f));
        }

        // ------------------------------------------------------------------
        // Receipt rendering (follows the old RGM / MASBATE layouts)
        // ------------------------------------------------------------------
        private void RenderReceipt(PrintPageEventArgs e, ReceiptLayout L)
        {
            Graphics g = e.Graphics;
            const float x = 2f;
            float w = L.Width;
            float y = 5f;

            using (var titleFont = new Font("Tahoma", L.TitleSize, FontStyle.Bold))
            using (var font = new Font("Tahoma", 7f, FontStyle.Regular))
            using (var boldFont = new Font("Tahoma", 8f, FontStyle.Bold))
            using (var pen = new Pen(Color.Black, 1f))
            {
                pen.DashStyle = DashStyle.Dash;
                Font totalFont = L.IsRgm ? boldFont : font;   // old RGM printed TOTAL in bold

                var invoice = _unitOfWork.Invoice
                    .Find(item => item.receiptno == _receiptNo)
                    .FirstOrDefault();

                if (invoice == null)
                {
                    DrawText(g, "No receipt found.", font, x, y, w, StringAlignment.Near);
                    return;
                }

                var items = _unitOfWork.ItemSales
                    .Find(item => item.receiptno == _receiptNo)
                    .ToList();

                var cashier = _unitOfWork.User
                    .Find(u => u.usernm == invoice.crtdby)
                    .FirstOrDefault();

                // ---------- Logo ----------
                if (_iswithLogo)
                {
                    string logoPath = Path.Combine(Application.StartupPath, "receiptlogo.jpg");
                    if (File.Exists(logoPath))
                    {
                        using (var image = Image.FromFile(logoPath))
                            g.DrawImage(image, (e.PageBounds.Width - image.Width) / 2f, y, image.Width, image.Height);
                    }
                    y += L.LogoOffset;
                }

                // ---------- Header ----------
                y += DrawText(g, _storename, titleFont, x, y, w, StringAlignment.Center);
                y += DrawText(g, _address, font, x, y, w, StringAlignment.Center);
                if (L.IsRgm)
                {
                    y += DrawText(g, "TIN No.: " + _tinno, font, x, y, w, StringAlignment.Center);
                    y += DrawText(g, "Contact No: " + _contactno, font, x, y, w, StringAlignment.Center);
                }
                y += 10f;
                DrawDash(g, pen, x, y, w);
                y += 10f;

                // ---------- Receipt info ----------
                if (L.IsRgm)
                {
                    var customer = _unitOfWork.Customers
                        .Find(c => c.customerid == invoice.customerid)
                        .FirstOrDefault();

                    string customerName = (customer == null || string.IsNullOrWhiteSpace(customer.customername))
                        ? "WALK-IN"
                        : customer.customername;

                    y += DrawText(g, "RECEIPT NO. : " + invoice.receiptno.ToString(), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "CUSTOMER: " + customerName, font, x, y, w, StringAlignment.Near);
                    if (customer != null && customerName != "WALK-IN")
                        y += DrawText(g, "ADDRESS: " + (customer.address ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "DRIVER: " + (invoice.drivername ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "CASHIER: " + (cashier == null ? string.Empty : cashier.nameuser ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "PAYMENT TYPE: " + (invoice.paymenttype ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "TABLE #: " + (invoice.tableno ?? string.Empty), font, x, y, w, StringAlignment.Near);
                }
                else
                {
                    y += DrawText(g, "RECEIPT NO. : " + invoice.receiptno.ToString("00000#"), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "CASHIER: " + (cashier == null ? string.Empty : cashier.nameuser ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "PAYMENT TYPE: " + (invoice.paymenttype ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    if (!string.IsNullOrEmpty(invoice.tableno))
                        y += DrawText(g, "TABLE #: " + invoice.tableno, font, x, y, w, StringAlignment.Near);
                }

                string dateText = "DATE: " + (invoice.solddate.HasValue ? invoice.solddate.Value.ToString("M/d/yyyy") : string.Empty);
                string timeText = "TIME : " + (invoice.solddate.HasValue ? invoice.solddate.Value.ToString("hh:mm tt") : string.Empty);
                DrawText(g, dateText, font, x, y, w, StringAlignment.Near);
                DrawText(g, timeText, font, x, y, w - L.TimeInset, StringAlignment.Far);
                y += font.GetHeight(g) + 12f;
                DrawDash(g, pen, x, y, w);
                y += 8f;

                // ---------- Item header ----------
                float qtyX = x + L.NameW;
                float priceX = qtyX + L.QtyW;
                float amtX = priceX + L.PriceW;

                DrawText(g, "NAME", font, x, y, L.NameW, StringAlignment.Near);
                DrawText(g, "QTY", font, qtyX, y, L.QtyW, StringAlignment.Center);
                DrawText(g, "PRICE", font, priceX, y, L.PriceW, L.PriceHeaderAlign);
                DrawText(g, "AMOUNT", font, amtX, y, L.AmtW, StringAlignment.Far);
                y += font.GetHeight(g) + 8f;
                DrawDash(g, pen, x, y, w);
                y += 8f;

                // ---------- Items ----------
                foreach (var item in items)
                {
                    string name = item.description ?? string.Empty;
                    if (L.IsRgm && !string.IsNullOrWhiteSpace(item.note))
                        name += " " + item.note;      // old RGM printed "description note"

                    float nameHeight = DrawText(g, name, font, x, y, w, StringAlignment.Near);
                    if (name.Length > L.WrapChars)
                        y += nameHeight;              // long name: numbers go on the next line

                    DrawText(g, item.quantity.HasValue ? item.quantity.Value.ToString(L.QtyFormat) : "0",
                        font, qtyX, y, L.QtyW, StringAlignment.Center);
                    DrawText(g, Money(item.price), font, priceX, y, L.PriceW, StringAlignment.Far);
                    DrawText(g, Money(item.totaldue), font, amtX, y, L.AmtW, StringAlignment.Far);
                    y += font.GetHeight(g);
                }

                y += 8f;
                DrawDash(g, pen, x, y, w);

                // ---------- NO OF ITEMS (RGM only) ----------
                if (L.IsRgm)
                {
                    y += 6f;
                    y += DrawPair(g, "NO OF ITEMS", Money(invoice.noofitems), font, x, y, L.SumLabelW, L.SumValueW);
                    y += 6f;
                    DrawDash(g, pen, x, y, w);
                }

                // ---------- Totals ----------
                y += 8f;
                y += DrawPair(g, "SUBTOTAL", Money(invoice.subtotal), font, x, y, L.SumLabelW, L.SumValueW);
                y += DrawPair(g, "DISCOUNT", Money(invoice.subdiscount), font, x, y, L.SumLabelW, L.SumValueW);
                y += DrawPair(g, "REDEEMED", Money(invoice.loyaltyamt), font, x, y, L.SumLabelW, L.SumValueW);
                y += DrawPair(g, "TOTAL", Money(invoice.amountdue), totalFont, x, y, L.SumLabelW, L.SumValueW);
                y += 8f;
                DrawDash(g, pen, x, y, w);
                y += 8f;

                y += DrawPair(g, "CASH", Money(invoice.cashtender), font, x, y, L.SumLabelW, L.SumValueW);
                if (L.IsRgm)
                    y += DrawPair(g, "BALANCE", Money(invoice.balance), font, x, y, L.SumLabelW, L.SumValueW);
                y += DrawPair(g, "CHANGE", Money(invoice.changeamt), font, x, y, L.SumLabelW, L.SumValueW);
                y += 8f;
                DrawDash(g, pen, x, y, w);

                // ---------- VAT breakdown (RGM only) ----------
                if (L.IsRgm)
                {
                    y += 8f;
                    DrawText(g, "NET", font, qtyX, y, L.PriceW, StringAlignment.Far);                 // x + 95, width 45
                    DrawText(g, "VAT", font, x + L.SumLabelW, y, L.SumValueW, StringAlignment.Far);   // x + 100, width 85
                    y += font.GetHeight(g);
                    y += DrawVatRow(g, "VATABLE", invoice.vatsales, invoice.vatamount, font, x, y, L, qtyX);
                    y += DrawVatRow(g, "VAT EXEMPT", invoice.vatexemptsales, null, font, x, y, L, qtyX);
                    y += DrawVatRow(g, "VAT ZERO-RATED", invoice.zeroratedsales, null, font, x, y, L, qtyX);
                    y += 8f;
                    DrawDash(g, pen, x, y, w);
                }

                y += 10f;

                // ---------- Loyalty points ----------
                if (invoice.customerid.GetValueOrDefault() > 1 && _isPointSystem)
                {
                    y += DrawText(g, "LOYALTY POINTS", font, x, y, w, StringAlignment.Center);
                    y += DrawPair(g, "Earned", Points(invoice.earned), font, x, y, L.SumLabelW, L.SumValueW);
                    y += DrawPair(g, "Previous Balance", Points(invoice.previouspoints), font, x, y, L.SumLabelW, L.SumValueW);
                    y += DrawPair(g, "Current Balance", Points(invoice.currentpoints), font, x, y, L.SumLabelW, L.SumValueW);

                    string redeemed = invoice.pointsRedeem.HasValue
                        ? invoice.pointsRedeem.Value.ToString("0.##")
                        : "0 (" + Money(invoice.loyaltyamt) + ")";
                    y += DrawPair(g, "Redeemed", redeemed, font, x, y, L.SumLabelW, L.SumValueW);
                    y += 8f;
                    DrawDash(g, pen, x, y, w);
                    y += 10f;
                }

                // ---------- Note (RGM only) ----------
                if (L.IsRgm && !string.IsNullOrEmpty(invoice.note))
                {
                    y += DrawText(g, "NOTE: " + invoice.note, font, x, y, w, StringAlignment.Center);
                    y += 8f;
                    DrawDash(g, pen, x, y, w);
                    y += 10f;
                }

                // ---------- Footer ----------
                if (!L.IsRgm)
                    y += 20f;
                DrawText(g, _footer, font, x, y, w, StringAlignment.Center);
            }
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------
        private static string Money(decimal? value)
        {
            return (value ?? 0m).ToString("N2");
        }

        private static string Points(decimal? value)
        {
            return (value ?? 0m).ToString("0.##");
        }

        private static void DrawDash(Graphics graphics, Pen pen, float x, float y, float width)
        {
            graphics.DrawLine(pen, x, y, width, y);
        }

        private static float DrawPair(Graphics graphics, string label, string value, Font font,
            float x, float y, float labelWidth, float valueWidth)
        {
            float h = DrawText(graphics, label, font, x, y, labelWidth, StringAlignment.Near);
            h = Math.Max(h, DrawText(graphics, value, font, x + labelWidth, y, valueWidth, StringAlignment.Far));
            return h;
        }

        private static float DrawVatRow(Graphics graphics, string label, decimal? net, decimal? vat, Font font,
            float x, float y, ReceiptLayout L, float netX)
        {
            float h = DrawText(graphics, label, font, x, y, L.SumLabelW, StringAlignment.Near);
            h = Math.Max(h, DrawText(graphics, Money(net), font, netX, y, L.PriceW, StringAlignment.Far));
            h = Math.Max(h, DrawText(graphics, Money(vat), font, x + L.SumLabelW, y, L.SumValueW, StringAlignment.Far));
            return h;
        }

        private static float DrawText(Graphics graphics, string text, Font font, float x, float y, float width, StringAlignment alignment)
        {
            if (string.IsNullOrEmpty(text))
                return 0f;

            using (var format = new StringFormat())
            {
                format.Alignment = alignment;
                format.LineAlignment = StringAlignment.Near;
                format.Trimming = StringTrimming.Word;
                SizeF measured = graphics.MeasureString(text, font, new SizeF(width, 1000f), format);
                float height = Math.Max(font.GetHeight(graphics), measured.Height);
                graphics.DrawString(text, font, Brushes.Black, new RectangleF(x, y, width, height), format);
                return height;
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
            if (_receiptNo <= 0)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (!HasPhysicalPrinter(recordDoc))
            {
                MessageBox.Show("No receipt printer found. Please set a printer in Settings.", "Print Receipt", // Why it always shows this error? can you fix?
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                recordDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Print Receipt",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isPrint = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnEscOrCancel_Click(object sender, EventArgs e)
        {
            if (!_isPrint && _OpenType != "PURCHASED" && _OpenType != "KITCHEN" && HasPhysicalPrinter(recordBlank))
            {
                try
                {
                    recordBlank.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Opening Cash Drawer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //MessageBox.Show("Warning: Printing is currently suspended due to a backlog; please contact IT or the system administrator for assistance.", "Opening Cash Drawer",
                    //MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                _isPrint = true;
            }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PrintConfirmView_KeyDown(object sender, KeyEventArgs e)
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

        private void PrintConfirmView_FormClosed(object sender, FormClosedEventArgs e)
        {
            printPreviewControl1.Document = null;
            if (recordDoc != null)
                recordDoc.Dispose();
            if (recordBlank != null)
                recordBlank.Dispose();
            _unitOfWork.Dispose();
        }
    }
}