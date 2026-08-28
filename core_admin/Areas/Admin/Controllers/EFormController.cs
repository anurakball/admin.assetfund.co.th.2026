using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class EFormController : AdminCoreController
    {
        public EFormController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("EForm");

            Module = _admin.setModuleCMSPage(Module);
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
