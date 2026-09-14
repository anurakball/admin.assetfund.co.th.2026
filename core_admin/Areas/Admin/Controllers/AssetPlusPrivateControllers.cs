using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // =========================================================================
    //  กลุ่มเมนู "กองทุนส่วนบุคคล" + "เกี่ยวกับกองทุนสำรองเลี้ยงชีพ"
    //  ที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus (mod_tb_fund_private*, mod_tb_fund_prov)
    //
    //  ทุกตารางในกลุ่มนี้มีโครงเหมือนกันหมด : title / en_title / info / en_info / img1 / en_img1
    //  + คู่ pb_* จึงใช้ view ร่วมกัน (Views/ApPrivatePage/) และไม่ต้องมีตรรกะเฉพาะเมนู
    //  ตัวที่ต่างคือ ApPrivateInterested (กล่องข้อมูลผู้สนใจ — คนละสคีมา ดู/ลบ/export เท่านั้น)
    // =========================================================================

    /// <summary>กองทุนส่วนบุคคล / รู้จักกองทุนส่วนบุคคล — ระบบเดิม: mod_tb_fund_private</summary>
    public class ApPrivateController : AdminLegacyController
    {
        public ApPrivateController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivate");
        }
    }

    /// <summary>กองทุนส่วนบุคคล / ขั้นตอนการลงทุน — ระบบเดิม: mod_tb_fund_private_investment_process</summary>
    public class ApPrivateProcessController : AdminLegacyController
    {
        public ApPrivateProcessController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivateProcess");
        }
    }

    /// <summary>กองทุนส่วนบุคคล / นโยบายการลงทุน — ระบบเดิม: mod_tb_fund_private_investment_policy</summary>
    public class ApPrivatePolicyController : AdminLegacyController
    {
        public ApPrivatePolicyController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivatePolicy");
        }
    }

    /// <summary>กองทุนส่วนบุคคล / คำถามที่พบบ่อย — ระบบเดิม: mod_tb_fund_private_investment_qanda</summary>
    public class ApPrivateQandaController : AdminLegacyController
    {
        public ApPrivateQandaController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivateQanda");
        }
    }

    /// <summary>กองทุนส่วนบุคคล / ติดต่อเรา — ระบบเดิม: mod_tb_fund_private_contact_us</summary>
    public class ApPrivateContactController : AdminLegacyController
    {
        public ApPrivateContactController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivateContact");
        }
    }

    /// <summary>
    /// กองทุนส่วนบุคคล / ติดต่อกองทุนส่วนบุคคล — ระบบเดิม: mod_tb_fund_private_interested
    ///
    /// กล่องรับข้อมูลผู้สนใจที่กรอกจากหน้าเว็บ — หลังบ้านทำได้แค่ ดู / ค้นหา / ลบ / export
    /// (ตาราง tb_fund_private_interested ไม่มีคอลัมน์ last_user / pb_last_user / title
    ///  จึงเปิดเมนู Status / Approve ไม่ได้ — ระบบเดิมก็ตั้ง can_add / can_edit / can_status = false เช่นกัน)
    /// </summary>
    public class ApPrivateInterestedController : AdminLegacyController
    {
        public ApPrivateInterestedController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApPrivateInterested");
        }
    }

    /// <summary>กองทุนสำรองเลี้ยงชีพ / เกี่ยวกับกองทุนสำรองเลี้ยงชีพ — ระบบเดิม: mod_tb_fund_prov</summary>
    public class ApProvController : AdminLegacyController
    {
        public ApProvController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApProv");
        }
    }
}
