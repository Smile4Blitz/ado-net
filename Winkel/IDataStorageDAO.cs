
namespace Winkel
{
    public interface IDataStorageDAO
    {
        void AddCustomer(Customer customer);
        void AddOrder(Order order);
        void DeleteCustomer(Customer customer); 
        Customer GetCustomerById(int id);
        List<Customer> GetCustomers();
    }
}