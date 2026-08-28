using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutText3Controller : AdminCoreController
    {
        public AboutText3Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutText3");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
