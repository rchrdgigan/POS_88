using System;

namespace POS_88.Database
{

    public class inventory
    {
        public string itemcode { get; set; }

        public string itemname { get; set; }

        public string unit { get; set; }

        public string categoryid { get; set; }

        public string manufacturerid { get; set; }

        public string supplierid { get; set; }

        public Decimal? cost { get; set; }

        public Decimal? wholesaleprice { get; set; }

        public Decimal? promoprice { get; set; }

        public Decimal? price { get; set; }

        public Decimal? quantity { get; set; }

        public Decimal? alertquantity { get; set; }

        public string vatstatus { get; set; }

        public string availability { get; set; }

        public string remarks { get; set; }
    }

}
