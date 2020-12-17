using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Interfaces;
using Models;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(string connectionString, IRepositoryContextFactory contextFactory)
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Chat> GetChat(int clientId1, int clientId2)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Chats.FirstOrDefaultAsync(c
                => c.IdSource == clientId1 && c.IdTarget == clientId2
                || c.IdSource == clientId2 && c.IdTarget == clientId1);
        }

        public async Task AddChat(Chat chat)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
        }
    }
}
