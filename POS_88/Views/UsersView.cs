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

namespace POS_88.Views
{
    public partial class UsersView : Form, IUsersView
    {
        private readonly SkeletonLoader usersSkeletonLoader;
        private readonly SkeletonLoader totalusersSkeletonLoader;
        private readonly BindingSource pagedUsersBindingSource = new BindingSource();
        private BindingSource usersSourceBindingSource;
        private int currentPageIndex;
        private int pageSize = 20;

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
        public UsersView()
        {
            InitializeComponent();
            ConfigureUsersGrid();
            usersSkeletonLoader = CreateSkeletonLoader(dataGridViewX1);
            totalusersSkeletonLoader = CreateSkeletonLoader(lblTotalQty, 100);

            cbBoxSelectCountEntries.SelectedIndexChanged += cbBoxSelectCountEntries_SelectedIndexChanged;
            btnPriveiwPage.Click += btnPriveiwPage_Click;
            btnNextPage.Click += btnNextPage_Click;
            cbBoxSelectCountEntries.SelectedIndex = 0;

            btnAddUser.Click += (sender, args) => 
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

                    using (AddOrEditUserView addView = new AddOrEditUserView())
                    {
                        new AddOrEditUserPresenter(
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
            btnSearch.Click += (sender, args) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnClearFilter.Click += (sender, args) => ClearFiltersEvent?.Invoke(this, EventArgs.Empty);
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
            RefreshUsersPage();
        }

        private void btnPriveiwPage_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                RefreshUsersPage();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            DataTable usersTable = usersSourceBindingSource == null
                ? null
                : usersSourceBindingSource.DataSource as DataTable;
            if (usersTable == null)
                return;

            int pageCount = GetPageCount(usersTable.Rows.Count);
            if (currentPageIndex + 1 < pageCount)
            {
                currentPageIndex++;
                RefreshUsersPage();
            }
        }

        private int GetPageCount(int rowCount)
        {
            return Math.Max(1, (int)Math.Ceiling((double)rowCount / pageSize));
        }

        private void RefreshUsersPage()
        {
            DataTable usersTable = usersSourceBindingSource == null
                ? null
                : usersSourceBindingSource.DataSource as DataTable;
            if (usersTable == null)
            {
                pagedUsersBindingSource.DataSource = new DataTable();
                lblShowEntriesCount.Text = "Showing 0 to 0 of 0 entries";
                btnPriveiwPage.Enabled = false;
                btnNextPage.Enabled = false;
                return;
            }

            int rowCount = usersTable.Rows.Count;
            int pageCount = GetPageCount(rowCount);
            currentPageIndex = Math.Max(0, Math.Min(currentPageIndex, pageCount - 1));

            int startIndex = currentPageIndex * pageSize;
            int endIndex = (int)Math.Min((long)rowCount, (long)startIndex + pageSize);
            DataTable pageTable = usersTable.Clone();
            for (int rowIndex = startIndex; rowIndex < endIndex; rowIndex++)
                pageTable.ImportRow(usersTable.Rows[rowIndex]);

            pagedUsersBindingSource.DataSource = pageTable;
            lblShowEntriesCount.Text = rowCount == 0
                ? "Showing 0 to 0 of 0 entries"
                : string.Format("Showing {0} to {1} of {2} entries",
                    startIndex + 1, endIndex, rowCount);
            btnPriveiwPage.Enabled = currentPageIndex > 0;
            btnNextPage.Enabled = currentPageIndex + 1 < pageCount;
        }

        private void UsersSourceBindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            currentPageIndex = 0;
            RefreshUsersPage();
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
            column.Width = 280;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewX1.Columns.Add(column);
        }

        private void dataGridViewX1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dataGridViewX1.Columns[e.ColumnIndex].Name != "Action")
                return;

            e.PaintBackground(e.CellBounds, true);
            string[] labels = { "Cash Count", "Edit", "Remove" };
            Color[] colors = { Color.YellowGreen, Color.Navy, Color.FromArgb(244, 67, 54) };
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

        private void dataGridViewX1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dataGridViewX1.Columns[e.ColumnIndex].Name != "Action" ||
                dataGridViewX1.Rows[e.RowIndex].IsNewRow)
                return;

            int actionIndex = GetClickedActionIndex(e.X, dataGridViewX1.Columns[e.ColumnIndex].Width);
            if (actionIndex < 0)
                return;

            EventHandler<UserActionEventArgs> action;

         
            switch (actionIndex)
            {
                case 0:
                    action = CashCountUserEvent;
                    break;
                case 1:
                    action = EditUserEvent;
                    break;
                default:
                    if (MessageBox.Show("Are you sure remove this user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    action = RemoveUserEvent;
                    break;
            }

            object value = dataGridViewX1.Rows[e.RowIndex].Cells["USERNAME"].Value;
            string userName = value == null || value == DBNull.Value
                ? string.Empty
                : Convert.ToString(value);
            action?.Invoke(this, new UserActionEventArgs(userName));

        }

        private int[] GetActionSegmentWidths(int cellWidth)
        {
            int cashCountWidth = cellWidth * 45 / 100;
            int editWidth = cellWidth * 25 / 100;
            return new[] { cashCountWidth, editWidth, cellWidth - cashCountWidth - editWidth };
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

        private void dataGridViewX1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridViewX1.Columns["Action"].DisplayIndex = dataGridViewX1.Columns.Count - 1;
        }


        public string AuthenticatedUserName { get; set; }


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

        public string TotalUsers
        {
            get { return lblTotalQty.Text; }
            set { lblTotalQty.Text = value ?? string.Empty; }
        }

        public event EventHandler ClearFiltersEvent;
        public event EventHandler SearchEvent;
        public event EventHandler<UserActionEventArgs> CashCountUserEvent;
        public event EventHandler<UserActionEventArgs> EditUserEvent;
        public event EventHandler<UserActionEventArgs> RemoveUserEvent;

        public void ClearFilters()
        {
            txtSearchParam.Text = string.Empty;
            this.txtSearchParam.Focus();
        }

        private void SetLoading(Control target, SkeletonLoader loader, bool isLoading)
        {
            loader.Visible = isLoading;
            target.Visible = !isLoading;
            if (isLoading)
                loader.BringToFront();
        }

        public void SetUsersBindingSource(BindingSource list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (usersSourceBindingSource != null)
                usersSourceBindingSource.ListChanged -= UsersSourceBindingSource_ListChanged;

            usersSourceBindingSource = list;
            usersSourceBindingSource.ListChanged += UsersSourceBindingSource_ListChanged;
            dataGridViewX1.DataSource = pagedUsersBindingSource;
            RefreshUsersPage();
        }

        public void ShowUsersLoading()
        {
            SetLoading(dataGridViewX1, usersSkeletonLoader, true);
            SetLoading(lblTotalQty, totalusersSkeletonLoader, true);
        }
        public void HideUsersLoading()
        {
            SetLoading(dataGridViewX1, usersSkeletonLoader, false);
            SetLoading(lblTotalQty, totalusersSkeletonLoader, false);
        }

        //Singleton pattern (Open a single form instance)
        private static UsersView instance;
        public static UsersView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new UsersView();
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
