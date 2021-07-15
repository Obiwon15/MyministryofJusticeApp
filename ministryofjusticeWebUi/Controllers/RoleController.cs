using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private readonly IUserUnitOfWork _unitOfWork;

        public RoleController()
        {
        }

        public RoleController(IUserUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Role
        public ActionResult ManageRoles()
        {
            var roles = _unitOfWork.RoleService.GetRoles();
            return View(roles);
        }

        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.RoleService.CreateRoleAsync(model.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("ManageRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
        public async Task<ActionResult> EditRole(string id)
        {
            var roleDb = await _unitOfWork.RoleService.FindRoleByIdAsync(id);
            var role = Mapper.Map<IdentityRole, RoleViewModel>(roleDb);
            return View("EditRole", role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.RoleService.UpdateRoleAsync(model.Id, model.Name);
                if (result.Succeeded)
                {
                    // viewbag success message
                    return RedirectToAction("ManageRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
    }
}