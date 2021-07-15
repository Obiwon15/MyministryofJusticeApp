using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ministryofjusticeWebUi.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [DisplayName("Put your comment ")]
        [Required(ErrorMessage = "comment is Required")]
        public string CommentMessage { get; set; }
    }
}