using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MemberNewsController : AdminCoreController
    {
        private static readonly char[] EmailLineSeparators = ['\n', '\r'];
        public MemberNewsController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MemberNews");
        }

        [HttpPost]
        public virtual IActionResult Index(IFormCollection collection)
        {
            var emailList = new List<string>();
            string recipientType = collection["recipientType"].ToString().Trim();

            if (recipientType == "all")
            {
                // เลือกเฉพาะประเภทสมาชิกที่ติ๊ก checkbox (member_type) — รับเฉพาะค่า 1-4 ที่ถูกต้อง
                var memberTypeParams = new Dictionary<string, object>();
                var memberTypePlaceholders = new List<string>();
                int mtIndex = 0;
                foreach (var mtRaw in collection["memberTypes"])
                {
                    if (int.TryParse(mtRaw, out int mt) && mt >= 1 && mt <= 4)
                    {
                        string pName = "mt" + mtIndex;
                        memberTypePlaceholders.Add("@" + pName);
                        memberTypeParams.Add(pName, mt);
                        mtIndex++;
                    }
                }

                string sqlMember = "SELECT email FROM web_member WHERE status = '1'";
                if (memberTypePlaceholders.Count > 0)
                {
                    sqlMember += " AND member_type IN (" + string.Join(",", memberTypePlaceholders) + ")";
                }
                else
                {
                    // ไม่ได้เลือกประเภทใดเลย → ไม่ส่งให้ใคร (กันส่งไปทั้งหมดโดยไม่ตั้งใจ)
                    sqlMember += " AND 1 = 0";
                }

                var dt = _db.ExecuteQuery(sqlMember, memberTypeParams);
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
                action_info: string.Format("ส่งข่าวสารสมาชิก: ทั้งหมด {0} รายการ, สำเร็จ {1}, ไม่สำเร็จ {2}", emailList.Count, successCount, failCount),
                action_url: Request.Host.Value + Request.Path.Value,
                action_table: "web_member",
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
