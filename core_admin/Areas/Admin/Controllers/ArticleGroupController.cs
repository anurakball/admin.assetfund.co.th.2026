using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ArticleGroupController : AdminCoreController
    {
        public ArticleGroupController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("ArticleGroup");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
