using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class BuyNPAEmailController : AdminCoreController
    {
        public BuyNPAEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("BuyNPAEmail");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
