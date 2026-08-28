using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomePopUpController : AdminCoreController
    {
        public HomePopUpController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomePopUp");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
