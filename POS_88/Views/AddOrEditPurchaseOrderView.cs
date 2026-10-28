using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class AddOrEditPurchaseOrderView : Form, IAddOrEditPurchaseOrderView
    {
        private const string ItemCodeDesc = "ITEM CODE & DESCRIPTION";
        private const string Barcode = "BARCODE";

        public string SuppId { get; set; }

        public event EventHandler SaveEvent;
        public event EventHandler SearchSupplierEvent;
        public event EventHandler SearchItemsEvent;
        public event EventHandler PrintEvent;
        public event EventHandler ReceivedEvent;
        public event EventHandler<ItemEventArgs> AddItemEvent;

        public AddOrEditPurchaseOrderView()
        {
            InitializeComponent();
            InitializeControls();
            InitializeGrid();
            ApplyTheme();

            btnClose.Click += (s, e) => Close();
            btnCancel.Click += (s, e) => Close();
            btnMinimize.Click += (s, e) => WindowState = FormWindowState.Minimized;
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnRemove.Click += btnRemove_Click;
            btnPrint.Click += (s, e) => PrintEvent?.Invoke(this, EventArgs.Empty);
            btnReceived.Click += (s, e) => ReceivedEvent?.Invoke(this, EventArgs.Empty);

            txtSearchItems.TextChanged += txtSearch_TextChanged;
            txtSearchItems.KeyDown += txtSearch_KeyDown;
            listViewItems.KeyDown += listViewItems_KeyDown;
            listViewItems.MouseDown += listViewItems_MouseDown;
            searchTimer.Tick += searchTimer_Tick;

            cBoxSupplier.SelectedIndexChanged += SupplierSelectedIndexChanged;

            dataGridViewX1.CellEndEdit += dataGridViewX1_CellEndEdit;
            dataGridViewX1.KeyDown += dataGridViewX1_KeyDown;
            dataGridViewX1.CellDoubleClick += dataGridViewX1_CellDoubleClick;
            dataGridViewX1.CellValidating += dataGridViewX1_CellValidating;
            dataGridViewX1.DataError += (s, e) => e.ThrowException = false;


            this.CalculateTotal();
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


        #region Initialization

        private void InitializeGrid()
        {
            dataGridViewX1.ReadOnly = false;   // grid-level ReadOnly overrides column ReadOnly
            dataGridViewX1.Enabled = true;

            dataGridViewX1.AutoGenerateColumns = false;
            dataGridViewX1.AllowUserToAddRows = false;

            if (dataGridViewX1.Columns.Count == 0)
            {
                dataGridViewX1.Columns.Add("ItemCode", "Item Code");
                dataGridViewX1.Columns.Add("ItemName", "Item Name");
                dataGridViewX1.Columns.Add("Quantity", "Available Stock");
                dataGridViewX1.Columns.Add("Qty", "Order Quantity");
                dataGridViewX1.Columns.Add("Price", "Price");
                dataGridViewX1.Columns.Add("Total", "Total");
            }

            foreach (DataGridViewColumn col in dataGridViewX1.Columns)
                col.ReadOnly = true;

            // editable columns
            dataGridViewX1.Columns["Qty"].ReadOnly = false;
            dataGridViewX1.Columns["Price"].ReadOnly = false;

            // value types (so the number format is applied)
            dataGridViewX1.Columns["Quantity"].ValueType = typeof(decimal);
            dataGridViewX1.Columns["Qty"].ValueType = typeof(decimal);
            dataGridViewX1.Columns["Price"].ValueType = typeof(decimal);
            dataGridViewX1.Columns["Total"].ValueType = typeof(decimal);

            // number formatting / alignment
            dataGridViewX1.Columns["Quantity"].DefaultCellStyle.Format = "N2";
            dataGridViewX1.Columns["Qty"].DefaultCellStyle.Format = "N2";
            dataGridViewX1.Columns["Price"].DefaultCellStyle.Format = "N2";
            dataGridViewX1.Columns["Total"].DefaultCellStyle.Format = "N2";

            dataGridViewX1.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewX1.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewX1.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewX1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridViewX1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewX1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void listViewItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ListViewItem hit = listViewItems.HitTest(e.Location).Item;
            if (hit == null || cBoxSearchType.Text != ItemCodeDesc) return;

            string code = hit.Name;            // ITEM_CODE (the key used in SetSearchResults)
            listViewItems.Visible = false;     // close the dropdown
            RaiseAddItem(code);                // presenter looks it up and adds the row to dataGridViewX1
        }

        private void InitializeControls()
        {
            listViewItems.BringToFront();
            listViewItems.Width = txtSearchItems.Width;
            listViewItems.Location = new Point(listViewItems.Location.X, txtSearchItems.Location.Y + 40);
            listViewItems.View = View.Details;
            listViewItems.FullRowSelect = true;
            listViewItems.HoverSelection = true;
            listViewItems.Font = new Font("Century Gothic", 9f, FontStyle.Regular);
            listViewItems.HeaderStyle = ColumnHeaderStyle.None;
            listViewItems.Height = 380;
            listViewItems.Columns.Add("Name");
            listViewItems.Columns.Add("Price");
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewItems.BorderStyle = BorderStyle.FixedSingle;
            listViewItems.Visible = false;
        }

        #endregion

        #region Item search

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            if (txtSearchItems.Text != "")
            {
                SearchItemsEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                listViewItems.Items.Clear();
                listViewItems.Visible = false;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                if (cBoxSearchType.Text == Barcode && txtSearchItems.Text != "")
                {
                    RaiseAddItem(txtSearchItems.Text);
                }
                else if (cBoxSearchType.Text == ItemCodeDesc
                         && txtSearchItems.Text != ""
                         && listViewItems.Visible
                         && listViewItems.Items.Count > 0)
                {
                    RaiseAddItem(listViewItems.Items[0].Name);
                }
                return;
            }

            if (e.KeyCode == Keys.Down
                && cBoxSearchType.Text == ItemCodeDesc
                && listViewItems.Items.Count > 0)
            {
                listViewItems.Focus();
                listViewItems.Items[0].Focused = true;
                listViewItems.Items[0].Selected = true;
            }
        }

        private void listViewItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && cBoxSearchType.Text == ItemCodeDesc
                && listViewItems.SelectedItems.Count > 0)
            {
                e.SuppressKeyPress = true;
                RaiseAddItem(listViewItems.SelectedItems[0].Name);
                return;
            }

            if (e.KeyCode == Keys.Up
                && listViewItems.SelectedItems.Count > 0
                && listViewItems.SelectedItems[0].Index == 0)
            {
                txtSearchItems.Focus();
            }
        }

        private void RaiseAddItem(string code)
        {
            AddItemEvent?.Invoke(this, new ItemEventArgs(code));
            txtSearchItems.Clear();
            listViewItems.Items.Clear();
            listViewItems.Visible = false;
            txtSearchItems.Focus();
        }

        public void SetSearchResults(IList<ItemLookup> results, bool showList)
        {
            listViewItems.Items.Clear();
            foreach (var r in results)
            {
                var item = listViewItems.Items.Add(r.Code, r.Name, 0);
                item.SubItems.Add(" " + r.Code);
            }
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewItems.Visible = showList && results.Count > 0;
        }

        #endregion

        #region Grid
        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string col = dataGridViewX1.Columns[e.ColumnIndex].Name;
            if (col == "Qty" || col == "Price")
            {
                dataGridViewX1.CurrentCell = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridViewX1.BeginEdit(true);   // true = select all text
            }
        }

        private void CalculateTotal()
        {
            decimal grandTotal = 0M;
            decimal totalQty = 0M;

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow) continue;

                decimal qty = ToDecimal(row.Cells["Qty"].Value);
                decimal price = ToDecimal(row.Cells["Price"].Value);
                decimal lineTotal = qty * price;

                row.Cells["Total"].Value = lineTotal;
                totalQty += qty;
                grandTotal += lineTotal;
            }

            txtTotalQty.Text = totalQty.ToString("N2");
            txtGrandTotal.Text = grandTotal.ToString("N2");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.CurrentRow == null || dataGridViewX1.CurrentRow.IsNewRow) return;

            dataGridViewX1.Rows.Remove(dataGridViewX1.CurrentRow);
            CalculateTotal();
        }

        private void dataGridViewX1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string col = dataGridViewX1.Columns[e.ColumnIndex].Name;
            if (col != "Qty" && col != "Price") return;

            string text = Convert.ToString(e.FormattedValue);
            decimal value;
            if (!decimal.TryParse(text, out value) || value < 0M || (col == "Qty" && value == 0M))
            {
                MessageBox.Show(col == "Qty"
                    ? "Order Quantity must be a number greater than 0."
                    : "Price must be a number of 0 or more.",
                    "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;   // keeps the cell in edit mode until fixed (Esc cancels the edit)
            }
        }

        public void AddOrderDetail(string itemCode, string itemName, decimal? quantity,
                                   decimal? qty, decimal? price, decimal? total)
        {
            dataGridViewX1.Rows.Add(itemCode, itemName, quantity, qty, price, total);
            CalculateTotal();
        }

        public bool TryIncrementItem(string itemCode, decimal qty)
        {
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToString(row.Cells["ItemCode"].Value) != itemCode) continue;

                decimal newQty = ToDecimal(row.Cells["Qty"].Value) + qty;
                row.Cells["Qty"].Value = newQty;

                row.Cells["Total"].Value = newQty * ToDecimal(row.Cells["Price"].Value);

                dataGridViewX1.ClearSelection();
                row.Selected = true;
                dataGridViewX1.CurrentCell = row.Cells[0];
                return true;
            }
            return false;
        }

        public IList<OrderLine> GetOrderLines()
        {
            dataGridViewX1.EndEdit();
            var lines = new List<OrderLine>();
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow) continue;
                lines.Add(new OrderLine
                {
                    ItemCode = Convert.ToString(row.Cells["ItemCode"].Value),
                    Qty = ToDecimal(row.Cells["Qty"].Value),
                    Price = ToDecimal(row.Cells["Price"].Value),
                    Total = ToDecimal(row.Cells["Total"].Value)
                });
            }
            return lines;
        }

        public void SelectLastOrderDetail()
        {
            if (dataGridViewX1.Rows.Count == 0) return;
            int last = dataGridViewX1.Rows.Count - 1;
            dataGridViewX1.ClearSelection();
            dataGridViewX1.Rows[last].Selected = true;
            dataGridViewX1.CurrentCell = dataGridViewX1.Rows[last].Cells[0];
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string col = dataGridViewX1.Columns[e.ColumnIndex].Name;
            if (col != "Qty" && col != "Price") return;

            var row = dataGridViewX1.Rows[e.RowIndex];
            decimal qty = ToDecimal(row.Cells["Qty"].Value);
            decimal price = ToDecimal(row.Cells["Price"].Value);

            row.Cells["Qty"].Value = qty;
            row.Cells["Price"].Value = price;
            row.Cells["Total"].Value = qty * price;

            CalculateTotal();
        }

        private void dataGridViewX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && !dataGridViewX1.IsCurrentCellInEditMode
                && dataGridViewX1.CurrentRow != null && !dataGridViewX1.CurrentRow.IsNewRow)
            {
                dataGridViewX1.Rows.Remove(dataGridViewX1.CurrentRow);
                CalculateTotal();
                e.Handled = true;
            }
        }

        private static decimal ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value) return 0M;
            decimal d;
            return decimal.TryParse(Convert.ToString(value), out d) ? d : 0M;
        }

        #endregion

        #region Supplier

        private void SupplierSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxSupplier.SelectedValue == null) return;
            SuppId = cBoxSupplier.SelectedValue.ToString();
            SearchSupplierEvent?.Invoke(this, EventArgs.Empty);
        }

        public void SetSupplierList(BindingSource list)
        {
            cBoxSupplier.DataSource = list;
            cBoxSupplier.DisplayMember = "suppliername";
            cBoxSupplier.ValueMember = "supplierid";
            cBoxSupplier.SelectedIndex = -1;
        }

        public string Supplier
        {
            get { return Convert.ToString(cBoxSupplier.SelectedValue ?? cBoxSupplier.Text); }
            set
            {
                if (cBoxSupplier.DataSource == null)
                {
                    cBoxSupplier.Text = value;
                    return;
                }
                if (string.IsNullOrEmpty(value))
                {
                    cBoxSupplier.SelectedIndex = -1;
                    return;
                }
                cBoxSupplier.SelectedValue = value;
                if (cBoxSupplier.SelectedIndex < 0)
                    cBoxSupplier.Text = value;
            }
        }

        #endregion

        #region Mode / status

        public void SetEditMode(bool isEditMode)
        {
            lblModalTitle.Text = isEditMode ? "Edit Purchase Order" : "New Purchase Order";
            btnSave.Text = isEditMode ? "Update" : "Save";
            btnPrint.Enabled = isEditMode;
            btnReceived.Enabled = isEditMode;
        }

        public void IsReceived()
        {
            btnSave.Enabled = false;
            btnReceived.Enabled = false;
        }

        public void StatusReceived(string value)
        {
            btnReceived.Text = value;
        }

        public void ClearForm()
        {
            OrderNo = string.Empty;
            Remarks = string.Empty;
            MovementType = string.Empty;
            Supplier = string.Empty;
            Address = string.Empty;
            ContactNumber = string.Empty;
            OrderDate = DateTime.Today;
            dataGridViewX1.Rows.Clear();
            CalculateTotal();
        }

        #endregion

        #region Simple properties

        public string Address { get { return txtAddress.Text; } set { txtAddress.Text = value; } }
        public string ContactNumber { get { return txtContactNumber.Text; } set { txtContactNumber.Text = value; } }
        public string MovementType { get { return cbBoxMovementType.Text; } set { cbBoxMovementType.Text = value; } }
        public DateTime OrderDate { get { return dtpOrderDate.Value; } set { dtpOrderDate.Value = value; } }
        public string OrderNo { get { return txtOrderNo.Text; } set { txtOrderNo.Text = value; } }
        public string Remarks { get { return txtRemarks.Text; } set { txtRemarks.Text = value; } }
        public string SearchItems { get { return txtSearchItems.Text; } set { txtSearchItems.Text = value; } }
        public string SearchType { get { return cBoxSearchType.Text; } set { cBoxSearchType.Text = value; } }

        public void SetMovementTypeList(IList<string> list)
        {
            cbBoxMovementType.DataSource = list;
            cbBoxMovementType.SelectedIndex = -1;
        }

        public void SetSearchByList(IList<string> list)
        {
            cBoxSearchType.DataSource = list;
            cBoxSearchType.SelectedIndex = -1;
        }

        #endregion
    }
}