using System;


namespace POS_88.Database
{
    public class spGetMonthlySales_Result
    {
        public int? YEAR { get; set; }

        public int? MONTHNO { get; set; }

        public Decimal? TOTALSALES { get; set; }
    }
}
