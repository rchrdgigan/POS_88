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

namespace POS_88.Views
{
    public partial class StoreSettingsView : Form
    {
        private int settingID = 1;
        private UnitOfWorkPOS _unitOfWork;

        public StoreSettingsView()
        {
            InitializeComponent();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            LoadSettings();

            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                setting setting = new setting();
                setting entity = this._unitOfWork.Setting.Find(x => x.settingsid == this.settingID).FirstOrDefault();
                entity.settingsid = this.settingID;
                entity.storename = this.txtStoreName.Text;
                entity.address = this.txtaddress.Text;
                entity.tinno = this.txtTinNo.Text;
                entity.contactno = this.txtContactNo.Text;
                entity.withAR = new bool?(this.chkAcctReceiv.Checked);
                entity.withLogo = new bool?(this.chkLogo.Checked);
                entity.touscreenmode = new bool?(this.chkTouchScreen.Checked);
                entity.printername = this.txtPrinterNm.Text;
                entity.footer = this.txtFooter.Text;
                entity.itemcodefrom = new int?(Convert.ToInt32(this.txtItemCodeFrom.Text));
                entity.itemcodeto = new int?(Convert.ToInt32(this.txtItemCodeTo.Text));
                entity.weightfrom = new int?(Convert.ToInt32(this.txtWeightFrom.Text));
                entity.weightto = new int?(Convert.ToInt32(this.txtWeightTo.Text));
                entity.nodecimal = new int?(Convert.ToInt32(this.txtNoDecimal.Text));
                entity.downpayment = this.txtDownpayment.Value;
                entity.wholesalemarkup = this.txtWholesaleMarkup.Value;
                entity.retailmarkup = this.txtRetailMarkUp.Value;
                entity.poleport = this.txtPortNo.Text;
                entity.defaultTopDisplay = this.txtTopDisplay.Text;
                entity.defaultBottomDisplay = this.txtBottomDisplay.Text;
                entity.enablepoledisplay = new bool?(this.chkEnablePole.Checked);
                entity.amountspent = this.txtAmountSpent.Value;
                entity.pointsearned = this.txtPointsReceived.Value;
                entity.conversionrate = this.txtConversionRate.Value;
                entity.pointspesos = this.txtPesos.Value;
                entity.enablepointspayment = new bool?(this.chkPointSystem.Checked);
                entity.enablecreditlimit = new bool?(this.chkCreditLimit.Checked);
                Settings.Default.PrinterName = this.txtPrinterNm.Text;
                Settings.Default.Save();
                this._unitOfWork.Setting.Update(entity);
                this._unitOfWork.Complete();
                int num = (int)MessageBox.Show("Store settings saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Store settings saved failed:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void LoadSettings()
        {
            try
            {
                setting setting = this._unitOfWork.Setting.Find(x => x.settingsid == this.settingID).FirstOrDefault();
                this.txtStoreName.Text = setting.storename;
                this.txtaddress.Text = setting.address;
                this.txtTinNo.Text = setting.tinno;
                this.txtContactNo.Text = setting.contactno;
                this.txtPrinterNm.Text = setting.printername;
                this.txtFooter.Text = setting.footer;
                this.chkAcctReceiv.Checked = setting.withAR.GetValueOrDefault();
                this.chkLogo.Checked = setting.withLogo.GetValueOrDefault();
                this.chkTouchScreen.Checked = setting.touscreenmode.GetValueOrDefault();
                this.txtItemCodeFrom.Text = Convert.ToString((object)setting.itemcodefrom);
                this.txtItemCodeTo.Text = Convert.ToString((object)setting.itemcodeto);
                this.txtWeightFrom.Text = Convert.ToString((object)setting.weightfrom);
                this.txtWeightTo.Text = Convert.ToString((object)setting.weightto);
                this.txtNoDecimal.Text = Convert.ToString((object)setting.nodecimal);
                this.txtWholesaleMarkup.Value = Convert.ToDecimal(setting.downpayment);
                this.txtWholesaleMarkup.Value = Convert.ToDecimal(setting.wholesalemarkup);
                this.txtRetailMarkUp.Value = Convert.ToDecimal(setting.retailmarkup);
                this.txtPortNo.Text = Convert.ToString(setting.poleport);
                this.txtTopDisplay.Text = Convert.ToString(setting.defaultTopDisplay);
                this.txtBottomDisplay.Text = Convert.ToString(setting.defaultBottomDisplay);
                this.chkEnablePole.Checked = setting.enablepoledisplay.GetValueOrDefault();
                this.txtAmountSpent.Value = setting.amountspent;
                this.txtPointsReceived.Value = setting.pointsearned;
                this.txtConversionRate.Value = setting.conversionrate;
                this.txtPesos.Value = setting.pointspesos;
                this.chkPointSystem.Checked = setting.enablepointspayment.GetValueOrDefault();
                if (txtPrinterNm.Text == "")
                {
                    this.txtPrinterNm.Text = Settings.Default.PrinterName;
                }
                this.chkCreditLimit.Checked = setting.enablecreditlimit.GetValueOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Store settings error:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
