using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutStructureController : AdminCoreController
    {
        public AboutStructureController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutStructure");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
