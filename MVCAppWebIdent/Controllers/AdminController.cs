using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCAppWebIdent.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
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
            return View();
        }

    }
}
