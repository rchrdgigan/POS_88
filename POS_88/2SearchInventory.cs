using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SearchRequestOrders
    {
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public string isProcessed { get; set; }

        public string SearchParam { get; set; }
    }
}
