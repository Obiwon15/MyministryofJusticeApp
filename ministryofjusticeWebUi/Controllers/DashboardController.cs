using System.Threading.Tasks;
using System.Web.Mvc;
using ministryofjusticeDomain.Enum;
using ministryofjusticeWebUi.Interfaces;

namespace ministryofjusticeWebUi.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICaseService _caseService;
        private readonly IUserServices _userServices;
        public DashboardController()
        {

        }
        public DashboardController(ICaseService caseService, IUserServices userServices)
        {
            _caseService = caseService;
            _userServices = userServices;
        }
        // GET: Dashboard/Index
        public async Task<ActionResult> Index()
        {
            var user = await _userServices.GetProfile(User.Identity.Name);
            ViewBag.fullname = user.FullName;
            var cases = _caseService.GetCases(QueryType.Status);
            if (User.IsInRole("Director of Department"))
            {
                cases = _caseService.GetCases(QueryType.Assigned);
            }
            if (User.IsInRole("Lawyer"))
                cases = _caseService.FilterLawyerCases(cases);
            return View(cases);
        }

        public PartialViewResult UserDetail()
        {
            var user = _userServices.GetCurrentProfile();
            ViewBag.fullname = user.FullName;
            return PartialView("_UserDropDown");
        }
    }
}