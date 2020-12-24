using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface ITaskInfoRepository
    {
        Task<List<TaskInfo>> GetTasksByTags(IEnumerable<string> tags);
    }
}
