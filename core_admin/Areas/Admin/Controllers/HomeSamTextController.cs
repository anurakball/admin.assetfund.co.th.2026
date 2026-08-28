using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamTextController : AdminCoreController
    {
        public HomeSamTextController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
