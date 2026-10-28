using System;


namespace POS_88.Database
{
    public class spGetCashInoutHistory_Result
    {
      public string TYPE { get; set; }

        public Decimal? AMOUNT { get; set; }

        public string NOTE { get; set; }

        public DateTime? ACTIONDATE { get; set; }
    }

}
