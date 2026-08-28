using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FacebookPixelController : AdminCoreController
    {
        public FacebookPixelController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FacebookPixel");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
