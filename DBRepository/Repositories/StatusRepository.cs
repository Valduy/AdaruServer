using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using TaskStatus = Models.TaskStatus;

namespace DBRepository.Repositories
{
    public class StatusRepository : BaseRepository, IStatusRepository
    {
        public StatusRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<TaskStatus> GetTaskStatus(int statusId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.TaskStatuses.First(ts => ts.Id == statusId);
        }

        public async Task<List<TaskStatus>> GetAllTaskStatuses()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.TaskStatuses.ToListAsyncSafe();
        }
    }
}
