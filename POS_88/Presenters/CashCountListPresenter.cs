using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Views;
using ClosedXML.Excel;
using System.Diagnostics;

namespace POS_88.Presenters
{
    public class CashCountListPresenter
    {
        private readonly ICashCountListView view;
        private readonly POSImplementation posImplementation;
        private readonly BindingSource cashCountListBindingSource;
        private readonly string userID;

        public CashCountListPresenter(ICashCountListView view, string userID)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            if (string.IsNullOrWhiteSpace(userID))
                throw new ArgumentException("An item code is required.", "username");

            this.view = view;
            this.userID = userID;
            this.posImplementation = new POSImplementation();
            this.cashCountListBindingSource = new BindingSource();

            this.view.SetCashCountListBindingSource(this.cashCountListBindingSource);
            this.view.SearchEvent += SearchCashCountList;
            this.view.ClearEvent += SearchCashCountList;
            this.view.ExportExcelEvent += this.ExporttoExcel;

            if (LoadUserCashCountList())
                SearchCashCountList(this, EventArgs.Empty);

        }

        private void ExporttoExcel(object sender, EventArgs e)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParse(view.From, out from) || !DateTime.TryParse(view.To, out to))
            {
                MessageBox.Show("Enter a valid stock-history date range.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (from.Date > to.Date)
            {
                MessageBox.Show("The From date must not be later than the To date.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel File|*.xlsx";
                saveFileDialog.Title = "Save to Excel";
                int num1 = (int)saveFileDialog.ShowDialog();
                Cursor.Current = Cursors.WaitCursor;
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = this.posImplementation.SearchCashCount(new SearchRequestCashCount()
                {
                    from = from.Date,
                    to = to.Date,
                    UserNm = userID
                });
                string fileName = saveFileDialog.FileName;
                if (fileName != "")
                {
                    using (XLWorkbook xlWorkbook = new XLWorkbook())
                    {
                        xlWorkbook.Worksheets.Add(dataTable2, "Cash Count");
                        xlWorkbook.SaveAs(fileName);
                    }
                    int num2 = (int)MessageBox.Show("Report Successfully Exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Process.Start(fileName);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message, "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private bool LoadUserCashCountList()
        {
            try
            {
                UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
                try
                {
                    user item = unitOfWork.User
                        .Find(value => value.usernm == userID)
                        .FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The selected user could not be found.", "No User Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                finally
                {
                    unitOfWork.Dispose();
                }

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SearchCashCountList(object sender, EventArgs e)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParse(view.From, out from) || !DateTime.TryParse(view.To, out to))
            {
                MessageBox.Show("Enter a valid date range.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (from.Date > to.Date)
            {
                MessageBox.Show("The From date must not be later than the To date.", "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable cashCount = this.posImplementation.SearchCashCount(new SearchRequestCashCount
                {
                    from = from.Date,
                    to = to.Date,
                    UserNm = userID
                });

                cashCountListBindingSource.DataSource = cashCount;

                decimal openingAmount = GetTotal(cashCount, "OpeningAmt");
                decimal grandTotal = GetTotal(cashCount, "GrandTotal");

                view.GrandTotal = "Grand Total: ₱ " + grandTotal.ToString("N");
                view.OpeningAmount = "Opening Amount: ₱ " + openingAmount.ToString("N");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Cash Count List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private decimal GetTotal(DataTable cashCount, string columnName)
        {
            object total = cashCount.Compute("SUM(" + columnName + ")", string.Empty);
            return total == DBNull.Value ? 0M : Convert.ToDecimal(total);
        }
    }
}
