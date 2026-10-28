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
    public partial class SetupView : Form, ISetupView
    {
        private readonly SkeletonLoader setupSkeletonLoader;
        private readonly SkeletonLoader totaldataSkeletonLoader;
        private readonly BindingSource pagedSetupBindingSource = new BindingSource();
        private BindingSource setupSourceBindingSource;
        private int currentPageIndex;
        private int pageSize = 20;

        public string SelectedSetup { get; set; }

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

        public SetupView()
        {
            InitializeComponent();
            ConfigureUsersGrid();
            setupSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totaldataSkeletonLoader = CreateSkeletonLoader(lblTotalList, 100);
            btnCategory.Click += NavButton_Click;
            btnManufacturer.Click += NavButton_Click;
            btnSupplier.Click += NavButton_Click;
            btnSearch.Click += (sender, args) => SearchSetupEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearSetupEvent?.Invoke(this, EventArgs.Empty);
            cbBoxSelectCountEntries.SelectedIndexChanged += cbBoxSelectCountEntries_SelectedIndexChanged;
            btnPriveiwPage.Click += btnPriveiwPage_Click;
            btnNextPage.Click += btnNextPage_Click;
            cbBoxSelectCountEntries.SelectedIndex = 0;
            SelectedSetup = "Category";

            btnAdd.Click += (sender, args) =>
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

                    switch (SelectedSetup)
                    {
                        case "Category":
                            using (AddOrEditCategoryView addView = new AddOrEditCategoryView())
                            {
                                new AddOrEditCategoryPresenter(
                                    addView,
                                    AuthenticatedUserName,
                                    null,
                                    () => SearchSetupEvent?.Invoke(this, EventArgs.Empty));
                                addView.StartPosition = FormStartPosition.CenterParent;
                                //addView.ShowDialog(modalOwner);
                                POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                            }
                            break;
                        case "Manufacturer":
                            using (AddOrEditManufacturerView addView = new AddOrEditManufacturerView())
                            {
                                new AddOrEditManufacturerPresenter(
                                    addView,
                                    AuthenticatedUserName,
                                    null,
                                    () => SearchSetupEvent?.Invoke(this, EventArgs.Empty));
                                addView.StartPosition = FormStartPosition.CenterParent;
                                //addView.ShowDialog(modalOwner);
                                POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                            }
                            break;
                        case "Supplier":
                            using (AddOrEditSupplierView addView = new AddOrEditSupplierView())
                            {
                                new AddOrEditSupplierPresenter(
                                    addView,
                                    AuthenticatedUserName,
                                    null,
                                    () => SearchSetupEvent?.Invoke(this, EventArgs.Empty));
                                addView.StartPosition = FormStartPosition.CenterParent;
                                //addView.ShowDialog(modalOwner);
                                POSTheme.ShowSmoothFadeDialog(addView, modalOwner);
                            }
                            break;
                    }
                    
                }
            };

            btnMyStore.Click += delegate {
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

                    new StoreSettingsView().ShowDialog();
                }
            };
        }

        private void cbBoxSelectCountEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEntryCount = Convert.ToString(cbBoxSelectCountEntries.SelectedItem);
            int selectedPageSize;
            pageSize = string.Equals(selectedEntryCount, "All", StringComparison.OrdinalIgnoreCase)
                ? int.MaxValue
                : int.TryParse(selectedEntryCount, out selectedPageSize) && selectedPageSize > 0
                    ? selectedPageSize
                    : 20;
            currentPageIndex = 0;
            RefreshSetupPage();
        }

        private void btnPriveiwPage_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                RefreshSetupPage();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            DataTable setupTable = setupSourceBindingSource == null
                ? null
                : setupSourceBindingSource.DataSource as DataTable;
            if (setupTable == null)
                return;

            int pageCount = GetPageCount(setupTable.Rows.Count);
            if (currentPageIndex + 1 < pageCount)
            {
                currentPageIndex++;
                RefreshSetupPage();
            }
        }

        private int GetPageCount(int rowCount)
        {

            return Math.Max(1, (int)Math.Ceiling((double)rowCount / pageSize));
        }

        private void RefreshSetupPage()
        {
            DataTable setupTable = setupSourceBindingSource == null
                ? null
                : setupSourceBindingSource.DataSource as DataTable;

            if (setupTable == null)
            {
                pagedSetupBindingSource.DataSource = new DataTable();
                lblShowEntriesCount.Text = "Showing 0 to 0 of 0 entries";
                btnPriveiwPage.Enabled = false;
                btnNextPage.Enabled = false;
                return;
            }

            int rowCount = setupTable.Rows.Count;
            int pageCount = GetPageCount(rowCount);
            currentPageIndex = Math.Max(0, Math.Min(currentPageIndex, pageCount - 1));

            int startIndex = currentPageIndex * pageSize;
            int endIndex = (int)Math.Min((long)rowCount, (long)startIndex + pageSize);
            DataTable pageTable = setupTable.Clone();
            for (int rowIndex = startIndex; rowIndex < endIndex; rowIndex++)
                pageTable.ImportRow(setupTable.Rows[rowIndex]);

            pagedSetupBindingSource.DataSource = pageTable;
            lblShowEntriesCount.Text = rowCount == 0
                ? "Showing 0 to 0 of 0 entries"
                : string.Format("Showing {0} to {1} of {2} entries",
                    startIndex + 1, endIndex, rowCount);
            btnPriveiwPage.Enabled = currentPageIndex > 0;
            btnNextPage.Enabled = currentPageIndex + 1 < pageCount;
        }

        private void SetupSourceBindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            currentPageIndex = 0;
            RefreshSetupPage();
        }

        private void ConfigureUsersGrid()
        {
            if (dataGridViewX1 != null)
            {
                dataGridViewX1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
                InitializeUserActionColumn();
                dataGridViewX1.DataBindingComplete += dataGridViewX1_DataBindingComplete;
                dataGridViewX1.CellPainting += dataGridViewX1_CellPainting;
                dataGridViewX1.CellMouseClick += dataGridViewX1_CellMouseClick;
            }
            dataGridViewX1.EnableHeadersVisualStyles = false;

            // Keep column headers unchanged when selected
            dataGridViewX1.ColumnHeadersDefaultCellStyle.BackColor =
                Color.White;

            dataGridViewX1.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.Black;

            dataGridViewX1.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                Color.White;

            dataGridViewX1.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                Color.Black;

            // Keep selected rows from turning orange
            dataGridViewX1.DefaultCellStyle.SelectionBackColor =
                Color.White;

            dataGridViewX1.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            // Prevent row-header selection from changing its colors
            dataGridViewX1.RowHeadersDefaultCellStyle.SelectionBackColor =
                dataGridViewX1.RowHeadersDefaultCellStyle.BackColor;

            dataGridViewX1.RowHeadersDefaultCellStyle.SelectionForeColor =
                dataGridViewX1.RowHeadersDefaultCellStyle.ForeColor;
            dataGridViewX1.RowTemplate.Height = 30;
            dataGridViewX1.AllowUserToResizeRows = false;
        }

        private void InitializeUserActionColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Action";
            column.HeaderText = "Action";
            column.Width = 180;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewX1.Columns.Add(column);
        }

        private void dataGridViewX1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridViewX1.Columns["Action"].DisplayIndex = dataGridViewX1.Columns.Count - 1;
        }


        private void dataGridViewX1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dataGridViewX1.Columns[e.ColumnIndex].Name != "Action")
                return;

            e.PaintBackground(e.CellBounds, true);
            string[] labels = { "Edit", "Remove" };
            Color[] colors = { Color.Navy, Color.FromArgb(244, 67, 54) };
            int[] segmentWidths = GetActionSegmentWidths(e.CellBounds.Width);
            int x = e.CellBounds.Left;
            for (int i = 0; i < labels.Length; i++)
            {
                Rectangle buttonBounds = new Rectangle(
                    x + 2,
                    e.CellBounds.Top + 2,
                    Math.Max(0, segmentWidths[i] - 4),
                    Math.Max(0, e.CellBounds.Height - 4));
                using (SolidBrush brush = new SolidBrush(colors[i]))
                    e.Graphics.FillRectangle(brush, buttonBounds);
                ControlPaint.DrawBorder(e.Graphics, buttonBounds,
                    ControlPaint.Dark(colors[i]), ButtonBorderStyle.Solid);
                TextRenderer.DrawText(e.Graphics, labels[i], e.CellStyle.Font,
                    buttonBounds, Color.White,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine);
                x += segmentWidths[i];
            }
            e.Handled = true;
        }

        private int[] GetActionSegmentWidths(int cellWidth)
        {
            int editWidth = cellWidth * 50 / 100;
            return new[] { editWidth, cellWidth - editWidth };
        }

        private int GetClickedActionIndex(int x, int cellWidth)
        {
            int[] widths = GetActionSegmentWidths(cellWidth);
            int segmentStart = 0;
            for (int i = 0; i < widths.Length; i++)
            {
                if (x >= segmentStart + 2 && x < segmentStart + widths[i] - 2)
                    return i;
                segmentStart += widths[i];
            }
            return -1;
        }

        private void dataGridViewX1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dataGridViewX1.Columns[e.ColumnIndex].Name != "Action" ||
                dataGridViewX1.Rows[e.RowIndex].IsNewRow)
                return;

            int actionIndex = GetClickedActionIndex(e.X, dataGridViewX1.Columns[e.ColumnIndex].Width);
            if (actionIndex < 0)
                return;

            EventHandler<SetupActionEventArgs> action;
            object value = dataGridViewX1.Rows[e.RowIndex].Cells["ID"].Value;
            string id = value == null || value == DBNull.Value
                ? string.Empty
               : Convert.ToString(value);

            switch (actionIndex)
            {
                case 0:
                    action = EditSetupEvent;
                    break;
                default:
                    if (MessageBox.Show(string.Format("Are you sure remove this {0} record?\nID: {1}",SelectedSetup.ToLower(), id),
                        "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    action = RemoveSetupEvent;
                    break;
            }
            
            action?.Invoke(this, new SetupActionEventArgs(id));

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
            if (button == btnCategory)
            {
                SelectedSetup = "Category";
                SearchSetupEvent?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnManufacturer)
            {
                SelectedSetup = "Manufacturer";
                SearchSetupEvent?.Invoke(this, EventArgs.Empty);
            }
            else if (button == btnSupplier)
            {
                SelectedSetup = "Supplier";
                SearchSetupEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearFilters()
        {
            txtSearchParam.Text = string.Empty;
            this.txtSearchParam.Focus();
        }

        //Singleton pattern (Open a single form instance)
        private static SetupView instance;

        public string SearchParam
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

        public string AuthenticatedUserName { get; set; }

        public string TotalList
        {
            get
            {
                return lblTotalList.Text;
            }

            set
            {
                lblTotalList.Text = value;
            }
        }

        public event EventHandler SearchSetupEvent;
        public event EventHandler ClearSetupEvent;
        public event EventHandler<SetupActionEventArgs> EditSetupEvent;
        public event EventHandler<SetupActionEventArgs> RemoveSetupEvent;

        public static SetupView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new SetupView();
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

        public void SetSetupBindingSource(BindingSource list)
        {
            dataGridViewX1.DataSource = list;


            if (list == null)
                throw new ArgumentNullException("list");

            if (setupSourceBindingSource != null)
                setupSourceBindingSource.ListChanged -= SetupSourceBindingSource_ListChanged;

            setupSourceBindingSource = list;
            setupSourceBindingSource.ListChanged += SetupSourceBindingSource_ListChanged;
            dataGridViewX1.DataSource = pagedSetupBindingSource;
            RefreshSetupPage();
        }

        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }


        public void ShowDataLoading()
        {
            SetLoading(dataGridViewX1, setupSkeletonLoader, true);
            SetLoading(lblTotalList, totaldataSkeletonLoader, true);
        }

        public void HideDataLoading()
        {
            SetLoading(dataGridViewX1, setupSkeletonLoader, false);
            SetLoading(lblTotalList, totaldataSkeletonLoader, false);
        }

        public void Test()
        {
            throw new NotImplementedException();
        }
    }
}
