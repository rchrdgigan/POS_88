using System;


namespace POS_88.Database
{
    public class spGetPurchasedItems_Result
    {
        public string ITEM_CODE { get; set; }

        public string NAME { get; set; }

        public Decimal? COST { get; set; }

        public Decimal? PRICE { get; set; }

        public Decimal? QTY { get; set; }

        public string DISCOUNT { get; set; }

        public Decimal? SUBTOTAL { get; set; }

        public Decimal? AMOUNT_DUE { get; set; }

        public string VAT_STATUS { get; set; }

        public string PRICE_TYPE { get; set; }
    }

}
