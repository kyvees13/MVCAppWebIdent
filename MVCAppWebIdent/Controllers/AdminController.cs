using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAppWebIdent.Models;
using System.Text.Json;

namespace MVCAppWebIdent.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public JsonResult GetAccountByID(string id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            return Json(new EditModel { Email = user.Email, Password = user.PasswordHash });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult SaveProperties(EditModel _EditModel)
        {

            var currentUser = _userManager.FindByIdAsync(_EditModel.Id).Result;

            if (currentUser == null)
            {
                return Json(new { Success = "False", Message = "User not found" });
            }

            currentUser.Email = _EditModel.Email;

            var _json_serialize = JsonSerializer.Serialize(_EditModel.GetType().GetProperties().ToList());

            return Json(new { Success = "True", Message = "Success changed data", Changed = _json_serialize });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult DeleteAccountByID(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;

            if (user != null)
            {
                var resp1 = _userManager.RemoveFromRolesAsync(
                    user: user, roles: _userManager.GetRolesAsync(user).Result.Cast<string>());

                var resp = _userManager.DeleteAsync(user);

                if (resp.Result.Succeeded)
                {                    
                    return Json(new 
                    { 
                        Success = "True", 
                        Message = "Success deleted" 
                    });
                }
                else if (resp.Result.Errors.Count() > 0)
                {
                    return Json(new
                    {
                        Success = "False",
                        Message = JsonSerializer.Serialize(resp.Result.Errors.ToList())
                    });
                }

                return Json(new
                {
                    Success = "False",
                    Message = "Unexpected"
                });


            }

            return Json(new
            {
                Success = "False",
                Message = "User not found"
            });

        }

        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Roles()
        {
            List<SelectListItem> _select = new List<SelectListItem>();

            var roles = _roleManager.Roles;

            if (roles != null)
            {
                foreach (IdentityRole role in roles)
                    _select.Add(new SelectListItem { Text = role.Name });
            }

            ViewBag.select = _select;

            return View();
        }

        [Authorize(Policy = "Admin")]
        public IActionResult GetTablePartialView(string roleName)
        {
            bool isExistRole = _roleManager.RoleExistsAsync(roleName).Result;

            if (isExistRole)
            {
                var _specific_users = _userManager.GetUsersInRoleAsync(roleName).Result;
                return PartialView(_specific_users);
            }

            return PartialView(new List<IdentityUser>());
        }

        [Authorize(Policy = "Admin")]
        public IActionResult GetPopUpEditModalPartialView()
        {
            return PartialView("_PopupEditRoleModelPartial");
        }

        [Authorize(Policy = "Admin")]
        public IActionResult GetPopUpDeleteModalPartialView()
        {
            return PartialView("_PopupDeleteRoleModelPartial");
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public JsonResult LoadRoles()
        {
            List<SelectListItem> _select = new List<SelectListItem>();

            var roles = _roleManager.Roles;

            if (roles != null)
            {
                foreach (IdentityRole role in roles) 
                    _select.Add(new SelectListItem { Text = role.Name });
            }

            return Json(_select.ToList());
        }
    }
}
