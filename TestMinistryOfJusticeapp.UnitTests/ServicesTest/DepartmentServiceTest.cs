using System;
using System.Collections.Generic;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Services;
using ministryofjusticeWebUi.Models;
using Moq;
using NUnit.Framework;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class DepartmentServiceTest
    {
        private Mock<IDepartmentUnitOfWork> _departmentUnitOfWorkMock;
        private DepartmentServices _departmentServices;
        private List<Department> _department;
        private List<ApplicationUser> _users;
        private readonly Mock<IMappingEngine> _mapper = new Mock<IMappingEngine>();
        private DepartmentViewModel _departmentModel;
        private Department _department1;
        private List<Department> _dbList;
        private Department _dbForDepartment;

        [SetUp]
        public void Setup()
        {
            _departmentUnitOfWorkMock = new Mock<IDepartmentUnitOfWork>();
            _departmentServices = new DepartmentServices(_departmentUnitOfWorkMock.Object);

            _department = new List<Department>();
            _users = new List<ApplicationUser>();
            _departmentModel = new DepartmentViewModel();
            _department1 = new Department();
            _dbList = new List<Department>();

            Mapper.Initialize(cfg => { cfg.CreateMap<DepartmentViewModel, Department>(); });
        }


        [Test(Description = "ManageDepartment_WhenCalled_ReturnManageDepartmentViewModel")]
        public void NewTest_For_ManageDepartment()
        {
            //Arrange
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.GetDepartments()).Returns(_department);
            _departmentUnitOfWorkMock.Setup(_ => _.UserManagerRepo.GetAllUsers()).Returns(_users);

            //Act
            var result = _departmentServices.ManageDepartment();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ManageDepartmentViewModel), result);
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.GetDepartments());
            _departmentUnitOfWorkMock.Verify(_ => _.UserManagerRepo.GetAllUsers());
        }

        [Test(Description = "CreateDepartment_WhenCalled_CreateDepartment")]
        [TestCase(1)]
        [TestCase(3)]
        public void NewTest_For_CreateDepartment(int id)
        {

            //Arrange
            _mapper.Setup(_ => _.Map<DepartmentViewModel, Department>(_departmentModel));
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.Insert(It.IsAny<Department>())).Callback<Department>(_dbList.Add).Verifiable();

            //Act
            var result = _departmentServices.CreateDepartment(_departmentModel);

            //Assert
            Assert.That(result, Is.True);
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.Insert(It.IsAny<Department>()));
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.Save());
        }

        [Test(Description = "UpdateDepartment_IfIdIsNotEqualToTheId_ReturnNull")]
        [TestCase(1)]
        [TestCase(2)]
        public void NewTest_For_UpDateDepartmentIsNull(int id)
        {
            //Arrange
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.GetById(_departmentModel.Id)).Callback(() => throw new Exception());


            //Assert
            var ex = Assert.Throws<Exception>(() => _departmentServices.UpdateDepartment(_departmentModel));
            Assert.That(ex.Message, Is.Not.EqualTo("Actual exception message"));
        }

        [Test(Description = "UpdateDepartment_WhenCalled_UpDateDepartment")]
        [TestCase(1)]
        [TestCase(2)]
        public void NewTest_For_UpDateDepartment(int id)
        {
            //Arrange
            _dbForDepartment = new Department { Id = id };
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.GetById(_departmentModel.Id)).Returns(_dbForDepartment);
            _mapper.Setup(_ => _.Map(_departmentModel, _dbForDepartment));
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.Update(_dbForDepartment)).Verifiable();

            //Act
            var result = _departmentServices.UpdateDepartment(_departmentModel);

            //Assert
            Assert.That(result, Is.True);
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.GetById(_dbForDepartment.Id));
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.Update(_dbForDepartment));
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.Save());

        }

        [Test(Description = "GetDepartmentId_WhenCalled_ReturnTheDepartment")]
        [TestCase(1)]
        [TestCase(2)]
        public void newTest_For_GetDepartmentById(int id)
        {
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.GetById(id)).Returns(() => _department1);

            var result = _departmentServices.GetDepartmentById(id);

            Assert.IsInstanceOf(typeof(Department), result);
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.GetById(id));
        }

        [Test(Description = "GetAllDepartment_WhenCalled_ReturnIEnumerableOfDepartment")]
        public void newTest_For_GetAllDepartment()
        {
            _departmentUnitOfWorkMock.Setup(_ => _.DepartmentRepo.GetDepartments());

            var result = _departmentServices.GetAllDepartments();

            Assert.IsInstanceOf(typeof(IEnumerable<Department>), result);
            _departmentUnitOfWorkMock.Verify(_ => _.DepartmentRepo.GetDepartments());
        }
    }
}