using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface IUsersView
    {
        string AuthenticatedUserName { get; set; }

        //Fields
        string TotalUsers { get; set; }

        // Filter fields
        string SearchParam { get; set; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler ClearFiltersEvent;
        event EventHandler<UserActionEventArgs> CashCountUserEvent;
        event EventHandler<UserActionEventArgs> EditUserEvent;
        event EventHandler<UserActionEventArgs> RemoveUserEvent;

        // Combo methods
        void ClearFilters();
        void ShowUsersLoading();
        void HideUsersLoading();

        //Methods
        void SetUsersBindingSource(BindingSource list);
        void Show();
    }

    public class UserActionEventArgs : EventArgs
    {
        public UserActionEventArgs(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}
