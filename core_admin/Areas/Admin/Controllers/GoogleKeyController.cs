using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class GoogleKeyController : AdminCoreController
    {
        public GoogleKeyController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("GoogleKey");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
