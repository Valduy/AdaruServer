using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Extensions;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;
using Microsoft.EntityFrameworkCore;
using Task = AdaruServer.Models.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Models.Task> GetTask(int taskId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async System.Threading.Tasks.Task AddTask(Models.Task task)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.AddAsync(task);
            await context.SaveChangesAsync();
        }

        public async Task<List<Models.Task>> GetAllTasks()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.ToListAsyncSafe();
        }

        public async Task<List<Models.Task>> GetCustomerTasks(int customerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.Where(t => t.IdCustomer == customerId).ToListAsyncSafe();
        }

        public async Task<List<Tag>> GetTaskTags(int taskId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.Where(t => context.TaskTags
                .Where(tt => tt.IdTask == taskId)
                .Select(tt => tt.IdTag).Contains(t.Id)).ToListAsyncSafe();
        }
    }
}
