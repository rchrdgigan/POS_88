using POS_88.Database;
using POS_88.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_88.Presenters
{
    public class PurchaseItemsPresenter
    {
        private readonly UnitOfWorkPOS unitOfWork;

        public PurchaseItemsPresenter()
        {
            unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
        }

        public spGetSalesByTransaction_Result GetTransaction(int receiptNo)
        {
            if (receiptNo <= 0)
                return null;

            return unitOfWork.ItemSales.GetSalesByTransaction(" WHERE receiptno=" + receiptNo).FirstOrDefault();
        }

        public IEnumerable<spGetPurchasedItems_Result> GetPurchasedItems(int receiptNo)
        {
            if (receiptNo <= 0)
                return Enumerable.Empty<spGetPurchasedItems_Result>();

            return unitOfWork.ItemSales.GetPurchasedItems(receiptNo);
        }
    }
}
