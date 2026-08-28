using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AdminAccessController : AdminCoreController
    {
        public AdminAccessController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AdminAccess");
        }

/*public override IActionResult Index()
{
return View();
}
//test
*/

        [HttpPost]
        [ModuleCheck("add")]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                var insertFields = _admin.setFieldsCreate(Module, collection, false);
                //  return Json(insertFields);
                var affected = _db.Insert(Module.Config.Table, insertFields);

                if (affected != 0)
                {
                    int access_id = 0;
                    var lastInsert = _db.ExecuteQuery(string.Format("SELECT MAX(id) as last_id from {0} WHERE web_id = @web_id GROUP BY id ORDER BY id desc LIMIT 1", Module.Config.Table), new() { { "web_id", _currentWebID } });

                    if (lastInsert.Rows.Count > 0 && !string.IsNullOrEmpty(lastInsert.Rows[0]["last_id"].ToString()) && _utility.isInt(lastInsert.Rows[0]["last_id"] + ""))
                    {
                        access_id = Convert.ToInt32(lastInsert.Rows[0]["last_id"].ToString());
                    }

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "add",
                        action_info: string.Format("เพิ่มข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, access_id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(insertFields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                        );
                    #endregion

                    #region Admin Module
                    var adminMenu = _admin.AdminMenu();
                    var allMenu = new List<AdminMenuModel>();
                    if (adminMenu != null && access_id != 0)
                    {
                        foreach (var item in adminMenu)
                        {
                            if (item.SubMenu != null && item.SubMenu.Count > 0)
                            {
                                foreach (var item2 in item.SubMenu)
                                {
                                    allMenu.Add(item2);
                                }
                            }
                            else
                            {
                                allMenu.Add(item);
                            }
                        }
                    }

                    foreach (var item in allMenu)
                    {
                        if (item.ModuleName != null && collection.ContainsKey(item.ModuleName))
                        {
                            var para = new Dictionary<string, object>();
                            para.Add("web_id", _currentWebID);
                            para.Add("updated_at", System.DateTime.Now);
                            para.Add("updated_by", _session.GetString("admin_user") ?? "");
                            para.Add("access_id", access_id);
                            para.Add("mod_name", item.ModuleName);

                            var postName = new List<string>() {
                                item.ModuleName + "_can_add",
                                item.ModuleName + "_can_edit",
                                item.ModuleName + "_can_delete",
                                item.ModuleName + "_can_move",
                                item.ModuleName + "_can_status",
                                item.ModuleName + "_can_export",
                                item.ModuleName + "_can_approve",
                            };

                            foreach (string name in postName)
                            {
                                if (collection.ContainsKey(name) && !string.IsNullOrEmpty(collection[name]) && collection[name].ToString() == "1")
                                {
                                    para.Add(name.Replace(item.ModuleName + "_", ""), 1);
                                }
                                else
                                {
                                    para.Add(name.Replace(item.ModuleName + "_", ""), 0);
                                }
                            }

                            var selectAccess = _db.ExecuteQuery("select id from web_admin_module where access_id = @access_id and mod_name = @mod_name and web_id = @web_id limit 1", new() { { "access_id", access_id }, { "mod_name", item.ModuleName }, { "web_id", _currentWebID } });
                            if (selectAccess.Rows.Count > 0)
                            {
                                _db.Update("web_admin_module", "WHERE id = @id", para, new() { { "id", Convert.ToInt32(selectAccess.Rows[0]["id"]) } });
                            }
                            else
                            {
                                para.Add("created_at", System.DateTime.Now);
                                para.Add("created_by", _session.GetString("admin_user") ?? "");
                                _db.Insert("web_admin_module", para);
                            }
                        }
                    }
                    #endregion

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
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                #region Select Item Edit #########
                var itemEdit = _db.ExecuteQuery(string.Format("select * from {0} where id = @id and web_id = @web_id limit 1", Module.Config.Table), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                else
                {
                    //----- check locked row (waiting for approve) -----//
                    if (Module.Config.CanApprove == true && !string.IsNullOrEmpty(itemEdit.Rows[0]["pb_status"].ToString()) && itemEdit.Rows[0]["pb_status"] + "" == "0")
                    {
                        TempData["alert_message"] = "รายการที่รออนุมัติ ไม่สามารถแก้ไขได้";
                        TempData["alert_class"] = "alert-warning";
                        return RedirectToActionPermanent("Edit", new { id = id });
                    }
                }
                #endregion

                var updateFields = _admin.setFieldsUpdate(Module, collection, false);
                //return Json(updateFields);
                var affected = _db.Update(Module.Config.Table, "WHERE id = @id", updateFields, new Dictionary<string, object>() { { "id", id } });

                if (affected != 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "edit",
                        action_info: string.Format("แก้ไขข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: _db.DataTableToJSONWithJSONNet(itemEdit).TrimStart('[').TrimEnd(']'),
                        new_value: JsonConvert.SerializeObject(updateFields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    int access_id = id;

                    #region Admin Module
                    var adminMenu = _admin.AdminMenu();
                    var allMenu = new List<AdminMenuModel>();
                    if (adminMenu != null && access_id != 0)
                    {
                        foreach (var item in adminMenu)
                        {
                            if (item.SubMenu != null && item.SubMenu.Count > 0)
                            {
                                foreach (var item2 in item.SubMenu)
                                {
                                    allMenu.Add(item2);
                                }
                            }
                            else
                            {
                                allMenu.Add(item);
                            }
                        }
                    }

                    foreach (var item in allMenu)
                    {
                        if (item.ModuleName != null && collection.ContainsKey(item.ModuleName))
                        {
                            var para = new Dictionary<string, object>();
                            para.Add("web_id", _currentWebID);
                            para.Add("updated_at", System.DateTime.Now);
                            para.Add("updated_by", _session.GetString("admin_user") ?? "");
                            para.Add("access_id", access_id);
                            para.Add("mod_name", item.ModuleName);

                            var postName = new List<string>() {
                                item.ModuleName + "_can_add",
                                item.ModuleName + "_can_edit",
                                item.ModuleName + "_can_delete",
                                item.ModuleName + "_can_move",
                                item.ModuleName + "_can_status",
                                item.ModuleName + "_can_export",
                                item.ModuleName + "_can_approve",
                            };

                            foreach (string name in postName)
                            {
                                if (collection.ContainsKey(name) && !string.IsNullOrEmpty(collection[name]) && collection[name].ToString() == "1")
                                {
                                    para.Add(name.Replace(item.ModuleName + "_", ""), 1);
                                }
                                else
                                {
                                    para.Add(name.Replace(item.ModuleName + "_", ""), 0);
                                }
                            }

                            var selectAccess = _db.ExecuteQuery("select id from web_admin_module where access_id = @access_id and mod_name = @mod_name and web_id = @web_id limit 1", new() { { "access_id", access_id }, { "mod_name", item.ModuleName }, { "web_id", _currentWebID } });
                            if (selectAccess.Rows.Count > 0)
                            {
                                _db.Update("web_admin_module", "WHERE id = @id", para, new() { { "id", Convert.ToInt32(selectAccess.Rows[0]["id"]) } });
                            }
                            else
                            {
                                para.Add("created_at", System.DateTime.Now);
                                para.Add("created_by", _session.GetString("admin_user") ?? "");
                                _db.Insert("web_admin_module", para);
                            }
                        }
                        else if (item.ModuleName != null && !collection.ContainsKey(item.ModuleName))
                        {
                            _db.ExecuteNonQuery("DELETE FROM web_admin_module where access_id = @access_id and mod_name = @mod_name and web_id = @web_id ", new() { { "access_id", access_id }, { "mod_name", item.ModuleName }, { "web_id", _currentWebID } });
                        }
                    }
                    #endregion

                    TempData["alert_message"] = string.Format("แก้ไขข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถแก้ไขข้อมูลได้";
                    return RedirectToActionPermanent("Edit", new { id = id });
                }
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
        }
    }
}
