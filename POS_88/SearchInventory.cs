using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SearchRequestInventory
    {
        public string categoryid { get; set; }

        public string supplierid { get; set; }

        public string vatstatus { get; set; }

        public string availability { get; set; }

        public string SearchParam { get; set; }
    }
}
