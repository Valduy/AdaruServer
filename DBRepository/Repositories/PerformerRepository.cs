using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Helpers;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
{
    public class PerformerRepository : BaseRepository, IPerformerRepository
    {
        public PerformerRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<PerformerInfo> GetPerformer(int performerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.PerformerInfos.FirstOrDefault(pi => pi.Id == performerId);
        }

        public async Task<List<Tag>> GetPerformerTags(int performerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Tags.Where(t => context.PerformerTags
                .Where(pt => pt.IdPerformer == performerId)
                .Select(pt => pt.IdTag).Contains(t.Id)).ToListAsyncSafe();
        }

        public async Task<List<PerformerInfo>> GetPerformers()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.PerformerInfos.ToListAsyncSafe();
        }

        public async Task<List<PerformerInfo>> GetPerformers(IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"select * from get_performers_by_tags({string.Join(',', parameters)})";
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            return reader.Select(r => new PerformerInfo()
            {
                Id = (int)r[0],
                Login = r[1].ToString(),
                Username = r[2].ToString(),
                Role = r[3].ToString(),
                Path = r[4] is DBNull ? null : r[4].ToString(),
                Resume = r[5] is DBNull ? null : r[5].ToString(),
                Raiting = r[6] is DBNull ? 0 : (decimal)r[6],
                Expirience = r[7] is DBNull ? 0 : (long)r[7],
            }).ToList();
        }

        public async Task AddTagsToPerformer(PerformerInfo performer, IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"call add_tags_to_performer({performer.Id}, {string.Join(',', parameters)})";

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (PostgresException ex)
            {
                switch (ex.SqlState)
                {
                    case PgsqlErrors.RaiseException:
                        throw new RepositoryException(ex.MessageText);
                }

                throw;
            }
        }

        public async Task DeletePerformerTags(PerformerInfo performer, IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"call delete_performer_tags({performer.Id}, {string.Join(',', parameters)})";
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
