using System;


namespace POS_88.Database
{

    public class paymenthistory
    {
        public int? receiptno { get; set; }

        public string paytype { get; set; }

        public string paymode { get; set; }

        public Decimal? amount { get; set; }

        public Decimal? balance { get; set; }

        public Decimal? cashtender { get; set; }

        public DateTime? paydate { get; set; }

        public string usernm { get; set; }

        public bool readflag { get; set; }

        public string remarks { get; set; }

        public int SqNo { get; set; }
    }

}
