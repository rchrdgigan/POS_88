using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;
using System.Collections.Generic;
using System.Reflection;

namespace POS_88.Presenters
{
    public class CustomersPresenter
    {

        private readonly ICustomersView view;
        private readonly string authenticatedUserName;
        private readonly POSImplementation posImplementation;
        private UnitOfWorkPOS unitOfWork;
        private BindingSource customersBindingSource;

        public CustomersPresenter(ICustomersView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.posImplementation = new POSImplementation();
            this.customersBindingSource = new BindingSource();

            this.view.SearchEvent += SearchUsers;
            this.view.ClearEvent += ClearFilters;
            this.view.DeleteEvent += DeleteCustomers;

            view.SetCustomersListBindingSource(this.customersBindingSource);
            this.LoadCustomers();
        }

        private async void DeleteCustomers(object sender, EventArgs e)
        {
            int selectedID;
            string selectedCustomer = this.view.SelectedCustomers;
            if (string.IsNullOrWhiteSpace(selectedCustomer))
            {
                MessageBox.Show("Select an customer before deleting.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to delete selected customer data?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!int.TryParse(selectedCustomer, out selectedID))
            {
                MessageBox.Show("The selected customer id not converting a string to int.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                customer customers = this.unitOfWork.Customers
                    .Find(x => x.customerid == selectedID)
                    .FirstOrDefault();

                if (customers == null)
                {
                    MessageBox.Show("The selected expens could not be found.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.unitOfWork.Customers.Remove(customers);
                this.unitOfWork.Complete();

                MessageBox.Show("Customer deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await this.GetCustomersData(this.view.SearchParms);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadCustomers()
        {
            await this.GetCustomersData(this.view.SearchParms);
        }

        public async Task GetCustomersData(string param)
        {
            this.view.ShowCustomersListLoading();
            try
            {
                DataTable dataTable = new DataTable();
                IEnumerable<spCustomers_Result> customers = await Task.Run(() => this.unitOfWork.Customers.GetCustomers(param));
                PropertyInfo[] properties = typeof(spCustomers_Result).GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    DataColumnCollection columns = dataTable.Columns;
                    string name = propertyInfo.Name;
                    Type dataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    if ((object)dataType == null)
                        dataType = propertyInfo.PropertyType;
                    DataColumn column = new DataColumn(name, dataType);
                    columns.Add(column);
                }
                foreach (spCustomers_Result spCustomersResult in customers)
                {
                    object[] objArray = new object[properties.Length];
                    for (int index = 0; index < properties.Length; ++index)
                        objArray[index] = properties[index].GetValue((object)spCustomersResult);
                    dataTable.Rows.Add(objArray);
                }

                view.TotalCustomers = "Total Customers: " + customers.Count().ToString();
                this.customersBindingSource.DataSource = dataTable;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideCustomersListLoading();
            }
        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetCustomersData(string.Empty);
        }

        private async void SearchUsers(object sender, EventArgs e)
        {
            await this.GetCustomersData(this.view.SearchParms);
        }

        public void Reload()
        {
            this.LoadCustomers();
        }
    }
}
