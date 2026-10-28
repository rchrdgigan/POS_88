using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Presenters
{
    public class AddOrEditManufacturerPresenter
    {
        private readonly IAddOrEditManufacturerView view;
        private readonly UnitOfWorkPOS unitOfWork;
        private readonly POSImplementation posImplementation;
        private readonly string userName; //auth user
        private readonly string manufacturerID;
        private readonly string saveType;
        private readonly Action afterSave;

        public AddOrEditManufacturerPresenter(
           IAddOrEditManufacturerView view,
           string userName,
           string manufacturerID,
           Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.manufacturerID = manufacturerID;
            this.saveType = string.IsNullOrWhiteSpace(manufacturerID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());

            this.view.SaveEvent += SaveManufacturer;
            this.view.ClearEvent += ClearFields;

            this.view.SetEditMode(this.saveType != "INSERT");

            if (this.saveType != "INSERT")
                LoadCategory();
        }

        private void LoadCategory()
        {
            manufacturer manufac = unitOfWork.Manufacturer.Find(x => x.manufacturerid == manufacturerID).FirstOrDefault();
            this.view.ManufacturerId = Convert.ToString(manufac.manufacturerid);
            this.view.ManufacturerName = manufac.manufacturername;
        }

        private void ClearFields(object sender, EventArgs e)
        {
            this.view.ClearFields();
        }

        private void SaveManufacturer(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.ManufacturerId) || string.IsNullOrWhiteSpace(view.ManufacturerName))
            {
                MessageBox.Show("All fields are required.", "Creating Manufacturer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string text = this.posImplementation.SaveManufacturer(new manufacturer()
                {
                    manufacturerid = this.view.ManufacturerId,
                    manufacturername = this.view.ManufacturerName
                }, this.saveType);

                if (text == "")
                {
                    int num = (int)MessageBox.Show("Manufacturer has been saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                MessageBox.Show(exception.Message, "Manufacturer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
