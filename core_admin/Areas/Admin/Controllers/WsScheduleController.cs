using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    /// <summary>
    /// จุดที่ตัวจับเวลา (scheduler) เรียกเพื่อดึงข้อมูลจาก web service ของ Asset Plus เข้าฐานข้อมูลอัตโนมัติ
    /// พอร์ตมาจาก <c>backoffice/ws_schedule/*.aspx</c> ของระบบเดิม
    ///
    /// URL คงรูปเดิมไว้ทั้งชุด เพื่อให้ย้ายมาที่ระบบใหม่ได้โดยแก้แค่ชื่อโฮสต์ใน scheduler :
    ///
    /// | ระบบเดิม                                        | ระบบใหม่ (รับทั้งแบบมีและไม่มี .aspx)          |
    /// |------------------------------------------------|-----------------------------------------------|
    /// | /assetplus/backoffice/ws_schedule/ws_get_nav.aspx            | /ws_schedule/ws_get_nav[.aspx]            |
    /// | /assetplus/backoffice/ws_schedule/ws_get_other_indices.aspx  | /ws_schedule/ws_get_other_indices[.aspx]  |
    /// | /assetplus/backoffice/ws_schedule/ws_get_performance.aspx    | /ws_schedule/ws_get_performance[.aspx]    |
    /// | /assetplus/backoffice/ws_schedule/ws_get_fundfact.aspx       | /ws_schedule/ws_get_fundfact[.aspx]       |
    ///
    /// **พารามิเตอร์ที่ส่งให้ web service เหมือนระบบเดิมทุกตัว** :
    ///   NAVAnnounce()                 — ไม่มีพารามิเตอร์
    ///   MartketOtherIndices(date)     — วันที่วันนี้ dd/MM/yyyy
    ///   FundReturnPerformance(date)   — วันที่วันนี้ dd/MM/yyyy
    ///   FundFactSheet(fundDate)       — วันที่วันนี้ dd/MM/yyyy
    ///
    /// สิ่งที่ระบบใหม่เพิ่มให้ (ไม่กระทบการทำงานเดิม) :
    ///   • <c>?date=dd/MM/yyyy</c> — ระบุวันที่เองแทนวันนี้ (ระบบเดิมใช้วันนี้เสมอ) ใช้ตอนดึงข้อมูลย้อนหลัง
    ///   • <c>?key=…</c>          — กุญแจกันคนนอกยิง (ตั้งที่ appsettings → WsSchedule:Key ; ไม่ตั้ง = เปิดเหมือนเดิม)
    ///   • <c>?format=json</c>    — ตอบเป็น JSON ให้ scheduler อ่านผลง่าย (ค่าปกติเป็นข้อความเหมือนเดิม)
    ///   • เก็บ XML ที่ได้ลงแฟ้ม <c>xml_file/&lt;ชื่อ&gt;.xml</c> เหมือนเดิม (ตั้งที่ WsSchedule:XmlPath)
    /// </summary>
    [Route("ws_schedule")]
    public class WsScheduleController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly DBHelper _db;
        private readonly Utility _utility;
        private readonly AssetPlusImporter _importer;

        public WsScheduleController(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
            _db = new DBHelper(env, config);
            _utility = new Utility(env, config);
            _importer = new AssetPlusImporter(_db);
        }

        // =============================================================================
        //  endpoints  (ชื่อ action = ชื่อไฟล์เดิม)
        // =============================================================================

        [HttpGet("ws_get_nav")]
        [HttpGet("ws_get_nav.aspx")]
        [HttpPost("ws_get_nav")]
        [HttpPost("ws_get_nav.aspx")]
        public IActionResult ws_get_nav() => Run("NAVAnnounce", null, "NAVAnnounce.xml", "ArrayOfNAV",
            (XmlElement data, string date, string user, out string detail) => _importer.ImportNav(data, user, out detail));

        [HttpGet("ws_get_other_indices")]
        [HttpGet("ws_get_other_indices.aspx")]
        [HttpPost("ws_get_other_indices")]
        [HttpPost("ws_get_other_indices.aspx")]
        public IActionResult ws_get_other_indices() => Run("MartketOtherIndices", "date", "MartketOtherIndices.xml", "OtherIndices",
            (XmlElement data, string date, string user, out string detail) => _importer.ImportOtherIndices(data, date, user, out detail));

        [HttpGet("ws_get_performance")]
        [HttpGet("ws_get_performance.aspx")]
        [HttpPost("ws_get_performance")]
        [HttpPost("ws_get_performance.aspx")]
        public IActionResult ws_get_performance() => Run("FundReturnPerformance", "date", "FundReturnPerformance.xml", "ReturnPerformance",
            (XmlElement data, string date, string user, out string detail) => _importer.ImportPerformance(data, date, user, out detail));

        [HttpGet("ws_get_fundfact")]
        [HttpGet("ws_get_fundfact.aspx")]
        [HttpPost("ws_get_fundfact")]
        [HttpPost("ws_get_fundfact.aspx")]
        public IActionResult ws_get_fundfact() => Run("FundFactSheet", "fundDate", "FundFactSheet.xml", "ArrayOfFundFact",
            (XmlElement data, string date, string user, out string detail) => _importer.ImportFundFact(data, user, out detail));

        /// <summary>หน้าตรวจสถานะ (แทน ws_schedule/test.aspx) — บอกว่าตั้งค่า endpoint ไว้อย่างไรและเรียก URL ไหนได้บ้าง</summary>
        [HttpGet("")]
        [HttpGet("test")]
        [HttpGet("test.aspx")]
        public IActionResult test()
        {
            var ws = new AssetPlusWsClient(_config);
            var sb = new StringBuilder();
            sb.AppendLine("ws_schedule (Asset Plus) — พร้อมใช้งาน");
            sb.AppendLine("web service : " + (ws.IsConfigured ? ws.Url : "(ยังไม่ได้ตั้งค่า AssetPlusWS:URL)"));
            sb.AppendLine("โฟลเดอร์เก็บ XML : " + XmlDir());
            sb.AppendLine("กุญแจ (WsSchedule:Key) : " + (RequiredKey() == "" ? "ไม่ได้ตั้ง — เรียกได้เลย" : "ตั้งไว้แล้ว ต้องส่ง ?key=…"));
            sb.AppendLine();
            sb.AppendLine("เรียกได้ที่ (ต่อท้าย .aspx ก็ได้) :");
            foreach (string a in new[] { "ws_get_nav", "ws_get_other_indices", "ws_get_performance", "ws_get_fundfact" })
                sb.AppendLine("  " + _utility.rootURL() + "/ws_schedule/" + a);
            sb.AppendLine();
            sb.AppendLine("พารามิเตอร์เสริม : ?date=dd/MM/yyyy (ค่าปกติ = วันนี้) · ?format=json · ?key=…");
            return Content(sb.ToString(), "text/plain; charset=utf-8");
        }

        // =============================================================================
        //  แกนกลาง
        // =============================================================================

        private delegate int ImportFunc(XmlElement data, string date, string user, out string detail);

        /// <summary>
        /// ลำดับการทำงานเดียวกับ ws_schedule/*.aspx ของระบบเดิม :
        ///   1. เรียก web service ด้วยพารามิเตอร์เดิม
        ///   2. เขียน XML ที่ได้ลงแฟ้ม (ทับของเดิม) เพื่อให้ตรวจย้อนหลังได้
        ///   3. อ่าน XML นั้นเข้าฐานข้อมูล ด้วยผู้ใช้ชื่อ <c>ws_auto</c>
        /// </summary>
        private IActionResult Run(string operation, string? dateParam, string xmlFileName, string emptyRootName, ImportFunc import)
        {
            var started = DateTime.Now;

            //----- กุญแจ (ถ้าตั้งไว้) -----
            string need = RequiredKey();
            if (need != "" && (Request.Query["key"] + "") != need)
            {
                return Respond(operation, 0, "", "ปฏิเสธการเข้าถึง : key ไม่ถูกต้อง", started, StatusCodes.Status403Forbidden);
            }

            //----- วันที่ : ระบบเดิมใช้ DateTime.Now.ToString("dd/MM/yyyy") เสมอ -----
            //      ⚠ ต้องเป็น ค.ศ. — culture ของแอปเป็น th-TH (ปฏิทินพุทธ) ถ้าใช้ ToString เฉย ๆ จะได้ปี พ.ศ.
            //        แล้ว web service เดิมจะไม่รู้จักวันที่นั้น จึงบังคับ InvariantCulture ตรงนี้
            string date = (Request.Query["date"] + "").Trim();
            if (date == "") date = DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            //----- 1) เรียก web service -----
            var ws = new AssetPlusWsClient(_config);
            XmlElement? data = ws.Call(operation, dateParam, date, out string error);

            //----- 2) เขียนแฟ้ม XML (ระบบเดิมเขียนโครงเปล่าไว้เมื่อ web service ไม่ตอบ) -----
            string xmlPath = "";
            try
            {
                string dir = XmlDir();
                Directory.CreateDirectory(dir);
                xmlPath = Path.Combine(dir, xmlFileName);
                string xmlText = data != null
                    ? data.OuterXml
                    : string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><{0} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></{0}>", emptyRootName);
                System.IO.File.WriteAllText(xmlPath, xmlText, new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                //----- เขียนแฟ้มไม่ได้ต้องไม่ทำให้การนำเข้าล้ม (แฟ้มเป็นแค่ร่องรอยไว้ตรวจ) -----
                _utility.writeLogs("ws_schedule เขียนแฟ้ม XML ไม่ได้ (" + xmlFileName + ") - " + ex.Message);
            }

            if (data == null)
            {
                _utility.writeLogs(string.Format("ws_schedule {0} : เรียก web service ไม่สำเร็จ - {1}", operation, error));
                return Respond(operation, 0, "", "เรียก web service ไม่สำเร็จ : " + error, started, StatusCodes.Status502BadGateway);
            }

            //----- 3) นำเข้าฐานข้อมูล -----
            try
            {
                int n = import(data, date, AssetPlusImporter.ScheduleUser, out string detail);
                WriteLog(operation, n, detail, "");
                return Respond(operation, n, detail, "", started, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _utility.writeLogs(string.Format("ws_schedule {0} : นำเข้าไม่สำเร็จ - {1}", operation, ex.Message));
                WriteLog(operation, 0, "", ex.Message);
                return Respond(operation, 0, "", "นำเข้าข้อมูลไม่สำเร็จ : " + ex.Message, started, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>บันทึกผลลงตาราง log ของหลังบ้านเดิม (tb_admin_log) ให้ผู้ดูแลเห็นว่า scheduler ทำงานเมื่อไร</summary>
        private void WriteLog(string operation, int rows, string detail, string error)
        {
            try
            {
                string text = error == ""
                    ? string.Format("ws_schedule {0} : นำเข้า {1:n0} รายการ {2}", operation, rows, detail)
                    : string.Format("ws_schedule {0} : ล้มเหลว - {1}", operation, error);

                //----- คอลัมน์ตรงกับที่ระบบเดิมเขียน (include/record_manage.aspx) :
                //      user_id, name, lastupdate (unix), lastupdate_date (ข้อความ), action_info, action_url, action_code, action_table, ip
                _db.ExecuteNonQuery(
                    "insert into [tb_admin_log] (user_id, name, lastupdate, lastupdate_date, action_info, action_url, action_code, action_table, ip) " +
                    "values (@u, @u, @t, @d, @info, @url, @act, @tbl, @ip)",
                    new Dictionary<string, object>()
                    {
                        { "u", AssetPlusImporter.ScheduleUser },
                        { "t", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                        { "d", DateTime.Now.ToString("d/M/yyyy HH:mm:ss") },   // th-TH → พ.ศ. เหมือนระบบเดิม
                        { "info", text },
                        { "url", Request.Host.Value + Request.Path.Value },
                        { "act", error == "" ? "add" : "error" },
                        { "tbl", "ws_schedule" },
                        { "ip", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "" },
                    });
            }
            catch (Exception ex)
            {
                //----- log เป็นข้อมูลเสริม ล้มเหลวได้โดยไม่กระทบผลการนำเข้า -----
                _utility.writeLogs("ws_schedule เขียน tb_admin_log ไม่ได้ - " + ex.Message);
            }
        }

        private IActionResult Respond(string operation, int rows, string detail, string error, DateTime started, int status)
        {
            double sec = Math.Round((DateTime.Now - started).TotalSeconds, 2);
            Response.StatusCode = status;

            if ((Request.Query["format"] + "").ToLowerInvariant() == "json")
            {
                return new JsonResult(new
                {
                    ok = error == "",
                    operation,
                    rows,
                    detail,
                    error,
                    seconds = sec,
                    at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                });
            }

            var sb = new StringBuilder();
            sb.AppendLine(error == "" ? "OK" : "ERROR");
            sb.AppendLine("operation : " + operation);
            sb.AppendLine("rows      : " + rows.ToString("n0"));
            if (detail != "") sb.AppendLine("detail    : " + detail);
            if (error != "") sb.AppendLine("error     : " + error);
            sb.AppendLine("seconds   : " + sec);
            return Content(sb.ToString(), "text/plain; charset=utf-8");
        }

        /// <summary>
        /// โฟลเดอร์เก็บ XML ที่ดึงมาได้ (ค่าปกติ = <c>App_Data/ws_schedule</c> ข้าง ๆ โปรเจกต์)
        ///
        /// ⚠ ตั้งใจไม่ให้อยู่ใต้ <c>wwwroot</c> — ระบบเดิมเก็บไว้ใน backoffice/ws_schedule/xml_file
        /// ซึ่งเปิดโหลดจากเว็บได้ ข้อมูล NAV/Fund Fact ทั้งชุดจึงหลุดได้ถ้าเดา URL ถูก
        /// </summary>
        private string XmlDir()
        {
            string p = _config["WsSchedule:XmlPath"] ?? "";
            return string.IsNullOrWhiteSpace(p) ? Path.Combine(_env.ContentRootPath, "App_Data", "ws_schedule") : p;
        }

        private string RequiredKey() => (_config["WsSchedule:Key"] ?? "").Trim();
    }
}
