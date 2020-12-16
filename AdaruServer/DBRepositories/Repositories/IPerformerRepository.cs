using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;

namespace AdaruServer.DBRepositories.Repositories
{
    public class PerformerRepository : IPerformerRepository
    {
        public Task<PerformerInfo> GetPerformer(int performerId)
        {

            throw new NotImplementedException();
        }

        public Task<List<PerformerInfo>> GetPerformers()
        {
            throw new NotImplementedException();
        }
    }
}
