using System;

namespace POS_88.Database
{
    public class spGetZReading_Result
    {
        public Decimal? GROSSSALES { get; set; }

        public Decimal? REGDISCOUNT { get; set; }

        public Decimal? SCDISCOUNT { get; set; }

        public Decimal? PWDDISCOUNT { get; set; }

        public Decimal? NETSALES { get; set; }

        public Decimal? VATSALES { get; set; }

        public Decimal? VATAMT { get; set; }

        public Decimal? NONVATSALES { get; set; }

        public Decimal? VATEXEMSALES { get; set; }

        public Decimal? ZERORATEDSALES { get; set; }

        public Decimal? NOOFITEMS { get; set; }

        public int? NOOFTRANSACTIONS { get; set; }
    }
}
