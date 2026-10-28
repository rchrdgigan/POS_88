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
    public class AddOrEditCategoryPresenter
    {
        private readonly IAddOrEditCategoryView view;
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly POSImplementation posImplementation;
        private readonly string userName; //auth user
        private readonly string catID;
        private readonly string saveType;
        private readonly Action afterSave;

        public AddOrEditCategoryPresenter(
            IAddOrEditCategoryView view,
            string userName,
            string catID,
            Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.catID = catID;
            this.saveType = string.IsNullOrWhiteSpace(catID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());

            this.view.SaveEvent += SaveCategory;
            this.view.ClearEvent += ClearFields;

            this.view.SetEditMode(this.saveType != "INSERT");

            if (this.saveType != "INSERT")
                LoadCategory();
        }

        private void LoadCategory()
        {
            category cat = unitOfWork.Category.Find(x => x.categoryid == catID).FirstOrDefault();
            this.view.CategoryId = Convert.ToString(cat.categoryid);
            this.view.CatName = cat.categoryname;
        }

        private void ClearFields(object sender, EventArgs e)
        {
            this.view.ClearFields();
        }

        private void SaveCategory(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.CategoryId) || string.IsNullOrWhiteSpace(view.CatName))
            {
                MessageBox.Show("All fields are required.", "Creating Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string text = this.posImplementation.SaveCategory(new category()
                {
                    categoryid = this.view.CategoryId,
                    categoryname = this.view.CatName
                }, this.saveType);

                if (text == "")
                {
                    int num = (int)MessageBox.Show("Category has been saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                MessageBox.Show(exception.Message, "Category", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
