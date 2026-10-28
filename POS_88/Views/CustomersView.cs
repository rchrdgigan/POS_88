using POS_88.Presenters;
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
    public partial class CustomersView : Form, ICustomersView
    {
        private readonly SkeletonLoader customersSkeletonLoader;
        private readonly SkeletonLoader totalcustomersSkeletonLoader;

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
        public CustomersView()
        {
            InitializeComponent();

            customersSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totalcustomersSkeletonLoader = CreateSkeletonLoader(lblTotalCustomers, 100);

            btnClearFilter.Click += (sender, args) =>
            {
                ClearEvent?.Invoke(this, EventArgs.Empty);
            };
            btnDelete.Click += (sender, args) => DeleteEvent?.Invoke(this, EventArgs.Empty);
            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);

            btnAddNew.Click += (sender, args) =>
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

                    using (AddOrEditCustomersView addView = new AddOrEditCustomersView())
                    {
                        new AddOrEditCustomersPresenter(
                            addView,
                            null,
                            null,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addView.StartPosition = FormStartPosition.CenterParent;
                        //addView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                    }
                }
            };

            btnEdit.Click += (sender, args) =>
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an customer before editing.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                    using (AddOrEditCustomersView editView = new AddOrEditCustomersView())
                    {
                        new AddOrEditCustomersPresenter(
                            editView,
                            null,
                            this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString(),
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        editView.StartPosition = FormStartPosition.CenterParent;
                        //editView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(editView, modalOwner);
                    }
                }
            };

            btnSalesHistory.Click += (sender, args) =>
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

                    using (SalesHistoryView viewSales = new SalesHistoryView())
                    {
                        new SalesHistoryPresenter(
                            viewSales,
                            this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString());
                        viewSales.StartPosition = FormStartPosition.CenterParent;
                        //addView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(viewSales, modalOwner);
                    }
                }
            };

            dataGridViewX1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public string SearchParms
        {
            get
            {
                return txtSearchParam.Text;
            }

            set
            {
                txtSearchParam.Text = value;
            }
        }

        public string SelectedCustomers
        {
            get
            {
                if (dataGridViewX1 == null || dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells.Count == 0)
                    return string.Empty;

                var value = dataGridViewX1.CurrentRow.Cells[0].Value;
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }

        public string TotalCustomers
        {
            get { return lblTotalCustomers.Text; }
            set { lblTotalCustomers.Text = value; }
        }

        public event EventHandler ClearEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SearchEvent;

        public void ClearFilters()
        {
            this.SearchParms = string.Empty;
        }

        public void HideCustomersListLoading()
        {
            SetLoading(dataGridViewX1, customersSkeletonLoader, false);
            SetLoading(lblTotalCustomers, totalcustomersSkeletonLoader, false);
        }

        public void SetCustomersListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }

        public void ShowCustomersListLoading()
        {
            SetLoading(dataGridViewX1, customersSkeletonLoader, true);
            SetLoading(lblTotalCustomers, totalcustomersSkeletonLoader, true);
        }

        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }


        private static CustomersView instance;
        public static CustomersView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new CustomersView();
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
