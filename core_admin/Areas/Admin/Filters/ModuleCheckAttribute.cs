using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using thaicredit_hr_admin.Areas.Admin.Controllers;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Filters
{
    public class ModuleCheckAttribute : Attribute, IActionFilter
    {
        private string _access;
        public ModuleCheckAttribute(string access = "")
        {
            _access = access;
        }
        public Module? Module { set; get; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
            IConfiguration _config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            IWebHostEnvironment _hostingEnvironment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            IHttpContextAccessor _context = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            AdminHelpers _admin = new AdminHelpers(_hostingEnvironment, _config, _context);

            int _access_id = context.HttpContext.Session.GetInt32("admin_access_id") ?? 0;

            var controllerUsingThisAttribute = ((AdminCoreController)context.Controller);

            /* now you can use the public properties from the controller */
            Module = controllerUsingThisAttribute.Module;

            if (Module == null)
            {
                context.Result = controllerUsingThisAttribute.View("~/Areas/Admin/Views/Shared/Error.cshtml",
                    new ErrorAdminModel
                    {
                        ErrorTitle = "Invalid module config",
                        ErrorDetail = "Module not define."
                    });
            }
            else if (
                (context.HttpContext.Session.GetInt32("admin_web_id") ?? 0) != 0 &&
                AdminHelpers.IsMicrositeAllowed(Module.Name) == false
                )
            {
                //----- microsite (web_id != 0) เข้าได้เฉพาะโมดูลใน allowlist เท่านั้น
                //      ปิดกั้นการเข้าผ่าน URL ตรง ๆ ไม่ใช่แค่ซ่อนเมนู (ไม่มีผลกับเว็บไซต์หลัก web_id = 0)
                #region Logs Action
                _admin.ActionLogs(
                    admin_user_id: context.HttpContext.Session.GetInt32("admin_user_id") ?? 0,
                    admin_username: context.HttpContext.Session.GetString("admin_user"),
                    action: "access_denied",
                    action_info: string.Format("ปฏิเสธการเข้าใช้ (microsite ไม่ได้รับอนุญาต) : {0}", Module.Config.TextBreadcrumb),
                    action_url: context.HttpContext.Request.Host.Value + context.HttpContext.Request.Path.Value,
                    action_table: Module.Config.Table
                );
                #endregion

                context.Result = controllerUsingThisAttribute.View("~/Areas/Admin/Views/Shared/Error.cshtml",
                new ErrorAdminModel
                {
                    ErrorTitle = "Access denied!",
                    ErrorDetail = string.Format("เมนูนี้ไม่เปิดให้ใช้งานสำหรับ Microsite <br/>(<strong>{0}</strong>)", Module.Config.TextBreadcrumb),
                    ErrorClass = "alert-warning"
                });
            }
            else if (_admin.checkAccess(Module, _access_id, _access) == false)
            {
                #region Logs Action
                _admin.ActionLogs(
                    admin_user_id: context.HttpContext.Session.GetInt32("admin_user_id") ?? 0,
                    admin_username: context.HttpContext.Session.GetString("admin_user"),
                    action: "access_denied",
                    action_info: string.Format("ปฏิเสธการเข้าใช้ : {0} ({1})", Module.Config.TextBreadcrumb, (_access == "" ? "view" : _access).ToUpper()),
                    action_url: context.HttpContext.Request.Host.Value + context.HttpContext.Request.Path.Value,
                    action_table: Module.Config.Table
                );
                #endregion

                Console.WriteLine("access denine");

                context.Result = controllerUsingThisAttribute.View("~/Areas/Admin/Views/Shared/Error.cshtml",
                new ErrorAdminModel
                {

                    ErrorTitle = "Access denied!",
                    ErrorDetail = string.Format("ไม่มีสิทธิเข้าถึง <strong>{0}</strong> <br/>(module_name: {1}, access_id: {2}, access_type: <strong>{3}</strong>)", Module.Config.TextBreadcrumb, Module.Name, _access_id, (_access == "" ? "view" : _access).ToUpper()),
                    ErrorClass = "alert-warning"
                });
            } 
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
            //throw new NotImplementedException();
        }
    }
}
