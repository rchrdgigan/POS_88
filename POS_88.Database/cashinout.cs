using System;

namespace POS_88.Database
{
    public class cashinout
    {
        public string cashType { get; set; }

        public Decimal? amount { get; set; }

        public string note { get; set; }

        public int? ZReadNo { get; set; }

        public string createdBy { get; set; }

        public DateTime? createdDt { get; set; }

        public int SqNo { get; set; }
    }
}
