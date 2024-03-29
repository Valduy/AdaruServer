﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Models;

namespace DBRepository.Repositories
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
            return context.Tags.FirstOrDefault(t => t.Id == tagId);
        }

        public async Task<List<Tag>> GetAllTags()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.ToListAsyncSafe();
        }
    }
}
