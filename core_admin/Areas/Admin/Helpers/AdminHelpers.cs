using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Primitives;
using System.Data;
using System.Globalization;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class AdminHelpers
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _context;
        private readonly ISession _session;
        private Utility _utility;
        private DBHelper _db;

        public int _currentWebID = -1;

        public AdminHelpers(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig);

            SetSessionWebID();
        }

        /// <summary>
        /// เมนู/โมดูลที่ microsite (web_id != 0) เข้าถึงได้ ตามข้อตกลงกับลูกค้า
        /// นอกเหนือจากรายการนี้ = ซ่อนจากเมนูด้านซ้าย และเข้าผ่าน URL ตรง ๆ ไม่ได้
        /// (เป็น allowlist โดยตั้งใจ — โมดูลใหม่ที่เพิ่มภายหลังจะถูกปิดสำหรับ microsite อัตโนมัติ)
        /// ไม่มีผลกับเว็บไซต์หลัก (web_id = 0) ซึ่งยังเห็นเมนูครบเหมือนเดิม
        /// </summary>
        public static readonly List<string> MicrositeAllowModules = new()
        {
            //----- หน้าเว็บไซต์ (เฉพาะ 4 เมนู)
            "CMSPage", "HomeHeader", "HomeFooter", "HomeSEO",
            //----- ข้อมูลหน้าแรก2 (ทั้งกลุ่ม) — กลุ่ม "ข้อมูลหน้าแรก" (HomeImageSlide/HomeSamText*) ไม่เปิดให้ microsite
            "HeroBanner",
            "MicrositeIntro", "MicrositeRole", "MicrositeBannerHalf", "MicrositeDoc",
            "MicrositeBenefit", "MicrositeAssetHead", "MicrositeAsset", "MicrositeTerm",
            "MicrositeHighlight", "MicrositeContactQr", "MicrositeCounter", "MicrositeHelp",
            "MicrositePrepare", "MicrositeLead", "MicrositeFaq",
            //----- คำถามที่พบบ่อย (ทั้งกลุ่ม)
            "FAQGroup", "FAQ",
            //----- E-Form (ทั้งกลุ่ม)
            "EFormGroup", "EForm", "EFormSubmit",
            //----- ผู้ดูแลระบบ (ทั้งกลุ่ม)
            "AdminAccess", "AdminUser", "AdminLog", "AdminUserNotLogin",
            //----- Widget2 (ทั้งกลุ่ม)
            "WidgetGroup2", "Widget2"
        };

        public static bool IsMicrositeAllowed(string? moduleName)
        {
            return MicrositeAllowModules.Any(m => string.Equals(m, moduleName ?? "", StringComparison.OrdinalIgnoreCase));
        }

        public List<AdminMenuModel>? AdminMenu()
        {
            var allMenu = new AdminMenu().Menu();
            var webID = _session.GetInt32("admin_web_id") ?? 0;
            if (webID != 0)
            {
                //----- microsite : เก็บเฉพาะเมนูย่อยที่อยู่ใน allowlist และตัดกลุ่มที่ไม่เหลือเมนูย่อยทิ้ง
                var filtered = new List<AdminMenuModel>();
                foreach (var group in allMenu ?? new List<AdminMenuModel>())
                {
                    if (group.SubMenu != null && group.SubMenu.Count > 0)
                    {
                        var subs = group.SubMenu.Where(s => IsMicrositeAllowed(s.ModuleName)).ToList();
                        if (subs.Count > 0)
                        {
                            filtered.Add(new AdminMenuModel()
                            {
                                Title = group.Title,
                                Link = group.Link,
                                ModuleName = group.ModuleName,
                                Icon = group.Icon,
                                SubMenu = subs
                            });
                        }
                    }
                    else if (IsMicrositeAllowed(group.ModuleName))
                    {
                        filtered.Add(group);
                    }
                }
                allMenu = filtered;
            }
            else
            {
                //----- เว็บไซต์หลักใช้ชุด widget เดิม จึงซ่อนเมนู widget2
                //----- และซ่อนกลุ่ม "เนื้อหาหน้าแรก" (เฉพาะ microsite เท่านั้นที่ใช้)
                allMenu = allMenu?.Where(c =>
                    (c.Title ?? "").ToLower() != "widget2"
                    && (c.Title ?? "") != "เนื้อหาหน้าแรก").ToList();
            }
            return allMenu;
        }

        public List<Module> AllModule()
        {
            var allModules = new AdminMenu().AllModule();
            var webID = _session.GetInt32("admin_web_id") ?? 0;
            if (webID != 0)
            {
                var micrositeHideMenu = new List<string>()
                {
                    "microsite",
                    "ExchangeRates".ToLower(),
                    //----- microsite ใช้ชุด widget2 จึงซ่อนโมดูล widget เดิม
                    "Widget".ToLower(),
                    "WidgetGroup".ToLower()
                };
                //----- c.Name อาจเป็น null (บางเมนูไม่ได้ตั้งค่าไว้) จึงต้อง null-safe ไม่งั้น throw ตอน login microsite
                //      ส่วนการปิดกั้นเข้า URL ตรง ๆ ตาม allowlist ทำที่ [ModuleCheck] (ModuleCheckAttribute)
                allModules = allModules?.Where(c => !micrositeHideMenu.Contains((c.Name ?? "").ToLower())).ToList();
            }
            else
            {
                //----- เว็บไซต์หลักใช้ชุด widget เดิม จึงซ่อนโมดูล widget2
                var mainHideMenu = new List<string>()
                {
                    "Widget2".ToLower(),
                    "WidgetGroup2".ToLower()
                };
                allModules = allModules?.Where(c => !mainHideMenu.Contains((c.Name ?? "").ToLower())).ToList();
            }
            return allModules;
        }

        public Module? GetModule(string ModuleName = "")
        {
            return AllModule().FirstOrDefault(c => c.Name == ModuleName);
        }

        #region Widget set (web_id)
        //----- เว็บไซต์หลัก (web_id = 0) ใช้ชุด widget เดิม, microsite (web_id != 0) ใช้ชุด widget2
        public bool UseWidget2()
        {
            return (_session.GetInt32("admin_web_id") ?? 0) != 0;
        }
        public string WidgetTable()
        {
            return UseWidget2() ? "web_widget2" : "web_widget";
        }
        public string WidgetGroupTable()
        {
            return UseWidget2() ? "web_widget_group2" : "web_widget_group";
        }
        #endregion

        public DataRow? getAccess(string ModuleName = "", int access_id = 0)
        {
            var accessMaster = _db.ExecuteQuery("select top 1 id from [2026_web_admin_access] where id = @access_id and web_id = @web_id", new() { { "access_id", access_id }, { "web_id", _currentWebID } });
            var access = _db.ExecuteQuery("select top 1 * from [2026_web_admin_module] where access_id = @access_id and mod_name = @mod_name and web_id = @web_id", new() { { "access_id", access_id }, { "mod_name", ModuleName }, { "web_id", _currentWebID } });
            if (access.Rows.Count > 0 && accessMaster.Rows.Count > 0)
            {
                return access.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public bool checkAccess(Module? Module, int access_id = 0, string access_type = "")
        {
            //----- Module เป็น null ได้ เมื่อเมนูอ้างถึงโมดูลที่ถูกซ่อนสำหรับไซต์นี้ (เช่น Microsite บน microsite)
            if (Module == null || Module.Name == null || Module.Name == "")
            {
                return false;
            }
            else
            {
                var getAccess = this.getAccess(Module.Name, access_id);
                if (getAccess != null)
                {
                    access_type = access_type.ToLower();
                    if (access_type == "")
                    {
                        return true;
                    }
                    else if (access_type == "add" && Module.Config.CanAdd == true && !string.IsNullOrEmpty(getAccess["can_add"].ToString()) && getAccess["can_add"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "edit" && Module.Config.CanEdit == true && !string.IsNullOrEmpty(getAccess["can_edit"].ToString()) && getAccess["can_edit"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "delete" && Module.Config.CanDelete == true && !string.IsNullOrEmpty(getAccess["can_delete"].ToString()) && getAccess["can_delete"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "move" && Module.Config.CanMove == true && !string.IsNullOrEmpty(getAccess["can_move"].ToString()) && getAccess["can_move"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "status" && Module.Config.CanStatus == true && !string.IsNullOrEmpty(getAccess["can_status"].ToString()) && getAccess["can_status"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "export" && Module.Config.CanExport == true && !string.IsNullOrEmpty(getAccess["can_export"].ToString()) && getAccess["can_export"].ToString() == "1")
                    {
                        return true;
                    }
                    else if (access_type == "approve" && Module.Config.CanApprove == true && !string.IsNullOrEmpty(getAccess["can_approve"].ToString()) && getAccess["can_approve"].ToString() == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public Dictionary<string, object>? CheckAdmin(string username, string password, int webID = 0)
        {
            var sql_param = new Dictionary<string, object>() { { "username", username } };
            string sql_password = "";
            if (password != "")
            {
                sql_password = " and password = @password ";
                sql_param.Add("password", password);
            }


            if (webID > -1)
            {
                sql_password += " and web_id = @web_id ";
                sql_param.Add("web_id", webID);
            }

            string sql = $"select top 1 * from [2026_web_admin] where username = @username {sql_password}";
            var admin_user = _db.ExecuteQuery(sql, sql_param);
            if (admin_user.Rows.Count > 0)
            {
                var admin = admin_user.Rows[0];
                return new Dictionary<string, object>()
                {
                    { "id", Convert.ToInt32(admin["id"]) },
                    { "access_id", admin["access_id"] },
                    { "username", (admin["username"] + "").Trim() },
                    { "password", (admin["password"] + "") .Trim() },
                    { "name", (admin["name"] + "") .Trim() },
                    { "surname", (admin["surname"] + "") .Trim() },
                    { "section", (admin["section"] + "") .Trim() },
                    { "email", (admin["email"] + "") .Trim() },
                    { "status", admin["status"] },
                    { "force_change_password", admin["force_change_password"] },
                    { "use_otp", admin["use_otp"] },
                    { "otp", admin["otp"] },
                    { "otp_dt", admin["otp_dt"] },
                    { "last_change_password_at", admin["last_change_password_at"] },
                    { "web_id",  Convert.ToInt32(admin["web_id"]) }
                };
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, object>? CheckAdmin(string username, int webID = 0)
        {
            string? password = "";
            return CheckAdmin(username, password, webID);
        }

        public Dictionary<string, object>? AdminInfo(string username)
        {
            int webID = 0;
            if (_session.GetInt32("admin_web_id").HasValue)
            {
                webID = _session.GetInt32("admin_web_id").Value;
            }
            return CheckAdmin(username, webID);
        }

        public bool CheckAdminWeb(int userID, int webID)
        {
            var sql_param = new Dictionary<string, object>() { { "id", userID }, { "web_id", webID } };
            
            string sql = $"select top 1 id from [2026_web_admin] where id = @id and web_id = @web_id";
            var admin_user = _db.ExecuteQuery(sql, sql_param);

            //----- เช็คแค่ว่ามีไซต์นี้จริง ไม่ผูกกับ approve/ช่วงวันที่แสดงผล (เกณฑ์นั้นใช้กับ front-end)
            var web = WebInfo(webID, false, false);

            return (admin_user.Rows.Count > 0 && web != null) ? true : false;
        }

        public void setAdminSession(string username, int webID = 0)
        {
            var admin_user = CheckAdmin(username, webID);
            if (admin_user != null)
            {
                _session.SetString("admin_login", "1");              
                _session.SetString("admin_id", admin_user["id"] + "");              
                _session.SetString("admin_user", (admin_user["username"] + "").Trim());
                _session.SetString("admin_name", (admin_user["name"] + "").Trim());
                _session.SetString("admin_surname", (admin_user["surname"] + "").Trim());
                _session.SetString("admin_pass", (admin_user["password"] + "").Trim());

                _session.SetString("admin_fm_path1", "Files/Site" + admin_user["web_id"] + "/");
                _session.SetString("admin_fm_path2", "Files/Site" + admin_user["web_id"] + "/" + admin_user["id"] + "/");

                _session.SetInt32("admin_user_id", Convert.ToInt32(admin_user["id"] + ""));
                _session.SetInt32("admin_web_id", Convert.ToInt32(admin_user["web_id"] + ""));
                _session.SetInt32("admin_access_id", Convert.ToInt32(admin_user["access_id"] + ""));
                #region ############ Force Change Password ################
                string Text_Password_Expired_In = "";
                if ((admin_user["force_change_password"] + "").ToString().Trim() != "1")
                {
                    var Config_Password = _config.GetSection("ConfigPassword");
                    int Config_Password_ExpiresInDay = Convert.ToInt32(Config_Password.GetSection("ExpiresInDay").Value.ToString());
                    int Config_Password_AlertInDay = Convert.ToInt32(Config_Password.GetSection("AlertInDay").Value.ToString());
                    if (!string.IsNullOrEmpty(admin_user["last_change_password_at"].ToString().Trim()))
                    {
                        System.DateTime Date_Expired = Convert.ToDateTime(admin_user["last_change_password_at"].ToString().Trim()).ToUniversalTime().ToLocalTime().AddDays(Config_Password_ExpiresInDay);
                        double Get_Day_Left = (Date_Expired.Date - System.DateTime.Now.Date).TotalDays;
                        if (Get_Day_Left > 0)
                        {
                            if (Convert.ToDouble(Get_Day_Left) <= Convert.ToDouble(Config_Password_AlertInDay))
                            {
                                Text_Password_Expired_In = "รหัสผ่านจะหมดอายุในอีก " + Get_Day_Left.ToString("#,##0") + " วัน";
                            }
                        }
                        else
                        {
                            Text_Password_Expired_In = "รหัสผ่านหมดอายุแล้ว กรุณาเปลี่ยนรหัสผ่าน";

                            var Param_Force_Change_Password = new Dictionary<string, object>();
                            Param_Force_Change_Password.Add("force_change_password", 1);
                            _db.Update("web_admin", "WHERE id = @id", Param_Force_Change_Password, new Dictionary<string, object>() { { "id", Convert.ToInt32((admin_user["id"] + "").ToString()) } });
                        }
                    }
                }
                else
                {
                    Text_Password_Expired_In = "รหัสผ่านหมดอายุแล้ว กรุณาเปลี่ยนรหัสผ่าน";
                }
                _session.SetString("admin_text_password_expired_in", Text_Password_Expired_In.ToString().Trim());
                #endregion

                #region ----- Cookie webID -----
                try
                {
                    _context.HttpContext.Response.Cookies.Append(
                        "webID",
                        admin_user["web_id"] + "",
                        new CookieOptions()
                        {
                            Path = "/",
                            HttpOnly = false,
                            Secure = false
                        }
                    );
                }
                catch
                {

                }
                #endregion
            }
        }

        public void LockUser(string username, int webID = 0)
        {
            _db.Update(
                "web_admin",
                "where username = @username and web_id = @web_id ",
                new Dictionary<string, object>() { { "status", 0 } },
                new Dictionary<string, object>() { { "username", username }, { "web_id", webID } }
                );
        }

        public void ClearSession()
        {
            string sPrefix = "admin_";
            var sessionObjects = new List<string>();
            try
            {
                foreach (var item in _session.Keys)
                {
                    if (item.ToString().StartsWith(sPrefix))
                    {
                        sessionObjects.Add(item.ToString());
                    }
                }
            }
            catch
            {

            }

            foreach (var item in sessionObjects)
            {
                _session.Remove(item);
            }
        }

        public string getIP()
        {
            var result = string.Empty;

            //first try to get IP address from the forwarded header
            if (_context.HttpContext.Request.Headers != null)
            {
                //the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                //connecting to a web server through an HTTP proxy or load balancer

                var forwardedHeader = _context.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!StringValues.IsNullOrEmpty(forwardedHeader))
                    result = forwardedHeader.FirstOrDefault();
            }

            //if this header not exists try get connection remote IP address
            if (string.IsNullOrEmpty(result) && _context.HttpContext.Connection.RemoteIpAddress != null)
                result = _context.HttpContext.Connection.RemoteIpAddress.ToString();

            return result;
        }

        public void ActionLogs(int admin_user_id, string admin_username, string action, string action_info, string action_url = "", string action_table = "", string old_value = "", string new_value = "", string mod_name = "", string mod_name_txt = "")
        { 
            _db.Insert("web_admin_log",
                new Dictionary<string, object>() {
                    { "created_at", System.DateTime.Now },
                    { "updated_at", System.DateTime.Now },
                    { "created_by", "system" },
                    { "updated_by", "system" },
                    { "admin_user_id", admin_user_id },
                    { "admin_username", admin_username },
                    { "action", action },
                    { "action_info", action_info },
                    { "action_url", action_url },
                    { "action_table", action_table },
                    { "ip", getIP() },
                    { "old_value", old_value },
                    { "new_value", new_value },
                    { "web_id", _currentWebID },
                    { "mod_name", mod_name },
                    { "mod_name_txt", mod_name_txt }
                });
        }

        public Module setModuleCMSPage(Module Module)
        {
            #region ----- Check CMS Page -----
            if (Module != null && Module.Config.IsCMSPage == true)
            {
                if (Module.Config.IsCMSPage != null && Module.Config.IsCMSPage == true && Module.Config.FieldCMSPage != "")
                {
                    if (Module.Config.FieldSearch != null)
                    {
                        Module.Config.FieldSearch.Add(new(Module.Config.FieldCMSPage, new() { Module.Config.FieldCMSPage }));
                    }
                    else
                    {
                        Module.Config.FieldSearch = new()
                        {
                            new (Module.Config.FieldCMSPage, new() { Module.Config.FieldCMSPage })
                        };
                    }

                    if (Module.Config.FieldSearchIsEqual != null)
                    {
                        Module.Config.FieldSearchIsEqual.Add(Module.Config.FieldCMSPage);
                    }
                    else
                    {
                        Module.Config.FieldSearchIsEqual = new List<string>() { Module.Config.FieldCMSPage };
                    }

                }

                // set default parent_id
                if (string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage)))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage, "0");
                }
            }
            #endregion

            return Module;
        }

        public Module setBreadcrumbCMSPage(Module Module)
        {
            #region ----- Check CMS Page -----
            if (Module != null && Module.Config.IsCMSPage == true && Module.Config.FieldCMSPage != "")
            {
                // parent lv
                int parent_lv = 1;
                string TextBreadcrumb = "";
                string parent_id = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage) ?? "0";

                while (parent_id != "0")
                {
                    //string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage) ?? "0";
                    var parentDT = _db.ExecuteQuery(string.Format("select top 1 id, {1}, title from {0} where cast(id as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and web_id = @web_id", Db.T(Module.Config.Table), Module.Config.FieldCMSPage), new Dictionary<string, object>() { { "cate_val", parent_id }, { "web_id", _currentWebID} });
                    if (parentDT.Rows.Count > 0)
                    {
                        parent_id = parentDT.Rows[0][Module.Config.FieldCMSPage].ToString() ?? "0";
                        TextBreadcrumb = "/" + parentDT.Rows[0]["title"] + TextBreadcrumb;
                    }
                    else
                    {
                        break;
                    }
                    parent_lv++;
                    if (parent_lv == 10) { break; }
                }

                _session.SetInt32("admin_" + Module.Name + "_parent_lv", parent_lv);
                Module.Config.TextBreadcrumb += TextBreadcrumb;

                if (parent_lv > 1)
                {
                    Module.Config.Text = Module.Config.TextBreadcrumb.Split('/').ToList().Last();
                }
            }
            #endregion

            return Module;
        }

        public Module setSessionRequest(Module Module, HttpRequest Request)
        {
            #region ------------- Page ------------
            if (!string.IsNullOrEmpty(Request.Query["page"]) && _utility.isInt(Request.Query["page"].ToString()))
            {
                Module.Config.Page = Convert.ToInt32(Request.Query["page"].ToString());
                _session.SetString("admin_" + Module.Name + "_page", Request.Query["page"].ToString());
            }
            else if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_page")))
            {
                Module.Config.Page = Convert.ToInt32(_session.GetString("admin_" + Module.Name + "_page"));
            }
            #endregion

            #region ------------- PerPage ------------
            if (!string.IsNullOrEmpty(Request.Query["perpage"]) && _utility.isInt(Request.Query["perpage"].ToString()))
            {
                Module.Config.PerPage = Convert.ToInt32(Request.Query["perpage"].ToString());
                _session.SetString("admin_" + Module.Name + "_perpage", Request.Query["perpage"].ToString());
            }
            else if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_perpage")))
            {
                Module.Config.PerPage = Convert.ToInt32(_session.GetString("admin_" + Module.Name + "_perpage"));
            }
            #endregion

            #region ------------- OrderBy ------------
            if (!string.IsNullOrEmpty(Request.Query["orderby"]))
            {
                Module.Config.OrderBy = Request.Query["orderby"].ToString();
                _session.SetString("admin_" + Module.Name + "_orderby", Request.Query["orderby"].ToString());
            }
            else if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_orderby")))
            {
                Module.Config.OrderBy = _session.GetString("admin_" + Module.Name + "_orderby");
            }
            #endregion

            #region ------------- Sort ------------
            if (!string.IsNullOrEmpty(Request.Query["sort"]))
            {
                Module.Config.Sort = Request.Query["sort"].ToString();
                _session.SetString("admin_" + Module.Name + "_sort", Request.Query["sort"].ToString());
            }
            else if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_sort")))
            {
                Module.Config.Sort = _session.GetString("admin_" + Module.Name + "_sort");
            }
            #endregion

            #region ------------- Date Search -------------
            if (Module.Config.EnableDateSearch == true)
            {
                if (!string.IsNullOrEmpty(Request.Query["after"]))
                {
                    var dateArr = Request.Query["after"].ToString().Split('/');
                    if (dateArr.Length == 3 && _utility.isInt(dateArr[0]) && _utility.isInt(dateArr[1]) && _utility.isInt(dateArr[2]))
                    {
                        _session.SetString("admin_" + Module.Name + "_after", string.Format("{0}-{1}-{2}", dateArr[2], dateArr[1], dateArr[0]));
                    }
                }
                else if (Request.Query.ContainsKey("after") && Request.Query["after"].ToString() == "")
                {
                    _session.Remove("admin_" + Module.Name + "_after");
                }

                if (!string.IsNullOrEmpty(Request.Query["before"]))
                {
                    var dateArr = Request.Query["before"].ToString().Split('/');
                    if (dateArr.Length == 3 && _utility.isInt(dateArr[0]) && _utility.isInt(dateArr[1]) && _utility.isInt(dateArr[2]))
                    {
                        _session.SetString("admin_" + Module.Name + "_before", string.Format("{0}-{1}-{2}", dateArr[2], dateArr[1], dateArr[0]));
                    }
                }
                else if (Request.Query.ContainsKey("before") && Request.Query["before"].ToString() == "")
                {
                    _session.Remove("admin_" + Module.Name + "_before");
                }
            }
            #endregion

            #region ------------- Field Search --------------
            if (Module.Config.FieldSearch != null && Module.Config.FieldSearch.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> FieldSearch in Module.Config.FieldSearch)
                {
                    if (!string.IsNullOrEmpty(Request.Query[FieldSearch.Key]))
                    {
                        _session.SetString("admin_" + Module.Name + "_search_" + FieldSearch.Key, Request.Query[FieldSearch.Key].ToString());
                    }
                    else if (Request.Query.ContainsKey(FieldSearch.Key) && Request.Query[FieldSearch.Key].ToString() == "")
                    {
                        _session.Remove("admin_" + Module.Name + "_search_" + FieldSearch.Key);
                    }
                }
            }
            #endregion

            return Module;
        }

        // แปลงค่าวันที่จากฟอร์ม (dd/MM/yyyy) ให้เป็นรูปแบบ YYYYMMDD (ปี พ.ศ.) เพื่อให้ ORDER BY ทำงานถูกต้อง
        // ใช้กับคอลัมน์ date_news / pb_date_news ของทุกโมดูลที่ใช้ตัวเลือกวันที่ (InputDate)
        public string ConvertDateNewsToYYYYMMDD(string input)
        {
            input = (input ?? "").Trim();
            if (input == "" || input.IndexOf('/') < 0) return input; // ว่าง หรือ เป็น YYYYMMDD อยู่แล้ว -> คงค่าเดิม

            string[] arr = input.Split('/');
            if (arr.Length != 3) return input;

            if (!int.TryParse(arr[0], out int d) || !int.TryParse(arr[1], out int m) || !int.TryParse(arr[2], out int y))
            {
                return input;
            }

            if (y < 2500) { y += 543; } // ค.ศ. -> พ.ศ.

            return y.ToString("D4") + m.ToString("D2") + d.ToString("D2");
        }

        public Dictionary<string, object>? setFieldsCreate(Module Module, IFormCollection f, bool defaultFields = true)
        {
            var fields = new Dictionary<string, object>();
            if (Module.Config.FieldCreate != null && Module.Config.FieldCreate.Count() > 0)
            {
                foreach (string field in Module.Config.FieldCreate)
                {
                    if (f.ContainsKey(field) && !string.IsNullOrEmpty(f[field]))
                    {
                        if (field == "form_id")
                        {
                            fields.Add(field, f[field].ToString());
                        }
                        else if (field == "id" || field.Contains("_id") || field == "round" || field == "highlight_status" || field == "suggest_status")
                        {
                            fields.Add(field, Convert.ToInt32(f[field]));
                        }
                        else if (field.EndsWith("_date"))
                        {
                            string dateFormat = "dd/MM/yyyy HH:mm";
                            if (field == "as_of_date")
                            {
                                dateFormat = "dd/MM/yyyy HH:mm:ss";
                            }
                            fields.Add(field, DateTime.ParseExact(f[field], dateFormat, null).ToUniversalTime().ToLocalTime());
                        }
                        else if (field == "date_news" || field == "pb_date_news")
                        {
                            fields.Add(field, ConvertDateNewsToYYYYMMDD(f[field].ToString()));
                        }
                        else
                        {
                            fields.Add(field, f[field].ToString());
                        }
                    }
                    else if (field == "created_at")
                    {
                        fields.Add(field, System.DateTime.Now);
                    }
                    else if (field == "updated_at")
                    {
                        fields.Add(field, System.DateTime.Now);
                    }
                    else if (field == "created_by")
                    {
                        fields.Add(field, _session.GetString("admin_name") ?? "");
                    }
                    else if (field == "updated_by")
                    {
                        fields.Add(field, _session.GetString("admin_name") ?? "");
                    }
                    else if (field == "sort")
                    {
                        fields.Add(field, getSort(Module));
                    }
                    else if (field == "status")
                    {
                        fields.Add(field, 1);
                    }
                    else if (field == "pb_status")
                    {
                        fields.Add(field, 0);
                    }
                    else if (field == "show_front")
                    {
                        fields.Add(field, 0);
                    }
                    else
                    {
                        //fields.Add(field, DBNull.Value);
                    }
                }
            }
            if (defaultFields == true)
            {
                if (!fields.ContainsKey("created_at"))
                {
                    fields.Add("created_at", System.DateTime.Now);
                }
                if (!fields.ContainsKey("updated_at"))
                {
                    fields.Add("updated_at", System.DateTime.Now);
                }
                if (!fields.ContainsKey("created_by"))
                {
                    fields.Add("created_by", _session.GetString("admin_name") ?? "");
                }
                if (!fields.ContainsKey("updated_by"))
                {
                    fields.Add("updated_by", _session.GetString("admin_name") ?? "");
                }
                if (!fields.ContainsKey("status"))
                {
                    fields.Add("status", 1);
                }
                if (!fields.ContainsKey("pb_status"))
                {
                    fields.Add("pb_status", 0);
                }
                if (!fields.ContainsKey("show_front"))
                {
                    fields.Add("show_front", 0);
                }
            }

            fields.Add("web_id", _currentWebID);

            if (Module.Config.TableModuleID != null && Module.Config.TableModuleID > 0)
            {
                fields.Add("module_id", Module.Config.TableModuleID);
            }

            #region ----- Manual id for non-identity tables -----
            // web_core_single.id is not an auto-identity column (unlike web_core_group),
            // so inserts must supply the next id explicitly.
            // Scoped strictly to the FooterPoll module — it is the only web_core_single
            // module with CanAdd enabled; all 34 others are edit-only (CanAdd = false).
            if (Module.Name == "FooterPoll" && Module.Config.Table == "web_core_single" && !fields.ContainsKey("id"))
            {
                var dtNextId = _db.ExecuteQuery("select coalesce(max(id), 0) + 1 as next_id from [2026_web_core_single]");
                int next_id = (dtNextId.Rows.Count > 0) ? Convert.ToInt32(dtNextId.Rows[0]["next_id"]) : 1;
                fields.Add("id", next_id);
            }
            #endregion

            return fields;
        }

        public Dictionary<string, object>? setFieldsUpdate(Module Module, IFormCollection f, bool defaultFields = true)
        {
            var fields = new Dictionary<string, object>();
            if (Module.Config.FieldUpdate != null && Module.Config.FieldUpdate.Count() > 0)
            {
                foreach (string field in Module.Config.FieldUpdate)
                {
                    if (f.ContainsKey(field) && !string.IsNullOrEmpty(f[field]))
                    {
                        if (field == "form_id")
                        {
                            fields.Add(field, f[field].ToString());
                        }
                        else if(field == "id" || field.Contains("_id") || field == "round" || field == "type" || field == "member_type" || field == "confirm_email" || field == "highlight_status" || field == "suggest_status")
                        {
                            fields.Add(field, Convert.ToInt32(f[field]));
                        }
                        else if (field.EndsWith("_date"))
                        {
                            string dateFormat = "dd/MM/yyyy HH:mm";
                            if (field == "as_of_date")
                            {
                                dateFormat = "dd/MM/yyyy HH:mm:ss";
                            }
                            fields.Add(field, DateTime.ParseExact(f[field], dateFormat, null).ToUniversalTime().ToLocalTime());
                        }
                        else if (field == "date_news" || field == "pb_date_news")
                        {
                            fields.Add(field, ConvertDateNewsToYYYYMMDD(f[field].ToString()));
                        }
                        else
                        {
                            fields.Add(field, f[field].ToString());
                        }
                    }
                    else if (f.ContainsKey(field) && string.IsNullOrEmpty(f[field]))
                    {
                        fields.Add(field, DBNull.Value);
                    }
                    else if (field == "created_at")
                    {
                        fields.Add(field, System.DateTime.Now);
                    }
                    else if (field == "updated_at")
                    {
                        fields.Add(field, System.DateTime.Now);
                    }
                    else if (field == "created_by")
                    {
                        fields.Add(field, _session.GetString("admin_name") ?? "");
                    }
                    else if (field == "updated_by")
                    {
                        fields.Add(field, _session.GetString("admin_name") ?? "");
                    }
                    else if (field == "status")
                    {
                        fields.Add(field, 1);
                    }
                    else if (field == "pb_status")
                    {
                        fields.Add(field, 0);
                    }
                    else
                    {
                        fields.Add(field, DBNull.Value);
                    }
                }
            }
            if (defaultFields == true)
            {
                if (!fields.ContainsKey("updated_at"))
                {
                    fields.Add("updated_at", System.DateTime.Now);
                }
                if (!fields.ContainsKey("updated_by"))
                {
                    fields.Add("updated_by", _session.GetString("admin_name") ?? "");
                }
                if (!fields.ContainsKey("pb_status"))
                {
                    fields.Add("pb_status", 0);
                }
            }
            return fields;
        }

        public Dictionary<string, string>? getSearchInputValue(Module Module)
        {
            var searchInputVal = new Dictionary<string, string>();
            if (Module.Config.FieldSearch != null && Module.Config.FieldSearch.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> FieldSearch in Module.Config.FieldSearch)
                {
                    if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key)) && FieldSearch.Value.Count > 0)
                    {
                        searchInputVal.TryAdd(FieldSearch.Key, _session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key) ?? "");
                    }
                }
            }
            return searchInputVal;
        }

        public bool showMoveTools(Module Module)
        {
            bool showMoveTools = false;
            if (Module.Config.CanMove == true)
            {
                var searchInputValue = getSearchInputValue(Module);
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
                {
                    string group_id = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                    if (Module.Config.OrderBy.ToLower() == "sort" && Module.Config.Sort.ToLower() == "asc" && group_id != "")
                    {
                        showMoveTools = true;
                    }
                    if (searchInputValue.ContainsKey(Module.Config.TableCateField))
                    {
                        searchInputValue.Remove(Module.Config.TableCateField);
                    }
                }
                else
                {
                    if (Module.Config.OrderBy.ToLower() == "sort" && Module.Config.Sort.ToLower() == "asc")
                    {
                        showMoveTools = true;
                    }
                }

                if (Module.Config.IsCMSPage == true && Module.Config.FieldCMSPage != "")
                {
                    searchInputValue.Remove(Module.Config.FieldCMSPage);
                }

                if (Module.Config.EnableDateSearch == true && (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_after")) || !string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_before"))))
                {
                    showMoveTools = false;
                }

                foreach (var item in searchInputValue)
                {
                    if (item.Value != "")
                    {
                        showMoveTools = false;
                    }
                }
            }
            return showMoveTools;
        }

        public int getSort(Module Module)
        {
            int default_sort = 10;

            if (Module != null && !string.IsNullOrEmpty(Module.Config.Table))
            {
                try
                {
                    if (Module.Config.TableModuleID != null && Convert.ToInt32(Module.Config.TableModuleID) > 0)
                    { 
                        if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
                        {
                            string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                            var dtSort = _db.ExecuteQuery(string.Format("select max(sort)+10 as max_sort from {0} where cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and module_id = '" + Module.Config.TableModuleID + "' and web_id = @web_id ", Module.Config.Table, Module.Config.TableCateField), new() { { "cate_val", cate_val }, { "web_id", _currentWebID } });
                         
                            if (dtSort.Rows.Count > 0)
                            { 
                                default_sort = Convert.ToInt32(dtSort.Rows[0]["max_sort"]);
                            }
                        }
                        else
                        {  
                            var dtSort = _db.ExecuteQuery(string.Format("select max(sort)+10 as max_sort from {0} where web_id = @web_id and module_id = '" + Module.Config.TableModuleID + "' ", Module.Config.Table), new Dictionary<string, object>() { { "web_id", _currentWebID } });
                         
                            if (dtSort.Rows.Count > 0)
                            { 
                                default_sort = Convert.ToInt32(dtSort.Rows[0]["max_sort"]);
                            }
                        }     
                    }
                    else if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
                    {
                        string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                        var dtSort = _db.ExecuteQuery(string.Format("select max(sort)+10 as max_sort from {0} where cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and web_id = @web_id ", Db.T(Module.Config.Table), Module.Config.TableCateField), new() { { "cate_val", cate_val }, { "web_id", _currentWebID } });
                        if (dtSort.Rows.Count > 0)
                        {
                            default_sort = Convert.ToInt32(dtSort.Rows[0]["max_sort"]);
                        }
                    }
                    else if (Module.Config.IsCMSPage == true && Module.Config.FieldCMSPage != "")
                    {
                        string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage) ?? "";
                        var dtSort = _db.ExecuteQuery(string.Format("select max(sort)+10 as max_sort from {0} where cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max))  and web_id = @web_id", Db.T(Module.Config.Table), Module.Config.FieldCMSPage), new() { { "cate_val", cate_val }, { "web_id", _currentWebID } });
                        if (dtSort.Rows.Count > 0)
                        {
                            default_sort = Convert.ToInt32(dtSort.Rows[0]["max_sort"]);
                        }
                    }
                    else
                    {
                        var dtSort = _db.ExecuteQuery(string.Format("select max(sort)+10 as max_sort from {0} where web_id = @web_id ", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "web_id", _currentWebID} });
                        if (dtSort.Rows.Count > 0)
                        {
                            default_sort = Convert.ToInt32(dtSort.Rows[0]["max_sort"]);
                        }
                    }
                }
                catch
                {
                    default_sort = 10;
                }
                return default_sort;
            }
            else
            {
                return default_sort;
            }
        }

        public void reSort(Module Module)
        {
            if (Module.Config.TableModuleID != null && Convert.ToInt32(Module.Config.TableModuleID) > 0)
            {
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
                {
                    string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                    var allRows = _db.ExecuteQuery(string.Format("select id from {0} where module_id = '" + Module.Config.TableModuleID + "' and cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and web_id = @web_id order by sort asc", Module.Config.Table, Module.Config.TableCateField), new Dictionary<string, object>() { { "cate_val", cate_val }, { "web_id", _currentWebID } }); 
                    if (allRows.Rows.Count > 0)
                    {
                        int sort = 10;
                        foreach (System.Data.DataRow row in allRows.Rows)
                        {
                            _db.Update(Module.Config.Table, " where id=@id ", new Dictionary<string, object>() { { "sort", sort } }, new Dictionary<string, object>() { { "id", Convert.ToInt32(row["id"]) } });
                            sort += 10;
                        }
                    }
                }
                else
                {
                    var allRows = _db.ExecuteQuery(string.Format("select id from {0} where module_id = '" + Module.Config.TableModuleID + "' and web_id = @web_id order by sort asc", Module.Config.Table), new() { { "web_id", _currentWebID } });
                    if (allRows.Rows.Count > 0)
                    {
                        int sort = 10;
                        foreach (System.Data.DataRow row in allRows.Rows)
                        {
                            _db.Update(Module.Config.Table, " where id=@id ", new Dictionary<string, object>() { { "sort", sort } }, new Dictionary<string, object>() { { "id", Convert.ToInt32(row["id"]) } });
                            sort += 10;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
            {
                string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                var allRows = _db.ExecuteQuery(string.Format("select id from {0} where cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and web_id = @web_id order by sort asc", Db.T(Module.Config.Table), Module.Config.TableCateField), new Dictionary<string, object>() { { "cate_val", cate_val }, { "web_id", _currentWebID } });
                if (allRows.Rows.Count > 0)
                {
                    int sort = 10;
                    foreach (System.Data.DataRow row in allRows.Rows)
                    {
                        _db.Update(Module.Config.Table, " where id=@id ", new Dictionary<string, object>() { { "sort", sort } }, new Dictionary<string, object>() { { "id", Convert.ToInt32(row["id"]) } });
                        sort += 10;
                    }
                }
            }
            else if (Module.Config.IsCMSPage == true && Module.Config.FieldCMSPage != "")
            {
                string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage) ?? "";
                var allRows = _db.ExecuteQuery(string.Format("select id from {0} where cast({1} as nvarchar(max)) = cast(@cate_val as nvarchar(max)) and web_id = @web_id order by sort asc", Db.T(Module.Config.Table), Module.Config.FieldCMSPage), new Dictionary<string, object>() { { "cate_val", cate_val }, { "web_id", _currentWebID } });
                if (allRows.Rows.Count > 0)
                {
                    int sort = 10;
                    foreach (System.Data.DataRow row in allRows.Rows)
                    {
                        _db.Update(Module.Config.Table, " where id=@id ", new Dictionary<string, object>() { { "sort", sort } }, new Dictionary<string, object>() { { "id", Convert.ToInt32(row["id"]) } });
                        sort += 10;
                    }
                }
            }
            else
            {
                var allRows = _db.ExecuteQuery(string.Format("select id from {0} where web_id = @web_id order by sort asc", Db.T(Module.Config.Table)), new() { { "web_id", _currentWebID } });
                if (allRows.Rows.Count > 0)
                {
                    int sort = 10;
                    foreach (System.Data.DataRow row in allRows.Rows)
                    {
                        _db.Update(Module.Config.Table, " where id=@id ", new Dictionary<string, object>() { { "sort", sort } }, new Dictionary<string, object>() { { "id", Convert.ToInt32(row["id"]) } });
                        sort += 10;
                    }
                }
            }
        }

        public int getChildCount(Module Module, int id = 0)
        {
            var dt = _db.ExecuteQuery(string.Format("SELECT COUNT(id) as count_item from {0} WHERE cast({1} as nvarchar(max)) = cast(@id as nvarchar(max)) and web_id = @web_id ", Db.T(Module.Config.Table), Module.Config.FieldCMSPage), new() { { "id", id }, { "web_id", _currentWebID } });
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["count_item"]);
            }
            else
            {
                return 0;
            }
        }

        public int getParentID(Module Module, int id = 0)
        {
            var dt = _db.ExecuteQuery(string.Format("SELECT top 1 {1} as parnt_id from {0} WHERE cast(id as nvarchar(max)) = cast(@id as nvarchar(max)) and web_id = @web_id", Db.T(Module.Config.Table), Module.Config.FieldCMSPage), new() { { "id", id }, { "web_id", _currentWebID } });
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["parnt_id"]);
            }
            else
            {
                return 0;
            }
        }

        public List<Dictionary<string, object>> getUser()
        {
            var data = _db.ExecuteQuery("select id, access_id, name, surname from [2026_web_admin] where web_id = @web_id", new() { { "web_id", _currentWebID } });
            var dict = data.AsEnumerable().Select(
                row => data.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,    // Key
                    column => row[column] as object // Value
                )
            ).ToList();
            return dict;
        }

        public Dictionary<string, object>? WebInfo(int webID, bool checkShowFrontAndStatus = true, bool checkDate = true)
        {
            if (webID == 0)
            {
                return new Dictionary<string, object>()
                {
                    { "id", 0 },
                    { "title", "เว็บไซต์หลัก" }
                };
            }

            var sql_param = new Dictionary<string, object>() { { "id", webID } };

            string sqlShowFrontAndStatus = checkShowFrontAndStatus ? "and status = 1 and show_front = 1" : "";
            string sqlCheckDate = checkDate ? "and SYSDATETIMEOFFSET() >= pb_issue_date and SYSDATETIMEOFFSET() <= pb_expiry_date" : "";

            string sql = $"select top 1 * from [2026_web_microsite] where id = @id {sqlShowFrontAndStatus} {sqlCheckDate}";
            var webInfo = _db.ExecuteQuery(sql, sql_param);
            if (webInfo.Rows.Count > 0)
            {
                var web = webInfo.Rows[0];
                return new Dictionary<string, object>()
                {
                    { "id", Convert.ToInt32(web["id"]) },
                    { "title", web["title"] + "" },
                    // pb_url = slug ที่ front-end ใช้ resolve microsite (URL หน้าแรก = {FrontURL}/th/{pb_url})
                    { "url", webInfo.Columns.Contains("pb_url") ? web["pb_url"] + "" : "" }
                };
            }
            else
            {
                return null;
            }
        }

        public void SetSessionWebID()
        {
            _currentWebID = _session.GetInt32("admin_web_id") ?? -1;
        }

        #region Input function
        public string InputText(string input_label, string input_name, string input_value, string input_placeholder = "", int maxlength = 255)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{5}"" class=""form-label"">{0}</label>
	                <input type=""text"" class=""form-control"" id=""{5}"" name=""{1}"" value=""{2}"" placeholder=""{3}"" maxlength=""{4}"">
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_placeholder,
                maxlength,
                "id_" + input_name);
        }

        public string InputTextLink(string input_label, string input_name, string input_value, string input_placeholder = "", int maxlength = 8000)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{5}"" class=""form-label"">{0}</label>
	                <input type=""text"" class=""form-control"" id=""{5}"" name=""{1}"" value=""{2}"" placeholder=""{3}"" maxlength=""{4}""> 
                    <a href=""javascript:OpenLinkHelper('{5}');"">[Link Helper]</a>
                </div>";
 
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_placeholder,
                maxlength,
                "id_" + input_name);
        }

        public string InputTextLinkCMSPage(string input_label, string input_name, string input_value, string input_placeholder = "", int maxlength = 8000)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{5}"" class=""form-label"">{0}</label>
	                <input type=""text"" class=""form-control"" id=""{5}"" name=""{1}"" value=""{2}"" placeholder=""{3}"" maxlength=""{4}""> 
                    <a href=""javascript:OpenLinkCMSPageHelper('{5}');"">[Link Helper]</a>
                </div>";

            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_placeholder,
                maxlength,
                "id_" + input_name);
        }

        public string InputTextLinkCMSPage2(string input_label, string input_name, string input_value, string input_placeholder = "", int maxlength = 8000)
        {
            string help_main = "หน้าแรก,เกี่ยวกับเรา,ทรัพย์สินรอการขาย,บริหารหนี้ด้อยคุณภาพ,ข่าวสารและประกาศ,ร่วมงานกับเรา,ติดต่อเรา,อื่นๆ";
            string[] help_sub = new string[99];

            help_sub[0] = "หน้าแรก$home";

            help_sub[1] = @"
            วิสัยทัศน์/พันธกิจ$abo-vis,
            ประวัติความเป็นมา$abo-his,
            โครงสร้างองค์กร$abo-org,
            รายงานประจำปี$abo-ann,
            รายงานการเงิน$abo-fin,
            คณะกรรมการบริษัท$abo-boa,
            คณะกรรมการอื่นๆ$abo-com,
            คณะผู้บริหารระดับสูง$abo-boe,
            คณะผู้บริหารฝ่าย$abo-vic,
            การบริหารสินทรัพย์ด้อยคุณภาพ บสส.$bus-man,
            การบริหารทรัพย์สินรอการขาย$bus-ass,
            การสร้างโอกาสทางธุรกิจ บสส.$bus-opp";

            help_sub[2] = @"
            ค้นหาทรัพย์สิน$npa-sea,
            ประเภททรัพย์$npa-ass,
            โปรโมชั่นทรัพย์เด่น$pro,
            ขั้นตอนการซื้อทรัพย์$npa-pro";

            help_sub[3] = @"
            ภาพรวมการบริหารหนี้$dep-ove,
            ขั้นตอนการปรับโครงสร้างหนี้$dep-res,
            สิทธิประโยชน์ลูกหนี้$dep-ben,
            ดาวน์โหลดแบบฟอร์ม / เอกสารที่เกี่ยวข้อง$dep-dow,
            ลงทะเบียนปรับโครงสร้างหนี้$dep-reg,
            คลินิกแก้หนี้$dep-cli";

            help_sub[4] = @"
            ข่าวประชาสัมพันธ์$new,
            บทความและวารสาร$art,
            วิดีโอ / สื่อประชาสัมพันธ์$vid,
            อัตราดอกเบี้ย$ann-int,
            จัดซื้อจัดจ้าง$ann-pro,
            NPL/NPA$ann-npl,
            ทั่วไป$ann-gen";

            help_sub[5] = @"
            ตำแหน่งงานว่าง$car,
            วัฒนธรรมองค์กร$car2";

            help_sub[6] = @"
            สำนักงานใหญ่ / สาขา$con,
            ช่องทางร้องเรียน / ข้อเสนอแนะ$sug,
            ดาวน์โหลดแบบฟอร์ม$doc-dow,
            คำถามที่พบบ่อย$faq-cor,
            หน่วยงานที่เกี่ยวข้อง$con-rel,
            แลกเปลี่ยนลิงค์$lin";

            help_sub[7] = @"
            สมัครสมาชิก$mem-reg,
            เข้าสู่ระบบ$mem-log,
            ค้นหาในเว็บไซต์$sea,
            ค้นหาทรัพย์ตามแผนที่$sea-map,
            จับคู่ความต้องการทรัพย์สิน NPA$npa-mat,
            ทรัพย์ขายทอดตลาดของกรมบังคับคดี$npa-leg,
            แผนผังเว็บไซต์$sit,
            แบบสำรวจ$sur,
            สำหรับเจ้าหน้าที่$sta,
            คำนวณสินเชื่อ$cal,
            ประกาศความเป็นส่วนตัว$policy1,
            ข้อตกลงและเงื่อนไขการใช้บริการอิเล็กทรอนิกส์$policy2,
            การเปิดเผยข้อมูลและความโปร่งใสในการดำเนินงาน$ope";

            string text_option = "";
            string[] help_main_arr = help_main.Split(',');
            int j = 0;
            foreach (string item in help_main_arr)
            {
                text_option = text_option + "<optgroup label='" + item + "'>";
                string[] help_sub_arr = help_sub[j].Split(',');
                foreach (string item_sub in help_sub_arr)
                {
                    string[] item_sub_2 = item_sub.Split('$');
                    text_option = text_option + "<option value='" + item_sub_2[1] + "'>" + @item_sub_2[0] + "</option>";
                }
                text_option = text_option + "</optgroup>";
                j++;
            }
 
            string html = @"<div class='field'><label for='id_" + input_name + "' class='form-label'>" + input_label + "</label><select class='form-select' id='id_" + input_name + "'  name='" + input_name + "'>" + text_option + "</select></div>";

            if(input_value != "")
            {
                html = html + "<script>document.getElementById('id_" + input_name + "').value = '" + input_value + "'</script>";
            }
 
            return html;
        }

        public string InputDateTime(string input_label, string input_name, string input_value, bool use_second = false)
        {
            string update_value = "";
            if (input_value.IndexOf('/') > -1)
            {
                update_value = input_value;
            }
            else
            {
                string y1 = input_value.Substring(0, 4);
                string m1 = input_value.Substring(4, 2);
                string d1 = input_value.Substring(6, 2);

                update_value = d1 + "/" + m1 + "/" + y1;
            }

            string html = @"
                <div class=""field"">
	                <label for=""{3}"" class=""form-label"">{0}</label>
	                <input type=""text"" class=""form-control {4}"" id=""{3}"" name=""{1}"" value=""{2}"" >
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(update_value),
                "id_" + input_name,
                use_second ? "cms_datetime_s" : "cms_datetime"
                );
        }

        public string InputDate(string input_label, string input_name, string input_value)
        {
            string update_value = "";
            if(input_value.IndexOf('/') > -1)
            {
                update_value = input_value;
            }
            else
            {
                string y1 = input_value.Substring(0, 4);
                string m1 = input_value.Substring(4, 2);
                string d1 = input_value.Substring(6, 2);

                update_value = d1+"/"+m1+"/"+y1;
            }
            
            string html = @"
                <div class=""field"">
	                <label for=""{3}"" class=""form-label"">{0}</label>
	                <input type=""text"" class=""form-control cms_date"" id=""{3}"" name=""{1}"" value=""{2}"" >
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(update_value),
                "id_" + input_name);
        }

        public string InputColor(string input_label, string input_name, string input_value = "#0146B2")
        {
            string html = @"
                <div class=""field"">
	                <label for=""{3}"" class=""form-label"">{0}</label>
	                <input type=""color"" class=""form-control form-control-color"" id=""{3}"" name=""{1}"" value=""{2}"" placeholder=""{3}"">
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                "id_" + input_name);
        }

        // ป้องกันการแทรกไฟล์ CSS/JS ของตัวเลือกไอคอนซ้ำในหน้าเดียวกัน
        private bool _bsIconAssetsEmitted = false;

        // อินพุตสำหรับเลือกไอคอน Bootstrap Icons (เก็บค่าเช่น "bi bi-telephone")
        // มีปุ่มเปิดโมดอลให้เลือกไอคอนจากรายการทั้งหมดของ Bootstrap Icons
        public string InputBootstrapIcon(string input_label, string input_name, string input_value)
        {
            string id = "id_" + input_name;
            string val = (input_value ?? "").Trim();

            // คลาสไอคอนสำหรับรูปตัวอย่าง — ถ้ายังไม่ได้เลือกให้แสดงไอคอนจาง
            string previewClass = val.Contains("bi-") ? val : "bi bi-emoji-neutral text-muted";
            bool hasValue = val.Contains("bi-");

            string assets = "";
            if (!_bsIconAssetsEmitted)
            {
                _bsIconAssetsEmitted = true;
                assets = @"
                <link rel=""stylesheet"" href=""/css/Admin/bootstrap-icon-picker.css"" />
                <script src=""/js/bootstrap-icons-list.js""></script>
                <script src=""/js/Admin/bootstrap-icon-picker.js""></script>";
            }

            string html = @"
                {6}
                <div class=""field"">
                    <label for=""{0}"" class=""form-label"">{1}</label>
                    <div class=""input-group"">
                        <span class=""input-group-text bsicon-preview""><i id=""{0}_preview"" class=""{4}""></i></span>
                        <input type=""text"" class=""form-control"" id=""{0}"" name=""{2}"" value=""{3}"" placeholder=""ยังไม่ได้เลือกไอคอน"" oninput=""refreshBsIconPreview('{0}')"">
                        <button class=""btn btn-danger"" type=""button"" id=""{0}_del"" style=""{5}"" onclick=""clearBsIcon('{0}')""><i class=""bi bi-trash""></i></button>
                        <button class=""btn btn-warning"" type=""button"" onclick=""openBsIconPicker('{0}')""><i class=""bi bi-grid-3x3-gap-fill""></i> เลือกไอคอน</button>
                    </div>
                </div>
                <script>refreshBsIconPreview('{0}');</script>";

            return string.Format(html,
                id,                                              // {0}
                input_label,                                     // {1}
                input_name,                                      // {2}
                System.Web.HttpUtility.HtmlEncode(val),          // {3}
                previewClass,                                    // {4}
                hasValue ? "" : "display:none;",                 // {5}
                assets);                                         // {6}
        }

        public string InputEmail(string input_label, string input_name, string input_value, string input_placeholder = "name@example.com", int maxlength = 8000)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{5}"" class=""form-label"">{0}</label>
	                <input type=""email"" class=""form-control"" id=""{5}"" name=""{1}"" value=""{2}"" placeholder=""{3}"" maxlength=""{4}"">
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_placeholder,
                maxlength,
                "id_" + input_name);
        }

        public string InputPassword(string input_label, string input_name, string input_value, string input_placeholder = "", int maxlength = 8000)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{5}"" class=""form-label"">{0}</label>
	                <input type=""password"" class=""form-control"" id=""{5}"" name=""{1}"" value=""{2}"" placeholder=""{3}"" maxlength=""{4}"">
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_placeholder,
                maxlength,
                "id_" + input_name);
        }

        public string InputTextArea(string input_label, string input_name, string input_value, int input_row = 3)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{4}"" class=""form-label"">{0}</label>
	                <textarea class=""form-control"" id=""{4}"" name=""{1}"" rows=""{3}"">{2}</textarea>
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_row,
                "id_" + input_name);
        }

        public string InputTextArea2(string input_label, string input_name, string input_value, int input_row = 3)
        {
            string html = @"
                <div class=""field"">
	                <label for=""{4}"" class=""form-label"">{0}</label>
	                <textarea style='height:900px;font-size:14px;font-family:courier new' class=""form-control"" id=""{4}"" name=""{1}"" rows=""{3}"">{2}</textarea>
                </div>";
            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                input_row,
                "id_" + input_name);
        }

        public string InputTextEditor(string input_label, string input_name, string input_value)
        {
            string html = @"
            <div class=""mb-3"">
	            <label for=""{3}"" class=""form-label"">{0}</label>
	            <textarea class=""form-control"" id=""{3}"" name=""{1}"" rows=""8"" >{2}</textarea>
            </div>
            <script>
            innitCK(""{3}"");
            </script>";

            return string.Format(html,
                input_label,
                input_name,
                input_value,
                "id_" + input_name
                );
        }

        public string InputTextEditor_Eform(string input_label, string input_name, string input_value)
        {
            string html = @"
            <div class=""mb-3"">
	            <label for=""{3}"" class=""form-label"">{0}</label>
	            <textarea class=""form-control"" id=""{3}"" name=""{1}"" rows=""8"" >{2}</textarea>
                <div align='center'><a href=""javascript:OpenAddForm('{3}')"">[Insert E-Form]</a></div>
            </div>
            
            <script>
            innitCK(""{3}"");
            </script>";

            return string.Format(html,
                input_label,
                input_name,
                input_value,
                "id_" + input_name
                );
        }

        public string InputFileManeger(string input_label, string input_name, string input_value, string only_mimes = "image")
        {
            string html = @"
                <div class=""field"">
                    <label for=""{3}"" class=""form-label"">{0}</label>
                    <div class=""input-group"">
	                    <input type=""text"" class=""form-control"" id=""{3}"" name=""{1}"" value=""{2}"">
		                <button class=""btn btn-danger"" type=""button"" id=""{3}_del"" style=""display:none;"" onClick=""removeFileFM('{3}', $(this))""> <i class=""bi bi-trash""></i> </button>
		                <button class=""btn btn-warning"" data-only-mimes=""{4}"" type=""button"" onclick=""openElFinder('{3}', $(this))""><i class=""bi bi-folder-plus""></i></button>
                    </div>
                    <div class=""show-img"" id=""{3}_img_section"">
                        <img id=""{3}_thumbnail"" class=""img-thumbnail"">
                        <video id=""{3}_thumbnail2"" class=""video-thumbnail"" controls>
	                        <source src="""" type=""video/mp4"">
	                        Your browser does not support the video tag.
                        </video>
                    </div>
                </div>

                <script>
                    checkFileFM(""{3}"");
                </script>";

            return string.Format(html,
                input_label,
                input_name,
                System.Web.HttpUtility.HtmlEncode(input_value),
                "id_" + input_name,
                only_mimes
                );
        }

        public string InputFileManegerMulti(string input_label, string input_name, string input_value, string only_mimes = "image")
        {
            string html = "";
            string html_main = "";
            string last_input_value = "";

            if (input_value.IndexOf(",") > -1)
            {
                input_value = input_value + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            }

            for (int i=1;i<=20;i++)
            {
                html = @"
                    <span id='span_{1}' style='display:none;'>
                    <div class=""field"">
                    <label for=""{3}"" class=""form-label"">{0}</label>
                    <div class=""input-group"">
	                    <input type=""text"" class=""form-control"" id=""{3}"" name=""{5}"" value=""{2}"" readonly>
		                <button class=""btn btn-danger"" type=""button"" id=""{3}_del"" style=""display:none;"" onClick=""removeFileFM('{3}', $(this))""> <i class=""bi bi-trash""></i> </button>
		                <button class=""btn btn-warning"" data-only-mimes=""{4}"" type=""button"" onclick=""openElFinder('{3}', $(this))""><i class=""bi bi-folder-plus""></i></button>
                    </div>
                    <div class=""show-img"" id=""{3}_img_section"">
                        <img id=""{3}_thumbnail"" class=""img-thumbnail"">
                        <video id=""{3}_thumbnail2"" class=""video-thumbnail"" controls>
	                        <source src="""" type=""video/mp4"">
	                        Your browser does not support the video tag.
                        </video>
                    </div>
                    </div><br>
                    </span>

                    <script>
                    checkFileFM(""{3}"");
                    </script>";

                last_input_value = input_value;

                if (input_value.IndexOf(",") > -1)
                {
                    string[] arr_input_value = input_value.Split(",");
                    if (arr_input_value[i-1] != "")
                    {
                        last_input_value = arr_input_value[i-1]; 
                    }
                    else
                    {
                        last_input_value = ""; 
                    }
                }

                /*
                if (input_value.IndexOf(",") > -1)
                {
                    string[] arr_input_value = input_value.Split(",");
                    if (arr_input_value[1] != "")
                    {
                        input_value = arr_input_value[1];
                    }
                    else
                    {
                        input_value = "";
                    }
                }     
                else
                {
                    input_value = "";
                } 
                */

                html = string.Format(html, "#"+i+" "+input_label+"", input_name+"_"+i, System.Web.HttpUtility.HtmlEncode(last_input_value), "id_"+input_name+"_"+i, only_mimes, input_name);
                html_main = html_main + html;
            }

            return html_main;
        }

        public string InputFileManegerMultiEdit(string input_label, string input_name, string input_value, string only_mimes = "image")
        {
            string html = "";
            string html_main = "";
            string last_input_value = "";
             
            if(true)
            //if (input_value.IndexOf(",") > -1)
            {
                input_value = input_value + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            }

            for (int i = 1; i <= 20; i++)
            {
                html = @"
                    <span id='span_{1}' style='display:none;'>
                    <div class=""field"">
                    <label for=""{3}"" class=""form-label"">{0}</label>
                    <div class=""input-group"">
	                    <input type=""text"" class=""form-control"" id=""{3}"" name=""{5}"" value=""{2}"" readonly>
		                <button class=""btn btn-danger"" type=""button"" id=""{3}_del"" style=""display:none;"" onClick=""removeFileFM('{3}', $(this))""> <i class=""bi bi-trash""></i> </button>
		                <button class=""btn btn-warning"" data-only-mimes=""{4}"" type=""button"" onclick=""openElFinder('{3}', $(this))""><i class=""bi bi-folder-plus""></i></button>
                    </div>
                    <div class=""show-img"" id=""{3}_img_section"">
                        <img id=""{3}_thumbnail"" class=""img-thumbnail"">
                        <video id=""{3}_thumbnail2"" class=""video-thumbnail"" controls>
	                        <source src="""" type=""video/mp4"">
	                        Your browser does not support the video tag.
                        </video>
                    </div>
                    </div><br>
                    </span>

                    <script>
                    checkFileFM(""{3}"");
                    </script>";

                last_input_value = input_value;

                if(true)
                //if (input_value.IndexOf(",") > -1)
                {
                    string[] arr_input_value = input_value.Split(",");
                    if (arr_input_value[i - 1] != "")
                    {
                        last_input_value = arr_input_value[i - 1];
                    }
                    else
                    {
                        last_input_value = "";
                    }
                }

                /*
                if (input_value.IndexOf(",") > -1)
                {
                    string[] arr_input_value = input_value.Split(",");
                    if (arr_input_value[1] != "")
                    {
                        input_value = arr_input_value[1];
                    }
                    else
                    {
                        input_value = "";
                    }
                }     
                else
                {
                    input_value = "";
                } 
                */

                html = string.Format(html, "#" + i + " " + input_label + "", input_name + "_" + i, System.Web.HttpUtility.HtmlEncode(last_input_value), "id_" + input_name + "_" + i, only_mimes, input_name);
                html_main = html_main + html;
            }

            return html_main;
        }

        public string InputUrlTarget(string input_label, string input_name, string input_value)
        {
            string _self = (input_value == "" || input_value == "_top") ? "checked" : "";
            string _blank = (input_value == "_blank") ? "checked" : "";
            string html = @"
                <div class=""field"">
                    <label class=""form-label mb-2"">{0}</label>
                    <div class=""row"">
                        <div class=""col-md-6 col-sm-6 col-6"">
	                        <div class=""form-check"">
		                        <input class=""form-check-input"" type=""radio"" name=""{1}"" id=""id_{1}_1"" value=""_top"" {2}>
		                        <label class=""form-check-label"" for=""id_{1}_1"">
		                            Redirect
		                        </label>
	                        </div>
                        </div>
                        <div class=""col-md-6 col-sm-6 col-6"">
	                        <div class=""form-check"">
		                        <input class=""form-check-input"" type=""radio"" name=""{1}"" id=""id_{1}_2"" value=""_blank"" {3}>
		                        <label class=""form-check-label"" for=""id_{1}_2"">
		                            New tab
		                        </label>
	                        </div>
                        </div>
                    </div>
                </div>";
            return string.Format(html, input_label, input_name, _self, _blank);
        }
        
        public string InputSelectDB(string input_label, string input_name, string input_value, bool showAllOption = false, string table_name = "", string value_field = "id", string text_field = "title", string order_by = "id", string sort = "asc", string txtAllOption = "ทั้งหมด", string ModuleID = "")
        {
            string selectOption = "";
            if (showAllOption)
            {
                selectOption = "<option value=\"\">" + txtAllOption + "</option>";
            }
            string q_module_id = "";
            if(ModuleID != "")
            {
                q_module_id = " and module_id = '" + ModuleID + "' ";
            }
            var optionDT = _db.ExecuteQuery(string.Format("select {0}, {1} from {2} where web_id = @web_id " + q_module_id + " order by {3} {4}", value_field, text_field, table_name, order_by, sort), new() { { "web_id", _currentWebID } });
            if (optionDT.Rows.Count > 0)
            {
                foreach (System.Data.DataRow item in optionDT.Rows)
                {
                    string selected = (item[value_field] + "" == input_value) ? "selected" : "";
                    selectOption += string.Format("<option value=\"{0}\" {2}>{1}</option>", item[value_field], item[text_field], selected);
                }
            }
            string html = @"
            <div class=""field"">
	        <label for=""{0}"" class=""form-label"">{1}</label>
	        <select class=""form-select"" id=""{0}"" name=""{2}"">
		        {3}
	        </select>
            </div>";
            return string.Format(html,
                "id_" + input_name,
                input_label,
                input_name,
                selectOption
                );
        }

        public string InputSelectDBcat(string input_label, string input_name, string input_value, bool showAllOption = false, string table_name = "", string value_field = "id", string text_field = "title", string order_by = "id", string sort = "asc", string txtAllOption = "ทั้งหมด", string ModuleID = "")
        {
            string selectOption = "";
            if (showAllOption)
            {
                selectOption = "<option value=\"\">" + txtAllOption + "</option>";
            }
            string q_module_id = "";
            if (ModuleID != "")
            {
                q_module_id = " and module_id = '" + ModuleID + "' ";
            }
            var optionDT = _db.ExecuteQuery(string.Format("select {0}, {1}, cat_id from {2} where web_id = @web_id " + q_module_id + " order by cat_id asc,sort asc", value_field, text_field, table_name, order_by, sort), new() { { "web_id", _currentWebID } });
            if (optionDT.Rows.Count > 0)
            {
                foreach (System.Data.DataRow item in optionDT.Rows)
                {
                    var optionDT2 = _db.ExecuteQuery(string.Format("select title from {0} where id = @id", Db.T(table_name)), new() { { "id", item["cat_id"] } });
                    string txt1 = "";
                    if (optionDT2.Rows.Count > 0)
                    {
                        txt1 = optionDT2.Rows[0]["title"]+"";
                    }
                    string selected = (item[value_field] + "" == input_value) ? "selected" : "";
                    selectOption += string.Format("<option value=\"{0}\" {2}>{3} / {1}</option>", item[value_field], item[text_field], selected, txt1);
                }
            }
            string html = @"
            <div class=""field"">
	        <label for=""{0}"" class=""form-label"">{1}</label>
	        <select class=""form-select"" id=""{0}"" name=""{2}"">
		        {3}
	        </select>
            </div>";
            return string.Format(html,
                "id_" + input_name,
                input_label,
                input_name,
                selectOption
                );
        }

        public string InputSelectDB_WG(string input_label, string input_name, string input_value, bool showAllOption = false, string table_name = "", string value_field = "id", string text_field = "title", string order_by = "id", string sort = "asc", string txtAllOption = "ทั้งหมด", string add_new_url = "", string module_id = "")
        {
            string selectOption = "";
            if (showAllOption)
            {
                selectOption = "<option value=\"\">" + txtAllOption + "</option>";
            }
            string sql_for_module_id = "";
            if(module_id != "")
            {
                sql_for_module_id = " and module_id = '" + module_id + "' ";
            }
            var optionDT = _db.ExecuteQuery(string.Format("select {0}, {1} from {2} where web_id = @web_id order by {3} {4}", value_field, text_field, table_name, order_by, sort), new() { { "web_id", _currentWebID } });
            if (optionDT.Rows.Count > 0)
            {
                foreach (System.Data.DataRow item in optionDT.Rows)
                {
                    string selected = (item[value_field] + "" == input_value) ? "selected" : "";
                    selectOption += string.Format("<option value=\"{0}\" {2}>{1}</option>", item[value_field], item[text_field], selected);
                }
            }
 
            string html = @"
            <div class=""field"">
	        <label for=""{0}"" class=""form-label"">{1}</label>
	        <select class=""form-select"" id=""{0}"" name=""{2}"">
		        {3}
	        </select>
            <a href='/Admin/{4}/Create' target='_blank' style='text-decoration:underline;color:#666'>[Add]</a>
            <a href='javascript:;' onclick=""goto_edit('{4}',document.getElementById('{0}').value)"" style='text-decoration:underline;color:#666'>[Edit]</a>
            <a href='javascript:;' onclick='location.reload();' style='text-decoration:underline;color:#666'>[Refresh]</a>
            </div>
            ";
            return string.Format(html,
                "id_" + input_name,
                input_label,
                input_name,
                selectOption,
                add_new_url
                );
        }

        public string InputSelectDB_Product(string input_label, string input_name, string input_value, bool showAllOption = false, string table_name = "", string value_field = "id", string text_field = "title", string order_by = "id", string sort = "asc", string txtAllOption = "ทั้งหมด")
        {
            string selectOption = "";
            if (showAllOption)
            {
                selectOption = "<option value=\"\">" + txtAllOption + "</option>";
            }
            var optionDT = _db.ExecuteQuery(string.Format("select * from {2} where web_id = @web_id order by cat_id ASC,sort ASC", value_field, text_field, table_name, order_by, sort), new() { { "web_id", _currentWebID } });
            if (optionDT.Rows.Count > 0)
            {
                foreach (System.Data.DataRow item in optionDT.Rows)
                { 
                    var optionDT_cat = _db.ExecuteQuery(string.Format("select title from web_product_main where id = @id and web_id = @web_id ", value_field, text_field, table_name, order_by, sort), new() { { "id", Convert.ToInt32(item["cat_id"]) }, { "web_id", _currentWebID } });

                    string this_main = "";
                    if(optionDT_cat.Rows.Count > 0)
                    {
                        this_main = optionDT_cat.Rows[0]["title"].ToString();
                    }

                    string selected = (item[value_field] + "" == input_value) ? "selected" : "";
                    selectOption += string.Format("<option value=\"{0}\" {2}>{1}</option>", item[value_field], this_main + " : " + item[text_field], selected);
                }
            }
            string html = @"
            <div class=""field"">
	        <label for=""{0}"" class=""form-label"">{1}</label>
	        <select class=""form-select"" id=""{0}"" name=""{2}"">
		        {3}
	        </select>
            </div>";
            return string.Format(html,
                "id_" + input_name,
                input_label,
                input_name,
                selectOption
                );
        }

        public string InputNumber(string input_label, string input_name, int min_value, int max_value, decimal input_value)
        {
            // Sanitize input values to protect against XSS
            input_label = System.Web.HttpUtility.HtmlEncode(input_label);
            input_name = System.Web.HttpUtility.HtmlEncode(input_name);

            // Create the HTML string for the number input element with label, min, max, and initial value
            string html = $"<div class=\"field\"><label for=\"id_{input_name}\" class=\"form-label\">{input_label}</label>";
            html += $"<input class=\"form-control\" type=\"number\" name=\"{input_name}\" id=\"id_{input_name}\" value=\"{input_value}\" min=\"{min_value}\" max=\"{max_value}\"></div>";

            return html;
        }

        public string GetCatTitle(string cat_table, string id)
        {
            string cat_title = "";
             
            var dt = _db.ExecuteQuery("select title from " + cat_table + " where id = '" + id + "'"); 
            if (dt.Rows.Count > 0)
            {
                cat_title = dt.Rows[0]["title"]+""; 
            }

            return cat_title;
        }

        // หา id ของหมวด (web_core_group ฯลฯ) จากชื่อที่ตรงเป๊ะ ภายใต้ module_id เดียวกัน
        // ใช้แทนการ hardcode id ในวิว (เช่น หมวด "ประกาศเชิญชวน" ของประกาศจัดซื้อจัดจ้าง)
        // คืนค่าว่างเมื่อไม่พบ — ผู้เรียกต้องเช็คก่อนใช้
        public string GetCatIdByTitle(string cat_table, int module_id, string title)
        {
            var dt = _db.ExecuteQuery(
                string.Format("select top 1 id from {0} where web_id = @web_id and module_id = @module_id and title = @title order by id", Db.T(cat_table)),
                new() { { "web_id", _currentWebID }, { "module_id", module_id }, { "title", title } });

            return (dt.Rows.Count > 0) ? dt.Rows[0]["id"] + "" : "";
        }

        public string ViewTR(string label, string value, string classString = "")
        {
            string html = @"
                <tr>
	                <td><div class=""table-title {2}"">{0}</div></td>
	                <td><div class=""table-desc"">{1}</div></td>
                </tr>";
            return string.Format(html,
                label,
                value,
                classString
                );
        }


        #endregion


    }
}
