using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Repositories;
using ministryofjusticeDomain.Services;
using ministryofjusticeWebUi.Infrastructures;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class CaseServiceTest
    {
        private Mock<ICaseUnitOfWork> _caseUnitOfWorkMock;
        private IEnumerable<Case> caseList;
        private IEnumerable<DepartmentHead> deptHead;
        private IEnumerable<Lawyer> lawyers;
        private IEnumerable<CaseDetailsViewModel> caseList2;
        private Mock<IMappingEngine> mapper = new Mock<IMappingEngine>();

        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<Case, CaseDetailsViewModel>(); });
            lawyers = new List<Lawyer>
            {
                new Lawyer {Id = 1, DepartmentId = 1, ApplicationUserId = "receiver"},
                new Lawyer {Id = 2, DepartmentId = 1, ApplicationUserId = "receiv"},
                new Lawyer {Id = 3, DepartmentId = 1, ApplicationUserId = "rec"},
            };
            caseList = new List<Case>
            {
                new Case
                {
                    Id = 1, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Processing, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 1, LawyerId = 1
                },
                new Case
                {
                    Id = 2, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Accepted, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 2, LawyerId = 1
                },
                new Case
                {
                    Id = 3, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Pending, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 2, LawyerId = 1
                }
            };
            caseList2 = new List<CaseDetailsViewModel>
            {
                new CaseDetailsViewModel
                {
                    Id = 1, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Pending, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 1, LawyerId = 1
                },
                new CaseDetailsViewModel
                {
                    Id = 2, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Accepted, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 2, LawyerId = 1
                },
                new CaseDetailsViewModel
                {
                    Id = 3, CaseID = "", Fullname = "", Email = "", PhoneNumber = "", AdditionalInformation = "",
                    SubmitDate = DateTime.Now, StatusID = Status.Accepted, GenderID = Gender.Male,
                    ReportSubject = "", DepartmentId = 2, LawyerId = null
                }
            };
        }

        [Test]
        [TestCase(1)]
        public void Test_Getting_Case_By_Id(int id)
        {
            var user = new ApplicationUser
            {
                Id = "receiver"
            };
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            var caseObject = new Case
            {
                Id = 1, CaseID = "", SubmitDate = DateTime.Now, StatusID = Status.Pending, GenderID = Gender.Male,
                DepartmentId = 1, LawyerId = 1, Lawyer = new Lawyer
                {
                    ApplicationUserId = "receiver", Id = 1
                }
            };
            var dept = new Department
                {Id = 1, DepartmentHead = new DepartmentHead {Id = 1, ApplicationUserId = "newuser"}};
            var lawyer = new Lawyer {Id = 1, DepartmentId = 1};
            _mock.Setup(mr => mr.CaseRepo.GetById(caseObject.Id))
                .Returns(caseList.SingleOrDefault(x => x.Id == caseObject.Id));
            _mock.Setup(mr => mr.DepartmentRepo.GetById(caseObject.DepartmentId)).Returns(dept);
            _mock.Setup(mr => mr.LawyerRepo.GetById(caseObject.LawyerId.Value)).Returns(lawyer);
            _mock.Setup(mr => mr.UserManagerRepo.GetUserDetails(caseObject.Lawyer.ApplicationUserId))
                .Returns(user);
            _caseUnitOfWorkMock.Setup(x => x.UserManagerRepo.GetCurrentUser()).Returns(user);

            var result = _case.GetCaseDetails(id);

            Assert.AreEqual(result.GetType(), typeof(CaseDetailsViewModel));
            _caseUnitOfWorkMock.Verify(r => r.CaseRepo.GetById(caseObject.Id));
            _caseUnitOfWorkMock.Verify(r => r.DepartmentRepo.GetById(caseObject.DepartmentId));
            _caseUnitOfWorkMock.Verify(r => r.LawyerRepo.GetById(caseObject.LawyerId.Value));
        }

        [Test]
        public void Test_Getting_Cases_Based_On_Their_Status()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            var caseObject = new Case
            {
                Id = 1, CaseID = "", Fullname = "", SubmitDate = DateTime.Now, StatusID = Status.Pending,
                GenderID = Gender.Male, DepartmentId = 1, LawyerId = 1
            };
            var status = "Pending";
            var result = _case.GetCasesByStatus(caseList2, status);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Can_Return_All_Cases_SearchBy_CaseID_Or_ClientEmail()
        {
            //Arrage
            var _mock = new Mock<ICaseUnitOfWork>();
            SearchCategory cases = SearchCategory.CaseId;
            var caseID = "category=CaseId&param=CASEMOJ541";
            var _case = new CaseService(_mock.Object);
            _mock.Setup(mr => mr.CaseRepo.Search(cases, caseID)).Returns(caseList.Where(x => x.CaseID == caseID));

            //Act
            var result = _case.SearchCases(caseID);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("categoryList=CAseId&ParamsCASEMOJ541", "")]
        public void NewCase_For_Search(string CaseId, string Params)
        {
            var caseTCase = new List<Case>();
            var queryList = new List<QueryType>();
            var categoryList = new List<SearchCategory>();
            SearchCategory cases = SearchCategory.CaseId;

            var data = _caseUnitOfWorkMock.Setup(_ => _.CaseRepo.Search(SearchCategory.CaseId, Params))
                .Returns(() => caseTCase.Where(_ => _.CaseID == CaseId));

            //Act_
            var caseService = new CaseService(_caseUnitOfWorkMock.Object);
            var result = caseService.SearchCases(CaseId);

            Assert.Equals(result.Count(), data);
        }

        [Test]
        public void Inset_Comments()
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _case = new CaseService(_caseUnitOfWorkMock.Object);
            var comment = new Comment
            {
                Id = 1,
                CommentMessage = "",
                CaseId = 1,
                CreatedAt = DateTime.Now
            };
            _caseUnitOfWorkMock.Setup(mr => mr.CommentRepo.Insert(comment));
            _caseUnitOfWorkMock.Setup(mr => mr.CommentRepo.Save());
            _case.InsertComment(comment);
        }

        [Test]
        public void Notify_Client()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_caseUnitOfWorkMock.Object);
            string receiverEmail = "a@gmail.com";
            string receiverName = "Test";
            var model = new CaseDetailsViewModel
            {
                Id = 1,
                CaseID = "",
                Fullname = "",
                Email = "",
                PhoneNumber = "",
                AdditionalInformation = "",
                SubmitDate = DateTime.Now,
                StatusID = Status.Pending,
                GenderID = Gender.Male,
                ReportSubject = "",
                DepartmentId = 1,
                LawyerId = 1
            };
            _mock.Setup(mr => mr.CaseRepo.GetById(model.Id)).Returns(caseList.SingleOrDefault(x => x.Id == model.Id));
            _mock.Setup(mr => mr.MailSender.SendCaseStatus(receiverEmail, receiverName, new Case()))
                .Returns(Task.CompletedTask);
            _case.NotifyClient(model);
        }

        [Test]
        public void Can_Audit_Cases_WhenAccepted()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            var model = new DepartmentHead()
            {
                Id = 1,
                ApplicationUserId = "userone",
                User = new ApplicationUser() {Id = "userone", DepartmentId = 1}
            };

            CaseAction action = CaseAction.Accept;
            int id = 1;
            _mock.Setup((mr => mr.CaseRepo.GetById(id))).Returns(caseList.SingleOrDefault(x => x.Id == id));
            _mock.Setup(mr => mr.DepartmentHeadRepo.GetDepartmentHead(id))
                .Returns(model);
            _mock.Setup(mr => mr.CaseRepo.AssignToDept(id));
            _mock.Setup(mr => mr.NotificationRepo.CreateNotification(id, model.ApplicationUserId, "Testing"));
            _mock.Setup(mr => mr.CommentRepo.Save());

            //Act
            _case.AuditCase(id, action);

        }

        [Test]
        public void Can_Audit_Cases_WhenRejected()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            var model = new DepartmentHead()
            {
                Id = 1,
                ApplicationUserId = "userone",
                User = new ApplicationUser() {Id = "userone", DepartmentId = 1}
            };

            CaseAction action = CaseAction.Reject;
            int id = 1;
            _mock.Setup((mr => mr.CaseRepo.GetById(id))).Returns(caseList.SingleOrDefault(x => x.Id == id));
            _mock.Setup(mr => mr.DepartmentHeadRepo.GetDepartmentHead(id))
                .Returns(model);
            _mock.Setup(mr => mr.CaseRepo.RejectCase(id));
            _mock.Setup(mr => mr.CommentRepo.Save());

            //Act
            _case.AuditCase(id, action);
        }

        [Test]
        public void Test_Assign_Action()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_caseUnitOfWorkMock.Object);
            var user = new ApplicationUser
            {
                Id = "receiver",
                FirstName = "fname",
                LastName = "lname",
                Email = "email",
                PhoneNumber = "phonenumber"
            };
            var caseObject = new Case
            {
                Id = 1,
                CaseID = "",
                SubmitDate = DateTime.Now,
                StatusID = Status.Pending,
                GenderID = Gender.Male,
                DepartmentId = 1,
                LawyerId = 1,
                Lawyer = new Lawyer
                {
                    ApplicationUserId = "receiver",
                    Id = 1
                }
            };
            int caseId = 1;
            int lawyerId = 1;
            CaseAction action = CaseAction.Assign;
            CaseAction action2 = CaseAction.UnAssign;
            AssignedTo assigned = AssignedTo.Lawyer;
            _mock.Setup(mr => mr.LawyerRepo.GetById(lawyerId)).Returns(lawyers.SingleOrDefault(x => x.Id == lawyerId));
            _mock.Setup(mr => mr.UserManagerRepo.GetUserDetails(caseObject.Lawyer.ApplicationUserId)).Returns(user);
            _mock.Setup(mr => mr.MailSender.NotifyAsync(user.Email, user.FullName, assigned))
                .Returns(Task.CompletedTask);
            _mock.Setup(mr => mr.NotificationRepo.CreateNotification(caseId, user.Email, "Testing"));
            _mock.Setup(mr => mr.CaseRepo.AssignCaseToLawyer(lawyerId, caseId)).Returns(true);
            _mock.Setup(mr => mr.CaseRepo.UnAssignCaseToLawyer(lawyerId, caseId)).Returns(true);
            var result = _case.AssignAction(caseId, lawyerId, action);
            var result2 = _case.AssignAction(caseId, lawyerId, action2);
            Assert.AreEqual(result, true);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void Get_Case_Lawyer()
        {
            var model = new Case
            {
                Id = 1,
                CaseID = "",
                Fullname = "",
                Email = "",
                PhoneNumber = "",
                AdditionalInformation = "",
                SubmitDate = DateTime.Now,
                StatusID = Status.Pending,
                GenderID = Gender.Male,
                ReportSubject = "",
                DepartmentId = 1,
                LawyerId = 1
            };
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_caseUnitOfWorkMock.Object);
            int id = 1;
            _caseUnitOfWorkMock.Setup(mr => mr.CaseRepo.GetById(id)).Returns(model);
            _caseUnitOfWorkMock.Setup(mr => mr.LawyerRepo.GetLawyersInDepartment(model.DepartmentId))
                .Returns(lawyers.Where(i => i.DepartmentId == model.DepartmentId));
            var result = _case.GetCaseLawyers(id);
            Assert.IsNotNull(result);
            _mock.Verify(r => r.CaseRepo.GetById(id));
            _mock.Verify(mr => mr.LawyerRepo.GetLawyersInDepartment(model.DepartmentId));
        }

        [Test]
        public void Get_Lawyers_Cases()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);

            var userId = 2;
            _mock.Setup(mr => mr.LawyerRepo.GetAllLawyerCase()).Returns(caseList.Where(i => i.LawyerId == userId));
            mapper.Setup(mr => mr.Map<IEnumerable<Case>, IEnumerable<CaseDetailsViewModel>>(caseList))
                .Returns(caseList2);
            var result = _case.GetLawyersCases();
            /*Assert.IsInstanceOfType(result, typeof(CaseDetailsViewModel));*/
            /*Assert.AreEqual(result, caseList2);*/
            Assert.AreEqual(0, result.Count());

        }

        [Test]
        public void Get_Department_Cases()
        {
            var user = new ApplicationUser
            {
                Id = "receiver",
                FirstName = "fname",
                LastName = "lname",
                Email = "email",
                PhoneNumber = "phonenumber",
                DepartmentId = 1
            };
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_caseUnitOfWorkMock.Object);
            string assigned = "Assigned";
            _mock.Setup(x => x.UserManagerRepo.GetCurrentUser()).Returns(user);
            var result = _case.GetDepartmentCases(caseList2, assigned);
            Assert.AreEqual(0, result.Count());
            _mock.Verify(x => x.UserManagerRepo.GetCurrentUser());
        }

        [Test]
        public void FilterLawyerCase()
        {
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            var userId = "receiver";
            IEnumerable<CaseDetailsViewModel> model = caseList2.Where(x => x.LawyerId != null);
            _mock.Setup(mr => mr.LawyerRepo.GetCurrentLawyer())
                .Returns(lawyers.SingleOrDefault(x => x.ApplicationUserId == userId));
            //Act
            var result = _case.FilterLawyerCases(caseList2);
            //Assert
            Assert.AreEqual(2, result.Count());
            _mock.Verify(mr=> mr.LawyerRepo.GetCurrentLawyer());
        }
        [Test]
        public void CaseStatusChange()
        {
            //Assign
            var _mock = new Mock<ICaseUnitOfWork>();
            var _case = new CaseService(_mock.Object);
            int caseId = 1;
            var status = Status.Processing;
            var query = caseList.SingleOrDefault(x => x.Id == caseId && x.StatusID == status);
            _mock.Setup(x => x.CaseRepo.ChangeCaseStatus(caseId, status))
                .Returns(query);
            _mock.Setup(mr => mr.MailSender.SendCaseStatus(query.Email, query.Fullname, query))
                .Returns(Task.CompletedTask);
            _mock.Setup(mr => mr.NotificationRepo.AlertAttorneyGeneral(caseId, "Request to close case"));
            _mock.Setup(mr => mr.CaseRepo.Save());
            //Act
            var result = _case.CaseStatusChange(caseId, status);
            //Assert
            Assert.IsTrue(result);
        }


}

    
}
