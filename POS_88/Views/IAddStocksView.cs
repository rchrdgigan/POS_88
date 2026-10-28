using System;
namespace POS_88.Views
{
    public interface IAddStocksView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        string ItemCode { get; set; }
        string ItemName { get; set; }
        string CurrentQty { get; set; }
        decimal Quantity { get; set; }
        decimal NewStocks { get; set; }
        string Remarks { get; set; }
        void ClearFields();
        void Close();
    }
}
