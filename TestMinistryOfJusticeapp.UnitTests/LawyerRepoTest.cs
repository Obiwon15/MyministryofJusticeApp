using System.Collections.Generic;
using System.Web.Mvc;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Controllers;
using Moq;
using NUnit.Framework;

namespace TestMinistryOfJusticeapp.UnitTests
{
    [TestFixture]
   public  class LawyerRepoTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Test(Description = "LawyersViewingAllCases_WhenThereAreCasesAssignToALawyer_ReturnsAllTheLawyerCase")]
        public void New_Test_For_Lawyers()
        {
            _unitOfWorkMock.Setup(x => x.LawyerRepo.GetAllLawyerCase())
                .Returns(_get_lawyer_cases());

            var controller = new CaseController(_unitOfWorkMock.Object);

            //Act
            var result = controller.LawyerViewAllCases() as ViewResult;

            var data = (List<Case>)result.Model;

            //Assert

            Assert.AreEqual(_get_lawyer_cases().Count, data.Count);
        }

        private List<Case> _get_lawyer_cases() => new List<Case>
        {
            new Case
            {
                Id = 1, AdditionalInformation = "This is our case ooo", Email = "chy@gmail.com", DepartmentId = 1,
                LawyerId = 1
            },
            new Case
            {
                Id = 2, AdditionalInformation = "This is our case ooo", Email = "ch@gmail.com", DepartmentId = 1,
                LawyerId = 2
            },
            new Case
            {
                Id = 3, AdditionalInformation = "This is our case ooo", Email = "hy@gmail.com", DepartmentId = 1,
                LawyerId = 1
            },
            new Case
            {
                Id = 4, AdditionalInformation = "This is our case ooo", Email = "cy@gmail.com", DepartmentId = 1,
                LawyerId = 2
            },
        };

       
    }
}
