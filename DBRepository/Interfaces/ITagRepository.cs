using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetTag(int tagId);
        Task<List<Tag>> GetAllTags();
    }
}
