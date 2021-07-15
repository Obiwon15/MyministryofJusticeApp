using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class UserServicesTest
    {
        private Mock<IMappingEngine> mapper = new Mock<IMappingEngine>();
        private ApplicationUser user;
        private ProfileViewModel model;
        private CreateAccountViewModel account;
        private IEnumerable<IdentityRole> role;
        private IEnumerable<Department> departments;
        private IEnumerable<ApplicationUser> users;
        [SetUp]
        public void SetUp()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateAccountViewModel, ApplicationUser>();
                cfg.CreateMap<ApplicationUser, ProfileViewModel>();
                cfg.CreateMap<ProfileViewModel, ApplicationUser>();
            });
            model = new ProfileViewModel { FirstName = "fname", Email = "e@gmail.com", LastName = "lastname", ConfirmPassword = "ZXcvbn55!", License = "licencse", OldPassword = "oldpassword", Password = "ZXcvbn55!" };
            user = new ApplicationUser { UserName = "User", FirstName = "First", LastName = "Last", Email = "a@mail.com", DepartmentId = 1, Department = new Department { Id = 1, DepartmentName = "" } };
            account = new CreateAccountViewModel { RoleId = "roleid1", FirstName = "fname", Email = "email", LastName = "lname", DepartmentId = 1 };
            role = new List<IdentityRole>
            {
                new IdentityRole{Id = "roleid1", Name = "Rolename1"}, new IdentityRole{Id = "roleid2", Name = "Rolename2"}, new IdentityRole{Id = "roleid3", Name = "System Administrator"}
            };
            departments = new List<Department>
            {
                new Department{Id = 1, DepartmentName = "dept"}, new Department{Id = 2, DepartmentName = "dept2"}, new Department{Id = 3, DepartmentName = "dept3"}
            };
            users = new List<ApplicationUser>
            {
                new ApplicationUser{Id = "One", UserName = "User", FirstName = "First", LastName = "Last", Email = "a@mail.com", DepartmentId = 1, Department = new Department{Id = 1, DepartmentName = ""} },
                new ApplicationUser{Id = "two", UserName = "User2", FirstName = "First", LastName = "Last", Email = "b@mail.com", DepartmentId = 2, Department = new Department{Id = 2, DepartmentName = ""} },
                new ApplicationUser{Id = "three", UserName = "User3", FirstName = "First", LastName = "Last", Email = "c@mail.com", DepartmentId = 1, Department = new Department{Id = 1, DepartmentName = ""} },
            };
        }
        [Test]
        public void Create_Account_Test_Return_Task_IdentityResult()
        {
            //Arrange
            var mockUser = new Mock<UserManager<ApplicationUser>>();
            var allUsers = new List<ApplicationUser>();
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            var appUser = new ApplicationUser { FirstName = "firstname", Id = "user1", Email = "username", UserName = "username" };

            _mock.Setup(x => x.UserManagerRepo.CreateUser(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser user) => { allUsers.Add(user); }).Returns(IdentityResult.Success);

            _mock.Setup(mr => mr.RoleService.FindRoleByIdAsync(account.RoleId))
                .ReturnsAsync(role.SingleOrDefault(x => x.Id == account.RoleId));
            _mock.Setup(mr => mr.RoleService.AssignRoleAsync(appUser.Id, account.RoleId))
                .ReturnsAsync(IdentityResult.Success);
            _mock.Setup(mr => mr.UserManagerRepo.CreateLawyer(appUser.Id, user.DepartmentId.Value))
                .Returns(Task.CompletedTask);
            _mock.Setup(mr => mr.UserManagerRepo.CreateDeptHead(appUser.Id, user.DepartmentId.Value))
                .Returns(Task.CompletedTask);
            _mock.Setup(mr => mr.MailSender.AccountCreatedAsync(user.Email, user.FirstName))
                .Returns(Task.CompletedTask);
            //Act
            var result = _user.CreateAccount(account);

            //Assert
            Assert.AreEqual(result.GetType(), typeof(IdentityResult));

            _mock.Verify(r => r.UserManagerRepo.CreateUser(user));
            _mock.Verify(r => r.RoleService.FindRoleByIdAsync(account.RoleId));
            _mock.Verify(r => r.RoleService.AssignRoleAsync(appUser.Id, account.RoleId));
            _mock.Verify(r => r.UserManagerRepo.CreateLawyer(appUser.Id, user.DepartmentId.Value));
            _mock.Verify(r => r.UserManagerRepo.CreateDeptHead(appUser.Id, user.DepartmentId.Value));
            _mock.Verify(r => r.MailSender.AccountCreatedAsync(user.Email, user.FirstName));
        }

        [Test]
        public void Get_All_Roles()
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.RoleService.GetRoles()).Returns(role);
            var result = _user.GetRoles();
            Assert.AreEqual(3, result.Count());
            _mock.Verify(r => r.RoleService.GetRoles());
        }
        [Test]
        public void Get_All_Departments()
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.DepartmentRepo.GetDepartments()).Returns(departments);
            var result = _user.GetDepartments();
            Assert.AreEqual(3, result.Count());
            _mock.Verify(r => r.DepartmentRepo.GetDepartments());
        }

        [Test]
        public void Get_Account_View_Model()
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.DepartmentRepo.GetDepartments()).Returns(departments);
            _mock.Setup(mr => mr.RoleService.GetRoles()).Returns(role);
            var result = _user.GetAccountViewModel();
            Assert.AreEqual(result.GetType(), typeof(CreateAccountViewModel));
            _mock.Verify(r => r.DepartmentRepo.GetDepartments());
            _mock.Verify(r => r.RoleService.GetRoles());
        }

        [Test]
        public void Fetch_Dashboard_Data()
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.DepartmentRepo.GetDepartments()).Returns(departments);
            _mock.Setup(mr => mr.UserManagerRepo.GetAllUsers()).Returns(users);
            _mock.Setup(mr => mr.RoleService.GetRoles()).Returns(role.Where(r => r.Name != "System Administrator"));
            var result = _user.FetchDashboardData();
            Assert.AreEqual(result.GetType(), typeof(DashboardViewModel));
            _mock.Verify(r => r.DepartmentRepo.GetDepartments());
            _mock.Verify(r => r.UserManagerRepo.GetAllUsers());
            _mock.Verify(r => r.RoleService.GetRoles());
        }

        [Test]
        [TestCase("a@mail.com")]
        public void Get_Profile(string name)
        {
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.UserManagerRepo.GetUser(name)).ReturnsAsync(users.SingleOrDefault(x => x.Email == name));
            var result = _user.GetProfile(name);
            Assert.AreEqual(result.GetType(), typeof(Task<ProfileViewModel>));
            _mock.Verify(r => r.UserManagerRepo.GetUser(name));
        }

        [Test]
        public void Update_Profile()
        {
            var allUsers = new List<ApplicationUser>();
            var _mock = new Mock<IUserUnitOfWork>();
            var _user = new UserServices(_mock.Object);
            _mock.Setup(mr => mr.UserManagerRepo.ChangePassword(model.Email, model.OldPassword, model.Password)).ReturnsAsync(IdentityResult.Success);
            mapper.Setup(r => r.Map<ProfileViewModel, ApplicationUser>(model)).Returns(user);
            _mock.Setup(x => x.ProfileRepo.UpdateProfile(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser user) =>
                {
                    allUsers.Add(user);
                });
            var result = _user.UpdateProfile(model);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(Task<IdentityResult>));
            _mock.Verify(r => r.UserManagerRepo.ChangePassword(model.Email, model.OldPassword, model.Password));
            _mock.Verify(r => r.ProfileRepo.UpdateProfile(It.IsAny<ApplicationUser>()));
        }
    }
}
