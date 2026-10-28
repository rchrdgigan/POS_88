using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public interface ICashCountListView
    {
        event EventHandler SearchEvent;
        event EventHandler ClearEvent;
        event EventHandler ExportExcelEvent;
        string From { get; set; }
        string To { get; set; }

        string GrandTotal { get; set; }
        string OpeningAmount { get; set; }

        void SetCashCountListBindingSource(BindingSource list);
        void Show();
    }
}
