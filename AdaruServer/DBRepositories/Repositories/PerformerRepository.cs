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
    public class PerformerRepository : BaseRepository, IPerformerRepository
    {
        public PerformerRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<PerformerInfo> GetPerformer(int performerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.PerformerInfos.FirstOrDefaultAsync(pi => pi.Id == performerId);
        }

        public async Task<List<PerformerInfo>> GetPerformers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.PerformerInfos.ToListAsyncSafe();
        }
    }
}
