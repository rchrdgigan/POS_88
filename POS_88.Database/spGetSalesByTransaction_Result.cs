using System;


namespace POS_88.Database
{

    public class spGetSalesByTransaction_Result
    {
        public int RECEIPT_NO { get; set; }

        public DateTime? DATE { get; set; }

        public string CUSTOMER { get; set; }

        public string PAYMENT_TYPE { get; set; }

        public string STAFF { get; set; }

        public Decimal? COST { get; set; }

        public Decimal? SUBTOTAL { get; set; }

        public Decimal? SUB_DISCOUNT { get; set; }

        public Decimal? AMOUNT_DUE { get; set; }

        public Decimal? CASH_TENDER { get; set; }

        public Decimal? CHANGE { get; set; }

        public Decimal? AMOUNT_PAID { get; set; }

        public Decimal? BALANCE { get; set; }

        public string STATUS { get; set; }

        public Decimal? VAT_SALES { get; set; }

        public Decimal? VAT_AMOUNT { get; set; }

        public Decimal? NON_VAT_SALES { get; set; }

        public Decimal? VAT_EXEMPT_SALES { get; set; }

        public Decimal? ZERO_RATED_SALES { get; set; }

        public Decimal? NO_OF_ITEMS { get; set; }

        public string DRIVER_NAME { get; set; }

        public string NOTE { get; set; }

        public string INVOICE_TYPE { get; set; }
    }

}
