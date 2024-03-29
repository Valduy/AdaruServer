﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetClient(int clientId);
        Task<Client> GetClient(string login);
        Task AddClient(Client client);
        Task UpdateClient(Client client);
    }
}
