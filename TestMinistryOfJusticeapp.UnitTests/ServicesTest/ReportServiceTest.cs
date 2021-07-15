using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;
using NUnit.Framework;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class ReportServiceTest
    {
        private Mock<IReportUnitOfWork> _reportUnitOfMock;
        private ReportService _reportService;
        private IEnumerable<Case> caseList;
        private Mock<IMappingEngine> mapper = new Mock<IMappingEngine>();

        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Case, CaseDetailsViewModel>();
                cfg.CreateMap<CaseViewModel, Case>();
            });
            _reportUnitOfMock = new Mock<IReportUnitOfWork>();
            _reportService = new ReportService(_reportUnitOfMock.Object);
            caseList = new List<Case>
            {
                new Case
                {
                    Id = 1, CaseID = "", Fullname = "", Email = "", PhoneNumber = "",AdditionalInformation = "", SubmitDate = DateTime.Now, StatusID = Status.Pending, GenderID = Gender.Male,
                    ReportSubject = "",DepartmentId = 1, LawyerId = 1
                },
                new Case
                {
                    Id = 2, CaseID = "caseId", Fullname = "", Email = "", PhoneNumber = "",AdditionalInformation = "", SubmitDate = DateTime.Now, StatusID = Status.Accepted, GenderID = Gender.Male,
                    ReportSubject = "",DepartmentId = 2, LawyerId = 1
                }
            };

        }
        [Test]
        [TestCase("caseId")]
        public void Find_Case_Return_CaseViewModel_Or_NullIfAny(string caseId)
        {
            //Arrange
            _reportUnitOfMock.Setup(mr => mr.CaseRepo.TrackCase(caseId))
                .Returns(caseList.SingleOrDefault(s => s.CaseID == caseId));

            //Act
            var result = _reportService.FindCase(caseId);
            //Assert
            Assert.AreEqual(caseId, result.CaseID);
        }

        [Test]
        [TestCase(@"c:\Temp\CaseFiles.txt")]
        public void Submit_A_Case(string path)
        {
            var reportCase = new List<Case>();
            var reportFile = new List<File>();
            var report = new CaseViewModel
            {
                DepartmentId = 1,
                Email = "email",
                StatusID = Status.Accepted,
                Fullname = "fullname",
                ReportSubject = "report"
            };
            var reportDb2 = new Case
            {
                DepartmentId = 1,
                Email = "email",
                StatusID = Status.Pending,
                Fullname = "fullname",
                ReportSubject = "report",
                CaseID = "CASEMOJfgg12j",
                SubmitDate = DateTime.Now
            };

            var filee = new Mock<HttpPostedFileBase>();
            filee.Setup(f => f.ContentLength).Returns(1);
            filee.Setup(f => f.FileName).Returns("test.txt");
            var reportDb = new Case
            {
                DepartmentId = 1,
                Email = "email",
                StatusID = Status.Pending,
                Fullname = "fullname",
                ReportSubject = "report"
            };

            _reportUnitOfMock.Setup(mr => mr.CaseRepo.PostCase(It.IsAny<Case>())).Returns(reportDb2).Verifiable();
            _reportUnitOfMock.Setup(_ => _.CaseRepo.Insert(It.IsAny<Case>()))
                .Callback<Case>(reportCase.Add)
                .Verifiable();
            _reportUnitOfMock.Setup(_ => _.CaseRepo.Save());
            _reportUnitOfMock.Setup(mr => mr.CaseFileRepo.UploadFile(new File(), path, filee.Object)).Returns(new File());
            _reportUnitOfMock.Setup(_ => _.CaseFileRepo.Insert(It.IsAny<File>()))
                .Callback<File>(reportFile.Add)
                .Verifiable();
            _reportUnitOfMock.Setup(_ => _.CaseFileRepo.Save());
            _reportUnitOfMock.Setup(mr => mr.MailSender.SendCaseStatus("test@email.com", "TEST", new Case())).Returns(Task.CompletedTask);

            var result = _reportService.SubmitCase(report, filee.Object, path);
            Assert.AreEqual(result.GetType(), typeof(Case));
            _reportUnitOfMock.Verify(mr => mr.CaseRepo.PostCase(It.IsAny<Case>()));
            _reportUnitOfMock.Verify(_ => _.CaseRepo.Insert(It.IsAny<Case>()));
            _reportUnitOfMock.Verify(_ => _.CaseFileRepo.Insert(It.IsAny<File>()));
        }
    }
}
