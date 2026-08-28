using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard3Controller : AdminCoreController
    {
        public AboutBoard3Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard3");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
