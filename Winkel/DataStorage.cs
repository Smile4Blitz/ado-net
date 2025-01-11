using System.Data.Common;
using System.Text;
using System.Configuration;
using System;

namespace Winkel
{
    public abstract class DataStorage
    {

        // Gebruik deze constanten; 
        // zo hoef je niet op juiste spelling te letten
        // en kan je de juiste waarde uit de aangeboden lijst halen 
        // (enkele letters intikken volstaat).
        public const string CUSTOMERNUMBER = "customerNumber";
        public const string CUSTOMERNAME = "customerName";
        public const string CONTACTLASTNAME = "contactLastName";
        public const string CONTACTFIRSTNAME = "contactFirstName";
        public const string PHONE = "phone";
        public const string ADDRESSLINE1 = "addressLine1";
        public const string ADDRESSLINE2 = "addressLine2";
        public const string CITY = "city";
        public const string STATE = "state";
        public const string POSTALCODE = "postalCode";
        public const string COUNTRY = "country";
        public const string SALESREPEMPLOYEENUMBER = "salesRepEmployeeNumber";
        public const string CREDITLIMIT = "creditLimit";

        public const string ORDERNUMBER = "orderNumber";
        public const string ORDERDATE = "orderDate";
        public const string REQUIREDDATE = "requiredDate";
        public const string SHIPPEDDATE = "shippedDate";
        public const string STATUS = "status";
        public const string COMMENTS = "comments";

        public const string PRODUCTCODE = "productCode";
        public const string QUANTITYORDERED = "quantityOrdered";
        public const string PRICEEACH = "priceEach";
        public const string ORDERLINENUMBER = "orderLineNumber";

        protected readonly DbProviderFactory factory;
        protected readonly string connectionString;
        protected readonly string factoryString;

        protected StringBuilder errorMessages = new (); // kan handig zijn; niet verplicht.

        public DataStorage()
        {
            connectionString = ConfigurationManager.ConnectionStrings["customers"].ConnectionString.ToString();
            factoryString = ConfigurationManager.ConnectionStrings["customers"].ProviderName.ToString();
            factory = DbProviderFactories.GetFactory(factoryString);
        }

        protected DbConnection GetConnection()
        {
            DbConnection? conn = factory.CreateConnection();
            if (conn == null)
            {
                throw new Exception("GetConnection: couldn't create a connection.");
            }
            conn.ConnectionString = connectionString;
            return conn;
        }



    }
}
