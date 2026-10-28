using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface IPurchaseOrderView
    {
        string AuthenticatedUserName { get; set; }

        //Fields
        string GrandTotal {get; set;}
        string TotalOrders {get; set;}
        string TotalQty {get; set;}

        // Filter fields
        string SearchParam { get; set; }
        string IsReceived { get; set; }
        DateTime DateOrderFrom { get; set; }
        DateTime DateOrderTo { get; set; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler ClearFiltersEvent;
        event EventHandler ExportExcelEvent;

        // Combo methods
        void ClearFilters();
        void ShowPurchaseOrderLoading();
        void HidePurchaseOrderLoading();
        void SetIsReceivedList(List<string> list);


        //Methods
        void SetPurchaseOrderListBindingSource(BindingSource list); //purchase order with filter
        void Show();
        //void Hide();


    }
}
