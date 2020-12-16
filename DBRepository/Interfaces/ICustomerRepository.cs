using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerInfo> GetCustomer(int customerId);
        Task<List<CustomerInfo>> GetCustomers();
    }
}
