using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Views.ViewComponents
{
    public class AdminPreviewsViewComponent : ViewComponent
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly AdminHelpers _admin;
        public AdminPreviewsViewComponent(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
        }
        public async Task<IViewComponentResult> InvokeAsync(string previewsURL = "")
        {
            ViewBag.previewsURL = previewsURL;

            return View("~/Areas/Admin/Views/Shared/_PartialAdminPreviews.cshtml");
        }
    }
}
