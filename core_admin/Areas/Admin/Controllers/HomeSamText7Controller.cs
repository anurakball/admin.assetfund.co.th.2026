using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamText7Controller : AdminCoreController
    {
        public HomeSamText7Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText7");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
