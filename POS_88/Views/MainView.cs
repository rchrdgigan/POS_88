using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace POS_88.Views
{
    public partial class MainView : Form, IMainView
    {
        private bool sidebarCollapsed = false;
        private string authenticatedName;
        private string authenticatedUsertype;
        public string _userName;

        public MainView()
        {
            InitializeComponent();

            _userName = AuthenticatedUserName;
            AuthenticatedName = null;
            AuthenticatedUsertype = null;
            ApplyTheme();
            panelMark.Visible = false;

            IsMdiContainer = true;

            // MDI background
            foreach (Control c in Controls)
            {
                MdiClient mdi = c as MdiClient;

                if (mdi != null)
                {
                    mdi.BackColor = Color.FromArgb(245, 247, 250);
                    break;
                }
            }

            // Window buttons
            btnClose.Click += delegate { this.Close(); };
            btnLogout.Click += btnLogout_Click;

            btnMinimize.Click += delegate
            {
                this.WindowState = FormWindowState.Minimized;
            };

            // Sidebar
            btnBurger.Click += delegate
            {
                sidebarCollapsed = !sidebarCollapsed;
                sideBar();
                //timerSidebar_Tick.Start();
            };

            // Navigation
            btnDashboard.Click += NavButton_Click;
            btnInventory.Click += NavButton_Click;
            btnSales.Click += NavButton_Click;
            btnPurchasing.Click += NavButton_Click;
            btnCashier.Click += NavButton_Click;
            btnExpenses.Click += NavButton_Click;
            btnUsers.Click += NavButton_Click;
            btnSettings.Click += NavButton_Click;
            btnMyStore.Click += NavButton_Click;
            btnCustomers.Click += NavButton_Click;
        }


        private void ApplyTheme()
        {
            // Sidebar and topbar gradients
            POSTheme.ApplySidebar(panelSideBar);
            POSTheme.ApplyTopbar(panel2);

            // Topbar text and controls
            lblDateTime.ForeColor = POSTheme.Text;
            lblDateTime.BackColor = Color.Transparent;

            btnBurger.BackColor = POSTheme.Navy;
            btnBurger.UseVisualStyleBackColor = false;

            btnMinimize.BackColor = POSTheme.Navy;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.UseVisualStyleBackColor = false;

            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

            // User information
            lblName.ForeColor = POSTheme.Text;
            lblRole.ForeColor = POSTheme.MutedText;
        }


        private void ApplySidebarGradient()
        {
            panelSideBar.Paint += (sender, e) =>
            {
                Rectangle rect = panelSideBar.ClientRectangle;

                if (rect.Width <= 0 || rect.Height <= 0)
                    return;

                using (LinearGradientBrush brush =
                    new LinearGradientBrush(
                        rect,
                        Color.FromArgb(15, 23, 42),
                        Color.FromArgb(8, 15, 32),
                        LinearGradientMode.Vertical))
                {
                    ColorBlend blend = new ColorBlend();

                    blend.Colors = new Color[]
                    {
                        Color.FromArgb(15, 23, 42),
                        Color.FromArgb(20, 43, 80),
                        Color.FromArgb(8, 15, 32)
                    };

                    blend.Positions = new float[] { 0f, 0.45f, 1f };

                    brush.InterpolationColors = blend;

                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            panelSideBar.Invalidate();
        }

        public string AuthenticatedUserName { get; set; }
        public string AuthenticatedUsertype
        {
            get { return authenticatedUsertype; }
            set
            {
                authenticatedUsertype = value;
                lblRole.Text = string.IsNullOrWhiteSpace(value)
                    ? "..."
                    : value;
                UpdateUserNavVisibility();
            }
        }
        public string AuthenticatedName
        {
            get { return authenticatedName; }
            set
            {
                authenticatedName = value;
                lblName.Text = string.IsNullOrWhiteSpace(value)
                    ? "Not Login"
                    : value;
                UpdateUserNavVisibility();
            }
        }

        private void UpdateUserNavVisibility()
        {
            panelUserNav.Visible = !string.IsNullOrWhiteSpace(authenticatedUsertype)
                && !string.IsNullOrWhiteSpace(authenticatedName);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure want to logout?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            foreach (Form child in MdiChildren.ToArray())
                child.Hide();

            AuthenticatedUserName = null;
            AuthenticatedUsertype = null;
            AuthenticatedName = null;

            panelMark.Visible = false;

            using (Form backdrop = new Form())
            {
                backdrop.FormBorderStyle = FormBorderStyle.None;
                backdrop.StartPosition = FormStartPosition.Manual;
                backdrop.ShowInTaskbar = false;
                backdrop.BackColor = System.Drawing.Color.Black;
                backdrop.Opacity = 0.45;
                backdrop.Bounds = Bounds;
                backdrop.Show(this);
                backdrop.BringToFront();

                using (LoginView loginView = new LoginView())
                {
                    if (loginView.ShowDialog(this) != DialogResult.OK)
                    {
                        Close();
                        return;
                    }

                    AuthenticatedUserName = loginView.AuthenticatedUserName;
                    AuthenticatedUsertype = loginView.AuthenticatedUsertype;
                    AuthenticatedName = loginView.AuthenticatedName;
                }
            }

            
        }


        private void NavButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            // Mark selected button
            panelMark.Visible = true;
            panelMark.Top = button.Top;
            panelMark.Height = button.Height;

            // Invoke view event
            if (button == btnDashboard)
            {
                ShowDashboardView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnInventory)
            {
                ShowInventoryView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnSales)
            {
                ShowSalesView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnPurchasing)
            {
                ShowPurchasingView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnUsers)
            {
                ShowUsersView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnExpenses)
            {
                ShowExpensesView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnCustomers)
            {
                ShowCustomersView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnSettings)
            {
                ShowSetupView?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnCashier)
            {
                foreach (Form child in MdiChildren.ToArray())
                    child.Hide();
                new CashierView(AuthenticatedUserName).ShowDialog();
            }
        }

        //Event
        public event EventHandler ShowDashboardView;
        public event EventHandler ShowInventoryView;
        public event EventHandler ShowSalesView;
        public event EventHandler ShowPurchasingView;
        public event EventHandler ShowUsersView;
        public event EventHandler ShowExpensesView;
        public event EventHandler ShowSetupView;
        public event EventHandler ShowCustomersView;

        //Timer function
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToLongDateString() + " " +
                              DateTime.Now.ToLongTimeString();
        }

        //Timer slider animation
        private void timerSidebar_Tick_Tick(object sender, EventArgs e)
        {
            if (sidebarCollapsed)
            {
                if (panelSideBar.Width > 83)
                    panelSideBar.Width -= 10;
                //else
                    //timerSidebar_Tick.Stop();
            }
            else
            {
                if (panelSideBar.Width < 216)
                    panelSideBar.Width += 10;
                //else
                    //timerSidebar_Tick.Stop();
            }
        }

        // slider no animation
        private void sideBar()
        {
            if (sidebarCollapsed)
            {
                if (panelSideBar.Width > 83)
                    panelSideBar.Width -= 129;
            }
            else
            {
                if (panelSideBar.Width < 216)
                    panelSideBar.Width += 129;
               
            }
        }
    }
}
