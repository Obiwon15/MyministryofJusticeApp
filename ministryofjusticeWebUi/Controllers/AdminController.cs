using System.Threading.Tasks;
using System.Web.Mvc;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
    [Authorize(Roles = "System Administrator")]
    public class AdminController : Controller
    {
        private readonly IUserServices _userServices;

        public AdminController()
        {
        }

        public AdminController(IUserServices userServices) => _userServices = userServices;

            // GET: Admin
        public ActionResult ManageAccounts()
        {
            var vm = _userServices.FetchDashboardData();
            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateAccount()
        {
            var vm = _userServices.GetAccountViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.CreateAccount(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("ManageAccounts");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            model.Department = _userServices.GetDepartments();
            model.Roles = _userServices.GetRoles();
            ModelState.AddModelError("", "Error creating account");
            return View(model);
        }
    }
}