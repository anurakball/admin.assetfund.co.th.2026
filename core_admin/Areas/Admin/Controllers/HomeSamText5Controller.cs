using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Models;

using thaicredit_hr_admin.Areas.Admin.Helpers;
namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class HomeSamText5Controller : AdminCoreController
    {
        public HomeSamText5Controller(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("HomeSamText5");
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

                var productGroupDT = new System.Data.DataTable();
                productGroupDT.Columns.Add("id");
                productGroupDT.Columns.Add("name");

                var mysqlConfig = _config.GetSection("MySQLConnection");
                string connStr = string.Format(
                    "Server={0};User ID={1};Password={2};Database={3}",
                    mysqlConfig["Server"], mysqlConfig["UserID"], mysqlConfig["Password"], mysqlConfig["Database"]
                );
                try
                {
                    using var conn = new MySqlConnection(connStr);
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT id, product_group FROM product_group ORDER BY id ASC";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        productGroupDT.Rows.Add(reader["id"].ToString(), reader["product_group"].ToString());
                    }
                }
                catch (Exception mysqlEx)
                {
                    ViewBag.productGroupError = mysqlEx.Message;
                }

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
                ViewBag.productGroups = productGroupDT;
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
    }
}
