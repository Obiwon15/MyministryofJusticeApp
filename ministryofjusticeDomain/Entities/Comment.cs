using System;
using System.Collections.Generic;
using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public string CommentMessage { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
        public DateTime CreatedAt { get; set; }
        

    }
}