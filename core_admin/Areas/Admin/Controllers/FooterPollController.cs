using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterPollController : AdminCoreController
    {
        public FooterPollController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterPoll");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
