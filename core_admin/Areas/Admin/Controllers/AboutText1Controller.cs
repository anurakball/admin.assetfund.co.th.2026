using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutText1Controller : AdminCoreController
    {
        public AboutText1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutText1");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
