using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class DebtText2Controller : AdminCoreController
    {
        public DebtText2Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("DebtText2");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
