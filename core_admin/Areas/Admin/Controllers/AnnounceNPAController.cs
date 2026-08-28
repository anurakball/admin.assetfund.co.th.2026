using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceNPAController : AdminCoreController
    {
        public AnnounceNPAController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceNPA");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
