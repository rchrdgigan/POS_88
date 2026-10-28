using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Business;
using POS_88.Database;
using POS_88.Repository;

namespace POS_88.Persistence
{
    public class UnitOfWorkPOS : POSRepository
    {
        private readonly POSDBEntities _context;

        public UnitOfWorkPOS(POSDBEntities context)
        {
            this._context = context;
            this.Inventory = (POSBusiness.IInventoryRepository)new POSRepository.InventoryRepository(this._context);
            this.Category = (POSBusiness.ICategoryRepository)new POSRepository.CategoryRepository(this._context);
            this.Manufacturer = (POSBusiness.IManufacturerRepository)new POSRepository.ManufacturerRepository(this._context);
            this.Supplier = (POSBusiness.ISupplierRepository)new POSRepository.SupplierRepository(this._context);
            this.Customers = (POSBusiness.ICustomersRepository)new POSRepository.CustomersRepository(this._context);
            this.VATStatus = (POSBusiness.IVATStatusRepository)new POSRepository.VATStatusRepository(this._context);
            this.Invoice = (POSBusiness.IInvoiceRepository)new POSRepository.InvoiceRepository(this._context);
            this.ItemSales = (POSBusiness.IItemSalesRepository)new POSRepository.ItemSalesRepository(this._context);
            this.User = (POSBusiness.IUserRepository)new POSRepository.UserRepository(this._context);
            this.StockHistory = (POSBusiness.IStockHistoryRepository)new POSRepository.StockHistoryRepository(this._context);
            this.Setting = (POSBusiness.ISettingsRepository)new POSRepository.SettingsRepository(this._context);
            this.Expenses = (POSBusiness.IExpensesRepository)new POSRepository.ExpensesRepository(this._context);
            this.CashCount = (POSBusiness.ICashCountRepository)new POSRepository.CashCountRepository(this._context);
            this.Payment = (POSBusiness.IPaymentRepository)new POSRepository.PaymentRepository(this._context);
            this.Order = (POSBusiness.IOrderRepository)new POSRepository.OrderRepository(this._context);
            this.OrderDetail = (POSBusiness.IOrderDetailRepository)new POSRepository.OrderDetailRepository(this._context);
            this.CashInOut = (POSBusiness.ICashInOutRepository)new POSRepository.CashInOutRepository(this._context);
        }

        public POSBusiness.IInventoryRepository Inventory { get; private set; }

        public POSBusiness.ICategoryRepository Category { get; private set; }

        public POSBusiness.IManufacturerRepository Manufacturer { get; private set; }

        public POSBusiness.ISupplierRepository Supplier { get; private set; }

        public POSBusiness.ICustomersRepository Customers { get; private set; }

        public POSBusiness.IVATStatusRepository VATStatus { get; private set; }

        public POSBusiness.IInvoiceRepository Invoice { get; private set; }

        public POSBusiness.IItemSalesRepository ItemSales { get; private set; }

        public POSBusiness.IUserRepository User { get; private set; }

        public POSBusiness.IStockHistoryRepository StockHistory { get; private set; }

        public POSBusiness.ISettingsRepository Setting { get; private set; }

        public POSBusiness.IExpensesRepository Expenses { get; private set; }

        public POSBusiness.ICashCountRepository CashCount { get; private set; }

        public POSBusiness.IPaymentRepository Payment { get; private set; }

        public POSBusiness.IOrderRepository Order { get; private set; }

        public POSBusiness.IOrderDetailRepository OrderDetail { get; private set; }

        public POSBusiness.ICashInOutRepository CashInOut { get; private set; }

        public int Complete() => this._context.SaveChanges();

        public void Dispose() => this._context.Dispose();
    }

}
