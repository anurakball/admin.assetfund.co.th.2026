using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DownloadController : AdminCoreController
    {
        public DownloadController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Download");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
