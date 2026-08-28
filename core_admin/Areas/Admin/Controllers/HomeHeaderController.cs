using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeHeaderController : AdminCoreController
    {
        public HomeHeaderController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeHeader");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
