using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Ocsp;
using System.Web;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Filters
{
    public class AdminLoginAttribute : Attribute, IActionFilter
    {
        private bool _json_response;
        public AdminLoginAttribute(bool json_response = false)
        {
            _json_response = json_response;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
            IConfiguration _config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            IWebHostEnvironment _hostingEnvironment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            IHttpContextAccessor _context = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            AdminHelpers _admin = new AdminHelpers(_hostingEnvironment, _config, _context);
            Utility _utility = new Utility(_hostingEnvironment, _config);

            string? admin_login = context.HttpContext.Session.GetString("admin_login");
            string? admin_username = context.HttpContext.Session.GetString("admin_user");
            string? admin_password = context.HttpContext.Session.GetString("admin_pass");
            int? admin_web_id = context.HttpContext.Session.GetInt32("admin_web_id");

            bool go2login = false;

            if (admin_login == null || admin_username == null || admin_web_id == null)
            {
                go2login = true;
            }
            else
            {
                #region ################### Select Admin ###############
                try
                {
                    var admin_user = _admin.CheckAdmin(admin_username.ToString(), admin_password.ToString(), Convert.ToInt32(admin_web_id));
                    //context.Result = new JsonResult( new { admin_user = admin_user, admin_username = admin_username, admin_password = admin_password });
                    if (admin_user == null)
                    {
                        go2login = true;
                        _admin.ClearSession();
                    }
                    else
                    {
                        /*check user web*/
                        _admin.SetSessionWebID();
                        if (!_admin.CheckAdminWeb(Convert.ToInt32(admin_user["id"]), _admin._currentWebID))
                        {
                            go2login = true;
                            _admin.ClearSession();
                        }

                        if ((int)admin_user["force_change_password"] == 1)
                        {
                            string path = context.HttpContext.Request.Path.ToString();

                            if (path.ToLower() != ("/Admin/User/ChangePassword").ToLower() && path.ToLower() != ("/Admin/User/Logout").ToLower())
                            {
                                context.Result = new RedirectResult(string.Format("/Admin/User/ChangePassword"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    go2login = true;
                    //context.Result = new ContentResult() { Content = ex.Message, ContentType = "text/html" };
                    _utility.writeLogs("AdminLoginAttr: " + ex.Message);
                }
                #endregion
            }

            if (go2login)
            {
                if (_json_response == true)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new { status = 401, message = "Authorization Required." });
                }
                else
                {
                    string cookieWebID = _context.HttpContext.Request.Cookies["webID"] + "";

                    string path = context.HttpContext.Request.Path.ToString();
                    string query = context.HttpContext.Request.QueryString.ToString();

                    //context.Result = new ContentResult() { Content = "path="+ path + "<br/>query=" + query, ContentType = "text/html" };

                    if (path != "/")
                    {
                        context.Result = new RedirectResult($"/Admin/User/Login?webID={cookieWebID}&targetUrl={path}{HttpUtility.UrlEncode(query)}");
                    }
                    else
                    {
                        context.Result = new RedirectResult($"/Admin/User/Login?webID={cookieWebID}");
                    }
                }
            }
            else
            {
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("th");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
            //throw new NotImplementedException();
        }
    }
}
