using Microsoft.AspNetCore.Mvc;

namespace tcrbank_html_cms.Views.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Areas/Admin/Views/Shared/_PartialHeader.cshtml");
        }
    }
}
