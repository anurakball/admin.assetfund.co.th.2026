using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AgentEmailController : AdminCoreController
    {
        public AgentEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AgentEmail");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
