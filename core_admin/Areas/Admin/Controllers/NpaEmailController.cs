using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NpaEmailController : AdminCoreController
    {
        public NpaEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NpaEmail");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
