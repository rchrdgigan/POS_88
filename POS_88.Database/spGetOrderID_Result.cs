using System;


namespace POS_88.Database
{

    public class spGetOrderID_Result
    {
        public int OrderID { get; set; }

        public string OrderNo { get; set; }

        public int? CustomerId { get; set; }

        public string SupplierId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? OrderReceived { get; set; }

        public Decimal? TotalItems { get; set; }

        public Decimal? GrandTotal { get; set; }

        public string Remarks { get; set; }

        public string isProcessed { get; set; }

        public int? ReceiptNo { get; set; }

        public string createdBy { get; set; }

        public DateTime? createdDt { get; set; }
    }

}
