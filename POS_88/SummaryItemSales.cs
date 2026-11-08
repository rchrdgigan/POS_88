using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SummaryItemSales
    {
        public Decimal SUBTOTAL { get; set; }

        public int TOTAL_QTY { get; set; }

        public Decimal SUB_TOTAL { get; set; }

        public Decimal DISCOUNT { get; set; }

        public Decimal TOTAL_DUE { get; set; }
    }
}
