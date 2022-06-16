using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Roles()
        {
            return View(_userManager.Users);
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Edit(string id)
        {
            var a = 0;
            return Ok(a);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var a = 0;
            return Ok(a);
        }

    }
}
