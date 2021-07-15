using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using static System.Web.HttpContext;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Services
{
    /// <summary>
    /// Service responsible for adding roles, deleting roles and assigning roles
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationRoleManager _roleManager;
     

        public RoleService()
        {
            _roleManager = Current.GetOwinContext().Get<ApplicationRoleManager>();
            _userManager = Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        /// <summary>
        /// Method to create roles
        /// </summary>
        /// <param name="roleName"></param>
        public void CreateRoles(string roleName)
        {
            if (!_roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole();
                role.Name = roleName;
                _roleManager.CreateAsync(role);
            }
        }

        /// <summary>
        /// Method to delete role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task DeleteRole(string roleName)
        {
            if (_roleManager.RoleExists(roleName))
            {
                var del=_roleManager.FindByName(roleName);
                await _roleManager.DeleteAsync(del);
            }
        }

        /// <summary>
        /// Gets a the roles
        /// </summary>
        /// <returns>Returns all roles</returns>
        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        /// <summary>
        /// Creates a role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (!_roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole()
                {
                    Name = roleName,
                };

                return await _roleManager.CreateAsync(role);
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = _roleManager.FindById(roleId);
            if (role != null)
            {
                return await _roleManager.DeleteAsync(role);
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// Assigns a user to a role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> AssignRoleAsync(string userId, string roleId)
        {
            var role = _roleManager.FindById(roleId);
            if (role != null)
            {
                return await _userManager.AddToRoleAsync(userId, role.Name);
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RemoveRoleAsync(string userId, string roleId)
        {
            var role = _roleManager.FindById(roleId);
            if (role != null)
            {
                return await _userManager.RemoveFromRoleAsync(userId, role.Name);
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newRoleName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateRoleAsync(string roleId, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role != null)
            {
                return await _roleManager.UpdateAsync(role);
            }
            return IdentityResult.Failed();
        }
        public async Task<IdentityRole> FindRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
    }
}