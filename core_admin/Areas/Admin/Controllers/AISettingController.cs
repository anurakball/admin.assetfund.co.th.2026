using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AISettingController : AdminCoreController
    {
        public AISettingController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AISetting");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
