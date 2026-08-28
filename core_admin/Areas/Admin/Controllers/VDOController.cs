using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class VDOController : AdminCoreController
    {
        public VDOController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("VDO");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
