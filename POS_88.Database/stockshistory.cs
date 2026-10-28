using System;

namespace POS_88.Database
{
    public class stockshistory
    {
        public string itemcode { get; set; }

        public string documenttype { get; set; }

        public int? receiptno { get; set; }

        public int? customerid { get; set; }

        public DateTime? stockdate { get; set; }

        public Decimal? quantity { get; set; }

        public Decimal? instock { get; set; }

        public string createdby { get; set; }

        public int sqno { get; set; }
    }
}
