using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;
using System.Windows.Forms;

namespace POS_88.Presenters
{
    public class DashboardPresenter
    {
        private IDashboardView view;
        private UnitOfWorkPOS _unitOfWork;


        private BindingSource customerBindingSource;
        private BindingSource topBindingSource;

        public DashboardPresenter(IDashboardView view)
        {
            this.view = view;
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());

            customerBindingSource = new BindingSource();
            topBindingSource = new BindingSource();

            view.SetTopCustomerListBindingSource(customerBindingSource);
            view.SetTopItemListBindingSource(topBindingSource);

            view.Show();
            //EFResourceChecker.Check();
            this.LoadDashboard();
        }


        private async Task LoadChart()
        {
            int year = DateTime.Now.Year;
            List<spGetMonthlySalesByYear_Result> monthlySalesByYear = await Task.Run(() =>
                this._unitOfWork.ItemSales.GetMonthlySalesByYear(year).ToList());
            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();
            foreach (spGetMonthlySalesByYear_Result salesByYearResult in monthlySalesByYear)
            {
                labels.Add(salesByYearResult.MONTHNM ?? string.Empty);
                values.Add(salesByYearResult.TOTALSALES ?? 0M);
            }
            view.SetMonthlySalesChart("Monthly Sales for Year " + year, labels, values);
        }

        private async Task LoadSummary()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var summary = await Task.Run(() => new
            {
                NumItems = (Decimal)this._unitOfWork.Inventory.GetAll().Count<inventory>(),
                TotalStocks = this._unitOfWork.Inventory.GetTotalStocks(),
                CriticalStocks = (Decimal)this._unitOfWork.Inventory.GetCriticalStocks(),
                TotalExpenses = this._unitOfWork.Expenses.GetTotalExpenses(year, month)
            });

            view.NumItem = $"{summary.NumItems:n}";
            view.TotalStock = $"{summary.TotalStocks:n}";
            view.CriticalStocks = $"{summary.CriticalStocks:n}";
            view.TotalExpenses = summary.TotalExpenses.ToString("N");
        }

        private async Task LoadSalesToday()
        {
            spGetSalesToday_Result salesTodayResult = await Task.Run(() =>
                this._unitOfWork.ItemSales.GetSalesToday(DateTime.Now).FirstOrDefault<spGetSalesToday_Result>());
            Decimal totalSales = salesTodayResult != null && salesTodayResult.SALES.HasValue
                ? Convert.ToDecimal((object)salesTodayResult.SALES)
                : 0M;
            Decimal totalCost = salesTodayResult != null && salesTodayResult.COST.HasValue
                ? Convert.ToDecimal((object)salesTodayResult.COST)
                : 0M;
            Decimal totalDiscount = salesTodayResult != null && salesTodayResult.DISCOUNT.HasValue
                ? Convert.ToDecimal((object)salesTodayResult.DISCOUNT)
                : 0M;

            view.TodayTotalSales = $"{totalSales:n}";
            view.TodayTotalCost = $"{totalCost:n}";
            view.TodayTotalDiscount = $"{totalDiscount:n}";
            view.TodayNetProfit = $"{totalSales - totalCost:n}";
        }

        private async Task LoadTopItems()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            List<spGetTopItems_Result> items = await Task.Run(() =>
                this._unitOfWork.ItemSales.GetTopItems(year, month).ToList());

            this.topBindingSource.DataSource = items;
        }

        private async Task LoadTopCustomers()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            List<spGetTopCustomers_Result> customers = await Task.Run(() =>
                this._unitOfWork.ItemSales.GetTopCustomers(year, month).ToList());

            this.customerBindingSource.DataSource = customers;
        }

        private async void LoadDashboard()
        {
            this.view.ShowSummaryLoading();
            this.view.ShowSalesTodayLoading();
            this.view.ShowChartLoading();
            this.view.ShowTopItemsLoading();
            this.view.ShowTopCustomersLoading();

            await this.LoadSection(this.LoadSummary, this.view.HideSummaryLoading);
            await this.LoadSection(this.LoadSalesToday, this.view.HideSalesTodayLoading);
            await this.LoadSection(this.LoadChart, this.view.HideChartLoading);
            await this.LoadSection(this.LoadTopItems, this.view.HideTopItemsLoading);
            await this.LoadSection(this.LoadTopCustomers, this.view.HideTopCustomersLoading);
        }

        public void Reload()
        {
            this.LoadDashboard();
        }

        private async Task LoadSection(Func<Task> load, Action hideLoading)
        {
            try
            {
                await load();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                hideLoading();
            }
        }

    }
}
