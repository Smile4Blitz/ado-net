using System.Data.Common;
using Microsoft.Identity.Client.Extensions.Msal;
using Winkel;


// Register provider
DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);


DataStorageMetReader storageReader = new DataStorageMetReader();
DataStorageMetDataTable storageDataSet = new DataStorageMetDataTable();

WriteCustomers(storageReader);

//Klant toevoegen
AddCustomer(storageReader);

WriteCustomers(storageReader);

//Bestelling toevoegen;
AddOrder(storageReader);

//DataSet Tests
TestDataSet(storageDataSet);

static void TestDataSet(DataStorageMetDataTable storageDataSet)
{
    Customer c = new()
    {
        AddressLine1 = "EEN !",
        AddressLine2 = "TWEE !",
        City = "city",
        ContactFirstName = "voornaampje",
        ContactLastName = "achternaam contactpersoon",
        Country = "BE",
        CreditLimit = 50.00,
        CustomerName = "naam van klant",
        CustomerNumber = 789789789,
        Phone = "003212456",
        PostalCode = "XM4545",
        SalesRepEmployeeNumber = 10101010,
        State = "West-Vlaanderen"
    };

    storageDataSet.AddCustomer(c);
    List<Customer> customers = storageDataSet.GetCustomers();
    Console.WriteLine($"DataSet customerId: {storageDataSet.GetCustomerById(c.CustomerNumber)}");
    storageDataSet.DeleteCustomer(c);
    storageDataSet.SyncDb();
    Console.WriteLine($"Are customers lists the same: {customers.Count == storageDataSet.GetCustomers().Count}");
    Console.WriteLine($"DataSet customerId: {storageDataSet.GetCustomerById(c.CustomerNumber)}");
}

static void WriteCustomers(DataStorageMetReader storage)
{
    List<Customer> customers = storage.GetCustomers();

    Console.WriteLine("lijst heeft lengte " + customers.Count);

    foreach (Customer cust in customers)
    {
        Console.WriteLine(cust.ToString());
    }
    Console.WriteLine("EINDE DATABASELIJST");

}

static void AddCustomer(DataStorageMetReader storage)
{

    Customer c = new()
    {
        AddressLine1 = "EEN !",
        AddressLine2 = "TWEE !",
        City = "city",
        ContactFirstName = "voornaampje",
        ContactLastName = "achternaam contactpersoon",
        Country = "BE",
        CreditLimit = 50.00,
        CustomerName = "naam van klant",
        CustomerNumber = 789789789,
        Phone = "003212456",
        PostalCode = "XM4545",
        SalesRepEmployeeNumber = 10101010,
        State = "West-Vlaanderen"
    };


    storage.AddCustomer(c);

}

static void AddOrder(DataStorageMetReader storage)
{
    Random random = new();
    Order order = new()
    {
        Comments = "dit order heeft geen comments",
        CustomerNumber = 99999,
        Number = 110000 + random.Next(1, 10000),
        Ordered = DateTime.ParseExact("01/12/2018", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
        Required = DateTime.ParseExact("25/12/2018", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
        Shipped = DateTime.ParseExact("13/12/2018", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
        Status = "ok"
    };

    for (int i = 0; i < 5; i++)
    {
        OrderDetail detail = new()
        {
            OrderNumber = order.Number,
            OrderLineNumber = 1 + i,
            Price = 10.0 * (1 + i),
            ProductCode = "" + ((1 + i) * 111),
            Quantity = 100 * (1 + i)
        };
        order.Details.Add(detail);
    }

    storage.AddOrder(order);
}