using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Views
{
    public interface IAddOrEditManufacturerView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        string ManufacturerId { get; set; }
        string ManufacturerName { get; set; }

        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}
