using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class VDOGroupsubController : AdminCoreController
    {
        public VDOGroupsubController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("VDOGroupsub");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
