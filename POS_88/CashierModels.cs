using System;

namespace POS_88
{

    public enum SearchMode { ItemCodeAndDescription, Barcode, BarcodeWeighingScale }

    public enum CashierDialog { ZReading, RecentSales, Reprint, CashCount, Stocks, PickItem, VoidSales, CustomerDisplay }

    public class SearchResultItem
    {
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>One row of the cart. Replaces reading/writing DataGridView cells.</summary>
    public class CartLine
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string PriceType { get; set; }
        public string VatStatus { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal Cost { get; set; }
        public decimal CurrentQty { get; set; }
        public string Note { get; set; } = "";
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public string DiscountMode { get; set; }       // "EXACT AMOUNT" or percentage
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } = ""; // discount name, e.g. "SENIOR"
        public string DiscountText { get; set; } = "0";
        public decimal DiscountAmt { get; set; }
        public decimal TotalDue { get; set; }

        /// <summary>Recompute total from price x quantity, then re-apply the discount.</summary>
        public void Reprice()
        {
            Total = Price * Quantity;
            Rediscount();
        }

        /// <summary>Recompute discount and amount due from the current Total.</summary>
        public void Rediscount()
        {
            if (DiscountValue <= 0 || string.IsNullOrEmpty(DiscountMode))
            {
                DiscountAmt = 0m;
                DiscountText = "0";
            }
            else if (DiscountMode == "EXACT AMOUNT")
            {
                DiscountAmt = Math.Min(DiscountValue, Total);
                DiscountText = DiscountAmt.ToString("N");
            }
            else
            {
                DiscountAmt = Total * (DiscountValue / 100m);
                DiscountText = $"{DiscountValue}% {DiscountAmt:N}";
            }
            TotalDue = Total - DiscountAmt;
        }
    }

    public class CartTotals
    {
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountDue { get; set; }
        public decimal VatSales { get; set; }
        public decimal VatAmount { get; set; }
        public decimal NonVatSales { get; set; }
        public decimal VatExemptSales { get; set; }
        public decimal ZeroRatedSales { get; set; }
        public decimal NoOfItems { get; set; }
        public decimal TotalCost { get; set; }
    }

}
