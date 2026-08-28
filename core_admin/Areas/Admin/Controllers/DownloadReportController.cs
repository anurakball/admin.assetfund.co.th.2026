using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DownloadReportController : AdminCoreController
    {
        public DownloadReportController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DownloadReport");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
