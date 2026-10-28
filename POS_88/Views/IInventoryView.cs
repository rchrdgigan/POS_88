using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface IInventoryView
    {
        string AuthenticatedUserName { get; set; }

        //Fields
        string TotalCost { set; }
        string TotalPrice { set; }
        string TotalQuantity { set; }
        string NoOfItems { set; }

        // Filter fields
        string SearchParam { get; set; }
        string CategoryId { get; set; }
        string SupplierId { get; set; }
        string VatStatus { get; set; }
        string Availability { get; set; }
        string SelectedItemCode { get; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler DeleteEvent;
        event EventHandler ClearFiltersEvent;

        // Combo methods
        void SetCategoryList(BindingSource list);
        void SetSupplierList(BindingSource list);
        void SetVatStatusList(BindingSource list);
        void SetAvailabilityList(List<string> list);
        void ClearFilters();
        void ShowFilterLoading();
        void HideFilterLoading();
        void ShowInventoryLoading();
        void HideInventoryLoading();


        //Methods
        void SetInventoryListBindingSource(BindingSource list); //inventory with filter
        void Show();
    }
}
