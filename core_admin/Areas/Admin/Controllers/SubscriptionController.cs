using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class SubscriptionController : AdminCoreController
    {
        public SubscriptionController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Subscription");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
