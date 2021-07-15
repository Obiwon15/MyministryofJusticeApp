using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserServices _userServices;
      
        public ProfileController(IUserServices userServices) 
        {
            _userServices = userServices;
        }

        public ProfileController()
        {
        }

        // GET: Profile
        public async Task<ActionResult> UpdateProfile()
        {
            var vm = await _userServices.GetProfile(User.Identity.Name);
            return View(vm);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userServices.UpdateProfile(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            result.Errors.ForEach(error => ModelState.AddModelError("", error));
            return View(model);
        }
    }
}