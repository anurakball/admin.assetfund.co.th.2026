using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _context;
        private readonly ISession _session;
        private readonly Utility _utility;
        private readonly DBHelper _db;
        private readonly AdminHelpers _admin;
        private readonly IAzureAdAuthService _azureAd;

        //----- ประเภทการ Login (web_admin.login_type)
        private const int LOGIN_TYPE_NORMAL = 1;   // แบบปกติ    : ตรวจรหัสผ่านในระบบ (SHA512) — พฤติกรรมเดิมทั้งหมด
        private const int LOGIN_TYPE_EMPLOYEE = 2; // แบบพนักงาน : ตรวจ Username/Password กับ Azure AD (OAuth2/OIDC)

        private int _requestWebID = 0;
        private int _currentWebID = -1;

        public UserController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext, IAzureAdAuthService azureAd)
        {
            _azureAd = azureAd;
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig);
            _admin = new AdminHelpers(_hostingEnvironment, _config, _context);

            _currentWebID = _admin._currentWebID;
        }

        private bool CheckWeb(string webID)
        {
            try
            {
                if (!_utility.isInt(webID))
                {
                    return false;
                }

                var webInfo = _admin.WebInfo(Convert.ToInt32(webID), false, false);
                if (webInfo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // GET: Admin/User
        [AdminLogin]
        public IActionResult Index()
        {
            return Redirect("/Admin/User/Dashboard");
            //return View("~/Areas/Admin/Views/User/Index.cshtml");
        }

        [AdminNotLogin]
        public IActionResult Login(string targetUrl = "", string webID = "")
        {
            if (targetUrl != "")
            {
                _session.SetString("admin_login_redirect", targetUrl);
            }

            #region ----- web id -----
            ViewBag.webID = "";
            ViewBag.webTitle = "";
            if (webID != null && webID.Trim() != "")
            {
                if (!CheckWeb(webID))
                {
                    return RedirectToAction("Login", new { targetUrl = targetUrl });
                }
                else
                {
                    var webInfo = _admin.WebInfo(Convert.ToInt32(webID), false, false);
                    if (webInfo != null)
                    {
                        ViewBag.webID = webID;
                        ViewBag.webTitle = webInfo["title"] + "";
                    }
                }
            }
            #endregion

            #region ----- all microsite -----
            ViewBag.AllMicrosite = null;
            //----- แสดง microsite ทั้งหมด (ไม่กรอง approve/ช่วงวันที่) ให้ผู้ใช้เลือกไซต์ตอน login
            //      ใช้ pb_title (ค่าที่อนุมัติแล้ว) ถ้าว่างให้ fallback เป็น title
            string sql = $"select id, case when coalesce(pb_title, '') <> '' then pb_title else title end as pb_title from web_microsite where id > 0 order by title asc";
            var selectSite = _db.ExecuteQuery(sql);
            if (selectSite.Rows.Count > 0)
            {
                var AllMicrosite = selectSite.Rows;
                ViewBag.AllMicrosite = AllMicrosite;
            }
            #endregion

            AssemblyInformationalVersionAttribute infoVersion = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();
            var appVersion = (infoVersion != null) ? infoVersion.InformationalVersion : "-";
            ViewBag.appVersion = appVersion;

            //return View("~/Areas/Admin/Views/Login/Index.cshtml");
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost]
        [AdminNotLogin]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection f)
        {
            #region ----- web id -----
            string webID = f["web_id"] + "";
            string targetUrl = _session.GetString("admin_login_redirect") ?? "";
            if (webID != null && webID.Trim() != "")
            {
                if (!CheckWeb(webID))
                {
                    TempData["alert_message"] = $"เว็ปไซต์ไม่ถูกต้อง";
                    return RedirectToAction("Login", new { targetUrl = targetUrl });
                }
                else
                {
                    var webInfo = _admin.WebInfo(Convert.ToInt32(webID), false, false);
                    if (webInfo != null)
                    {
                        _requestWebID = Convert.ToInt32(webInfo["id"]);
                    }
                }
            }
            else
            {
                webID = "0";
                _requestWebID = Convert.ToInt32(webID);
            }
            #endregion

            //return Ok(f.ToList());
            if (
                f.ContainsKey("username") && f.ContainsKey("password") &&
                f["username"].ToString().Trim() != "" &&
                f["password"].ToString().Trim() != ""
                )
            {
                string username = f["username"].ToString();
                string password = f["password"].ToString();
                var admin_user = _admin.CheckAdmin(username, _requestWebID);
                TempData["guest_username"] = username;
                //return Ok(new { admin_user, pass_input = _utility.GenerateSHA512String(password) });
                if (admin_user == null)
                {
                    TempData["alert_message"] = "Username / Password ไม่ถูกต้อง";

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: 0,
                        admin_username: "",
                        action: "login_fail_user",
                        action_info: "เข้าสู่ระบบ ไม่สำเร็จ : username ไม่ถูกต้อง)",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin",
                        old_value: JsonConvert.SerializeObject(f)
                    );
                    #endregion

                    if (f["relogin"] + "" != null && f["relogin"] + "" == "1")
                    {
                        return Content("0");
                    }

                    return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
                }
                else
                {
                    #region ----- Check web id -----
                    if (!_admin.CheckAdminWeb(Convert.ToInt32(admin_user["id"]), _requestWebID))
                    {
                        TempData["alert_message"] = $"Username / Password ไม่ถูกต้อง";

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: 0,
                            admin_username: "",
                            action: "login_fail_user_web",
                            action_info: "เข้าสู่ระบบ ไม่สำเร็จ : username, web ไม่ถูกต้อง)",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_admin",
                            old_value: JsonConvert.SerializeObject(f)
                        );
                        #endregion

                        return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
                    }
                    #endregion

                    if (admin_user["status"].ToString() == "0")
                    {
                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: Convert.ToInt32(admin_user["id"]),
                            admin_username: admin_user["username"] + "",
                            action: "login_fail_user_lock",
                            action_info: "เข้าสู่ระบบ ไม่สำเร็จ : ผู้ใช้ถูกล็อค",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_admin",
                            old_value: JsonConvert.SerializeObject(f)
                        );
                        #endregion

                        if (f["relogin"] + "" != null && f["relogin"] + "" == "1")
                        {
                            return Content("0");
                        }

                        TempData["alert_message"] = "Username ของท่านถูกระงับ กรุณาติดต่อเจ้าหน้าที่่";
                        return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
                    }

                    #region ----- ตรวจ Username / Password ตาม "ประเภทการ Login" -----
                    // login_type = 1 (แบบปกติ)    : เทียบ SHA512 กับคอลัมน์ password เหมือนเดิมทุกประการ
                    // login_type = 2 (แบบพนักงาน) : ส่ง Username/Password ไปตรวจกับ Azure AD (OAuth2/OIDC)
                    //                               ผู้ใช้ประเภทนี้ไม่มีรหัสผ่านเก็บในระบบ (คอลัมน์ password เป็นค่าว่าง)
                    // ขั้นตอนอื่นหลังจากนี้ (session, สิทธิ์โมดูล, log, นับ login ผิด) ใช้ของเดิมร่วมกันทั้งสองประเภท
                    int loginType = LOGIN_TYPE_NORMAL;
                    if (admin_user.ContainsKey("login_type") && (admin_user["login_type"] + "").Trim() == LOGIN_TYPE_EMPLOYEE.ToString())
                    {
                        loginType = LOGIN_TYPE_EMPLOYEE;
                    }

                    bool passwordValid;
                    if (loginType == LOGIN_TYPE_EMPLOYEE)
                    {
                        var azureResult = _azureAd.ValidateCredential(username, password);
                        passwordValid = azureResult.IsSuccess;

                        //----- กรณีที่ "ไม่ใช่" รหัสผ่านผิด (ตั้งค่าไม่ครบ / ติดต่อ Azure ไม่ได้ / บัญชีถูกล็อคที่ฝั่ง Azure)
                        //      ไม่นับเป็นการกรอกรหัสผ่านผิด และไม่ล็อค user ในระบบ
                        if (!passwordValid && azureResult.Status != AzureAdAuthStatus.InvalidCredentials)
                        {
                            #region Logs Action
                            //----- web_admin_log.action_info เป็น varchar(255) จึงต้องตัดความยาวก่อนบันทึก
                            //      (ข้อความเต็มจาก Azure ยาวเกินได้ง่าย เช่น AADSTS65001 ที่มี Trace/Correlation ID)
                            string azureLogInfo = string.Format("เข้าสู่ระบบ (Azure AD) ไม่สำเร็จ : {0} - {1}", azureResult.ErrorCode, azureResult.RawError);
                            if (azureLogInfo.Length > 250)
                            {
                                azureLogInfo = azureLogInfo.Substring(0, 250);
                            }

                            _admin.ActionLogs(
                                admin_user_id: Convert.ToInt32(admin_user["id"]),
                                admin_username: admin_user["username"] + "",
                                action: "login_fail_azure",
                                action_info: azureLogInfo,
                                action_url: Request.Host.Value + Request.Path.Value,
                                action_table: "web_admin"
                            );
                            #endregion

                            if (f["relogin"] + "" != null && f["relogin"] + "" == "1")
                            {
                                return Content("0");
                            }

                            TempData["alert_message"] = azureResult.Message;
                            return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
                        }
                    }
                    else
                    {
                        passwordValid = _utility.GenerateSHA512String(password) == (admin_user["password"] + "").Trim();
                    }
                    #endregion

                    if (!passwordValid)
                    {
                        int no_login_fail_to_suspend = _config.GetValue<int>("ConfigPassword:LoginFailCount");

                        #region ######## Count wrong password #######
                        if (_session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"]) == null || _session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"]) + "" == "0")
                        {
                            _session.SetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"], "1");
                        }
                        else
                        {
                            _session.SetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"], (Convert.ToInt32(_session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"]).ToString()) + 1).ToString());
                        }

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: Convert.ToInt32(admin_user["id"]),
                            admin_username: admin_user["username"] + "",
                            action: "login_fail_password",
                            action_info: "เข้าสู่ระบบ ไม่สำเร็จ : password ไม่ถูกต้อง",
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: "web_admin",
                            old_value: JsonConvert.SerializeObject(f)
                        );
                        #endregion

                        if (Convert.ToInt32(_session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"])) >= no_login_fail_to_suspend)
                        {
                            #region Logs Action
                            _admin.ActionLogs(
                                admin_user_id: (int)admin_user["id"],
                                admin_username: admin_user["username"] + "",
                                action: "user_lock",
                                action_info: String.Format("ผู้ใช้ถูกล็อค : กรอกรหัสผ่านผิด {0} ครั้ง", _session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"])),
                                action_url: Request.Host.Value + Request.Path.Value,
                                action_table: "web_admin"
                            );
                            #endregion

                            _admin.LockUser(username, _requestWebID);
                        }

                        if (f["relogin"] + "" != null && f["relogin"] + "" == "1")
                        {
                            return Content("0");
                        }

                        //TempData["alert_message"] = string.Format("Password ไม่ถูกต้อง (ครั้งที่ {0})", _session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"]));
                        TempData["alert_message"] = "Username / Password ไม่ถูกต้อง";
                        return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
                        #endregion
                    }

                    #region ############ Two Factor ################ [ปิดการใช้งาน 2FA ทั้งระบบ - comment ไว้ก่อน]
                    /*
                    if ((admin_user["use_otp"] + "").ToString().Trim() == "1")
                    {
                        _session.SetString("admin_user_otp", (admin_user["username"] + "").Trim());
                        _session.SetString("admin_pass_otp", (admin_user["password"] + "").Trim());
                        _session.SetString("admin_web_id_otp", (admin_user["web_id"] + "").Trim());

                        SendEmailAuthen(_requestWebID);

                        return RedirectToAction("LoginAuthen", new { targetUrl = targetUrl, webID = _requestWebID });
                    }
                    */
                    #endregion

                    #region Set Admin Session ----
                    _admin.setAdminSession(username, _requestWebID);
                    _admin.SetSessionWebID();
                    #endregion

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "login",
                        action_info: "เข้าสู่ระบบ",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin"
                    );
                    #endregion

                    #region ----- Clear Login fail count -----
                    if (_session.GetString("guest_try_login_" + _requestWebID + "_" + admin_user["username"]) != null)
                    {
                        _session.Remove("guest_try_login_" + _requestWebID + "_" + admin_user["username"]);
                    }
                    #endregion
                       
                    /*
                    string dir = PathHelper.MapPath("~/Files/"+ admin_user["id"].ToString());
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                         
                        using (FileStream fs = System.IO.File.Create(dir+"/file.txt"))
                        {    
                            Byte[] title = new UTF8Encoding(true).GetBytes(".");
                            fs.Write(title, 0, title.Length); 
                        }
                    }
                    */


                    if (f["relogin"]+"" != null && f["relogin"]+"" == "1")
                    {
                        return Content("1");
                    }

                    string? admin_login_redirect = _session.GetString("admin_login_redirect");
                    if (admin_login_redirect != null && admin_login_redirect != "")
                    {
                        _session.Remove("admin_login_redirect");
                        return Redirect(admin_login_redirect);
                    }
                    else
                    {
                        return Redirect("/Admin/User/Index");
                    }
                }
            }
            else
            {
                if (f["relogin"] + "" != null && f["relogin"] + "" == "1")
                {
                    return Content("0");
                }

                TempData["alert_message"] = "กรุณากรอก Username หรือ Password";
                return RedirectToAction("Login", new { targetUrl = targetUrl, webID = _requestWebID });
            }
        }

        #region ############ Two Factor (2FA) ################ [ปิดการใช้งานทั้งหมด - comment ไว้ก่อน]
        /*
        [HttpGet]
        [AdminNotLogin]
        public IActionResult LoginAuthen(string webID)
        {
            #region ----- web id -----
            if (webID != null && webID.Trim() != "")
            {
                if (!CheckWeb(webID))
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                webID = "0";
            }
            _requestWebID = Convert.ToInt32(webID);
            #endregion

            ViewBag.webID = _requestWebID;
            if ((_session.GetString("admin_user_otp") + "").ToString().Trim() != "" &&
                (_session.GetString("admin_pass_otp") + "").ToString().Trim() != ""
                )
            {
                string username = (_session.GetString("admin_user_otp") + "").ToString().Trim();
                var admin_user = _admin.CheckAdmin(username, _requestWebID);
                if (admin_user == null)
                {
                    TempData["alert_message"] = "Username / Password ไม่ถูกต้อง";
                    return RedirectToAction("Login", new { webID = _requestWebID });
                }
            }
            else
            {
                TempData["alert_message"] = "กรุณากรอก Username หรือ Password";
                return RedirectToAction("Login", new { webID = _requestWebID });

            }
            ViewBag.OTP_alert_message = null;
            if (TempData["OTP_alert_message"] != null && (TempData["OTP_alert_message"] + "").ToString().Trim() != "")
            {
                ViewBag.OTP_alert_message = (TempData["OTP_alert_message"] + "").ToString().Trim();
            }
            return View("~/Areas/Admin/Views/User/LoginAuthen.cshtml");
        }

        [HttpPost]
        [AdminNotLogin]
        [ValidateAntiForgeryToken]
        public IActionResult LoginAuthen(IFormCollection f)
        {
            string webID = f["web_id"] + "";

            #region ----- web id -----
            if (webID != null && webID.Trim() != "")
            {
                if (!CheckWeb(webID))
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                webID = "0";
            }
            _requestWebID = Convert.ToInt32(webID);
            #endregion

            if (f.ContainsKey("PassOTP") == false || f["PassOTP"].ToString().Trim() == "")
            {
                TempData["OTP_alert_message"] = "กรุณากรอก OTP";
                return RedirectToAction("LoginAuthen", new { webID = _requestWebID });
            }
            string Data_Pass_OTP = f["PassOTP"].ToString().Trim();

            if (
                (_session.GetString("admin_user_otp") + "").ToString().Trim() != "" &&
                (_session.GetString("admin_pass_otp") + "").ToString().Trim() != ""
                )
            {
                string username = (_session.GetString("admin_user_otp") + "").ToString().Trim();
                string password = (_session.GetString("admin_pass_otp") + "").ToString().Trim();
                var admin_user = _admin.CheckAdmin(username, _requestWebID);
                if (admin_user == null)
                {
                    TempData["alert_message"] = "Username / Password ไม่ถูกต้อง";
                    return RedirectToAction("Login", new { webID = _requestWebID });
                }
                else
                {
                    if (admin_user["status"].ToString() == "0")
                    {
                        TempData["alert_message"] = "Username ของท่านถูกระงับ กรุณาติดต่อเจ้าหน้าที่่";
                        return RedirectToAction("Login", new { webID = _requestWebID });
                    }

                    #region ############ Two Factor ################
                    if (Data_Pass_OTP.ToString().Trim() == (admin_user["otp"]).ToString().Trim())
                    {
                        if (Convert.ToDateTime(admin_user["otp_dt"].ToString().Trim()).ToLocalTime() >= System.DateTime.Now)
                        {
                            _db.Update("web_admin", "WHERE username = @username and web_id = @web_id ",
                            new Dictionary<string, object> {
                                { "otp", "" },
                            },
                            new Dictionary<string, object> {
                                { "username", username.ToString().Trim() },
                                { "web_id", Convert.ToInt32(admin_user["web_id"]) }
                            });
                        }
                        else
                        {
                            TempData["OTP_alert_message"] = "OTP หมดอายุ กรุณาส่ง OTP ใหม่";
                            return RedirectToAction("LoginAuthen", new { webID = _requestWebID });
                        }
                    }
                    else
                    {
                        TempData["OTP_alert_message"] = "OTP ไม่ถูกต้อง";
                        return RedirectToAction("LoginAuthen", new { webID = _requestWebID });
                    }

                    #endregion

                    #region Set Admin Session ----
                    _admin.setAdminSession(username, _requestWebID);
                    _admin.SetSessionWebID();
                    #endregion

                    #region ############ Force Change Password ################
                    var Config_Password = _config.GetSection("ConfigPassword");
                    int Config_Password_ExpiresInDay = Convert.ToInt32(Config_Password.GetSection("ExpiresInDay").Value.ToString());
                    if (Convert.ToDateTime(admin_user["last_change_password_at"].ToString().Trim()).AddDays(Config_Password_ExpiresInDay).ToLocalTime() < System.DateTime.Now)
                    {
                        _session.SetInt32("admin_force_change_password", 1);
                        var Param_Force_Change_Password = new Dictionary<string, object>();
                        Param_Force_Change_Password.Add("force_change_password", 1);
                        _db.Update("web_admin", "WHERE id = @id and web_id = @web_id ", Param_Force_Change_Password, new Dictionary<string, object>() { { "id", (admin_user["id"] + "").ToString() }, { "web_id", _requestWebID } });
                    }
                    else
                    {
                        _session.SetInt32("admin_force_change_password", 0);
                    }
                    #endregion

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "login_2fa",
                        action_info: "เข้าสู่ระบบ (2FA)",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin"
                    );
                    #endregion

                    string? admin_login_redirect = _session.GetString("admin_login_redirect");
                    if (admin_login_redirect != null && admin_login_redirect != "")
                    {
                        _session.Remove("admin_login_redirect");
                        return Redirect(admin_login_redirect);
                    }
                    else
                    {
                        return Redirect("/Admin/User/Index");
                    }
                }
            }
            else
            {
                TempData["alert_message"] = "กรุณากรอก Username หรือ Password";
                return RedirectToAction("Login", new { webID = _requestWebID });
            }
        }

        public int SendEmailAuthen(int webID = 0)
        {
            try
            {
                _requestWebID = webID;
                string username = (_session.GetString("admin_user_otp") + "").ToString().Trim();
                var admin_user = _admin.CheckAdmin(username, _requestWebID);
                if (admin_user != null)
                {
                    string Email = (admin_user["email"] + "").ToString().Trim();
                    Random generator = new Random();
                    int r = generator.Next(1000, 1000000);
                    string Data_Otp = r.ToString().PadLeft(6, '0');

                    int otp_expire = 15;//นาที

                    string path = _hostingEnvironment.ContentRootPath + "/Storage/Template/Email/TwoFactor.html";
                    string html = System.IO.File.ReadAllText(path);
                    html = html.Replace("{{Logo}}", _utility.rootURL() + "/assets/images/logo/logo.png");
                    html = html.Replace("{{text_otp}}", Data_Otp.ToString());
                    html = html.Replace("{{text_otp_expire}}", "รหัสจะหมดอายุใน " + otp_expire.ToString() + " นาที");

                    _db.Update("web_admin", "WHERE username = @username and web_id = @web_id ",
                    new Dictionary<string, object> {
                    { "otp", Data_Otp.ToString() },
                    { "otp_dt", System.DateTime.Now.AddMinutes(otp_expire) },
                    },
                    new Dictionary<string, object> {
                    { "username", username.ToString().Trim() },
                    { "web_id", Convert.ToInt32(admin_user["web_id"]) }
                    });

                    bool result = _utility.Email(Email.ToString(), "ธนาคารไทยเครดิต เพื่อรายย่อย จำกัด (มหาชน) : รหัสยืนยันสำหรับเข้าสู่ระบบ", html);
                    if (result)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage.ToString(), "Logs_SendEmailAuthen_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

                return 0;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int reSendOTP()
        {
            int webID = 0;
            if (_session.GetString("admin_web_id_otp") != "" && _utility.isInt(_session.GetString("admin_web_id_otp")))
            {
                webID = Convert.ToInt32(_session.GetString("admin_web_id_otp"));
            }
            int results = SendEmailAuthen(webID);
            return results;
        }
        */
        #endregion

        [HttpPost]
        [AdminLogin]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            #region Logs Action
            _admin.ActionLogs(
                admin_user_id: (int)_session.GetInt32("admin_user_id"),
                admin_username: _session.GetString("admin_user"),
                action: "logout",
                action_info: "ออกจากระบบ",
                action_url: Request.Host.Value + Request.Path.Value,
                action_table: "web_admin"
            );
            #endregion

            int currentWebID = _admin._currentWebID;

            _admin.ClearSession();

            if (currentWebID > 0)
            {
                return Redirect($"/Admin/User/Login?webID={_admin._currentWebID}");
            }
            return Redirect("/Admin/User/Login");
        }

        [AdminLogin]
        public IActionResult Edit()
        {
            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));
            return View("~/Areas/Admin/Views/User/Edit.cshtml");
        }

        [HttpPost]
        [AdminLogin]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IFormCollection f)
        {
            if (string.IsNullOrWhiteSpace(f["name"]))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ ชื่อ";
            }
            else if (string.IsNullOrWhiteSpace(f["surname"]))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ สกุล";
            }
            else if (string.IsNullOrWhiteSpace(f["email"]))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ อีเมล";
            }
            else
            {
                _db.Update("web_admin", "where id = @id",
                    new Dictionary<string, object> {
                        { "name", f["name"].ToString() },
                        { "surname", f["surname"].ToString() },
                        { "section", f["section"].ToString() },
                        { "email", f["email"].ToString() },
                        //{ "use_otp", Convert.ToInt32(f["use_otp"]) },   // [ปิดการใช้งาน 2FA]
                        { "updated_at", System.DateTime.Now },
                    },
                    new Dictionary<string, object> {
                        { "id", _session.GetInt32("admin_user_id") }
                    });

                #region Logs Action
                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "edit_profile",
                    action_info: "แก้ไข ข้อมูลส่วนตัว",
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: "web_admin"
                );
                #endregion

                TempData["alert_class"] = "alert-success";
                TempData["alert_message"] = "แก้ไขข้อมูลสำเร็จ";
            }

            return Redirect("/Admin/User/Edit");
        }

        /// <summary>ผู้ใช้ "แบบพนักงาน" ไม่มีรหัสผ่านในระบบ (ตรวจกับ Azure AD) จึงเปลี่ยนรหัสผ่านที่นี่ไม่ได้</summary>
        private bool IsEmployeeLogin()
        {
            return (_session.GetInt32("admin_login_type") ?? LOGIN_TYPE_NORMAL) == LOGIN_TYPE_EMPLOYEE;
        }

        [AdminLogin]
        public IActionResult ChangePassword()
        {
            if (IsEmployeeLogin())
            {
                TempData["alert_class"] = "alert-warning";
                TempData["alert_message"] = "บัญชีนี้เข้าสู่ระบบด้วย Azure AD (แบบพนักงาน) กรุณาเปลี่ยนรหัสผ่านที่ระบบขององค์กร";
                return Redirect("/Admin/User/Index");
            }

            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));
            ViewBag.pass_old = "";
            ViewBag.pass_new = "";
            ViewBag.pass_new_confirm = "";

            return View("~/Areas/Admin/Views/User/ChangePassword.cshtml");
        }

        [HttpPost]
        [AdminLogin]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(IFormCollection f)
        {
            if (IsEmployeeLogin())
            {
                TempData["alert_class"] = "alert-warning";
                TempData["alert_message"] = "บัญชีนี้เข้าสู่ระบบด้วย Azure AD (แบบพนักงาน) กรุณาเปลี่ยนรหัสผ่านที่ระบบขององค์กร";
                return Redirect("/Admin/User/Index");
            }

            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));
            ViewBag.pass_old = "";
            ViewBag.pass_new = "";
            ViewBag.pass_new_confirm = "";

            string pass_old = f["pass_old"].ToString();
            string pass_new = f["pass_new"].ToString();
            string pass_new_confirm = f["pass_new_confirm"].ToString();

            if (string.IsNullOrWhiteSpace(pass_old))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ รหัสผ่านเดิม";
            }
            else if (string.IsNullOrWhiteSpace(pass_new))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ รหัสผ่านใหม่";
            }
            else if (string.IsNullOrWhiteSpace(pass_new_confirm))
            {
                TempData["alert_class"] = "alert-danger";
                TempData["alert_message"] = "กรุณาระบุ ยืนยันรหัสผ่านใหม่";
            }
            else
            { 
                if (pass_new.Length < 8 || pass_new.Length > 20)
                {
                    TempData["alert_class"] = "alert-warning";
                    TempData["alert_message"] = "ต้องมีจำนวน 8-20 ตัวอักษร";
                    return Redirect("/Admin/User/ChangePassword"); 
                }

                string[] arr_s = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                bool check_found = false;
                for (int i = 0; i < arr_s.Length; i++)
                {
                    if (pass_new.IndexOf(arr_s[i]) > -1)
                    {
                        check_found = true;
                    }
                }
                if(check_found == false)
                {
                    TempData["alert_class"] = "alert-warning";
                    TempData["alert_message"] = "ต้องมีตัวอักษรภาษาอังกฤษพิมพ์เล็ก อย่างน้อย 1 ตัวอักษร";
                    return Redirect("/Admin/User/ChangePassword"); 
                }

                string[] arr_s1 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                check_found = false;
                for (int i = 0; i < arr_s1.Length; i++)
                {
                    if (pass_new.IndexOf(arr_s1[i]) > -1)
                    {
                        check_found = true;
                    }
                }
                if (check_found == false)
                {
                    TempData["alert_class"] = "alert-warning";
                    TempData["alert_message"] = "ต้องมีตัวอักษรภาษาอังกฤษพิมพ์ใหญ่ อย่างน้อย 1 ตัวอักษร";
                    return Redirect("/Admin/User/ChangePassword");
                }

                string[] arr_s2 = { "!", "@", "#", "$", "%", "^", "&", "?", "-", "_" };
                check_found = false;
                for (int i = 0; i < arr_s2.Length; i++)
                {
                    if (pass_new.Contains(arr_s2[i]))
                    {
                        check_found = true;
                    }
                }
                if (check_found == false)
                {
                    TempData["alert_class"] = "alert-warning";
                    TempData["alert_message"] = "รหัสผ่านต้องมีอักษระพิเศษ เช่น ! @ # $ % ^ & ? - _ ";
                    return Redirect("/Admin/User/ChangePassword");
                }

                string[] arr_s3 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                check_found = false;
                for (int i = 0; i < arr_s3.Length; i++)
                {
                    if (pass_new.IndexOf(arr_s3[i]) > -1)
                    {
                        check_found = true;
                    }
                }
                if (check_found == false)
                {
                    TempData["alert_class"] = "alert-warning";
                    TempData["alert_message"] = "รหัสผ่านต้องมีตัวเลข 0 - 9";
                    return Redirect("/Admin/User/ChangePassword");
                }
  
                ViewBag.pass_old = pass_old;
                ViewBag.pass_new = pass_new;
                ViewBag.pass_new_confirm = pass_new_confirm;

                var admin_user = _admin.AdminInfo(_session.GetString("admin_user"));
                if (_utility.GenerateSHA512String(pass_old) != (admin_user["password"] + "").Trim())
                {
                    TempData["alert_class"] = "alert-danger";
                    TempData["alert_message"] = "รหัสผ่านเดิม ไม่ถูกต้อง";
                }
                else
                {
                    #region ############### ตรวจสอบการตั้งรหัสผ่านเดิม ##############
                    var Config_Password = _config.GetSection("ConfigPassword");
                    string Config_Password_Times = Config_Password.GetSection("LastPassword").Value.ToString();
                    var SQL_Old_Password_Last = _db.ExecuteQuery("SELECT password FROM web_admin_password_log WHERE admin_user=@admin_user and web_id = @web_id ORDER BY created_at DESC Limit " + Config_Password_Times.ToString(), new Dictionary<string, object>() { { "admin_user", (admin_user["username"] + "").ToString() }, { "web_id", _currentWebID } });

                    if (SQL_Old_Password_Last.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow r_Old_Password_Last in SQL_Old_Password_Last.Rows)
                        {
                            if (_utility.GenerateSHA512String(pass_new.ToString().Trim()) == (r_Old_Password_Last["password"] + "").ToString().Trim())
                            {
                                TempData["alert_class"] = "alert-warning";
                                TempData["alert_message"] = "กรุณาตั้งรหัสผ่านใหม่ รหัสผ่านนี้ถูกใช้ไปก่อนหน้านี้แล้ว";
                                return Redirect("/Admin/User/ChangePassword");
                            }
                        }
                    }
                    #endregion
                     
                    int Result_SQL = _db.Update("web_admin", "where id = @id and web_id = @web_id ",
                    new Dictionary<string, object> {
                        { "password", _utility.GenerateSHA512String(pass_new) },
                        { "updated_at", System.DateTime.Now },
                        { "last_change_password_at", System.DateTime.Now },
                        { "force_change_password", 0 },
                    },
                    new Dictionary<string, object> {
                        { "id", _session.GetInt32("admin_user_id") },
                        { "web_id", _currentWebID }
                    });

                    if (Result_SQL != 0)
                    {
                        #region ############### Log Password Admin ##############
                        if (!string.IsNullOrEmpty(pass_new.ToString()))
                        {
                            var Param_Log_PassWord_Admin = new Dictionary<string, object>();
                            Param_Log_PassWord_Admin.Add("created_at", System.DateTime.Now);
                            Param_Log_PassWord_Admin.Add("updated_at", System.DateTime.Now);
                            Param_Log_PassWord_Admin.Add("created_by", _session.GetString("admin_name") ?? "");
                            Param_Log_PassWord_Admin.Add("updated_by", _session.GetString("admin_name") ?? "");
                            Param_Log_PassWord_Admin.Add("sort", 0);
                            Param_Log_PassWord_Admin.Add("status", 1);
                            Param_Log_PassWord_Admin.Add("admin_user", (admin_user["username"] + "").ToString());
                            Param_Log_PassWord_Admin.Add("password", _utility.GenerateSHA512String(pass_new.ToString()));
                            Param_Log_PassWord_Admin.Add("web_id", _currentWebID);
                            _db.Insert("web_admin_password_log", Param_Log_PassWord_Admin);
                        }
                        #endregion
                        _session.SetInt32("admin_force_change_password", 0);
                        _session.Remove("admin_text_password_expired_in");
                    }

                    _session.SetString("admin_pass", _utility.GenerateSHA512String(pass_new));

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "change_password",
                        action_info: "เปลี่ยนรหัสผ่าน",
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: "web_admin"
                    );
                    #endregion

                    TempData["alert_class"] = "alert-success";
                    TempData["alert_message"] = "แก้ไขข้อมูลสำเร็จ";
                    return Redirect("/Admin/User/ChangePassword");
                }
            }
            return View("~/Areas/Admin/Views/User/ChangePassword.cshtml");
        }

        [AdminLogin]
        public IActionResult LastActivity()
        {
            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));

            ViewBag.activ_logs = null;
            var activ_logs = _db.ExecuteQuery("select created_at, admin_username, action_info from web_admin_log where admin_user_id = @id and web_id = @web_id and (action <> 'login' AND action <> 'logout') order by created_at desc limit 10", new() { { "id", Convert.ToInt32(_session.GetInt32("admin_user_id")) }, { "web_id", _currentWebID } });
            if (activ_logs != null && activ_logs.Rows.Count > 0)
            {
                ViewBag.activ_logs = activ_logs.Rows;
                //return Ok(DataTableToJSONWithJSONNet(activ_logs));
            }
            return View("~/Areas/Admin/Views/User/LastActivity.cshtml");
        }

        [AdminLogin]
        public IActionResult Dashboard()
        {
            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));

            ViewBag.active_user_today = null;
            ViewBag.total_user_today = null;
            ViewBag.sum_weekly = null;
                
            var r = _db.ExecuteQuery("select distinct admin_username from web_admin_log where web_id = @web_id AND created_at > '" + DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("en-US")) + " 00:00:00' AND created_at < '" + DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("en-US")) + " 23:59:59'",new() {{"web_id",_currentWebID}});
            if (r != null && r.Rows.Count > 0)
            {
                ViewBag.active_user_today = r.Rows.Count;
            }
            r = _db.ExecuteQuery("select * from web_admin", new() { { "web_id", _currentWebID } });
            if (r != null && r.Rows.Count > 0)
            {
                ViewBag.total_user_today = r.Rows.Count;
            }
            r = _db.ExecuteQuery("select distinct admin_username from web_admin_log where web_id = @web_id AND created_at > '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd", new CultureInfo("en-US")) + " 00:00:00' AND created_at < '" + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd", new CultureInfo("en-US")) + " 23:59:59'", new() {{"web_id",_currentWebID}});
            if (r != null && r.Rows.Count > 0)
            {
                var all_r = Convert.ToDouble(ViewBag.total_user_today);
                var this_r = Convert.ToDouble(r.Rows.Count);

                all_r = all_r;
                this_r = this_r;

                this_r = this_r;

                var re_r = this_r / all_r;
                re_r = re_r * 100;
                 
                ViewBag.sum_weekly = re_r.ToString("0.00");
            }

            r = _db.ExecuteQuery("SELECT mod_name, mod_name_txt FROM ( SELECT DISTINCT ON (mod_name) mod_name, mod_name_txt, id FROM web_admin_log WHERE action IN ('add','edit','delete') ORDER BY mod_name, id DESC ) t ORDER BY id DESC LIMIT 8", new() { { "web_id", _currentWebID } });
            if (r != null && r.Rows.Count > 0)
            {
                ViewBag.LastUse = r.Rows;
            }

            ViewBag._admin = _admin;
            ViewBag.user = _admin.AdminInfo(_session.GetString("admin_user"));
             
            var activ_logs = _db.ExecuteQuery("select created_at, admin_username, action_info from web_admin_log where admin_user_id = @id and web_id = @web_id and (action <> 'login' AND action <> 'logout') order by created_at desc limit 10", new() { { "id", Convert.ToInt32(_session.GetInt32("admin_user_id")) }, { "web_id", _currentWebID } });
            if (activ_logs != null && activ_logs.Rows.Count > 0)
            {
                ViewBag.activ_logs = activ_logs.Rows; 
            }

            return View("~/Areas/Admin/Views/User/Dashboard.cshtml");
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        [HttpGet] 
        public IActionResult AjaxCheckSession()
        { 
            string? admin_login = _session.GetString("admin_login");
            string? admin_username = _session.GetString("admin_user");
            string? admin_password = _session.GetString("admin_pass");
            int? admin_web_id = _session.GetInt32("admin_web_id");
            string re = "";
            if (admin_login == null || admin_username == null || admin_web_id == null)
            {
                re = "1";
            }
            else
            {
                var admin_user = _admin.CheckAdmin(admin_username.ToString(), admin_password.ToString(), Convert.ToInt32(admin_web_id));
                if (admin_user == null)
                {
                    re = "1";
                }
            }
            return Content(re.ToString());
        }

        // ==================================================================
        // NPA Price Sync : /user/update_npa_price
        // ซิงก์ราคาทรัพย์ NPA จาก MySQL (tb_product2) -> PostgreSQL (web_npa_price)
        // ตรวจจับ "ทรัพย์ใหม่" และ "ทรัพย์ปรับราคา" แล้วส่งอีเมลแจ้งเตือน
        // ==================================================================
        [HttpGet]
        [Route("user/update_npa_price")]
        [Route("Admin/User/update_npa_price")]
        public IActionResult update_npa_price()
        {
            // ปลายทางอีเมล (ตอนนี้ทดสอบก่อน — อนาคตจะเปลี่ยนวิธีเรียกปลายทาง)
            const string MAIL_TO = "anurakball@gmail.com";
            const string MAIL_SUBJECT = "แจ้งเตือนทรัพย์ใหม่/ปรับราคา";

            // filter มาตรฐานของทรัพย์ที่ "แสดงผล/ใช้งานได้จริง" ใน tb_product2
            const string MYSQL_FILTER =
                "p.is_deleted = 0 AND p.is_show = 1 AND p.status <> '2' AND p.status <> '3'";

            try
            {
                // ---------- 1.1 : reset new = 'N' ทุก row ----------
                _db.ExecuteNonQuery("UPDATE web_npa_price SET \"new\" = 'N'");

                // ---------- 1.2 : ตรวจจับ + เพิ่มทรัพย์ใหม่ ----------
                // ดึง npa_id ที่มีอยู่แล้วใน PG
                var existingIds = new HashSet<long>();
                var existDt = _db.ExecuteQuery("SELECT npa_id FROM web_npa_price WHERE npa_id IS NOT NULL");
                foreach (DataRow r in existDt.Rows)
                {
                    existingIds.Add(Convert.ToInt64(r["npa_id"]));
                }
                long maxNpaId = existingIds.Count > 0 ? existingIds.Max() : 0;

                // ดึงทรัพย์ทั้งหมดที่ผ่าน filter จาก MySQL
                var mysqlRows = _db.ExecuteQueryMySQL(
                    "SELECT p.id, p.product_code, p.project, p.price " +
                    "FROM tb_product2 p WHERE " + MYSQL_FILTER);

                long maxMysqlId = 0;                                   // max id เฉพาะที่ผ่าน filter
                var newIds = new List<int>();
                var newCodes = new List<string>();
                var newPrices = new List<int?>();
                // dict: product_code -> project (ใช้เติมชื่อโครงการในอีเมล)
                var projectByCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // list สำหรับ bulk update ราคา (ข้อ 1.3)
                var updCodes = new List<string>();
                var updPrices = new List<int?>();

                foreach (DataRow r in mysqlRows.Rows)
                {
                    long id = Convert.ToInt64(r["id"]);
                    if (id > maxMysqlId) maxMysqlId = id;

                    string code = (r["product_code"] == null || r["product_code"] == DBNull.Value)
                        ? "" : r["product_code"].ToString().Trim();
                    if (code == "") continue; // ข้ามทรัพย์ที่ไม่มี product_code

                    string project = (r["project"] == null || r["project"] == DBNull.Value)
                        ? "" : r["project"].ToString().Trim();
                    if (!projectByCode.ContainsKey(code)) projectByCode[code] = project;

                    int? price = (r["price"] == null || r["price"] == DBNull.Value)
                        ? (int?)null : Convert.ToInt32(r["price"]);

                    // เตรียม update ราคา (ทุก row ที่ผ่าน filter) สำหรับข้อ 1.3
                    updCodes.Add(code);
                    updPrices.Add(price);

                    // ทรัพย์ใหม่ = id ที่ยังไม่มีใน npa_id (วิธี set-difference ทนทานกว่าเทียบ max)
                    if (!existingIds.Contains(id))
                    {
                        newIds.Add((int)id);
                        newCodes.Add(code);
                        newPrices.Add(price);
                    }
                }

                // insert ทรัพย์ใหม่ (new = 'Y', old_price = price เพื่อไม่ให้ถูกนับซ้ำเป็น "ปรับราคา")
                int insertedNew = 0;
                if (newIds.Count > 0)
                {
                    _db.ExecuteNonQuery(
                        "INSERT INTO web_npa_price (npa_id, product_code, price, old_price, \"new\") " +
                        "SELECT t.id, t.code, t.price, t.price, 'Y' " +
                        "FROM unnest(@ids::int[], @codes::text[], @prices::int[]) AS t(id, code, price)",
                        new Dictionary<string, object>
                        {
                            { "ids", newIds.ToArray() },
                            { "codes", newCodes.ToArray() },
                            { "prices", newPrices.ToArray() }
                        });
                    insertedNew = newIds.Count;
                }

                // ---------- 1.3 : update price ทุกทรัพย์ที่ผ่าน filter (ไม่แตะ old_price/product_code/new) ----------
                int priceUpdated = 0;
                if (updCodes.Count > 0)
                {
                    priceUpdated = _db.ExecuteNonQuery(
                        "UPDATE web_npa_price w SET price = t.price " +
                        "FROM unnest(@codes::text[], @prices::int[]) AS t(code, price) " +
                        "WHERE TRIM(w.product_code) = t.code",
                        new Dictionary<string, object>
                        {
                            { "codes", updCodes.ToArray() },
                            { "prices", updPrices.ToArray() }
                        });
                }

                // ---------- 1.4 : เช็ค 2 เงื่อนไข + ส่งอีเมล ----------
                // เงื่อนไข 2 : ทรัพย์ใหม่ (new = 'Y')
                var newAssets = _db.ExecuteQuery(
                    "SELECT product_code, price, old_price, npa_id FROM web_npa_price WHERE \"new\" = 'Y' ORDER BY npa_id");
                // เงื่อนไข 1 : ราคาเปลี่ยน (price <> old_price) — ตัดทรัพย์ใหม่ออก (ทรัพย์ใหม่ price = old_price อยู่แล้ว)
                var priceChanges = _db.ExecuteQuery(
                    "SELECT product_code, price, old_price, npa_id FROM web_npa_price " +
                    "WHERE price IS DISTINCT FROM old_price ORDER BY npa_id");

                // ส่งอีเมลเฉพาะเมื่อมีรายการ (ทรัพย์ใหม่ หรือ ปรับราคา อย่างน้อย 1 รายการ)
                bool hasItems = newAssets.Rows.Count > 0 || priceChanges.Rows.Count > 0;
                bool emailSent = false;
                if (hasItems)
                {
                    string emailHtml = BuildNpaPriceEmail(newAssets, priceChanges, projectByCode);
                    emailSent = _utility.Email(MAIL_TO, MAIL_SUBJECT, emailHtml);
                }

                // ---------- 1.5 : จัดข้อมูลให้พร้อมสำหรับรอบถัดไป ----------
                _db.ExecuteNonQuery("UPDATE web_npa_price SET \"new\" = 'N'");   // 1.5.1
                _db.ExecuteNonQuery("UPDATE web_npa_price SET old_price = price"); // 1.5.2

                return Json(new
                {
                    success = true,
                    max_npa_id_before = maxNpaId,
                    max_mysql_id_filtered = maxMysqlId,
                    mysql_filtered_rows = mysqlRows.Rows.Count,
                    inserted_new = insertedNew,
                    price_rows_updated = priceUpdated,
                    new_asset_count = newAssets.Rows.Count,
                    price_change_count = priceChanges.Rows.Count,
                    email_to = MAIL_TO,
                    email_sent = emailSent,
                    email_skipped_no_items = !hasItems
                });
            }
            catch (Exception e)
            {
                string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage + " - update_npa_price\r\n" + e.StackTrace,
                    "Logs_update_npa_price_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                return Json(new { success = false, error = e.Message });
            }
        }

        // สร้าง HTML อีเมลแจ้งเตือน (ตารางที่ 1 = ทรัพย์ใหม่, ตารางที่ 2 = ปรับราคา)
        private string BuildNpaPriceEmail(DataTable newAssets, DataTable priceChanges, Dictionary<string, string> projectByCode)
        {
            string ProjectOf(string code)
            {
                if (!string.IsNullOrEmpty(code) && projectByCode.TryGetValue(code, out var p) && !string.IsNullOrEmpty(p))
                    return System.Net.WebUtility.HtmlEncode(p);
                return "-";
            }
            string Money(object v)
            {
                if (v == null || v == DBNull.Value) return "-";
                return Convert.ToInt64(v).ToString("#,##0") + " บาท";
            }
            string Code(object v)
            {
                string c = (v == null || v == DBNull.Value) ? "" : v.ToString().Trim();
                return System.Net.WebUtility.HtmlEncode(c == "" ? "-" : c);
            }

            var sb = new StringBuilder();
            sb.Append("<div style='font-family:Tahoma,Arial,sans-serif;font-size:14px;color:#222;max-width:820px;margin:auto;'>");
            sb.Append("<h2 style='color:#0047B6;margin:0 0 4px;'>แจ้งเตือนทรัพย์ใหม่ / ปรับราคา</h2>");
            sb.Append("<p style='color:#666;margin:0 0 16px;'>ประจำวันที่ " +
                DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " น. (ระบบซิงก์ราคาทรัพย์ NPA อัตโนมัติ)</p>");

            // ---------- ตารางที่ 1 : ทรัพย์ใหม่ ----------
            sb.Append("<h3 style='color:#0a7d33;border-left:4px solid #0a7d33;padding-left:8px;'>1. รายการทรัพย์ใหม่ (" +
                newAssets.Rows.Count.ToString("#,##0") + " รายการ)</h3>");
            if (newAssets.Rows.Count == 0)
            {
                sb.Append("<p style='color:#999;'>- ไม่มีรายการ -</p>");
            }
            else
            {
                sb.Append("<table style='border-collapse:collapse;width:100%;margin-bottom:24px;'>");
                sb.Append("<thead><tr style='background:#0a7d33;color:#fff;'>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;width:50px;'>ลำดับ</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;'>รหัสทรัพย์</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;'>ชื่อโครงการ</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;text-align:right;'>ราคา</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 1;
                foreach (DataRow r in newAssets.Rows)
                {
                    string code = (r["product_code"] == null || r["product_code"] == DBNull.Value) ? "" : r["product_code"].ToString().Trim();
                    string bg = (i % 2 == 0) ? "#f3f8f4" : "#ffffff";
                    sb.Append("<tr style='background:" + bg + ";'>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;text-align:center;'>" + i + "</td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;'>" + Code(r["product_code"]) + "</td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;'>" + ProjectOf(code) + "</td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;text-align:right;'>" + Money(r["price"]) + "</td>");
                    sb.Append("</tr>");
                    i++;
                }
                sb.Append("</tbody></table>");
            }

            // ---------- ตารางที่ 2 : ปรับราคา ----------
            sb.Append("<h3 style='color:#b8860b;border-left:4px solid #b8860b;padding-left:8px;'>2. รายการเปลี่ยนแปลงราคา (" +
                priceChanges.Rows.Count.ToString("#,##0") + " รายการ)</h3>");
            if (priceChanges.Rows.Count == 0)
            {
                sb.Append("<p style='color:#999;'>- ไม่มีรายการ -</p>");
            }
            else
            {
                sb.Append("<table style='border-collapse:collapse;width:100%;margin-bottom:24px;'>");
                sb.Append("<thead><tr style='background:#b8860b;color:#fff;'>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;width:50px;'>ลำดับ</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;'>ทรัพย์ (รหัส / โครงการ)</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;text-align:right;'>ราคาเดิม</th>");
                sb.Append("<th style='border:1px solid #ccc;padding:8px;text-align:right;'>ราคาใหม่</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 1;
                foreach (DataRow r in priceChanges.Rows)
                {
                    string code = (r["product_code"] == null || r["product_code"] == DBNull.Value) ? "" : r["product_code"].ToString().Trim();
                    string bg = (i % 2 == 0) ? "#fbf7ec" : "#ffffff";
                    sb.Append("<tr style='background:" + bg + ";'>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;text-align:center;'>" + i + "</td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;'><b>" + Code(r["product_code"]) + "</b><br/><span style='color:#666;'>" + ProjectOf(code) + "</span></td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;text-align:right;color:#b00;'>" + Money(r["old_price"]) + "</td>");
                    sb.Append("<td style='border:1px solid #ccc;padding:8px;text-align:right;color:#0a7d33;'>" + Money(r["price"]) + "</td>");
                    sb.Append("</tr>");
                    i++;
                }
                sb.Append("</tbody></table>");
            }

            sb.Append("<hr style='border:none;border-top:1px solid #eee;margin:16px 0;'/>");
            sb.Append("<p style='color:#999;font-size:12px;'>อีเมลฉบับนี้ส่งอัตโนมัติจากระบบ SAM Admin กรุณาอย่าตอบกลับ</p>");
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
