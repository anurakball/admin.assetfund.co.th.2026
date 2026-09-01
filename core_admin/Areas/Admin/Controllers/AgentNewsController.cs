using Microsoft.AspNetCore.Mvc;
using System.Text;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AgentNewsController : AdminCoreController
    {
        private static readonly char[] EmailLineSeparators = ['\n', '\r'];
        public AgentNewsController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AgentNews");
        }

        /// <summary>
        /// เปิดหน้าส่งอีเมล — ถ้ามี ?uid=... (uid ของ web_agent = id ของ web_member ส่งมาจากหน้า "ไฟล์แนบ" ของผู้สมัครตัวแทน)
        /// จะเตรียมค่าเริ่มต้นให้: ผู้รับ = กำหนดเอง + อีเมลผู้สมัคร, หัวเรื่อง และเนื้อหาอีเมลเป็นตารางแจ้งสถานะเอกสาร
        /// </summary>
        public override IActionResult Index()
        {
            var result = base.Index();
            SetDocumentStatusPrefill(Request.Query["uid"].ToString());
            return result;
        }

        /// <summary>
        /// เตรียมค่าเริ่มต้นของฟอร์มจาก uid (id ของสมาชิกใน web_member)
        /// </summary>
        private void SetDocumentStatusPrefill(string uid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uid) || !int.TryParse(uid.Trim(), out int memberId) || memberId <= 0)
                    return;

                var dt = _db.ExecuteQuery(
                    @"select top 1 a.id, a.uid, a.title, a.name, a.surname,
                             a.upfile1, a.upfile2, a.upfile3, a.upfile4,
                             m.email
                      from [2026_web_agent] a
                      left join [2026_web_member] m on m.id = a.uid
                      where a.uid = @uid and a.web_id = @web_id
                      order by a.id desc",
                    new Dictionary<string, object>() { { "uid", memberId }, { "web_id", _currentWebID } }
                );
                if (dt.Rows.Count == 0) return;

                var r = dt.Rows[0];
                string email = (r["email"]?.ToString() ?? "").Trim();
                string firstname = (r["name"]?.ToString() ?? "").Trim();
                string surname = (r["surname"]?.ToString() ?? "").Trim();
                string prefix = TitlePrefix(r["title"]?.ToString());
                string fullName = (prefix + firstname + " " + surname).Trim();

                var files = new List<string>();
                for (int i = 1; i <= 4; i++)
                {
                    string f = (r["upfile" + i]?.ToString() ?? "").Trim();
                    if (!string.IsNullOrEmpty(f)) files.Add("ไฟล์แนบ " + i);
                }

                ViewBag.PrefillRecipientType = "custom";
                ViewBag.PrefillCustomEmails = email;
                ViewBag.PrefillSubject = string.IsNullOrEmpty(fullName)
                    ? "แจ้งผลการตรวจสอบเอกสารประกอบการสมัครเป็นตัวแทนขายทรัพย์ (Agent)"
                    : string.Format("แจ้งผลการตรวจสอบเอกสารประกอบการสมัครเป็นตัวแทนขายทรัพย์ (Agent) - {0}", fullName);
                // ลิงก์ให้ผู้สมัคร login แล้วเด้งไปหน้าจัดการเอกสาร (apply-agent step3) ทันที
                // โดเมนมาจาก config "FrontURL" (dev = https://localhost:7169, production = https://www.assetfund.co.th)
                string manageDocUrl = _utility.frontURL("/th/login?returnUrl=%2Fth%2Fapply-agent%2Fstep3");

                ViewBag.PrefillBody = BuildDocumentStatusBody(fullName, files, manageDocUrl);
            }
            catch (Exception e)
            {
                _utility.writeLogs("AgentNews prefill error - " + e.Message);
            }
        }

        private static string TitlePrefix(string? title) => (title ?? "").Trim() switch
        {
            "1" => "นาย",
            "2" => "นาง",
            "3" => "นางสาว",
            _ => ""
        };

        /// <summary>
        /// เนื้อหาอีเมลตั้งต้น (HTML สำหรับ CKEditor) — ตารางแจ้งสถานะเอกสารเฉพาะไฟล์แนบที่ผู้สมัครส่งมาจริง
        /// เจ้าหน้าที่กรอก "ผ่าน/ไม่ผ่าน" ในคอลัมน์ขวา และเหตุผลในแถวล่างสุดของตาราง
        /// </summary>
        private static string BuildDocumentStatusBody(string fullName, List<string> fileLabels, string manageDocUrl)
        {
            string safeName = System.Net.WebUtility.HtmlEncode(string.IsNullOrEmpty(fullName) ? "-" : fullName);

            var sb = new StringBuilder();
            sb.Append("<p>เรียน คุณ ").Append(safeName).Append("</p>");
            sb.Append("<p>ตามที่ท่านได้สมัครเป็นตัวแทนขายทรัพย์ (Agent) กับ บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด (Asset Plus) ")
              .Append("และได้จัดส่งเอกสารประกอบการสมัครมายังบริษัทฯ นั้น บริษัทฯ ขอแจ้งผลการตรวจสอบเอกสารของท่าน ดังนี้</p>");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr><th colspan=\"2\">ผลการตรวจสอบเอกสารประกอบการสมัครเป็นตัวแทนขายทรัพย์ (Agent)<br>ผู้สมัคร : ")
              .Append(safeName).Append("</th></tr>");
            sb.Append("<tr><th>รายการเอกสาร</th><th>ผลการตรวจสอบ</th></tr>");
            sb.Append("</thead><tbody>");

            if (fileLabels.Count == 0)
            {
                sb.Append("<tr><td>ไม่พบเอกสารแนบ</td><td>&nbsp;</td></tr>");
            }
            else
            {
                foreach (var label in fileLabels)
                {
                    sb.Append("<tr><td>").Append(label).Append("</td><td>&nbsp;</td></tr>");
                }
            }

            sb.Append("<tr><td colspan=\"2\"><strong>เหตุผล / รายละเอียดเพิ่มเติม (กรณีเอกสารไม่ผ่าน)</strong><br>")
              .Append("&nbsp;<br>&nbsp;<br>&nbsp;</td></tr>");
            sb.Append("</tbody></table>");

            // rel ต้องเป็น "noopener noreferrer" ให้ตรงกับที่ CKEditor สร้างเอง (link decorator ของ external link)
            // ถ้าใส่ rel ค่าอื่น CKEditor จะซ้อน <a> ซ้อน <a> ทำให้ตอน parse เป็นอีเมลลิงก์หลุด (ข้อความไม่คลิกได้)
            string safeUrl = System.Net.WebUtility.HtmlEncode(manageDocUrl ?? "");
            sb.Append("<p><a href=\"").Append(safeUrl).Append("\" target=\"_blank\" rel=\"noopener noreferrer\">")
              .Append("กรุณาดำเนินการแก้ไข/จัดส่งเอกสารตามรายละเอียดข้างต้นอีกครั้ง</a>")
              .Append(" เพื่อให้บริษัทฯ ตรวจสอบและดำเนินการต่อไป</p>");
            sb.Append("<p>จึงเรียนมาเพื่อโปรดทราบ</p>");
            sb.Append("<p>ขอแสดงความนับถือ<br>บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด (Asset Plus)</p>");

            return sb.ToString();
        }

        [HttpPost]
        public virtual IActionResult Index(IFormCollection collection)
        {
            var emailList = new List<string>();
            string recipientType = collection["recipientType"].ToString().Trim();

            if (recipientType == "all")
            {
                var dt = _db.ExecuteQuery(
                    "SELECT wm.email FROM [2026_web_agent] wa INNER JOIN [2026_web_member] wm ON wm.id = wa.uid WHERE wa.status = '1'",
                    new Dictionary<string, object>());
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string email = row["email"]?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(email))
                        emailList.Add(email);
                }
            }
            else if (recipientType == "custom")
            {
                string rawEmails = collection["customEmails"].ToString();
                var parts = rawEmails.Split(EmailLineSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    string email = part.Trim();
                    if (!string.IsNullOrEmpty(email))
                        emailList.Add(email);
                }
            }

            string subject = collection["emailSubject"].ToString().Trim();
            string body = collection["emailBody"].ToString();

            // ไฟล์แนบ (ไม่บังคับ, สูงสุด 3 ไฟล์) — เก็บที่ wwwroot/Files/subscription_news/ ฝั่ง admin
            var attachments = BroadcastMail.SaveAttachments(collection, _hostingEnvironment.WebRootPath);

            // แปลง HTML จาก CKEditor เป็นเนื้อหาอีเมล (inline style + ฝังรูปแบบ cid:)
            var mailBody = BroadcastMail.PrepareBody(body, _hostingEnvironment.WebRootPath, _utility.rootURL());

            var sendResult = BroadcastMail.Send(_config, emailList, subject, mailBody, attachments);
            int successCount = sendResult.SuccessCount;
            int failCount = sendResult.FailCount;
            var sendResults = sendResult.Results;

            _admin.ActionLogs(
                admin_user_id: (int)(_session.GetInt32("admin_user_id") ?? 0),
                admin_username: _session.GetString("admin_user"),
                action: "send_email",
                action_info: string.Format("ส่งอีเมล Agent News: ทั้งหมด {0} รายการ, สำเร็จ {1}, ไม่สำเร็จ {2}", emailList.Count, successCount, failCount),
                action_url: Request.Host.Value + Request.Path.Value,
                action_table: "web_agent",
                mod_name: Module.Name,
                mod_name_txt: Module.Config.Text
            );

            ViewBag.TotalCount = emailList.Count;
            ViewBag.SuccessCount = successCount;
            ViewBag.FailCount = failCount;
            ViewBag.SendResults = sendResults;
            ViewBag.Module = Module;
            ViewBag._utility = _utility;
            ViewBag._admin = _admin;
            ViewBag._session = _session;
            ViewBag._db = _db;
            ViewBag.Title = "ผลการส่งอีเมล";
            ViewBag.ModuleName = Module.Name;

            return View("~/Areas/Admin/Views/SubscriptionEmail/SendResult.cshtml");
        }
    }
}
