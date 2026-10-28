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

    class AddOrEditExpensesPresenter
    {
        private readonly IAddOrEditExpensesView view;
        private readonly POSImplementation posImplementation;
        private readonly string userName;
        private readonly string expensID;
        private readonly string saveType;
        private readonly Action afterSave;
        private static string title = "Expens";

        public AddOrEditExpensesPresenter(
                IAddOrEditExpensesView view,
                string userName,
                string expensID,
                Action afterSave)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.userName = string.IsNullOrWhiteSpace(userName) ? Environment.UserName : userName;
            this.expensID = expensID;
            this.saveType = string.IsNullOrWhiteSpace(expensID) ? "INSERT" : "UPDATE";
            this.afterSave = afterSave;
            this.posImplementation = new POSImplementation();

            this.view.SaveEvent += SaveItem;
            this.view.ClearEvent += ClearFields;

            this.view.SetEditMode(this.saveType != "INSERT");
            ResetFields();
            if (this.saveType != "INSERT")
                LoadExpenses(expensID);
        }

        private void LoadExpenses(string expensID)
        {
            int expensIntID;
            if (!int.TryParse(expensID, out expensIntID))
            {
                MessageBox.Show("The selected expens id not converting a string to int.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            try
            {
                expens expens = unitOfWork.Expenses
                    .Find(value => value.expensesid == expensIntID).FirstOrDefault();
                if (expens == null)
                {
                    MessageBox.Show("The selected expens could not be found.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    view.Close();
                    return;
                }

                view.Date = expens.dtexpenses ?? DateTime.Today;
                view.Payee = expens.payee;
                view.Description = expens.description;
                view.Category = expens.category;
                view.Vat = FormatAmount(expens.vat);
                view.Amount = FormatAmount(expens.amount);
                view.TinNo = expens.tinno;
                view.Address = expens.address;
                view.Remarks = expens.remarks;
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

        private static string FormatAmount(decimal? value)
        {
            return value.GetValueOrDefault().ToString("0.00", CultureInfo.CurrentCulture);
        }

        private void ClearFields(object sender, EventArgs e)
        {
            if (saveType != "INSERT")
            {
                LoadExpenses(expensID);
                return;
            }

            ResetFields();
        }

        private void SaveItem(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.Payee) || string.IsNullOrWhiteSpace(view.Description))
            {
                MessageBox.Show("Payee and description are required.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int expensIntID;
            int.TryParse(expensID, out expensIntID);
           
            decimal vat;
            decimal amount;
            if (!TryParseAmount(view.Vat, "Vat", out vat) ||
                !TryParseAmount(view.Amount, "Wholesale price", out amount))
                return;

            expens item = new expens
            {
                expensesid = expensIntID,
                dtexpenses = view.Date,
                payee = view.Payee.Trim(),
                description = view.Description,
                category = view.Category,
                vat = vat,
                amount = amount,
                tinno = view.TinNo,
                address = view.Address,
                remarks = view.Remarks
            };

            try
            {
                string error = posImplementation.SaveExpenses(item, saveType, userName);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Expens has been saved successfully.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (afterSave != null)
                    afterSave();

                if (saveType == "INSERT")
                    ResetFields();
                else
                    view.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(title + exception.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryParseAmount(string text, string fieldName, out decimal amount)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                amount = 0M;
                return true;
            }

            if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out amount) && amount >= 0M)
                return true;

            MessageBox.Show(fieldName + " must be a valid non-negative number.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void ResetFields()
        {
            view.ClearFields();
            view.Amount = "0.00";
            view.Vat = "0.00";
        }
    }
}
