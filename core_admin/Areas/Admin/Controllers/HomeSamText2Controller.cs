using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamText2Controller : AdminCoreController
    {
        public HomeSamText2Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText2");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
