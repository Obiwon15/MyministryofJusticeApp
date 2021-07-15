using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;
using NUnit.Framework;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class CommentServiceTest
    {
        private Mock<ICommentRepo> _commentMock;
        private readonly Mock<IMappingEngine> mapper = new Mock<IMappingEngine>();
        private IEnumerable<Comment> caseComments;

        [SetUp]
        public void SetUp()
        {
            _commentMock = new Mock<ICommentRepo>();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CommentViewModel, Comment>();
                cfg.CreateMap<Comment, CommentViewModel>();
                cfg.CreateMap<IList<Comment>, IList<CommentViewModel>>();
            });

            caseComments = new List<Comment>
            {
                new Comment {Id = 1, CaseId = 1}, new Comment {Id = 2, CaseId = 1}, new Comment {Id = 3, CaseId = 2},
                new Comment {Id = 4, CaseId = 1}
            };
        }

        [Test]
        public void Create_Comment_Should_Add_Record()
        {
            //Arrange
            var comments = new List<Comment>();
            var _case = new CommentService(_commentMock.Object);

            _commentMock.Setup(_ => _.Insert(It.IsAny<Comment>()))
                .Callback<Comment>(comments.Add)
                .Verifiable();
            _commentMock.Setup(_ => _.Save());
            var commentViewModel = new CommentViewModel
            {
                Id = 1,
                CaseId = 1,
                CommentMessage = "Unit Testing with NUNIT",
                ApplicationUserId = "appUser"
            };
            //Act
            var result = _case.CreateComment(commentViewModel);
            //Assert
            Assert.NotZero(result);
            _commentMock.Verify();
        }

        [Test]
        [TestCase(1)]
        public void Cet_Comment_Returned_Mapped_Commet(int id)
        {
            var expected = new List<Comment>
            {
                new Comment {Id = 1, CaseId = 1}, new Comment {Id = 2, CaseId = 1}, new Comment {Id = 3, CaseId = 1},
                new Comment {Id = 4, CaseId = 1}
            };
            //Arrange
            var _case = new CommentService(_commentMock.Object);
            _commentMock.Setup(mr => mr.GetAll()).Returns(expected);

            //Act
            var result = _case.GetComments(id);

            //Assert
            Assert.NotZero(result.Count());
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(CommentViewModel));
            _commentMock.Verify(c => c.GetAll());
        }
    }
}