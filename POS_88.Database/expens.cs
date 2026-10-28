using System;


namespace POS_88.Database
{
    public class expens
    {
        public int expensesid { get; set; }

        public DateTime? dtexpenses { get; set; }

        public string payee { get; set; }

        public string description { get; set; }

        public string category { get; set; }

        public Decimal? vat { get; set; }

        public Decimal? amount { get; set; }

        public string tinno { get; set; }

        public string address { get; set; }

        public string remarks { get; set; }
    }
}
