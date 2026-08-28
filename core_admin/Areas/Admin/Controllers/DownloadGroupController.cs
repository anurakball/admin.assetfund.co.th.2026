using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DownloadGroupController : AdminCoreController
    {
        public DownloadGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DownloadGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
