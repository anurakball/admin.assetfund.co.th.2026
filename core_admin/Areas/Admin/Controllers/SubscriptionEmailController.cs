using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class EmailSendResult
    {
        public string Email { get; set; } = "";
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
    }

    public class SubscriptionEmailController : AdminCoreController
    {
        private static readonly char[] EmailLineSeparators = ['\n', '\r'];
        public SubscriptionEmailController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("SubscriptionEmail");
        }

        [HttpPost]
        public virtual IActionResult Index(IFormCollection collection)
        {
            var emailList = new List<string>();
            string recipientType = collection["recipientType"].ToString().Trim();

            if (recipientType == "all")
            {
                var dt = _db.ExecuteQuery(
                    "SELECT email FROM web_subscription WHERE status = '1'",
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

            #region ----- บันทึกประวัติการส่งข่าวสารทางอีเมล (web_subscription_news) -----
            try
            {
                string adminUser = _session.GetString("admin_user") ?? "";
                _db.ExecuteNonQuery(
                    @"INSERT INTO web_subscription_news
                        (created_at, updated_at, created_by, updated_by, web_id, status, pb_status,
                         recipient_type, subject, body, file1, file2, file3,
                         total_count, success_count, fail_count)
                      VALUES
                        (now(), now(), @created_by, @updated_by, @web_id, 1, 1,
                         @recipient_type, @subject, @body, @file1, @file2, @file3,
                         @total_count, @success_count, @fail_count)",
                    new Dictionary<string, object>()
                    {
                        { "created_by", adminUser },
                        { "updated_by", adminUser },
                        { "web_id", _currentWebID },
                        { "recipient_type", recipientType == "custom" ? "custom" : "all" },
                        { "subject", string.IsNullOrEmpty(subject) ? (object)DBNull.Value : subject },
                        { "body", string.IsNullOrEmpty(body) ? (object)DBNull.Value : body },
                        { "file1", (object?)attachments[0]?.StoredName ?? DBNull.Value },
                        { "file2", (object?)attachments[1]?.StoredName ?? DBNull.Value },
                        { "file3", (object?)attachments[2]?.StoredName ?? DBNull.Value },
                        { "total_count", emailList.Count },
                        { "success_count", successCount },
                        { "fail_count", failCount },
                    });
            }
            catch (Exception exSave)
            {
                _utility.writeLogs("web_subscription_news insert error: " + exSave.Message);
            }
            #endregion

            _admin.ActionLogs(
                admin_user_id: (int)(_session.GetInt32("admin_user_id") ?? 0),
                admin_username: _session.GetString("admin_user"),
                action: "send_email",
                action_info: string.Format("ส่งอีเมล Subscription: ทั้งหมด {0} รายการ, สำเร็จ {1}, ไม่สำเร็จ {2}", emailList.Count, successCount, failCount),
                action_url: Request.Host.Value + Request.Path.Value,
                action_table: "web_subscription",
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
