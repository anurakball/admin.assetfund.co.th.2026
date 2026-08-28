using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MicrositeSubmitController : AdminCoreController
    {
        public MicrositeSubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MicrositeSubmit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
