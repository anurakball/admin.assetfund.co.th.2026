using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // CMS ทรัพย์ที่ร่วมรายการ (ข้อมูลแยกอิสระ ไม่เชื่อมกับ MySQL tb_product2)
    public class MicrositeAssetController : AdminCoreController
    {
        public MicrositeAssetController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MicrositeAsset");
        }
    }
}
