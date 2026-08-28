using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NplWebController : AdminCoreController
    {
        public NplWebController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NplWeb");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
