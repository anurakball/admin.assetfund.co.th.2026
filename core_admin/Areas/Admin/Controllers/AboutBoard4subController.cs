using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard4subController : AdminCoreController
    {
        public AboutBoard4subController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard4sub");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
