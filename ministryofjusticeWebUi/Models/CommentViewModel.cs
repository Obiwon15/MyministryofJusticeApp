using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ministryofjusticeWebUi.Models
{
    public class CommentViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string ApplicationUserId { get; set; }

        [JsonProperty("caseId")]
        public int CaseId { get; set; }

        [JsonProperty("message")]
        [DisplayName("Put your comment ")]
        [Required(ErrorMessage = "comment is Required")]
        public string CommentMessage { get; set; }
        
    }
}