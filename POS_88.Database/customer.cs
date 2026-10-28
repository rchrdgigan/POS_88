using System;


namespace POS_88.Database
{
    public class customer
    {
        public int customerid { get; set; }

        public string customername { get; set; }

        public string address { get; set; }

        public string contactno { get; set; }

        public string idno { get; set; }

        public Decimal? loyaltypoints { get; set; }

        public Decimal? credit_limit { get; set; }
    }
}

