using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DebtText6Controller : AdminCoreController
    {
        public DebtText6Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DebtText6");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
