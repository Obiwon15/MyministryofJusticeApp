using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using static System.Web.HttpContext;

namespace ministryofjusticeDomain.Repositories
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileRepo(ApplicationDbContext context)
        {
            _context = context;
            _userManager = Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        /// <summary>
        /// Updates user profile role tables based on their roles
        /// </summary>
        /// <param name="nUser"></param>
        public void UpdateProfile(ApplicationUser nUser)
        {
            var user = _userManager.FindByEmail(nUser.Email);
            user.FirstName = nUser.FirstName;
            user.LastName = nUser.LastName;
            user.EmailConfirmed = true;
            _userManager.Update(user);
        }
    }
}
