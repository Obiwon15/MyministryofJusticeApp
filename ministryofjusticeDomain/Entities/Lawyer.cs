using System;
using System.Collections.Generic;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Entities
{
    public class Lawyer
    {
        public int Id { get; set; }
        public string License { get; set; }
        public string ApplicationUserId { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Case> AssignedCases { get; set; }
        public DateTime TimeRegister { get; set; }
    }
}
