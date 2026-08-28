using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class GoogleAnalyticsLogsController : AdminCoreController
    {
        public GoogleAnalyticsLogsController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("GoogleAnalyticsLogs");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
