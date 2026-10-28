using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface ISalesHistoryView
    {
        event EventHandler SearchEvent;
        event EventHandler ClearEvent;

        string CustomerID { get; set; }
        string CustomerName { get; set; }
        string CustomerContact { get; set; }
        string ModalTitle { get; set; }

        string TotalRevenue { get; set; }
        string AmountPaid { get; set; }
        string Balance { get; set; }
        string GrossProfit { get; set; }

        DateTime From { get; set; }
        DateTime To { get; set; }

        void SetSalesHistoryListBindingSource(BindingSource list);
    }
}
