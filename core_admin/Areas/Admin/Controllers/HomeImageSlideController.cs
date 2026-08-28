using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeImageSlideController : AdminCoreController
    {
        public HomeImageSlideController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeImageSlide");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
