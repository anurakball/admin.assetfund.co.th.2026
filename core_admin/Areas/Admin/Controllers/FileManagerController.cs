using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class FileManagerController : AdminCoreController
    {
        public FileManagerController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("FileManager");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
