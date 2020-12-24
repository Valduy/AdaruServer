using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBRepository.Helpers;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBRepository.Repositories
{
    public class TaskInfoRepository : BaseRepository, ITaskInfoRepository
    {
        public TaskInfoRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<List<TaskInfo>> GetTasksByTags(IEnumerable<string> tags)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var connection = context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            var parameters = (tags as string[] ?? tags.ToArray()).Select(t => $"\'{t}\'");
            command.CommandText = $"select * from get_tasks_by_tags({string.Join(',', parameters)})";
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            return reader.Select(r => new TaskInfo()
            {
                IdClient = (int)r[0],
                Login = r[1].ToString(),
                Username = r[2].ToString(),
                IdTask = (int)r[3],
                Header = r[4].ToString(),
                Content = r[5].ToString(),
                Status = r[6].ToString(),
                Time = (DateTime)r[7],
            }).ToList();
        }
    }
}
