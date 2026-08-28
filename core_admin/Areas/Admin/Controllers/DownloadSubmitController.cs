using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DownloadSubmitController : AdminCoreController
    {
        public DownloadSubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DownloadSubmit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
