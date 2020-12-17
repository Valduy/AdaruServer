using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerInfo> GetCustomer(int customerId);
        Task<List<CustomerInfo>> GetCustomers();
    }
}
