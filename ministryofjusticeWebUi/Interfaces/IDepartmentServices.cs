using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Entities;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
	public interface IDepartmentServices
	{
		ManageDepartmentViewModel ManageDepartment();
		bool CreateDepartment(DepartmentViewModel departmentViewModel);
		bool UpdateDepartment(DepartmentViewModel model);
		Department GetDepartmentById(int id);
        IEnumerable<Department> GetAllDepartments();
    }
}