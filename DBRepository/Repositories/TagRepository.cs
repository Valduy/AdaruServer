using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Extensions;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;

namespace AdaruServer.DBRepositories.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Tag> GetTag(int tagId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
        }

        public async Task<List<Tag>> GetAllTags()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.ToListAsyncSafe();
        }
    }
}
