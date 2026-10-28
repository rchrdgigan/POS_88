using POS_88.Database;
using POS_88.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace POS_88.Persistence
{
    public class POSImplementation
    {
        private UnitOfWorkPOS _unitOfWork;
        private POSDBEntities _context;

        public POSImplementation()
        {
            this._unitOfWork = new UnitOfWorkPOS(new POSDBEntities());
            this._context = new POSDBEntities();
        }

        public string SaveItem(inventory _inventory, string saveType, string usernm)
        {
            string str = "";
            bool flag = this._unitOfWork.Inventory.Exists((Expression<System.Func<inventory, bool>>)(x => x.itemcode == _inventory.itemcode));
            Decimal? cost1 = _inventory.cost;
            Decimal? price = _inventory.price;
            int num1;
            if (!(cost1.GetValueOrDefault() > price.GetValueOrDefault() & cost1.HasValue & price.HasValue))
            {
                Decimal? cost2 = _inventory.cost;
                Decimal? wholesaleprice = _inventory.wholesaleprice;
                num1 = cost2.GetValueOrDefault() > wholesaleprice.GetValueOrDefault() & cost2.HasValue & wholesaleprice.HasValue ? 1 : 0;
            }
            else
                num1 = 1;
            if (num1 != 0)
                return "Cost cannot be greater than the price";
            if (flag && saveType == "INSERT")
                return "Item Code already exist!";
            try
            {
                if (saveType == "INSERT")
                {
                    this._unitOfWork.Inventory.Add(_inventory);
                    this._unitOfWork.Complete();
                    this.AddHistory(_inventory.itemcode, Convert.ToInt32((object)_inventory.quantity), "Inventory Count", 0, usernm, "ADD", 0);
                }
                else
                {
                    inventory inventory = new inventory();
                    Decimal num2 = Convert.ToDecimal((object)this._unitOfWork.Inventory.Find((Expression<System.Func<inventory, bool>>)(x => x.itemcode == _inventory.itemcode)).FirstOrDefault<inventory>().quantity);
                    Decimal num3 = Convert.ToDecimal((object)_inventory.quantity) - num2;
                    this._context.Entry<inventory>(_inventory).State = EntityState.Modified;
                    this._context.SaveChanges();
                    this._unitOfWork.StockHistory.LogStocksMovement("ADD", "Inventory Count", new int?(0), new int?(0), _inventory.itemcode, new Decimal?(num3), usernm);
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public DataTable SearchOrders(SearchRequestOrders request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestOrders);
            foreach (PropertyInfo property in typeof(SearchRequestOrders).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty && s != "ALL")
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestOrders).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} AND (orderid LIKE '%{s}%' OR supplier.suppliername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} AND convert(DATE,OrderDate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} AND {name}='{s}'";
                                    break;
                            }
                        }
                        else
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} WHERE (orderid LIKE '%{s}%' OR supplier.suppliername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} WHERE convert(DATE,OrderDate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} WHERE {name}='{s}'";
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            IEnumerable<spGetOrders_Result> orders = this._unitOfWork.Order.GetOrders(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetOrders_Result).GetProperties();
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
            foreach (spGetOrders_Result spGetOrdersResult in orders)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)spGetOrdersResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public string SaveExpenses(expens _expenses, string saveType, string usernm)
        {
            string str = "";
            try
            {
                if (saveType == "INSERT")
                {
                    this._unitOfWork.Expenses.Add(_expenses);
                    this._unitOfWork.Complete();
                }
                else
                {
                    this._unitOfWork.Expenses.Update(_expenses);
                    this._unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public DataTable SearchSaleswithParam(string strParam)
        {
            IEnumerable<spGetSalesByTransaction_Result> salesByTransaction = this._unitOfWork.ItemSales.GetSalesByTransaction(strParam);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetSalesByTransaction_Result).GetProperties();
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
            foreach (spGetSalesByTransaction_Result transactionResult in salesByTransaction)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)transactionResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public void AddHistory(
          string itemcode,
          int qty,
          string docType,
          int customerID,
          string usernm,
          string addType,
          int origQty)
        {
            inventory inventory1 = new inventory();
            inventory inventory2 = this._unitOfWork.Inventory.Find((Expression<System.Func<inventory, bool>>)(x => x.itemcode == itemcode)).FirstOrDefault<inventory>();
            stockshistory entity = new stockshistory();
            entity.itemcode = itemcode;
            entity.documenttype = docType;
            entity.customerid = new int?(customerID);
            entity.stockdate = new DateTime?(DateTime.Now);
            if (addType == "ADD")
            {
                entity.quantity = new Decimal?((Decimal)qty);
                entity.instock = inventory2.quantity;
            }
            else
            {
                int num = qty - origQty;
                entity.quantity = new Decimal?((Decimal)num);
                entity.instock = new Decimal?((Decimal)(origQty + num));
            }
            entity.createdby = usernm;
            this._unitOfWork.StockHistory.Add(entity);
            this._unitOfWork.Complete();
        }

        public string SaveCategory(category _category, string saveType)
        {
            string str = "";
            if (this._unitOfWork.Category.Exists((Expression<System.Func<category, bool>>)(x => x.categoryid == _category.categoryid)) && saveType == "INSERT")
                return "Category ID already exist!";
            try
            {
                if (saveType == "INSERT")
                    this._unitOfWork.Category.Add(_category);
                else
                    this._unitOfWork.Category.Update(_category);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string SaveManufacturer(manufacturer _manufacturer, string saveType)
        {
            string str = "";
            if (this._unitOfWork.Manufacturer.Exists((Expression<System.Func<manufacturer, bool>>)(x => x.manufacturerid == _manufacturer.manufacturerid)) && saveType == "INSERT")
                return "Manufacturer ID already exist!";
            try
            {
                if (saveType == "INSERT")
                    this._unitOfWork.Manufacturer.Add(_manufacturer);
                else
                    this._unitOfWork.Manufacturer.Update(_manufacturer);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string SaveSupplier(supplier _supplier, string saveType)
        {
            string str = "";
            if (this._unitOfWork.Supplier.Exists((Expression<System.Func<supplier, bool>>)(x => x.supplierid == _supplier.supplierid)) && saveType == "INSERT")
                return "Supplier ID already exist!";
            try
            {
                if (saveType == "INSERT")
                    this._unitOfWork.Supplier.Add(_supplier);
                else
                    this._unitOfWork.Supplier.Update(_supplier);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string SaveCustomer(customer _customer, string saveType)
        {
            string str = "";
            if (this._unitOfWork.Customers.Exists((Expression<System.Func<customer, bool>>)(x => x.customername == _customer.customername)) && saveType == "INSERT")
                return "Customer already exist!";
            try
            {
                if (saveType == "INSERT")
                    this._unitOfWork.Customers.Add(_customer);
                else
                    this._unitOfWork.Customers.Update(_customer);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string SaveUser(user _user, string saveType)
        {
            string str = "";
            if (this._unitOfWork.User.Exists((Expression<System.Func<user, bool>>)(x => x.usernm == _user.usernm)) && saveType == "INSERT")
                return "Username already exist!";
            try
            {
                if (saveType == "INSERT")
                    this._unitOfWork.User.Add(_user);
                else
                    this._unitOfWork.User.Update(_user);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string SaveSupplier(supplier _supplier)
        {
            string str = "";
            if (this._unitOfWork.Supplier.Exists((Expression<System.Func<supplier, bool>>)(x => x.supplierid == _supplier.supplierid)))
                return "Supplier ID already exist!";
            try
            {
                this._unitOfWork.Supplier.Add(_supplier);
                this._unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public DataTable SearchInventory(SearchRequestInventory request)
        {
            string str = "";
            int num = 0;
            Type type = typeof(SearchRequestInventory);
            foreach (PropertyInfo property in typeof(SearchRequestInventory).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty && s != "ALL")
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestInventory).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (num > 0)
                    {
                        if (name == "SearchParam")
                            str = $"{str} AND (itemcode LIKE '%{s}%' OR itemname LIKE '%{s}%')";
                        else
                            str = $"{str} AND {name}='{s}'";
                    }
                    else if (name == "SearchParam")
                        str = $"{str} WHERE (itemcode LIKE '%{s}%' OR itemname LIKE '%{s}%')";
                    else
                        str = $"{str} WHERE {name}='{s}'";
                    ++num;
                }
            }
            IEnumerable<spGetInventory_Result> inventory = this._unitOfWork.Inventory.GetInventory(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetInventory_Result).GetProperties();
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
            foreach (spGetInventory_Result getInventoryResult in inventory)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)getInventoryResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchSales(SearchRequestSales request)
        {
            string usernm = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestSales);
            foreach (PropertyInfo property in typeof(SearchRequestSales).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty && s != "ALL")
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestSales).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    usernm = $"{usernm} AND (receiptno LIKE '%{s}%' OR customers.customername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    usernm = $"{usernm} AND convert(DATE,solddate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    usernm = $"{usernm} AND {name}='{s}'";
                                    break;
                            }
                        }
                        else
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    usernm = $"{usernm} WHERE (receiptno LIKE '%{s}%' OR customers.customername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    usernm = $"{usernm} WHERE convert(DATE,solddate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    usernm = $"{usernm} WHERE {name}='{s}'";
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            IEnumerable<spGetSalesByTransaction_Result> salesByTransaction = this._unitOfWork.ItemSales.GetSalesByTransaction(usernm);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetSalesByTransaction_Result).GetProperties();
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
            foreach (spGetSalesByTransaction_Result transactionResult in salesByTransaction)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)transactionResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchPaymentHistory(SearchRequestSales request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestSales);
            foreach (PropertyInfo property in typeof(SearchRequestSales).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty && s != "ALL")
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestSales).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} AND (invoice.receiptno LIKE '%{s}%' OR customers.customername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} AND convert(DATE,paydate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} AND {name}='{s}'";
                                    break;
                            }
                        }
                        else
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} WHERE (invoice.receiptno LIKE '%{s}%' OR customers.customername LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} WHERE convert(DATE,paydate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} WHERE {name}='{s}'";
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            IEnumerable<spGetPaymentHistory_Result> paymentHistory = this._unitOfWork.ItemSales.GetPaymentHistory(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetPaymentHistory_Result).GetProperties();
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
            foreach (spGetPaymentHistory_Result paymentHistoryResult in paymentHistory)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)paymentHistoryResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchItemSales(SearchRequestSales request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestSales);
            foreach (PropertyInfo property in typeof(SearchRequestSales).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty && s != "ALL")
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestSales).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} AND (itemsales.itemcode LIKE '%{s}%' OR inventory.itemname LIKE '%{s}%' OR supplier.suppliername LIKE '%{s}%' OR category.categoryname LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} AND convert(DATE,solddate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} AND {name}='{s}'";
                                    break;
                            }
                        }
                        else
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} WHERE (itemsales.itemcode LIKE '%{s}%' OR inventory.itemname LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} WHERE convert(DATE,solddate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} WHERE {name}='{s}'";
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            IEnumerable<spGetSalesByItem_Result> salesByItems = this._unitOfWork.ItemSales.GetSalesByItems(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetSalesByItem_Result).GetProperties();
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
            foreach (spGetSalesByItem_Result salesByItemResult in salesByItems)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)salesByItemResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchExpenses(SearchRequestSales request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestSales);
            foreach (PropertyInfo property in typeof(SearchRequestSales).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty)
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestSales).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} AND (category LIKE '%{s}%' OR description LIKE '%{s}%' OR payee LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} AND convert(DATE,dtexpenses) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} AND {name}='{s}'";
                                    break;
                            }
                        }
                        else
                        {
                            switch (name)
                            {
                                case "SearchParam":
                                    str = $"{str} WHERE (category LIKE '%{s}%' OR description LIKE '%{s}%' OR payee LIKE '%{s}%')";
                                    break;
                                case "to":
                                    str = $"{str} WHERE convert(DATE,dtexpenses) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                                    break;
                                default:
                                    str = $"{str} WHERE {name}='{s}'";
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            IEnumerable<spGetExpenses_Result> expenses = this._unitOfWork.Expenses.GetExpenses(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetExpenses_Result).GetProperties();
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
            foreach (spGetExpenses_Result getExpensesResult in expenses)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)getExpensesResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchStocksHistory(SearchRequestStocksHistory request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestStocksHistory);
            foreach (PropertyInfo property in typeof(SearchRequestStocksHistory).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty)
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestStocksHistory).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            if (name == "to")
                                str = $"{str} AND convert(DATE,stockdate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                            else
                                str = $"{str} AND {name}='{s}'";
                        }
                        else if (name == "to")
                            str = $"{str} WHERE convert(DATE,stockdate) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                        else
                            str = $"{str} WHERE {name}='{s}'";
                        ++num;
                    }
                }
            }
            IEnumerable<spGetStocksHistory_Result> stocksHistory = this._unitOfWork.StockHistory.GetStocksHistory(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetStocksHistory_Result).GetProperties();
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
            foreach (spGetStocksHistory_Result stocksHistoryResult in stocksHistory)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)stocksHistoryResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public DataTable SearchCashCount(SearchRequestCashCount request)
        {
            string str = "";
            DateTime dateTime1 = DateTime.Parse(request.from.ToString());
            DateTime dateTime2 = DateTime.Parse(request.to.ToString());
            int num = 0;
            Type type = typeof(SearchRequestCashCount);
            foreach (PropertyInfo property in typeof(SearchRequestCashCount).GetProperties())
            {
                string s = Convert.ToString(property.GetValue((object)request));
                if (s != string.Empty)
                {
                    string name = property.Name.ToString();
                    if (typeof(SearchRequestCashCount).GetProperty(name).PropertyType.FullName == "System.DateTime")
                        s = DateTime.Parse(s).ToString("MM/dd/yyyy");
                    if (name != "from")
                    {
                        if (num > 0)
                        {
                            if (name == "to")
                                str = $"{str} AND convert(DATE,TimeIn) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                            else
                                str = $"{str} AND {name}='{s}'";
                        }
                        else if (name == "to")
                            str = $"{str} WHERE convert(DATE,TimeIn) BETWEEN '{dateTime1.ToString("MM/dd/yyyy")}' AND '{dateTime2.ToString("MM/dd/yyyy")}'";
                        else
                            str = $"{str} WHERE {name}='{s}'";
                        ++num;
                    }
                }
            }
            IEnumerable<spGetCashCount_Result> cashCount = this._unitOfWork.CashCount.GetCashCount(str);
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = typeof(spGetCashCount_Result).GetProperties();
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
            foreach (spGetCashCount_Result getCashCountResult in cashCount)
            {
                object[] objArray = new object[properties.Length];
                for (int index = 0; index < properties.Length; ++index)
                    objArray[index] = properties[index].GetValue((object)getCashCountResult);
                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }

        public IEnumerable<spSearchByItem_Result> SearchItem(string searchBy, string param)
        {
            return this._unitOfWork.Inventory.SearchItem(searchBy, param);
        }

        public string SaveOrder(Order _order, string saveType)
        {
            string str = "";
            try
            {
                if (saveType == "INSERT")
                {
                    this._unitOfWork.Order.Add(_order);
                    this._unitOfWork.Complete();
                }
                else
                {
                    Order order = new Order();
                    order = this._unitOfWork.Order.Find((Expression<System.Func<Order, bool>>)(x => x.OrderID == _order.OrderID)).FirstOrDefault<Order>();
                    this._context.Entry<Order>(_order).State = EntityState.Modified;
                    this._context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
    }

}
