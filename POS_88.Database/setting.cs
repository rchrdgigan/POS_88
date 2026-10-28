using System;


namespace POS_88.Database
{

    public class setting
    {
        public int settingsid { get; set; }

        public string storename { get; set; }

        public string address { get; set; }

        public string tinno { get; set; }

        public string contactno { get; set; }

        public string printername { get; set; }

        public string footer { get; set; }

        public bool? withAR { get; set; }

        public bool? withLogo { get; set; }

        public Decimal downpayment { get; set; }

        public bool? touscreenmode { get; set; }

        public string ServerNm { get; set; }

        public string DatabaseNm { get; set; }

        public string DbUser { get; set; }

        public string DbPw { get; set; }

        public int? itemcodefrom { get; set; }

        public int? itemcodeto { get; set; }

        public int? weightfrom { get; set; }

        public int? weightto { get; set; }

        public int? nodecimal { get; set; }

        public Decimal retailmarkup { get; set; }

        public Decimal wholesalemarkup { get; set; }

        public string poleport { get; set; }

        public string defaultTopDisplay { get; set; }

        public string defaultBottomDisplay { get; set; }

        public bool? enablepoledisplay { get; set; }

        public Decimal amountspent { get; set; }

        public Decimal pointsearned { get; set; }

        public Decimal conversionrate { get; set; }

        public Decimal pointspesos { get; set; }

        public string imagePath { get; set; }

        public bool? enablepointspayment { get; set; }

        public bool? enablecreditlimit { get; set; }
    }

}
