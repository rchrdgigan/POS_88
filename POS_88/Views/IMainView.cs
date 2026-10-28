using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IMainView
    {
        //Events
        event EventHandler ShowDashboardView;
        event EventHandler ShowInventoryView;
        event EventHandler ShowSalesView;
        event EventHandler ShowPurchasingView;
        event EventHandler ShowUsersView;
        event EventHandler ShowExpensesView;
        event EventHandler ShowSetupView;
        event EventHandler ShowCustomersView;

    }
}
