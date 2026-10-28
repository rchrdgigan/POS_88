using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface ISalesView
    {
        // UI Display Methods
        void Show();
        void Hide();
        void RefreshData();

        // Search & Filter Methods
        void GetSalesData(string searchParam);
        void SearchSales();
        void LoadSalesData();

        // Data Properties
        string SearchText { get; set; }
        string TypeOfSales { get; set; }
        string PaymentType { get; set; }
        string PaymentStatus { get; set; }
        string StaffName { get; set; }
        string StaffId { get; }
        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }

        void SetStaffList(BindingSource staff);
        void SetSalesTypeList(IList<string> salesTypes);
        void SetPaymentStatusList(IList<string> statuses);
        void SetPaymentTypeList(IList<string> paymentTypes);
        void SetSummaryVisibility(bool showGrid, bool showRevenue, bool showAmountPaid,
            bool showBalance, bool showGrossProfit);
        void ShowFilterLoading();
        void HideFilterLoading();
        void ShowSalesLoading();
        void HideSalesLoading();

        // Grid Properties
        DataTable SalesGridData { get; set; }
        DataTable SalesGridSummaryData { get; set; }
        DataGridView SalesGrid { get; }
        DataGridView SalesGridSummary { get; }

        // Summary Display Properties
        string TotalRevenue { get; set; }
        string AmountPaid { get; set; }
        string Balance { get; set; }
        string GrossProfit { get; set; }

        // Button/Action Methods
        void OnPaymentClick();
        void OnVoidSalesClick();
        void OnCompleteTransactionClick();
        void OnReprintClick();
        void OnReloadClick();
        void OnSearchClick();

        // Events
        event EventHandler SearchRequested;
        event EventHandler ReloadRequested;
        event EventHandler PaymentRequested;
        event EventHandler VoidSalesRequested;
        event EventHandler CompleteTransactionRequested;
        event EventHandler ReprintRequested;
    }
}
