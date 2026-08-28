using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LinkController : AdminCoreController
    {
        public LinkController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Link");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
