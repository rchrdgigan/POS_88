using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Presenters
{
    class PrintConfirmPresenter
    {
        private readonly UnitOfWorkPOS unitOfWork;

        public PrintConfirmPresenter()
        {
            unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
        }
    }
}
