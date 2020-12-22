using System.Collections.Generic;
using System.Threading.Tasks;
using TaskStatus = Models.TaskStatus;

namespace DBRepository.Interfaces
{
    public interface IStatusRepository
    {
        Task<TaskStatus> GetTaskStatus(int statusId);
        Task<TaskStatus> GetTaskStatus(string status);
        Task<List<TaskStatus>> GetAllTaskStatuses();
    }
}
