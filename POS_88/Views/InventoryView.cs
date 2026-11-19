using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Presenters;
using System.Collections;
using ClosedXML.Excel;

namespace POS_88.Views
{
    public partial class InventoryView : Form, IInventoryView
    {
        private readonly SkeletonLoader categorySkeletonLoader;
        private readonly SkeletonLoader supplierSkeletonLoader;
        private readonly SkeletonLoader vatStatusSkeletonLoader;
        private readonly SkeletonLoader availabilitySkeletonLoader;
        private readonly SkeletonLoader inventorySkeletonLoader;
        private readonly SkeletonLoader totalCostSkeletonLoader;
        private readonly SkeletonLoader totalPriceSkeletonLoader;
        private readonly SkeletonLoader totalQuantitySkeletonLoader;
        private readonly SkeletonLoader noOfItemsSkeletonLoader;
        public bool IsCashierMode { get; private set; }


        public InventoryView()
        {
            InitializeComponent();

            categorySkeletonLoader = CreateSkeletonLoader(cbBoxCategory);
            supplierSkeletonLoader = CreateSkeletonLoader(cbBoxSupplier);
            vatStatusSkeletonLoader = CreateSkeletonLoader(cbBoxVatStat);
            availabilitySkeletonLoader = CreateSkeletonLoader(cbBoxAvailability);
            inventorySkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totalCostSkeletonLoader = CreateSkeletonLoader(lblCost, 110);
            totalPriceSkeletonLoader = CreateSkeletonLoader(lblPrice, 110);
            totalQuantitySkeletonLoader = CreateSkeletonLoader(lblQty, 100);
            noOfItemsSkeletonLoader = CreateSkeletonLoader(lblNoItems, 70);

            if (dataGridViewX1 != null)
            {
                dataGridViewX1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (sender, args) => DeleteEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearFiltersEvent?.Invoke(this, EventArgs.Empty);
            btnBarcode.Click += (sender, args) => 
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an item before viewing barcode.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ArrayList paramArrayList = new ArrayList();
                ArrayList paramValueArrayList = new ArrayList();
                string str = this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString();
                paramArrayList.Add((object)"@ItemCode");
                paramValueArrayList.Add((object)str);

                ReportViewerView reportView = new ReportViewerView(
                    "BARCODE2.rpt",
                    paramArrayList,
                    paramValueArrayList);
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


            //Show modal with backdrop opacity
            btnAddItem.Click += (sender, args) =>
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

                    using (AddOrEditItemView addItemView = new AddOrEditItemView())
                    {
                        new AddOrEditItemPresenter(
                            addItemView,
                            AuthenticatedUserName, // auth username
                            null,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addItemView.StartPosition = FormStartPosition.CenterParent;
                        //addItemView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addItemView, modalOwner);
                    }
                }
            };

            btnAddStock.Click += (sender, args) =>
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an item before adding stocks.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string itemCode = dataGridViewX1.CurrentRow.Cells[0].Value.ToString();
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

                    using (AddStocksView addStockView = new AddStocksView())
                    {
                        new AddStocksPresenter(
                            addStockView,
                            itemCode,
                            AuthenticatedUserName,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        addStockView.StartPosition = FormStartPosition.CenterParent;
                        //addStockView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(addStockView, modalOwner);
                    }
                }
            };

            btnEdit.Click += (sender, args) =>
            {
                if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Select an item before editing.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (AddOrEditItemView editItemView = new AddOrEditItemView())
                    {
                        new AddOrEditItemPresenter(
                            editItemView,
                            AuthenticatedUserName,
                            this.dataGridViewX1.CurrentRow.Cells[0].Value.ToString(),
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        editItemView.StartPosition = FormStartPosition.CenterParent;
                        //editItemView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(editItemView, modalOwner);
                    }
                }
            };

            btnPrint.Click += (sender, args) => {
                ArrayList paramArrayList = new ArrayList();
                ArrayList paramValueArrayList = new ArrayList();
                string str = Convert.ToString(this.cbBoxCategory.SelectedValue);
                paramArrayList.Add((object)"@CategoryID");
                paramValueArrayList.Add((object)str);

                ReportViewerView reportView = new ReportViewerView(
                    "Inventory.rpt",
                    paramArrayList,
                    paramValueArrayList);
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

            btnExportExcel.Click += delegate { ExporttoExcel(); };

            btnStockHistory.Click += (sender, args) =>
            {
                string itemCode = SelectedItemCode;
                if (string.IsNullOrWhiteSpace(itemCode))
                {
                    MessageBox.Show("Select an item to view its stock history.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    using (StockHistoryView stockHistoryView = new StockHistoryView())
                    {
                        new StockHistoryPresenter(stockHistoryView, itemCode);
                        stockHistoryView.StartPosition = FormStartPosition.CenterParent;
                        //stockHistoryView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(stockHistoryView, modalOwner);
                    }
                }
            };

            btnImportStocks.Click += (sender, args) =>
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

                    using (ImportStocksView importStocksView = new ImportStocksView())
                    {
                        new ImportStocksPresenter(
                            importStocksView,
                            AuthenticatedUserName,
                            () => SearchEvent?.Invoke(this, EventArgs.Empty));
                        importStocksView.StartPosition = FormStartPosition.CenterParent;
                        //importStocksView.ShowDialog(modalOwner);
                        POSTheme.ShowSmoothFadeDialog(importStocksView, modalOwner);
                    }
                }
            };

        }
        //how to apply this configuration when trigger 2 params in cashier then it will open this inventory form not in admin panel but in cashier to show only the stocks?
        public InventoryView(string usernm, string cashierTrap) : this()
        {
            IsCashierMode = true;
            AuthenticatedUserName = usernm;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            //MaximizeBox = false;
            //MinimizeBox = false;
            //ShowInTaskbar = false;
            Height = 709;
            Width = 1105;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Inventory";

            // view-only: hide instead of disable so the cashier doesn't see dead buttons
            btnAddItem.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnPrint.Visible = false;
            btnImportStocks.Visible = false;
            btnStockHistory.Visible = false;
            btnExportExcel.Visible = false;

            tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Absolute;
            tableLayoutPanel1.ColumnStyles[1].Width = 100F;

            btnAddStock.Visible = true;
            btnAddStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddStock.Location = new Point(panel5.Width - btnAddStock.Width - 7, 7);

            dataGridViewX1.ReadOnly = true;
            dataGridViewX1.AllowUserToAddRows = false;
            dataGridViewX1.AllowUserToDeleteRows = false;
        }

        private void ExporttoExcel()
        {
            var cols = dataGridViewX1.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
            var rows = dataGridViewX1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();

            if (cols.Count == 0 || rows.Count == 0)
            {
                MessageBox.Show("There is no inventory to export.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog
            {
                Filter = "Excel workbook|*.xlsx",
                Title = "Export Inventory",
                FileName = "Inventory.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    UseWaitCursor = true;
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Inventory");

                        for (int c = 0; c < cols.Count; c++)
                        {
                            ws.Cell(1, c + 1).Value = cols[c].HeaderText;
                            bool isCode = "ITEM_CODE".Equals(cols[c].DataPropertyName, StringComparison.OrdinalIgnoreCase)
                                       || "ITEM_CODE".Equals(cols[c].HeaderText, StringComparison.OrdinalIgnoreCase);

                            for (int r = 0; r < rows.Count; r++)
                            {
                                object raw = rows[r].Cells[cols[c].Index].Value;
                                object v = raw == null || raw == DBNull.Value ? string.Empty : raw;
                                var cell = ws.Cell(r + 2, c + 1);

                                if (isCode)
                                {
                                    string s = Convert.ToString(v);
                                    cell.Value = s == "" ? "" : "'" + s;
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else cell.Value = v;
                            }
                        }

                        var table = ws.Range(1, 1, rows.Count + 1, cols.Count).CreateTable("InventoryTable");
                        table.Theme = XLTableTheme.TableStyleMedium2;
                        ws.SheetView.FreezeRows(1);
                        ws.Columns().AdjustToContents();
                        wb.SaveAs(dlg.FileName);
                    }
                    MessageBox.Show("Inventory exported successfully.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not export inventory. " + ex.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { UseWaitCursor = false; }
            }
        }

        public event EventHandler SearchEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler ClearFiltersEvent;

        public string AuthenticatedUserName { get; set; }

        public string TotalCost
        {
            set
            {
                lblCost.Text = value;
            }
        }

        public string TotalPrice
        {
            set
            {
                lblPrice.Text = value;
            }
        }

        public string TotalQuantity
        {
            set
            {
                lblQty.Text = value;
            }
        }

        public string NoOfItems
        {
            set
            {
                lblNoItems.Text = value;
            }
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

        public string CategoryId
        {
            get
            {
                return cbBoxCategory.SelectedValue == null ? string.Empty : Convert.ToString(cbBoxCategory.SelectedValue);
            }

            set
            {
                cbBoxCategory.Text = value;
            }
        }

        public string SupplierId
        {
            get
            {
                return cbBoxSupplier.SelectedValue == null ? string.Empty : Convert.ToString(cbBoxSupplier.SelectedValue);
            }

            set
            {
                cbBoxSupplier.Text = value;
            }
        }

        public string VatStatus
        {
            get
            {
                return cbBoxVatStat.SelectedValue == null ? string.Empty : Convert.ToString(cbBoxVatStat.SelectedValue);
            }

            set
            {
                cbBoxVatStat.Text = value;
            }
        }

        public string Availability
        {
            get
            {
                return (string)cbBoxAvailability.Text;
            }

            set
            {
                cbBoxAvailability.Text = value;
            }
        }

        public string SelectedItemCode
        {
            get
            {
                if (dataGridViewX1 == null || dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.Cells.Count == 0)
                    return string.Empty;

                var value = dataGridViewX1.CurrentRow.Cells[0].Value;
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }


        //Singleton pattern (Open a single form instance)
        private static InventoryView instance;
        public static InventoryView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new InventoryView();
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

        public void SetCategoryList(BindingSource list)
        {
            cbBoxCategory.DataSource = list;
            cbBoxCategory.DisplayMember = "categoryname";
            cbBoxCategory.ValueMember = "categoryid";
            cbBoxCategory.SelectedIndex = -1;
        }

        public void SetSupplierList(BindingSource list)
        {
            cbBoxSupplier.DataSource = list;
            cbBoxSupplier.DisplayMember = "suppliername";
            cbBoxSupplier.ValueMember = "supplierid";
            cbBoxSupplier.SelectedIndex = -1;
        }

        public void SetVatStatusList(BindingSource list)
        {
            cbBoxVatStat.DataSource = list;
            cbBoxVatStat.DisplayMember = "vatname";
            cbBoxVatStat.ValueMember = "vatid";
            cbBoxVatStat.SelectedIndex = -1;
        }

        public void SetAvailabilityList(List<string> list)
        {
            cbBoxAvailability.DataSource = null;
            cbBoxAvailability.Items.Clear();

            foreach (string item in list)
            {
                cbBoxAvailability.Items.Add(item);
            }

            cbBoxAvailability.SelectedIndex = 0;
        }

        public void ClearFilters()
        {
            cbBoxCategory.SelectedIndex = -1;
            cbBoxCategory.Text = string.Empty;
            cbBoxSupplier.SelectedIndex = -1;
            cbBoxSupplier.Text = string.Empty;
            cbBoxVatStat.SelectedIndex = -1;
            cbBoxVatStat.Text = string.Empty;
            if (cbBoxAvailability.Items.Count > 0)
                cbBoxAvailability.SelectedIndex = 0;
            else
                cbBoxAvailability.Text = string.Empty;
            SearchParam = string.Empty;
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

        public void ShowFilterLoading()
        {
            SetLoading(cbBoxCategory, categorySkeletonLoader, true);
            SetLoading(cbBoxSupplier, supplierSkeletonLoader, true);
            SetLoading(cbBoxVatStat, vatStatusSkeletonLoader, true);
            SetLoading(cbBoxAvailability, availabilitySkeletonLoader, true);
        }

        public void HideFilterLoading()
        {
            SetLoading(cbBoxCategory, categorySkeletonLoader, false);
            SetLoading(cbBoxSupplier, supplierSkeletonLoader, false);
            SetLoading(cbBoxVatStat, vatStatusSkeletonLoader, false);
            SetLoading(cbBoxAvailability, availabilitySkeletonLoader, false);
        }

        public void ShowInventoryLoading()
        {
            SetLoading(dataGridViewX1, inventorySkeletonLoader, true);
            SetLoading(lblCost, totalCostSkeletonLoader, true);
            SetLoading(lblPrice, totalPriceSkeletonLoader, true);
            SetLoading(lblQty, totalQuantitySkeletonLoader, true);
            SetLoading(lblNoItems, noOfItemsSkeletonLoader, true);
        }

        public void HideInventoryLoading()
        {
            SetLoading(dataGridViewX1, inventorySkeletonLoader, false);
            SetLoading(lblCost, totalCostSkeletonLoader, false);
            SetLoading(lblPrice, totalPriceSkeletonLoader, false);
            SetLoading(lblQty, totalQuantitySkeletonLoader, false);
            SetLoading(lblNoItems, noOfItemsSkeletonLoader, false);
        }

        public void SetInventoryListBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;
        }
    }
}
