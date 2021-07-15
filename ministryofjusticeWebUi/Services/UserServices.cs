using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Services
{
	public class UserServices : IUserServices
	{
		private readonly IUserUnitOfWork _userUnitOfWork;

		public UserServices(IUserUnitOfWork userUnitOfWork)
		{
            _userUnitOfWork = userUnitOfWork;
		}

		public async Task<IdentityResult> CreateAccount(CreateAccountViewModel model)
		{
			var user = Mapper.Map<CreateAccountViewModel, ApplicationUser>(model);
			var result = _userUnitOfWork.UserManagerRepo.CreateUser(user);
			if (result.Succeeded)
			{
				var role = await _userUnitOfWork.RoleService.FindRoleByIdAsync(model.RoleId);
				var roleResult = await _userUnitOfWork.RoleService.AssignRoleAsync(user.Id, model.RoleId);
				if (roleResult.Succeeded)
				{
					if (role.Name == "Lawyer")
					{
						await _userUnitOfWork.UserManagerRepo.CreateLawyer(user.Id, model.DepartmentId);
					}
					else if (role.Name == "Director of Department")
					{
						await _userUnitOfWork.UserManagerRepo.CreateDeptHead(user.Id, model.DepartmentId);
					}
					await _userUnitOfWork.MailSender.AccountCreatedAsync(user.Email, user.FullName);
				}
			}
			return result;
		}


		public async Task<IdentityResult> UpdateProfile(ProfileViewModel model)
		{
			var result =
			  await _userUnitOfWork.UserManagerRepo.ChangePassword(model.Email, model.OldPassword, model.Password);

			if (result.Succeeded)
			{
				var user = Mapper.Map<ProfileViewModel, ApplicationUser>(model);
                _userUnitOfWork.ProfileRepo.UpdateProfile(user);
			}
            return result;
        }

        public DashboardViewModel FetchDashboardData()
        {
            return new DashboardViewModel()
            {
                Department = _userUnitOfWork.DepartmentRepo.GetDepartments(),
                Users = _userUnitOfWork.UserManagerRepo.GetAllUsers(),
                Roles = _userUnitOfWork.RoleService.GetRoles().Where(r => r.Name != "System Administrator")
            };
        }

        public CreateAccountViewModel GetAccountViewModel()
        {
            return new CreateAccountViewModel()
            {
                Department = _userUnitOfWork.DepartmentRepo.GetDepartments(),
                Roles = _userUnitOfWork.RoleService.GetRoles()
            };
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _userUnitOfWork.DepartmentRepo.GetDepartments();
        }
        public IEnumerable<IdentityRole> GetRoles()
        {
            return _userUnitOfWork.RoleService.GetRoles(); ;
        }

		public async Task<ProfileViewModel> GetProfile( string name)
		{
			var user = await _userUnitOfWork.UserManagerRepo.GetUser(name);
			var vm = Mapper.Map<ApplicationUser, ProfileViewModel>(user);
			return vm;
		}

        public ProfileViewModel GetCurrentProfile()
        {
            return Mapper.Map<ProfileViewModel>(_userUnitOfWork.UserManagerRepo.GetCurrentUser());
        }
	}
}