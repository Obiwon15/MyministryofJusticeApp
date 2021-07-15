using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ministryofjusticeDomain.Enum
{
    /// <summary>
    /// Represents the current status of a case from the start stage Pending, to the end stage closed
    /// </summary>
    public enum Status
    {
        [Display(Name = "Rejected")]
        Rejected = 0,
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "Assigned")]
        Assigned = 2,
        [Display(Name = "Accepted")]
        Accepted = 3,
        [Display(Name = "Processing")]
        Processing = 4,
        [Display(Name="Request to Close")]
        RequestClose = 5,
        [Display(Name="Closed")]
        Closed = 6
    }
}
