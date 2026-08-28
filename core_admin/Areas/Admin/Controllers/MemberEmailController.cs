using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MemberEmailController : AdminCoreController
    {
        public MemberEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MemberEmail");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
