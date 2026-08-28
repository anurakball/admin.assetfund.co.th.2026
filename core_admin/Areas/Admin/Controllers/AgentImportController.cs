using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using thaicredit_hr_admin.Areas.Admin.Filters;

using thaicredit_hr_admin.Areas.Admin.Helpers;
namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AgentImportController : AdminCoreController
    {
        // โครงคอลัมน์ของไฟล์ (หัวตาราง/ลำดับ/สี) อยู่ที่ Helpers/AgentImportSchema.cs ที่เดียว
        // — ใช้ร่วมกับไฟล์ Export ของเมนู "รายชื่อผู้สมัครตัวแทน" (AgentList) เพื่อให้ 2 ไฟล์เหมือนกันเป๊ะ
        private const int NpaStartColumn = Helpers.AgentImportSchema.NpaStartColumn;
        private static int NpaSlotCount => Helpers.AdminMenu.NpaSlotCount;
        private static int TotalColumns => Helpers.AgentImportSchema.TotalColumns;

        // ─────────────────────────────────────────────────────────────────────────
        //  กฎตรวจรูปแบบข้อมูล — ต้องตรงกับฟอร์มสมัครฝั่ง front-end (d:\Project\sam.or.th)
        //  เพื่อให้ข้อมูลที่เข้ามาทาง Import กับทางหน้าเว็บอยู่ในรูปแบบเดียวกันเสมอ
        //
        //   • อีเมล    /th/register            Views/Member/Register.cshtml
        //              /^[^@\s]+@[^@\s]+\.[^@\s]+$/
        //   • มือถือ   /th/register            ตัดอักขระที่ไม่ใช่ตัวเลขทิ้งก่อน แล้วต้องตรง /^0\d{9}$/ (10 หลัก ขึ้นต้น 0)
        //   • เลขบัตร  /th/apply-agent/step2   input name="pid" — ตัวเลขล้วน ยาว 13 หลัก
        //              (maxlength="13" inputmode="numeric" + oninput ตัดอักขระที่ไม่ใช่ตัวเลข)
        //   • โทรศัพท์ /th/apply-agent/step2   CUS_BASE_PHONE / CUS_CONTACT_PHONE — ตัวเลขล้วน ไม่เกิน 10 หลัก
        //              (เบอร์บ้านต่างจังหวัด 9 หลักก็ใช้ได้ จึงยอมรับ 9–10 หลัก)
        // ─────────────────────────────────────────────────────────────────────────
        private static readonly System.Text.RegularExpressions.Regex EmailRx =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static readonly System.Text.RegularExpressions.Regex MobileRx =
            new(@"^0\d{9}$", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static readonly System.Text.RegularExpressions.Regex NonDigitRx =
            new(@"[^0-9]", System.Text.RegularExpressions.RegexOptions.Compiled);
        // แท็ก HTML ในช่องข้อความ — ชื่อ/ที่อยู่ไม่มีทางมี tag จริง ๆ ตัดทิ้งกัน payload หลุดลง DB
        private static readonly System.Text.RegularExpressions.Regex HtmlTagRx =
            new(@"<[^>]*>?", System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>เหลือเฉพาะตัวเลข (เทียบเท่า <c>value.replace(/[^0-9]/g, "")</c> ของฟอร์มหน้าเว็บ)</summary>
        private static string Digits(string? s) => NonDigitRx.Replace(s ?? "", "");

        /// <summary>
        /// ล้างช่องข้อความอิสระก่อนเก็บลง DB — ตัดแท็ก HTML + อักขระควบคุมทิ้ง
        /// (ป้องกันเชิงลึกอีกชั้นของ stored XSS นอกเหนือจากการ escape ตอน render)
        /// </summary>
        private static string Clean(string? s)
        {
            var v = (s ?? "").Trim();
            if (v.Length == 0) return "";
            v = HtmlTagRx.Replace(v, "");
            v = new string(v.Where(c => !char.IsControl(c)).ToArray());
            return v.Trim();
        }

        public AgentImportController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
            : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AgentImport");
        }

        public override IActionResult Index()
        {
            if (TempData["import_success"] != null)
                ViewBag.ImportSuccess = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(TempData["import_success"]!.ToString()!);
            if (TempData["import_failed"] != null)
                ViewBag.ImportFailed = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(TempData["import_failed"]!.ToString()!);

            ViewBag.Module = Module;
            ViewBag.Title = Module.Config.Text;
            ViewBag.ModuleName = Module.Name;
            ViewBag._utility = _utility;
            ViewBag._admin = _admin;
            ViewBag._session = _session;
            ViewBag._db = _db;
            ViewBag.searchInputVal = new Dictionary<string, string>();
            return View("~/Areas/Admin/Views/AgentImport/Index.cshtml");
        }

        [HttpGet]
        public IActionResult DownloadTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();

            // ไฟล์มี 2 ชีต:
            //   ชีตที่ 1 "Import Agent"   = ชีตที่ระบบอ่านตอนนำเข้า — มีแต่หัวคอลัมน์ ให้กรอกข้อมูลจริงลงไป
            //   ชีตที่ 2 "ตัวอย่างข้อมูล"    = ตัวอย่าง 5 รายการกรอกครบทุกช่อง ไว้ดู/คัดลอกไปวางในชีตที่ 1
            // แยกชีตเพราะระบบอ่าน Worksheets[0] เสมอ — ถ้าวางตัวอย่างไว้ชีตแรก
            // คนที่อัปโหลดไฟล์ template ทั้งที่ยังไม่แก้จะนำเข้าข้อมูลตัวอย่างเข้าระบบจริงทันที
            var ws       = excel.Workbook.Worksheets.Add("Import Agent");
            var wsSample = excel.Workbook.Worksheets.Add("ตัวอย่างข้อมูล");

            // หัวคอลัมน์ + ลำดับ + สี มาจาก AgentImportSchema (ตัวเดียวกับที่ AgentList ใช้ตอน Export)
            var columns = Helpers.AgentImportSchema.Columns;
            Helpers.AgentImportSchema.WriteHeader(ws);
            Helpers.AgentImportSchema.WriteHeader(wsSample);

            // ── ตัวอย่าง 5 รายการ — กรอกครบทุกช่อง ใช้รหัส Master Data จริง ──
            //    10/1004/100402 = กรุงเทพฯ/เขตบางรัก/แขวงสีลม        50/5001/500101 = เชียงใหม่/เมืองเชียงใหม่/ศรีภูมิ
            //    20/2001/200104 = ชลบุรี/เมืองชลบุรี/แสนสุข            40/4001/400101 = ขอนแก่น/เมืองขอนแก่น/ในเมือง
            //    83/8301/830101 = ภูเก็ต/เมืองภูเก็ต/ตลาดใหญ่
            var samples = new List<string[]>
            {
                // บุคคลธรรมดา (type=2) — กรุงเทพฯ ที่อยู่ติดต่อเดียวกับทะเบียนบ้าน
                new[] { "2", "นาย", "", "สมชาย", "ใจดี", "somchai2569", "somchai.j@example.com", "0812345678", "",
                        "1", "สมชาย", "ใจดี", "1100200300401",
                        "12/34", "5", "ศาลาแดง 2", "สีลม",   "100402", "1004", "10", "10500",
                        "12/34", "5", "ศาลาแดง 2", "สีลม",   "100402", "1004", "10", "10500",
                        "021234567", "0812345678" },
                // บุคคลธรรมดา (type=2) — เชียงใหม่
                new[] { "2", "นางสาว", "", "สุดา", "ทองดี", "sudathong01", "suda.t@example.com", "0898765432", "",
                        "3", "สุดา", "ทองดี", "3500100200302",
                        "99", "4", "ช้างเผือก 5", "ห้วยแก้ว", "500101", "5001", "50", "50200",
                        "99", "4", "ช้างเผือก 5", "ห้วยแก้ว", "500101", "5001", "50", "50200",
                        "053112233", "0898765432" },
                // นิติบุคคล (type=1) — ชลบุรี, ที่อยู่ติดต่อคนละที่กับที่ตั้งบริษัท
                new[] { "1", "อื่นๆ", "บริษัท", "บริษัท เอสเอเอ็ม พร็อพเพอร์ตี้ จำกัด", "", "samproperty01", "contact@samproperty.example.com", "0865551234", "",
                        "4", "บริษัท เอสเอเอ็ม พร็อพเพอร์ตี้ จำกัด", "", "0205500001234",
                        "168/9", "3", "สุขุมวิท 42", "สุขุมวิท", "200104", "2001", "20", "20000",
                        "77/1", "1", "บางแสนสาย 4", "ลงหาดบางแสน", "200104", "2001", "20", "20000",
                        "038112244", "0865551234" },
                // บุคคลธรรมดา (type=2) — ขอนแก่น + ผูกรหัสทรัพย์หลายรายการ
                new[] { "2", "นาง", "", "วิภา", "ศรีสุข", "wipasrisuk", "wipa.s@example.com", "0821119876", "",
                        "2", "วิภา", "ศรีสุข", "3400500600703",
                        "45/6", "12", "มิตรภาพ 15", "มิตรภาพ", "400101", "4001", "40", "40000",
                        "45/6", "12", "มิตรภาพ 15", "มิตรภาพ", "400101", "4001", "40", "40000",
                        "043223344", "0821119876" },
                // นิติบุคคล (type=1) — ภูเก็ต
                new[] { "1", "อื่นๆ", "ห้างหุ้นส่วนจำกัด", "หจก. อันดามัน เอสเตท", "", "andamanestate", "info@andaman-estate.example.com", "0843337788", "",
                        "4", "หจก. อันดามัน เอสเตท", "", "0833500002345",
                        "88", "2", "ถลาง 7", "เทพกระษัตรี", "830101", "8301", "83", "83000",
                        "88", "2", "ถลาง 7", "เทพกระษัตรี", "830101", "8301", "83", "83000",
                        "076334455", "0843337788" },
            };
            // รหัสทรัพย์ตัวอย่าง (คอลัมน์ที่เหลือเว้นว่าง) — ช่องที่ไม่ได้ระบุจะถูกเติมเป็นค่าว่างให้ครบ 30 ช่อง
            var sampleNpa = new List<string[]>
            {
                new[] { "8Z3954" },
                new[] { "1T2428", "8Z4461" },
                new[] { "8Z4390" },
                new[] { "3A0575", "8Z3954", "1T2428" },
                Array.Empty<string>(),
            };

            for (int r = 0; r < samples.Count; r++)
            {
                var line = samples[r]
                    .Concat(Enumerable.Range(0, NpaSlotCount)
                        .Select(k => k < sampleNpa[r].Length ? sampleNpa[r][k] : ""))
                    .ToArray();
                for (int i = 0; i < Math.Min(line.Length, columns.Count); i++)
                {
                    // บังคับเป็น "ข้อความ" ทุกช่อง — กันเลขบัตร/เบอร์โทรที่ขึ้นต้นด้วย 0 โดนตัดศูนย์หน้าทิ้ง
                    var cell = wsSample.Cells[r + 2, i + 1];
                    cell.Style.Numberformat.Format = "@";
                    cell.Value = line[i];
                }
            }

            // ชีตกรอกข้อมูลตั้งรูปแบบทั้งคอลัมน์เป็น "ข้อความ" — กัน Excel แปลง "0812345678"
            // เป็นตัวเลข 812345678 (ศูนย์หน้าหาย) ตอนผู้ใช้พิมพ์เอง
            Helpers.AgentImportSchema.SetTextFormat(ws);

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            wsSample.Cells[wsSample.Dimension.Address].AutoFitColumns();

            string importDir = Path.Combine(_hostingEnvironment.WebRootPath, "Files", "import");
            Directory.CreateDirectory(importDir);
            var fileBytes = excel.GetAsByteArray();
            System.IO.File.WriteAllBytes(Path.Combine(importDir, "agent_import_template.xlsx"), fileBytes);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "agent_import_template.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import(IFormFile importFile)
        {
            var successList = new List<Dictionary<string, string>>();
            var failedList  = new List<Dictionary<string, string>>();

            if (importFile == null || importFile.Length == 0)
            {
                TempData["alert_message"] = "กรุณาเลือกไฟล์ Excel";
                TempData["alert_class"] = "alert-warning";
                return RedirectToAction("Index");
            }

            string ext = Path.GetExtension(importFile.FileName).ToLower();
            if (ext != ".xlsx" && ext != ".xls")
            {
                TempData["alert_message"] = "กรุณาเลือกไฟล์ Excel (.xlsx, .xls) เท่านั้น";
                TempData["alert_class"] = "alert-warning";
                return RedirectToAction("Index");
            }

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var stream  = importFile.OpenReadStream();
                using var package = new ExcelPackage(stream);
                var ws = package.Workbook.Worksheets[0];

                if (ws.Dimension == null || ws.Dimension.Rows < 2)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลในไฟล์ Excel (ต้องมีข้อมูลอย่างน้อย 1 แถวหลัง header)";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                int rowCount = ws.Dimension.Rows;
                // ผูก offset ของเครื่องไปด้วย เพราะคอลัมน์ปลายทางเป็น datetimeoffset
                var now = new DateTimeOffset(DateTime.Now);

                for (int row = 2; row <= rowCount; row++)
                {
                    string G(int col) => ws.Cells[row, col].Text?.Trim() ?? "";
                    // ช่องข้อความอิสระอ่านผ่าน Clean() เพื่อตัดแท็ก HTML/อักขระควบคุมออกก่อนเสมอ
                    string C(int col) => Clean(G(col));

                    int.TryParse(G(1), out int mem_type);
                    if (mem_type < 1 || mem_type > 2) mem_type = 1;
                    string mem_title     = C(2);
                    string mem_titleoth  = C(3);
                    string mem_firstname = C(4);
                    string mem_surname   = C(5);
                    string mem_username  = C(6);
                    string mem_email     = C(7);
                    string mem_mobile    = G(8);
                    string mem_lineuid   = C(9);

                    string agt_title_str     = G(10);
                    string agt_name          = C(11);
                    string agt_surname       = C(12);
                    string agt_idcard        = G(13);
                    string agt_base_addr     = C(14);
                    string agt_base_moo      = C(15);
                    string agt_base_soi      = C(16);
                    string agt_base_road     = C(17);
                    string agt_base_tambol   = G(18);
                    string agt_base_amphur   = G(19);
                    string agt_base_province = G(20);
                    string agt_base_zipcode  = G(21);
                    string agt_contact_arrr    = C(22);
                    string agt_contact_moo     = C(23);
                    string agt_contact_soi     = C(24);
                    string agt_contact_road    = C(25);
                    string agt_contact_tambol  = G(26);
                    string agt_contact_amphur  = G(27);
                    string agt_contact_province = G(28);
                    string agt_contact_zipcode  = G(29);
                    string agt_base_phone    = G(30);
                    string agt_contact_phone = G(31);

                    // เบอร์โทร/เลขบัตร: ตัดอักขระที่ไม่ใช่ตัวเลขทิ้งก่อนตรวจและก่อนบันทึก
                    // เหมือนที่ฟอร์มหน้าเว็บทำตอนพิมพ์ — "081-234-5678" = "0812345678",
                    // "100-000-0000-3" = "1000000000003" (ทำให้จับซ้ำกับระเบียนเดิมได้ ไม่สร้างคนซ้ำ)
                    mem_mobile        = Digits(mem_mobile);
                    agt_idcard        = Digits(agt_idcard);
                    agt_base_phone    = Digits(agt_base_phone);
                    agt_contact_phone = Digits(agt_contact_phone);
                    // รหัสทรัพย์ NPA — คอลัมน์ขวาสุดของไฟล์ (NpaStartColumn .. TotalColumns)
                    // ไฟล์ template รุ่นเก่าที่มีคอลัมน์น้อยกว่ายังใช้ได้: G() ที่เลยขอบไฟล์คืน "" (ช่องที่เหลือเป็น NULL)
                    var npaids = new string[NpaSlotCount];
                    for (int k = 0; k < NpaSlotCount; k++)
                        npaids[k] = G(NpaStartColumn + k);

                    // แถวว่างจริง ๆ (ท้ายไฟล์) ข้ามเงียบ ๆ — เทียบจากค่าดิบในเซลล์ ไม่ใช่ค่าที่ normalize แล้ว
                    // เพื่อไม่ให้แถวที่กรอกมาแบบผิดรูปแบบล้วน (เช่นเลขบัตรเป็นตัวอักษร) หายไปโดยไม่มีรายงาน
                    if (string.IsNullOrEmpty(G(6)) && string.IsNullOrEmpty(G(7)) && string.IsNullOrEmpty(G(13)))
                        continue;

                    string skipReason = "";

                    // ---- ช่องบังคับ + รูปแบบ (กฎเดียวกับฟอร์มสมัครหน้าเว็บ ดูหมายเหตุบนสุดของไฟล์) ----
                    if (string.IsNullOrEmpty(mem_username))
                        skipReason = "ไม่มี Username";
                    else if (string.IsNullOrEmpty(mem_email))
                        skipReason = "ไม่มี Email";
                    else if (!EmailRx.IsMatch(mem_email))
                        skipReason = $"รูปแบบอีเมลไม่ถูกต้อง (\"{mem_email}\")";
                    else if (mem_email.Length > 200)
                        skipReason = "อีเมลยาวเกิน 200 ตัวอักษร";
                    // กรอกมาแต่ไม่มีตัวเลขเลย (เช่น "abcxyz") ถือเป็น "รูปแบบผิด" ไม่ใช่ "ไม่ได้กรอก"
                    else if (string.IsNullOrEmpty(mem_mobile) && string.IsNullOrEmpty(G(8)))
                        skipReason = "ไม่มีเบอร์มือถือ";
                    else if (!MobileRx.IsMatch(mem_mobile))
                        skipReason = $"รูปแบบเบอร์มือถือไม่ถูกต้อง ต้องเป็นตัวเลข 10 หลักขึ้นต้นด้วย 0 (\"{G(8)}\")";
                    else if (string.IsNullOrEmpty(agt_idcard) && string.IsNullOrEmpty(G(13)))
                        skipReason = "ไม่มีเลขบัตรประชาชน/เลขผู้เสียภาษี";
                    else if (agt_idcard.Length != 13)
                        skipReason = $"เลขบัตรประชาชน/เลขผู้เสียภาษีต้องเป็นตัวเลข 13 หลัก (\"{G(13)}\")";
                    else if (mem_username.Length > 100)
                        skipReason = "Username ยาวเกิน 100 ตัวอักษร";
                    else if (agt_base_phone.Length > 0 && (agt_base_phone.Length < 9 || agt_base_phone.Length > 10))
                        skipReason = $"รูปแบบโทรศัพท์บ้านไม่ถูกต้อง ต้องเป็นตัวเลข 9-10 หลัก (\"{G(30)}\")";
                    else if (agt_contact_phone.Length > 0 && (agt_contact_phone.Length < 9 || agt_contact_phone.Length > 10))
                        skipReason = $"รูปแบบโทรศัพท์ติดต่อไม่ถูกต้อง ต้องเป็นตัวเลข 9-10 หลัก (\"{G(31)}\")";

                    // ---- เลขบัตรประชาชน/เลขผู้เสียภาษีซ้ำ = โหมดอัปเดต (ไม่ใช่ข้ามแถว) ----
                    // เจอเลขบัตรเดิมในฐานข้อมูล → UPDATE ทับข้อมูลทั้งหมดของตัวแทน+สมาชิกรายนั้น
                    // ไม่เจอ → INSERT ตามการทำงานเดิม
                    bool isUpdate       = false;
                    long existAgentId   = 0;
                    long existMemberId  = 0;   // 0 = ยังไม่มีสมาชิกผูกอยู่ (ต้อง insert สมาชิกใหม่)
                    string existCodeId  = "";

                    if (string.IsNullOrEmpty(skipReason))
                    {
                        var chk = _db.ExecuteQuery(
                            "SELECT top 1 id, uid, codeid FROM [2026_web_agent] WHERE idcard = @idcard ORDER BY id",
                            new Dictionary<string, object> { { "idcard", agt_idcard } });
                        if (chk.Rows.Count > 0)
                        {
                            isUpdate     = true;
                            existAgentId = Convert.ToInt64(chk.Rows[0]["id"]);
                            existCodeId  = chk.Rows[0]["codeid"] == DBNull.Value ? "" : chk.Rows[0]["codeid"].ToString()!;
                            long uid     = chk.Rows[0]["uid"] == DBNull.Value ? 0 : Convert.ToInt64(chk.Rows[0]["uid"]);

                            // uid อาจว่าง/ชี้ไปสมาชิกที่ถูกลบไปแล้ว → ถือว่ายังไม่มีสมาชิก แล้วสร้างใหม่ให้
                            if (uid > 0)
                            {
                                var m = _db.ExecuteQuery(
                                    "SELECT top 1 id FROM [2026_web_member] WHERE id = @id",
                                    new Dictionary<string, object> { { "id", uid } });
                                if (m.Rows.Count > 0) existMemberId = uid;
                            }
                        }
                    }

                    // username/email ต้องไม่ชนกับ "สมาชิกรายอื่น" (โหมดอัปเดตยกเว้นสมาชิกของตัวเอง)
                    if (string.IsNullOrEmpty(skipReason))
                    {
                        var chk = _db.ExecuteQuery(
                            "SELECT top 1 id FROM [2026_web_member] WHERE LOWER(username) = LOWER(@username) AND id <> @self",
                            new Dictionary<string, object> { { "username", mem_username }, { "self", existMemberId } });
                        if (chk.Rows.Count > 0) skipReason = "username ซ้ำ";
                    }
                    if (string.IsNullOrEmpty(skipReason))
                    {
                        var chk = _db.ExecuteQuery(
                            "SELECT top 1 id FROM [2026_web_member] WHERE LOWER(email) = LOWER(@email) AND id <> @self",
                            new Dictionary<string, object> { { "email", mem_email }, { "self", existMemberId } });
                        if (chk.Rows.Count > 0) skipReason = "email ซ้ำ";
                    }

                    // ---- จังหวัด/อำเภอ/ตำบล ต้องบันทึกเป็น code id ของ web_data_* ----
                    // ไฟล์ควรส่ง code มาตรง ๆ (template ใหม่) — ถ้าเป็นไฟล์เก่าที่ส่ง "ชื่อ" มา ระบบแปลงให้อัตโนมัติ
                    // จับคู่ไม่ได้ = ข้ามแถวพร้อมบอกเหตุผล (ไม่ปล่อยข้อความหลุดลง DB ให้ข้อมูลปนกันอีก)
                    if (string.IsNullOrEmpty(skipReason))
                    {
                        (agt_base_province, agt_base_amphur, agt_base_tambol, agt_base_zipcode, skipReason) =
                            ResolveAddress(agt_base_province, agt_base_amphur, agt_base_tambol, agt_base_zipcode, "ทะเบียนบ้าน");
                    }
                    if (string.IsNullOrEmpty(skipReason))
                    {
                        (agt_contact_province, agt_contact_amphur, agt_contact_tambol, agt_contact_zipcode, skipReason) =
                            ResolveAddress(agt_contact_province, agt_contact_amphur, agt_contact_tambol, agt_contact_zipcode, "ที่อยู่ติดต่อ");
                    }

                    if (!string.IsNullOrEmpty(skipReason))
                    {
                        failedList.Add(new() {
                            { "row", row.ToString() },
                            { "username", mem_username },
                            { "email", mem_email },
                            { "idcard", agt_idcard },
                            { "name", $"{agt_name} {agt_surname}".Trim() },
                            { "reason", skipReason }
                        });
                        continue;
                    }

                    int.TryParse(agt_title_str, out int agt_title);
                    if (agt_title < 1 || agt_title > 4) agt_title = 1;

                    object T(string s) => string.IsNullOrEmpty(s) ? (object)DBNull.Value : s;

                    // สร้างรหัสผ่านใหม่เฉพาะกรณีที่ต้องเพิ่มสมาชิกใหม่เท่านั้น
                    // โหมดอัปเดตคงรหัสผ่านเดิมไว้ (ไฟล์ Excel ไม่มีคอลัมน์รหัสผ่าน)
                    bool needNewMember   = existMemberId == 0;
                    string plainPassword = needNewMember ? GenerateRandomPassword(10) : "";

                    try
                    {
                        long memberId;
                        long agentId;
                        string codeId;

                        using var conn = new SqlConnection(_db.DefaultConnectionString);
                        conn.Open();
                        using var tx = conn.BeginTransaction();

                        if (needNewMember)
                        {
                            // ---- INSERT web_member ----
                            const string memberSql = @"
INSERT INTO [2026_web_member]
  (type, title, titleoth, firstname, surname, username, email, mobile, lineuid,
   password, reg_date, last_login, accept_mail, confirm_otp, confirm_email,
   refemail, lineid, web_id, sort, pb_status, show_front, status,
   created_at, updated_at, created_by, updated_by)
OUTPUT INSERTED.id
VALUES
  (@type, @title, @titleoth, @firstname, @surname, @username, @email, @mobile, @lineuid,
   @password, @now, @now, 1, 0, 1,
   NULL, NULL, 0, 1, 1, 1, 1,
   @now, @now, 'import', 'import')";

                            using var cmd = new SqlCommand(memberSql, conn);
                            cmd.Transaction = tx;
                            cmd.Parameters.AddWithValue("type",      mem_type);
                            cmd.Parameters.AddWithValue("title",     T(mem_title));
                            cmd.Parameters.AddWithValue("titleoth",  T(mem_titleoth));
                            cmd.Parameters.AddWithValue("firstname", T(mem_firstname));
                            cmd.Parameters.AddWithValue("surname",   T(mem_surname));
                            cmd.Parameters.AddWithValue("username",  mem_username);
                            cmd.Parameters.AddWithValue("email",     mem_email);
                            cmd.Parameters.AddWithValue("mobile",    T(mem_mobile));
                            cmd.Parameters.AddWithValue("lineuid",   T(mem_lineuid));
                            cmd.Parameters.AddWithValue("password",  BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12));
                            cmd.Parameters.AddWithValue("now",       now);
                            memberId = Convert.ToInt64(cmd.ExecuteScalar());
                        }
                        else
                        {
                            // ---- UPDATE web_member (ทับทุกฟิลด์ที่มาจากไฟล์ ยกเว้นรหัสผ่าน/วันสมัคร) ----
                            memberId = existMemberId;
                            const string memberUpdSql = @"
UPDATE [2026_web_member] SET
  type = @type, title = @title, titleoth = @titleoth,
  firstname = @firstname, surname = @surname,
  username = @username, email = @email, mobile = @mobile, lineuid = @lineuid,
  updated_at = @now, updated_by = 'import'
WHERE id = @id";

                            using var cmd = new SqlCommand(memberUpdSql, conn);
                            cmd.Transaction = tx;
                            cmd.Parameters.AddWithValue("type",      mem_type);
                            cmd.Parameters.AddWithValue("title",     T(mem_title));
                            cmd.Parameters.AddWithValue("titleoth",  T(mem_titleoth));
                            cmd.Parameters.AddWithValue("firstname", T(mem_firstname));
                            cmd.Parameters.AddWithValue("surname",   T(mem_surname));
                            cmd.Parameters.AddWithValue("username",  mem_username);
                            cmd.Parameters.AddWithValue("email",     mem_email);
                            cmd.Parameters.AddWithValue("mobile",    T(mem_mobile));
                            cmd.Parameters.AddWithValue("lineuid",   T(mem_lineuid));
                            cmd.Parameters.AddWithValue("now",       now);
                            cmd.Parameters.AddWithValue("id",        memberId);
                            cmd.ExecuteNonQuery();
                        }

                        // ---- codeid: โหมดอัปเดตใช้รหัสเดิม (สร้างใหม่เฉพาะตอนที่ยังไม่มี) ----
                        if (isUpdate && !string.IsNullOrEmpty(existCodeId))
                        {
                            codeId = existCodeId;
                        }
                        else
                        {
                            string datePrefix = now.ToString("yyyyMMdd");
                            using var seqCmd = new SqlCommand("SELECT COUNT(*) + 1 FROM [2026_web_agent] WHERE codeid LIKE @prefix", conn);
                            seqCmd.Transaction = tx;
                            seqCmd.Parameters.AddWithValue("prefix", $"{datePrefix}%");
                            long nextSeq = Convert.ToInt64(seqCmd.ExecuteScalar() ?? 1L);
                            codeId = $"{datePrefix}{nextSeq:D4}";
                        }

                        // ช่องรหัสทรัพย์ npaid1..npaidN สร้างจาก AdminMenu.NpaFields (จำนวนตาม NpaSlotCount)
                        // ผูกค่าฝั่งตัวแทนชุดเดียวกันทั้ง INSERT และ UPDATE
                        void BindAgent(SqlCommand cmd)
                        {
                            cmd.Parameters.AddWithValue("codeid",  codeId);
                            cmd.Parameters.AddWithValue("uid",     memberId);
                            cmd.Parameters.AddWithValue("title",   agt_title);
                            cmd.Parameters.AddWithValue("name",    T(agt_name));
                            cmd.Parameters.AddWithValue("surname", T(agt_surname));
                            cmd.Parameters.AddWithValue("idcard",  T(agt_idcard));
                            cmd.Parameters.AddWithValue("cus_base_addr",     T(agt_base_addr));
                            cmd.Parameters.AddWithValue("cus_base_moo",      T(agt_base_moo));
                            cmd.Parameters.AddWithValue("cus_base_soi",      T(agt_base_soi));
                            cmd.Parameters.AddWithValue("cus_base_road",     T(agt_base_road));
                            cmd.Parameters.AddWithValue("cus_base_tambol",   T(agt_base_tambol));
                            cmd.Parameters.AddWithValue("cus_base_amphur",   T(agt_base_amphur));
                            cmd.Parameters.AddWithValue("cus_base_province", T(agt_base_province));
                            cmd.Parameters.AddWithValue("cus_base_zipcode",  T(agt_base_zipcode));
                            cmd.Parameters.AddWithValue("cus_contact_arrr",    T(agt_contact_arrr));
                            cmd.Parameters.AddWithValue("cus_contact_moo",     T(agt_contact_moo));
                            cmd.Parameters.AddWithValue("cus_contact_soi",     T(agt_contact_soi));
                            cmd.Parameters.AddWithValue("cus_contact_road",    T(agt_contact_road));
                            cmd.Parameters.AddWithValue("cus_contact_tambol",  T(agt_contact_tambol));
                            cmd.Parameters.AddWithValue("cus_contact_amphur",  T(agt_contact_amphur));
                            cmd.Parameters.AddWithValue("cus_contact_province",T(agt_contact_province));
                            cmd.Parameters.AddWithValue("cus_contact_zipcode", T(agt_contact_zipcode));
                            cmd.Parameters.AddWithValue("cus_base_phone",    T(agt_base_phone));
                            cmd.Parameters.AddWithValue("cus_contact_phone", T(agt_contact_phone));
                            for (int k = 0; k < NpaSlotCount; k++)
                                cmd.Parameters.AddWithValue(Helpers.AdminMenu.NpaFields[k], T(npaids[k]));
                            cmd.Parameters.AddWithValue("now",     now);
                        }

                        if (isUpdate)
                        {
                            // ---- UPDATE web_agent (ทับข้อมูลทั้งหมด รวมช่องรหัสทรัพย์) ----
                            // ไม่แตะ status/approved/adddate/upfile* เพื่อไม่ให้สถานะที่อนุมัติไว้แล้วเปลี่ยน
                            string npaSet = string.Join(",\n  ", Helpers.AdminMenu.NpaFields.Select(f => $"{f} = @{f}"));
                            string agentUpdSql = $@"
UPDATE [2026_web_agent] SET
  codeid = @codeid, uid = @uid, title = @title, name = @name, surname = @surname, idcard = @idcard,
  cus_base_addr = @cus_base_addr, cus_base_moo = @cus_base_moo,
  cus_base_soi = @cus_base_soi, cus_base_road = @cus_base_road,
  cus_base_tambol = @cus_base_tambol, cus_base_amphur = @cus_base_amphur,
  cus_base_province = @cus_base_province, cus_base_zipcode = @cus_base_zipcode,
  cus_contact_arrr = @cus_contact_arrr, cus_contact_moo = @cus_contact_moo,
  cus_contact_soi = @cus_contact_soi, cus_contact_road = @cus_contact_road,
  cus_contact_tambol = @cus_contact_tambol, cus_contact_amphur = @cus_contact_amphur,
  cus_contact_province = @cus_contact_province, cus_contact_zipcode = @cus_contact_zipcode,
  cus_base_phone = @cus_base_phone, cus_contact_phone = @cus_contact_phone,
  {npaSet},
  updated_at = @now, updated_by = 'import'
WHERE id = @id";

                            using var cmd = new SqlCommand(agentUpdSql, conn);
                            cmd.Transaction = tx;
                            BindAgent(cmd);
                            cmd.Parameters.AddWithValue("id", existAgentId);
                            cmd.ExecuteNonQuery();
                            agentId = existAgentId;
                        }
                        else
                        {
                            // ---- INSERT web_agent ----
                            string npaCols = string.Join(", ", Helpers.AdminMenu.NpaFields);
                            string npaVals = string.Join(", ", Helpers.AdminMenu.NpaFields.Select(f => "@" + f));
                            string agentSql = $@"
INSERT INTO [2026_web_agent]
  (codeid, uid, title, name, surname, idcard,
   cus_base_addr, cus_base_moo, cus_base_soi, cus_base_road,
   cus_base_tambol, cus_base_amphur, cus_base_province, cus_base_zipcode,
   cus_contact_arrr, cus_contact_moo, cus_contact_soi, cus_contact_road,
   cus_contact_tambol, cus_contact_amphur, cus_contact_province, cus_contact_zipcode,
   cus_base_phone, cus_contact_phone,
   {npaCols},
   upfile1, upfile2, upfile3, upfile4,
   adddate, status, approved, codefile, web_id, sort, pb_status, show_front,
   created_at, updated_at, created_by, updated_by)
OUTPUT INSERTED.id
VALUES
  (@codeid, @uid, @title, @name, @surname, @idcard,
   @cus_base_addr, @cus_base_moo, @cus_base_soi, @cus_base_road,
   @cus_base_tambol, @cus_base_amphur, @cus_base_province, @cus_base_zipcode,
   @cus_contact_arrr, @cus_contact_moo, @cus_contact_soi, @cus_contact_road,
   @cus_contact_tambol, @cus_contact_amphur, @cus_contact_province, @cus_contact_zipcode,
   @cus_base_phone, @cus_contact_phone,
   {npaVals},
   NULL, NULL, NULL, NULL,
   @now, 1, 1, NULL, 0, 1, 1, 1,
   @now, @now, 'import', 'import')";

                            using var cmd = new SqlCommand(agentSql, conn);
                            cmd.Transaction = tx;
                            BindAgent(cmd);
                            agentId = Convert.ToInt64(cmd.ExecuteScalar());
                        }

                        tx.Commit();

                        successList.Add(new() {
                            { "row",       row.ToString() },
                            { "action",    isUpdate ? "update" : "insert" },
                            { "member_id", memberId.ToString() },
                            { "agent_id",  agentId.ToString() },
                            { "codeid",    codeId },
                            { "username",  mem_username },
                            { "email",     mem_email },
                            { "name",      $"{agt_name} {agt_surname}".Trim() },
                            { "idcard",    agt_idcard },
                            { "password",  plainPassword }
                        });
                    }
                    catch (Exception ex)
                    {
                        failedList.Add(new() {
                            { "row",      row.ToString() },
                            { "username", mem_username },
                            { "email",    mem_email },
                            { "idcard",   agt_idcard },
                            { "name",     $"{agt_name} {agt_surname}".Trim() },
                            { "reason",   "DB Error: " + ex.Message }
                        });
                    }
                }

                int insertCount = successList.Count(x => x.GetValueOrDefault("action") == "insert");
                int updateCount = successList.Count(x => x.GetValueOrDefault("action") == "update");

                TempData["import_success"] = JsonConvert.SerializeObject(successList);
                TempData["import_failed"]  = JsonConvert.SerializeObject(failedList);
                TempData["alert_message"]  = $"นำเข้าข้อมูลเสร็จสิ้น: <strong>เพิ่มใหม่ {insertCount} รายการ</strong>, <strong>อัปเดต {updateCount} รายการ</strong>, ข้ามรายการ {failedList.Count} รายการ";
                TempData["alert_class"]    = successList.Count > 0 ? "alert-success" : "alert-warning";
            }
            catch (Exception e)
            {
                TempData["alert_message"] = $"เกิดข้อผิดพลาด: {e.Message}";
                TempData["alert_class"]   = "alert-danger";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// ตรวจจังหวัด/อำเภอ/ตำบล ของไฟล์ Excel
        ///
        /// ระบบ Agent เก็บที่อยู่เป็น <b>รหัส (code) ของ web_data_*</b> ทุกช่องทาง — ทั้งฟอร์มสมัคร
        /// /th/apply-agent/step2 (dropdown ส่ง code) และ Service API ฝั่ง front-end (AuthController)
        /// ไฟล์ Import จึงต้องกรอกเป็น <b>รหัสตัวเลข</b> เท่านั้น กรอกเป็นชื่อจังหวัด/อำเภอจะถูกปฏิเสธทั้งแถว
        /// (ถ้าปล่อยให้ส่งชื่อมาได้ ข้อมูลจะปนกัน 2 รูปแบบเหมือนข้อมูลชุดเก่า)
        ///
        /// เงื่อนไข: จังหวัด 2 หลัก / อำเภอ 4 หลัก / ตำบล 6 หลัก, ต้องมีจริงในตารางอ้างอิง,
        /// และต้องเป็นลูกของกันตามลำดับชั้น (อำเภออยู่ในจังหวัดนั้น, ตำบลอยู่ในอำเภอนั้น)
        /// กรอกที่อยู่มาแล้วต้องครบทั้ง 3 ระดับ — รหัสไปรษณีย์เว้นว่างได้ ระบบเติมจากตำบลให้
        /// </summary>
        private (string province, string amphur, string tambol, string zipcode, string reason)
            ResolveAddress(string province, string amphur, string tambol, string zipcode, string label)
        {
            // ไม่กรอกที่อยู่มาเลย = ไม่บังคับ (คอลัมน์ที่อยู่ไม่ใช่ฟิลด์บังคับของไฟล์นำเข้า)
            if (string.IsNullOrEmpty(province) && string.IsNullOrEmpty(amphur) && string.IsNullOrEmpty(tambol))
                return ("", "", "", zipcode, "");

            // กรอกมาบางส่วน = ข้อมูลที่อยู่ไม่สมบูรณ์ (หน้าเว็บบังคับเลือกครบทั้ง 3 ระดับ)
            if (string.IsNullOrEmpty(province) || string.IsNullOrEmpty(amphur) || string.IsNullOrEmpty(tambol))
                return (province, amphur, tambol, zipcode,
                    $"ที่อยู่ ({label}) ต้องกรอกรหัสจังหวัด/อำเภอ/ตำบล ให้ครบทั้ง 3 ช่อง");

            // ── รูปแบบต้องเป็นรหัสตัวเลขตามจำนวนหลักของ master data ──
            (string val, string name, int len)[] codes =
            {
                (province, "จังหวัด",     2),
                (amphur,   "อำเภอ/เขต",  4),
                (tambol,   "ตำบล/แขวง",  6),
            };
            foreach (var (val, name, len) in codes)
            {
                if (Digits(val) != val || val.Length != len)
                    return (province, amphur, tambol, zipcode,
                        $"รหัส{name} ({label}) ต้องเป็นตัวเลข {len} หลัก (\"{val}\") — ไฟล์นำเข้ารับเฉพาะรหัส Master Data ไม่รับชื่อ");
            }

            if (!string.IsNullOrEmpty(zipcode) && (Digits(zipcode) != zipcode || zipcode.Length != 5))
                return (province, amphur, tambol, zipcode,
                    $"รหัสไปรษณีย์ ({label}) ต้องเป็นตัวเลข 5 หลัก (\"{zipcode}\")");

            // ── มีอยู่จริง + เป็นลูกของกันตามลำดับชั้น (กันตำบลเชียงใหม่ไปอยู่ใต้จังหวัดกรุงเทพฯ) ──
            var chk = _db.ExecuteQuery(
                $@"select top 1 s.zip_code
                   from {Db.T(Helpers.GeoHelper.SubdistrictTable)} s
                   join {Db.T(Helpers.GeoHelper.DistrictTable)}    d on d.id = s.district_id
                   join {Db.T(Helpers.GeoHelper.ProvinceTable)}    p on p.id = d.province_id
                   where s.code = @t and d.code = @a and p.code = @p",
                new Dictionary<string, object>
                {
                    { "t", int.Parse(tambol) }, { "a", int.Parse(amphur) }, { "p", int.Parse(province) }
                });
            if (chk.Rows.Count == 0)
                return (province, amphur, tambol, zipcode,
                    $"รหัสที่อยู่ ({label}) ไม่ตรงกับ Master Data — ตำบล {tambol} ต้องอยู่ในอำเภอ {amphur} และจังหวัด {province}");

            if (string.IsNullOrEmpty(zipcode))
                zipcode = chk.Rows[0]["zip_code"]?.ToString() ?? "";

            return (province, amphur, tambol, zipcode, "");
        }

        private static string GenerateRandomPassword(int length)
        {
            const string upper   = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower   = "abcdefghijklmnopqrstuvwxyz";
            const string digits  = "0123456789";
            const string special = "!@#$%";
            string all = upper + lower + digits + special;

            var bytes = new byte[length];
            System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);

            var chars = new char[length];
            chars[0] = upper[bytes[0] % upper.Length];
            chars[1] = lower[bytes[1] % lower.Length];
            chars[2] = digits[bytes[2] % digits.Length];
            chars[3] = special[bytes[3] % special.Length];
            for (int i = 4; i < length; i++)
                chars[i] = all[bytes[i] % all.Length];

            return new string(chars.OrderBy(_ => Guid.NewGuid()).ToArray());
        }
    }
}
