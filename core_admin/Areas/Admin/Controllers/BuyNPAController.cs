using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class BuyNPAController : AdminCoreController
    {
        public BuyNPAController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("BuyNPA");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
