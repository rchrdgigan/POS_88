using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SearchRequestSales
    {
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public string crtdby { get; set; }

        public string paymentstatus { get; set; }

        public string invoicetype { get; set; }

        public string paymenttype { get; set; }

        public string SearchParam { get; set; }
    }

}
