using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Enum;

namespace ministryofjusticeWebUi.Models
{
    public class SearchCasesViewModel
    {
        [Required(ErrorMessage = "You have to enter a category")]
        public SearchCategory Category { get; set; }
        [Required(ErrorMessage = "Cannot be empty")]
        public string SearchValue { get; set; }
    }
}