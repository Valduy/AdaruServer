using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Models;

namespace DBRepository.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<UserRole> GetUserRole(int roleId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.UserRoles.FirstOrDefault(ur => ur.Id == roleId);
        }

        public async Task<UserRole> GetUserRole(string role)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.UserRoles.FirstOrDefault(ur => ur.Role == role);
        }

        public async Task<List<UserRole>> GetAllUserRoles()
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.UserRoles.ToListAsyncSafe();
        }
    }
}
