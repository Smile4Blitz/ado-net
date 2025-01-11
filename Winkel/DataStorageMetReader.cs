using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Numerics;
using Microsoft.IdentityModel.Protocols;
using Winkel;

namespace Winkel
{
    public class DataStorageMetReader : DataStorage
    {
        const string GET_ALL_CUSTOMERS_CONFIG_KEY = "SELECT_ALL_CUSTOMERS";
        const string GET_CUSTOMER_BY_CUSTOMERID_CONFIG_KEY = "SELECT_CUSTOMER_BY_ID";
        const string ADD_CUSTOMER_CONFIG_KEY = "INSERT_NEW_CUSTOMER";
        const string INSERT_NEW_ORDER_CONFIG_KEY = "INSERT_NEW_ORDER";

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = [];
            String? query = ConfigurationManager.AppSettings[GET_ALL_CUSTOMERS_CONFIG_KEY] ?? throw new NoQueryException("GetCustomer");

            try
            {
                using DbConnection conn = GetConnection();
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                DbDataReader dbDataReader = cmd.ExecuteReader();

                while (dbDataReader.Read())
                    customers.Add(DataReadCustomer(dbDataReader));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            String? query = ConfigurationManager.AppSettings[GET_CUSTOMER_BY_CUSTOMERID_CONFIG_KEY] ?? throw new NoQueryException("GetCustomerById");
            using DbConnection conn = GetConnection();
            conn.Open();

            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;

            AddParameter(cmd, CUSTOMERNUMBER, DbType.Int32, id);

            DbDataReader dbDataReader = cmd.ExecuteReader();
            if (!dbDataReader.HasRows)
                throw new NoResultsFound("GetCustomerById");
            return DataReadCustomer(dbDataReader);
        }

        public void AddCustomer(Customer customer)
        {
            int rowsAffected;
            String? query = ConfigurationManager.AppSettings[ADD_CUSTOMER_CONFIG_KEY] ?? throw new NoQueryException("AddCustomer");

            try
            {
                using DbConnection conn = GetConnection();
                conn.Open();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                AddParameter(cmd, CUSTOMERNUMBER, DbType.Int32, GetMaxCustomerNumber() + 1);
                AddParameter(cmd, CUSTOMERNAME, DbType.String, customer.CustomerName);
                AddParameter(cmd, CONTACTLASTNAME, DbType.String, customer.ContactLastName);
                AddParameter(cmd, CONTACTFIRSTNAME, DbType.String, customer.ContactFirstName);
                AddParameter(cmd, PHONE, DbType.String, customer.Phone);
                AddParameter(cmd, ADDRESSLINE1, DbType.String, customer.AddressLine1);
                AddParameter(cmd, ADDRESSLINE2, DbType.String, customer.AddressLine2);
                AddParameter(cmd, CITY, DbType.String, customer.City);
                AddParameter(cmd, STATE, DbType.String, customer.State);
                AddParameter(cmd, POSTALCODE, DbType.String, customer.PostalCode);
                AddParameter(cmd, COUNTRY, DbType.String, customer.Country);
                AddParameter(cmd, SALESREPEMPLOYEENUMBER, DbType.Int32, customer.SalesRepEmployeeNumber);
                AddParameter(cmd, CREDITLIMIT, DbType.Double, customer.CreditLimit);

                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new NoRowsAffectedException("AddCustomer");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public void AddOrder(Order order)
        {
            int rowsAffected;
            String? query = ConfigurationManager.AppSettings[INSERT_NEW_ORDER_CONFIG_KEY] ?? throw new NoQueryException("AddOrder");

            try
            {
                using DbConnection conn = GetConnection();
                conn.Open();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                AddParameter(cmd, ORDERNUMBER, DbType.Int32, order.Number);
                AddParameter(cmd, ORDERDATE, DbType.Date, order.Ordered);
                AddParameter(cmd, REQUIREDDATE, DbType.Date, order.Required);
                AddParameter(cmd, SHIPPEDDATE, DbType.Date, order.Shipped);
                AddParameter(cmd, STATUS, DbType.String, order.Status);
                AddParameter(cmd, COMMENTS, DbType.String, order.Comments);
                AddParameter(cmd, CUSTOMERNUMBER, DbType.String, order.CustomerNumber);

                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new NoRowsAffectedException("AddOrder");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        int GetMaxCustomerNumber()
        {
            int result = 0;
            String? query = ConfigurationManager.AppSettings["SELECT_MAX_CUSTOMER_NUMBER"] ?? throw new NoQueryException("GetMaxCustomerNumber");

            try
            {
                using DbConnection conn = this.GetConnection();
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                DbDataReader dbDataReader = cmd.ExecuteReader();
                dbDataReader.Read();
                result = dbDataReader.GetInt32(0);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return result;
        }

        static Customer DataReadCustomer(DbDataReader dbDataReader)
        {
            return new()
            {
                CustomerNumber = dbDataReader.IsDBNull(CUSTOMERNUMBER) ? 0 : dbDataReader.GetInt32(CUSTOMERNUMBER),
                CustomerName = dbDataReader.IsDBNull(CUSTOMERNAME) ? "" : dbDataReader.GetString(CUSTOMERNAME),
                ContactLastName = dbDataReader.IsDBNull(CONTACTLASTNAME) ? "" : dbDataReader.GetString(CONTACTLASTNAME),
                ContactFirstName = dbDataReader.IsDBNull(CONTACTFIRSTNAME) ? "" : dbDataReader.GetString(CONTACTFIRSTNAME),
                Phone = dbDataReader.IsDBNull(PHONE) ? "" : dbDataReader.GetString(PHONE),
                AddressLine1 = dbDataReader.IsDBNull(ADDRESSLINE1) ? "" : dbDataReader.GetString(ADDRESSLINE1),
                AddressLine2 = dbDataReader.IsDBNull(ADDRESSLINE2) ? "" : dbDataReader.GetString(ADDRESSLINE2),
                City = dbDataReader.IsDBNull(CITY) ? "" : dbDataReader.GetString(CITY),
                State = dbDataReader.IsDBNull(STATE) ? "" : dbDataReader.GetString(STATE),
                PostalCode = dbDataReader.IsDBNull(POSTALCODE) ? "" : dbDataReader.GetString(POSTALCODE),
                Country = dbDataReader.IsDBNull(COUNTRY) ? "" : dbDataReader.GetString(COUNTRY),
                SalesRepEmployeeNumber = dbDataReader.IsDBNull(SALESREPEMPLOYEENUMBER) ? 0 : dbDataReader.GetInt32(SALESREPEMPLOYEENUMBER),
                CreditLimit = dbDataReader.IsDBNull(CREDITLIMIT) ? 0.0 : dbDataReader.GetDouble(CREDITLIMIT)
            };
        }

        static void AddParameter(DbCommand cmd, string parameterName, DbType dbType, Object? value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.DbType = dbType;
            dbParameter.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(dbParameter);
        }
    }
}
