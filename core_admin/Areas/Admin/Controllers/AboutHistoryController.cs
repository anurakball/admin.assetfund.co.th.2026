using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutHistoryController : AdminCoreController
    {
        public AboutHistoryController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutHistory");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
