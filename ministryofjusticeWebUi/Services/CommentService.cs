using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ministryofjusticeWebUi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _commentRepo;

        public CommentService(ICommentRepo commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public int CreateComment(CommentViewModel commentModel)
        {
            var comment = Mapper.Map<Comment>(commentModel);
            _commentRepo.Insert(comment);
            _commentRepo.Save();
            return comment.Id;
        }

        public IEnumerable<CommentViewModel> GetComments(int caseId)
        {
            var caseComments = _commentRepo.GetAll().Where(c=>c.CaseId == caseId);
            return Mapper.Map<IEnumerable<CommentViewModel>>(caseComments);
        }
    }
}