using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using ClosedXML.Excel;

using POS_88.Presenters;

namespace POS_88.Views
{
    public partial class PurchaseOrderView : Form, IPurchaseOrderView
    {
        private readonly SkeletonLoader purchaseSkeletonLoader;
        private readonly SkeletonLoader totalGrandSkeletonLoader;
        private readonly SkeletonLoader totalOrdersSkeletonLoader;
        private readonly SkeletonLoader totalQtySkeletonLoader;

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

        public PurchaseOrderView()
        {
            InitializeComponent();

            purchaseSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totalGrandSkeletonLoader = CreateSkeletonLoader(lblGrandTotal, 110);
            totalOrdersSkeletonLoader = CreateSkeletonLoader(lblTotalOrders, 110);
            totalQtySkeletonLoader = CreateSkeletonLoader(lblTotalQty, 100);
            
            if (dataGridViewX1 != null)
            {
                dataGridViewX1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }

            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearFiltersEvent?.Invoke(this, EventArgs.Empty);

            btnAddPurchaseOrder.Click += (sender, args) =>
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

                    using (AddOrEditPurchaseOrderView addView = new AddOrEditPurchaseOrderView())
                    {
                        new AddOrEditPurchaseOrderPresenter(
                            addView,
                            AuthenticatedUserName,
                            null,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addView.StartPosition = FormStartPosition.CenterParent;
                        //addView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                    }
                }
            };

            btnEditPurchaseOrder.Click += (sender, args) =>
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an record before editing.", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (AddOrEditPurchaseOrderView addView = new AddOrEditPurchaseOrderView())
                    {
                        new AddOrEditPurchaseOrderPresenter(
                            addView,
                            AuthenticatedUserName,
                            this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString(),
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addView.StartPosition = FormStartPosition.CenterParent;
                        //addView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                    }
                }
            };

            btnPrint.Click += (sender, args) =>
            {
                ArrayList paramArrayList = new ArrayList();
                ArrayList paramValueArrayList = new ArrayList();
                paramArrayList.Add((object)"@OrderID");
                paramValueArrayList.Add((object)this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString());
                ReportViewerView reportView = new ReportViewerView(
                    "Orders.rpt", paramArrayList, paramValueArrayList);
                try
                {
                    new ReportViewerPresenter(reportView).LoadReport();
                    reportView.Show();
                }
                catch (Exception exception)
                {
                    reportView.Dispose();
                    MessageBox.Show(exception.Message, "Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnExportExcel.Click += (s, e) => ExportExcelEvent?.Invoke(this, EventArgs.Empty);

        }




        public string AuthenticatedUserName { get; set; }

        public event EventHandler SearchEvent;
        public event EventHandler ExportExcelEvent;
        public event EventHandler ClearFiltersEvent;

        //Singleton pattern (Open a single form instance)
        private static PurchaseOrderView instance;
        public static PurchaseOrderView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new PurchaseOrderView();
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

        public void ClearFilters()
        {
            txtSearchParam.Text = string.Empty;
            cbBoxIsReceived.Text = string.Empty;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;  
        }
        public string SearchParam
        {
            get
            {
                return (string)txtSearchParam.Text;
            }

            set
            {
                txtSearchParam.Text = value;
            }
        }

        public string IsReceived
        {
           get
            {
                return (string)cbBoxIsReceived.Text;
            }

            set
            {
                cbBoxIsReceived.Text = value;
            }
        }

        public DateTime DateOrderFrom
        {
            get { return dtpFrom.Value; }
            set
            {
               dtpFrom.Value = value;
            }
        }

         public string GrandTotal
        {
            get { return lblGrandTotal.Text; }
            set { lblGrandTotal.Text = value ?? string.Empty; }
        }

        public string TotalOrders
        {
            get { return lblTotalOrders.Text; }
            set { lblTotalOrders.Text = value ?? string.Empty; }
        }

        public string TotalQty
        {
            get { return lblTotalQty.Text; }
            set { lblTotalQty.Text = value ?? string.Empty; }
        }

        public DateTime DateOrderTo
        {
            get { return dtpTo.Value; }
            set
            {
              dtpTo.Value = value;
            }
        }

        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }

        public void ShowPurchaseOrderLoading()
        {
            SetLoading(dataGridViewX1, purchaseSkeletonLoader, true);
            SetLoading(lblGrandTotal, totalGrandSkeletonLoader, true);
            SetLoading(lblTotalOrders, totalOrdersSkeletonLoader, true);
            SetLoading(lblTotalQty, totalQtySkeletonLoader, true);
        }

        public void HidePurchaseOrderLoading()
        {
            SetLoading(dataGridViewX1, purchaseSkeletonLoader, false);
            SetLoading(lblGrandTotal, totalGrandSkeletonLoader, false);
            SetLoading(lblTotalOrders, totalOrdersSkeletonLoader, false);
            SetLoading(lblTotalQty, totalQtySkeletonLoader, false);
        }

        public void SetPurchaseOrderListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }

        public void SetIsReceivedList(List<string> list)
        {
            cbBoxIsReceived.DataSource = list;
        }
    }
}
