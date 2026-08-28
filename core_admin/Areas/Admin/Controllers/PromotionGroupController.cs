using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class PromotionGroupController : AdminCoreController
    {
        public PromotionGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("PromotionGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
