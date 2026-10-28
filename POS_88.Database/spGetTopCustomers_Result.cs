using System;

namespace POS_88.Database
{
    public class spGetTopCustomers_Result
    {
        public int? CUSTOMER_ID { get; set; }

        public string NAME { get; set; }

        public Decimal? TOTAL_SALES { get; set; }
    }
}
