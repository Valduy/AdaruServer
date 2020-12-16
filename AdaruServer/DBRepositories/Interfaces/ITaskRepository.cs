using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<Models.Task> GetTask(int taskId);
        Task AddTask(Models.Task task);
        Task<List<Models.Task>> GetAllTasks();
        Task<List<Models.Task>> GetCustomerTasks(int customerId);
    }
}
