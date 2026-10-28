using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class AddOrEditItemView : Form, IAddOrEditItemView
    {
        public AddOrEditItemView()
        {
            InitializeComponent();
            ApplyTheme();
            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
            btnSave.Click += (sender, args) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearEvent?.Invoke(this, EventArgs.Empty);
            btnBrowserPhoto.Click += (sender, args) => BrowseImageEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyTheme()
        {
            // topbar gradients
            lblModalTitle.BackColor = Color.Transparent;
            POSTheme.ApplyTopbar(panel1);

            // button close
            btnMinimize.BackColor = POSTheme.Navy;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

            // button close
            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;
        }


        public event EventHandler SaveEvent;
        public event EventHandler ClearEvent;
        public event EventHandler BrowseImageEvent;

        public string ItemCode { get { return txtItemCode.Text; } set { txtItemCode.Text = value; } }
        public string ItemName { get { return txtName.Text; } set { txtName.Text = value; } }
        public string Unit { get { return txtUnit.Text; } set { txtUnit.Text = value; } }
        public string CategoryId { get { return Convert.ToString(cbBoxCategory.SelectedValue); } set { SetSelectedValue(cbBoxCategory, value); } }
        public string SupplierId { get { return Convert.ToString(cbBoxSupplier.SelectedValue); } set { SetSelectedValue(cbBoxSupplier, value); } }
        public string ManufacturerId { get { return Convert.ToString(cbBoxManufacturer.SelectedValue); } set { SetSelectedValue(cbBoxManufacturer, value); } }
        public string CostPrice { get { return txtCost.Text; } set { txtCost.Text = value; } }
        public string WholesalePrice { get { return txtWholesale.Text; } set { txtWholesale.Text = value; } }
        public string RetailPrice { get { return txtRetail.Text; } set { txtRetail.Text = value; } }
        public string PromoPrice { get { return txtPromo.Text; } set { txtPromo.Text = value; } }
        public decimal Quantity { get { return numQty.Value; } set { numQty.Value = value; } }
        public decimal AlertQuantity { get { return numAlertQty.Value; } set { numAlertQty.Value = value; } }
        public string VatStatus { get { return Convert.ToString(cbBoxVatStat.SelectedValue); } set { SetSelectedValue(cbBoxVatStat, value); } }
        public string Availability { get { return cbBoxAvailability.Text; } set { cbBoxAvailability.Text = value; } }
        public string Remarks { get { return txtRemarks.Text; } set { txtRemarks.Text = value; } }
        public decimal WholesaleMarkup { get { return numWholesaleMarkup.Value; } set { numWholesaleMarkup.Value = value; } }
        public decimal RetailMarkup { get { return numRetailMarkup.Value; } set { numRetailMarkup.Value = value; } }
        public decimal PromoMarkup { get { return numPromoMarkup.Value; } set { numPromoMarkup.Value = value; } }
        public Image ItemImage { get { return pictureBox1.Image; } set { pictureBox1.Image = value; } }

        public void SetCategoryList(BindingSource list)
        {
            SetComboList(cbBoxCategory, list, "categoryname", "categoryid");
        }

        public void SetSupplierList(BindingSource list)
        {
            SetComboList(cbBoxSupplier, list, "suppliername", "supplierid");
        }

        public void SetManufacturerList(BindingSource list)
        {
            SetComboList(cbBoxManufacturer, list, "manufacturername", "manufacturerid");
        }

        public void SetVatStatusList(BindingSource list)
        {
            SetComboList(cbBoxVatStat, list, "vatname", "vatid");
        }

        public void SetAvailabilityList(IList<string> list)
        {
            cbBoxAvailability.DataSource = null;
            cbBoxAvailability.Items.Clear();

            if (list != null)
            {
                foreach (string item in list)
                    cbBoxAvailability.Items.Add(item);
            }

            cbBoxAvailability.SelectedIndex = cbBoxAvailability.Items.Count > 0 ? 0 : -1;
        }

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Item" : "Add New Item";
            btnSave.Text = isEditMode ? "Update" : "Save";
            txtItemCode.Enabled = !isEditMode;
        }

        public void ClearFields()
        {
            ItemCode = string.Empty;
            ItemName = string.Empty;
            Unit = string.Empty;
            CategoryId = string.Empty;
            SupplierId = string.Empty;
            ManufacturerId = string.Empty;
            CostPrice = string.Empty;
            WholesalePrice = string.Empty;
            RetailPrice = string.Empty;
            PromoPrice = string.Empty;
            Quantity = 0;
            AlertQuantity = 0;
            VatStatus = string.Empty;
            Availability = string.Empty;
            Remarks = string.Empty;
            WholesaleMarkup = 0;
            RetailMarkup = 0;
            PromoMarkup = 0;
            ItemImage = null;
        }

        private static void SetComboList(ComboBox comboBox, BindingSource list, string displayMember, string valueMember)
        {
            comboBox.DataSource = list;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            comboBox.SelectedIndex = -1;
        }

        private static void SetSelectedValue(ComboBox comboBox, string value)
        {
            comboBox.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(value))
                comboBox.SelectedValue = value;
        }

      
    }
}
