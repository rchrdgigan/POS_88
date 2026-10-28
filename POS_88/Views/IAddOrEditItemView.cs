using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface IAddOrEditItemView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;
        event EventHandler BrowseImageEvent;

        string ItemCode { get; set; }
        string ItemName { get; set; }
        string Unit { get; set; }
        string CategoryId { get; set; }
        string SupplierId { get; set; }
        string ManufacturerId { get; set; }
        string CostPrice { get; set; }
        string WholesalePrice { get; set; }
        string RetailPrice { get; set; }
        string PromoPrice { get; set; }
        decimal Quantity { get; set; }
        decimal AlertQuantity { get; set; }
        string VatStatus { get; set; }
        string Availability { get; set; }
        string Remarks { get; set; }
        decimal WholesaleMarkup { get; set; }
        decimal RetailMarkup { get; set; }
        decimal PromoMarkup { get; set; }
        Image ItemImage { get; set; }

        void SetCategoryList(BindingSource list);
        void SetSupplierList(BindingSource list);
        void SetManufacturerList(BindingSource list);
        void SetVatStatusList(BindingSource list);
        void SetAvailabilityList(IList<string> list);
        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}