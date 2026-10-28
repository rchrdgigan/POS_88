using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88
{
    public class SummarySales
    {
        public Decimal TOTAL_COST { get; set; }

        public Decimal SUBTOTAL { get; set; }

        public Decimal SUB_DISCOUNT { get; set; }

        public Decimal AMOUNT_DUE { get; set; }

        public Decimal VAT_SALES { get; set; }

        public Decimal VAT_AMOUNT { get; set; }

        public Decimal NON_VAT_SALES { get; set; }

        public Decimal VAT_EXEMPT_SALES { get; set; }

        public Decimal ZERO_RATED_SALES { get; set; }

        public Decimal NO_OF_ITEMS { get; set; }
    }
}
