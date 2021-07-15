using System.Web.Mvc;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
	{
		private readonly IDepartmentServices _departmentServices;

		public DepartmentController()
		{
		}

		public DepartmentController(IDepartmentServices departmentServices) => _departmentServices = departmentServices;


		// GET: Department
		public ActionResult ManageDepartments()
		{
			var vm = _departmentServices.ManageDepartment();
			return View(vm);
		}

		[HttpGet]
		public ActionResult CreateDepartment()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateDepartment(DepartmentViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = _departmentServices.CreateDepartment(model);
				if (result)
				{
					TempData["Message"] = "Department Created Successfully";
					return View("CreateDepartment");
				}
				ModelState.AddModelError("", "An Error Occured");
			}

			return View(model);
		}

		[HttpGet]
		public ActionResult UpdateDepartment(int id)
		{
			var department = _departmentServices.GetDepartmentById(id);
			return View(department);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateDepartment(DepartmentViewModel model)
		{
			var result = _departmentServices.UpdateDepartment(model);
			if (result)
			{
				TempData["Message"] = "Department Updated Successfully";
				return View("UpdateDepartment");
			}
			else
			{
				return HttpNotFound("There is no department found");

			}
		}
	}
}