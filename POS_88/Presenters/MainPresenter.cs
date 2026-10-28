using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Views;


namespace POS_88.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView;
        private DashboardPresenter dashboardPresenter;
        private InventoryPresenter inventoryPresenter;
        private SalesPresenter salesPresenter;
        private PurchaseOrderPresenter purchasingPresenter;
        private UsersPresenter usersPresenter;
        private ExpensesPresenter expensesPresenter;
        private SetupPresenter setupPresenter;
        private CustomersPresenter customerPresenter;

        public MainPresenter(IMainView mainView)
        {
            this.mainView = mainView;
            this.mainView.ShowDashboardView += ShowDashboardView;
            this.mainView.ShowInventoryView += ShowInventoryView;
            this.mainView.ShowSalesView += ShowSalesView;
            this.mainView.ShowPurchasingView += ShowPurchasingView;
            this.mainView.ShowUsersView += ShowUsersView;
            this.mainView.ShowExpensesView += ShowExpenses;
            this.mainView.ShowSetupView += ShowSetupView;
            this.mainView.ShowCustomersView += ShowCustomersView;
        }

        private void ShowCustomersView(object sender, EventArgs e)
        {
            ICustomersView view = CustomersView.GetInstance((MainView)mainView);
            if (customerPresenter == null)
                customerPresenter = new CustomersPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            else
                customerPresenter.Reload();

            view.Show();
        }

        private void ShowSetupView(object sender, EventArgs e)
        {
            ISetupView view = SetupView.GetInstance((MainView)mainView);
            if (setupPresenter == null)
                setupPresenter = new SetupPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            view.Show();
        }

        private void ShowExpenses(object sender, EventArgs e)
        {
            IExpensesView view = ExpensesView.GetInstance((MainView)mainView);
            if (expensesPresenter == null)
                expensesPresenter = new ExpensesPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            else
                expensesPresenter.Reload();

            view.Show();
        }

        private void ShowUsersView(object sender, EventArgs e)
        {
            IUsersView view = UsersView.GetInstance((MainView)mainView);
            if (usersPresenter == null)
                usersPresenter = new UsersPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            else
                usersPresenter.Reload();

            view.Show();
        }

        private void ShowPurchasingView(object sender, EventArgs e)
        {
            IPurchaseOrderView view = PurchaseOrderView.GetInstance((MainView)mainView);
            if (purchasingPresenter == null)
                purchasingPresenter = new PurchaseOrderPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            else
                purchasingPresenter.Reload();

            view.Show();
        }

        private void ShowSalesView(object sender, EventArgs e)
        {
            ISalesView view = SalesView.GetInstance((MainView)mainView);
            if (salesPresenter == null)
                salesPresenter = new SalesPresenter(view, ((MainView)mainView).AuthenticatedUserName);
            else
                salesPresenter.Reload();

            view.Show();

        }

        private void ShowDashboardView(object sender, EventArgs e)
        {
            IDashboardView view = DashboardView.GetInstance((MainView)mainView);
            view.AuthenticatedName = ((MainView)mainView).AuthenticatedName;

            if (dashboardPresenter == null)
                dashboardPresenter = new DashboardPresenter(view);
            else
                dashboardPresenter.Reload();

            view.Show();
        }

        private void ShowInventoryView(object sender, EventArgs e)
        {
            IInventoryView view = InventoryView.GetInstance((MainView)mainView);
            if (inventoryPresenter == null)
                inventoryPresenter = new InventoryPresenter(view);
            else
                inventoryPresenter.Reload();

            view.Show();
            view.AuthenticatedUserName = ((MainView)mainView).AuthenticatedUserName;
        }
    }
}
