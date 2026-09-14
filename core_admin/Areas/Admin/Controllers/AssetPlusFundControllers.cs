using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // =========================================================================
    //  เมนูลูกของ "ประเภทกองทุนรวม" ที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus
    //
    //      ประเภทกองทุนรวม (ApFundCat / tb_fund_cat)
    //        └── รายชื่อกองทุน (ApFund / tb_fund)              ← [จัดการกองทุน]
    //              └── เอกสารกองทุน (ApFundDoc / tb_fund_doc)  ← [จัดการไฟล์]
    //
    //  ทั้งสองเมนูไม่อยู่ในเมนูด้านซ้าย (เหมือนระบบเดิม) — เข้าถึงผ่านปุ่มในหน้า list ของเมนูแม่
    //  ระบบเดิม: mod_tb_fund + mod_main_fund / mod_tb_fund_doc + mod_main_fund_doc
    // =========================================================================

    /// <summary>
    /// ข้อมูลกองทุน / ประเภทกองทุนรวม / รายชื่อกองทุน — ระบบเดิม: mod_tb_fund (tb_fund)
    ///
    /// พฤติกรรมที่คัดลอกมาจากระบบเดิม (mod_main_fund/add.aspx, edit.aspx, include/record_manage.aspx):
    ///   • fundcode ห้ามซ้ำทั้งตาราง
    ///   • เพิ่มกองทุนใหม่ → สร้างแถวเอกสารมาตรฐาน 18 รายการใน tb_fund_doc ให้อัตโนมัติ (file_id 1–18)
    ///   • แก้ไขกองทุนที่ยังไม่มีแถวเอกสาร → สร้างให้ย้อนหลัง
    ///   • เปลี่ยน fundcode → ตาม fundcode ใหม่ไปแก้ใน tb_fund_doc ทุกแถว (cat_id / fundcode / pb_*)
    ///   • ลบกองทุน → ลบเอกสารของ fundcode นั้นทิ้งด้วย
    /// </summary>
    public class ApFundController : AdminLegacyController
    {
        public ApFundController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFund");
        }

        /// <summary>ส่งรายการ "ประเภทกองทุนรวม" ให้ dropdown ในฟอร์ม + จำกลุ่มที่กำลังดูอยู่</summary>
        private void SetFundCatList()
        {
            ViewBag.FundCatList = _db.ExecuteQuery("select id, title, en_title from [tb_fund_cat] order by sort asc, id asc");
            ViewBag.CurrentCatId = _session.GetString("admin_" + Module.Name + "_search_cat_id") ?? "";
        }

        public override IActionResult Create()
        {
            SetFundCatList();
            return base.Create();
        }

        public override IActionResult Edit(int id)
        {
            SetFundCatList();
            return base.Edit(id);
        }

        /// <summary>fundcode ห้ามซ้ำ — ระบบเดิมเช็คก่อน insert แล้ว alert + history.back()</summary>
        protected override string? ValidateLegacy(IFormCollection f, int id)
        {
            string code = (f["fundcode"] + "").Trim();
            if (code == "") return "กรุณากรอก Fund Code";

            var dt = _db.ExecuteQuery(
                "select top 1 id from [tb_fund] where LOWER(cast(fundcode as nvarchar(max))) = LOWER(cast(@code as nvarchar(max))) and id <> @id",
                new Dictionary<string, object>() { { "code", code }, { "id", id } });

            return dt.Rows.Count > 0 ? "กองทุนนี้มีในระบบแล้ว กรุณาตรวจสอบอีกครั้ง" : null;
        }

        /// <summary>เพิ่มกองทุนใหม่ → สร้างแถวเอกสารมาตรฐาน 18 รายการให้</summary>
        protected override void AfterLegacyCreate(int newId, IFormCollection f, Dictionary<string, object> fields)
        {
            EnsureFundDocRows((f["fundcode"] + "").Trim());
        }

        /// <summary>
        /// แก้ไขกองทุน — ระบบเดิมทำ 2 อย่างเพิ่ม:
        ///   1) ถ้ายังไม่มีแถวเอกสารมาตรฐานของ fundcode นี้ ให้สร้างย้อนหลัง
        ///   2) ถ้า fundcode เปลี่ยน ต้องตามไปแก้ทุกแถวใน tb_fund_doc ไม่งั้นเอกสารจะหลุดจากกองทุน
        /// </summary>
        protected override void AfterLegacyUpdate(int id, System.Data.DataRow oldRow, IFormCollection f, Dictionary<string, object> fields)
        {
            string oldCode = (oldRow["fundcode"] + "").Trim();
            string newCode = (f["fundcode"] + "").Trim();

            if (oldCode != "" && newCode != "" && oldCode != newCode)
            {
                _db.ExecuteNonQuery(
                    "update [tb_fund_doc] set cat_id = @new, fundcode = @new, pb_cat_id = @new, pb_fundcode = @new where fundcode = @old",
                    new Dictionary<string, object>() { { "new", newCode }, { "old", oldCode } });
            }

            EnsureFundDocRows(newCode);
        }

        /// <summary>ลบกองทุน → ลบเอกสารของ fundcode นั้นทิ้งด้วย (include/record_manage.aspx)</summary>
        protected override void BeforeLegacyDelete(int id, System.Data.DataRow row)
        {
            string code = (row["fundcode"] + "").Trim();
            if (code == "") return;

            _db.ExecuteNonQuery("delete from [tb_fund_doc] where fundcode = @code",
                new Dictionary<string, object>() { { "code", code } });
        }

        /// <summary>
        /// สร้างแถวเอกสารมาตรฐาน 18 รายการของ fundcode นี้ ถ้ายังไม่มี
        /// (ระบบเดิมเช็คด้วย <c>file_id &gt; 0</c> เหมือนกัน — มีอยู่แล้วจะไม่สร้างซ้ำ)
        /// </summary>
        private void EnsureFundDocRows(string fundcode)
        {
            if (string.IsNullOrEmpty(fundcode)) return;

            var exists = _db.ExecuteQuery(
                "select count(id) as c from [tb_fund_doc] where cast(fundcode as nvarchar(max)) = cast(@code as nvarchar(max)) and file_id > 0",
                new Dictionary<string, object>() { { "code", fundcode } });
            if (exists.Rows.Count > 0 && Convert.ToInt32(exists.Rows[0]["c"]) > 0) return;

            long now = UnixNow();
            string user = CurrentUser();

            for (int z = 1; z <= ApFundDocController.FixedDocNames.Length - 1; z++)
            {
                _db.Insert("tb_fund_doc", new Dictionary<string, object>()
                {
                    { "cat_id", fundcode }, { "fundcode", fundcode },
                    { "title", "" }, { "en_title", "" }, { "file_id", z },
                    { "file1", "" }, { "en_file1", "" },
                    { "pb_cat_id", "" }, { "pb_fundcode", "" }, { "pb_title", "" }, { "pb_en_title", "" },
                    { "pb_file_id", 0 }, { "pb_file1", "" }, { "pb_en_file1", "" },
                    { "lastcreate", now }, { "lastupdate", now },
                    { "sort", z * 10 }, { "status", 1 }, { "pb_status", 0 },
                    { "last_user", user }, { "pb_last_user", "" }, { "show_front", 0 },
                    { "file_n", "" }, { "pb_file_n", "" },
                    { "type_file", "file" }, { "pb_type_file", "" },
                    { "link_file", "" }, { "pb_link_file", "" },
                    { "url_target", "" }, { "pb_url_target", "" },
                });
            }
        }
    }

    /// <summary>
    /// ข้อมูลกองทุน / … / เอกสารกองทุน — ระบบเดิม: mod_tb_fund_doc (tb_fund_doc)
    ///
    /// พฤติกรรมที่คัดลอกมาจากระบบเดิม (mod_main_fund_doc/add.aspx, edit.aspx, include/image_copy_*_doc.aspx):
    ///   • แถว file_id 1–18 = เอกสารมาตรฐาน ลบไม่ได้ / เปลี่ยนชื่อเอกสารไม่ได้ (ชื่อมาจากตารางคงที่)
    ///   • แถว file_id = 0 = เอกสารที่ผู้ใช้เพิ่มเอง ลบได้ และตั้งชื่อไฟล์เองผ่านช่อง file_n
    ///   • ไฟล์เก็บที่โฟลเดอร์ upload_otherdocs ของเว็บเดิม (คนละที่กับ upload ของเมนูอื่น)
    ///   • ชื่อไฟล์ถูกกำหนดตายตัว : &lt;fundcode&gt;_&lt;ชื่อชนิดเอกสาร|file_n&gt;[_en].&lt;ext&gt;
    ///   • แก้ file_n แล้วไม่ได้อัปโหลดไฟล์ใหม่ → เปลี่ยนชื่อไฟล์เดิมบนดิสก์ตาม
    /// </summary>
    public class ApFundDocController : AdminLegacyController
    {
        /// <summary>
        /// ชื่อเอกสารมาตรฐาน 18 รายการ (index = file_id, index 0 ไม่ใช้)
        /// ตรงกับ <c>file_id_name[]</c> ใน mod_tb_fund_doc/iframe_data.aspx
        /// </summary>
        public static readonly string[] FixedDocNames = new string[]
        {
            "",
            "หนังสือชี้ชวนส่วนสรุปข้อมูลสำคัญ",
            "หนังสือชี้ชวนส่วนข้อมูลกองทุนรวม",
            "รายงานสถานะกองทุนรายเดือน",
            "เอกสารการขาย",
            "รายงานสรุปจำนวนเงินลงทุนในตราสารหนี้ เงินฝาก หรือตราสารกึ่งหนี้กึ่งทุน",
            "ข้อมูลกองทุนหลัก",
            "รายงานรอบ 6 เดือน",
            "รายงานประจำปี",
            "วันหยุด (เฉพาะกองทุนต่างประเทศ)",
            "ตารางขายคืน สับเปลี่ยน ครบกำหนดอายุ กองทุน",
            "เอกสารข้อมูลนำเสนอ (Presentation)",
            "รายละเอียดเงินลงทุนรายไตรมาส",
            "ตารางรับซื้อคืนหน่วยลงทุนอัตโนมัติ",
            "หนังสือชี้ชวนส่วนข้อมูลโครงการ",
            "เปิดเผยข้อมูล",
            "ASP Next Door",
            "Fund Commentary",
            "VDO แนะนำกองทุน",
        };

        /// <summary>
        /// ส่วนท้ายของชื่อไฟล์ตาม file_id (index = file_id, index 0 ไม่ใช้)
        /// ตรงกับ switch(file_id) ใน include/image_copy_edit_doc.aspx — ห้ามแก้ เพราะเว็บเดิมอ่านไฟล์ด้วยชื่อนี้
        /// </summary>
        public static readonly string[] FixedFileSlugs = new string[]
        {
            "",
            "fundfactsheet", "q&a", "portfoliostatus", "salekit", "fixedreport", "masterfund",
            "semireport", "annualreport", "fundholiday", "fundcalendar", "presentation",
            "qfundallocate", "autoredemptioncalendar", "prospectus", "announcement",
            "nextdoor", "fundcommentary", "vdo",
        };

        /// <summary>ชื่อเอกสารที่จะแสดงในหน้า list — file_id = 0 ใช้ title ที่ผู้ใช้กรอก นอกนั้นใช้ชื่อมาตรฐาน</summary>
        public static string DocName(object? fileId, object? title)
        {
            int id = 0;
            int.TryParse((fileId ?? "") + "", out id);
            if (id > 0 && id < FixedDocNames.Length) return FixedDocNames[id];
            string t = (title ?? "") + "";
            return t == "" ? "-" : t;
        }

        public ApFundDocController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundDoc");
        }

        //======================================================================
        //  โฟลเดอร์ / ชื่อไฟล์
        //======================================================================

        /// <summary>
        /// เอกสารกองทุนเก็บที่ <c>upload_otherdocs</c> ของเว็บเดิม ไม่ใช่ <c>upload</c> เหมือนเมนูอื่น
        /// ตั้งค่าที่ appsettings → LegacyUploadDoc:Path (ไม่ตั้ง = ใช้ LegacyUpload:Path)
        /// </summary>
        protected override string LegacyUploadPath()
        {
            string p = _config["LegacyUploadDoc:Path"] ?? "";
            return string.IsNullOrWhiteSpace(p) ? base.LegacyUploadPath() : p;
        }

        protected override string LegacyUploadUrl()
        {
            string u = _config["LegacyUploadDoc:Url"] ?? "";
            if (string.IsNullOrWhiteSpace(u)) return base.LegacyUploadUrl();
            if (!u.EndsWith("/")) u += "/";
            return u;
        }

        /// <summary>
        /// ชื่อไฟล์ตามกฎของระบบเดิม : <c>&lt;fundcode&gt;_&lt;slug&gt;[_en].&lt;ext&gt;</c>
        ///   • แถวมาตรฐาน (file_id 1–18) → slug มาจาก <see cref="FixedFileSlugs"/>
        ///   • แถวที่ผู้ใช้เพิ่มเอง (file_id = 0) → slug คือค่าในช่อง "ชื่อไฟล์" (file_n)
        /// ชื่อนี้คงที่ต่อ (กองทุน × ชนิดเอกสาร × ภาษา) — อัปโหลดใหม่จึงทับไฟล์เดิมเสมอ เหมือนระบบเดิม
        /// </summary>
        protected override string LegacyUploadFileName(string field, string ext, IFormCollection f)
        {
            string fundcode = (f["fundcode"] + "").Trim();
            string lang = field.StartsWith("en_") ? "_en" : "";

            int fileId = 0;
            int.TryParse((f["file_id"] + "").Trim(), out fileId);

            string slug = (fileId > 0 && fileId < FixedFileSlugs.Length)
                ? FixedFileSlugs[fileId]
                : (f["file_n"] + "").Trim();

            if (fundcode == "" || slug == "")
            {
                //----- ข้อมูลไม่พอจะตั้งชื่อตามกฎเดิม ใช้รูปแบบมาตรฐานของเครื่องยนต์กลางแทน (กันไฟล์ชนกัน) -----
                return base.LegacyUploadFileName(field, ext, f);
            }

            return string.Format("{0}_{1}{2}.{3}", fundcode, slug, lang, ext);
        }

        //======================================================================
        //  ฟอร์ม
        //======================================================================

        /// <summary>ข้อมูลกองทุนที่กำลังดูอยู่ (fundcode + ชื่อประเภทกองทุนรวม) สำหรับหัวฟอร์ม</summary>
        private void SetFundContext(string fundcode)
        {
            ViewBag.CurrentFundCode = fundcode;
            ViewBag.FundCatTitle = "";
            ViewBag.FixedDocNames = FixedDocNames;

            if (string.IsNullOrEmpty(fundcode)) return;

            var dt = _db.ExecuteQuery(
                "select top 1 c.title from [tb_fund] f left join [tb_fund_cat] c on cast(f.cat_id as nvarchar(max)) = cast(c.id as nvarchar(max)) " +
                "where cast(f.fundcode as nvarchar(max)) = cast(@code as nvarchar(max))",
                new Dictionary<string, object>() { { "code", fundcode } });
            if (dt.Rows.Count > 0) ViewBag.FundCatTitle = dt.Rows[0]["title"] + "";
        }

        public override IActionResult Create()
        {
            SetFundContext(_session.GetString("admin_" + Module.Name + "_search_fundcode") ?? "");
            return base.Create();
        }

        public override IActionResult Edit(int id)
        {
            var row = _db.ExecuteQuery("select top 1 fundcode from [tb_fund_doc] where id = @id",
                                       new Dictionary<string, object>() { { "id", id } });
            SetFundContext(row.Rows.Count > 0 ? row.Rows[0]["fundcode"] + "" : "");
            return base.Edit(id);
        }

        //======================================================================
        //  กฎการบันทึก
        //======================================================================

        /// <summary>
        /// ตรวจแบบเดียวกับ save_form() ของระบบเดิม (ฝั่ง server เพื่อไม่ให้ข้ามได้)
        ///   • ประเภท "Download File" ของแถวที่ผู้ใช้เพิ่มเอง ต้องมีชื่อไฟล์
        ///   • ประเภท "URL" ต้องมี URL
        /// </summary>
        protected override string? ValidateLegacy(IFormCollection f, int id)
        {
            string typeFile = (f["type_file"] + "").Trim();
            int fileId = 0;
            int.TryParse((f["file_id"] + "").Trim(), out fileId);

            if (typeFile == "link")
            {
                if ((f["link_file"] + "").Trim() == "") return "กรุณากรอก URL";
                return null;
            }

            //----- เอกสารที่ผู้ใช้เพิ่มเอง : บังคับชื่อเอกสาร 2 ภาษา + ชื่อไฟล์ (ใช้ตั้งชื่อไฟล์จริงบนดิสก์) -----
            if (fileId == 0)
            {
                if ((f["title"] + "").Trim() == "") return "กรุณากรอกชื่อเอกสารภาษาไทย";
                if ((f["en_title"] + "").Trim() == "") return "กรุณากรอกชื่อเอกสารภาษาอังกฤษ";
                if ((f["file_n"] + "").Trim() == "") return "กรุณากรอกชื่อไฟล์";
            }
            return null;
        }

        /// <summary>เอกสารที่เพิ่มเองต้องผูกกับกองทุนที่กำลังดูอยู่ และมี file_id = 0 เสมอ</summary>
        protected override void BeforeLegacyCreate(IFormCollection f, Dictionary<string, object> fields)
        {
            string fundcode = (f["fundcode"] + "").Trim();
            fields["cat_id"] = fundcode;          // ระบบเดิมเก็บ fundcode ไว้ในทั้ง cat_id และ fundcode
            fields["fundcode"] = fundcode;
            fields["file_id"] = 0;

            //----- คู่คอลัมน์ pb_* ที่ฟอร์มไม่ได้ส่งมา : ระบบเดิม insert เป็นค่าว่างไว้ ไม่ใช่ NULL -----
            foreach (string c in new[] { "pb_cat_id", "pb_fundcode", "pb_title", "pb_en_title",
                                         "pb_file1", "pb_en_file1", "pb_file_n", "pb_type_file",
                                         "pb_link_file", "pb_url_target" })
            {
                if (!fields.ContainsKey(c)) fields[c] = "";
            }
            if (!fields.ContainsKey("pb_file_id")) fields["pb_file_id"] = 0;
        }

        /// <summary>
        /// เปลี่ยนชื่อไฟล์บนดิสก์เมื่อผู้ใช้แก้ช่อง "ชื่อไฟล์" (file_n) โดยไม่ได้อัปโหลดไฟล์ใหม่
        /// ตรงกับ mod_main_fund_doc/edit.aspx — ไม่ทำแบบนี้ ไฟล์เดิมจะยังใช้ชื่อเก่าแล้วเว็บหาไม่เจอ
        /// </summary>
        protected override void BeforeLegacyUpdate(int id, System.Data.DataRow oldRow, IFormCollection f, Dictionary<string, object> fields)
        {
            int fileId = 0;
            int.TryParse((oldRow["file_id"] + "") + "", out fileId);
            if (fileId > 0) return;                       // แถวมาตรฐานไม่มีช่อง file_n ให้แก้

            string oldName = (oldRow["file_n"] + "").Trim();
            string newName = (f["file_n"] + "").Trim();
            if (oldName == "" || newName == "" || oldName == newName) return;

            RenameIfKept("file1", oldRow, f, fields, oldName, newName);
            RenameIfKept("en_file1", oldRow, f, fields, oldName, newName);
        }

        /// <summary>เปลี่ยนชื่อไฟล์เดิมของคอลัมน์หนึ่ง — ทำเฉพาะเมื่อไม่ได้อัปโหลดไฟล์ใหม่มาแทน</summary>
        private void RenameIfKept(string field, System.Data.DataRow oldRow, IFormCollection f,
                                  Dictionary<string, object> fields, string oldName, string newName)
        {
            var upload = f.Files.GetFile(field);
            if (upload != null && upload.Length > 0) return;      // มีไฟล์ใหม่แล้ว ชื่อถูกตั้งใหม่ให้เอง

            string current = (oldRow[field] + "").Trim();
            if (current == "") return;

            string renamed = current.Replace(oldName, newName);
            if (renamed == current) return;

            try
            {
                string dir = LegacyUploadPath();
                string from = Path.Combine(dir, current);
                string to = Path.Combine(dir, renamed);
                if (System.IO.File.Exists(from))
                {
                    if (System.IO.File.Exists(to)) System.IO.File.Delete(to);
                    System.IO.File.Move(from, to);
                }
            }
            catch (Exception e)
            {
                //----- เปลี่ยนชื่อไฟล์ไม่สำเร็จ ต้องไม่ทำให้การบันทึกล้ม (ระบบเดิมก็ครอบ try/catch เงียบ ๆ) -----
                _utility.writeLogs("ApFundDoc RenameIfKept (" + field + ") - " + e.Message);
            }

            fields[field] = renamed;
        }

        /// <summary>
        /// ลบได้เฉพาะเอกสารที่ผู้ใช้เพิ่มเอง (file_id = 0)
        /// เอกสารมาตรฐาน 18 รายการเป็นโครงของกองทุน — ระบบเดิมไม่แสดง checkbox/ปุ่มลบให้ด้วยซ้ำ
        /// </summary>
        [HttpPost]
        [ModuleCheck("delete")]
        public override IActionResult Delete(IFormCollection f)
        {
            if (!f.ContainsKey("id") || string.IsNullOrEmpty(f["id"]))
            {
                return base.Delete(f);
            }

            var allowed = new List<string>();
            int blocked = 0;
            foreach (string sid in f["id"].ToString().Split(','))
            {
                if (!_utility.isInt(sid)) continue;
                var dt = _db.ExecuteQuery("select top 1 file_id from [tb_fund_doc] where id = @id",
                                          new Dictionary<string, object>() { { "id", Convert.ToInt32(sid) } });
                if (dt.Rows.Count == 0) continue;

                int fileId = 0;
                int.TryParse(dt.Rows[0]["file_id"] + "", out fileId);
                if (fileId > 0) { blocked++; continue; }
                allowed.Add(sid.Trim());
            }

            if (allowed.Count == 0)
            {
                TempData["alert_message"] = blocked > 0
                    ? "เอกสารมาตรฐานของกองทุนลบไม่ได้ (ลบได้เฉพาะเอกสารที่เพิ่มเองเท่านั้น)"
                    : "กรุณาระบุรายการที่ต้องการลบ";
                TempData["alert_class"] = "alert-warning";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }

            var filtered = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            foreach (var kv in f) filtered[kv.Key] = kv.Value;
            filtered["id"] = string.Join(",", allowed);

            var result = base.Delete(new FormCollection(filtered));
            if (blocked > 0)
            {
                TempData["alert_message"] = (TempData["alert_message"] + "")
                    + string.Format(" (ข้าม {0:n0} รายการ เพราะเป็นเอกสารมาตรฐานของกองทุน)", blocked);
                TempData["alert_class"] = "alert-warning";
            }
            return result;
        }
    }
}
