using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamText3Controller : AdminCoreController
    {
        public HomeSamText3Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText3");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
