using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface IStockHistoryView
    {
        event EventHandler SearchEvent;
        event EventHandler ClearEvent;

        string ItemCode { get; set; }
        string ItemName { get; set; }
        string From { get; set; }
        string To { get; set; }

        void SetStockHistoryListBindingSource(BindingSource list);
    }
}
