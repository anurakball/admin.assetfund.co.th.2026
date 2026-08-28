using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AboutBoard3catController : AdminCoreController
    {
        public AboutBoard3catController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AboutBoard3cat");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
