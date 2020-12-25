using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IInviteRepository
    {
        public Task<Invite> GetInvite(int taskId);
        public Task<List<Invite>> GetInvitesToPerformer(int performerId);
        public Task<List<Invite>> GetCustomerInvites(int customerId);
        public Task DeleteInvite(Invite invite);
        public Task AddInvite(Invite invite);
    }
}
