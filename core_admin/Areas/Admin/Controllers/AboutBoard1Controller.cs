using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard1Controller : AdminCoreController
    {
        public AboutBoard1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard1");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
