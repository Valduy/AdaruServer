using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IPerformerRepository
    {
        Task<PerformerInfo> GetPerformer(int performerId);
        Task<List<Tag>> GetPerformerTags(int performerId);
        Task<List<PerformerInfo>> GetPerformers();
        Task<List<PerformerInfo>> GetPerformers(IEnumerable<string> tags);
        Task AddTagsToPerformer(PerformerInfo performer, IEnumerable<string> tags);
        Task DeletePerformerTags(PerformerInfo performer, IEnumerable<string> tags);
    }
}
