using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface ICustomersView
    {
        event EventHandler SearchEvent;
        event EventHandler ClearEvent;
        event EventHandler DeleteEvent;

        string TotalCustomers { get; set; }
        string SearchParms { get; set; }
        string SelectedCustomers { get; }

        void SetCustomersListBindingSource(BindingSource list);
        void ClearFilters();
        void ShowCustomersListLoading();
        void HideCustomersListLoading();
        void Show();
    }
}
