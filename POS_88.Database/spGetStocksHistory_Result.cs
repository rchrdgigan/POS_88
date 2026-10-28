using System;


namespace POS_88.Database
{
    public class spGetStocksHistory_Result
    {
        public string ITEM_CODE { get; set; }

        public string DOCUMENT_TYPE { get; set; }

        public int? RECEIPT_NO { get; set; }

        public string CUSTOMER { get; set; }

        public DateTime? STOCK_DATE { get; set; }

        public Decimal? QUANTITY { get; set; }

        public Decimal? IN_STOCK { get; set; }

        public string USER { get; set; }
    }
}
