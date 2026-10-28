using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace POS_88.Database
{

    public class POSDBEntities : DbContext
    {
        public POSDBEntities()
          : base("name=POSDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<manufacturer> manufacturers { get; set; }

        public virtual DbSet<vat> vats { get; set; }

        public virtual DbSet<user> users { get; set; }

        public virtual DbSet<stockshistory> stockshistories { get; set; }

        public virtual DbSet<cashcount> cashcounts { get; set; }

        public virtual DbSet<itemsale> itemsales { get; set; }

        public virtual DbSet<paymenthistory> paymenthistories { get; set; }

        public virtual DbSet<supplier> suppliers { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<cashinout> cashinouts { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<customer> customers { get; set; }

        public virtual DbSet<inventory> inventories { get; set; }

        public virtual DbSet<setting> settings { get; set; }

        public virtual DbSet<category> categories { get; set; }

        public virtual DbSet<expens> expenses { get; set; }

        public virtual DbSet<invoice> invoices { get; set; }

        public virtual ObjectResult<spGetCategory_Result> spGetCategory(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCategory_Result>(nameof(spGetCategory), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetManufacturer_Result> spGetManufacturer(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetManufacturer_Result>(nameof(spGetManufacturer), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetSupplier_Result> spGetSupplier(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSupplier_Result>(nameof(spGetSupplier), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetrReceiptNo_Result> spGetrReceiptNo(string usernm)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetrReceiptNo_Result>(nameof(spGetrReceiptNo), usernm != null ? new ObjectParameter(nameof(usernm), (object)usernm) : new ObjectParameter(nameof(usernm), typeof(string)));
        }

        public virtual ObjectResult<spGetMonthlySales_Result> spGetMonthlySales(int? param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetMonthlySales_Result>(nameof(spGetMonthlySales), param.HasValue ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(int)));
        }

        public virtual ObjectResult<spGetMonthlySales1_Result> spGetMonthlySales1(int? param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetMonthlySales1_Result>(nameof(spGetMonthlySales1), param.HasValue ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(int)));
        }

        public virtual ObjectResult<spGetMonthlySalesByYear_Result> spGetMonthlySalesByYear(int? param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetMonthlySalesByYear_Result>(nameof(spGetMonthlySalesByYear), param.HasValue ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(int)));
        }

        public virtual ObjectResult<spGetSalesToday_Result> spGetSalesToday(DateTime? salesdt)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSalesToday_Result>(nameof(spGetSalesToday), salesdt.HasValue ? new ObjectParameter("Salesdt", (object)salesdt) : new ObjectParameter("Salesdt", typeof(DateTime)));
        }

        public virtual ObjectResult<spGetTopCustomers_Result> spGetTopCustomers(
          int? salesYr,
          int? salesMonth)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetTopCustomers_Result>(nameof(spGetTopCustomers), salesYr.HasValue ? new ObjectParameter("SalesYr", (object)salesYr) : new ObjectParameter("SalesYr", typeof(int)), salesMonth.HasValue ? new ObjectParameter("SalesMonth", (object)salesMonth) : new ObjectParameter("SalesMonth", typeof(int)));
        }

        public virtual ObjectResult<spGetUsers_Result> spGetUsers(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetUsers_Result>(nameof(spGetUsers), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetExpenses_Result> spGetExpenses(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetExpenses_Result>(nameof(spGetExpenses), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<Decimal?> spGetTotalExpenses(int? expenseYr, int? expenseMonth)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Decimal?>(nameof(spGetTotalExpenses), expenseYr.HasValue ? new ObjectParameter("ExpenseYr", (object)expenseYr) : new ObjectParameter("ExpenseYr", typeof(int)), expenseMonth.HasValue ? new ObjectParameter("ExpenseMonth", (object)expenseMonth) : new ObjectParameter("ExpenseMonth", typeof(int)));
        }

        public virtual int spUpdateQty(string itemcode, Decimal? qty)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction(nameof(spUpdateQty), itemcode != null ? new ObjectParameter(nameof(itemcode), (object)itemcode) : new ObjectParameter(nameof(itemcode), typeof(string)), qty.HasValue ? new ObjectParameter(nameof(qty), (object)qty) : new ObjectParameter(nameof(qty), typeof(Decimal)));
        }

        public virtual int spUpdateStocks(string updateType, string itemcode, Decimal? qty)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction(nameof(spUpdateStocks), updateType != null ? new ObjectParameter(nameof(updateType), (object)updateType) : new ObjectParameter(nameof(updateType), typeof(string)), itemcode != null ? new ObjectParameter(nameof(itemcode), (object)itemcode) : new ObjectParameter(nameof(itemcode), typeof(string)), qty.HasValue ? new ObjectParameter(nameof(qty), (object)qty) : new ObjectParameter(nameof(qty), typeof(Decimal)));
        }

        public virtual int spVoidItemQty(int? receiptNo, string itemcode, Decimal? qty)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction(nameof(spVoidItemQty), receiptNo.HasValue ? new ObjectParameter(nameof(receiptNo), (object)receiptNo) : new ObjectParameter(nameof(receiptNo), typeof(int)), itemcode != null ? new ObjectParameter(nameof(itemcode), (object)itemcode) : new ObjectParameter(nameof(itemcode), typeof(string)), qty.HasValue ? new ObjectParameter(nameof(qty), (object)qty) : new ObjectParameter(nameof(qty), typeof(Decimal)));
        }

        public virtual ObjectResult<int?> spGetTotalCriticalStocks()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int?>(nameof(spGetTotalCriticalStocks));
        }

        public virtual ObjectResult<Decimal?> spGetTotalStocks()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Decimal?>(nameof(spGetTotalStocks));
        }

        public virtual ObjectResult<spGetTopItems_Result> spGetTopItems(int? salesYr, int? salesMonth)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetTopItems_Result>(nameof(spGetTopItems), salesYr.HasValue ? new ObjectParameter("SalesYr", (object)salesYr) : new ObjectParameter("SalesYr", typeof(int)), salesMonth.HasValue ? new ObjectParameter("SalesMonth", (object)salesMonth) : new ObjectParameter("SalesMonth", typeof(int)));
        }

        public virtual ObjectResult<spGetPurchasedItems_Result> spGetPurchasedItems(int? receiptNo)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetPurchasedItems_Result>(nameof(spGetPurchasedItems), receiptNo.HasValue ? new ObjectParameter(nameof(receiptNo), (object)receiptNo) : new ObjectParameter(nameof(receiptNo), typeof(int)));
        }

        public virtual ObjectResult<spGetSalesByItemwithParam_Result> spGetSalesByItemwithParam(
          DateTime? from,
          DateTime? to)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSalesByItemwithParam_Result>(nameof(spGetSalesByItemwithParam), from.HasValue ? new ObjectParameter("From", (object)from) : new ObjectParameter("From", typeof(DateTime)), to.HasValue ? new ObjectParameter("To", (object)to) : new ObjectParameter("To", typeof(DateTime)));
        }

        public virtual ObjectResult<spGetCriticalStocks_Result> spGetCriticalStocks()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCriticalStocks_Result>(nameof(spGetCriticalStocks));
        }

        public virtual int spLogStocksMovement(
          string movmentType,
          string documentType,
          int? receiptNo,
          int? customerNo,
          string itemcode,
          Decimal? qty,
          string createdby)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction(nameof(spLogStocksMovement), movmentType != null ? new ObjectParameter(nameof(movmentType), (object)movmentType) : new ObjectParameter(nameof(movmentType), typeof(string)), documentType != null ? new ObjectParameter(nameof(documentType), (object)documentType) : new ObjectParameter(nameof(documentType), typeof(string)), receiptNo.HasValue ? new ObjectParameter(nameof(receiptNo), (object)receiptNo) : new ObjectParameter(nameof(receiptNo), typeof(int)), customerNo.HasValue ? new ObjectParameter(nameof(customerNo), (object)customerNo) : new ObjectParameter(nameof(customerNo), typeof(int)), itemcode != null ? new ObjectParameter(nameof(itemcode), (object)itemcode) : new ObjectParameter(nameof(itemcode), typeof(string)), qty.HasValue ? new ObjectParameter(nameof(qty), (object)qty) : new ObjectParameter(nameof(qty), typeof(Decimal)), createdby != null ? new ObjectParameter(nameof(createdby), (object)createdby) : new ObjectParameter(nameof(createdby), typeof(string)));
        }

        public virtual ObjectResult<spGetStocksHistory_Result> spGetStocksHistory(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetStocksHistory_Result>(nameof(spGetStocksHistory), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual int spUpdateInvoiceType(int? receiptNo, string invvoiceType, string creatdBy)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction(nameof(spUpdateInvoiceType), receiptNo.HasValue ? new ObjectParameter("ReceiptNo", (object)receiptNo) : new ObjectParameter("ReceiptNo", typeof(int)), invvoiceType != null ? new ObjectParameter("InvvoiceType", (object)invvoiceType) : new ObjectParameter("InvvoiceType", typeof(string)), creatdBy != null ? new ObjectParameter(nameof(creatdBy), (object)creatdBy) : new ObjectParameter(nameof(creatdBy), typeof(string)));
        }

        public virtual ObjectResult<spGetZReading_Result> spGetZReading(DateTime? salesdt)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetZReading_Result>(nameof(spGetZReading), salesdt.HasValue ? new ObjectParameter("Salesdt", (object)salesdt) : new ObjectParameter("Salesdt", typeof(DateTime)));
        }

        public virtual ObjectResult<spGetCashCount_Result> spGetCashCount(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCashCount_Result>(nameof(spGetCashCount), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetItemSalesbyID_Result> spGetItemSalesbyID(int? receiptNo)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetItemSalesbyID_Result>(nameof(spGetItemSalesbyID), receiptNo.HasValue ? new ObjectParameter(nameof(receiptNo), (object)receiptNo) : new ObjectParameter(nameof(receiptNo), typeof(int)));
        }

        public virtual ObjectResult<spGetPaymentHistory_Result> spGetPaymentHistory(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetPaymentHistory_Result>(nameof(spGetPaymentHistory), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetOrderID_Result> spGetOrderID(string usernm, string supplierId)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetOrderID_Result>(nameof(spGetOrderID), usernm != null ? new ObjectParameter(nameof(usernm), (object)usernm) : new ObjectParameter(nameof(usernm), typeof(string)), supplierId != null ? new ObjectParameter(nameof(supplierId), (object)supplierId) : new ObjectParameter(nameof(supplierId), typeof(string)));
        }

        public virtual ObjectResult<spGetZReadingByZno_Result> spGetZReadingByZno(int? zno)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetZReadingByZno_Result>(nameof(spGetZReadingByZno), zno.HasValue ? new ObjectParameter("Zno", (object)zno) : new ObjectParameter("Zno", typeof(int)));
        }

        public virtual ObjectResult<spGetCashInout_Result> spGetCashInout(int? zno)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCashInout_Result>(nameof(spGetCashInout), zno.HasValue ? new ObjectParameter("Zno", (object)zno) : new ObjectParameter("Zno", typeof(int)));
        }

        public virtual ObjectResult<spGetCashInoutHistory_Result> spGetCashInoutHistory(int? zno)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCashInoutHistory_Result>(nameof(spGetCashInoutHistory), zno.HasValue ? new ObjectParameter("Zno", (object)zno) : new ObjectParameter("Zno", typeof(int)));
        }

        public virtual ObjectResult<spGetOrders_Result> spGetOrders(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetOrders_Result>(nameof(spGetOrders), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spSearchByItem_Result> spSearchByItem(string searchBy, string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSearchByItem_Result>(nameof(spSearchByItem), searchBy != null ? new ObjectParameter("SearchBy", (object)searchBy) : new ObjectParameter("SearchBy", typeof(string)), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spCustomers_Result> spCustomers(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spCustomers_Result>(nameof(spCustomers), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetInventory_Result> spGetInventory(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetInventory_Result>(nameof(spGetInventory), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spGetSalesByItem_Result> spGetSalesByItem(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSalesByItem_Result>(nameof(spGetSalesByItem), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }

        public virtual ObjectResult<spalkytest_Result> spalkytest(int? receiptno)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spalkytest_Result>(nameof(spalkytest), receiptno.HasValue ? new ObjectParameter(nameof(receiptno), (object)receiptno) : new ObjectParameter(nameof(receiptno), typeof(int)));
        }

        public virtual ObjectResult<spGetSalesByTransaction_Result> spGetSalesByTransaction(string param)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSalesByTransaction_Result>(nameof(spGetSalesByTransaction), param != null ? new ObjectParameter("Param", (object)param) : new ObjectParameter("Param", typeof(string)));
        }
    }

}
