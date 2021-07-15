using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ministryofjusticeWebUi.Models
{
    public class CaseViewModel
    {
        //public int? Id { get; set; }
        public string CaseID { get; set; }
        [Required(ErrorMessage = "You have to enter your full name")]
        [Display(Name = "Full Name")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "You have to enter your email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "You have to enter a heading for this report")]
        [Display(Name = "Report Title")]
        public string ReportSubject { get; set; }
        [Required(ErrorMessage = "Add Report Details")]
        [DisplayName("Details of Report")]
        [DataType(DataType.MultilineText)]
        public string AdditionalInformation { get; set; }
        [Required(ErrorMessage = "You have to enter your phone number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Selelct Gender")]
        [Display(Name="Gender")]
        public Gender GenderID { get; set; }
        public Status StatusID { get; set; }
        public DateTime SubmitDate { get; set; }
        [Required(ErrorMessage = "You have to choose a report type")]
        [Display(Name = "Report Type")]
        public int DepartmentId { get; set; }
        public IEnumerable<Department> Departments { get; set; }

    }
}