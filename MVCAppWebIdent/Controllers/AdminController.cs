using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCAppWebIdent.Controllers
{
    public class AdminController : Controller
    {
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
