using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Presenters
{
    class SetupPresenter
    {
        private readonly ISetupView view;
        private readonly string authenticatedUserName;

        private UnitOfWorkPOS _unitOfWork;
        private BindingSource setupBindingSource;

        private string selectedSetup;

        public SetupPresenter(ISetupView view)
            : this(view, string.Empty)
        {
        }

        public SetupPresenter(ISetupView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;

            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.setupBindingSource = new BindingSource();

            this.selectedSetup = view.SelectedSetup;

            this.view.SearchSetupEvent += SearchUsers;
            this.view.ClearSetupEvent += ClearFilters;
            this.view.RemoveSetupEvent += RemoveSetup;
            this.view.EditSetupEvent += EditSetup;

            view.AuthenticatedUserName = authenticatedUserName;
            view.SetSetupBindingSource(this.setupBindingSource);
            view.Show();
            this.LoadSetup();
        }


        private void EditSetup(object sender, SetupActionEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (string.IsNullOrEmpty(e._id))
                return;

            try
            {
                Form viewForm = this.view as Form;
                Form modalOwner = (viewForm != null ? (viewForm.MdiParent ?? viewForm) : null);

                using (Form backdrop = new Form())
                {
                    if (modalOwner != null)
                    {
                        backdrop.FormBorderStyle = FormBorderStyle.None;
                        backdrop.StartPosition = FormStartPosition.Manual;
                        backdrop.ShowInTaskbar = false;
                        backdrop.BackColor = Color.Black;
                        backdrop.Opacity = 0.45;
                        backdrop.Bounds = modalOwner.Bounds;
                        backdrop.Show(modalOwner);
                        backdrop.BringToFront();
                    }

                    switch (view.SelectedSetup)
                    {
                        case "Category":
                            using (AddOrEditCategoryView editView = new AddOrEditCategoryView())
                            {
                                new AddOrEditCategoryPresenter(
                                    editView,
                                    this.authenticatedUserName,
                                    e._id,
                                    this.LoadSetup);

                                editView.StartPosition = FormStartPosition.CenterParent;

                                if (modalOwner != null)
                                    //editView.ShowDialog(modalOwner);
                                    POSTheme.ShowSmoothFadeDialog(editView, modalOwner);

                                else
                                    editView.ShowDialog();
                            }
                            break;
                        case "Manufacturer":
                            using (AddOrEditManufacturerView editView = new AddOrEditManufacturerView())
                            {
                                new AddOrEditManufacturerPresenter(
                                    editView,
                                    this.authenticatedUserName,
                                    e._id,
                                    this.LoadSetup);

                                editView.StartPosition = FormStartPosition.CenterParent;

                                if (modalOwner != null)
                                    //editView.ShowDialog(modalOwner);
                                    POSTheme.ShowSmoothFadeDialog(editView, modalOwner);

                                else
                                    editView.ShowDialog();
                            }
                            break;
                        case "Supplier":
                            using (AddOrEditSupplierView editView = new AddOrEditSupplierView())
                            {
                                new AddOrEditSupplierPresenter(
                                    editView,
                                    this.authenticatedUserName,
                                    e._id,
                                    this.LoadSetup);

                                editView.StartPosition = FormStartPosition.CenterParent;

                                if (modalOwner != null)
                                    //editView.ShowDialog(modalOwner);
                                    POSTheme.ShowSmoothFadeDialog(editView, modalOwner);

                                else
                                    editView.ShowDialog();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void RemoveSetup(object sender, SetupActionEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            try
            {
                switch (view.SelectedSetup)
                {
                    case "Category":

                        category categoryToRemove = this._unitOfWork.Category
                            .Find(x => x.categoryid == e._id)
                            .FirstOrDefault();
                        if (categoryToRemove == null)
                        {
                            MessageBox.Show("The selected category could not be found.", "Category",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        this._unitOfWork.Category.Remove(categoryToRemove);
                        this._unitOfWork.Complete();
                        break;

                    case "Manufacturer":

                        manufacturer manufacturerToRemove = this._unitOfWork.Manufacturer
                            .Find(x => x.manufacturerid == e._id)
                            .FirstOrDefault();
                        if (manufacturerToRemove == null)
                        {
                            MessageBox.Show("The selected manufacturer could not be found.", "Manufacturer",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        this._unitOfWork.Manufacturer.Remove(manufacturerToRemove);
                        this._unitOfWork.Complete();
                        break;

                    case "Supplier":
                        supplier supplierToRemove = this._unitOfWork.Supplier
                            .Find(x => x.supplierid == e._id)
                            .FirstOrDefault();
                        if (supplierToRemove == null)
                        {
                            MessageBox.Show("The selected supplier could not be found.", "Supplier",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        this._unitOfWork.Supplier.Remove(supplierToRemove);
                        this._unitOfWork.Complete();
                        break;

                }
                await this.GetSetupData(this.view.SearchParam);
                MessageBox.Show(view.SelectedSetup + " deleted successfully!", "Deleting Record...",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private async void LoadSetup()
        {
            await this.GetSetupData(this.view.SearchParam);
        }

        public async Task GetSetupData(string param)
        {
            this.view.ShowDataLoading();
            try
            {
                DataTable dataTable = new DataTable();
                switch (view.SelectedSetup)
                {
                    case "Category":
                        IEnumerable<spGetCategory_Result> category = await Task.Run(() => this._unitOfWork.Category.GetCategory(param));
                        PropertyInfo[] properties1 = typeof(spGetCategory_Result).GetProperties();
                        foreach (PropertyInfo propertyInfo in properties1)
                        {
                            DataColumnCollection columns = dataTable.Columns;
                            string name = propertyInfo.Name;
                            Type dataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                            if ((object)dataType == null)
                                dataType = propertyInfo.PropertyType;
                            DataColumn column = new DataColumn(name, dataType);
                            columns.Add(column);
                        }
                        using (IEnumerator<spGetCategory_Result> enumerator = category.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                spGetCategory_Result current = enumerator.Current;
                                object[] objArray = new object[properties1.Length];
                                for (int index = 0; index < properties1.Length; ++index)
                                    objArray[index] = properties1[index].GetValue((object)current);
                                dataTable.Rows.Add(objArray);
                            }
                            break;
                        }
                    case "Manufacturer":
                        IEnumerable<spGetManufacturer_Result> manufacturer = await Task.Run(() => this._unitOfWork.Manufacturer.GetManufacturer(param));
                        PropertyInfo[] properties2 = typeof(spGetManufacturer_Result).GetProperties();
                        foreach (PropertyInfo propertyInfo in properties2)
                        {
                            DataColumnCollection columns = dataTable.Columns;
                            string name = propertyInfo.Name;
                            Type dataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                            if ((object)dataType == null)
                                dataType = propertyInfo.PropertyType;
                            DataColumn column = new DataColumn(name, dataType);
                            columns.Add(column);
                        }
                        using (IEnumerator<spGetManufacturer_Result> enumerator = manufacturer.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                spGetManufacturer_Result current = enumerator.Current;
                                object[] objArray = new object[properties2.Length];
                                for (int index = 0; index < properties2.Length; ++index)
                                    objArray[index] = properties2[index].GetValue((object)current);
                                dataTable.Rows.Add(objArray);
                            }
                            break;
                        }
                    case "Supplier":
                        IEnumerable<spGetSupplier_Result> supplier = await Task.Run(() => this._unitOfWork.Supplier.GetSupplier(param));
                        PropertyInfo[] properties3 = typeof(spGetSupplier_Result).GetProperties();
                        foreach (PropertyInfo propertyInfo in properties3)
                        {
                            DataColumnCollection columns = dataTable.Columns;
                            string name = propertyInfo.Name;
                            Type dataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                            if ((object)dataType == null)
                                dataType = propertyInfo.PropertyType;
                            DataColumn column = new DataColumn(name, dataType);
                            columns.Add(column);
                        }
                        using (IEnumerator<spGetSupplier_Result> enumerator = supplier.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                spGetSupplier_Result current = enumerator.Current;
                                object[] objArray = new object[properties3.Length];
                                for (int index = 0; index < properties3.Length; ++index)
                                    objArray[index] = properties3[index].GetValue((object)current);
                                dataTable.Rows.Add(objArray);
                            }
                            break;
                        }
                }
                this.setupBindingSource.DataSource = dataTable;
                view.TotalList = "Total Record: " + Convert.ToString(dataTable.Rows.Count);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideDataLoading();
            }

        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetSetupData(string.Empty);
        }

        private async void SearchUsers(object sender, EventArgs e)
        {
            await this.GetSetupData(this.view.SearchParam);
        }

        public void Reload()
        {
            this.LoadSetup();
        }

    }
}
