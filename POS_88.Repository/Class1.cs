using POS_88.Business;
using POS_88.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace POS_88.Repository
{
    public class POSRepository : POSBusiness
    {
        public class InventoryRepository :
            POS_88.Repository.Repository<inventory>,
            POSBusiness.IInventoryRepository,
            IRepository<inventory>
        {
            private POSDBEntities _context;

            public InventoryRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetInventory_Result> GetInventory(string param)
            {
                return this._context.spGetInventory(param).ToList();
            }

            public IEnumerable<spGetCriticalStocks_Result> GetCriticalItems()
            {
                return this._context.spGetCriticalStocks().ToList();
            }

            public IEnumerable<spSearchByItem_Result> SearchItem(string searchBy, string param)
            {
                return this._context.spSearchByItem(searchBy, param).ToList();
            }

            public int UpdateQty(string itemcode, decimal qty)
            {
                return this._context.spUpdateQty(
                    itemcode,
                    new decimal?(qty)
                );
            }

            public int VoidItemQty(int receiptno, string itemcode, int qty)
            {
                return this._context.spVoidItemQty(
                    new int?(receiptno),
                    itemcode,
                    new decimal?((decimal)qty)
                );
            }

            public int UpdateStocks(string updateType, string itemcode, decimal qty)
            {
                return this._context.spUpdateStocks(
                    updateType,
                    itemcode,
                    new decimal?(qty)
                );
            }

            public decimal GetTotalStocks()
            {
                return this._context.spGetTotalStocks()
                    .SingleOrDefault<decimal?>()
                    .Value;
            }

            public int GetCriticalStocks()
            {
                return this._context.spGetTotalCriticalStocks()
                    .SingleOrDefault<int?>()
                    .Value;
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }
        public class CategoryRepository :
           POS_88.Repository.Repository<category>,
           POSBusiness.ICategoryRepository,
           IRepository<category>
        {
            private POSDBEntities _context;

            public CategoryRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetCategory_Result> GetCategory(string param)
            {
                return this._context.spGetCategory(param).ToList();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }

        public class ManufacturerRepository :
            POS_88.Repository.Repository<manufacturer>,
            POSBusiness.IManufacturerRepository,
            IRepository<manufacturer>
        {
            private POSDBEntities _context;

            public ManufacturerRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetManufacturer_Result> GetManufacturer(string param)
            {
                return this._context.spGetManufacturer(param).ToList();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class SupplierRepository :
            POS_88.Repository.Repository<supplier>,
            POSBusiness.ISupplierRepository,
            IRepository<supplier>
        {
            private POSDBEntities _context;

            public SupplierRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetSupplier_Result> GetSupplier(string param)
            {
                return this._context.spGetSupplier(param).ToList();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class CustomersRepository :
            POS_88.Repository.Repository<customer>,
            POSBusiness.ICustomersRepository,
            IRepository<customer>
        {
            private POSDBEntities _context;

            public CustomersRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spCustomers_Result> GetCustomers(string param)
            {
                return this._context.spCustomers(param).ToList();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class VATStatusRepository :
            POS_88.Repository.Repository<vat>,
            POSBusiness.IVATStatusRepository,
            IRepository<vat>
        {
            private POSDBEntities _context;

            public VATStatusRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetSupplier_Result> GetVATStatus(string param)
            {
                return this._context.spGetSupplier(param).ToList();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class InvoiceRepository :
            POS_88.Repository.Repository<invoice>,
            POSBusiness.IInvoiceRepository,
            IRepository<invoice>
        {
            private POSDBEntities _context;

            public InvoiceRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetrReceiptNo_Result> GetReceiptNo(string usernm)
            {
                return this._context.spGetrReceiptNo(usernm).ToList();
            }

            public int UpdateInvoiceType(
                int receiptNo,
                string invoiceType,
                string usernm)
            {
                return this._context.spUpdateInvoiceType(
                    new int?(receiptNo),
                    invoiceType,
                    usernm);
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class ItemSalesRepository :
            POS_88.Repository.Repository<itemsale>,
            POSBusiness.IItemSalesRepository,
            IRepository<itemsale>
        {
            private POSDBEntities _context;

            public ItemSalesRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetSalesByTransaction_Result> GetSalesByTransaction(string param)
            {
                return (IEnumerable<spGetSalesByTransaction_Result>)this._context.spGetSalesByTransaction(param).ToList<spGetSalesByTransaction_Result>();
            }

            public IEnumerable<spGetPaymentHistory_Result> GetPaymentHistory(string param)
            {
                return (IEnumerable<spGetPaymentHistory_Result>)this._context.spGetPaymentHistory(param).ToList<spGetPaymentHistory_Result>();
            }

            public IEnumerable<spGetSalesByItem_Result> GetSalesByItems(string param)
            {
                return (IEnumerable<spGetSalesByItem_Result>)this._context.spGetSalesByItem(param).ToList<spGetSalesByItem_Result>();
            }

            public IEnumerable<spGetItemSalesbyID_Result> GetItemSalesByID(int receiptNo)
            {
                return (IEnumerable<spGetItemSalesbyID_Result>)this._context.spGetItemSalesbyID(new int?(receiptNo)).ToList<spGetItemSalesbyID_Result>();
            }

            public IEnumerable<spGetMonthlySales_Result> GetMonthlySales(int param)
            {
                return (IEnumerable<spGetMonthlySales_Result>)this._context.spGetMonthlySales(new int?(param)).ToList<spGetMonthlySales_Result>();
            }

            public IEnumerable<spGetMonthlySalesByYear_Result> GetMonthlySalesByYear(int param)
            {
                return (IEnumerable<spGetMonthlySalesByYear_Result>)this._context.spGetMonthlySalesByYear(new int?(param)).ToList<spGetMonthlySalesByYear_Result>();
            }

            public IEnumerable<spGetSalesToday_Result> GetSalesToday(DateTime salesdt)
            {
                return (IEnumerable<spGetSalesToday_Result>)this._context.spGetSalesToday(new DateTime?(salesdt)).ToList<spGetSalesToday_Result>();
            }

            public IEnumerable<spGetZReading_Result> GetZReadingReport(DateTime salesdt)
            {
                return (IEnumerable<spGetZReading_Result>)this._context.spGetZReading(new DateTime?(salesdt)).ToList<spGetZReading_Result>();
            }

            public IEnumerable<spGetZReadingByZno_Result> GetZReadingReportByZno(int zNo)
            {
                return (IEnumerable<spGetZReadingByZno_Result>)this._context.spGetZReadingByZno(new int?(zNo)).ToList<spGetZReadingByZno_Result>();
            }

            public IEnumerable<spGetTopItems_Result> GetTopItems(int salesYr, int salesMonth)
            {
                return (IEnumerable<spGetTopItems_Result>)this._context.spGetTopItems(new int?(salesYr), new int?(salesMonth)).ToList<spGetTopItems_Result>();
            }

            public IEnumerable<spGetTopCustomers_Result> GetTopCustomers(int salesYr, int salesMonth)
            {
                return (IEnumerable<spGetTopCustomers_Result>)this._context.spGetTopCustomers(new int?(salesYr), new int?(salesMonth)).ToList<spGetTopCustomers_Result>();
            }

            public IEnumerable<spGetPurchasedItems_Result> GetPurchasedItems(int receiptNo)
            {
                return (IEnumerable<spGetPurchasedItems_Result>)this._context.spGetPurchasedItems(new int?(receiptNo)).ToList<spGetPurchasedItems_Result>();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }

        }



        public class UserRepository :
            POS_88.Repository.Repository<user>,
            POSBusiness.IUserRepository,
            IRepository<user>
        {
            private POSDBEntities _context;

            public UserRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetUsers_Result> GetUsers(string param)
            {
                return (IEnumerable<spGetUsers_Result>)this._context.spGetUsers(param).ToList<spGetUsers_Result>();
            }

            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }

        }


        public class StockHistoryRepository :
             POS_88.Repository.Repository<stockshistory>,
             POSBusiness.IStockHistoryRepository,
             IRepository<stockshistory>
        {
            private POSDBEntities _context;

            public StockHistoryRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public int LogStocksMovement(
              string movmentType,
              string documentType,
              int? receiptNo,
              int? customerNo,
              string itemcode,
              Decimal? qty,
              string createdby)
            {
                return this._context.spLogStocksMovement(movmentType, documentType, receiptNo, customerNo, itemcode, qty, createdby);
            }

            public IEnumerable<spGetStocksHistory_Result> GetStocksHistory(string param)
            {
                return (IEnumerable<spGetStocksHistory_Result>)this._context.spGetStocksHistory(param).ToList<spGetStocksHistory_Result>();
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }

        public class SettingsRepository :
            POS_88.Repository.Repository<setting>,
            POSBusiness.ISettingsRepository,
            IRepository<setting>
        {
            private POSDBEntities _context;


            public SettingsRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }



        public class ExpensesRepository :
            POS_88.Repository.Repository<expens>,
            POSBusiness.IExpensesRepository,
            IRepository<expens>
        {
            private POSDBEntities _context;


            public ExpensesRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetExpenses_Result> GetExpenses(string param)
            {
                return (IEnumerable<spGetExpenses_Result>)this._context.spGetExpenses(param).ToList<spGetExpenses_Result>();
            }

            public Decimal GetTotalExpenses(int expenseYr, int expenseMonth)
            {
                return this._context.spGetTotalExpenses(new int?(expenseYr), new int?(expenseMonth)).SingleOrDefault<Decimal?>().Value;
            }


            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }



        public class CashCountRepository :
            POS_88.Repository.Repository<cashcount>,
            POSBusiness.ICashCountRepository,
            IRepository<cashcount>
        {
            private POSDBEntities _context;


            public CashCountRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetCashCount_Result> GetCashCount(string param)
            {
                return (IEnumerable<spGetCashCount_Result>)this._context.spGetCashCount(param).ToList<spGetCashCount_Result>();
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }



        public class PaymentRepository :
            POS_88.Repository.Repository<paymenthistory>,
            POSBusiness.IPaymentRepository,
            IRepository<paymenthistory>
        {
            private POSDBEntities _context;


            public PaymentRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class OrderRepository :
           POS_88.Repository.Repository<Order>,
           POSBusiness.IOrderRepository,
           IRepository<Order>
        {
            private POSDBEntities _context;


            public OrderRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetOrderID_Result> GetOrderID(string usernm, string customerId)
            {
                return (IEnumerable<spGetOrderID_Result>)this._context.spGetOrderID(usernm, customerId).ToList<spGetOrderID_Result>();
            }

            public IEnumerable<spGetOrders_Result> GetOrders(string param)
            {
                return (IEnumerable<spGetOrders_Result>)this._context.spGetOrders(param).ToList<spGetOrders_Result>();
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }



        public class OrderDetailRepository :
           POS_88.Repository.Repository<OrderDetail>,
           POSBusiness.IOrderDetailRepository,
           IRepository<OrderDetail>
        {
            private POSDBEntities _context;


            public OrderDetailRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }


        public class CashInOutRepository :
           POS_88.Repository.Repository<cashinout>,
           POSBusiness.ICashInOutRepository,
           IRepository<cashinout>
        {
            private POSDBEntities _context;


            public CashInOutRepository(POSDBEntities context)
                : base((DbContext)context)
            {
                _context = context;
            }

            public IEnumerable<spGetCashInout_Result> GetCashInOut(int zNo)
            {
                return (IEnumerable<spGetCashInout_Result>)this._context.spGetCashInout(new int?(zNo)).ToList<spGetCashInout_Result>();
            }

            public IEnumerable<spGetCashInoutHistory_Result> GetCashInOutHistory(int zNo)
            {
                return (IEnumerable<spGetCashInoutHistory_Result>)this._context.spGetCashInoutHistory(new int?(zNo)).ToList<spGetCashInoutHistory_Result>();
            }

            //public POSDBEntities PayrollContext => this.Context as POSDBEntities;
            public POSDBEntities PayrollContext
            {
                get
                {
                    return this.Context as POSDBEntities;
                }
            }
        }

    }
}
