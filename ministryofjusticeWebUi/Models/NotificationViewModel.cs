using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeWebUi.Models
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ReceiverId { get; set; }
        public bool IsViewed { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}