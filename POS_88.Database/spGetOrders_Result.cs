using System;

namespace POS_88.Database
{

    public class spGetOrders_Result
    {
        public int ORDER_ID { get; set; }

        public string ORDER_NO { get; set; }

        public string SUPPLIER_NAME { get; set; }

        public string ADDRESS { get; set; }

        public string CONTACT_NO { get; set; }

        public DateTime? ORDER_DATE { get; set; }

        public DateTime? DATE_RECEIVED { get; set; }

        public Decimal? TOTAL_QTY { get; set; }

        public Decimal? GRAND_TOTAL { get; set; }

        public string TYPE { get; set; }

        public string IS_RECEIVED { get; set; }

        public string REMARKS { get; set; }
    }

}
