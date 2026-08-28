using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MicrositeController : AdminCoreController
    {
        private string _errorMessage = "ไม่สามารถใช้ URL นี้ได้";
        private string _cleanURL = "";
        public MicrositeController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("Microsite");

            Module = _admin.setModuleCMSPage(Module);
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/

        [HttpPost]
        [ModuleCheck("add")]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                var f = collection;

                #region Check requried field 

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

                #region ----- check URL -----
                if (true)
                {
                    if (f["url"].ToString().Trim() != "")
                    {
                        bool checkFormat = CheckURL(f["url"].ToString().Trim());
                        if (checkFormat)
                        {

                        }
                        else
                        {
                            TempData["alert_message"] = _errorMessage;
                            return Redirect("Create");
                        }
                    }
                    else
                    {
                        TempData["alert_message"] = "ระบุ URL";
                        return Redirect("Create");
                    }
                }
                #endregion

                #region Set Cate session 
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                var insertFields = _admin.setFieldsCreate(Module, collection);
                insertFields["url"] = _cleanURL;
                //return Json(insertFields);
                var affected = _db.Insert(Module.Config.Table, insertFields);

                if (affected != 0)
                {
                    int last_create_id = 0;
                    var lastInsert = _db.ExecuteQuery(string.Format("SELECT top 1 MAX(id) as last_id from {0} WHERE web_id = @web_id GROUP BY id ORDER BY id desc", Db.T(Module.Config.Table)), new() { { "web_id", _currentWebID } });

                    if (lastInsert.Rows.Count > 0 && !string.IsNullOrEmpty(lastInsert.Rows[0]["last_id"].ToString()) && _utility.isInt(lastInsert.Rows[0]["last_id"] + ""))
                    {
                        last_create_id = Convert.ToInt32(lastInsert.Rows[0]["last_id"].ToString());
                    }

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "add",
                        action_info: string.Format("เพิ่มข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, last_create_id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(insertFields)
                    );
                    #endregion


                    #region ----- Microsite Access, Module, User -----
                    if (last_create_id > 0)
                    {
                        int resultInsertUser = -1;
                        bool resultInsertModule = false;
                        int newWebID = last_create_id;

                        //----- CreateRootAccess
                        int newAccessID = CreateRootAccess(newWebID);

                        //----- CreateRootAccessModlue
                        if (newAccessID > 0)
                        {
                            resultInsertModule = CreateRootAccessModlue(newWebID, newAccessID);
                        }

                        //----- CreateUser
                        if (resultInsertModule)
                        {
                            resultInsertUser = CreateUser(newWebID, newAccessID, f);
                        }

                        //----- CreateDefalutData
                        if (resultInsertUser > 0)
                        {
                            CreateDefalutData(newWebID);
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
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
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

                #region ----- check URL -----
                if (true)
                {
                    var f = collection;
                    if (f["url"].ToString().Trim() != "")
                    {
                        bool checkFormat = CheckURL(f["url"].ToString().Trim(), id);
                        if (checkFormat)
                        {

                        }
                        else
                        {
                            TempData["alert_message"] = _errorMessage;
                            return RedirectToActionPermanent("Edit", new { id = id });
                        }
                    }
                    else
                    {
                        TempData["alert_message"] = "ระบุ URL";
                        return RedirectToActionPermanent("Edit", new { id = id });
                    }
                }
                #endregion

                #region Set Cate session ############
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                var updateFields = _admin.setFieldsUpdate(Module, collection);
                //return Json(updateFields);
                var affected = _db.Update(Module.Config.Table, "WHERE id = @id and web_id = @web_id", updateFields, new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });

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
                        new_value: JsonConvert.SerializeObject(updateFields)
                    );
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckURL(IFormCollection f)
        {
            if (f["url"].ToString().Trim() != "")
            {
                int checkID = 0;
                if (f["check_id"] + "" != "" && _utility.isInt(f["check_id"] + ""))
                {
                    checkID = Convert.ToInt32(f["check_id"] + "");
                }

                bool checkFormat = CheckURL(f["url"].ToString().Trim(), checkID);
                if (checkFormat)
                {
                    return Json(new { status = 1, message = "สามารถใช้ URL นี้ได้", url = _cleanURL });
                }
                else
                {
                    return Json(new { status = 0, message = _errorMessage });
                }
            }
            else
            {
                return Json(new { status = 0, message = "ระบุ URL ที่ต้องการตรวจสอบ" });
            }
        }

        private bool CheckURL(string url, int id = 0)
        {
            //----- check format -----//
            if (!IsSlugValid(url))
            {
                _errorMessage += ", รูปแบบไม่ถูกต้อง (a-z,0-9,-)";
                return false;
            }

            //----- check db -----//
            var param = new Dictionary<string, object>() { { "url", url } };
            string conditionID = "";
            if (id > 0)
            {
                conditionID = $" and id <> @id ";
                param.Add("id", id);
            }
            var checkURL = _db.ExecuteQuery($"select top 1 title, url from {Db.T(Module.Config.Table)} where LOWER(url) = LOWER(@url) {conditionID}", param);
            if (checkURL.Rows.Count > 0)
            {
                _errorMessage = $"URL ถูกใช้งานแล้ว ({checkURL.Rows[0]["title"]}, {checkURL.Rows[0]["url"]})";
                return false;
            }

            _cleanURL = GenerateSlug(url);
            return true;
        }

        static bool IsSlugValid(string input)
        {
            // Define a regular expression pattern for a valid slug
            string pattern = "^[a-z0-9-]+$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(input, pattern);
        }

        private string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private int CreateRootAccess(int webID)
        {
            var site = _admin.WebInfo(webID, false, false);
            if (site != null)
            {
                var field = new Dictionary<string, object>()
                {
                    {"created_at", System.DateTime.Now },
                    {"updated_at", System.DateTime.Now },
                    {"created_by", _session.GetString("admin_name") ?? "" },
                    {"updated_by", _session.GetString("admin_name") ?? "" },
                    {"sort", 10 },
                    {"status", 1 },
                    {"admin_type", 0 },
                    {"title", "Root" },
                    {"web_id", webID },
                };

                var insertResult = _db.Insert("web_admin_access", field);

                if (insertResult > 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "microsite_add_root_access",
                        action_info: $"เพิ่ม Root access สำหรับ Microsite : {site["title"]} ({webID})",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin_access",
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(field)
                    );
                    #endregion

                    var lastInsert = _db.ExecuteQuery(string.Format("SELECT top 1 MAX(id) as last_id from {0} WHERE web_id = @web_id GROUP BY id ORDER BY id desc", Db.T("web_admin_access")), new() { { "web_id", webID } });
                    if (lastInsert.Rows.Count > 0 && !string.IsNullOrEmpty(lastInsert.Rows[0]["last_id"].ToString()) && _utility.isInt(lastInsert.Rows[0]["last_id"] + ""))
                    {
                        return Convert.ToInt32(lastInsert.Rows[0]["last_id"].ToString());
                    }
                }
            }

            return -1;
        }

        private bool CreateRootAccessModlue(int webID, int accessID)
        {
            var site = _admin.WebInfo(webID, false, false);
            if (site != null)
            {
                var allInsertedModule = new List<Dictionary<string, object>>();

                #region ----- All menu -----
                var adminMenu = _admin.AdminMenu();
                var allMenu = new List<AdminMenuModel>();
                if (adminMenu != null && accessID != 0)
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
                #endregion

                foreach (var item in allMenu)
                {
                    if (item.ModuleName != null)
                    {
                        var module = _admin.GetModule(item.ModuleName);
                        if (module != null)
                        {
                            var para = new Dictionary<string, object>();
                            para.Add("web_id", webID);
                            para.Add("updated_at", System.DateTime.Now);
                            para.Add("updated_by", _session.GetString("admin_user") ?? "");
                            para.Add("access_id", accessID);
                            para.Add("mod_name", item.ModuleName);

                            para.Add("can_add", module.Config.CanAdd == true ? 1 : 0);
                            para.Add("can_edit", module.Config.CanEdit == true ? 1 : 0);
                            para.Add("can_delete", module.Config.CanDelete == true ? 1 : 0);
                            para.Add("can_move", module.Config.CanMove == true ? 1 : 0);
                            para.Add("can_status", module.Config.CanStatus == true ? 1 : 0);
                            para.Add("can_export", module.Config.CanExport == true ? 1 : 0);
                            para.Add("can_approve", module.Config.CanApprove == true ? 1 : 0);
                            para.Add("created_at", System.DateTime.Now);
                            para.Add("created_by", _session.GetString("admin_user") ?? "");

                            var resultInsert = _db.Insert("web_admin_module", para);

                            para.Add("insert_result", resultInsert);

                            allInsertedModule.Add(para);
                        }

                    }
                }

                if (allInsertedModule.Count > 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "microsite_add_root_access_module",
                        action_info: $"เพิ่ม Access module สำหรับ Microsite : {site["title"]} ({webID})",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin_module",
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(allInsertedModule)
                    );
                    #endregion

                    return true;
                }
            }

            return false;
        }

        private int CreateUser(int webID, int accessID, IFormCollection f)
        {
            var site = _admin.WebInfo(webID, false, false);
            if (site != null)
            {
                var userModule = _admin.GetModule("AdminUser");
                var userFields = _admin.setFieldsCreate(userModule, f, false);

                if (userFields != null)
                {
                    if (userFields.ContainsKey("password") && !string.IsNullOrEmpty(userFields["password"].ToString()))
                    {
                        userFields["password"] = _utility.GenerateSHA512String(userFields["password"].ToString());
                        userFields.Add("last_change_password_at", System.DateTime.Now);
                    }
                    if (userFields.ContainsKey("access_id"))
                    {
                        userFields["access_id"] = accessID;
                    }
                    else
                    {
                        userFields.Add("access_id", accessID);
                    }

                    if (userFields.ContainsKey("use_otp"))
                    {
                        userFields["use_otp"] = Convert.ToInt32(userFields["use_otp"]);
                    }

                    if (userFields.ContainsKey("web_id"))
                    {
                        userFields["web_id"] = webID;
                    }
                    else
                    {
                        userFields.Add("web_id", webID);
                    }
                }

                var insertDefaultUser = _db.Insert("web_admin", userFields);

                if (insertDefaultUser > 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "microsite_add_root_user",
                        action_info: $"เพิ่ม Root user สำหรับ Microsite : {site["title"]} ({webID})",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin",
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(userFields)
                    );
                    #endregion
                }

                return insertDefaultUser;
            }
            return -1;
        }

        private void CreateDefalutData(int webID)
        {
            var site = _admin.WebInfo(webID, false, false);
            if (site != null)
            {
                #region ----- Default หน้าแรก -----
                if (true)
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "web_id", webID },
                        { "is_home", 1 },

                        { "created_at", System.DateTime.Now },
                        { "updated_at", System.DateTime.Now },
                        { "created_by", _session.GetString("admin_name") ?? "" },
                        { "updated_by", _session.GetString("admin_name") ?? "" },
                        { "sort", 10 },
                        { "status", 1 },
                        { "pb_status", 1 },
                        { "approve_by", _session.GetInt32("admin_user_id") ?? 0 },
                        { "show_front", 1 }
                    };

                    foreach (string s in new List<string>() { "", "pb_" })
                    {
                        data.Add($"{s}cat_id", 0);
                        data.Add($"{s}title", "หน้าแรก");
                        data.Add($"{s}en_title", "Home Page");
                        data.Add($"{s}url_target", "_self");
                        data.Add($"{s}issue_date", System.DateTime.Now);
                        data.Add($"{s}expiry_date", System.DateTime.Now.AddYears(10));
                        data.Add($"{s}issue_date_config", 1);
                        data.Add($"{s}page_type", 3);
                        data.Add($"{s}page_type_sub", 1);
                        data.Add($"{s}parent_id", 0);
                        data.Add($"{s}page_style", 1);
                    }

                    var insertResult = _db.Insert("web_cms_page", data);
                    if (insertResult > 0)
                    {
                        data.Add("insert_result", insertResult);

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "microsite_add_defalut_index",
                            action_info: $"เพิ่ม 'หน้าแรก' สำหรับ Microsite : {site["title"]} ({webID})",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_cms_page",
                            old_value: "",
                            new_value: JsonConvert.SerializeObject(data)
                        );
                        #endregion
                    }
                }
                #endregion

                #region ----- Default ปรับแต่ง Header -----
                if (true)
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "web_id", webID },

                        { "created_at", System.DateTime.Now },
                        { "updated_at", System.DateTime.Now },
                        { "created_by", _session.GetString("admin_name") ?? "" },
                        { "updated_by", _session.GetString("admin_name") ?? "" },
                        { "sort", 10 },
                        { "status", 1 },
                        { "pb_status", 1 },
                        { "approve_by", _session.GetInt32("admin_user_id") ?? 0 },
                        { "show_front", 1 }
                    };

                    foreach (string s in new List<string>() { "", "pb_" })
                    {
                        data.Add($"{s}cat_id", 0);
                        data.Add($"{s}title", "โลโก้เว็บไซต์");
                        data.Add($"{s}en_title", "");
                        data.Add($"{s}this_type", 1);
                        data.Add($"{s}issue_date", System.DateTime.Now);
                        data.Add($"{s}expiry_date", System.DateTime.Now.AddYears(10));
                    }

                    var insertResult = _db.Insert("web_home_header", data);
                    if (insertResult > 0)
                    {
                        data.Add("insert_result", insertResult);

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "microsite_add_defalut_header",
                            action_info: $"เพิ่ม 'ปรับแต่ง Header' สำหรับ Microsite : {site["title"]} ({webID})",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_home_header",
                            old_value: "",
                            new_value: JsonConvert.SerializeObject(data)
                        );
                        #endregion
                    }
                }
                #endregion

                #region ----- Default ปรับแต่ง Footer -----
                if (true)
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "web_id", webID },

                        { "created_at", System.DateTime.Now },
                        { "updated_at", System.DateTime.Now },
                        { "created_by", _session.GetString("admin_name") ?? "" },
                        { "updated_by", _session.GetString("admin_name") ?? "" },
                        { "sort", 10 },
                        { "status", 1 },
                        { "pb_status", 1 },
                        { "approve_by", _session.GetInt32("admin_user_id") ?? 0 },
                        { "show_front", 1 }
                    };

                    foreach (string s in new List<string>() { "", "pb_" })
                    {
                        data.Add($"{s}title", "");
                        data.Add($"{s}en_title", "");
                        data.Add($"{s}tel", "");
                        data.Add($"{s}email", "");
                        data.Add($"{s}sc_fb", "");
                        data.Add($"{s}sc_tw", "");
                        data.Add($"{s}sc_ln", "");
                        data.Add($"{s}sc_yt", "");
                    }

                    var insertResult = _db.Insert("web_home_footer", data);
                    if (insertResult > 0)
                    {
                        data.Add("insert_result", insertResult);

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "microsite_add_defalut_footer",
                            action_info: $"เพิ่ม 'ปรับแต่ง Footer' สำหรับ Microsite : {site["title"]} ({webID})",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_home_footer",
                            old_value: "",
                            new_value: JsonConvert.SerializeObject(data)
                        );
                        #endregion
                    }
                }
                #endregion

                #region ----- Default ตั้งค่า SEO เว็บไซต์ -----
                if (true)
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "web_id", webID },

                        { "created_at", System.DateTime.Now },
                        { "updated_at", System.DateTime.Now },
                        { "created_by", _session.GetString("admin_name") ?? "" },
                        { "updated_by", _session.GetString("admin_name") ?? "" },
                        { "sort", 10 },
                        { "status", 1 },
                        { "pb_status", 1 },
                        { "approve_by", _session.GetInt32("admin_user_id") ?? 0 },
                        { "show_front", 1 }
                    };

                    foreach (string s in new List<string>() { "", "pb_" })
                    {
                        data.Add($"{s}title", "");
                        data.Add($"{s}en_title", "");
                        data.Add($"{s}des", "");
                        data.Add($"{s}en_des", "");
                        data.Add($"{s}info", "");
                        data.Add($"{s}en_info", "");
                    }

                    var insertResult = _db.Insert("web_home_seo", data);
                    if (insertResult > 0)
                    {
                        data.Add("insert_result", insertResult);

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "microsite_add_defalut_seo",
                            action_info: $"เพิ่ม 'ตั้งค่า SEO เว็บไซต์' สำหรับ Microsite : {site["title"]} ({webID})",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_home_seo",
                            old_value: "",
                            new_value: JsonConvert.SerializeObject(data)
                        );
                        #endregion
                    }
                }
                #endregion

                //----- ไม่มี Default 'อัตราดอกเบี้ย' : ตาราง web_rate_cms ไม่มีในฐานข้อมูล sam
                //      และเมนู ExchangeRates ถูกซ่อนจาก microsite อยู่แล้ว (AdminHelpers.AdminMenu)

                #region ----- Default เนื้อหา single (1 เมนู = 1 ระเบียน) ของทุกเมนู microsite -----
                //----- เมนู microsite ที่เป็น "เครื่องยนต์ single" (web_core_single) จะแก้ไขอย่างเดียว (CanAdd=false)
                //      ถ้าไม่ seed ระเบียนไว้ตั้งแต่แรก เปิดเมนูมาจะว่างและกด "แก้ไข" ไม่ได้เลย
                //      จึงสร้างระเบียนเปล่า 1 แถว/โมดูล (อ้างอิงจาก allowlist เมนูซ้ายของ microsite) ให้พร้อมแก้ไขทันที
                //      อ้างชุดเมนูจาก AdminHelpers.MicrositeAllowModules เพื่อให้เมนู microsite ใหม่ในอนาคตถูก seed อัตโนมัติ
                var seededSingle = new List<Dictionary<string, object>>();
                foreach (var moduleName in AdminHelpers.MicrositeAllowModules)
                {
                    var m = _admin.GetModule(moduleName);
                    if (m == null) continue;

                    //----- เอาเฉพาะ single ที่แก้ไขอย่างเดียว (มี module_id) — item/group/news/eform มีปุ่ม "เพิ่ม" อยู่แล้วจึงไม่ต้อง seed
                    if ((m.Config.Table ?? "") != "web_core_single") continue;
                    if (m.Config.CanAdd == true) continue;
                    if (!(m.Config.TableModuleID != null && m.Config.TableModuleID > 0)) continue;

                    //----- idempotent : กันสร้างซ้ำถ้ามีอยู่แล้ว (web_id + module_id)
                    var exists = _db.ExecuteQuery(
                        "select top 1 id from [2026_web_core_single] where web_id = @web_id and module_id = @module_id",
                        new() { { "web_id", webID }, { "module_id", m.Config.TableModuleID } });
                    if (exists.Rows.Count > 0) continue;

                    var row = new Dictionary<string, object>()
                    {
                        { "web_id", webID },
                        { "module_id", m.Config.TableModuleID },

                        { "created_at", System.DateTime.Now },
                        { "updated_at", System.DateTime.Now },
                        { "created_by", _session.GetString("admin_name") ?? "" },
                        { "updated_by", _session.GetString("admin_name") ?? "" },
                        { "sort", 10 },
                        { "status", 1 },
                        { "pb_status", 1 },                 //----- 1 = ไม่ค้างอนุมัติ จึงกดแก้ไขได้ทันที (ดู Edit lock)
                        { "approve_by", _session.GetString("admin_user") ?? "" },
                        { "show_front", 1 },
                        { "issue_date", System.DateTime.Now },
                        { "expiry_date", System.DateTime.Now.AddYears(10) },
                        { "pb_issue_date", System.DateTime.Now },
                        { "pb_expiry_date", System.DateTime.Now.AddYears(10) },
                        { "issue_date_config", 1 },
                        { "pb_issue_date_config", 1 },
                    };

                    //----- ตั้งชื่อระเบียน = ชื่อเมนู เพื่อให้หน้า list อ่านออก (คู่ pb_ ตาม approval workflow)
                    foreach (string s in new List<string>() { "", "pb_" })
                    {
                        row.Add($"{s}title", m.Config.Text ?? "");
                        row.Add($"{s}en_title", "");
                    }

                    //----- web_core_single.id ไม่ใช่ auto-identity (ดู AdminHelpers.setFieldsCreate) จึงต้องกำหนด id เอง
                    var dtNextId = _db.ExecuteQuery("select coalesce(max(id), 0) + 1 as next_id from [2026_web_core_single]");
                    row["id"] = (dtNextId.Rows.Count > 0) ? Convert.ToInt32(dtNextId.Rows[0]["next_id"]) : 1;

                    var insertResult = _db.Insert("web_core_single", row);
                    if (insertResult > 0)
                    {
                        seededSingle.Add(new Dictionary<string, object>()
                        {
                            { "module", moduleName },
                            { "module_id", m.Config.TableModuleID },
                            { "insert_result", insertResult }
                        });
                    }
                }

                if (seededSingle.Count > 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "microsite_add_defalut_single",
                        action_info: $"เพิ่มระเบียนตั้งต้นเมนู single ({seededSingle.Count} เมนู) สำหรับ Microsite : {site["title"]} ({webID})",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_core_single",
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(seededSingle)
                    );
                    #endregion
                }
                #endregion
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WarpPortalTo(IFormCollection f)
        {
            int webID = -1;
            string username = "";
             
            #region ----- Input validate -----
            if (
                !string.IsNullOrWhiteSpace(f["webID"]) &&
                _utility.isInt(f["webID"]) &&
                Convert.ToInt32(f["webID"]) > 0
                )
            { 
                webID = Convert.ToInt32(f["webID"]);
            }
            else
            { 
                return Json(new { status = 0, message = $"เว็บไซต์ที่เลือกไม่ถูกต้อง" });
            }
            #endregion

            #region ----- Check webID -----
            var checkWeb = _admin.WebInfo(webID, false, false);
            if (checkWeb == null)
            {
                return Json(new { status = 0, message = $"ไม่พบเว็บไซต์ที่เลือก" });
            }
            #endregion
             
            #region ----- Check root user in microsite -----
            var selectUser = _db.ExecuteQuery(
                "select top 1 username, web_id, status, created_at from [2026_web_admin] where web_id = @web_id and status = 1 order by created_at asc",
                new Dictionary<string, object>() { { "web_id", webID } }
                );
             
            if (selectUser.Rows.Count > 0)
            {
                username = selectUser.Rows[0]["username"] + "";
            }
            else
            {
                return Json(new { status = 0, message = $"ไม่พบ User สำหรับเว็บไซต์ที่เลือก" });
            }
            #endregion
             
            #region Logs Action
            _admin.ActionLogs(
                admin_user_id: (int)_session.GetInt32("admin_user_id"),
                admin_username: _session.GetString("admin_user"),
                action: "login_with_mainsite",
                action_info: $"เข้าสู่ระบบ Microsite ({checkWeb["title"]})",
                action_url: Request.Host.Value + Request.Path.Value,
                action_table: "",
                old_value: "",
                new_value: JsonConvert.SerializeObject(new { webID, username })
            );
            #endregion

            _admin.setAdminSession(username, webID);
             
            return Json(new { status = 1, message = $"เข้าสู่ระบบแล้ว" });
        }

    }
}
