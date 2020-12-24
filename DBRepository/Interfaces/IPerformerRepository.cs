using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface IPerformerRepository
    {
        Task<PerformerInfo> GetPerformer(int performerId);
        Task<List<Tag>> GetPerformerTags(int performerId);
        Task<List<PerformerInfo>> GetPerformers();
        Task<List<PerformerInfo>> GetPerformers(IEnumerable<string> tags);
        System.Threading.Tasks.Task AddTagsToPerformer(PerformerInfo performer, IEnumerable<string> tags);
    }
}
