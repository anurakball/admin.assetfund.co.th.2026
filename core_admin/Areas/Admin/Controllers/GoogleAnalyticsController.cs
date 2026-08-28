using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class GoogleAnalyticsController : AdminCoreController
    {
        public GoogleAnalyticsController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("GoogleAnalytics");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
