using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Database
{
    public class spGetSalesByItem_Result
    {
        public string ITEMCODE { get; set; }

        public string NAME { get; set; }

        public string CATEGORY { get; set; }

        public string SUPPLIER { get; set; }

        public Decimal? COST { get; set; }

        public Decimal? PRICE { get; set; }

        public Decimal? TOTAL_QTY { get; set; }

        public Decimal? SUB_TOTAL { get; set; }

        public Decimal? DISCOUNT { get; set; }

        public Decimal? TOTAL_DUE { get; set; }

        public Decimal? TOTAL_COST { get; set; }
    }

}
