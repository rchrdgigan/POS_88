using System;

namespace POS_88.Database
{
    public class cashcount
    {
        public string TerminalId { get; set; }

        public string UserNm { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeoOut { get; set; }

        public int? OneThousand { get; set; }

        public Decimal? OneThousandAmt { get; set; }

        public int? FiveHundred { get; set; }

        public Decimal? FiveHundredAmt { get; set; }

        public int? TwoHundred { get; set; }

        public Decimal? TwoHundredAmt { get; set; }

        public int? OneHundred { get; set; }

        public Decimal? OneHundredAmt { get; set; }

        public int? Fifty { get; set; }

        public Decimal? FiftyAmt { get; set; }

        public int? Twenty { get; set; }

        public Decimal? TwentyAmt { get; set; }

        public int? TwentyCoins { get; set; }

        public Decimal? TwentyCoinsAmt { get; set; }

        public int? TenCoins { get; set; }

        public Decimal? TenCoinsAmt { get; set; }

        public int? FiveCoins { get; set; }

        public Decimal? FiveCoinsAmt { get; set; }

        public int? OneCoins { get; set; }

        public Decimal? OneCoinsAmt { get; set; }

        public int? FiveCents { get; set; }

        public Decimal? FiveCentsAmt { get; set; }

        public int? TenCents { get; set; }

        public Decimal? TenCentsAmt { get; set; }

        public int? TwentyFiveCents { get; set; }

        public Decimal? TwentyFiveCentsAmt { get; set; }

        public Decimal? OpeningAmt { get; set; }

        public Decimal? CashCountAmt { get; set; }

        public Decimal? GrandTotal { get; set; }

        public bool IsClosed { get; set; }

        public int SqNo { get; set; }
    }
}
