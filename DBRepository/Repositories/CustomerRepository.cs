using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Extensions;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;

namespace AdaruServer.DBRepositories.Repositories
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
            return await context.CustomerInfos.FirstOrDefaultAsync(ci => ci.Id == customerId);
        }

        public async Task<List<CustomerInfo>> GetCustomers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.CustomerInfos.ToListAsyncSafe();
        }
    }
}
