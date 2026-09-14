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
    ///   ข้อมูลกองทุน / ประเภทกองทุนรวม / รายชื่อกองทุน .... mod_tb_fund + mod_main_fund         (drill-down)
    ///   ข้อมูลกองทุน / … / รายชื่อกองทุน / เอกสารกองทุน ... mod_tb_fund_doc + mod_main_fund_doc  (drill-down)
    ///   กองทุนส่วนบุคคล / รู้จักกองทุนส่วนบุคคล  mod_tb_fund_private
    ///   กองทุนส่วนบุคคล / ขั้นตอนการลงทุน ....... mod_tb_fund_private_investment_process
    ///   กองทุนส่วนบุคคล / นโยบายการลงทุน ........ mod_tb_fund_private_investment_policy
    ///   กองทุนส่วนบุคคล / คำถามที่พบบ่อย ........ mod_tb_fund_private_investment_qanda
    ///   กองทุนส่วนบุคคล / ติดต่อเรา ............. mod_tb_fund_private_contact_us
    ///   กองทุนส่วนบุคคล / ติดต่อกองทุนส่วนบุคคล . mod_tb_fund_private_interested
    ///   กองทุนสำรองเลี้ยงชีพ / เกี่ยวกับกองทุนฯ .. mod_tb_fund_prov
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

        /// <summary>
        /// เมนู "หน้าเนื้อหาเดี่ยว" ของกองทุนส่วนบุคคล / กองทุนสำรองเลี้ยงชีพ
        /// (ระบบเดิม mod_tb_fund_private*, mod_tb_fund_prov — โครงตารางเหมือนกันทุกตัว:
        ///  title / en_title / info / en_info / img1 / en_img1 + คู่ pb_*)
        ///
        /// <paramref name="editTitle"/> = ฟอร์มให้แก้หัวข้อได้หรือไม่
        /// (ระบบเดิมของ tb_fund_prov ซ่อนช่องหัวข้อไว้ ส่งเป็น hidden แล้วไม่บันทึก จึงแก้ได้เฉพาะเนื้อหา)
        /// </summary>
        private static Module ApPrivatePage(string name, string text, string breadcrumb, string table,
                                            bool canAdd = false, bool canDelete = false, bool canMove = false,
                                            bool canStatus = false, bool editTitle = true)
        {
            var contentFields = editTitle
                ? new[] { "title", "en_title", "info", "en_info" }
                : new[] { "info", "en_info" };

            return new Module()
            {
                Name = name,
                Config = new Module.ModuleConfig()
                {
                    Text = text,
                    TextBreadcrumb = breadcrumb,
                    Table = table,
                    LegacyTable = true, LegacyIdManual = true,
                    OrderBy = "sort", Sort = "asc",
                    CanAdd = canAdd, CanEdit = true, CanDelete = canDelete, CanMove = canMove,
                    CanStatus = canStatus, CanApprove = true, CanExport = false,
                    EnableDateSearch = false, EnableIssueDate = false,
                    UseViewCreateFrom = "ApPrivatePage", UseViewEditFrom = "ApPrivatePage",
                    FieldSearch = new() { new("text", new() { "title", "en_title" }) },
                    ListData = new()
                    {
                        new("title", "หัวข้อ (ไทย)"),
                        new("en_title", "หัวข้อ (อังกฤษ)"),
                        new("pb_status", "สถานะ"),
                        new("lastupdate", "Last Update"),
                        new("last_user", "Edit By"),
                    },
                    ExportData = new() { new("title", "หัวข้อ (ไทย)"), new("en_title", "หัวข้อ (อังกฤษ)"), new("last_user", "Edit By") },
                    //----- ระบบเดิมประกาศ field_approve ชุดนี้เท่ากันทุกเมนู (รวม img1 ที่ฟอร์มไม่ได้ใช้แล้ว) -----
                    FieldApprove = new() { "title", "en_title", "info", "en_info", "img1", "en_img1" },
                    FieldCreate = LegacyAudit("title", "en_title", "info", "en_info"),
                    FieldUpdate = LegacyAuditUpdate(contentFields),
                }
            };
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

                //----- mod_tb_fund (+ mod_main_fund) : รายชื่อกองทุนของหมวดที่เลือก -----
                //      เข้าจากปุ่ม "จัดการกองทุน" ในหน้า ประเภทกองทุนรวม — ไม่มีในเมนูด้านซ้าย (เหมือนระบบเดิม)
                new Module()
                {
                    Name = "ApFund",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อกองทุน",
                        TextBreadcrumb = "ข้อมูลกองทุน/ประเภทกองทุนรวม/รายชื่อกองทุน",
                        Table = "tb_fund",
                        LegacyTable = true, LegacyIdManual = true,
                        TableCate = "tb_fund_cat", TableCateField = "cat_id",
                        TableCateTitle = "title", TableCateOrderby = "sort", TableCateSort = "asc", TableCateLabel = "ประเภทกองทุนรวม",
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFund", UseViewEditFrom = "ApFund",
                        //----- cat_id ต้องอยู่ใน FieldSearch ด้วย ไม่งั้น dropdown "ประเภทกองทุนรวม" จะไม่กรองรายการ
                        //      และค่ากลุ่มที่เลือกจะไม่ถูกจำใน session (ซึ่ง NextSort / LegacyReSort ใช้)
                        FieldSearch = new()
                        {
                            new("text", new() { "title", "en_title", "fundcode" }),
                            new("cat_id", new() { "cat_id" }),
                        },
                        FieldSearchIsEqual = new() { "cat_id" },
                        ListData = new()
                        {
                            new("title", "ชื่อกองทุน"),
                            new("fundcode", "Fund Code"),
                            new("template", "Template"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new()
                        {
                            new("fundcode", "Fund Code"), new("title", "ชื่อกองทุน (ไทย)"), new("en_title", "ชื่อกองทุน (อังกฤษ)"),
                            new("th_currencycode", "สกุลเงิน (ไทย)"), new("en_currencycode", "สกุลเงิน (อังกฤษ)"),
                            new("template", "Template"), new("morning_star", "Morning Star"), new("last_user", "Edit By"),
                        },
                        //----- ตรงกับ field_approve ของ mod_tb_fund/mod_config.aspx (คอลัมน์ seo_* ไม่มีคู่ pb_ จึงไม่อยู่ในชุดนี้) -----
                        FieldApprove = new()
                        {
                            "title", "en_title", "brief", "en_brief", "img1", "en_img1",
                            "title_info", "en_title_info", "cat_id", "fundcode",
                            "template", "morning_star", "th_currencycode", "en_currencycode",
                        },
                        FieldCreate = LegacyAudit(
                            "cat_id", "fundcode", "title", "en_title", "th_currencycode", "en_currencycode",
                            "brief", "en_brief", "title_info", "en_title_info", "img1", "en_img1",
                            "morning_star", "template",
                            "seo_title", "seo_keywords", "seo_description",
                            "seo_en_title", "seo_en_keywords", "seo_en_description"),
                        FieldUpdate = LegacyAuditUpdate(
                            "cat_id", "fundcode", "title", "en_title", "th_currencycode", "en_currencycode",
                            "brief", "en_brief", "title_info", "en_title_info", "img1", "en_img1",
                            "morning_star", "template",
                            "seo_title", "seo_keywords", "seo_description",
                            "seo_en_title", "seo_en_keywords", "seo_en_description"),
                    }
                },

                //----- mod_tb_fund_doc (+ mod_main_fund_doc) : เอกสารของกองทุน -----
                //      เข้าจากปุ่ม "จัดการไฟล์" ในหน้า รายชื่อกองทุน
                //      กลุ่มของเมนูนี้คือ fundcode (ไม่ใช่ id ของตารางแม่) — ระบบเดิมเก็บ fundcode ไว้ทั้งใน cat_id และ fundcode
                //      แต่ละกองทุนมีแถวเอกสารมาตรฐาน 18 รายการ (file_id 1-18) ที่ถูกสร้างอัตโนมัติและลบไม่ได้
                //      ส่วนเอกสารที่ผู้ใช้เพิ่มเองมี file_id = 0 และลบได้
                new Module()
                {
                    Name = "ApFundDoc",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "เอกสารกองทุน",
                        TextBreadcrumb = "ข้อมูลกองทุน/ประเภทกองทุนรวม/รายชื่อกองทุน/เอกสารกองทุน",
                        Table = "tb_fund_doc",
                        LegacyTable = true, LegacyIdManual = false,
                        TableCate = "tb_fund", TableCateField = "fundcode",
                        TableCateTitle = "fundcode", TableCateOrderby = "fundcode", TableCateSort = "asc", TableCateLabel = "กองทุน",
                        OrderBy = "sort", Sort = "asc",
                        CanAdd = true, CanEdit = true, CanDelete = true, CanMove = false, CanStatus = true, CanApprove = true, CanExport = true,
                        EnableDateSearch = false, EnableIssueDate = false,
                        UseViewCreateFrom = "ApFundDoc", UseViewEditFrom = "ApFundDoc",
                        FieldSearch = new()
                        {
                            new("text", new() { "title", "en_title", "file_n" }),
                            new("fundcode", new() { "fundcode" }),
                        },
                        FieldSearchIsEqual = new() { "fundcode" },
                        ListData = new()
                        {
                            new("fundcode", "Fund Code"),
                            new("title", "ชื่อเอกสาร"),
                            new("type_file", "ประเภท"),
                            new("pb_status", "สถานะ"),
                            new("lastupdate", "Last Update"),
                            new("last_user", "Edit By"),
                        },
                        ExportData = new()
                        {
                            new("fundcode", "Fund Code"), new("file_id", "ชนิดเอกสาร"), new("title", "ชื่อเอกสาร (ไทย)"),
                            new("en_title", "ชื่อเอกสาร (อังกฤษ)"), new("type_file", "ประเภท"),
                            new("file1", "ไฟล์ (ไทย)"), new("en_file1", "ไฟล์ (อังกฤษ)"), new("link_file", "URL"),
                        },
                        FieldApprove = new()
                        {
                            "cat_id", "fundcode", "title", "en_title", "file_id",
                            "file1", "en_file1", "file_n", "type_file", "link_file", "url_target",
                        },
                        FieldCreate = LegacyAudit(
                            "cat_id", "fundcode", "title", "en_title", "file_id",
                            "file1", "en_file1", "file_n", "type_file", "link_file", "url_target"),
                        //----- แก้ไข: กองทุน (fundcode/cat_id) และชนิดเอกสาร (file_id) เปลี่ยนไม่ได้ -----
                        FieldUpdate = LegacyAuditUpdate(
                            "title", "en_title", "file1", "en_file1", "file_n", "type_file", "link_file", "url_target"),
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

                #region กองทุนส่วนบุคคล (ระบบเดิม)
                //----- 5 เมนูแรกเป็น "หน้าเนื้อหาเดี่ยว" : ตารางมีระเบียนเดียว แก้ไขได้อย่างเดียว (ยกเว้นคำถามที่พบบ่อย) -----
                ApPrivatePage("ApPrivate", "รู้จักกองทุนส่วนบุคคล",
                              "กองทุนส่วนบุคคล/รู้จักกองทุนส่วนบุคคล", "tb_fund_private"),

                ApPrivatePage("ApPrivateProcess", "ขั้นตอนการลงทุน",
                              "กองทุนส่วนบุคคล/ขั้นตอนการลงทุน", "tb_fund_private_investment_process"),

                ApPrivatePage("ApPrivatePolicy", "นโยบายการลงทุน",
                              "กองทุนส่วนบุคคล/นโยบายการลงทุน", "tb_fund_private_investment_policy"),

                //----- mod_tb_fund_private_investment_qanda : เมนูเดียวในกลุ่มนี้ที่เพิ่ม/ลบ/เปิด-ปิดได้ -----
                ApPrivatePage("ApPrivateQanda", "คำถามที่พบบ่อย",
                              "กองทุนส่วนบุคคล/คำถามที่พบบ่อย", "tb_fund_private_investment_qanda",
                              canAdd: true, canDelete: true, canStatus: true),

                ApPrivatePage("ApPrivateContact", "ติดต่อเรา",
                              "กองทุนส่วนบุคคล/ติดต่อเรา", "tb_fund_private_contact_us"),

                //----- mod_tb_fund_private_interested : กล่องรับข้อมูลผู้สนใจจากหน้าเว็บ (ดู / ลบ / export เท่านั้น) -----
                //      ตารางนี้ไม่มีคอลัมน์ last_user / pb_last_user / title จึงเปิดได้เฉพาะ delete + export
                //      (ระบบเดิมมีปุ่ม Approve ในหน้า list แต่ field_approve เป็นชุดว่าง — กดแล้วไม่มีผลกับเนื้อหาใด)
                new Module()
                {
                    Name = "ApPrivateInterested",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ติดต่อกองทุนส่วนบุคคล",
                        TextBreadcrumb = "กองทุนส่วนบุคคล/ติดต่อกองทุนส่วนบุคคล",
                        Table = "tb_fund_private_interested",
                        LegacyTable = true, LegacyIdManual = true, LegacyApproveQueue = false,
                        OrderBy = "lastcreate", Sort = "desc",
                        CanAdd = false, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false, CanExport = true,
                        EnableDateSearch = true, EnableIssueDate = false,
                        FieldSearch = new() { new("text", new() { "firstname", "surname", "tel", "email" }) },
                        ListData = new()
                        {
                            new("firstname", "ชื่อ"),
                            new("surname", "นามสกุล"),
                            new("tel", "โทรศัพท์"),
                            new("email", "อีเมล"),
                            new("lastcreate", "วันที่ส่ง"),
                        },
                        //----- lastcreate เก็บเป็น unix seconds — แปลงเป็น dd/MM/yyyy ตอน export ให้ตรงกับ export.aspx ของระบบเดิม -----
                        ExportData = new()
                        {
                            new("firstname", "Firstname"), new("surname", "Surname"), new("tel", "Tel"), new("email", "Email"),
                            new("convert(varchar(10), dateadd(hour, 7, dateadd(second, lastcreate, '1970-01-01')), 103)", "Last Create"),
                        },
                        FieldCreate = new(), FieldUpdate = new(),
                    }
                },
                #endregion

                #region กองทุนสำรองเลี้ยงชีพ (ระบบเดิม)
                //----- mod_tb_fund_prov : เกี่ยวกับกองทุนสำรองเลี้ยงชีพ -----
                //      ระบบเดิมซ่อนช่องหัวข้อไว้ (ส่งเป็น hidden แล้วไม่บันทึก) จึงแก้ได้เฉพาะเนื้อหา
                ApPrivatePage("ApProv", "เกี่ยวกับกองทุนสำรองเลี้ยงชีพ",
                              "กองทุนสำรองเลี้ยงชีพ/เกี่ยวกับกองทุนสำรองเลี้ยงชีพ", "tb_fund_prov",
                              editTitle: false),

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
