using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(string connectionString, IRepositoryContextFactory contextFactory)
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Chat> GetChat(int chatId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Chats.FirstOrDefault(c => c.Id == chatId);
        }

        public async Task<Chat> GetChat(int clientId1, int clientId2)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Chats.FirstOrDefault(c
                => c.IdSource == clientId1 && c.IdTarget == clientId2
                || c.IdSource == clientId2 && c.IdTarget == clientId1);
        }

        public async Task<List<Chat>> GetChats(int id)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Chats.Where(c => c.IdSource == id || c.IdTarget == id).ToListAsyncSafe();
        }

        public async Task AddChat(Chat chat)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
        }
    }
}
