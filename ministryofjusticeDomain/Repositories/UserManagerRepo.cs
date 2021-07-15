using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using static System.Web.HttpContext;
using Microsoft.AspNet.Identity.Owin;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class UserManagerRepo : IUserManagerRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerRepo(ApplicationDbContext context)
        {
            _context = context;
            _userManager = Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        /// <summary>
        /// Returns the list of all the users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.Include(d => d.Department).ToList();
        }


        public IdentityResult CreateUser(ApplicationUser user)
        {
            user.UserName = user.Email;

            //generating user password
            var password = user.FirstName.ToLower() + "123@MOJ";

            //create user
            var result = _userManager.Create(user, password);

            if (result.Succeeded)
                return result;

            string[] errors = {$"Email '{user.Email}' is already registered!"};
            return IdentityResult.Failed(errors);
        }

        public async Task<ApplicationUser> GetUser(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed();

            var result = await _userManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);
            return result;
        }

        public async Task CreateDeptHead(string userId, int deptId)
        {
            var hod = new DepartmentHead() {ApplicationUserId = userId};
            _context.DepartmentHeads.Add(hod);
            _context.SaveChanges();

            var department = _context.Departments.Find(deptId);

            if (department != null)
                department.DeptHeadId = hod.Id;

            await _context.SaveChangesAsync();
        }

        public async Task CreateLawyer(string userId, int departmentId)
        {
            var lawyer = new Lawyer()
            {
                DepartmentId = departmentId,
                TimeRegister = DateTime.Now,
                ApplicationUserId = userId
            };
            _context.Lawyers.Add(lawyer);
            await _context.SaveChangesAsync();
        }

        public ApplicationUser GetCurrentUser()
        {
            return _userManager.FindByEmail(Current.User.Identity.Name);
        }

        public ApplicationUser GetUserDetails(string id)
        {
            return _userManager.Users.First(c => c.Id == id);
        }
    }
}