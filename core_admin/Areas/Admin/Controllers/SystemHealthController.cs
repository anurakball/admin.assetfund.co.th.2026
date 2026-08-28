using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class SystemHealthController : AdminCoreController
    {
        public SystemHealthController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("SystemHealth");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
