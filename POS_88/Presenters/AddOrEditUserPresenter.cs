using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{
    class AddOrEditUserPresenter
    {
        private readonly IAddOrEditUserView view;
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly POSImplementation posImplementation;
        private readonly string userName; //auth user
        private readonly string userID;
        private readonly string saveType;
        private readonly Action afterSave;

        public AddOrEditUserPresenter(
            IAddOrEditUserView view,
            string userName,  //auth user
            string userID,
            Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.userID = userID;
            this.saveType = string.IsNullOrWhiteSpace(userID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());

            this.view.SaveEvent += SaveUser;
            this.view.ClearEvent += ClearFields;

            this.view.SetEditMode(this.saveType != "INSERT");
            this.view.UserType = "ADMIN";
            if (this.saveType != "INSERT")
                LoadUser();
        }

        private void LoadUser()
        {
            user user = unitOfWork.User.Find(x => x.usernm == this.userID).FirstOrDefault<user>();
            this.view.UserName = Convert.ToString(user.usernm);
            this.view.FName = user.nameuser;
            this.view.UserType = user.usertype;
            this.view.Password = user.password;
            this.view.ConfirmPassword = user.password;
        }

        private void ClearFields(object sender, EventArgs e)
        {
            this.view.ClearFields();
        }

        private void SaveUser(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.UserName) || string.IsNullOrWhiteSpace(view.FName) ||  string.IsNullOrWhiteSpace(view.UserType) || string.IsNullOrWhiteSpace(view.Password))
            {
                MessageBox.Show("All fields are required.", "Creating User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
          
            try
            {
                string text = this.posImplementation.SaveUser(new user()
                {
                    usernm = this.view.UserName,
                    nameuser = this.view.FName,
                    usertype = this.view.UserType,
                    password = this.view.Password
                }, this.saveType);

                if (text == "")
                {
                    int num = (int)MessageBox.Show("User has been saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (afterSave != null)
                        afterSave();
                    this.view.ClearFields();
                    if (this.saveType != "INSERT")
                        view.Close();
                }
                else
                {
                    int num1 = (int)MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
