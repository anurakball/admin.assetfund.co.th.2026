namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class Widget2Controller : AdminCoreController
    {
        public Widget2Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Widget2");
        }
    }
}
