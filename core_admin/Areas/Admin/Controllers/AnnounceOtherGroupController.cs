using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceOtherGroupController : AdminCoreController
    {
        public AnnounceOtherGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceOtherGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
