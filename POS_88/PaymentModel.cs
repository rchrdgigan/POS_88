using System;

namespace POS_88
{
    public class PaymentModel
    {
        public int receiptno { get; set; }

        public Decimal amountDue { get; set; }

        public Decimal paymentAmt { get; set; }

        public Decimal changeAmt { get; set; }

        public Decimal balance { get; set; }

        public Decimal pointsRedeem { get; set; }

        public Decimal pointsAmt { get; set; }

        public int customerid { get; set; }

        public string tablenm { get; set; }

        public string note { get; set; }

        public string driverName { get; set; }

        public string paymenttype { get; set; }

        public string invoicetype { get; set; }

        public string status { get; set; }
    }
}
