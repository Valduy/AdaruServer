using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface IPerformerRepository
    {
        Task<PerformerInfo> GetPerformer(int performerId);
        Task<List<PerformerInfo>> GetPerformers();
    }
}
