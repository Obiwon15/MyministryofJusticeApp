using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentViewModel> GetComments(int caseId);
        int CreateComment(CommentViewModel commentModel);
    }
}
