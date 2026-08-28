using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceProGroupController : AdminCoreController
    {
        public AnnounceProGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceProGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
