using System.Collections.Generic;
using System.Threading.Tasks;
using TaskStatus = Models.TaskStatus;

namespace DBRepository.Interfaces
{
    public interface IStatusRepository
    {
        Task<TaskStatus> GetTaskStatus(int statusId);
        Task<List<TaskStatus>> GetAllTaskStatuses();
    }
}
