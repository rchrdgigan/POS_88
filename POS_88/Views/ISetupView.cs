using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface ISetupView
    {
     
        event EventHandler SearchSetupEvent;
        event EventHandler ClearSetupEvent;
        event EventHandler<SetupActionEventArgs> EditSetupEvent;
        event EventHandler<SetupActionEventArgs> RemoveSetupEvent;

        string SearchParam { get; set; }
        string AuthenticatedUserName { get; set; }
        string TotalList { get; set; }
        string SelectedSetup { get; set; }


        void SetSetupBindingSource(BindingSource list);

        void ShowDataLoading();
        void HideDataLoading();

        void Test();

        void ClearFilters();
        void Show();
    }

    public class SetupActionEventArgs : EventArgs
    {
        public SetupActionEventArgs(string id)
        {
            _id = id;
        }

        public string _id { get; private set; }
    }
}
