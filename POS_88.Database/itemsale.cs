using System;


namespace POS_88.Database
{
    public class itemsale
    {
        public int? receiptno { get; set; }

        public string itemcode { get; set; }

        public string description { get; set; }

        public Decimal? costprice { get; set; }

        public Decimal? price { get; set; }

        public Decimal? quantity { get; set; }

        public Decimal? total { get; set; }

        public string discountdesc { get; set; }

        public string discounttype { get; set; }

        public Decimal? discountamt { get; set; }

        public Decimal? totaldue { get; set; }

        public string vatstatus { get; set; }

        public string pricetype { get; set; }

        public string note { get; set; }

        public int SqNo { get; set; }
    }
}
