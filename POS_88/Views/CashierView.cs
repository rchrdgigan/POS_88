using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class CashierView : Form
    {
        public string SelectedType { get; set; }
        public CashierView()
        {
            InitializeComponent();
            ApplyTheme();

            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };


            btnRetail.Click += NavButton_Click;
            btnWholesale.Click += NavButton_Click;
            btnPromo.Click += NavButton_Click;
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            // Mark selected button
            panelMark.Visible = true;
            panelMark.Location = new Point(button.Location.X, button.Location.Y + button.Height - panelMark.Height);
            panelMark.Width = button.Width;

            // Invoke view event
            if (button == btnRetail)
            {
                SelectedType = "Retail";
                //SearchEvent?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnWholesale)
            {
                SelectedType = "Wholesale";
                //SearchEvent?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnPromo)
            {
                SelectedType = "Promo";
                //SearchEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ApplyTheme()
        {
            // Sidebar and topbar gradients
            POSTheme.ApplySidebar(panelLogo);
            POSTheme.ApplySidebar(bunifuCardsSidebar);
            POSTheme.ApplyTopbar(panel2);
            POSTheme.ApplyTopbar(panelBottom);

            // Topbar text and controls
            lblDateTime.ForeColor = POSTheme.Text;
            lblDateTime.BackColor = Color.Transparent;

            btnMinimize.BackColor = POSTheme.Navy;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.UseVisualStyleBackColor = false;

            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToLongDateString() + " " +
                              DateTime.Now.ToLongTimeString();
        }
    }
}
