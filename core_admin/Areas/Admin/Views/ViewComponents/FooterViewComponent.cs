using Microsoft.AspNetCore.Mvc;

namespace tcrbank_html_cms.Views.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Areas/Admin/Views/Shared/_PartialFooter.cshtml");
        }
    }
}

