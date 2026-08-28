using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class JobCMSController : AdminCoreController
    {
        public JobCMSController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("JobCMS");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
