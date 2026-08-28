using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceProController : AdminCoreController
    {
        public AnnounceProController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnouncePro");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
