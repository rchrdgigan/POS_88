using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace POS_88.Views
{
    public partial class DashboardView : Form, IDashboardView
    {
        //Fields
        private readonly SkeletonLoader numItemsSkeletonLoader;
        private readonly SkeletonLoader totalStocksSkeletonLoader;
        private readonly SkeletonLoader criticalStocksSkeletonLoader;
        private readonly SkeletonLoader expensesSkeletonLoader;
        private readonly SkeletonLoader todaySalesSkeletonLoader;
        private readonly SkeletonLoader todayCostsSkeletonLoader;
        private readonly SkeletonLoader todayDiscountsSkeletonLoader;
        private readonly SkeletonLoader todayProfitSkeletonLoader;
        private readonly SkeletonLoader chartSkeletonLoader;
        private readonly SkeletonLoader topItemsSkeletonLoader;
        private readonly SkeletonLoader topCustomersSkeletonLoader;
        private string authenticatedName;

        public DashboardView()
        {
            InitializeComponent();

            numItemsSkeletonLoader = CreateSkeletonLoader(lblNoOfItems);
            totalStocksSkeletonLoader = CreateSkeletonLoader(lblNoofStocks);
            criticalStocksSkeletonLoader = CreateSkeletonLoader(lblCriticalStocks);
            expensesSkeletonLoader = CreateSkeletonLoader(lblExpenses);
            todaySalesSkeletonLoader = CreateSkeletonLoader(lblTodaySales, 110);
            todayCostsSkeletonLoader = CreateSkeletonLoader(lblTodayCosts, 110);
            todayDiscountsSkeletonLoader = CreateSkeletonLoader(lblTodayDiscounts, 110);
            todayProfitSkeletonLoader = CreateSkeletonLoader(lblTodayGross, 110);
            chartSkeletonLoader = CreateSkeletonLoader(cartesianChart1);
            topItemsSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            topCustomersSkeletonLoader = CreateSkeletonLoader(dataGridViewX2);

            ConfigureTables();
            AuthenticatedName = null;

            pictureBox4.Click += PanelCriticalItems_Click;
            label8.Click += PanelCriticalItems_Click;
            lblCriticalStocks.Click += PanelCriticalItems_Click;
            panelCriticalItems.Click += PanelCriticalItems_Click;

            pictureBox4.MouseHover += PanelCriticalItems_MouseHover;
            label8.MouseHover += PanelCriticalItems_MouseHover;
            lblCriticalStocks.MouseHover += PanelCriticalItems_MouseHover;
            panelCriticalItems.MouseHover += PanelCriticalItems_MouseHover;

            pictureBox4.MouseLeave += PanelCriticalItems_MouseLeave;
            label8.MouseLeave += PanelCriticalItems_MouseLeave;
            lblCriticalStocks.MouseLeave += PanelCriticalItems_MouseLeave;
            panelCriticalItems.MouseLeave += PanelCriticalItems_MouseLeave;
        }

        private void PanelCriticalItems_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(255, 255, 128);
            label8.BackColor = Color.FromArgb(255, 255, 128);
            lblCriticalStocks.BackColor = Color.FromArgb(255, 255, 128);
            panelCriticalItems.BackColor = Color.FromArgb(255, 255, 128);
        }

        private void PanelCriticalItems_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(255, 255, 220);
            label8.BackColor = Color.FromArgb(255, 255, 220);
            lblCriticalStocks.BackColor = Color.FromArgb(255, 255, 220);
            panelCriticalItems.BackColor = Color.FromArgb(255, 255, 220);
        }

        private void PanelCriticalItems_Click(object sender, EventArgs e)
        {
            
            Form modalOwner = this.MdiParent ?? this;
            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = modalOwner.Bounds;
                backdrop.Show(modalOwner);
                backdrop.BringToFront();

                using (CriticalStocksView view = new CriticalStocksView())
                {
                    int num = (int)view.ShowDialog();
                }
            }
            //MessageBox.Show("Show Critical Items");
        }


        // Events
        public event EventHandler FilterCommboxEvent; //weekly, monthly, yearly and daily

        // Method
        private void ConfigureTables()
        {
            if (dataGridViewX1 != null)
            {
                dataGridViewX1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (dataGridViewX2 != null)
            {
                dataGridViewX2.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private SkeletonLoader CreateSkeletonLoader(Control target, int? width = null)
        {
            SkeletonLoader loader = new SkeletonLoader();
            loader.Anchor = target.Anchor;
            loader.Bounds = new Rectangle(target.Left, target.Top, width ?? target.Width, target.Height);
            loader.Visible = false;
            target.Parent.Controls.Add(loader);
            loader.BringToFront();
            return loader;
        }

        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }

        public void ShowSummaryLoading()
        {
            SetLoading(lblNoOfItems, numItemsSkeletonLoader, true);
            SetLoading(lblNoofStocks, totalStocksSkeletonLoader, true);
            SetLoading(lblCriticalStocks, criticalStocksSkeletonLoader, true);
            SetLoading(lblExpenses, expensesSkeletonLoader, true);
        }

        public void HideSummaryLoading()
        {
            SetLoading(lblNoOfItems, numItemsSkeletonLoader, false);
            SetLoading(lblNoofStocks, totalStocksSkeletonLoader, false);
            SetLoading(lblCriticalStocks, criticalStocksSkeletonLoader, false);
            SetLoading(lblExpenses, expensesSkeletonLoader, false);
        }

        public void ShowSalesTodayLoading()
        {
            SetLoading(lblTodaySales, todaySalesSkeletonLoader, true);
            SetLoading(lblTodayCosts, todayCostsSkeletonLoader, true);
            SetLoading(lblTodayDiscounts, todayDiscountsSkeletonLoader, true);
            SetLoading(lblTodayGross, todayProfitSkeletonLoader, true);
        }

        public void HideSalesTodayLoading()
        {
            SetLoading(lblTodaySales, todaySalesSkeletonLoader, false);
            SetLoading(lblTodayCosts, todayCostsSkeletonLoader, false);
            SetLoading(lblTodayDiscounts, todayDiscountsSkeletonLoader, false);
            SetLoading(lblTodayGross, todayProfitSkeletonLoader, false);
        }

        public void ShowChartLoading()
        {
            SetLoading(cartesianChart1, chartSkeletonLoader, true);
        }

        public void HideChartLoading()
        {
            SetLoading(cartesianChart1, chartSkeletonLoader, false);
        }

        public void ShowTopItemsLoading()
        {
            SetLoading(dataGridViewX1, topItemsSkeletonLoader, true);
        }

        public void HideTopItemsLoading()
        {
            SetLoading(dataGridViewX1, topItemsSkeletonLoader, false);
        }

        public void ShowTopCustomersLoading()
        {
            SetLoading(dataGridViewX2, topCustomersSkeletonLoader, true);
        }

        public void HideTopCustomersLoading()
        {
            SetLoading(dataGridViewX2, topCustomersSkeletonLoader, false);
        }


        public string NumItem
        {
            set { lblNoOfItems.Text = value; }
        }

        public string TotalStock
        {
            set { lblNoofStocks.Text = value; }
        }

        public string CriticalStocks
        {
            set { lblCriticalStocks.Text = value; }
        }

        public string TotalExpenses
        {
            set { lblExpenses.Text = value; }
        }

        public string TodayTotalSales
        {
            set { lblTodaySales.Text = value; }
        }

        public string TodayTotalCost
        {
            set { lblTodayCosts.Text = value; }
        }

        public string TodayTotalDiscount
        {
            set { lblTodayDiscounts.Text = value; }
        }
        public string TodayNetProfit
        {
            set { lblTodayGross.Text = value; }
        }

        public string AuthenticatedName
        {
            get { return authenticatedName; }
            set
            {
                authenticatedName = value;
                lblWelcome.Text = string.IsNullOrWhiteSpace(value)
                    ? "Welcome back! Here's what's happening today."
                    : "Welcome back, " + value + "! Here's what's happening today.";
            }
        }

        public void SetTopItemListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }

        public void SetTopCustomerListBindingSource(BindingSource list)
        {
            dataGridViewX2.DataSource = list;
        }

        public void SetMonthlySalesChart(string title, IList<string> labels, IList<decimal> values)
        {
            label11.Text = title;
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            cartesianChart1.Series.Add(new ColumnSeries
            {
                DataLabels = true,
                Values = new ChartValues<decimal>(values),
                LabelPoint = point => point.Y.ToString("N2")
            });
            cartesianChart1.AxisX.Add(new Axis
            {
                Labels = labels.ToList(),
                Separator = new Separator { Step = 1 }
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString("N0"),
                Separator = new Separator()
            });
        }

       

        //Singleton pattern (Open a single form instance)
        private static DashboardView instance;
        public static DashboardView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new DashboardView();
                instance.MdiParent = parentContainer;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }

    }
}
