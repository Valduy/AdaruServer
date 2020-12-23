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

        public async Task<List<PerformerInfo>> GetPerformers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.PerformerInfos.ToListAsyncSafe();
        }

        public async Task<List<PerformerInfo>> GetPerformers(IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"select * from get_performers_by_tags({string.Join(',', parameters)})";

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                var result = reader.Select<PerformerInfo>(r => new PerformerInfo()
                {
                    Id = (int) r["id"],
                    Login = r["login"].ToString(),
                    Username = r["username"].ToString(),
                    Role = r["role"].ToString(),
                    Path = r["path"] is DBNull ? null : r["path"].ToString(),
                    Resume = r["resume"] is DBNull ? null : r["resume"].ToString(),
                    Raiting = r["raiting"] is DBNull ? 0 : (short)r["raiting"],
                    Expirience = r["expirience"] is DBNull ? 0 : (long)r["expirience"],
                });

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            //catch (PostgresException ex)
            //{
            //    switch (ex.SqlState)
            //    {
            //        case PgsqlErrors.RaiseException:
            //            throw new RepositoryException(ex.MessageText);
            //    }

            //    throw;
            //}
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
