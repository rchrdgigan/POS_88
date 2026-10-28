using System;

namespace POS_88.Database
{
    public class OrderDetail
    {
        public int? OrderId { get; set; }

        public string ItemCode { get; set; }

        public Decimal? Qty { get; set; }

        public Decimal? price { get; set; }

        public Decimal? total { get; set; }

        public string PriceType { get; set; }

        public int SqNo { get; set; }
    }

}
