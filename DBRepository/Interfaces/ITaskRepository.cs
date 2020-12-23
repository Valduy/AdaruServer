using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface ITaskRepository
    {
        Task<Models.Task> GetTask(int taskId);
        Task AddTask(Models.Task task);
        Task<List<Models.Task>> GetAllTasks();
        Task<List<Models.Task>> GetTasks(Expression<Func<Models.Task, bool>> predicate);
        Task<List<Models.Task>> GetNewTasks();
        Task<List<Models.Task>> GetCustomerTasks(int customerId);
        Task<List<Models.Task>> GetPerformerTasks(int performerId);
        Task<List<Tag>> GetTaskTags(int taskId);
        Task UpdateTask(Models.Task task);
        Task AddTagsToTask(Models.Task task, IEnumerable<string> tags);
    }
}
