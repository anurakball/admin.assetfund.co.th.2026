using Microsoft.AspNetCore.Mvc.Filters;
using Ganss.Xss;
using thaicredit_hr_admin.Areas.Admin.Models;
using thaicredit_hr_admin.Areas.Admin.Controllers;

namespace thaicredit_hr_admin.Areas.Admin.Filters
{
    public class XssValidateAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerUsingThisAttribute = ((AdminCoreController)filterContext.Controller);
            var qs = filterContext.HttpContext.Request.Query.ToList();
            var sanitizer = new HtmlSanitizer();
            foreach (var all_q in qs)
            {
                string keyRecvd = all_q.Key.ToString();
                var keyEncd = sanitizer.Sanitize(keyRecvd);// AntiXssEncoder.HtmlEncode(keyRecvd);
                if (keyEncd != keyRecvd)
                {
                    //throw new ArgumentException($"Potentially dangerous keyRecvd: {keyRecvd}");
                    filterContext.Result = controllerUsingThisAttribute.View("~/Areas/Admin/Views/Shared/Error.cshtml",
                    new ErrorAdminModel
                    {
                        ErrorTitle = "Request denied !",
                        ErrorDetail = string.Format("Potentially dangerous query \"<strong>parameter</strong>\""),
                        ErrorClass = "alert-danger"
                    });
                }

                var valRecvd = all_q.Value.ToString();
                var valEncd = sanitizer.Sanitize(valRecvd);
                if (valEncd != valRecvd)
                {
                    //throw new ArgumentException($"Potentially dangerous valRecvd: {valRecvd}");
                    filterContext.Result = controllerUsingThisAttribute.View("~/Areas/Admin/Views/Shared/Error.cshtml",
                    new ErrorAdminModel
                    {
                        ErrorTitle = "Request denied !",
                        ErrorDetail = string.Format("Potentially dangerous query \"<strong>value</strong>\""),
                        ErrorClass = "alert-danger"
                    });
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
