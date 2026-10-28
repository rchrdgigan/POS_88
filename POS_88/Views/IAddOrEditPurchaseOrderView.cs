using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public class ItemEventArgs : EventArgs
    {
        public string ItemCode { get; private set; }
        public ItemEventArgs(string itemCode) { ItemCode = itemCode; }
    }

    public class ItemLookup
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class OrderLine
    {
        public string ItemCode { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
    public interface IAddOrEditPurchaseOrderView
    {
        event EventHandler SaveEvent;
        event EventHandler SearchSupplierEvent;
        event EventHandler PrintEvent;
        event EventHandler ReceivedEvent;
        event EventHandler SearchItemsEvent;
        event EventHandler<ItemEventArgs> AddItemEvent;

        string SuppId { get; set; }
        string Supplier { get; set; }
        string Address { get; set; }
        string ContactNumber { get; set; }
        string MovementType { get; set; }
        DateTime OrderDate { get; set; }
        string OrderNo { get; set; }
        string Remarks { get; set; }
        string SearchItems { get; set; }
        string SearchType { get; set; }

        void SetSupplierList(BindingSource list);
        void SetMovementTypeList(IList<string> list);
        void SetSearchByList(IList<string> list);
        void SetSearchResults(IList<ItemLookup> results, bool showList);
        void SetEditMode(bool isEditMode);
        void StatusReceived(string value);
        void AddOrderDetail(string itemCode, string itemName, decimal? quantity,
                            decimal? qty, decimal? price, decimal? total);
        bool TryIncrementItem(string itemCode, decimal qty);
        IList<OrderLine> GetOrderLines();
        void ClearForm();
        void SelectLastOrderDetail();
        void IsReceived();

        void Close();
        void Show();

    }
}
