using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;

namespace POS_88.Presenters
{

    class AddOrEditCustomersPresenter
    {
        private readonly IAddOrEditCustomersView view;
        private readonly POSImplementation posImplementation;
        private readonly string userName;
        private readonly string customerID;
        private readonly string saveType;
        private readonly Action afterSave;
        private static string title = "Customers";

        public AddOrEditCustomersPresenter(
                IAddOrEditCustomersView view,
                string userName,
                string customerID,
                Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.customerID = customerID;
            this.saveType = string.IsNullOrWhiteSpace(customerID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();

            this.view.SaveEvent += SaveCustomer;
            this.view.ClearEvent += ClearFields;

            this.view.SetEditMode(this.saveType != "INSERT");
            ResetFields();
            if (this.saveType != "INSERT")
                LoadCustomers(customerID);
        }

        private void LoadCustomers(string customerID)
        {
            int customIntID;
            if (!int.TryParse(customerID, out customIntID))
            {
                MessageBox.Show("The selected customer id not converting a string to int.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                customer customer = unitOfWork.Customers
                    .Find(value => value.customerid == customIntID).FirstOrDefault();
                if (customer == null)
                {
                    MessageBox.Show("The selected customer could not be found.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.Close();
                    return;
                }
                view.CustomerID = customerID;
                view.CAddress = customer.address;
                view.CName = customer.customername;
                view.ContactNo = customer.contactno;
                view.IDNo = customer.idno;
                view.LoyaltyPoints = Format(customer.loyaltypoints);
                view.CreditLimit = Format(customer.credit_limit);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private static decimal Format(decimal? value)
        {
            return Convert.ToDecimal(value);
        }

        private void ClearFields(object sender, EventArgs e)
        {
            if (saveType != "INSERT")
            {
                LoadCustomers(customerID);
                return;
            }

            ResetFields();
        }

        private void SaveCustomer(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.CustomerID) || string.IsNullOrWhiteSpace(view.CName))
            {
                MessageBox.Show("Customer name are required.", "Creating Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string text = this.posImplementation.SaveCustomer(new customer()
                {
                    customername = this.view.CName,
                    address = this.view.CAddress,
                    contactno = this.view.ContactNo,
                    idno = this.view.IDNo,
                    loyaltypoints = this.view.LoyaltyPoints,
                    credit_limit = this.view.CreditLimit

                }, this.saveType);

                if (text == "")
                {
                    int num = (int)MessageBox.Show("Customer has been saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                MessageBox.Show(exception.Message, "Customers", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFields()
        {
            view.ClearFields();
            view.LoyaltyPoints = Convert.ToDecimal("0.00");
            view.CreditLimit = Convert.ToDecimal("0.00");
        }
    }
}
