using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ArticleController : AdminCoreController
    {
        public ArticleController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Article");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
