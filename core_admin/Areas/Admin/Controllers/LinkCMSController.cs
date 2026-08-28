using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LinkCMSController : AdminCoreController
    {
        public LinkCMSController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LinkCMS");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
