using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceJobSubmitController : AdminCoreController
    {
        public AnnounceJobSubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceJobSubmit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
