using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FAQController : AdminCoreController
    {
        public FAQController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FAQ");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
