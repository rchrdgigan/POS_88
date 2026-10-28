using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IAddOrEditCustomersView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        string CustomerID { get; set; }
        string CName { get; set; }
        string CAddress { get; set; }
        string ContactNo { get; set; }
        string IDNo { get; set; }
        decimal LoyaltyPoints { get; set; }
        decimal CreditLimit { get; set; }

        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}
