using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Views.ViewComponents
{
    public class ToolWidgetViewComponent : ViewComponent
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly AdminHelpers _admin;
        public readonly DBHelper _db;
        public ToolWidgetViewComponent(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
            _db = new DBHelper(hostingEnvironment, iConfig);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            _admin.SetSessionWebID();
            //----- เครื่องมือ Drag-Drop เป็นพาเลตต์ "ค่าคงที่" (เก็บที่ web_id=0) ใช้ร่วมกันทุก microsite
            //      เลือกชุดตารางจากสิทธิ์ที่ล็อกอินเท่านั้น: web_id = 0 -> widget_group/widget,
            //      web_id != 0 (microsite) -> widget_group2/widget2  (ตัดสินโดย _admin.WidgetGroupTable()/WidgetTable())
            //      ไม่กรอง web_id หรือเงื่อนไขอื่น เพราะพาเลตต์เป็นค่าคงที่ชุดเดียว (การกรอง web_id ทำให้ microsite เห็นว่าง)
            var All_Widget = _db.ExecuteQuery(
                string.Format("select * from {0} order by sort asc", Db.T(_admin.WidgetGroupTable())));
            ViewBag.All_Widget = All_Widget;
            ViewBag.WidgetTable = _admin.WidgetTable();
            ViewBag.WidgetWebID = _admin._currentWebID;
            ViewBag._db = _db;

            return View("~/Areas/Admin/Views/Shared/_PartialToolWidget.cshtml");
        }
    }
}
