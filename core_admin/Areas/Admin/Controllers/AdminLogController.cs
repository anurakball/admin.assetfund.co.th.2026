namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AdminLogController : AdminCoreController
    {
        public AdminLogController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AdminLog");
        }
    }
}
