using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LinkGroupController : AdminCoreController
    {
        public LinkGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LinkGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
