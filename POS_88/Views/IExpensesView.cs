using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace POS_88.Views
{
    public interface IExpensesView
    {
        event EventHandler SearchEvent;
        event EventHandler ClearEvent;
        event EventHandler DeleteEvent;

        string TotalExpenses { get; set; }
        string SearchParms { get; set; }
        string From { get; set; }
        string To { get; set; }
        string SelectedExpense { get; }

        void SetExpensesListBindingSource(BindingSource list);
        void ClearFilters();
        void ShowExpensesListLoading();
        void HideExpensesListLoading();
        void Show();
    }
}
