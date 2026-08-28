using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class VDOGroupController : AdminCoreController
    {
        public VDOGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("VDOGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
