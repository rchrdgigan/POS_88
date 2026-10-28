using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IAddOrEditExpensesView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        DateTime Date { get; set; }
        string Payee { get; set; }
        string Description { get; set; }
        string Category { get; set; }
        string Vat { get; set; }
        string Amount { get; set; }
        string TinNo { get; set; }
        string Address { get; set; }
        string Remarks { get; set; }
     
        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}
