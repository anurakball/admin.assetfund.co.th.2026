using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MeetNPAEmailController : AdminCoreController
    {
        public MeetNPAEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MeetNPAEmail");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
