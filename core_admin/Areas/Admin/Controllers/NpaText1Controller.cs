using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NpaText1Controller : AdminCoreController
    {
        public NpaText1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NpaText1");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
