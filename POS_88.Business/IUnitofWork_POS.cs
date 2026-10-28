using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Business
{
    public class IUnitofWork_POS : POSBusiness
    {
        public interface IUnitOfWork_POS : IDisposable
        {
            POSBusiness.IInventoryRepository Inventory { get; }

            POSBusiness.ICategoryRepository Category { get; }

            POSBusiness.IManufacturerRepository Manufacturer { get; }

            POSBusiness.ISupplierRepository Supplier { get; }

            POSBusiness.ICustomersRepository Customers { get; }

            POSBusiness.IVATStatusRepository VAT { get; }

            POSBusiness.IInvoiceRepository Invoice { get; }

            POSBusiness.IItemSalesRepository ItemSales { get; }

            POSBusiness.IUserRepository User { get; }

            POSBusiness.IOrderRepository Order { get; }

            int Complete();
        }
    }
}
