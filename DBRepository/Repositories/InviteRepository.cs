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
    public class InviteRepository : BaseRepository, IInviteRepository
    {
        public InviteRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Invite> GetInvite(int taskId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Invites.FirstOrDefault(i => i.IdTask == taskId);
        }

        public async Task<List<Invite>> GetInvitesToPerformer(int performerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Invites.Where(i => i.IdPerformer == performerId).ToListAsyncSafe();
        }

        public async Task<List<Invite>> GetCustomerInvites(int customerId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Invites.Where(i => context.Tasks
                .Where(t => t.IdCustomer == customerId)
                .Any(t => t.Id == i.IdTask))
                .ToListAsyncSafe();
        }

        public async Task DeleteInvite(Invite invite)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            context.Invites.Remove(context.Invites.First(i => i.IdTask == invite.IdTask));
            await context.SaveChangesAsync();
        }

        public async Task AddInvite(Invite invite)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Invites.AddAsync(invite);
            await context.SaveChangesAsync();
        }
    }
}
