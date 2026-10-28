using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class AddStocksView : Form, IAddStocksView
    {
        public AddStocksView()
        {
            InitializeComponent();
            ApplyTheme();

            btnClose.Click += (sender, args) => Close();
            btnSave.Click += (sender, args) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearEvent?.Invoke(this, EventArgs.Empty);
            textBox3.TextChanged += (sender, args) => UpdateNewStocks();
            btnMinimize.Click += (sender, args) =>
            {
                WindowState = FormWindowState.Minimized;
            };
        }

        private void ApplyTheme()
        {
            // topbar gradients
            lblModalTitle.BackColor = Color.Transparent;
            POSTheme.ApplyTopbar(panel1);

            // button close
            btnMinimize.BackColor = POSTheme.Navy;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;

            // button close
            btnClose.BackColor = POSTheme.Danger;
            btnClose.ForeColor = Color.White;
            btnClose.UseVisualStyleBackColor = false;
        }

        public event EventHandler SaveEvent;
        public event EventHandler ClearEvent;

        public string ItemCode
        {
            get { return txtItemCode.Text; }
            set { txtItemCode.Text = value; }
        }

        public string ItemName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        public string CurrentQty
        {
            get { return txtUnit.Text; }
            set
            {
                txtUnit.Text = value;
                UpdateNewStocks();
            }
        }

        public decimal Quantity
        {
            get
            {
                decimal quantity;
                return decimal.TryParse(textBox3.Text, out quantity) ? quantity : 0M;
            }
            set { textBox3.Text = value.ToString(); }
        }

        public decimal NewStocks
        {
            get
            {
                decimal newStocks;
                return decimal.TryParse(textBox1.Text, out newStocks) ? newStocks : 0M;
            }
            set { textBox1.Text = value.ToString("N"); }
        }

        public string Remarks
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public void ClearFields()
        {
            Quantity = 0M;
            Remarks = string.Empty;
            UpdateNewStocks();
        }

        private void UpdateNewStocks()
        {
            decimal currentQty;
            decimal addedQty;
            decimal.TryParse(CurrentQty, out currentQty);
            decimal.TryParse(textBox3.Text, out addedQty);
            NewStocks = currentQty + addedQty;
        }


    }
}