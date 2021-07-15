using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using Newtonsoft.Json;

namespace ministryofjusticeWebUi.Models
{
    public class CreateAccountViewModel
    {
        public IEnumerable<Department> Department { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }


        [Display(Name = "Assign Department")] public int DepartmentId { get; set; }

        [Display(Name = "Assign Role")] public string RoleId { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
    }
}