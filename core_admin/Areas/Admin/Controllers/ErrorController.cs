using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using thaicredit_hr_admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Error/Index.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
