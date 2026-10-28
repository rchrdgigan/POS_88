using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Database;

namespace POS_88.Business
{
    public class POSBusiness
    {
        public interface IInventoryRepository : IRepository<inventory>
        {
            IEnumerable<spGetInventory_Result> GetInventory(string param);

            IEnumerable<spGetCriticalStocks_Result> GetCriticalItems();

            IEnumerable<spSearchByItem_Result> SearchItem(string searchBy, string param);

            int UpdateQty(string itemcode, Decimal qty);

            int UpdateStocks(string updateType, string itemcode, Decimal qty);

            Decimal GetTotalStocks();

            int GetCriticalStocks();

            int VoidItemQty(int receiptno, string itemcode, int qty);
        }

        public interface ICategoryRepository : IRepository<category>
        {
            IEnumerable<spGetCategory_Result> GetCategory(string param);
        }

        public interface IManufacturerRepository : IRepository<manufacturer>
        {
            IEnumerable<spGetManufacturer_Result> GetManufacturer(string param);
        }

        public interface ISupplierRepository : IRepository<supplier>
        {
            IEnumerable<spGetSupplier_Result> GetSupplier(string param);
        }

        public interface IVATStatusRepository : IRepository<vat>
        {
            IEnumerable<spGetSupplier_Result> GetVATStatus(string param);
        }

        public interface ICustomersRepository : IRepository<customer>
        {
            IEnumerable<spCustomers_Result> GetCustomers(string param);
        }

        public interface IInvoiceRepository : IRepository<invoice>
        {
            IEnumerable<spGetrReceiptNo_Result> GetReceiptNo(string usernm);

            int UpdateInvoiceType(int receiptNo, string invoiceType, string usernm);
        }

        public interface IItemSalesRepository : IRepository<itemsale>
        {
            IEnumerable<spGetSalesByTransaction_Result> GetSalesByTransaction(string usernm);

            IEnumerable<spGetSalesByItem_Result> GetSalesByItems(string param);

            IEnumerable<spGetMonthlySales_Result> GetMonthlySales(int param);

            IEnumerable<spGetMonthlySalesByYear_Result> GetMonthlySalesByYear(int param);

            IEnumerable<spGetSalesToday_Result> GetSalesToday(DateTime salesdt);

            IEnumerable<spGetZReading_Result> GetZReadingReport(DateTime salesdt);

            IEnumerable<spGetZReadingByZno_Result> GetZReadingReportByZno(int zno);

            IEnumerable<spGetTopItems_Result> GetTopItems(int salesYr, int salesMonth);

            IEnumerable<spGetTopCustomers_Result> GetTopCustomers(int salesYr, int salesMonth);

            IEnumerable<spGetPurchasedItems_Result> GetPurchasedItems(int receiptNo);

            IEnumerable<spGetItemSalesbyID_Result> GetItemSalesByID(int receiptNo);

            IEnumerable<spGetPaymentHistory_Result> GetPaymentHistory(string param);
        }

        public interface IUserRepository : IRepository<user>
        {
            IEnumerable<spGetUsers_Result> GetUsers(string param);
        }

        public interface IStockHistoryRepository : IRepository<stockshistory>
        {
            IEnumerable<spGetStocksHistory_Result> GetStocksHistory(string param);

            int LogStocksMovement(
              string movmentType,
              string documentType,
              int? receiptNo,
              int? customerNo,
              string itemcode,
              Decimal? qty,
              string createdby);
        }

        public interface ISettingsRepository : IRepository<setting>
        {
        }

        public interface IExpensesRepository : IRepository<expens>
        {
            IEnumerable<spGetExpenses_Result> GetExpenses(string param);

            Decimal GetTotalExpenses(int expenseYr, int expenseMonth);
        }

        public interface ICashCountRepository : IRepository<cashcount>
        {
            IEnumerable<spGetCashCount_Result> GetCashCount(string param);
        }

        public interface IPaymentRepository : IRepository<paymenthistory>
        {
        }

        public interface IOrderRepository : IRepository<Order>
        {
            IEnumerable<spGetOrderID_Result> GetOrderID(string usernm, string customerId);

            IEnumerable<spGetOrders_Result> GetOrders(string param);
        }

        public interface IOrderDetailRepository : IRepository<OrderDetail>
        {
        }

        public interface ICashInOutRepository : IRepository<cashinout>
        {
            IEnumerable<spGetCashInout_Result> GetCashInOut(int zNo);

            IEnumerable<spGetCashInoutHistory_Result> GetCashInOutHistory(int zNo);
        }
    }
}
