using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutVisionController : AdminCoreController
    {
        public AboutVisionController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutVision");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
