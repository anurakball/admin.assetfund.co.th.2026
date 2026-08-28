using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterPrivacy1SubmitController : AdminCoreController
    {
        public FooterPrivacy1SubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterPrivacy1Submit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
