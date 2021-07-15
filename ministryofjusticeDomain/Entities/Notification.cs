using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ReceiverId { get; set; }
        public bool IsViewed { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
