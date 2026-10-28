using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{

    public interface IDashboardView
    {
        //Fields
        string AuthenticatedName { get; set; }
        string NumItem { set; }
        string TotalStock { set; }
        string CriticalStocks { set; }
        string TotalExpenses { set; }
        string TodayTotalSales { set; }
        string TodayTotalCost { set; }
        string TodayTotalDiscount { set; }
        string TodayNetProfit { set; }


        //Events


        //Methods
        void SetTopItemListBindingSource(BindingSource list); //top items
        void SetTopCustomerListBindingSource(BindingSource list); //top customer
        void SetMonthlySalesChart(string title, IList<string> labels, IList<decimal> values); //chart
        void ShowSummaryLoading();
        void HideSummaryLoading();
        void ShowSalesTodayLoading();
        void HideSalesTodayLoading();
        void ShowChartLoading();
        void HideChartLoading();
        void ShowTopItemsLoading();
        void HideTopItemsLoading();
        void ShowTopCustomersLoading();
        void HideTopCustomersLoading();
        void Show(); //Optional
    }
}
