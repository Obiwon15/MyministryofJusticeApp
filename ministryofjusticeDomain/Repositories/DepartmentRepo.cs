using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using static System.Web.HttpContext;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class DepartmentRepo : GenericRepo<Department>, IDepartmentRepo
    {
        private new readonly ApplicationDbContext _context;

        public DepartmentRepo( ApplicationDbContext context)
            : base(context)
        {
            _context = Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        /// <summary>
        /// Adds a user to the department
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="departmentId"></param>
        public void AddUserToDepartment(string userId, int departmentId)
        {
            var department = _context.Departments.Find(departmentId);
            if (department != null)
            {
                var user = _context.Users.Find(userId);
                if (department.Users == null)
                {
                    IList<ApplicationUser> dUsers = new List<ApplicationUser>()
                    {
                        user
                    };
                    department.Users = dUsers;
                }
                else if (!department.Users.Contains(user))
                    department.Users.ToList().Add(user);
            }
        }


        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments.Include(d=>d.Lawyers).Include(d=>d.DepartmentHead);
        }
    }
}