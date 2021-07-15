using System;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;
using System.Web.Http;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Services;

namespace ministryofjusticeWebUi.Controllers
{
    [Authorize]
    public class CommentController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentController()
        {
        }

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        //[System.Web.Http.HttpPost]
        //public ActionResult CreateComment(CommentViewModel model)
        //{
          //  if (!ModelState.IsValid)
            //    return HttpNotFound();

            //var comment = Mapper.Map<CommentViewModel, Comment>(model);
            //_unitOfWork.CommentRepo.Insert(comment);
            //_unitOfWork.CommentRepo.Save();
            //model.Id = comment.Id;
            //return Json(new {status = "success", data = model});  
        //}

        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok(new{ stuff = "works" });
        }

        [HttpGet]
        public IHttpActionResult ShowComments(int id)
        {
            var comments = _commentService.GetComments(id);
            return Ok(new {comments = comments});
        }

        [HttpPost]
        public IHttpActionResult CreateComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var caseId = _commentService.CreateComment(model);
                return ShowComments(caseId);
            }

            return BadRequest(ModelState);
        }
    }
}