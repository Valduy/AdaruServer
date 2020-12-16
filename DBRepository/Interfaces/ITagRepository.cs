using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetTag(int tagId);
        Task<List<Tag>> GetAllTags();
    }
}
