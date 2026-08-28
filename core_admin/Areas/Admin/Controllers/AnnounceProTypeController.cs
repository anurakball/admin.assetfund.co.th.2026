using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceProTypeController : AdminCoreController
    {
        public AnnounceProTypeController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceProType");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
