using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<List<Message>> GetMessages(int chatId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Messages
                .Where(m => m.IdChat == chatId)
                .OrderBy(m => m.Time)
                .ThenBy(m => m.Id)
                .ToListAsyncSafe();
        }

        public async Task AddMessage(Message message)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();
        }
    }
}
