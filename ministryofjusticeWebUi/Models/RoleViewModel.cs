using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ministryofjusticeWebUi.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please enter the role name")]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}