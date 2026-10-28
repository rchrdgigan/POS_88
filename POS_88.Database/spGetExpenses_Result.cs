using System;


namespace POS_88.Database
{
    public class spGetExpenses_Result
    {
        public int EXPENSES_ID { get; set; }

        public DateTime? EXPENSES_DATE { get; set; }

        public string PAYEE { get; set; }

        public string DESCRIPTION { get; set; }

        public string CATEGORY { get; set; }

        public Decimal? VAT { get; set; }

        public Decimal? AMOUNT { get; set; }

        public string ADDRESS { get; set; }

        public string REMARKS { get; set; }
    }
}
