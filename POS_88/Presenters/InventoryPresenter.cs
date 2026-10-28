using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;
using System.Windows.Forms;
using System.Data;

namespace POS_88.Presenters
{
    public class InventoryPresenter
    {
        private IInventoryView view;
        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        private BindingSource inventoryBindingSource;

        public InventoryPresenter(IInventoryView view)
        {
            this.view = view;
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._posImplement = new POSImplementation();
            this.inventoryBindingSource = new BindingSource();

            this.view.SearchEvent += this.SearchInventory;
            this.view.DeleteEvent += this.DeleteItem;

            
            this.view.ClearFiltersEvent += this.ClearFilters;
            view.SetInventoryListBindingSource(this.inventoryBindingSource);
            view.Show();
            this.LoadInventory();
        }

        private async void DeleteItem(object sender, EventArgs e)
        {
            string itemCode = this.view.SelectedItemCode;
            if (string.IsNullOrWhiteSpace(itemCode))
            {
                MessageBox.Show("Select an item before deleting.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to delete selected item?\nItem code: " + itemCode, "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                inventory item = this._unitOfWork.Inventory
                    .Find(x => x.itemcode == itemCode)
                    .FirstOrDefault();

                if (item == null)
                {
                    MessageBox.Show("The selected item could not be found.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this._unitOfWork.Inventory.Remove(item);
                this._unitOfWork.Complete();

                MessageBox.Show("Item deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await this.GetInventoryData(this.view.SearchParam);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Reload()
        {
            this.LoadInventory();
        }

        private async void SearchInventory(object sender, EventArgs e)
        {
            await this.GetInventoryData(this.view.SearchParam);
        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetInventoryData(string.Empty);
        }

        private async void LoadInventory()
        {
            await this.PopulateCombo();
            await this.GetInventoryData(this.view.SearchParam);
        }

        private async Task PopulateCombo()
        {
            this.view.ShowFilterLoading();
            try
            {
                var filters = await Task.Run(() => new
                {
                    Categories = this._unitOfWork.Category.GetAll()
                        .OrderBy(item => item.categoryname).ToList(),
                    Suppliers = this._unitOfWork.Supplier.GetAll()
                        .OrderBy(item => item.suppliername).ToList(),
                    VatStatuses = this._unitOfWork.VATStatus.GetAll()
                        .OrderBy(item => item.vatname).ToList()
                });

                this.view.SetCategoryList(new BindingSource { DataSource = filters.Categories });
                this.view.SetSupplierList(new BindingSource { DataSource = filters.Suppliers });
                this.view.SetVatStatusList(new BindingSource { DataSource = filters.VatStatuses });
                this.view.SetAvailabilityList(new List<string> { "", "YES", "NO" });

                this.view.CategoryId = "ALL";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideFilterLoading();
            }
        }

        public async Task GetInventoryData(string param)
        {
            this.view.ShowInventoryLoading();
            try
            {
                string searchParam = string.IsNullOrWhiteSpace(param) ? this.view.SearchParam : param;
                SearchRequestInventory request = new SearchRequestInventory
                {
                    categoryid = this.view.CategoryId,
                    supplierid = this.view.SupplierId,
                    vatstatus = this.view.VatStatus,
                    availability = this.view.Availability,
                    SearchParam = searchParam
                };
                DataTable inventory = await Task.Run(() => this._posImplement.SearchInventory(request));

                this.inventoryBindingSource.DataSource = inventory;
                this.view.TotalCost = this.GetTotal(inventory, "COST").ToString("N");
                this.view.TotalPrice = this.GetTotal(inventory, "RETAIL_PRICE").ToString("N");
                this.view.TotalQuantity = this.GetTotal(inventory, "QUANTITY").ToString("N");
                this.view.NoOfItems = inventory.Rows.Count.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideInventoryLoading();
            }
        }

        private decimal GetTotal(DataTable inventory, string columnName)
        {
            object total = inventory.Compute("SUM(" + columnName + ")", string.Empty);
            return total == DBNull.Value ? 0M : Convert.ToDecimal(total);
        }

    }
}
