using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class BannerController : AdminCoreController
    {
        public BannerController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Banner");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
