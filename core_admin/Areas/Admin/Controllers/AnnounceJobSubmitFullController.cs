using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AnnounceJobSubmitFullController : AdminCoreController
    {
        public AnnounceJobSubmitFullController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AnnounceJobSubmitFull");
        }
    }
}
