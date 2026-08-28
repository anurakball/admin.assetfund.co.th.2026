using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FooterLinkController : AdminCoreController
    {
        public FooterLinkController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FooterLink");

            Module = _admin.setModuleCMSPage(Module);
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
