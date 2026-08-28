using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Views.ViewComponents
{
    public class AdminBreadcrumb2ViewComponent : ViewComponent
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly AdminHelpers _admin;
        public AdminBreadcrumb2ViewComponent(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
        }
        public async Task<IViewComponentResult> InvokeAsync(Module Module = null, List<string> Breadcrumb = null)
        {
            List<string> breadcrumb = new() { };
             
            if (Module != null)
            {
                breadcrumb = Module.Config.TextBreadcrumb.Split('/').ToList();
            }
            else if (Breadcrumb != null)
            {
                breadcrumb = Breadcrumb;
            }
            ViewBag.Breadcrumb = breadcrumb;
            return View("~/Areas/Admin/Views/Shared/_PartialAdminBreadcrumb2.cshtml");
        }
    }
}
