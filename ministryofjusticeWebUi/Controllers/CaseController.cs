using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
	[Authorize]
	public class CaseController : Controller
	{
		private readonly ICaseService _caseService;

		public CaseController()
		{
		}

		public CaseController(ICaseService caseService)
		{
			_caseService = caseService;
		}

        private int pageSize = 5;

		// GET: Case
		public ActionResult Index(string status = null, int page = 1)
        {
            var cases = _caseService.GetCases(QueryType.Status, status);
            var casesList = new CaseListViewModel()
            {
				Cases = cases
				    .OrderByDescending(_ => _.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
				PageInfo = new Pagination()
                {
					CurrentPage = page,
					ItemsPerPage = pageSize,
					TotalItems = cases.Count()
                }
            };
            ViewBag.status = status;
			ViewBag.Title = (status ?? "All") + " Cases";
			return View(casesList);
		}

		/// <summary>
		/// Returns a table of cases assigned to a department
		/// </summary>
		/// <param name="assigned">Either "Assigned" or "Unassigned"</param>
		/// <returns>List of cases based on the case details view model</returns>
		public ActionResult DepartmentCases(string assigned = null, int page = 1)
		{
			var cases = _caseService.GetCases(QueryType.Assigned, assigned);
            var casesList = new CaseListViewModel()
            {
                Cases = cases
				    .OrderByDescending(_ => _.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PageInfo = new Pagination()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = cases.Count()
                }
            };
			ViewBag.Title = (assigned ?? "All") + " Department cases";
            ViewBag.assigned = assigned;
			return View("Index", casesList);
		}

		/// <summary>
		/// Returns a detail view of the case
		/// </summary>
		/// <param name="id">the ID of the case</param>
		/// <returns></returns>
		public ActionResult CaseDetails(int id)
		{
			var vm = _caseService.GetCaseDetails(id);
			if (vm == null)
				return HttpNotFound("Case not found");
			return View(vm);
		}

		public ActionResult AssignedCases(int page = 1)
		{
			var lCases = _caseService.GetCases(QueryType.LawyerCase);
            var casesList = new CaseListViewModel()
            {
                Cases = lCases
				        .OrderByDescending(_ => _.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize),
                PageInfo = new Pagination()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = lCases.Count()
                }
            }; 
			return View("Index", casesList);
		}

		public ActionResult LawyerViewCase(int id)
		{
			var caseDetail = _caseService.GetCaseDetails(id);
			return View("CaseDetails", caseDetail);
		}

		public PartialViewResult ShowLawyers(int id)
		{
			var vm = _caseService.GetCaseLawyers(id);
			return PartialView("_AssignLawyer", vm);
		}

		public ActionResult AssignLawyer(int id, int lawyerId)
		{
			var boolean = _caseService.AssignAction(id, lawyerId, CaseAction.Assign);
			if (!boolean) return HttpNotFound("Case was not assigned");
			TempData["Message"] = $"Case has been assign";
			return RedirectToAction("DepartmentCases");
		}


		public ActionResult UnAssignLawyer(int id, int lawyerId)
		{
			var caseUnassigned = _caseService.AssignAction(id, lawyerId, CaseAction.UnAssign);

			if (!caseUnassigned) return HttpNotFound("case was not assigned");

			TempData["Message"] = $"Lawyer has been unassigned from the case";
			return RedirectToAction("DepartmentCases");
		}

		/// <summary>
		/// Called when the attorney general accepts a case
		/// </summary>
		/// <param name="id">The id of the case</param>
		/// <returns>The case details with case status of assigned</returns>
		public ActionResult AssignToDepartment(int id)
		{
            var caseDetails = _caseService.GetCaseDetails(id);
			try
            {
                _caseService.AuditCase(id, CaseAction.Accept);
                TempData["Message"] = $"Case has been assigned to {caseDetails.Department.DepartmentName} Department";

                //Notify user that this case has been accepted
                _caseService.NotifyClient(caseDetails);
                return View("CaseDetails", caseDetails);
            }
            catch
            {
                TempData["Message"] = $"Department does not have a department head";
				return View("CaseDetails", caseDetails);
            }
		}

		/// <summary>
		/// Called when the attorney general rejects a case
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The case details with case status of rejected</returns>
		public ActionResult RejectCase(int id)
		{
			_caseService.AuditCase(id, CaseAction.Reject);
			var vm = _caseService.GetCaseDetails(id);
			TempData["Message"] = $"Case has been rejected";

			//Notify user that this case has been rejected
			_caseService.NotifyClient(vm);
			return View("CaseDetails", vm);
		}

		public PartialViewResult SearchForm()
		{
			var vm = new SearchCasesViewModel();
			return PartialView("_SearchCase", vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SearchResult(SearchCasesViewModel model)
		{
			var category = model.Category;
			var query = model.SearchValue;
			return RedirectToAction("Search", new { category = category, query = query });
		}

		/// <summary>
		/// Returns search results for cases based on category and the query value
		/// </summary>
		/// <param name="category">Either case ID or client email</param>
		/// <param name="query">Query parameter</param>
		/// <returns></returns>
		public ActionResult Search(string category, string query)
		{
			var queryParam = $"category={category}&param={query}";
			var vm = _caseService.GetCases(QueryType.Search, queryParam);
			ViewBag.searchValue = query;
			ViewBag.searchParameter = category.ToLower();
			return View(vm);
		}

		public PartialViewResult GetComment(int caseId)
		{

			var vm = new CommentViewModel();

			return PartialView("_CaseComments");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveComment(CaseDetailsViewModel model)
		{
			if (!ModelState.IsValid)
				return RedirectToAction("CaseDetails");

			var comment = Mapper.Map<CommentViewModel, Comment>(model.Comment);
			comment.CreatedAt = DateTime.Now;
			_caseService.InsertComment(comment);
			model.Id = comment.Id;
			return PartialView("_CaseComments");
		}
		public ActionResult ProcessingCase(int id, Status status)
		{
			var boolean = _caseService.CaseStatusChange(id, status);
			if (!boolean) return HttpNotFound("Case was not assigned");
			var caseDetail = _caseService.GetCaseDetails(id);

			if (status == Status.Processing)
			{
				TempData["Message"] = $"You are currently processing this case";
				return RedirectToAction("CaseDetails", caseDetail);
			}
			if (status == Status.RequestClose)
			{
				TempData["Message"] = $"You request has been sent";
				return RedirectToAction("CaseDetails", caseDetail);
			}
			if (status == Status.Closed)
			{
				TempData["Message"] = $"You have closed case: {caseDetail.CaseID}";
				return RedirectToAction("RequestClose");
			}
			return RedirectToAction("CaseDetails", caseDetail);
		}

		public ActionResult RequestClose()
		{
			var cases = _caseService.GetCases(QueryType.Status, "Request to Close");

			return View(cases);

		}
	}
}