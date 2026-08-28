using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutAnnualController : AdminCoreController
    {
        public AboutAnnualController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutAnnual");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
