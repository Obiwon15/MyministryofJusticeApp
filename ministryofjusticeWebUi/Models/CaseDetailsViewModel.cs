using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ministryofjusticeWebUi.Models
{
    public class CaseDetailsViewModel
    {

        public int Id { get; set; }
        public string CaseID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string ReportSubject { get; set; }
        public string AdditionalInformation { get; set; }
        public string PhoneNumber { get; set; }
        public Gender GenderID { get; set; }
        public Status StatusID { get; set; }
        public DateTime SubmitDate { get; set; }
        public ICollection<Comment> Comments { get; set; } 
        public int DepartmentId { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Lawyer> Lawyers { get; set; }
        public Department Department { get; set; }

        public CommentViewModel Comment { get; set; } 
        public Lawyer Lawyer { get; set; }
        public int? LawyerId { get; set; }
        //  public ICollection<Lawyer> Lawyers { get; set; }
        public File File { get; set; }
        public Pagination Pagination { get; set; }

        public bool IsAccepted()
        {
            if (StatusID == Status.Accepted || StatusID == Status.Assigned
            || StatusID == Status.Processing || StatusID == Status.RequestClose) 
                return true;
            return false;
        }

        public string GetBadgeColor()
        {
            switch (StatusID)
            {
                case Status.Pending:
                    return "primary";
                case Status.Assigned:
                    return "success"; 
                case Status.Accepted:
                    return "secondary";
                case Status.Rejected:
                    return "danger";
                case Status.Closed:
                    return "black";
                case Status.RequestClose:
                    return "dark";
                default:
                    return "yellow";
            }
        }
        public string GetTextColor()
        {
            switch (StatusID)
            {
                case Status.Pending:
                    return "primary";
                case Status.Assigned:
                    return "success";
                case Status.Accepted:
                    return "secondary";
                case Status.Rejected:
                    return "danger";
                case Status.Closed:
                    return "black";
                case Status.RequestClose:
                    return "dark";
                default:
                    return "yellow";
            }
        }
    }
}