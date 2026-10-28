using System;


namespace POS_88.Database
{

    public class spGetrReceiptNo_Result
    {
        public int receiptno { get; set; }

        public DateTime? solddate { get; set; }

        public int? customerid { get; set; }

        public string tableno { get; set; }

        public string paymenttype { get; set; }

        public Decimal? subtotal { get; set; }

        public Decimal? subdiscount { get; set; }

        public Decimal? amountdue { get; set; }

        public Decimal? vatsales { get; set; }

        public Decimal? vatamount { get; set; }

        public Decimal? nonvatsales { get; set; }

        public Decimal? vatexemptsales { get; set; }

        public Decimal? zeroratedsales { get; set; }

        public Decimal? noofitems { get; set; }

        public string note { get; set; }

        public Decimal? cashtender { get; set; }

        public Decimal? changeamt { get; set; }

        public string crtdby { get; set; }

        public DateTime? crtddt { get; set; }
    }

}
