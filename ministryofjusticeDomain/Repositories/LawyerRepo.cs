using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using static System.Web.HttpContext;

namespace ministryofjusticeDomain.Repositories
{
    public class LawyerRepo : GenericRepo<Lawyer>, ILawyerRepo
    {
        private readonly ApplicationDbContext _context;

        public LawyerRepo(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<Lawyer> GetLawyersInDepartment(int departmentId)
        {
            var lawyers = _context.Lawyers.Where(l => l.DepartmentId == departmentId)
                .Include(l=>l.AssignedCases)
                .Include(l => l.User).ToList();
            return lawyers;
        }

        public IEnumerable<Case> GetAllLawyerCase()
        {
            var userId = Current.User.Identity.GetUserId();
            var lawyer = _context.Lawyers.FirstOrDefault(l => l.User.Id == userId);

            if (lawyer == null)
                throw new Exception("there is no current user");

            return _context.Cases.ToList().Where(l => l.LawyerId == lawyer.Id);
            
            
        }

        public Lawyer GetCurrentLawyer()
        {
            var userId = Current.User.Identity.GetUserId();
            var lawyer = _context.Lawyers.FirstOrDefault(l => l.ApplicationUserId == userId);
            return lawyer;
        }
    }
}