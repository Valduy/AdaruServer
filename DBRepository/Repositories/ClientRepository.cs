using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Interfaces;
using Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Client> GetClient(int clientId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Client> GetClient(string login)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Clients.FirstOrDefaultAsync(c => c.Login == login);
        }

        public async Task AddClient(Client client)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
        }
    }
}
