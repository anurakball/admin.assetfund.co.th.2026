using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace thaicredit_hr_admin.Areas.Admin.Filters
{
    public class AdminNotLoginAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //string? admin_login = context.HttpContext.Session.GetString("admin_login");
            string? admin_username = context.HttpContext.Session.GetString("admin_user");
            //string? admin_password = context.HttpContext.Session.GetString("admin_pass");

            if (admin_username != null && admin_username.ToString() != "")
            {
                context.Result = new RedirectResult("/Admin/User/Index");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

    }
}
