using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ministryofjusticeDomain.Interfaces.Services
{
    public interface IRoleService
    {
        IEnumerable<IdentityRole> GetRoles();
        void CreateRoles(string roleName);
        Task DeleteRole(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<IdentityResult> AssignRoleAsync(string userId, string roleId);
        Task<IdentityResult> RemoveRoleAsync(string userId, string roleId);
        Task<IdentityResult> UpdateRoleAsync(string roleId, string newRoleName);
        Task<IdentityRole> FindRoleByIdAsync(string id);
    }
}
