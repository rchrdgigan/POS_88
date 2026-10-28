using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;
using System.Data;

namespace POS_88.Presenters
{
    public class UsersPresenter
    {

        private readonly IUsersView view;
        private readonly string authenticatedUserName;

        private UnitOfWorkPOS _unitOfWork;
        private BindingSource usersBindingSource;

        public UsersPresenter(IUsersView view)
            : this(view, string.Empty)
        {
        }

        public UsersPresenter(IUsersView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;

            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.usersBindingSource = new BindingSource();

            this.view.SearchEvent += SearchUsers;
            this.view.ClearFiltersEvent += this.ClearFilters;
            this.view.RemoveUserEvent += RemoveUser;
            this.view.EditUserEvent += EditUser;
            this.view.CashCountUserEvent += CashCountUser;


            view.AuthenticatedUserName = authenticatedUserName;
            view.SetUsersBindingSource(this.usersBindingSource);
            view.Show();
            this.LoadUsers();
        }

        private void CashCountUser(object sender, UserActionEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (string.IsNullOrEmpty(e.UserName))
                return;

            try
            {
                // The presenter isn't a Form, so get the owner from the view
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

                    using (CashCountListView cashCountView = new CashCountListView())
                    {
                        // Refresh the grid after a successful save
                        new CashCountListPresenter(
                            cashCountView,
                            e.UserName);

                        cashCountView.StartPosition = FormStartPosition.CenterParent;

                        if (modalOwner != null)
                            //cashCountView.ShowDialog(modalOwner);
                            POSTheme.ShowSmoothFadeDialog(cashCountView, modalOwner);
                        else
                            cashCountView.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditUser(object sender, UserActionEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (string.IsNullOrEmpty(e.UserName))
                return;

            try
            {
                // The presenter isn't a Form, so get the owner from the view
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

                    using (AddOrEditUserView editView = new AddOrEditUserView())
                    {
                        // Refresh the grid after a successful save
                        new AddOrEditUserPresenter(
                            editView,
                            this.authenticatedUserName,
                            e.UserName,
                            this.LoadUsers);

                        editView.StartPosition = FormStartPosition.CenterParent;

                        if (modalOwner != null)
                        {
                            //editView.ShowDialog(modalOwner);
                            POSTheme.ShowSmoothFadeDialog(editView, modalOwner);
                        }
                        else
                            editView.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RemoveUser(object sender, UserActionEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            try
            {
                user userToRemove = this._unitOfWork.User
                    .Find(x => x.usernm == e.UserName)
                    .FirstOrDefault();
                if (userToRemove == null)
                {
                    MessageBox.Show("The selected user could not be found.", "Users",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this._unitOfWork.User.Remove(userToRemove);
                this._unitOfWork.Complete();
                await this.GetUsersData(this.view.SearchParam);
                MessageBox.Show("User deleted successfully!", "Users",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private async void LoadUsers()
        {
            await this.GetUsersData(this.view.SearchParam);
        }

        public async Task GetUsersData(string param)
        {
            this.view.ShowUsersLoading();
            try
            {
                DataTable dataTable = new DataTable();
                IEnumerable<spGetUsers_Result> users = await Task.Run(() => this._unitOfWork.User.GetUsers(param));
                PropertyInfo[] properties = typeof(spGetUsers_Result).GetProperties();
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
                foreach (spGetUsers_Result spGetUsersResult in users)
                {
                    object[] objArray = new object[properties.Length];
                    for (int index = 0; index < properties.Length; ++index)
                        objArray[index] = properties[index].GetValue((object)spGetUsersResult);
                    dataTable.Rows.Add(objArray);
                }

                view.TotalUsers = "Total Users: "+ users.Count().ToString();
               this.usersBindingSource.DataSource = dataTable;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideUsersLoading();
            }


        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetUsersData(string.Empty);
        }

        private async void SearchUsers(object sender, EventArgs e)
        {
            await this.GetUsersData(this.view.SearchParam);
        }

        public void Reload()
        {
            this.LoadUsers();
        }
    }
}
