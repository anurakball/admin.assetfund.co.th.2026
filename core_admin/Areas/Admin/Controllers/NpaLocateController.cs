using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using thaicredit_hr_admin.Areas.Admin.Filters;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NpaLocateController : AdminCoreController
    {
        public NpaLocateController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NpaLocate");
        } 
    }
}
