using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Models;

using thaicredit_hr_admin.Areas.Admin.Helpers;
namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AdminUserController : AdminCoreController
    {
        public AdminUserController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AdminUser");
        }

        [HttpPost]
        [ModuleCheck("add")]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                #region Check requried field
                var f = collection;

                if (string.IsNullOrWhiteSpace(f["name"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ ชื่อ";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["surname"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ สกุล";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["email"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ อีเมล";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["username"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ Username";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["password"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ Password";
                    return Redirect("Create");
                }
                #endregion

                #region Check username
                var checkUser = _db.ExecuteQuery("select top 1 id from [2026_web_admin] where TRIM(username) = TRIM(@username) and web_id = @web_id", new() { { "username", collection["username"].ToString() }, { "web_id", _currentWebID } });
                if (checkUser.Rows.Count > 0)
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = string.Format("Username <strong>{0}</strong> ไม่สามารถใช้งานได้", collection["username"].ToString());
                    return Redirect("Create");
                }
                #endregion

                #region Set Cate session 
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                var insertFields = _admin.setFieldsCreate(Module, collection, false);

                //--- custom value ----//
                if (insertFields != null)
                {
                    if (insertFields.ContainsKey("password") && !string.IsNullOrEmpty(insertFields["password"].ToString()))
                    {
                        insertFields["password"] = _utility.GenerateSHA512String(insertFields["password"].ToString());
                        insertFields.Add("last_change_password_at", System.DateTime.Now);

                        #region ############### ตรวจสอบการตั้งรหัสผ่านเดิม ##############
                        var Config_Password = _config.GetSection("ConfigPassword");
                        string Config_Password_Times = Config_Password.GetSection("LastPassword").Value.ToString();
                        var SQL_Old_Password_Last = _db.ExecuteQuery("SELECT TOP (" + Config_Password_Times.ToString() + ") password FROM [2026_web_admin_password_log] WHERE admin_user=@admin_user and web_id = @web_id ORDER BY created_at DESC", new Dictionary<string, object>() { { "admin_user", (insertFields["username"] + "").ToString() }, { "web_id", _currentWebID } });
                        if (SQL_Old_Password_Last.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow r_Old_Password_Last in SQL_Old_Password_Last.Rows)
                            {
                                if ((r_Old_Password_Last["password"] + "").ToString().Trim() == (insertFields["password"] + "").ToString().Trim())
                                {
                                    TempData["alert_message"] = "กรุณาตั้งรหัสผ่านใหม่ รหัสผ่านนี้ถูกใช้ไปก่อนหน้านี้แล้ว";
                                    return Redirect(string.Format("/Admin/{0}/Create", Module.Name));
                                }
                            }
                        }
                        #endregion
                    }
                    if (insertFields.ContainsKey("access_id"))
                    {
                        insertFields["access_id"] = Convert.ToInt32(insertFields["access_id"]);
                    }
                    if (insertFields.ContainsKey("use_otp"))
                    {
                        insertFields["use_otp"] = Convert.ToInt32(insertFields["use_otp"]);
                    }
                }

                var affected = _db.Insert(Module.Config.Table, insertFields);

                if (affected != 0)
                {
                    int access_id = 0;
                    var lastInsert = _db.ExecuteQuery(string.Format("SELECT top 1 MAX(id) as last_id from {0} WHERE web_id = @web_id GROUP BY id ORDER BY id desc", Db.T(Module.Config.Table)), new() { { "web_id", _currentWebID } });

                    if (lastInsert.Rows.Count > 0 && !string.IsNullOrEmpty(lastInsert.Rows[0]["last_id"].ToString()) && _utility.isInt(lastInsert.Rows[0]["last_id"] + ""))
                    {
                        access_id = Convert.ToInt32(lastInsert.Rows[0]["last_id"].ToString());
                    }

                    #region ############### Log Password Admin ##############
                    if (insertFields.ContainsKey("password") && !string.IsNullOrEmpty(insertFields["password"].ToString()))
                    {
                        var Param_Log_PassWord_Admin = new Dictionary<string, object>();
                        Param_Log_PassWord_Admin.Add("created_at", System.DateTime.Now);
                        Param_Log_PassWord_Admin.Add("updated_at", System.DateTime.Now);
                        Param_Log_PassWord_Admin.Add("created_by", _session.GetString("admin_name") ?? "");
                        Param_Log_PassWord_Admin.Add("updated_by", _session.GetString("admin_name") ?? "");
                        Param_Log_PassWord_Admin.Add("sort", 0);
                        Param_Log_PassWord_Admin.Add("status", 1);
                        Param_Log_PassWord_Admin.Add("admin_user", (insertFields["username"] + "").ToString());
                        Param_Log_PassWord_Admin.Add("password", (insertFields["password"] + "").ToString());
                        Param_Log_PassWord_Admin.Add("web_id", _currentWebID);
                        _db.Insert("web_admin_password_log", Param_Log_PassWord_Admin);
                    }
                    #endregion

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

                    TempData["alert_message"] = string.Format("เพิ่มข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถเพิ่มข้อมูลได้";
                    return Redirect(string.Format("/Admin/{0}/Create", Module.Name));
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
                #region Select Item Edit 
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                #endregion

                #region Check requried field
                var f = collection;

                if (string.IsNullOrWhiteSpace(f["name"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ ชื่อ";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["surname"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ สกุล";
                    return Redirect("Create");
                }
                else if (string.IsNullOrWhiteSpace(f["email"]))
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "กรุณาระบุ อีเมล";
                    return Redirect("Create");
                }

                #endregion

                #region Set Cate session 
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                var updateFields = _admin.setFieldsUpdate(Module, collection, false);

                //--- custom value ----//
                if (updateFields != null)
                {
                    if (updateFields.ContainsKey("password") && !string.IsNullOrEmpty(updateFields["password"].ToString()))
                    {
                        updateFields["password"] = _utility.GenerateSHA512String(updateFields["password"].ToString());
                        updateFields.Add("last_change_password_at", System.DateTime.Now);

                        #region ############### ตรวจสอบการตั้งรหัสผ่านเดิม ##############
                        var Config_Password = _config.GetSection("ConfigPassword");
                        string Config_Password_Times = Config_Password.GetSection("LastPassword").Value.ToString();
                        var SQL_Old_Password_Last = _db.ExecuteQuery("SELECT TOP (" + Config_Password_Times.ToString() + ") password FROM [2026_web_admin_password_log] WHERE admin_user=@admin_user and web_id = @web_id ORDER BY created_at DESC", new Dictionary<string, object>() { { "admin_user", (itemEdit.Rows[0]["username"] + "").ToString() }, { "web_id", _currentWebID } });
                        if (SQL_Old_Password_Last.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow r_Old_Password_Last in SQL_Old_Password_Last.Rows)
                            {
                                if ((r_Old_Password_Last["password"] + "").ToString().Trim() == (updateFields["password"] + "").ToString().Trim())
                                {
                                    TempData["alert_message"] = "กรุณาตั้งรหัสผ่านใหม่ รหัสผ่านนี้ถูกใช้ไปก่อนหน้านี้แล้ว";
                                    return Redirect(string.Format("/Admin/{0}/Edit/{1}", Module.Name, id));
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        updateFields.Remove("password");
                    }

                    if (updateFields.ContainsKey("access_id") && !string.IsNullOrEmpty(updateFields["access_id"].ToString()))
                    {
                        updateFields["access_id"] = Convert.ToInt32(updateFields["access_id"]);
                    }
                    else
                    {
                        updateFields.Remove("access_id");
                    }

                    if (updateFields.ContainsKey("use_otp") && !string.IsNullOrEmpty(updateFields["use_otp"].ToString()))
                    {
                        updateFields["use_otp"] = Convert.ToInt32(updateFields["use_otp"]);
                    }
                    else
                    {
                        updateFields.Remove("use_otp");
                    }
                }
                //return Json(updateFields);

                var affected = _db.Update(Module.Config.Table, "WHERE id = @id", updateFields, new Dictionary<string, object>() { { "id", id } });

                if (affected != 0)
                {
                    #region ############### Log Password Admin ##############
                    if (updateFields.ContainsKey("password") && !string.IsNullOrEmpty(updateFields["password"].ToString()))
                    {
                        var Param_Log_PassWord_Admin = new Dictionary<string, object>();
                        Param_Log_PassWord_Admin.Add("created_at", System.DateTime.Now);
                        Param_Log_PassWord_Admin.Add("updated_at", System.DateTime.Now);
                        Param_Log_PassWord_Admin.Add("created_by", _session.GetString("admin_name") ?? "");
                        Param_Log_PassWord_Admin.Add("updated_by", _session.GetString("admin_name") ?? "");
                        Param_Log_PassWord_Admin.Add("sort", 0);
                        Param_Log_PassWord_Admin.Add("status", 1);
                        Param_Log_PassWord_Admin.Add("admin_user", (itemEdit.Rows[0]["username"] + "").ToString());
                        Param_Log_PassWord_Admin.Add("password", (updateFields["password"] + "").ToString());
                        Param_Log_PassWord_Admin.Add("web_id", _currentWebID);
                        _db.Insert("web_admin_password_log", Param_Log_PassWord_Admin);
                    }
                    #endregion

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

                    if (id == _session.GetInt32("admin_user_id") && _session.GetString("admin_user") != null)
                    {
                        #region Update Admin Session ----
                        _admin.setAdminSession(_session.GetString("admin_user"));
                        #endregion
                    }

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

        [HttpPost]
        [ModuleCheck("delete")]
        public override IActionResult Delete(IFormCollection f)
        {
            try
            {
                // TODO: Add delete logic here
                if (f.ContainsKey("id") && !string.IsNullOrEmpty(f["id"]))
                {
                    string allID = f["id"].ToString();
                    var allIDar = allID.Split(',');
                    if (allIDar.Length == 0)
                    {
                        TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                    else
                    {
                        #region ----- check kill oneself -----
                        var allIDint = allIDar.Select(int.Parse).ToList();
                        if (allIDint.Where(c => c == _session.GetInt32("admin_user_id")).ToList().Count() > 0)
                        {
                            TempData["alert_message"] = string.Format("ไม่สามารถลบข้อมูลตัวเองได้ 😵‍💫");
                            return Redirect(string.Format("/Admin/{0}", Module.Name));
                        }
                        #endregion 


                        int rowDel = 0;
                        int rowFail = 0;
                        foreach (string id in allIDar)
                        {
                            #region ----- select item delete -----
                            var itemDelete = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                            #endregion

                            if (_utility.isInt(id) && itemDelete.Rows.Count > 0)
                            {
                                var affected = _db.ExecuteNonQuery(string.Format("delete from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                                rowDel += affected;

                                if (affected > 0)
                                {
                                    if (Module.Config.CanMove == true)
                                    {
                                        _admin.reSort(Module);
                                    }
                                    #region Logs Action
                                    _admin.ActionLogs(
                                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                                        admin_username: _session.GetString("admin_user"),
                                        action: "delete",
                                        action_info: string.Format("ลบข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                                        action_url: Request.Host.Value + Request.Path.Value,
                                        action_table: Module.Config.Table,
                                        old_value: _db.DataTableToJSONWithJSONNet(itemDelete).TrimStart('[').TrimEnd(']'),
                                        mod_name: Module.Name,
                                        mod_name_txt: Module.Config.Text
                                    );
                                    #endregion
                                }
                            }
                        }

                        TempData["alert_message"] = string.Format("ลบข้อมูลแล้ว {0:n0} รายการ", rowDel);
                        TempData["alert_class"] = "alert-success";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));

                    }
                }
                else
                {
                    TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }
 
        public IActionResult AnnounceSubmitFileStatus()
        {
            string id = Request.Query["id"];
            string tb = "web_announce_submit";
            string file_status = Request.Query["file_status"];
            
            try
            { 
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(tb)), new Dictionary<string, object>() { { "id", Convert.ToInt64(id) }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                { 
                    return Content("2");
                }  

                var affected = _db.Update(tb, "WHERE id = @id and web_id = @web_id ", new Dictionary<string, object>() { { "file_status", file_status } }, new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
 
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
                        old_value: "",
                        new_value: file_status,
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    return Content("1");
                }
                else
                {
                    return Content("2");
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

        public IActionResult AgentListApproved()
        {
            string id = Request.Query["id"];
            string tb = "web_agent";
            string approved = Request.Query["approved"];
            
            try
            { 
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(tb)), new Dictionary<string, object>() { { "id", Convert.ToInt64(id) }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                { 
                    return Content("2");
                }  

                var affected = _db.Update(tb, "WHERE id = @id and web_id = @web_id ", new Dictionary<string, object>() { { "approved", Convert.ToInt32(approved) } }, new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
 
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
                        old_value: "",
                        new_value: approved,
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    return Content("1");
                }
                else
                {
                    return Content("2");
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
