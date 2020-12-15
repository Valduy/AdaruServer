using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetClient(int clientId);
        Task<Client> GetClient(string login);
        Task AddClient(Client client);
    }
}
