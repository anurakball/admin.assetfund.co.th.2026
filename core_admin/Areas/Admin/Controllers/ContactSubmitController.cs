using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ContactSubmitController : AdminCoreController
    {
        public ContactSubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("ContactSubmit");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
