using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FAQGroupController : AdminCoreController
    {
        public FAQGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FAQGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
