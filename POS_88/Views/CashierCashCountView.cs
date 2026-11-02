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
using POS_88.Properties;
using System.Data.Entity;
using System.Globalization;

namespace POS_88.Views
{
    public partial class CashierCashCountView : Form
    {

        private string _UserNm;
        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        private POSDBEntities _context;
        private int _cashCountID;

        public CashierCashCountView()
        {
            InitializeComponent();
        }

        public CashierCashCountView(string userNm)
        {
            this.InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
            this._context = new POSDBEntities();
            this._UserNm = userNm;
            ApplyTheme();

            btnClose.Click += btnClose_Click;
            btnShift.Click += btnShift_Click;
            btnSave.Click += btnSave_Click;
            btnPrint.Click += btnPrint_Click;
            btnCashHistory.Click += btnCashHistory_Click;
            btnCashInOut.Click += btnCashInOut_Click;

            txtOpeningAmt.TextChanged += txtOpeningAmt_TextChanged;
            txt5CentQty.TextChanged += txt5CentQty_TextChanged;
            txt10CentQty.TextChanged += txt10CentQty_TextChanged;
            txt25CentQty.TextChanged += txt25CentQty_TextChanged;
            txt1PesoQty.TextChanged += txt1PesoQty_TextChanged;
            txt5PesoQty.TextChanged += txt5PesoQty_TextChanged;
            txt10PesoQty.TextChanged += txt10PesoQty_TextChanged;
            txt20PesoQty.TextChanged += txt20PesoQty_TextChanged;
            txt20BillQty.TextChanged += txt20BillQty_TextChanged;
            txt50BillQty.TextChanged += txt50BillQty_TextChanged;
            txt100BillQty.TextChanged += txt100BillQty_TextChanged;
            txt200BillQty.TextChanged += txt200BillQty_TextChanged;
            txt500BillQty.TextChanged += txt500BillQty_TextChanged;
            txt1000BillQty.TextChanged += txt1000BillQty_TextChanged;

        }

        private void ApplyTheme()
        {
            POSTheme.ApplyCardBunifu(bunifuCards1);

        }


        private void CashierCashCountView_Load(object sender, EventArgs e)
        {
            foreach (var lbl in new[] { lbl5CentAmt, lbl10CentAmt, lbl25CentAmt, lbl1PesoAmt, lbl5PesoAmt,
                                lbl10PesoAmt, lbl20PesoAmt, lbl20BillAmt, lbl50BillAmt, lbl100BillAmt,
                                lbl200BillAmt, lbl500BillAmt, lbl1000BillAmt, lblCashCount,
                                lblGrandTotal, lblOpeningAmt, lblCashIn, lblCashOut, lblgrandGrandTotal })
                lbl.Text = "0.00";

            this.LoadShiftStatus();
            this.loadCashCountDetails();
            this.LoadCashInOut();
        }

        public void LoadCashInOut()
        {
            decimal cashIn = 0M;
            decimal cashOut = 0M;
            decimal opening = ParseDecimal(this.lblOpeningAmt.Text);

            var list = this._unitOfWork.CashInOut.GetCashInOut(this._cashCountID);

            var outRow = list.FirstOrDefault(xy => xy.cashType == "CASH OUT");
            if (outRow != null)
            {
                this.lblCashOut.Text = Fmt(outRow.amount);
                cashOut = outRow.amount ?? 0M;
            }

            var inRow = list.FirstOrDefault(xy => xy.cashType == "CASH IN");
            if (inRow != null)
            {
                this.lblCashIn.Text = Fmt(inRow.amount);
                cashIn = inRow.amount ?? 0M;
            }

            this.lblgrandGrandTotal.Text = (opening + cashIn - cashOut).ToString("N");
        }

        private void loadCashCountDetails()
        {
            cashcount cc = this._unitOfWork.CashCount.Find(x => x.SqNo == this._cashCountID).FirstOrDefault();
            if (cc == null)
                return;

            this.txt1000BillQty.Text = (cc.OneThousand ?? 0).ToString();
            this.txt500BillQty.Text = (cc.FiveHundred ?? 0).ToString();
            this.txt200BillQty.Text = (cc.TwoHundred ?? 0).ToString();
            this.txt100BillQty.Text = (cc.OneHundred ?? 0).ToString();
            this.txt50BillQty.Text = (cc.Fifty ?? 0).ToString();
            this.txt20BillQty.Text = (cc.Twenty ?? 0).ToString();
            this.txt20PesoQty.Text = (cc.TwentyCoins ?? 0).ToString();
            this.txt10PesoQty.Text = (cc.TenCoins ?? 0).ToString();
            this.txt5PesoQty.Text = (cc.FiveCoins ?? 0).ToString();
            this.txt1PesoQty.Text = (cc.OneCoins ?? 0).ToString();
            this.txt25CentQty.Text = (cc.TwentyFiveCents ?? 0).ToString();
            this.txt10CentQty.Text = (cc.TenCents ?? 0).ToString();
            this.txt5CentQty.Text = (cc.FiveCents ?? 0).ToString();

            this.lbl1000BillAmt.Text = Fmt(cc.OneThousandAmt);
            this.lbl500BillAmt.Text = Fmt(cc.FiveHundredAmt);
            this.lbl200BillAmt.Text = Fmt(cc.TwoHundredAmt);
            this.lbl100BillAmt.Text = Fmt(cc.OneHundredAmt);
            this.lbl50BillAmt.Text = Fmt(cc.FiftyAmt);
            this.lbl20BillAmt.Text = Fmt(cc.TwentyAmt);
            this.lbl20PesoAmt.Text = Fmt(cc.TwentyCoinsAmt);
            this.lbl10PesoAmt.Text = Fmt(cc.TenCoinsAmt);
            this.lbl5PesoAmt.Text = Fmt(cc.FiveCoinsAmt);
            this.lbl1PesoAmt.Text = Fmt(cc.OneCoinsAmt);
            this.lbl25CentAmt.Text = Fmt(cc.TwentyFiveCentsAmt);
            this.lbl10CentAmt.Text = Fmt(cc.TenCentsAmt);
            this.lbl5CentAmt.Text = Fmt(cc.FiveCentsAmt);

            this.txtOpeningAmt.Text = Fmt(cc.OpeningAmt);
            this.lblCashCount.Text = Fmt(cc.CashCountAmt);
            this.lblGrandTotal.Text = Fmt(cc.GrandTotal);
        }

        private void LoadShiftStatus()
        {
            if (this.CheckIfhasShift())
            {
                this.btnShift.Text = "End Shift";
                this.btnShift.BackColor = Color.FromArgb((int)byte.MaxValue, 128 /*0x80*/, 0);
            }
            else
            {
                this.btnShift.BackColor = Color.FromArgb(110, 163, 20);
                this.btnShift.Text = "Start Shift";
            }
        }

        private bool CheckIfhasShift()
        {
            cashcount cashcount1 = new cashcount();
            cashcount cashcount2 = this._unitOfWork.CashCount.Find(x => x.UserNm == this._UserNm && x.IsClosed == false).FirstOrDefault();
            if (cashcount2 == null)
                return false;
            this._cashCountID = cashcount2.SqNo;
            return true;
        }

        private void CalculateAmt(TextBox txtObject, Decimal denomination)
        {
            int result = 0;
            if (!int.TryParse(txtObject.Text, out result))
                result = 0;
            Decimal num = (Decimal)result * denomination;
            if (denomination == Convert.ToDecimal(0.05))
                this.lbl5CentAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(0.1))
                this.lbl10CentAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(0.25))
                this.lbl25CentAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(1))
                this.lbl1PesoAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(5))
                this.lbl5PesoAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(10))
                this.lbl10PesoAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(20) && txtObject == this.txt20PesoQty)
                this.lbl20PesoAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(20) && txtObject == this.txt20BillQty)
                this.lbl20BillAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(50))
                this.lbl50BillAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(100))
                this.lbl100BillAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(200))
                this.lbl200BillAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(500))
                this.lbl500BillAmt.Text = num.ToString("N");
            else if (denomination == Convert.ToDecimal(1000))
                this.lbl1000BillAmt.Text = num.ToString("N");
            this.GetGrandTotal();
        }

        private void GetGrandTotal()
        {
            decimal num =
                ParseDecimal(this.lbl5CentAmt.Text) + ParseDecimal(this.lbl10CentAmt.Text) +
                ParseDecimal(this.lbl25CentAmt.Text) + ParseDecimal(this.lbl1PesoAmt.Text) +
                ParseDecimal(this.lbl5PesoAmt.Text) + ParseDecimal(this.lbl10PesoAmt.Text) +
                ParseDecimal(this.lbl20PesoAmt.Text) + ParseDecimal(this.lbl20BillAmt.Text) +
                ParseDecimal(this.lbl50BillAmt.Text) + ParseDecimal(this.lbl100BillAmt.Text) +
                ParseDecimal(this.lbl200BillAmt.Text) + ParseDecimal(this.lbl500BillAmt.Text) +
                ParseDecimal(this.lbl1000BillAmt.Text);

            this.lblCashCount.Text = num.ToString("N");
            this.lblGrandTotal.Text = num.ToString("N");
        }

        private void SaveCashCount(bool withShift)
        {
            try
            {
                if (!withShift && !this.CheckIfhasShift())
                {
                    int num1 = (int)MessageBox.Show("Kindly press Start Shift first before saving", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    cashcount entity = new cashcount();
                    if (this.CheckIfhasShift())
                        entity = this._unitOfWork.CashCount.Find(x => x.SqNo == this._cashCountID).FirstOrDefault();
                    entity.TerminalId = "T1";
                    entity.UserNm = this._UserNm;
                    entity.OneThousand = ParseInt(this.txt1000BillQty.Text);
                    entity.FiveHundred = ParseInt(this.txt500BillQty.Text);
                    entity.TwoHundred = ParseInt(this.txt200BillQty.Text);
                    entity.OneHundred = ParseInt(this.txt100BillQty.Text);
                    entity.Fifty = ParseInt(this.txt50BillQty.Text);
                    entity.Twenty = ParseInt(this.txt20BillQty.Text);
                    entity.TwentyCoins = ParseInt(this.txt20PesoQty.Text);
                    entity.TenCoins = ParseInt(this.txt10PesoQty.Text);
                    entity.FiveCoins = ParseInt(this.txt5PesoQty.Text);
                    entity.OneCoins = ParseInt(this.txt1PesoQty.Text);
                    entity.TwentyFiveCents = ParseInt(this.txt25CentQty.Text);
                    entity.TenCents = ParseInt(this.txt10CentQty.Text);
                    entity.FiveCents = ParseInt(this.txt5CentQty.Text);

                    entity.OneThousandAmt = ParseDecimal(this.lbl1000BillAmt.Text);
                    entity.FiveHundredAmt = ParseDecimal(this.lbl500BillAmt.Text);
                    entity.TwoHundredAmt = ParseDecimal(this.lbl200BillAmt.Text);
                    entity.OneHundredAmt = ParseDecimal(this.lbl100BillAmt.Text);
                    entity.FiftyAmt = ParseDecimal(this.lbl50BillAmt.Text);
                    entity.TwentyAmt = ParseDecimal(this.lbl20BillAmt.Text);
                    entity.TwentyCoinsAmt = ParseDecimal(this.lbl20PesoAmt.Text);
                    entity.TenCoinsAmt = ParseDecimal(this.lbl10PesoAmt.Text);
                    entity.FiveCoinsAmt = ParseDecimal(this.lbl5PesoAmt.Text);
                    entity.OneCoinsAmt = ParseDecimal(this.lbl1PesoAmt.Text);
                    entity.TwentyFiveCentsAmt = ParseDecimal(this.lbl25CentAmt.Text);
                    entity.TenCentsAmt = ParseDecimal(this.lbl10CentAmt.Text);
                    entity.FiveCentsAmt = ParseDecimal(this.lbl5CentAmt.Text);
                    entity.OpeningAmt = ParseDecimal(this.txtOpeningAmt.Text);
                    entity.CashCountAmt = ParseDecimal(this.lblCashCount.Text);
                    entity.GrandTotal = ParseDecimal(this.lblGrandTotal.Text);
                    if (this.CheckIfhasShift())
                    {
                        if (withShift)
                        {
                            entity.TimeoOut = new DateTime?(DateTime.Now);
                            entity.IsClosed = true;
                            this._context.Entry<cashcount>(entity).State = EntityState.Modified;
                            this._context.SaveChanges();
                            DateTime? nullable = entity.TimeIn;
                            string str1 = nullable.ToString();
                            nullable = entity.TimeoOut;
                            string str2 = nullable.ToString();
                            int num2 = (int)MessageBox.Show($"Save successfully! \n Start Shift: {str1}\n End Shift: {str2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.LoadShiftStatus();
                        }
                        else
                        {
                            this._context.Entry<cashcount>(entity).State = EntityState.Modified;
                            this._context.SaveChanges();
                            int num3 = (int)MessageBox.Show("Save successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        if (!withShift)
                            return;
                        entity.TimeIn = new DateTime?(DateTime.Now);
                        entity.IsClosed = false;
                        this._unitOfWork.CashCount.Add(entity);
                        this._unitOfWork.Complete();
                        int num4 = (int)MessageBox.Show("Save successfully! \n Start Shift: " + entity.TimeIn.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.LoadShiftStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        // ================================ >> Events
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
        private void btnSave_Click(object sender, EventArgs e) => this.SaveCashCount(false);
        private void btnShift_Click(object sender, EventArgs e) => this.SaveCashCount(true);

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.CheckIfhasShift() || this._cashCountID > 0)
            {
                Form modalOwner = this.MdiParent ?? this;
                using (Form backdrop = new Form())
                {
                    backdrop.FormBorderStyle = FormBorderStyle.None;
                    backdrop.StartPosition = FormStartPosition.Manual;
                    backdrop.ShowInTaskbar = false;
                    backdrop.BackColor = Color.Black;
                    backdrop.Opacity = 0.45;
                    backdrop.Bounds = modalOwner.Bounds;
                    backdrop.Show(modalOwner);
                    backdrop.BringToFront();

                    using (PrintCashCountView view = new PrintCashCountView(this._cashCountID))
                    {
                        view.StartPosition = FormStartPosition.CenterParent;
                        POSTheme.ShowSmoothFadeDialog(view, modalOwner);
                    }
                }
            }
            else
            {
                int num2 = (int)MessageBox.Show("Kindly press Start Shift first before saving", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void btnCashInOut_Click(object sender, EventArgs e)
        {
            Form modalOwner = this.MdiParent ?? this;
            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = modalOwner.Bounds;
                backdrop.Show(modalOwner);
                backdrop.BringToFront();

                using (CashInOutView view = new CashInOutView(this._cashCountID, this._UserNm, this))
                {
                    view.StartPosition = FormStartPosition.CenterParent;
                    POSTheme.ShowSmoothFadeDialog(view, modalOwner);
                }
            }
        }
        private void btnCashHistory_Click(object sender, EventArgs e)
        {
            Form modalOwner = this.MdiParent ?? this;
            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = modalOwner.Bounds;
                backdrop.Show(modalOwner);
                backdrop.BringToFront();

                using (CashInOutHistoryView view = new CashInOutHistoryView(this._cashCountID))
                {
                    view.StartPosition = FormStartPosition.CenterParent;
                    POSTheme.ShowSmoothFadeDialog(view, modalOwner);
                }
            }
            //new CashInOutHistoryView(this._cashCountID).ShowDialog();
        }
        private void txtOpeningAmt_TextChanged(object sender, EventArgs e)
        {
            this.lblOpeningAmt.Text = this.txtOpeningAmt.Text;
            this.LoadCashInOut();
        }

        private void txt5CentQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt5CentQty, Convert.ToDecimal(0.05));
        }
        private void txt10CentQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt10CentQty, Convert.ToDecimal(0.1));
        }
        private void txt25CentQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt25CentQty, Convert.ToDecimal(0.25));
        }
        private void txt1PesoQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt1PesoQty, Convert.ToDecimal(1));
        }
        private void txt5PesoQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt5PesoQty, Convert.ToDecimal(5));
        }
        private void txt10PesoQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt10PesoQty, Convert.ToDecimal(10));
        }
        private void txt20PesoQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt20PesoQty, Convert.ToDecimal(20));
        }
        private void txt20BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt20BillQty, Convert.ToDecimal(20));
        }
        private void txt50BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt50BillQty, Convert.ToDecimal(50));
        }
        private void txt100BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt100BillQty, Convert.ToDecimal(100));
        }
        private void txt200BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt200BillQty, Convert.ToDecimal(200));
        }
        private void txt500BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt500BillQty, Convert.ToDecimal(500));
        }
        private void txt1000BillQty_TextChanged(object sender, EventArgs e)
        {
            this.CalculateAmt(this.txt1000BillQty, Convert.ToDecimal(1000));
        }


        private static decimal ParseDecimal(string s)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out d) ? d : 0m;
        }

        private static int ParseInt(string s)
        {
            int i;
            return int.TryParse((s ?? "").Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out i) ? i : 0;
        }

        private static string Fmt(decimal? v)
        {
            return (v ?? 0m).ToString("N");
        }


    }
}
