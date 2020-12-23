using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetClient(int clientId);
        Task<Client> GetClient(string login);
        Task<List<PerformerInfo>> GetPerformers();
        Task<List<CustomerInfo>> GetPerformers(IEnumerable<string> tags);
        Task<List<CustomerInfo>> GetCustomers();
        Task<List<CustomerInfo>> GetCustomers(IEnumerable<string> tags);
        Task AddClient(Client client);
    }
}
