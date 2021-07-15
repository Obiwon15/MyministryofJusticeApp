using System.Collections.Generic;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public int? DeptHeadId { get; set; }
        public DepartmentHead DepartmentHead { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Lawyer> Lawyers { get; set; }
    }
}