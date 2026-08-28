namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class WidgetController : AdminCoreController
    {
        public WidgetController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Widget");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
