using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class ImportStocksView : Form
    {
        private DataTable importedTable;

        public ImportStocksView()
        {
            InitializeComponent();
            ApplyTheme();
            btnClose.Click += (sender, args) => Close();
            btnMinimize.Click += (sender, args) => WindowState = FormWindowState.Minimized;
            btnBrowseExcel.Click += (sender, args) => BrowseFileEvent?.Invoke(this, EventArgs.Empty);
            btnLoadData.Click += (sender, args) => LoadDataEvent?.Invoke(this, EventArgs.Empty); // if the file selected on browse can you auto click this and load the data?
            btnImportData.Click += (sender, args) => ImportDataEvent?.Invoke(this, EventArgs.Empty);
            chkSelectAll.CheckedChanged += (sender, args) => SetRowsSelected(chkSelectAll.Checked);
            // Commit checkbox changes immediately
            dataGridViewX1.CurrentCellDirtyStateChanged += (sender, args) =>
            {
                if (dataGridViewX1.IsCurrentCellDirty)
                {
                    dataGridViewX1.CommitEdit(
                        DataGridViewDataErrorContexts.Commit);
                }
            };
            // Handle invalid cell values
            dataGridViewX1.DataError += (sender, args) =>
            {
                args.ThrowException = false;
                args.Cancel = true;
            };
            // Allow editing
            dataGridViewX1.ReadOnly = false;
            dataGridViewX1.AllowUserToAddRows = false;

            // Edit when typing/F2
            dataGridViewX1.EditMode =
                DataGridViewEditMode.EditOnKeystrokeOrF2;

            // Double-click a cell to edit
            dataGridViewX1.CellDoubleClick += (sender, args) =>
            {
                if (args.RowIndex >= 0 && args.ColumnIndex >= 0)
                {
                    // Don't edit ImportStatus
                    if (dataGridViewX1.Columns[args.ColumnIndex].Name
                        != "ImportStatus")
                    {
                        dataGridViewX1.BeginEdit(true);
                    }
                }
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

        public event EventHandler BrowseFileEvent;
        public event EventHandler LoadDataEvent;
        public event EventHandler ImportDataEvent;


        public string FilePath
        {
            get { return txtFilePath.Text; }
            set { txtFilePath.Text = value ?? string.Empty; }
        }

        public string ImportType
        {
            get { return Convert.ToString(cBoxImportType.SelectedItem); }
        }

        public DataGridViewRowCollection ImportRows
        {
            get { return dataGridViewX1.Rows; }
        }

        public void SetImportTypes(IEnumerable<string> importTypes)
        {
            cBoxImportType.Items.Clear();
            foreach (string importType in importTypes)
                cBoxImportType.Items.Add(importType);

            if (cBoxImportType.Items.Count > 0)
                cBoxImportType.SelectedIndex = 0;
        }

        public void SetSheetNames(IEnumerable<string> sheetNames)
        {
            txtSheetName.Items.Clear();
            foreach (string sheetName in sheetNames)
                txtSheetName.Items.Add(sheetName);

            if (txtSheetName.Items.Count > 0)
                txtSheetName.SelectedIndex = 0;
        }

        public string SelectedWorksheet
        {
            get { return Convert.ToString(txtSheetName.SelectedItem); }
        }

        public void SetPreviewData(DataTable table)
        {
            ClearPreviewData();

            importedTable = table;

            dataGridViewX1.DataSource = table;

            // Add Select checkbox column
            dataGridViewX1.Columns.Insert(
                0,
                new DataGridViewCheckBoxColumn
                {
                    Name = "ImportSelected",
                    HeaderText = "Select",
                    Width = 55,
                    ReadOnly = false
                });

            // Add Import Status column
            dataGridViewX1.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "ImportStatus",
                    HeaderText = "Import Status",
                    ReadOnly = true,
                    Width = 180
                });

            // Make Excel/imported data editable.
            // Only ImportStatus remains read-only.
            foreach (DataGridViewColumn column in dataGridViewX1.Columns)
            {
                column.ReadOnly = column.Name == "ImportStatus";
            }

            dataGridViewX1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dataGridViewX1.Columns["ImportStatus"].DisplayIndex = 1;
            dataGridViewX1.Columns["ImportStatus"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.None;

            // Select all rows if checkbox is already checked
            if (chkSelectAll.Checked)
            {
                SetRowsSelected(true);
            }
        }

        public void ClearPreviewData()
        {
            importedTable = null;
            dataGridViewX1.DataSource = null;
            dataGridViewX1.Columns.Clear();
            ShowNoData();
        }

        public void ShowNoData()
        {
            picBoxNoData.Visible = true;
            lblNoData.Visible = true;
            chkSelectAll.Visible = false;
        }

        public void HideNoData()
        {
            picBoxNoData.Visible = false;
            lblNoData.Visible = false;
            chkSelectAll.Visible = true;
        }

        public object GetSourceValue(DataGridViewRow row, string columnName)
        {
            DataColumn column = FindSourceColumn(columnName);
            if (column == null)
                throw new InvalidOperationException("The Excel sheet is missing the " + columnName + " column.");

            return row.Cells[column.Ordinal + 1].Value;
        }

        public bool HasSourceColumn(string columnName)
        {
            return FindSourceColumn(columnName) != null;
        }

        public void SetRowStatus(DataGridViewRow row, string status)
        {
            row.Cells["ImportStatus"].Value = status;
        }

        private void SetRowsSelected(bool selected)
        {
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
                row.Cells["ImportSelected"].Value = selected;
        }

        private DataColumn FindSourceColumn(string columnName)
        {
            if (importedTable == null)
                return null;

            foreach (DataColumn candidate in importedTable.Columns)
            {
                if (string.Equals(candidate.ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    return candidate;
            }
            return null;
        }
    }
}
