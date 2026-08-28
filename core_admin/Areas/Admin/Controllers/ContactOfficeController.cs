using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ContactOfficeController : AdminCoreController
    {
        public ContactOfficeController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("ContactOffice");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
