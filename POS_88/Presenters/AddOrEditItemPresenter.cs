using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class AddOrEditItemPresenter
    {
            private readonly IAddOrEditItemView view;
            private readonly POSImplementation posImplementation;
            private readonly string userName;
            private readonly string itemCode;
            private readonly string saveType;
            private readonly Action afterSave;
            private readonly bool isNotFound;
            private string imagePath;
            private decimal defaultWholesaleMarkup;
            private decimal defaultRetailMarkup;

            public AddOrEditItemPresenter(
                IAddOrEditItemView view,
                string userName,
                string itemCode,
                Action afterSave,
                bool isNotFound = false)
            {
                if (view == null)
                    throw new ArgumentNullException("view");

                this.view = view;
                this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
                this.itemCode = itemCode;
                this.saveType = string.IsNullOrWhiteSpace(itemCode) ? "INSERT" : "UPDATE";
                this.afterSave = afterSave;
                this.posImplementation = new POSImplementation();
                this.isNotFound = isNotFound;
                this.view.SaveEvent += SaveItem;
                this.view.ClearEvent += ClearFields;
                this.view.BrowseImageEvent += BrowseImage;

                PopulateLookups();
                this.view.SetEditMode(this.saveType != "INSERT");
                ResetFields();

                if (this.saveType != "INSERT")
                    LoadItem(itemCode);
            }

        private void PopulateLookups()
        {
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                var categories = unitOfWork.Category.GetAll()
                    .OrderBy(item => item.categoryname).ToList();
                var suppliers = unitOfWork.Supplier.GetAll()
                    .OrderBy(item => item.suppliername).ToList();
                var manufacturers = unitOfWork.Manufacturer.GetAll()
                    .OrderBy(item => item.manufacturername).ToList();
                var vatStatuses = unitOfWork.VATStatus.GetAll()
                    .OrderBy(item => item.vatname).ToList();
                setting settings = unitOfWork.Setting
                    .Find(item => item.settingsid == 1).FirstOrDefault();

                view.SetCategoryList(new BindingSource { DataSource = categories });
                view.SetSupplierList(new BindingSource { DataSource = suppliers });
                view.SetManufacturerList(new BindingSource { DataSource = manufacturers });
                view.SetVatStatusList(new BindingSource { DataSource = vatStatuses });
                view.SetAvailabilityList(new List<string> { "", "YES", "NO" });

                if (settings != null)
                {
                    defaultWholesaleMarkup = settings.wholesalemarkup;
                    defaultRetailMarkup = settings.retailmarkup;
                    imagePath = settings.imagePath;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void LoadItem(string code)
        {
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                inventory item = unitOfWork.Inventory
                    .Find(value => value.itemcode == code).FirstOrDefault();
                if (item == null)
                {
                    MessageBox.Show("The selected item could not be found.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.Close();
                    return;
                }

                view.ItemCode = item.itemcode;
                view.ItemName = item.itemname;
                view.Unit = item.unit;
                view.CategoryId = item.categoryid;
                view.SupplierId = item.supplierid;
                view.ManufacturerId = item.manufacturerid;
                view.CostPrice = FormatAmount(item.cost);
                view.WholesalePrice = FormatAmount(item.wholesaleprice);
                view.RetailPrice = FormatAmount(item.price);
                view.PromoPrice = FormatAmount(item.promoprice);
                view.Quantity = item.quantity.GetValueOrDefault();
                view.AlertQuantity = item.alertquantity.GetValueOrDefault();
                view.VatStatus = item.vatstatus;
                view.Availability = item.availability;
                view.Remarks = item.remarks;
                LoadItemImage(item.itemcode);
            }
            catch (Exception)
            {
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void LoadItemImage(string code)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            string filePath = Path.Combine(imagePath, code + ".jpg");
            if (!File.Exists(filePath))
                return;

            using (Image image = Image.FromFile(filePath))
                view.ItemImage = new Bitmap(image);
        }

        private void SaveItem(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.ItemCode) || string.IsNullOrWhiteSpace(view.ItemName))
            {
                MessageBox.Show("Item code and name are required.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal cost;
            decimal wholesalePrice;
            decimal retailPrice;
            decimal promoPrice;
            if (!TryParseAmount(view.CostPrice, "Cost price", out cost) ||
                !TryParseAmount(view.WholesalePrice, "Wholesale price", out wholesalePrice) ||
                !TryParseAmount(view.RetailPrice, "Retail price", out retailPrice) ||
                !TryParseAmount(view.PromoPrice, "Promo price", out promoPrice))
                return;

            inventory item = new inventory
            {
                itemcode = view.ItemCode.Trim(),
                itemname = view.ItemName.Trim(),
                unit = view.Unit,
                categoryid = view.CategoryId,
                supplierid = view.SupplierId,
                manufacturerid = view.ManufacturerId,
                cost = cost,
                wholesaleprice = wholesalePrice,
                price = retailPrice,
                promoprice = promoPrice,
                quantity = view.Quantity,
                alertquantity = view.AlertQuantity,
                vatstatus = view.VatStatus,
                availability = view.Availability,
                remarks = view.Remarks
            };

            try
            {
                string error = posImplementation.SaveItem(item, saveType, userName);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveItemImage(item.itemcode);
                MessageBox.Show("Item has been saved successfully.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (afterSave != null)
                    afterSave();
                if (saveType == "INSERT")
                    if (this.isNotFound == true) // if Item not found in cashier add item and close.
                        view.Close();
                    else
                        ResetFields();
                else
                    view.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryParseAmount(string text, string fieldName, out decimal amount)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                amount = 0M;
                return true;
            }

            if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out amount) && amount >= 0M)
                return true;

            MessageBox.Show(fieldName + " must be a valid non-negative number.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void SaveItemImage(string code)
        {
            if (view.ItemImage == null)
                return;

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("The item was saved, but no image folder is configured.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Directory.CreateDirectory(imagePath);
                string filePath = Path.Combine(imagePath, code + ".jpg");
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    view.ItemImage.Save(stream, ImageFormat.Jpeg);
            }
            catch (Exception exception)
            {
                MessageBox.Show("The item was saved, but its image could not be saved: " + exception.Message,
                    "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BrowseImage(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                using (Image image = Image.FromFile(dialog.FileName))
                    view.ItemImage = new Bitmap(image);
            }
        }

        private void ClearFields(object sender, EventArgs e)
        {
            if (saveType != "INSERT")
            {
                LoadItem(itemCode);
                return;
            }

            ResetFields();
        }

        private void ResetFields()
        {
            view.ClearFields();
            view.CostPrice = "0.00";
            view.WholesalePrice = "0.00";
            view.RetailPrice = "0.00";
            view.PromoPrice = "0.00";
            view.WholesaleMarkup = defaultWholesaleMarkup;
            view.RetailMarkup = defaultRetailMarkup;
        }

        private static string FormatAmount(decimal? value)
        {
            return value.GetValueOrDefault().ToString("0.00", CultureInfo.CurrentCulture);
        }
    }
}