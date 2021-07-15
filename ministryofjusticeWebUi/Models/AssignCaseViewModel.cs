using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeWebUi.Models
{
    public class AssignCaseViewModel
    {
        [Required]
        public int CaseId { get; set; }
        public Case Case { get; set; }
        public IEnumerable<Lawyer> Lawyers { get; set; }
        [Display(Name = "Lawyer")]
        public int LawyerId { get; set; }

        public Lawyer Lawyer { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}