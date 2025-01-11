using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Winkel
{
    internal class DataStorageMetDataTable : DataStorage, IDataStorageDAO
    {
        const string CUSTOMER_TABLE_NAME = "Customers";

        const string SELECT_ALL_CUSTOMERS_CONFIG_KEY = "SELECT_ALL_CUSTOMERS";
        const string SELECT_CUSTOMER_BY_ID = "SELECT_CUSTOMER_BY_ID";
        const string DELETE_FROM_CUSTOMERS_BY_ID = "DELETE_CUSTOMER_BY_ID";

        readonly DbDataAdapter dataAdapter;
        readonly DataTable dataTable;

        public DataStorageMetDataTable() : base()
        {
            dataAdapter = factory.CreateDataAdapter() ?? throw new Exception();

            String? selectQuery = ConfigurationManager.AppSettings[SELECT_ALL_CUSTOMERS_CONFIG_KEY] ?? throw new NoQueryException("DataStorageMetDataTable");
            String? deleteQuery = ConfigurationManager.AppSettings[DELETE_FROM_CUSTOMERS_BY_ID] ?? throw new NoQueryException("DataStorageMetDataTable");

            dataTable = new(CUSTOMER_TABLE_NAME);

            using DbConnection conn = GetConnection();
            conn.Open();

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectQuery;
            dataAdapter.SelectCommand = cmd;

            DbCommand deleteCommand = conn.CreateCommand();
            deleteCommand.CommandText = deleteQuery;
            dataAdapter.DeleteCommand = deleteCommand;

            dataAdapter.Fill(dataTable);
        }

        public void AddCustomer(Customer customer)
        {
            DataRow newRow = dataTable.NewRow();

            newRow["CustomerNumber"] = customer.CustomerNumber;
            newRow["CustomerName"] = customer.CustomerName;
            newRow["ContactLastName"] = customer.ContactLastName;
            newRow["ContactFirstName"] = customer.ContactFirstName;
            newRow["Phone"] = customer.Phone;
            newRow["AddressLine1"] = customer.AddressLine1;
            newRow["AddressLine2"] = customer.AddressLine2;
            newRow["City"] = customer.City;
            newRow["State"] = customer.State;
            newRow["PostalCode"] = customer.PostalCode;
            newRow["Country"] = customer.Country;
            newRow["SalesRepEmployeeNumber"] = customer.SalesRepEmployeeNumber;
            newRow["CreditLimit"] = customer.CreditLimit;

            dataTable.Rows.Add(newRow);
        }

        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(Customer customer)
        {
            try
            {
                DbCommand dbCommand = dataAdapter.DeleteCommand ?? throw new NoQueryException("DeleteCustomer");
                AddParameter(dbCommand, CUSTOMERNUMBER, DbType.Int32, customer.CustomerNumber);
                dataAdapter.DeleteCommand = dbCommand;

                DataRow[] rows = dataTable.Select($"customerNumber = {customer.CustomerNumber}");
                foreach (DataRow row in rows)
                {
                    dataTable.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public Customer GetCustomerById(int id)
        {
            Customer customer = new();
            try
            {
                DataRow[] rows = dataTable.Select($"customerNumber = {id}");
                if (rows.Length == 0)
                    throw new NoResultsFound("GetCusomterById");
                DataRow row = rows[0];
                customer.CustomerNumber = row.Field<int>("CustomerNumber");
                customer.CustomerName = row.Field<string>("CustomerName");
                customer.ContactLastName = row.Field<string>("ContactLastName");
                customer.ContactFirstName = row.Field<string>("ContactFirstName");
                customer.Phone = row.Field<string>("Phone");
                customer.AddressLine1 = row.Field<string>("AddressLine1");
                customer.AddressLine2 = row.Field<string>("AddressLine2");
                customer.City = row.Field<string>("City");
                customer.State = row.Field<string>("State");
                customer.PostalCode = row.Field<string>("PostalCode");
                customer.Country = row.Field<string>("Country");
                customer.SalesRepEmployeeNumber = row.Field<int>("SalesRepEmployeeNumber");
                customer.CreditLimit = row.Field<double>("CreditLimit");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return customer;
        }

        public void SyncDb()
        {
            dataTable.AcceptChanges();
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = [];
            DataRow[] rows = dataTable.Select(null, null, DataViewRowState.CurrentRows);
            foreach (DataRow row in rows)
            {
                customers.Add(new()
                {
                    CustomerNumber = row.IsNull("CustomerNumber") ? 0 : row.Field<int>("CustomerNumber"),
                    CustomerName = row.IsNull("CustomerName") ? "" : row.Field<string>("CustomerName"),
                    ContactLastName = row.IsNull("ContactLastName") ? "" : row.Field<string>("ContactLastName"),
                    ContactFirstName = row.IsNull("ContactFirstName") ? "" : row.Field<string>("ContactFirstName"),
                    Phone = row.IsNull("Phone") ? "" : row.Field<string>("Phone"),
                    AddressLine1 = row.IsNull("AddressLine1") ? "" : row.Field<string>("AddressLine1"),
                    AddressLine2 = row.IsNull("AddressLine2") ? "" : row.Field<string>("AddressLine2"),
                    City = row.IsNull("City") ? "" : row.Field<string>("City"),
                    State = row.IsNull("State") ? "" : row.Field<string>("State"),
                    PostalCode = row.IsNull("PostalCode") ? "" : row.Field<string>("PostalCode"),
                    Country = row.IsNull("Country") ? "" : row.Field<string>("Country"),
                    SalesRepEmployeeNumber = row.IsNull("SalesRepEmployeeNumber") ? 0 : row.Field<int>("SalesRepEmployeeNumber"),
                    CreditLimit = row.IsNull("SalesRepEmployeeNumber") ? 0.0 : row.Field<double>("CreditLimit")
                });
            }

            return customers;
        }
    }
}
