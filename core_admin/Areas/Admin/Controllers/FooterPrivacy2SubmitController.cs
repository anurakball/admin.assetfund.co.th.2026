using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterPrivacy2SubmitController : AdminCoreController
    {
        public FooterPrivacy2SubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterPrivacy2Submit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
