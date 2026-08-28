using System.Data;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    // เมนูที่เปิดให้กดปุ่ม [Preview] ดูตัวอย่างหน้าเว็บ front-end ของ "ฉบับร่าง" ได้
    //
    // ปุ่มจะโผล่ทุกรายการ ไม่จำกัดสถานะอนุมัติ (เดิมจำกัดเฉพาะ pb_status = 0 "แก้แล้วรออนุมัติ"
    // ซึ่งเป็นกรณีที่ค่าฉบับร่าง — คอลัมน์ไม่มี prefix — ต่างจากค่าที่หน้าเว็บจริงแสดง คือคอลัมน์ pb_*)
    // รายการที่อนุมัติแล้วยังพรีวิวได้ เพียงแต่จะเห็นค่าเท่ากับหน้าเว็บจริง
    //
    // มีพรีวิว 3 แบบ:
    //   item — เมนูที่มี "หน้ารายละเอียดรายตัว" บนเว็บ  -> /_preview/item/{module}/{id}
    //   page — เมนูที่เนื้อหาไปโผล่เป็น section/รายการในหน้าเว็บหน้าหนึ่ง
    //          -> /_preview/page/{module}/{id} (เปิดหน้านั้นทั้งหน้า แต่ข้อมูลเมนูนี้เป็นฉบับร่าง)
    //   cms  — เมนูที่แก้ "ตัวหน้า CMS เอง" -> /_preview/cms/{module}/{id}
    //
    // ⚠ ต้อง sync กับ PreviewMap ฝั่ง front-end (d:\Project\sam.or.th\Helpers\PreviewMap.cs)
    //   เพราะเป็นคนละโปรเจกต์ ใช้ assembly ร่วมกันไม่ได้ — ชื่อโมดูลที่นี่ถูกส่งเป็น segment ใน URL
    public static class PreviewMenu
    {
        // ชื่อโมดูล (ตรงกับ Module.Name / AdminMenu.cs) -> ชนิดพรีวิว
        private static readonly Dictionary<string, string> _supported = new(StringComparer.OrdinalIgnoreCase)
        {
            // ── item: มีหน้ารายละเอียดรายตัว ──
            { "News",           "item" },   // ข่าวประชาสัมพันธ์      -> Views/News/NewsDetail.cshtml
            { "AnnouncePro",    "item" },   // ประกาศจัดซื้อจัดจ้าง   -> Views/Announce/ProcurementDetail.cshtml
            { "AnnounceOther",  "item" },   // ประกาศทั่วไป          -> Views/Announce/GeneralDetail.cshtml
            { "Promotion",      "item" },   // โปรโมชั่นทรัพย์เด่น     -> Views/Assets/PromotionDetail.cshtml

            // ── page: เป็น section/รายการในหน้าเว็บ ──
            { "AboutVision",    "page" },   // วิสัยทัศน์/พันธกิจ
            { "AboutHistory",   "page" },   // ประวัติความเป็นมา
            { "AboutStructure", "page" },   // โครงสร้างองค์กร
            { "AboutAnnual",    "page" },   // รายงานประจำปี
            { "AboutFin",       "page" },   // รายงานการเงิน
            { "AboutBoard1",    "page" },   // คณะกรรมการบริษัท
            { "AboutBoard2",    "page" },   // คณะกรรมการอื่นๆ
            { "AboutBoard3",    "page" },   // คณะผู้บริหารระดับสูง
            { "AboutBoard4",    "page" },   // คณะผู้บริหารฝ่าย
            { "NpaText1",       "page" },   // ขั้นตอนการซื้อทรัพย์
            { "DebtText1",      "page" },   // ภาพรวมการบริหารหนี้
            { "DebtText2",      "page" },   // ขั้นตอนการปรับโครงสร้างหนี้
            { "DebtText3",      "page" },   // สิทธิประโยชน์ลูกหนี้
            { "DebtText5",      "page" },   // ลงทะเบียนปรับโครงสร้างหนี้
            { "DebtText6",      "page" },   // คลินิกแก้หนี้
            { "Article",        "page" },   // บทความและวารสาร
            { "VDO",            "page" },   // วิดีโอ/สื่อประชาสัมพันธ์ (เข้าหน้าชุดวิดีโอที่สังกัด)
            { "AnnounceRate",   "page" },   // ประกาศอัตราดอกเบี้ย
            { "AnnounceNPA",    "page" },   // ประกาศจาก NPL/NPA
            { "AnnounceJob",    "page" },   // ตำแหน่งงาน
            { "ContactOffice",  "page" },   // สำนักงานใหญ่/สาขา
            { "Download",       "page" },   // ดาวน์โหลด
            { "FAQ",            "page" },   // คำถามที่พบบ่อย
            { "Link",           "page" },   // ลิงค์หน่วยงาน
            { "JobCMS",         "page" },   // รูปประกอบ (ร่วมงานกับเรา)
            { "ContactCMS",     "page" },   // ช่องทางร้องเรียน/ข้อเสนอแนะ
            { "LinkCMS",        "page" },   // แลกเปลี่ยนลิงค์
            { "FooterPoll",     "page" },   // แบบสำรวจ
            { "FooterPrivacy1", "page" },   // ประกาศความเป็นส่วนตัว
            { "FooterPrivacy2", "page" },   // ข้อตกลงและเงื่อนไข

            // ── page: widget หน้าแรก (พรีวิวจะเปิด "หน้าแรก" ของเว็บ) ──
            { "HomeImageSlide", "page" },   // รูปสไลด์หน้าแรก
            { "HomeSamText",    "page" },   // SAM ใส่ใจ
            { "HomeSamText2",   "page" },   // ภาพรวมการบริหารหนี้
            { "HomeSamText3",   "page" },   // ปิดหนี้ไว ไปต่อได้
            { "HomeSamText4",   "page" },   // ทรัพย์เด่นและน่าสนใจ
            { "HomeSamText5",   "page" },   // คุณกำลังมองหาอะไร
            { "HomeSamText6",   "page" },   // บ้านเด่นทำเลดี
            { "HomeSamText7",   "page" },   // วิสัยทัศน์ (หน้าแรก)

            // ── page: หมวด/กลุ่ม (พรีวิวจะเปิดหน้าที่หมวดนั้นไปปรากฏ) ──
            //   หมวดที่ front-end ไม่เคยอ่าน (NewsGroup, DownloadGroup, PromotionGroup,
            //   EFormGroup) ไม่ใส่ไว้ เพราะพรีวิวแล้วไม่มีอะไรให้ดู
            { "AboutBoard2cat",  "page" },  // คณะกรรมการอื่นๆ (กลุ่ม)
            { "AboutBoard3cat",  "page" },  // คณะผู้บริหารระดับสูง (กลุ่ม)
            { "AboutBoard4cat",  "page" },  // คณะผู้บริหารฝ่าย (สายงาน)
            { "AboutBoard4sub",  "page" },  // คณะผู้บริหารฝ่าย (กลุ่มย่อย)
            { "ArticleGroup",    "page" },  // บทความและวารสาร (กลุ่ม)
            { "VDOGroup",        "page" },  // วิดีโอ (ประเภท)
            { "VDOGroupsub",     "page" },  // ชุดวิดีโอ/เพลย์ลิสต์
            { "AnnounceOtherGroup","page" },// กลุ่มประกาศทั่วไป (ป้ายบนหน้าประกาศทั่วไป)
            { "AnnounceProGroup","page" },  // ประเภทประกาศจัดซื้อฯ
            { "AnnounceProType", "page" },  // วิธีการจัดซื้อ
            { "FAQGroup",        "page" },  // คำถามที่พบบ่อย (กลุ่ม)
            { "LinkGroup",       "page" },  // ลิงค์หน่วยงาน (กลุ่ม)
            { "NpaLocate",       "page" },  // ย่าน/ทำเล (ตัวกรองค้นหาทรัพย์)
            { "NpaFacility",     "page" },  // Facility (ตัวกรองค้นหาทรัพย์)

            // ── page: ส่วนประกอบที่อยู่บนทุกหน้า / หน้าแรก (ตารางเดี่ยว web_home_*) ──
            { "HomeHeader",     "page" },   // โลโก้/แถบหัวเว็บ
            { "HomeFooter",     "page" },   // ท้ายเว็บ
            { "HomeSEO",        "page" },   // SEO & Code
            { "HomePopUp",      "page" },   // ป๊อปอัพ
            { "HomeImageConf",  "page" },   // ตั้งค่าสไลด์หน้าแรก
            { "HomeIntroPage",  "page" },   // หน้า intro

            // ── cms: แก้ตัวหน้า CMS เอง ──
            { "CMSPage",        "cms" },    // หน้าเว็บไซต์
            { "CMSPageFooter1", "cms" },    // เมนู Footer กลุ่ม 1
            { "CMSPageFooter2", "cms" },    // เมนู Footer กลุ่ม 2
        };

        public static bool IsSupported(string? moduleName) =>
            !string.IsNullOrWhiteSpace(moduleName) && _supported.ContainsKey(moduleName);

        public static string? ModeOf(string? moduleName) =>
            IsSupported(moduleName) ? _supported[moduleName!] : null;

        // ระเบียนนี้พรีวิวได้ไหม
        //  เมนู cms: หน้าเว็บมี 4 ประเภท — 1 = เมนูกลุ่ม, 2 = ลิงก์ออกภายนอก (ทั้งคู่ไม่มีหน้าให้แสดง),
        //            3 = หน้าเนื้อหา, 4 = หน้าที่ดึงเนื้อหาจากโมดูลอื่น -> พรีวิวได้เฉพาะ 3 กับ 4
        //  เมนูอื่น: พรีวิวได้ทุกระเบียน
        //  ⚠ ใช้กับกรณีที่มีแค่ page_type ในมือ — ถ้ามี DBHelper ให้ใช้ PreviewRowGate ซึ่งครอบคลุมกว่า
        public static bool IsPreviewableRow(string? moduleName, string? pageType)
        {
            if (!IsSupported(moduleName)) return false;
            if (_supported[moduleName!] != "cms") return true;
            return pageType == "3" || pageType == "4";
        }

        public static PreviewRowGate CreateGate(string? moduleName, DBHelper db) =>
            new PreviewRowGate(moduleName, db);

        // URL หน้าพรีวิวฝั่ง front-end (ยังไม่ใส่ ?lang= — ฝั่ง JS เติมเองตามปุ่มเลือกภาษา)
        //   frontUrl = ค่าจาก Utility.frontURL()
        public static string BuildUrl(string frontUrl, string moduleName, string id)
        {
            var mode = _supported.TryGetValue(moduleName ?? "", out var m) ? m : "item";
            return $"{(frontUrl ?? "").TrimEnd('/')}/_preview/{mode}/{moduleName}/{id}";
        }
    }

    // ── กฎระดับ "แถว": ปิดปุ่ม [Preview] ของแถวที่พรีวิวแล้ว "ไม่เห็นการเปลี่ยนแปลง" ──────────
    //
    // ที่มา: กวาดทดสอบทุกแถวของทั้ง 64 เมนู (423 แถว) ด้วยการใส่ marker ลงคอลัมน์ฉบับร่างแล้วเปิด
    // หน้าพรีวิวจริง — พบ 7 แถวที่หน้าเว็บ front-end ไม่ได้อ่านค่าของแถวนั้นเลย ปุ่มพรีวิวของแถว
    // เหล่านี้จึงหลอกผู้ใช้ (กดแล้วเหมือนแก้ไม่สำเร็จ) ต้องซ่อนปุ่มทิ้ง
    //
    // คำนวณครั้งเดียวต่อการเรนเดอร์หน้า list (1 query ต่อเมนู) แล้วเก็บเป็น "เซ็ต id ที่ห้ามพรีวิว"
    // จึงไม่ผูกกับว่า list เลือกคอลัมน์ไหนมาแสดง
    //
    // ⚠ ต้อง sync กับ PreviewMap.RowPreviewable() ฝั่ง front-end (d:\Project\sam.or.th\Helpers\PreviewMap.cs)
    //   ซึ่งใช้กฎชุดเดียวกันเพื่อตอบ 404 ถ้ามีคนเปิด URL พรีวิวของแถวที่ปิดไว้
    public sealed class PreviewRowGate
    {
        // box_data ของหน้า CMS ที่ view ฝั่ง front-end เขียนหัวข้อ/เนื้อหาไว้ตายตัว ไม่อ่านระเบียน CMS เลย
        //   npa-sea = ค้นหาทรัพย์สิน, art = บทความและวารสาร (Views/News/Article.cshtml hardcode หัวข้อ),
        //   staff = สำหรับเจ้าหน้าที่, cal = คำนวณสินเชื่อ (Views/Member/{Staff,Calculator}.cshtml)
        public static readonly string[] NoPreviewBoxData = { "npa-sea", "art", "staff", "cal" };

        // เมนู web_core_single ที่ front-end อ่านด้วย LoadCoreSingle() = "ORDER BY id DESC LIMIT 1"
        // (หยิบระเบียน id มากสุดของโมดูลมาแสดงเพียงระเบียนเดียว) -> ระเบียนอื่นพรีวิวไม่เห็นผล
        // ปัจจุบันทุกโมดูลในลิสต์นี้มีระเบียนเดียว กฎจึงยังไม่ตัดใครออก แต่กันไว้เมื่อมีการเพิ่มระเบียนที่ 2
        private static readonly Dictionary<string, int> SingleLatestOnly = new(StringComparer.OrdinalIgnoreCase)
        {
            { "AboutVision", 8 }, { "AboutHistory", 9 }, { "NpaText1", 13 },
            { "DebtText1", 14 }, { "DebtText2", 15 }, { "DebtText3", 16 },
            { "DebtText5", 18 }, { "DebtText6", 19 }, { "JobCMS", 20 },
            { "ContactCMS", 21 }, { "LinkCMS", 22 },
            { "FooterPrivacy1", 24 }, { "FooterPrivacy2", 25 },
            // หมายเหตุ: FooterPoll (module 23) ไม่อยู่ในลิสต์ เพราะหน้าเว็บอ่านแบบแบ่งหน้า (แสดงทุกระเบียน)
        };

        private readonly HashSet<string> _blocked = new(StringComparer.Ordinal);

        public PreviewRowGate(string? moduleName, DBHelper db)
        {
            var mode = PreviewMenu.ModeOf(moduleName);
            if (mode == null || db == null) return;

            try
            {
                switch (mode)
                {
                    case "cms":
                        // ตารางของเมนู cms: CMSPage -> web_cms_page, CMSPageFooter1/2 -> web_cms_page_footer1/2
                        //  ใช้คอลัมน์ "ฉบับร่าง" (page_type / box_data) เพราะหน้าพรีวิวก็อ่านฉบับร่าง
                        string t = moduleName!.Equals("CMSPageFooter1", StringComparison.OrdinalIgnoreCase) ? "web_cms_page_footer1"
                                 : moduleName!.Equals("CMSPageFooter2", StringComparison.OrdinalIgnoreCase) ? "web_cms_page_footer2"
                                 : "web_cms_page";
                        // หน้าแรก (is_home = 1) ไม่ได้เรนเดอร์จากระเบียน CMS — Home/Index.cshtml ประกอบจาก
                        // widget หน้าแรก (web_home_*, web_core_single) ทั้งหน้า -> พรีวิวแล้วไม่เห็นค่าที่แก้
                        // (คอลัมน์ is_home มีเฉพาะ web_cms_page)
                        string homeCond = t == "web_cms_page" ? " OR COALESCE(is_home,'') = '1'" : "";
                        Block(db, $@"SELECT id FROM {Db.T(t)}
                                     WHERE COALESCE(page_type,'') NOT IN ('3','4')
                                        OR COALESCE(box_data,'') = ANY(@box){homeCond}",
                              new() { { "box", NoPreviewBoxData } });
                        break;

                    // ลิงค์หน่วยงาน (กลุ่ม): Views/Contact/Organization.cshtml ข้ามกลุ่มที่ไม่มีลิงก์ทั้งกลุ่ม
                    //   (if members.Count == 0 -> continue) -> กลุ่มว่างพรีวิวไม่เห็นอะไร
                    case "page" when moduleName!.Equals("LinkGroup", StringComparison.OrdinalIgnoreCase):
                        Block(db, @"SELECT g.id FROM [2026_web_core_group] g
                                    WHERE g.module_id = 15 AND NOT EXISTS (
                                        SELECT 1 FROM [2026_web_core_news] n
                                        WHERE n.module_id = 12 AND CAST(n.cat_id AS nvarchar(max)) = CAST(g.id AS nvarchar(max))
                                          AND n.status = '1' AND n.show_front = '1'
                                          AND (n.pb_issue_date_config = '1'
                                               OR (n.pb_issue_date <= SYSDATETIMEOFFSET() AND n.pb_expiry_date >= SYSDATETIMEOFFSET())))");
                        break;

                    // คณะผู้บริหารระดับสูง (กลุ่ม): Views/About/Executives.cshtml
                    //   - แสดงเฉพาะกลุ่มที่มีสมาชิก (LoadCoreGroupsByIds จาก pb_cat_id ของ item module 7)
                    //   - และ "ไม่แสดงหัวข้อกลุ่มแรก" (if groupIndex > 0) -> กลุ่มแรกตาม sort พรีวิวไม่เห็นผล
                    case "page" when moduleName!.Equals("AboutBoard3cat", StringComparison.OrdinalIgnoreCase):
                        Block(db, @"WITH shown AS (
                                        SELECT g.id, g.sort FROM [2026_web_core_group] g
                                        WHERE g.module_id = 2 AND EXISTS (
                                            SELECT 1 FROM [2026_web_core_item] i
                                            WHERE i.module_id = 7 AND CAST(i.cat_id AS nvarchar(max)) = CAST(g.id AS nvarchar(max))
                                              AND i.status = '1' AND i.show_front = '1')
                                    )
                                    SELECT g.id FROM [2026_web_core_group] g
                                    WHERE g.module_id = 2
                                      AND (g.id NOT IN (SELECT id FROM shown)
                                           OR g.id = (SELECT TOP 1 id FROM shown ORDER BY sort ASC, id ASC))");
                        break;

                    // รายการ ย่าน/ทำเล: หน้าแรกสร้างชิปย่านจาก "ช่องย่าน" ของ widget บ้านเด่นทำเลดี
                    //   (web_core_single module 6 คอลัมน์ pb_t2..pb_t11 เก็บ id ของ web_core_group module 18)
                    //   ย่านที่ไม่ได้ถูกเลือกไว้ในช่องเหล่านี้ ไม่ถูกเรนเดอร์ที่ไหนเลย -> พรีวิวไม่เห็นผล
                    case "page" when moduleName!.Equals("NpaLocate", StringComparison.OrdinalIgnoreCase):
                        Block(db, @"SELECT g.id FROM [2026_web_core_group] g
                                    WHERE g.module_id = 18
                                      AND CAST(g.id AS nvarchar(max)) NOT IN (
                                        SELECT x.v FROM [2026_web_core_single] s
                                          CROSS APPLY (VALUES (s.pb_t2),(s.pb_t3),(s.pb_t4),(s.pb_t5),(s.pb_t6),
                                                          (s.pb_t7),(s.pb_t8),(s.pb_t9),(s.pb_t10),(s.pb_t11)) AS x(v)
                                        WHERE s.module_id = 6 AND COALESCE(x.v,'') <> '')");
                        break;

                    case "page" when SingleLatestOnly.ContainsKey(moduleName!):
                        Block(db, @"SELECT id FROM [2026_web_core_single]
                                    WHERE module_id = @mid
                                      AND id <> (SELECT MAX(id) FROM [2026_web_core_single] WHERE module_id = @mid)",
                              new() { { "mid", SingleLatestOnly[moduleName!] } });
                        break;
                }
            }
            catch
            {
                // กฎนี้เป็นแค่ "ตัวช่วยซ่อนปุ่ม" — ถ้า query พลาดต้องไม่ทำให้หน้า list ล่ม
                _blocked.Clear();
            }
        }

        private void Block(DBHelper db, string sql, Dictionary<string, object>? ps = null)
        {
            var dt = db.ExecuteQuery(sql, ps ?? new Dictionary<string, object>());
            foreach (DataRow r in dt.Rows)
                _blocked.Add(r["id"]?.ToString() ?? "");
        }

        public bool CanPreview(string? id) =>
            !string.IsNullOrEmpty(id) && !_blocked.Contains(id);

        public bool CanPreview(DataRow row) =>
            row != null && row.Table.Columns.Contains("id") && CanPreview(row["id"]?.ToString());
    }
}
