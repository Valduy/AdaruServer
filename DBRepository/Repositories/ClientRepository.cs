using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Models;
using Npgsql;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
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
            return context.Clients.FirstOrDefault(c => c.Id == clientId);
        }

        public async Task<Client> GetClient(string login)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Clients.FirstOrDefault(c => c.Login == login);
        }

        public async Task<List<PerformerInfo>> GetPerformers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.PerformerInfos.ToListAsyncSafe();
        }

        public async Task<List<CustomerInfo>> GetPerformers(IEnumerable<string> tags)
        {
            //await using var connection = new NpgsqlConnection(connectionString: ConnectionString);
            //connection.Open();
            //await using var command = connection.CreateCommand();
            //var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            //command.CommandText = $"call get_performers_by_tags({string.Join(',', parameters)})";
            //var reader = command.ExecuteReader();
            //var result = ((IObjectContextAdapter)context)
            
            throw new NotImplementedException();
        }

        public async Task<List<CustomerInfo>> GetCustomers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.CustomerInfos.ToListAsyncSafe();
        }

        public Task<List<CustomerInfo>> GetCustomers(IEnumerable<string> tags)
        {
            throw new NotImplementedException();
        }

        public async Task AddClient(Client client)
        {
            try
            {
                await using var context = ContextFactory.CreateDbContext(ConnectionString);
                await context.Clients.AddAsync(client);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException innerException)
                {
                    switch (innerException.SqlState)
                    {
                        case PgsqlErrors.UniqueViolation:
                            throw new RepositoryException("Пользователь с таки логином уже существует.");
                    }
                }

                throw;
            }
        }
    }
}
