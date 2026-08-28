using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard4catController : AdminCoreController
    {
        public AboutBoard4catController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard4cat");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
