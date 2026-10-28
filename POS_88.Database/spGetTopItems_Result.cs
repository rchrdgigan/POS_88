using System;


namespace POS_88.Database
{
    public class spGetTopItems_Result
    {
        public string ITEMCODE { get; set; }

        public string NAME { get; set; }

        public Decimal? QTY { get; set; }

        public Decimal? TOTAL_SALES { get; set; }
    }

}
