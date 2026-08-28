using Microsoft.AspNetCore.Mvc.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Filters
{
    public class WebServiceToken
    {
        public class WebServiceTokenAttribute : Attribute, IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context)
            {
                IConfiguration _config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                IWebHostEnvironment _hostingEnvironment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                IHttpContextAccessor _context = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();

                WebService _WebService = new WebService(_hostingEnvironment, _config, _context);

                _WebService.RequestToken();
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                // Executed after execution of an action method
            }
        }
    }
}
