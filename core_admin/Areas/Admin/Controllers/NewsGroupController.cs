using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NewsGroupController : AdminCoreController
    {
        public NewsGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NewsGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
