using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DebtText1Controller : AdminCoreController
    {
        public DebtText1Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DebtText1");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
