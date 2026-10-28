using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SearchRequestCashCount
    {
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public string UserNm { get; set; }
    }
}
