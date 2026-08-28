using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterPrivacy1Controller : AdminCoreController
    {
        public FooterPrivacy1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterPrivacy1");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
