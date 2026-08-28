using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard2Controller : AdminCoreController
    {
        public AboutBoard2Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard2");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
