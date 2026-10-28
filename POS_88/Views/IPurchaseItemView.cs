using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IPurchaseItemView
    {
        int ReceiptNo { get; set; }
        void LoadDetails(int receiptNo);
    }
}
