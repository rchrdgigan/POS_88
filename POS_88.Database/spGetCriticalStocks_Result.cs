using System;


namespace POS_88.Database
{

    public class spGetCriticalStocks_Result
    {
        public string ITEM_CODE { get; set; }

        public string NAME { get; set; }

        public string CATEGORY { get; set; }

        public Decimal? COST { get; set; }

        public Decimal? RETAIL_PRICE { get; set; }

        public Decimal? QUANTITY { get; set; }

        public Decimal? ALERT_QTY { get; set; }

        public string SUPPLIER { get; set; }

        public string MANUFACTURER { get; set; }

        public string VAT_STATUS { get; set; }

        public string AVAILABILITY { get; set; }
    }

}
