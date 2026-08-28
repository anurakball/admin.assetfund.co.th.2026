using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSEOController : AdminCoreController
    {
        public HomeSEOController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSEO");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
