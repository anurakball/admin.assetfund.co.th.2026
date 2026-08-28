using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NplRegisterController : AdminCoreController
    {
        public NplRegisterController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NplRegister");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
