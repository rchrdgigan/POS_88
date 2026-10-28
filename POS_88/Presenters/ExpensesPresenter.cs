using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using POS_88.Views;
using POS_88.Database;
using POS_88.Persistence;

namespace POS_88.Presenters
{
    public class ExpensesPresenter
    {

        private readonly IExpensesView view;
        private readonly string authenticatedUserName;
        private readonly POSImplementation posImplementation;
        private UnitOfWorkPOS unitOfWork;
        private BindingSource expensesBindingSource;

        public ExpensesPresenter(IExpensesView view, string authenticatedUserName)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
            this.authenticatedUserName = authenticatedUserName ?? string.Empty;
            this.unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this.posImplementation = new POSImplementation();
            this.expensesBindingSource = new BindingSource();

            this.view.SearchEvent += SearchUsers;
            this.view.ClearEvent += ClearFilters;
            this.view.DeleteEvent += DeleteExpenses;

            view.SetExpensesListBindingSource(this.expensesBindingSource);
            this.LoadExpenses();
        }

        private async void DeleteExpenses(object sender, EventArgs e)
        {
            int selectedID;
            string selectedExpense = this.view.SelectedExpense;
            if (string.IsNullOrWhiteSpace(selectedExpense))
            {
                MessageBox.Show("Select an expens before deleting.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to delete selected expens data?", "Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!int.TryParse(selectedExpense, out selectedID))
            {
                MessageBox.Show("The selected expens id not converting a string to int.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                expens expens = this.unitOfWork.Expenses
                    .Find(x => x.expensesid == selectedID)
                    .FirstOrDefault();

                if (expens == null)
                {
                    MessageBox.Show("The selected expens could not be found.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.unitOfWork.Expenses.Remove(expens);
                this.unitOfWork.Complete();

                MessageBox.Show("Expens deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await this.GetExpensesData(this.view.SearchParms);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadExpenses()
        {
            await this.GetExpensesData(this.view.SearchParms);
        }

        public async Task GetExpensesData(string param)
        {
            DateTime from;
            DateTime to;

            if (!DateTime.TryParse(view.From, out from) || !DateTime.TryParse(view.To, out to))
            {
                MessageBox.Show("Enter a valid date range.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (from.Date > to.Date)
            {
                MessageBox.Show("The From date must not be later than the To date.", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.view.ShowExpensesListLoading();
            try
            {
                DataTable expensesData = await Task.Run(() => this.posImplementation.SearchExpenses(new SearchRequestSales()
                {
                    from = from.Date,
                    to = to.Date,
                    SearchParam = this.view.SearchParms
                }));
                this.expensesBindingSource.DataSource = expensesData;
                decimal totalAmount = GetTotal(expensesData, "AMOUNT");

                view.TotalExpenses = "Total Expenses: " + totalAmount.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.view.HideExpensesListLoading();
            }
        }

        private decimal GetTotal(DataTable cashCount, string columnName)
        {
            object total = cashCount.Compute("SUM(" + columnName + ")", string.Empty);
            return total == DBNull.Value ? 0M : Convert.ToDecimal(total);
        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            this.view.ClearFilters();
            await this.GetExpensesData(string.Empty);
        }

        private async void SearchUsers(object sender, EventArgs e)
        {
            await this.GetExpensesData(this.view.SearchParms);
        }

        public void Reload()
        {
            this.LoadExpenses();
        }
    }
}
