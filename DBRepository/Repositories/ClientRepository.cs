using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Helpers;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task UpdateClient(Client client)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var entry = context.Clients.FirstOrDefault(t => t.Id == client.Id)
                        ?? throw new RepositoryException("Такого пользователя не существует.");
            context.Entry(entry).CurrentValues.SetValues(client);
            await context.SaveChangesAsync();
        }
    }
}
