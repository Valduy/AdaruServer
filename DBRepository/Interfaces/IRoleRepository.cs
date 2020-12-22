using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface IRoleRepository
    {
        Task<UserRole> GetUserRole(int roleId);
        Task<UserRole> GetUserRole(string role);
        Task<List<UserRole>> GetAllUserRoles();
    }
}
