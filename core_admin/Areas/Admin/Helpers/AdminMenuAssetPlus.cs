namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// นิยามโมดูลของ "เมนูที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus"
    /// (ASP WebForms ที่ <c>http://localhost:8099/assetplus/backoffice/</c>)
    ///
    /// ทุกเมนูในไฟล์นี้ทำงานบน **ตารางเดิม** <c>tb_*</c> ใน database <c>asset_plus_uat</c>
    /// ซึ่ง **ห้ามแก้โครงสร้าง** — ดูรายละเอียดสคีมาที่ <see cref="Controllers.AdminLegacyController"/>
    ///
    /// ที่มาของแต่ละเมนู (โฟลเดอร์ในระบบเดิม):
    ///   หน้าหลัก / Get Other Indices ............ mod_tb_home_other_indices
    ///   ข้อมูลกองทุน / ประเภทกองทุนรวม ......... mod_tb_fund_cat
    ///   ข้อมูลกองทุน / Get Fund Fact Sheet ...... mod_tb_fund_fundfact
    ///   ข้อมูลกองทุน / Get NAV .................. mod_tb_fund_nav
    ///   ข้อมูลกองทุน / Delete NAV ............... mod_tb_fund_nav_del
    ///   ข้อมูลกองทุน / Get Performance .......... mod_tb_fund_performance
    ///   ปฏิทินกองทุน / หมวดหมู่ปฏิทิน .......... mod_tb_calendar_category
    ///   ปฏิทินกองทุน / ปฏิทินกองทุน ............ mod_tb_calendar
    ///   กองทุนสำรองเลี้ยงชีพ / Factsheet (Group)  mod_tb_fund_prov_sheet_cat
    ///   กองทุนสำรองเลี้ยงชีพ / Factsheet ........ mod_tb_fund_prov_sheet
    ///   กองทุนสำรองเลี้ยงชีพ / ข้อมูลอื่นๆ ...... mod_tb_fund_prov_other
    /// </summary>
    public partial class AdminMenu
    {
        //----- ฟิลด์ระบบของตารางเดิม (ใช้ซ้ำทุกเมนู) -----
        private static List<string> LegacyAudit(params string[] fields)
        {
            var l = new List<string>(fields);
            l.AddRange(new[] { "lastcreate", "lastupdate", "sort", "status", "pb_status", "last_user", "pb_last_user", "show_front" });
            return l;
        }
        private static List<string> LegacyAuditUpdate(params string[] fields)
        {
            var l = new List<string>(fields);
            l.AddRange(new[] { "lastupdate", "last_user", "pb_status" });
            return l;
        }

        public List<Module> AssetPlusLegacyModules()
        {
            return new List<Module>()
            {
                #region หน้าหลัก (ระบบเดิม)
                //----- mod_tb_home_other_indices : ดึงดัชนีตลาดจาก web service ตามวันที่ (can_add เท่านั้น) -----
                new Module()
                {
                    Name = "ApOtherIndices",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Get Other Indices",
                        TextBreadcrumb = "หน้าหลัก/Get Other Indices",
                        Table = "tb_home_other_indices",
                        LegacyTable = true,
                        LegacyApproveQueue = false,
                        OrderBy = "ValueDateFormat", Sort = "desc",
                        CanAdd = true, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApOtherIndices",
                        FieldSearch = new() { new("text", new() { "title", "IndexName" }) },
                        ListData = new()
                        {
                            new("title", "Index Name"),
                            new("IndexValue", "Index Value"),
                            new("Change", "Change"),
                            new("PercentChange", "Change (%)"),
                            new("ValueDate", "Value Date"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new()
                        {
                            new("title", "Index Name"), new("IndexValue", "Index Value"), new("Change", "Change"),
                            new("PercentChange", "Change (%)"), new("ValueDate", "Value Date"), new("last_user", "Edit By"),
                        },
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },
                #endregion

                #region ข้อมูลกองทุน (ระบบเดิม)
                //----- mod_tb_fund_cat : ประเภทกองทุนรวม — CRUD เต็ม + จัดลำดับ + อนุมัติ -----
                new Module()
                {
                    Name = "ApFundCat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประเภทกองทุนรวม",
                        TextBreadcrumb = "ข้อมูลกองทุน/ประเภทกองทุนรวม",
                        Table = "tb_fund_cat",
                        LegacyTable = true, LegacyIdManual = true,
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = false, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFundCat", UseViewEditFrom = "ApFundCat",
                        FieldSearch = new() { new("text", new() { "title", "en_title" }) },
                        ListData = new()
                        {
                            new("title", "ชื่อ (ไทย)"),
                            new("en_title", "ชื่อ (อังกฤษ)"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("title", "ชื่อ (ไทย)"), new("en_title", "ชื่อ (อังกฤษ)"), new("last_user", "Edit By") },
                        FieldApprove = new() { "title", "en_title" },
                        FieldCreate = LegacyAudit("title", "en_title"),
                        FieldUpdate = LegacyAuditUpdate("title", "en_title"),
                    }
                },

                //----- mod_tb_fund_fundfact : ดึง Fund Fact Sheet จาก web service ตามวันที่ -----
                new Module()
                {
                    Name = "ApFundFactSheet",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Get Fund Fact Sheet",
                        TextBreadcrumb = "ข้อมูลกองทุน/Get Fund Fact Sheet",
                        Table = "tb_fund_fundfact",
                        LegacyTable = true, LegacyApproveQueue = false,
                        OrderBy = "lastupdate", Sort = "desc",
                        CanAdd = true, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFundFactSheet",
                        FieldSearch = new() { new("text", new() { "title", "fundcode", "FundNameTH", "FundNameEN" }) },
                        ListData = new()
                        {
                            new("title", "Fund Code"),
                            new("FundNameTH", "Fund Name"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("title", "Fund Code"), new("FundNameTH", "Fund Name"), new("last_user", "Edit By") },
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },

                //----- mod_tb_fund_nav : ดึง NAV จาก web service ตามวันที่ -----
                new Module()
                {
                    Name = "ApFundNav",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Get NAV",
                        TextBreadcrumb = "ข้อมูลกองทุน/Get NAV",
                        Table = "tb_fund_nav",
                        LegacyTable = true, LegacyApproveQueue = false,
                        OrderBy = "NAVDateFormat", Sort = "desc",
                        CanAdd = true, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFundNav",
                        FieldSearch = new() { new("text", new() { "title", "FundCode", "FundNameTH", "FundNameEN" }) },
                        ListData = new()
                        {
                            new("title", "Fund"),
                            new("NAVDate", "NAV Date"),
                            new("NAVPerUnit", "NAV"),
                            new("TotalNAV", "มูลค่าทรัพย์สิน"),
                            new("Bid", "BID"),
                            new("Offer", "OFFER"),
                            new("BahtChange", "Change (Baht)"),
                            new("Change", "Change (%)"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new()
                        {
                            new("title", "Fund"), new("NAVDate", "NAV Date"), new("NAVPerUnit", "NAV"), new("TotalNAV", "มูลค่าทรัพย์สิน"),
                            new("Bid", "BID"), new("Offer", "OFFER"), new("BahtChange", "Change (Baht)"), new("Change", "Change (%)"),
                        },
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },

                //----- mod_tb_fund_nav_del : ลบ NAV ตามวันที่ + กองทุนที่เลือก (ทำงานบนตาราง tb_fund_nav เดียวกัน) -----
                new Module()
                {
                    Name = "ApFundNavDelete",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Delete NAV",
                        TextBreadcrumb = "ข้อมูลกองทุน/Delete NAV",
                        Table = "tb_fund_nav",
                        LegacyTable = true, LegacyApproveQueue = false,
                        OrderBy = "NAVDateFormat", Sort = "desc",
                        CanAdd = false, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = false,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewListFrom = "ApFundNavDelete",
                        FieldSearch = new(),
                        ListData = new(),
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },

                //----- mod_tb_fund_performance : ดึงผลการดำเนินงานจาก web service ตามวันที่ -----
                new Module()
                {
                    Name = "ApFundPerformance",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Get Performance",
                        TextBreadcrumb = "ข้อมูลกองทุน/Get Performance",
                        Table = "tb_fund_performance",
                        LegacyTable = true, LegacyApproveQueue = false,
                        OrderBy = "NAVDateFormat", Sort = "desc",
                        CanAdd = true, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFundPerformance",
                        FieldSearch = new() { new("text", new() { "title", "FundCode", "FundNameTH", "FundNameEN" }) },
                        ListData = new()
                        {
                            new("title", "Fund"),
                            new("InceptionDateTH", "Date"),
                            new("NAVPerUnit", "NAV"),
                            new("ThreeMonth", "3M"),
                            new("SixMonth", "6M"),
                            new("OneYear", "1Y"),
                            new("ThreeYear", "3Y"),
                            new("YTD", "YTD"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new()
                        {
                            new("title", "Fund"), new("InceptionDateTH", "Date"), new("NAVPerUnit", "NAV"),
                            new("ThreeMonth", "3M"), new("SixMonth", "6M"), new("OneYear", "1Y"), new("ThreeYear", "3Y"), new("YTD", "YTD"),
                        },
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },
                #endregion

                #region ปฏิทินกองทุน (ระบบเดิม)
                //----- mod_tb_calendar_category : หมวดหมู่ปฏิทิน — datatype ห้ามซ้ำ + แก้แล้ว cascade ไป tb_calendar -----
                new Module()
                {
                    Name = "ApCalendarCat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "หมวดหมู่ปฏิทิน",
                        TextBreadcrumb = "ปฏิทินกองทุน/หมวดหมู่ปฏิทิน",
                        Table = "tb_calendar_category",
                        LegacyTable = true,
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApCalendarCat", UseViewEditFrom = "ApCalendarCat",
                        UniqueFields = new() { "datatype" },
                        FieldSearch = new() { new("text", new() { "title", "en_title", "datatype" }) },
                        ListData = new()
                        {
                            new("datatype", "Data Type"),
                            new("title", "Title"),
                            new("color_code", "Color"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("datatype", "Data Type"), new("title", "Title"), new("en_title", "Title (EN)"), new("color_code", "Color") },
                        FieldApprove = new() { "title", "en_title", "color_code" },
                        FieldCreate = LegacyAudit("datatype", "title", "en_title", "color_code"),
                        FieldUpdate = LegacyAuditUpdate("datatype", "title", "en_title", "color_code"),
                    }
                },

                //----- mod_tb_calendar : ปฏิทินกองทุน (วันหยุด) -----
                new Module()
                {
                    Name = "ApCalendar",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ปฏิทินกองทุน",
                        TextBreadcrumb = "ปฏิทินกองทุน/ปฏิทินกองทุน",
                        Table = "tb_calendar",
                        LegacyTable = true,
                        OrderBy = "holidaydate", Sort = "desc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = false, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApCalendar", UseViewEditFrom = "ApCalendar",
                        FieldSearch = new() { new("text", new() { "holidaydesc", "en_holidaydesc", "fundcode", "datatype" }) },
                        ListData = new()
                        {
                            new("fundcode", "Fundcode"),
                            new("datatype", "Data Type"),
                            new("holidaydesc", "Desc"),
                            new("holidaydate", "Date"),
                            new("createdby", "Created By"),
                            new("pb_status", "สถานะ"),
                        },
                        ExportData = new()
                        {
                            new("fundcode", "Fundcode"), new("datatype", "Data Type"), new("holidaydesc", "Desc"),
                            new("en_holidaydesc", "Desc (EN)"), new("holidaydate", "Date"),
                        },
                        FieldApprove = new() { "fundcode", "holidaydate", "holidaydesc", "en_holidaydesc" },
                        FieldCreate = LegacyAudit("fundcode", "datatype", "holidaydate", "holidaydesc", "en_holidaydesc", "createddate", "createdby"),
                        FieldUpdate = LegacyAuditUpdate("fundcode", "datatype", "holidaydate", "holidaydesc", "en_holidaydesc"),
                    }
                },
                #endregion

                #region กองทุนสำรองเลี้ยงชีพ (ระบบเดิม)
                //----- mod_tb_fund_prov_sheet_cat : กลุ่มของ Factsheet -----
                new Module()
                {
                    Name = "ApProvSheetCat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Factsheet (Group)",
                        TextBreadcrumb = "กองทุนสำรองเลี้ยงชีพ/Factsheet (Group)",
                        Table = "tb_fund_prov_sheet_cat",
                        LegacyTable = true, LegacyIdManual = true,
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApProvSheetCat", UseViewEditFrom = "ApProvSheetCat",
                        FieldSearch = new() { new("text", new() { "title", "en_title" }) },
                        ListData = new()
                        {
                            new("title", "ชื่อ (ไทย)"),
                            new("en_title", "ชื่อ (อังกฤษ)"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("title", "ชื่อ (ไทย)"), new("en_title", "ชื่อ (อังกฤษ)") },
                        FieldApprove = new() { "title", "en_title" },
                        FieldCreate = LegacyAudit("title", "en_title"),
                        FieldUpdate = LegacyAuditUpdate("title", "en_title"),
                    }
                },

                //----- mod_tb_fund_prov_sheet : Factsheet (มีกลุ่ม + ไฟล์/ลิงก์ ทั้ง TH และ EN) -----
                new Module()
                {
                    Name = "ApProvSheet",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Factsheet",
                        TextBreadcrumb = "กองทุนสำรองเลี้ยงชีพ/Factsheet",
                        Table = "tb_fund_prov_sheet",
                        LegacyTable = true, LegacyIdManual = true,
                        TableCate = "tb_fund_prov_sheet_cat", TableCateField = "cat_id",
                        TableCateTitle = "title", TableCateOrderby = "title", TableCateSort = "asc", TableCateLabel = "Group",
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApProvSheet", UseViewEditFrom = "ApProvSheet",
                        FieldSearch = new() { new("text", new() { "title", "en_title" }) },
                        ListData = new()
                        {
                            new("title", "Title"),
                            new("link_type", "ประเภท"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("title", "Title"), new("en_title", "Title (EN)"), new("link_type", "ประเภท"), new("url", "URL") },
                        FieldApprove = new() { "cat_id", "title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type" },
                        FieldCreate = LegacyAudit("cat_id", "title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type"),
                        FieldUpdate = LegacyAuditUpdate("cat_id", "title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type"),
                    }
                },

                //----- mod_tb_fund_prov_other : ข้อมูลอื่นๆ (โครงเดียวกับ Factsheet แต่ไม่มีกลุ่ม) -----
                new Module()
                {
                    Name = "ApProvOther",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข้อมูลอื่นๆ",
                        TextBreadcrumb = "กองทุนสำรองเลี้ยงชีพ/ข้อมูลอื่นๆ",
                        Table = "tb_fund_prov_other",
                        LegacyTable = true, LegacyIdManual = true,
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApProvOther", UseViewEditFrom = "ApProvOther",
                        FieldSearch = new() { new("text", new() { "title", "en_title" }) },
                        ListData = new()
                        {
                            new("title", "Title"),
                            new("link_type", "ประเภท"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new() { new("title", "Title"), new("en_title", "Title (EN)"), new("link_type", "ประเภท"), new("url", "URL") },
                        FieldApprove = new() { "title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type" },
                        FieldCreate = LegacyAudit("title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type"),
                        FieldUpdate = LegacyAuditUpdate("title", "en_title", "img1", "en_img1", "url", "en_url", "url_target", "link_type"),
                    }
                },
                #endregion
            };
        }
    }
}
