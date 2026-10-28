using System;

namespace POS_88.Database
{
    public class spGetMonthlySalesByYear_Result
    {
        public int? MONTHNO { get; set; }

        public string MONTHNM { get; set; }

        public Decimal? TOTALSALES { get; set; }
    }
}
