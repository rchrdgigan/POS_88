using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace POS_88.Views
{
    public interface IAddOrEditUserView
    {
        event EventHandler SaveEvent;
        event EventHandler ClearEvent;

        string UserName { get; set; }
        string FName { get; set; }
        string UserType { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
      
        void SetEditMode(bool isEditMode);
        void ClearFields();
        void Close();
        void Show();
    }
}
