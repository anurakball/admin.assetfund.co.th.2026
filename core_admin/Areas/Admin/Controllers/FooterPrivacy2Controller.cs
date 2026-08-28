using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterPrivacy2Controller : AdminCoreController
    {
        public FooterPrivacy2Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterPrivacy2");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
