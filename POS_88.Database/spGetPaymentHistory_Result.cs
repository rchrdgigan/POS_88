using System;


namespace POS_88.Database
{

    public class spGetPaymentHistory_Result
    {
        public int RECEIPT_NO { get; set; }

        public DateTime? DATE { get; set; }

        public string CUSTOMER { get; set; }

        public Decimal? SUBTOTAL { get; set; }

        public Decimal? SUB_DISCOUNT { get; set; }

        public Decimal? AMOUNT_DUE { get; set; }

        public string PAYMENT_TYPE { get; set; }

        public DateTime? DATE_OF_PAYMENT { get; set; }

        public Decimal? AMOUNT_PAID { get; set; }

        public Decimal? BALANCE { get; set; }

        public Decimal? CASH_TENDER { get; set; }

        public string REMARKS { get; set; }

        public string STATUS { get; set; }

        public string INVOICE_TYPE { get; set; }
    }

}
