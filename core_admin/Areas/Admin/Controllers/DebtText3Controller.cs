using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DebtText3Controller : AdminCoreController
    {
        public DebtText3Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DebtText3");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
