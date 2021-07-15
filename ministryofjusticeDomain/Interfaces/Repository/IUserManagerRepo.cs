using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IUserManagerRepo
    {
        Task<ApplicationUser> GetUser(string email);
        Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword);
        Task CreateDeptHead(string userId, int deptId);
        Task CreateLawyer(string userId, int departmentId);
        IEnumerable<ApplicationUser> GetAllUsers();
        IdentityResult CreateUser(ApplicationUser user);
        ApplicationUser GetCurrentUser();
        ApplicationUser GetUserDetails(string id);
    }
}
