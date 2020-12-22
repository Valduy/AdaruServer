using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Task = Models.Task;

namespace DBRepository.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Task> GetTask(int taskId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            try
            {
                await using var context = ContextFactory.CreateDbContext(ConnectionString);
                await context.AddAsync(task);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException innerException)
                {
                    switch (innerException.SqlState)
                    {
                        case PgsqlErrors.RaiseException:
                            throw new RepositoryException(innerException.MessageText);
                    }
                }

                throw;
            }
        }

        public async Task<List<Models.Task>> GetAllTasks()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.ToListAsyncSafe();
        }

        public async Task<List<Models.Task>> GetTasks(Expression<Func<Models.Task, bool>> predicate)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.Where(predicate).ToListAsyncSafe();
        }

        public async Task<List<Task>> GetNewTasks()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks
                .Where(t => t.IdStatus == context.TaskStatuses.First(s => s.Status == "new").Id)
                .ToListAsyncSafe();
        }

        public async Task<List<Task>> GetCustomerTasks(int customerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.Where(t => t.IdCustomer == customerId).ToListAsyncSafe();
        }

        public async Task<List<Task>> GetPerformerTasks(int performerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tasks.Where(t => t.IdPerformer == performerId).ToListAsyncSafe();
        }

        public async Task<List<Tag>> GetTaskTags(int taskId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.Where(t => context.TaskTags
                .Where(tt => tt.IdTask == taskId)
                .Select(tt => tt.IdTag).Contains(t.Id)).ToListAsyncSafe();
        }

        public async System.Threading.Tasks.Task ChangeTaskStatus(int taskId, string status)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var task = context.Tasks.FirstOrDefault(t => t.Id == taskId) 
                       ?? throw new RepositoryException("Такой задачи не существует.");
            var temp = context.TaskStatuses.FirstOrDefault(ts => ts.Status == status)
                       ?? throw new RepositoryException("Нет такого статуса.");
            task.IdStatus =  temp.Id;
            await context.SaveChangesAsync();
        }
    }
}
