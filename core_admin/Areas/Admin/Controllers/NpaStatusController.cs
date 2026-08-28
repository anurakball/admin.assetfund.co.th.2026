using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using thaicredit_hr_admin.Areas.Admin.Filters;

using thaicredit_hr_admin.Areas.Admin.Helpers;
namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class NpaStatusController : AdminCoreController
    {
        public NpaStatusController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("NpaStatus");
        }

        [ModuleCheck("add")]
        public override IActionResult Create()
        {
            var npaLocateList = _db.ExecuteQuery(
                "SELECT id, title FROM [2026_web_core_group] WHERE module_id = '18' ORDER BY title ASC"
            );
            ViewBag.NpaLocateList = npaLocateList;

            var npaFacilityList = _db.ExecuteQuery(
                "SELECT id, title FROM [2026_web_core_group] WHERE module_id = '19' ORDER BY title ASC"
            );
            ViewBag.NpaFacilityList = npaFacilityList;

            return base.Create();
        }

        [HttpPost]
        [ModuleCheck("add")]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                var insertFields = _admin.setFieldsCreate(Module, collection, true);

                foreach (var intField in new[] { "highlight_status", "suggest_status" })
                {
                    string val = collection.ContainsKey(intField) ? collection[intField].ToString() : "0";
                    insertFields[intField] = int.TryParse(val, out int parsed) ? parsed : 0;
                }

                var affected = _db.Insert(Module.Config.Table, insertFields);

                if (affected != 0)
                {
                    TempData["alert_message"] = string.Format("เพิ่มข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถเพิ่มข้อมูลได้";
                    return Redirect("Create");
                }
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new thaicredit_hr_admin.Areas.Admin.Models.ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
        }

        [ModuleCheck("edit")]
        public override IActionResult Edit(int id)
        {
            var npaLocateList = _db.ExecuteQuery(
                "SELECT id, title FROM [2026_web_core_group] WHERE module_id = '18' ORDER BY title ASC"
            );
            ViewBag.NpaLocateList = npaLocateList;

            var npaFacilityList = _db.ExecuteQuery(
                "SELECT id, title FROM [2026_web_core_group] WHERE module_id = '19' ORDER BY title ASC"
            );
            ViewBag.NpaFacilityList = npaFacilityList;

            return base.Edit(id);
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
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

                var updateFields = _admin.setFieldsUpdate(Module, collection, true);

                foreach (var intField in new[] { "highlight_status", "suggest_status" })
                {
                    string val = collection.ContainsKey(intField) ? collection[intField].ToString() : "0";
                    updateFields[intField] = int.TryParse(val, out int parsed) ? parsed : 0;
                }

                var affected = _db.Update(Module.Config.Table, "WHERE id = @id and web_id = @web_id", updateFields,
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });

                if (affected != 0)
                {
                    TempData["alert_message"] = string.Format("แก้ไขข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถแก้ไขข้อมูลได้";
                    return RedirectToActionPermanent("Edit", new { id });
                }
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new thaicredit_hr_admin.Areas.Admin.Models.ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
        }

        [HttpGet]
        public IActionResult SearchProduct(string q)
        {
            if (string.IsNullOrEmpty(q) || q.Length < 3)
                return Json(new List<object>());

            var mysqlConfig = _config.GetSection("MySQLConnection");
            string connStr = string.Format(
                "Server={0};User ID={1};Password={2};Database={3}",
                mysqlConfig["Server"], mysqlConfig["UserID"], mysqlConfig["Password"], mysqlConfig["Database"]
            );

            var results = new List<object>();
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT top 50 id, product_code, project, product_desc_th
                    FROM tb_product2
                    WHERE product_code LIKE @q OR project LIKE @q";
                cmd.Parameters.AddWithValue("@q", "%" + q + "%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string text = reader["product_code"] + " : " + reader["project"];
                    results.Add(new { id = reader["id"], text });
                }
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }

            return Json(results);
        }
    }
}
