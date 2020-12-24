using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Helpers;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBRepository.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<CustomerInfo> GetCustomer(int customerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.CustomerInfos.FirstOrDefault(ci => ci.Id == customerId);
        }

        public async Task<List<CustomerInfo>> GetCustomers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.CustomerInfos.ToListAsyncSafe();
        }

        public async Task<List<CustomerInfo>> GetCustomers(IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"select * from get_customers_by_tags({string.Join(',', parameters)})";
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            return reader.Select(r => new CustomerInfo()
            {
                Id = (int)r[0],
                Login = r[1].ToString(),
                Username = r[2].ToString(),
                Role = r[3].ToString(),
                IdImage = (int?)r[4],
                Resume = r[5] is DBNull ? null : r[5].ToString(),
                Raiting = r[6] is DBNull ? 0 : (decimal)r[6],
                Expirience = r[7] is DBNull ? 0 : (long)r[7],
            }).ToList();
        }
    }
}
