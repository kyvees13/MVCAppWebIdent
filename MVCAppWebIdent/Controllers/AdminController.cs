﻿using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index() => PartialView("Index");

        [Authorize(Policy = "Admin")]
        public IActionResult _AdminNavigateBar() => PartialView("_AdminNavigateBar");

        [Authorize(Policy = "Admin")]
        public IActionResult GetPopUpEditModalPartialView() => PartialView("_PopupEditRoleModelPartial");

        [Authorize(Policy = "Admin")]
        public IActionResult GetPopUpDeleteModalPartialView() => PartialView("_PopupDeleteRoleModelPartial");

        [Authorize(Policy ="Admin")]
        [HttpPost]
        public JsonResult SetNewRole(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var result = _roleManager.CreateAsync(new IdentityRole(roleName));
                
                if (result.Result.Succeeded)
                {
                    return Json(new { Success = "True", Message = "Role was created" });
                }
                else
                {
                    return Json(new { Success = "False", Message = "Role was not created" });
                }
            }
            else
            {
                return Json(new { Success = "False", Message = "Role is exist" });
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public JsonResult GetAccountByID(string id)
        {
            var currentUser = _userManager.FindByIdAsync(id.ToString()).Result;

            if (currentUser == null)
            {
                return Json(new { Success = "False", Message = "User not found" });
            }

            return Json(new EditModel 
            { 
                Id = id, 

                Username = currentUser.UserName,

                Email = currentUser.Email, 
                EmailConfirmed = currentUser.EmailConfirmed,

                Phonenumber = currentUser.PhoneNumber,
                PhonenumberConfirmed = currentUser.PhoneNumberConfirmed,

                Password = currentUser.PasswordHash
            });;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult SaveProperties([FromBody] EditModel _EditModel)
        {

            var currentUser = _userManager.FindByIdAsync(_EditModel.Id).Result;

            if (currentUser == null)
            {
                return Json(new { Success = "False", Message = "User not found" });
            }

            currentUser.Email = _EditModel.Email;
            currentUser.PhoneNumber = _EditModel.Phonenumber;

            var result = _userManager.UpdateAsync(currentUser);

            if (result.Result.Succeeded)
            {
                return Json(new { Success = "True", Message = "Success changed data" });
            }

            return Json(new { Success = "False", Message = "Changing not succeeded" });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult DeleteAccountByID(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return Json(new { Success = "False", Message = "User not found" });
            }

            var removerolesResponse = _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result.Cast<string>());

            var deleteuserResponse = _userManager.DeleteAsync(user);

            if (deleteuserResponse.Result.Succeeded)
            {
                return Json(new
                {
                    Success = "True",
                    Message = "Success deleted"
                });
            }
            else if (deleteuserResponse.Result.Errors.Count() > 0)
            {
                return Json(new
                {
                    Success = "False",
                    Message = JsonSerializer.Serialize(deleteuserResponse.Result.Errors.ToList())
                });
            }

            return Json(new
            {
                Success = "False",
                Message = "Unexpected"
            });
        }

        [Authorize(Policy = "Admin")]
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
        public IActionResult Settings()
        {
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
    }
}
