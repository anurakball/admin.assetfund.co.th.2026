using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class BannerGroupController : AdminCoreController
    {
        public BannerGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("BannerGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
