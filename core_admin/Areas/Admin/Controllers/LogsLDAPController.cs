using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsLDAPController : AdminCoreController
    {
        public LogsLDAPController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsLDAP");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
