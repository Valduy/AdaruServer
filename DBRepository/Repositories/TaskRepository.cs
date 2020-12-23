using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;
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
                await context.Tasks.AddAsync(task);
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

        public async System.Threading.Tasks.Task UpdateTask(Task task)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var entry = context.Tasks.FirstOrDefault(t => t.Id == task.Id) 
                       ?? throw new RepositoryException("Такой задачи не существует.");
            context.Entry(entry).CurrentValues.SetValues(task);
            await context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddTagsToTask(Task task, IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"call add_tags_to_task({task.Id}, {string.Join(',', parameters)})";

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (PostgresException ex)
            {
                switch (ex.SqlState)
                {
                    case PgsqlErrors.RaiseException:
                        throw new RepositoryException(ex.MessageText);
                }
            }
        }
    }
}
