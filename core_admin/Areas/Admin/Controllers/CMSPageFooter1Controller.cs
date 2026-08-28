using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class CMSPageFooter1Controller : AdminCoreController
    {
        public CMSPageFooter1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("CMSPageFooter1");

            Module = _admin.setModuleCMSPage(Module);
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
