namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class Module
    {
        public string? Name;
        public ModuleConfig Config;

        public struct ModuleConfig
        {
            public string? Text { get; set; } = null;
            public string? TextBreadcrumb { get; set; } = null;
            public string? Table { get; set; } = null;
            public int? TableModuleID { get; set; } = 0;
            public string? TableCate { get; set; } = null;
            public string? TableCateField { get; set; } = null;
            public string? TableCateTitle { get; set; } = null;
            public string? TableCateLabel { get; set; } = "กลุ่ม";
            public string? TableCateOrderby { get; set; } = null;
            public string? TableCateSort { get; set; } = null;
            public string? TableCateModuleID { get; set; } = null;
            public bool? CanAdd { get; set; } = false;
            public bool? CanEdit { get; set; } = false;
            public bool? CanDelete { get; set; } = false;
            public bool? CanMove { get; set; } = false;
            public bool? CanStatus { get; set; } = false;
            public bool? CanExport { get; set; } = false;
            public bool? CanApprove { get; set; } = false;
            public bool? EnableDateSearch { get; set; } = false;
            /// <summary>
            /// แสดงส่วน "วันที่แสดงผล (แสดงตลอดเวลา / กำหนดเอง)" ในหน้า Create / Edit หรือไม่
            /// false = ซ่อนทั้งส่วน แล้วส่ง issue_date_config = 1 (แสดงตลอดเวลา) เป็น hidden input ให้เสมอ
            /// ใช้กับเมนูที่ front-end มีระบบเลือกนำไปแสดงเองอยู่แล้ว (เช่น NpaLocate / NpaFacility)
            /// </summary>
            public bool? EnableIssueDate { get; set; } = true;
            public List<KeyValuePair<string, List<string>>>? FieldSearch { get; set; } = new(){};
            public List<string> FieldSearchIsEqual { get; set; } = null;
            public string? OrderBy { get; set; } = "created_at";
            public string? Sort { get; set; } = "ASC";
            public int Page { get; set; } = 1;
            public int PerPage { get; set; } = 10;
            public List<KeyValuePair<string, string>>? ListData { get; set; } = new()
            {
                new KeyValuePair<string, string>("title", "หัวข้อ"),
                new KeyValuePair<string, string>("created_at", "วันที่สร้าง"),
                new KeyValuePair<string, string>("updated_at", "วันที่แก้ไข"),
                new KeyValuePair<string, string>("created_by", "สร้างโดย"),
                new KeyValuePair<string, string>("updated_by", "แก้ไขโดย"),
            };
            public List<KeyValuePair<string, string>> ExportData { get; set; } = new()
            {
                new KeyValuePair<string, string>("title", "หัวข้อ"),
                new KeyValuePair<string, string>("created_at", "วันที่สร้าง"),
                new KeyValuePair<string, string>("updated_at", "วันที่แก้ไข"),
                new KeyValuePair<string, string>("created_by", "สร้างโดย"),
                new KeyValuePair<string, string>("updated_by", "แก้ไขโดย"),
            };
            public List<string>? FieldApprove { get; set; } = null;
            public List<string>? FieldCreate { get; set; } = new()
            {
                "created_at", 
                "updated_at", 
                "created_by", 
                "updated_by"
            };
            public List<string>? FieldUpdate { get; set; } = new()
            {
                "updated_at",
                "updated_by"
            };
            public bool? IsCMSPage { get; set; } = false;
            public string FieldCMSPage { get; set; } = "parent_id";
            public bool? EnableViewDetail { get; set; } = false;
            public List<KeyValuePair<string, string>>? ViewDetailData { get; set; } = null;           
            /// <summary>
            /// ชื่อฟิลด์ใน ListData ที่จะแสดงเป็นลิงก์ไปยังเว็บ front-end (null = ปิด)
            /// ลิงก์ = FrontURL (จาก appsettings) + "/" + FrontLinkPrefix + ค่าในฟิลด์
            /// ต้องใช้ฟิลด์ที่ front-end ใช้ค้นหาจริง (เช่น pb_url ไม่ใช่ url)
            /// </summary>
            public string? FrontLinkField { get; set; } = null;
            public string FrontLinkPrefix { get; set; } = "th/";
            /// <summary>
            /// ฟิลด์ที่ต้องมีค่า = "1" ครบทุกตัว หน้านั้นจึงจะแสดงบน front-end จริง
            /// ถ้าไม่ครบ จะแสดง URL เป็นข้อความ + ป้ายเตือน แทนลิงก์ที่กดแล้วไม่เจอ
            /// </summary>
            public List<string>? FrontLinkRequire1Fields { get; set; } = null;
            public string UseViewListFrom { get; set; } = "AdminCore";
            public string UseViewDetailFrom { get; set; } = "AdminCore";
            public string UseViewCreateFrom { get; set; } = "AdminCore";
            public string UseViewEditFrom { get; set; } = "AdminCore";
            public ModuleConfig(){}
        }
    }

}
