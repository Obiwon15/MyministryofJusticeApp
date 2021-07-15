using System.Collections.Generic;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeWebUi.Models
{
    public class ManageDepartmentViewModel
    {
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}