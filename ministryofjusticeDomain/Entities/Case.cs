using ministryofjusticeDomain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ministryofjusticeDomain.Entities
{
    public class Case
    {
        public int Id { get; set; }
        public string CaseID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string ReportSubject { get; set; }
        public string AdditionalInformation { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime SubmitDate { get; set; }
        public int? LawyerId { get; set; }
        public Lawyer Lawyer { get; set; }
        public Status StatusID { get; set; }
        public Gender GenderID { get; set; }
        public ICollection<File> Files { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
