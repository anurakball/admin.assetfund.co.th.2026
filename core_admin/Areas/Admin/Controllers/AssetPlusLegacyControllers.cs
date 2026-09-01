using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // =========================================================================
    //  เมนู CRUD ที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus
    //  ทุกตัวใช้เครื่องยนต์กลาง AdminLegacyController (ตาราง tb_* ของระบบเดิม)
    // =========================================================================

    /// <summary>ข้อมูลกองทุน / ประเภทกองทุนรวม — ระบบเดิม: mod_tb_fund_cat (tb_fund_cat)</summary>
    public class ApFundCatController : AdminLegacyController
    {
        public ApFundCatController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundCat");
        }
    }

    /// <summary>ปฏิทินกองทุน / หมวดหมู่ปฏิทิน — ระบบเดิม: mod_tb_calendar_category (tb_calendar_category)</summary>
    public class ApCalendarCatController : AdminLegacyController
    {
        public ApCalendarCatController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApCalendarCat");
            //----- ห้ามลบหมวดหมู่ที่ยังมีวันหยุดผูกอยู่ (ระบบเดิมผูกด้วยคอลัมน์ datatype) -----
            LegacyChildTable = "tb_calendar";
            LegacyChildField = "datatype";
            LegacyParentField = "datatype";
        }

        /// <summary>
        /// ระบบเดิม (mod_tb_calendar_category/edit.aspx): ถ้าแก้ค่า datatype
        /// ต้องอัปเดต tb_calendar.datatype ของแถวเดิมตามไปด้วย ไม่งั้นวันหยุดจะหลุดหมวด
        /// </summary>
        protected override void AfterLegacyUpdate(int id, System.Data.DataRow oldRow, IFormCollection f, Dictionary<string, object> fields)
        {
            string oldType = (oldRow["datatype"] + "").Trim();
            string newType = f.ContainsKey("datatype") ? (f["datatype"] + "").Trim() : oldType;
            if (!string.IsNullOrEmpty(oldType) && !string.IsNullOrEmpty(newType) && oldType != newType)
            {
                _db.ExecuteNonQuery("update [tb_calendar] set datatype = @new where datatype = @old",
                    new Dictionary<string, object>() { { "new", newType }, { "old", oldType } });
            }
        }

        /// <summary>ลบหมวดหมู่ — ระบบเดิมกันไว้ด้วย table_sub (ทำใน AdminLegacyController ผ่าน LegacyChildTable)</summary>
    }

    /// <summary>ปฏิทินกองทุน / ปฏิทินกองทุน — ระบบเดิม: mod_tb_calendar (tb_calendar)</summary>
    public class ApCalendarController : AdminLegacyController
    {
        public ApCalendarController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApCalendar");
        }

        /// <summary>ส่งรายการหมวดหมู่ (datatype) ให้ฟอร์มเลือก — ระบบเดิมใช้ dropdown จาก tb_calendar_category</summary>
        private void SetDataTypeList()
        {
            ViewBag.DataTypeList = _db.ExecuteQuery("select datatype, title, en_title from [tb_calendar_category] where status = 1 order by sort asc, id asc");
        }

        public override IActionResult Create()
        {
            SetDataTypeList();
            return base.Create();
        }

        public override IActionResult Edit(int id)
        {
            SetDataTypeList();
            return base.Edit(id);
        }
    }

    /// <summary>กองทุนสำรองเลี้ยงชีพ / Factsheet (Group) — ระบบเดิม: mod_tb_fund_prov_sheet_cat</summary>
    public class ApProvSheetCatController : AdminLegacyController
    {
        public ApProvSheetCatController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApProvSheetCat");
            //----- ห้ามลบกลุ่มที่ยังมี Factsheet อยู่ (ระบบเดิม: table_sub = tb_fund_prov_sheet) -----
            LegacyChildTable = "tb_fund_prov_sheet";
            LegacyChildField = "cat_id";
        }
    }

    /// <summary>กองทุนสำรองเลี้ยงชีพ / Factsheet — ระบบเดิม: mod_tb_fund_prov_sheet</summary>
    public class ApProvSheetController : AdminLegacyController
    {
        public ApProvSheetController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApProvSheet");
        }
    }

    /// <summary>กองทุนสำรองเลี้ยงชีพ / ข้อมูลอื่นๆ — ระบบเดิม: mod_tb_fund_prov_other</summary>
    public class ApProvOtherController : AdminLegacyController
    {
        public ApProvOtherController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApProvOther");
        }
    }
}
