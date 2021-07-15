using System.Web;
using System.Web.Mvc;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IDepartmentServices _departmentServices;

        public HomeController()
        {
        }

        public HomeController(IReportService reportService, IDepartmentServices departmentServices)
        {
            _reportService = reportService;
            _departmentServices = departmentServices;
        }
        public ActionResult Index()
        {
            if (User.IsInRole("System Administrator")
                || User.IsInRole("Attorney General")
                || User.IsInRole("Director of Department")
                || User.IsInRole("Lawyer"))
            {
                return RedirectToAction("Index", "Dashboard");
            }
           
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        
        public ActionResult SubmitReport()
        {
            var model = new CaseViewModel
            {
                Departments = _departmentServices.GetAllDepartments()
            };
            return View(model);
        }

        /// <summary>
        /// Submits a case report
        /// </summary>
        /// <param name="report">case view model</param>
        /// <param name="file">Evidence file</param>
        /// <returns>Success view</returns>s
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SubmitReport(CaseViewModel report, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
                var filePath = Server.MapPath("~/CaseFiles/");
                var submittedCase = _reportService.SubmitCase(report, file, filePath);
                if (submittedCase != null)
                {
                    return RedirectToAction("SuccessReport", new { caseId = submittedCase.CaseID });
                }

            }
			report.Departments = _departmentServices.GetAllDepartments();
            ModelState.AddModelError("", "An error occured!");
			return View();
		}


        public ActionResult SuccessReport(string caseId = null)
        {
            if (caseId == null)
            {
                return RedirectToAction("CannotAccess");
            }
            var report = _reportService.FindCase(caseId);
            if (report != null)
                return View(report);
            return RedirectToAction("CannotAccess");
        }

        public ActionResult CannotAccess()
        {
            return View();
        }

        public ActionResult TrackReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrackReport(string caseId)
        {
            if (caseId != null)
            {
                var displayCase = _reportService.FindCase(caseId);
                if (displayCase != null)
                {
                    return PartialView("_CasePartial", displayCase);
                }
                ViewBag.Message = "Case ID cannot be found";
                return PartialView("_CaseNotFound");
            }

            ViewBag.Message = "You have to enter a case id!";
            return PartialView("_CaseNotFound");
        }
    }
	
}
