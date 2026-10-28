using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class ImportStocksPresenter
    {
        private static readonly string[] RequiredColumns =
        {
            "ITEM_CODE", "NAME", "UNIT", "CATEGORY", "MANUFACTURER", "SUPPLIER",
            "COST", "WHOLESALE_PRICE", "RETAIL_PRICE", "PROMO_PRICE", "QUANTITY",
            "ALERT_QTY", "VAT_STATUS", "AVAILABILITY"
        };

        private const string TypeNewOrUpdate = "Add or Update Data";
        private const string TypeAddQty = "Add Qty";

        private readonly ImportStocksView view;
        private readonly string userName;
        private readonly Action afterImport;

        public ImportStocksPresenter(ImportStocksView view, string userName, Action afterImport)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.afterImport = afterImport;

            this.view.SetImportTypes(new[] { TypeNewOrUpdate, TypeAddQty });
            this.view.BrowseFileEvent += BrowseFile;
            this.view.LoadDataEvent += LoadData;
            this.view.ImportDataEvent += ImportData;
        }

        private void BrowseFile(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";
                dialog.Title = "Import Excel";
                if (dialog.ShowDialog(view) != DialogResult.OK)
                    return;

                view.FilePath = dialog.FileName;
                view.ClearPreviewData();
                view.SetSheetNames(new string[0]);
            }

            bool sheetsFound = false;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(GetConnectionString(view.FilePath)))
                {
                    connection.Open();
                    List<string> names;
                    using (DataTable sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                    {
                        if (sheets == null)
                            throw new InvalidOperationException("No worksheets were found in the selected Excel file.");

                        names = sheets.Rows.Cast<DataRow>()
                            .Select(row => Convert.ToString(row["TABLE_NAME"]))
                            .Where(name => name.EndsWith("$", StringComparison.Ordinal)
                                || name.EndsWith("$'", StringComparison.Ordinal))
                            .ToList();
                    }
                    view.SetSheetNames(names);
                    sheetsFound = names.Count > 0;

                    if (!sheetsFound)
                        MessageBox.Show("No worksheets were found in the selected Excel file.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            // Connection is closed now, so it's safe to load
            if (sheetsFound)
                LoadData(this, EventArgs.Empty);
        }

        private void LoadData(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.FilePath) || string.IsNullOrWhiteSpace(view.SelectedWorksheet))
            {
                MessageBox.Show("Browse for an Excel file before loading data.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DataTable table = new DataTable();
                using (OleDbConnection connection = new OleDbConnection(GetConnectionString(view.FilePath)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(
                    "SELECT * FROM [" + view.SelectedWorksheet.Replace("]", "]]") + "]", connection))
                {
                    adapter.Fill(table);
                }

                if (table.Columns.Count == 0)
                    throw new InvalidOperationException("The selected worksheet contains no columns.");

                view.SetPreviewData(table);
                view.HideNoData();
                MessageBox.Show("Excel worksheet loaded successfully.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                view.ClearPreviewData();
                MessageBox.Show(exception.Message, "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ImportData(object sender, EventArgs e)
        {
            if (view.ImportRows.Count == 0)
            {
                MessageBox.Show("Load an Excel worksheet before importing.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (view.ImportType != TypeNewOrUpdate && view.ImportType != TypeAddQty)
            {
                MessageBox.Show("Select an import type.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> missingColumns = GetMissingColumns();
            if (missingColumns.Count > 0)
            {
                MessageBox.Show("The worksheet is missing required columns: " + string.Join(", ", missingColumns) + ".", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedCount = 0;
            int successCount = 0;
            int failureCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                foreach (DataGridViewRow row in view.ImportRows)
                {
                    if (row.IsNewRow || !Convert.ToBoolean(row.Cells["ImportSelected"].Value ?? false))
                        continue;

                    selectedCount++;
                    try
                    {
                        ImportRow(row);
                        view.SetRowStatus(row, "Success");
                        successCount++;
                    }
                    catch (Exception exception)
                    {
                        view.SetRowStatus(row, "Failed: " + exception.Message);
                        failureCount++;
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            if (selectedCount == 0)
            {
                MessageBox.Show("Select at least one row to import.", "Import Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (successCount > 0 && afterImport != null)
                afterImport();

            string summary = string.Format(
                CultureInfo.CurrentCulture,
                "Import finished. Successful: {0}. Failed: {1}. See Import Status for row details.",
                successCount,
                failureCount);
            MessageBox.Show(summary, "Import Stocks", MessageBoxButtons.OK,
                failureCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void ImportRow(DataGridViewRow row)
        {
            inventory importedItem = new inventory
            {
                itemcode = GetText(row, "ITEM_CODE").Trim(),
                itemname = GetText(row, "NAME").Trim(),
                unit = GetText(row, "UNIT").Trim(),
                categoryid = GetText(row, "CATEGORY").Trim(),
                manufacturerid = GetText(row, "MANUFACTURER").Trim(),
                supplierid = GetText(row, "SUPPLIER").Trim(),
                cost = GetDecimal(row, "COST", true),
                wholesaleprice = GetDecimal(row, "WHOLESALE_PRICE", true),
                price = GetDecimal(row, "RETAIL_PRICE", true),
                promoprice = GetDecimal(row, "PROMO_PRICE", true),
                quantity = GetDecimal(row, "QUANTITY", false),
                alertquantity = GetDecimal(row, "ALERT_QTY", true),
                vatstatus = GetText(row, "VAT_STATUS").Trim(),
                availability = GetText(row, "AVAILABILITY").Trim()
            };

            if (string.IsNullOrWhiteSpace(importedItem.itemcode))
                throw new FormatException("ITEM_CODE cannot be blank.");
            if (string.IsNullOrWhiteSpace(importedItem.itemname))
                throw new FormatException("NAME cannot be blank.");
            if (importedItem.quantity.GetValueOrDefault() < 0M)
                throw new FormatException("QUANTITY cannot be negative.");
            if (view.ImportType == "Add Stocks" && importedItem.quantity.GetValueOrDefault() <= 0M)
                throw new FormatException("QUANTITY must be greater than zero when adding stocks.");

            if (string.IsNullOrWhiteSpace(importedItem.categoryid))
                importedItem.categoryid = "ALL";

            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                inventory existingItem = unitOfWork.Inventory
                    .Find(item => item.itemcode == importedItem.itemcode)
                    .FirstOrDefault();

                if (existingItem == null)
                {
                    unitOfWork.Inventory.Add(importedItem);
                    unitOfWork.Complete();
                    unitOfWork.StockHistory.LogStocksMovement(
                        "ADD", "Inventory Count", 0, 0, importedItem.itemcode, importedItem.quantity, userName);
                    return;
                }

                if (view.ImportType == "Add Stocks")
                {
                    unitOfWork.Inventory.UpdateStocks("ADD", importedItem.itemcode, importedItem.quantity.Value);
                    unitOfWork.StockHistory.LogStocksMovement(
                        "ADD", "Add Stocks", 0, 0, importedItem.itemcode, importedItem.quantity, userName);
                    return;
                }

                decimal oldQuantity = existingItem.quantity.GetValueOrDefault();
                existingItem.itemname = importedItem.itemname;
                existingItem.unit = importedItem.unit;
                existingItem.categoryid = importedItem.categoryid;
                existingItem.manufacturerid = importedItem.manufacturerid;
                existingItem.supplierid = importedItem.supplierid;
                existingItem.cost = importedItem.cost;
                existingItem.wholesaleprice = importedItem.wholesaleprice;
                existingItem.price = importedItem.price;
                existingItem.promoprice = importedItem.promoprice;
                existingItem.quantity = importedItem.quantity;
                existingItem.alertquantity = importedItem.alertquantity;
                existingItem.vatstatus = importedItem.vatstatus;
                existingItem.availability = importedItem.availability;
                unitOfWork.Complete();

                decimal quantityChange = importedItem.quantity.GetValueOrDefault() - oldQuantity;
                if (quantityChange != 0M)
                {
                    unitOfWork.StockHistory.LogStocksMovement(
                        "ADD", "Inventory Count", 0, 0, importedItem.itemcode, quantityChange, userName);
                }
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private List<string> GetMissingColumns()
        {
            List<string> missingColumns = new List<string>();
            foreach (string requiredColumn in RequiredColumns)
            {
                if (!view.HasSourceColumn(requiredColumn))
                    missingColumns.Add(requiredColumn);
            }
            return missingColumns;
        }

        private string GetText(DataGridViewRow row, string columnName)
        {
            object value = view.GetSourceValue(row, columnName);
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value, CultureInfo.CurrentCulture);
        }

        private decimal GetDecimal(DataGridViewRow row, string columnName, bool allowBlank)
        {
            string value = GetText(row, columnName).Trim();
            if (allowBlank && value.Length == 0)
                return 0M;

            decimal result;
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result)
                && !decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                throw new FormatException(columnName + " must contain a valid number.");
            if (result < 0M)
                throw new FormatException(columnName + " cannot be negative.");
            return result;
        }

        private static string GetConnectionString(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string properties = string.Equals(extension, ".xls", StringComparison.OrdinalIgnoreCase)
                ? "Excel 8.0;HDR=YES;IMEX=1"
                : "Excel 12.0 Xml;HDR=YES;IMEX=1";
            return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath
                + ";Extended Properties=\"" + properties + "\";";
        }
    }
}
