using System;


namespace POS_88.Database
{
    public class spCustomers_Result
    {
        public int CUSTOMER_ID { get; set; }

        public string NAME { get; set; }

        public string ADDRESS { get; set; }

        public string CONTACT_NO { get; set; }

        public string ID_NO { get; set; }

        public Decimal? LOYALTY_POINTS { get; set; }

        public Decimal CREDIT_LIMIT { get; set; }
    }

}
