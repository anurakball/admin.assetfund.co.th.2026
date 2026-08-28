using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Models;

using thaicredit_hr_admin.Areas.Admin.Helpers;
namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamText6Controller : AdminCoreController
    {
        public HomeSamText6Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText6");
        }

        [ModuleCheck("edit")]
        public override IActionResult Edit(int id)
        {
            Module = _admin.setBreadcrumbCMSPage(Module);
            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/แก้ไข";

                var itemEdit = _db.ExecuteQuery(
                    string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)),
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } }
                );
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                var groupDT = _db.ExecuteQuery(
                    "SELECT id, title FROM [2026_web_core_group] WHERE module_id = @module_id ORDER BY title ASC",
                    new Dictionary<string, object>() { { "module_id", 18 } }
                );

                var facilityDT = _db.ExecuteQuery(
                    "SELECT id, title FROM [2026_web_core_group] WHERE module_id = @module_id ORDER BY title ASC",
                    new Dictionary<string, object>() { { "module_id", 19 } }
                );

                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.itemEdit = itemEdit.Rows[0];
                // ค่านี้ถูกฝังลงใน <script> ของ View (var jsonRow = @Html.Raw(...)) → ต้อง escape HTML กัน stored XSS
                ViewBag.itemJSONEdit = _db.DataTableToScriptJSON(itemEdit);
                ViewBag.groupDT = groupDT;
                ViewBag.facilityDT = facilityDT;
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }

            return View("~/Areas/Admin/Views/" + Module.Config.UseViewEditFrom + "/Edit.cshtml");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/
    }
}
