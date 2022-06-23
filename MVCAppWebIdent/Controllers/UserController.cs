using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCAppWebIdent.Models;
using System.Diagnostics;

namespace MVCAppWebIdent.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

        [Authorize(Policy="User")]
        public IActionResult Index() => PartialView("Dashboard");

        [Authorize(Policy = "User")]
        public IActionResult Dashboard() => PartialView("Dashboard");

        [Authorize(Policy = "User")]
        public IActionResult Charts() => PartialView("Charts");

        [Authorize(Policy = "User")]
        public IActionResult Analytics() => PartialView("Analytics");

        [Authorize(Policy = "User")]
        public IActionResult Wallets() => PartialView("Wallets");

        [Authorize(Policy = "User")]
        public IActionResult Settings() => PartialView("Settings");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}