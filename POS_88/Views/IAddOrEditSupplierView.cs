using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IAddOrEditSupplierView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        string SupplierId { get; set; }
        string SupplierName { get; set; }
        string SupplierAddress { get; set; }
        string SupplierCPNo { get; set; }

        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}
