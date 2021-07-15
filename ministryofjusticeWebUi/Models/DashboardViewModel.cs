using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeWebUi.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Department> Department { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}