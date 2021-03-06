using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCAppWebIdent.Models;
using System.Diagnostics;

namespace MVCAppWebIdent.Controllers
{
    public class CabinetController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CabinetController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Policy = "User")]
        public IActionResult Index() => View();

        [Authorize(Policy = "Admin")]
        public IActionResult _AdminNavigateBar() => PartialView();

        [Authorize(Policy = "User")]
        public IActionResult _UserNavigateBar() => PartialView();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}