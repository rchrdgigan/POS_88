using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using ClosedXML.Excel;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Views
{
    public partial class CriticalStocksView : Form
    {
        private UnitOfWorkPOS _unitOfWork;
        private POSImplementation _posImplement;
        public CriticalStocksView()
        {
            InitializeComponent();
            this._posImplement = new POSImplementation();
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            btnExportExcel.Click += btnExporttoExcel_Click;
        }

        private void CriticalStocksView_Load(object sender, EventArgs e)
        {
            this.dataGrid.DataSource = (object)this._unitOfWork.Inventory.GetCriticalItems();
        }

        private void btnExporttoExcel_Click(object sender, EventArgs e) => this.ExporttoExcel();

        private void ExporttoExcel()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Save to Excel";
            int num1 = (int)saveFileDialog.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dataTable = new DataTable();
            IEnumerable<spGetCriticalStocks_Result> criticalItems = this._unitOfWork.Inventory.GetCriticalItems();
            PropertyInfo[] properties = typeof(spGetCriticalStocks_Result).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataColumnCollection columns = dataTable.Columns;
                string name = propertyInfo.Name;
                Type dataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                if ((object)dataType == null)
                    dataType = propertyInfo.PropertyType;
                DataColumn column = new DataColumn(name, dataType);
                columns.Add(column);
            }
            foreach (spGetCriticalStocks_Result criticalStocksResult in criticalItems)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)criticalStocksResult);
                dataTable.Rows.Add(objArray);
            }
            string fileName = saveFileDialog.FileName;
            if (fileName != "")
            {
                using (XLWorkbook xlWorkbook = new XLWorkbook())
                {
                    xlWorkbook.Worksheets.Add(dataTable, "Critical Stocks");
                    xlWorkbook.SaveAs(fileName);
                }
                int num2 = (int)MessageBox.Show("Report Successfully Exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Process.Start(fileName);
            }
            Cursor.Current = Cursors.Default;
        }

    }
}
