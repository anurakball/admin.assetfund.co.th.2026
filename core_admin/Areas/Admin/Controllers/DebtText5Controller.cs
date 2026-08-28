using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DebtText5Controller : AdminCoreController
    {
        public DebtText5Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DebtText5");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
