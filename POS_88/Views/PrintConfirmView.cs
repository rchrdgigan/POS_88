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
    public partial class PrintConfirmView : Form, IPrintConfirmView
    {
        #region Layouts

        // Per-printer layout values (RGM = 58mm style, MASBATE = wider paper).
        private sealed class ReceiptLayout
        {
            public bool IsRgm;
            public float Width;
            public float TitleSize;
            public float NameW, QtyW, PriceW, AmtW;   // item columns
            public float SumLabelW, SumValueW;        // summary rows
            public int WrapChars;                     // longer descriptions push the numbers to the next line
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

        #endregion

        #region Fields

        private readonly UnitOfWorkPOS _unitOfWork;
        private readonly CashierView _cashierView;
        private readonly string _openType;

        private PrintDocument _receiptDoc;
        private PrintDocument _drawerDoc;      // blank page used to pop the cash drawer

        private int _receiptNo;
        private int _printerType;
        private string _printerName;

        private string _storeName;
        private string _address;
        private string _tinNo;
        private string _contactNo;
        private string _footer;
        private bool _withLogo;
        private bool _pointSystem;

        private bool _isPrinted;
        private bool _keyHandled;

        #endregion

        #region Construction

        public PrintConfirmView() : this(0, string.Empty, 1, null) { }

        public PrintConfirmView(int receiptNo) : this(receiptNo, string.Empty, 1, null) { }

        public PrintConfirmView(int receiptNo, string openType, int printerType)
            : this(receiptNo, openType, printerType, null)
        { }

        public PrintConfirmView(int receiptNo, string openType, int printerType, CashierView cashierView)
        {
            if (!IsValidPrinterType(printerType))
                throw new ArgumentOutOfRangeException("printerType", "Printer type must be 1 (RGM) or 2 (MASBATE).");

            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Fuchsia;
            TransparencyKey = Color.Fuchsia;
            StartPosition = FormStartPosition.CenterParent;

            _unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            _receiptNo = receiptNo;
            _cashierView = cashierView;
            _openType = openType ?? string.Empty;
            _printerType = printerType;
            _printerName = string.Empty;

            btnEscOrCancel.Click += btnEscOrCancel_Click;
            btnEnterOrPrint.Click += btnEnterOrPrint_Click;
            FormClosed += PrintConfirmView_FormClosed;

            if (_receiptNo > 0)
                LoadDetails(_receiptNo);
        }

        private static bool IsValidPrinterType(int printerType)
        {
            return printerType == 1 || printerType == 2;
        }

        #endregion

        #region Public API (IPrintConfirmView)

        public int PrinterType
        {
            get { return _printerType; }
            set
            {
                if (!IsValidPrinterType(value))
                    throw new ArgumentOutOfRangeException("value", "Printer type must be 1 (RGM) or 2 (MASBATE).");

                _printerType = value;
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

        public void PrintReceiptForTransaction()
        {
            if (_receiptNo <= 0)
                return;

            DisposeDocuments();

            _receiptDoc = new PrintDocument { DocumentName = "Customer Receipt" };
            _drawerDoc = new PrintDocument { DocumentName = "Opening Cash Drawer" };

            // KITCHEN prints the same layout as a normal receipt.
            if (_printerType == 1)
                _receiptDoc.PrintPage += PrintReceiptPageRGM;
            else
                _receiptDoc.PrintPage += PrintReceiptPageMASBATE;

            _drawerDoc.PrintPage += PrintReceiptBlank;

            string printerName = ResolvePrinterName();
            if (!string.IsNullOrEmpty(printerName))
            {
                var printerSettings = new PrinterSettings { PrinterName = printerName };
                if (printerSettings.IsValid)
                {
                    _receiptDoc.PrinterSettings = printerSettings;
                    _drawerDoc.PrinterSettings = printerSettings;
                }
            }

            if (printPreviewControl1 != null)
            {
                printPreviewControl1.Document = _receiptDoc;
                printPreviewControl1.AutoZoom = false;
                printPreviewControl1.UseAntiAlias = true;
                printPreviewControl1.Zoom = 1.5;
                printPreviewControl1.StartPage = 0;
                printPreviewControl1.InvalidatePreview();
                printPreviewControl1.Refresh();
            }
        }

        #endregion

        #region Data / printer resolution

        private void GetData()
        {
            var setting = _unitOfWork.Setting
                .Find(x => x.settingsid == 1)
                .FirstOrDefault();

            if (setting == null)
                return;

            _storeName = setting.storename ?? string.Empty;
            _address = setting.address ?? string.Empty;
            _footer = setting.footer ?? string.Empty;
            _tinNo = setting.tinno ?? string.Empty;
            _contactNo = setting.contactno ?? string.Empty;
            _withLogo = setting.withLogo.GetValueOrDefault();
            _pointSystem = setting.enablepointspayment.GetValueOrDefault();
            _printerName = string.IsNullOrWhiteSpace(setting.printername) ? string.Empty : setting.printername;
        }

        // Printers that are not usable for receipts. (PDF printers are allowed on purpose.)
        private static bool IsVirtualPrinter(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return name.IndexOf("XPS Document Writer", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   name.IndexOf("OneNote", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   name.IndexOf("Fax", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // Configured printer -> Windows default printer -> first non-virtual printer.
        private string ResolvePrinterName()
        {
            if (!string.IsNullOrWhiteSpace(_printerName) && !IsVirtualPrinter(_printerName))
                return _printerName;

            string defaultName = new PrinterSettings().PrinterName;
            if (!string.IsNullOrEmpty(defaultName) && !IsVirtualPrinter(defaultName))
                return defaultName;

            foreach (string name in PrinterSettings.InstalledPrinters)
            {
                if (!IsVirtualPrinter(name))
                    return name;
            }

            return _printerName;
        }

        private static bool HasPhysicalPrinter(PrintDocument doc)
        {
            return doc != null
                && doc.PrinterSettings.IsValid
                && !IsVirtualPrinter(doc.PrinterSettings.PrinterName);
        }

        private void DisposeDocuments()
        {
            if (printPreviewControl1 != null &&
                (printPreviewControl1.Document == _receiptDoc || printPreviewControl1.Document == _drawerDoc))
                printPreviewControl1.Document = null;

            if (_receiptDoc != null)
            {
                _receiptDoc.Dispose();
                _receiptDoc = null;
            }
            if (_drawerDoc != null)
            {
                _drawerDoc.Dispose();
                _drawerDoc = null;
            }
        }

        private void ClearCashierEntries()
        {
            if (_openType != "PURCHASED" && _openType != "KITCHEN" && _cashierView != null)
                _cashierView.ClearEntries();
        }

        #endregion

        #region Page handlers

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

        #endregion

        #region Receipt rendering

        private void RenderReceipt(PrintPageEventArgs e, ReceiptLayout layout)
        {
            Graphics g = e.Graphics;
            const float x = 2f;
            float w = layout.Width;
            float y = 5f;

            using (var titleFont = new Font("Tahoma", layout.TitleSize, FontStyle.Bold))
            using (var font = new Font("Tahoma", 7f, FontStyle.Regular))
            using (var boldFont = new Font("Tahoma", 8f, FontStyle.Bold))
            using (var pen = new Pen(Color.Black, 1f) { DashStyle = DashStyle.Dash })
            {
                Font totalFont = layout.IsRgm ? boldFont : font;   // RGM prints TOTAL in bold

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

                string cashierName = cashier == null ? string.Empty : cashier.nameuser ?? string.Empty;

                // ---------- Logo ----------
                if (_withLogo)
                {
                    string logoPath = Path.Combine(Application.StartupPath, "receiptlogo.jpg");
                    if (File.Exists(logoPath))
                    {
                        using (var image = Image.FromFile(logoPath))
                            g.DrawImage(image, (e.PageBounds.Width - image.Width) / 2f, y, image.Width, image.Height);
                    }
                    y += layout.LogoOffset;
                }

                // ---------- Header ----------
                y += DrawText(g, _storeName, titleFont, x, y, w, StringAlignment.Center);
                y += DrawText(g, _address, font, x, y, w, StringAlignment.Center);
                if (layout.IsRgm)
                {
                    y += DrawText(g, "TIN No.: " + _tinNo, font, x, y, w, StringAlignment.Center);
                    y += DrawText(g, "Contact No: " + _contactNo, font, x, y, w, StringAlignment.Center);
                }
                y = DrawSeparator(g, pen, x, y, w, 10f, 10f);

                // ---------- Receipt info ----------
                if (layout.IsRgm)
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
                    y += DrawText(g, "CASHIER: " + cashierName, font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "PAYMENT TYPE: " + (invoice.paymenttype ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "TABLE #: " + (invoice.tableno ?? string.Empty), font, x, y, w, StringAlignment.Near);
                }
                else
                {
                    y += DrawText(g, "RECEIPT NO. : " + invoice.receiptno.ToString("00000#"), font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "CASHIER: " + cashierName, font, x, y, w, StringAlignment.Near);
                    y += DrawText(g, "PAYMENT TYPE: " + (invoice.paymenttype ?? string.Empty), font, x, y, w, StringAlignment.Near);
                    if (!string.IsNullOrEmpty(invoice.tableno))
                        y += DrawText(g, "TABLE #: " + invoice.tableno, font, x, y, w, StringAlignment.Near);
                }

                string dateText = "DATE: " + (invoice.solddate.HasValue ? invoice.solddate.Value.ToString("M/d/yyyy") : string.Empty);
                string timeText = "TIME : " + (invoice.solddate.HasValue ? invoice.solddate.Value.ToString("hh:mm tt") : string.Empty);
                DrawText(g, dateText, font, x, y, w, StringAlignment.Near);
                DrawText(g, timeText, font, x, y, w - layout.TimeInset, StringAlignment.Far);
                y += font.GetHeight(g);
                y = DrawSeparator(g, pen, x, y, w, 12f, 8f);

                // ---------- Item header ----------
                float qtyX = x + layout.NameW;
                float priceX = qtyX + layout.QtyW;
                float amtX = priceX + layout.PriceW;

                DrawText(g, "NAME", font, x, y, layout.NameW, StringAlignment.Near);
                DrawText(g, "QTY", font, qtyX, y, layout.QtyW, StringAlignment.Center);
                DrawText(g, "PRICE", font, priceX, y, layout.PriceW, layout.PriceHeaderAlign);
                DrawText(g, "AMOUNT", font, amtX, y, layout.AmtW, StringAlignment.Far);
                y += font.GetHeight(g);
                y = DrawSeparator(g, pen, x, y, w, 8f, 8f);

                // ---------- Items ----------
                foreach (var item in items)
                {
                    string name = item.description ?? string.Empty;
                    if (layout.IsRgm && !string.IsNullOrWhiteSpace(item.note))
                        name += " " + item.note;

                    float nameHeight = DrawText(g, name, font, x, y, w, StringAlignment.Near);
                    if (name.Length > layout.WrapChars)
                        y += nameHeight;   // long name: numbers go on the next line

                    string qty = item.quantity.HasValue ? item.quantity.Value.ToString(layout.QtyFormat) : "0";
                    DrawText(g, qty, font, qtyX, y, layout.QtyW, StringAlignment.Center);
                    DrawText(g, Money(item.price), font, priceX, y, layout.PriceW, StringAlignment.Far);
                    DrawText(g, Money(item.totaldue), font, amtX, y, layout.AmtW, StringAlignment.Far);
                    y += font.GetHeight(g);
                }

                y = DrawSeparator(g, pen, x, y, w, 8f, layout.IsRgm ? 6f : 8f);

                // ---------- NO OF ITEMS (RGM only) ----------
                if (layout.IsRgm)
                {
                    y += DrawPair(g, "NO OF ITEMS", Money(invoice.noofitems), font, x, y, layout.SumLabelW, layout.SumValueW);
                    y = DrawSeparator(g, pen, x, y, w, 6f, 8f);
                }

                // ---------- Totals ----------
                y += DrawPair(g, "SUBTOTAL", Money(invoice.subtotal), font, x, y, layout.SumLabelW, layout.SumValueW);
                y += DrawPair(g, "DISCOUNT", Money(invoice.subdiscount), font, x, y, layout.SumLabelW, layout.SumValueW);
                y += DrawPair(g, "REDEEMED", Money(invoice.loyaltyamt), font, x, y, layout.SumLabelW, layout.SumValueW);
                y += DrawPair(g, "TOTAL", Money(invoice.amountdue), totalFont, x, y, layout.SumLabelW, layout.SumValueW);
                y = DrawSeparator(g, pen, x, y, w, 8f, 8f);

                y += DrawPair(g, "CASH", Money(invoice.cashtender), font, x, y, layout.SumLabelW, layout.SumValueW);
                if (layout.IsRgm)
                    y += DrawPair(g, "BALANCE", Money(invoice.balance), font, x, y, layout.SumLabelW, layout.SumValueW);
                y += DrawPair(g, "CHANGE", Money(invoice.changeamt), font, x, y, layout.SumLabelW, layout.SumValueW);
                y = DrawSeparator(g, pen, x, y, w, 8f, layout.IsRgm ? 8f : 0f);

                // ---------- VAT breakdown (RGM only) ----------
                if (layout.IsRgm)
                {
                    DrawText(g, "NET", font, qtyX, y, layout.PriceW, StringAlignment.Far);
                    DrawText(g, "VAT", font, x + layout.SumLabelW, y, layout.SumValueW, StringAlignment.Far);
                    y += font.GetHeight(g);
                    y += DrawVatRow(g, "VATABLE", invoice.vatsales, invoice.vatamount, font, x, y, layout, qtyX);
                    y += DrawVatRow(g, "VAT EXEMPT", invoice.vatexemptsales, null, font, x, y, layout, qtyX);
                    y += DrawVatRow(g, "VAT ZERO-RATED", invoice.zeroratedsales, null, font, x, y, layout, qtyX);
                    y = DrawSeparator(g, pen, x, y, w, 8f, 0f);
                }

                y += 10f;

                // ---------- Loyalty points ----------
                if (invoice.customerid.GetValueOrDefault() > 1 && _pointSystem)
                {
                    y += DrawText(g, "LOYALTY POINTS", font, x, y, w, StringAlignment.Center);
                    y += DrawPair(g, "Earned", Points(invoice.earned), font, x, y, layout.SumLabelW, layout.SumValueW);
                    y += DrawPair(g, "Previous Balance", Points(invoice.previouspoints), font, x, y, layout.SumLabelW, layout.SumValueW);
                    y += DrawPair(g, "Current Balance", Points(invoice.currentpoints), font, x, y, layout.SumLabelW, layout.SumValueW);

                    string redeemed = invoice.pointsRedeem.HasValue
                        ? invoice.pointsRedeem.Value.ToString("0.##")
                        : "0 (" + Money(invoice.loyaltyamt) + ")";
                    y += DrawPair(g, "Redeemed", redeemed, font, x, y, layout.SumLabelW, layout.SumValueW);
                    y = DrawSeparator(g, pen, x, y, w, 8f, 10f);
                }

                // ---------- Note (RGM only) ----------
                if (layout.IsRgm && !string.IsNullOrEmpty(invoice.note))
                {
                    y += DrawText(g, "NOTE: " + invoice.note, font, x, y, w, StringAlignment.Center);
                    y = DrawSeparator(g, pen, x, y, w, 8f, 10f);
                }

                // ---------- Footer ----------
                if (!layout.IsRgm)
                    y += 20f;
                DrawText(g, _footer, font, x, y, w, StringAlignment.Center);
            }
        }

        #endregion

        #region Drawing helpers

        private static string Money(decimal? value)
        {
            return (value ?? 0m).ToString("N2");
        }

        private static string Points(decimal? value)
        {
            return (value ?? 0m).ToString("0.##");
        }

        // Adds gapBefore, draws a dashed line from x to endX, and returns the y after gapAfter.
        private static float DrawSeparator(Graphics g, Pen pen, float x, float y, float endX, float gapBefore, float gapAfter)
        {
            y += gapBefore;
            g.DrawLine(pen, x, y, endX, y);
            return y + gapAfter;
        }

        private static float DrawPair(Graphics g, string label, string value, Font font,
            float x, float y, float labelWidth, float valueWidth)
        {
            float h = DrawText(g, label, font, x, y, labelWidth, StringAlignment.Near);
            return Math.Max(h, DrawText(g, value, font, x + labelWidth, y, valueWidth, StringAlignment.Far));
        }

        private static float DrawVatRow(Graphics g, string label, decimal? net, decimal? vat, Font font,
            float x, float y, ReceiptLayout layout, float netX)
        {
            float h = DrawText(g, label, font, x, y, layout.SumLabelW, StringAlignment.Near);
            h = Math.Max(h, DrawText(g, Money(net), font, netX, y, layout.PriceW, StringAlignment.Far));
            return Math.Max(h, DrawText(g, Money(vat), font, x + layout.SumLabelW, y, layout.SumValueW, StringAlignment.Far));
        }

        // Draws wrapped text and returns the height it used.
        private static float DrawText(Graphics g, string text, Font font, float x, float y, float width, StringAlignment alignment)
        {
            if (string.IsNullOrEmpty(text))
                return 0f;

            using (var format = new StringFormat())
            {
                format.Alignment = alignment;
                format.LineAlignment = StringAlignment.Near;
                format.Trimming = StringTrimming.Word;

                SizeF measured = g.MeasureString(text, font, new SizeF(width, 1000f), format);
                float height = Math.Max(font.GetHeight(g), measured.Height);
                g.DrawString(text, font, Brushes.Black, new RectangleF(x, y, width, height), format);
                return height;
            }
        }

        #endregion

        #region Form behaviour

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.Return)
            {
                // Bit 30 of lParam is set while the key is auto-repeating (held down).
                bool isRepeat = (msg.LParam.ToInt64() & 0x40000000) != 0;
                if (isRepeat || _keyHandled)
                    return true;

                _keyHandled = true;

                if (keyData == Keys.Escape)
                    btnEscOrCancel_Click(this, EventArgs.Empty);
                else
                    btnEnterOrPrint_Click(this, EventArgs.Empty);

                // The form stayed open (e.g. "No receipt printer found"): allow another try.
                if (!IsDisposed && DialogResult == DialogResult.None)
                    _keyHandled = false;

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

            if (!HasPhysicalPrinter(_receiptDoc))
            {
                MessageBox.Show("No receipt printer found. Please set a printer in Settings.", "Print Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _receiptDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _isPrinted = true;
            ClearCashierEntries();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnEscOrCancel_Click(object sender, EventArgs e)
        {
            bool shouldOpenDrawer = !_isPrinted
                && _openType != "PURCHASED"
                && _openType != "KITCHEN"
                && HasPhysicalPrinter(_drawerDoc);

            if (shouldOpenDrawer)
            {
                try
                {
                    _drawerDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Opening Cash Drawer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _isPrinted = true;
            }

            ClearCashierEntries();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Kept in case the Designer wires it up; ProcessCmdKey already handles Esc/Enter.
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
            DisposeDocuments();
            _unitOfWork.Dispose();
        }

        #endregion

        private void PrintConfirmView_Load(object sender, EventArgs e)
        {

        }
    }
}