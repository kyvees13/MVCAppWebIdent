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
        public IActionResult Index() => View();

        [Authorize(Policy = "User")]
        public IActionResult Dashboard() => View();

        [Authorize(Policy = "User")]
        public IActionResult Charts() => View();

        [Authorize(Policy = "User")]
        public IActionResult Analytics() => View();

        [Authorize(Policy = "User")]
        public IActionResult Wallets() => View();

        [Authorize(Policy = "User")]
        public IActionResult Settings() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}