using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
	public interface IUserServices
	{
		Task<IdentityResult> CreateAccount(CreateAccountViewModel model);
		Task<IdentityResult> UpdateProfile(ProfileViewModel model);
		IEnumerable<Department> GetDepartments();
        IEnumerable<IdentityRole> GetRoles();
        DashboardViewModel FetchDashboardData();
        CreateAccountViewModel GetAccountViewModel();
		Task<ProfileViewModel> GetProfile(string name);
        ProfileViewModel GetCurrentProfile();
    }
}
