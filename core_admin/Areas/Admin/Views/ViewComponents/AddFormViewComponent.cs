using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Views.ViewComponents
{
    public class AddFormViewComponent : ViewComponent
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly AdminHelpers _admin;
        public readonly DBHelper _db;
        public AddFormViewComponent(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
            _db = new DBHelper(hostingEnvironment, iConfig);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*
            _admin.SetSessionWebID();
            var All_Page = _db.ExecuteQuery(string.Format("select * from web_cms_page where web_id = @web_id "), new Dictionary<string, object>() { { "web_id", _admin._currentWebID } });
            ViewBag.All_Page = All_Page;
            */

            ViewBag._db = _db;

            return View("~/Areas/Admin/Views/Shared/_PartialAddForm.cshtml");
        }
    }
}
