using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface IPerformerRepository
    {
        Task<PerformerInfo> GetPerformer(int performerId);
        Task<List<PerformerInfo>> GetPerformers();
    }
}
