using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Data;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AgentListController : AdminCoreController
    {
        public AgentListController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AgentList");
        }

        /// <summary>
        /// Export Excel ของเมนูนี้ = <b>ไฟล์รูปแบบเดียวกับไฟล์ Import</b> (AgentImport/DownloadTemplate)
        /// ไม่ใช่ตารางรายงานแบบเมนูอื่น — ตั้งใจให้ "Export → แก้ไข → Import กลับ" ได้ทันทีโดยไม่ต้องจัดคอลัมน์ใหม่
        ///
        /// จึงไม่ใช้ Module.Config.ExportData ของ AdminCoreController แต่ใช้ AgentImportSchema
        /// (หัวตาราง/ลำดับ/สี/รูปแบบข้อความ ตัวเดียวกับ template) — 1 แถว = ตัวแทน 1 ราย พร้อมช่องของสมาชิก
        /// ที่ผูกกันด้วย web_agent.uid = web_member.id
        ///
        /// เงื่อนไขการกรอง (ค้นหา/ช่วงวันที่/ติ๊กเลือกเฉพาะบางรายการ) ใช้ตัวเดียวกับ AdminCoreController.Export
        /// คือ SQL ที่เข้ารหัสมาจากหน้า list (ViewBag.sqlQuery) + export_id
        /// </summary>
        [HttpPost]
        [ModuleCheck("export")]
        public override IActionResult Export(IFormCollection f)
        {
            if (!f.ContainsKey("export") || string.IsNullOrEmpty(f["export"]))
            {
                TempData["alert_message"] = "ไม่สามารถ export ข้อมูลได้ (query not set)";
                TempData["alert_class"] = "alert-danger";
                return Redirect("/Admin/" + Module.Name);
            }

            try
            {
                string dataDecrypt = _utility.Decrypt(f["export"].ToString(), _utility.appKey());
                if (string.IsNullOrEmpty(dataDecrypt))
                {
                    TempData["alert_message"] = "ไม่สามารถ export ข้อมูลได้ (decrypt)";
                    TempData["alert_class"] = "alert-danger";
                    return Redirect("/Admin/" + Module.Name);
                }

                var sqlObject = JObject.Parse(dataDecrypt);
                string sql = sqlObject["sql"]!.ToString();
                var sqlParameter = JsonConvert.DeserializeObject<Dictionary<string, object>>(sqlObject["parameter"]!.ToString())
                                   ?? new Dictionary<string, object>();

                // ติ๊กเลือกเฉพาะบางรายการในหน้า list → จำกัดด้วย id IN (...)
                // รับเฉพาะตัวเลขล้วนเท่านั้น (ค่ามาจาก client) — มีตัวไหนไม่ใช่ตัวเลข = ไม่ใส่เงื่อนไขนี้เลย
                if (f.ContainsKey("export_id") && !string.IsNullOrEmpty(f["export_id"]))
                {
                    var ids = f["export_id"].ToString()
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList();
                    if (ids.Count > 0 && ids.All(x => int.TryParse(x, out _)))
                    {
                        var parts = sql.Split("where", 2, StringSplitOptions.None);
                        if (parts.Length > 1)
                            sql = $" {parts[0]} where id IN ({string.Join(",", ids)}) and {parts[1]} ";
                    }
                }

                sqlParameter.TryAdd("web_id", _currentWebID);

                // alias เป็น c0..cN (ASCII สั้น) — ไม่ใช้หัวตารางภาษาไทยเป็น alias เพราะ PostgreSQL
                // ตัดชื่อคอลัมน์ที่ยาวเกิน 63 ไบต์ (ไทย = 3 ไบต์/ตัว) แล้วอาจได้ชื่อซ้ำกัน
                var schema = Helpers.AgentImportSchema.Columns;
                string selectList = string.Join(", ", schema.Select((c, i) => $"{c.Sql} as c{i}"));
                var commandSql = sql.Replace("select *", "select " + selectList);

                var dt = _db.ExecuteQuery(commandSql, sqlParameter);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var excel = new ExcelPackage();
                // ชื่อชีตเดียวกับชีตแรกของ template — ระบบ Import อ่าน Worksheets[0] เสมอ
                var ws = excel.Workbook.Worksheets.Add("Import Agent");

                Helpers.AgentImportSchema.WriteHeader(ws);
                Helpers.AgentImportSchema.SetTextFormat(ws);

                // ข้อมูลเก่าบางแถวเก็บที่อยู่เป็น "ชื่อ" ไม่ใช่รหัส แต่ไฟล์ Import รับเฉพาะรหัส
                // → แปลงให้เป็นรหัสตอนเขียนไฟล์ ไฟล์ที่ได้จึงนำกลับไป Import ได้โดยไม่ต้องแก้มือ
                // (แปลงไม่ได้ = คงค่าเดิมไว้ ไม่ทำข้อมูลหาย แล้วให้ฝั่ง Import รายงานแถวนั้นแทน)
                var geo = new Helpers.GeoHelper.CodeResolver(_db);
                int iBaseT = Helpers.AgentImportSchema.IndexOf("cus_base_tambol");
                int iBaseA = Helpers.AgentImportSchema.IndexOf("cus_base_amphur");
                int iBaseP = Helpers.AgentImportSchema.IndexOf("cus_base_province");
                int iConT  = Helpers.AgentImportSchema.IndexOf("cus_contact_tambol");
                int iConA  = Helpers.AgentImportSchema.IndexOf("cus_contact_amphur");
                int iConP  = Helpers.AgentImportSchema.IndexOf("cus_contact_province");

                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    var vals = new string[schema.Count];
                    for (int c = 0; c < schema.Count && c < dt.Columns.Count; c++)
                    {
                        var v = dt.Rows[r][c];
                        vals[c] = v == DBNull.Value ? "" : (v.ToString() ?? "");
                    }

                    vals[iBaseP] = geo.Province(vals[iBaseP]);
                    vals[iBaseA] = geo.District(vals[iBaseA], vals[iBaseP]);
                    vals[iBaseT] = geo.Subdistrict(vals[iBaseT], vals[iBaseA]);
                    vals[iConP]  = geo.Province(vals[iConP]);
                    vals[iConA]  = geo.District(vals[iConA], vals[iConP]);
                    vals[iConT]  = geo.Subdistrict(vals[iConT], vals[iConA]);

                    for (int c = 0; c < schema.Count; c++)
                    {
                        // เขียนเป็น string เสมอ — ค่าที่ขึ้นต้นด้วย 0 (เบอร์โทร/รหัสไปรษณีย์/เลขบัตร)
                        // ต้องไม่ถูก Excel ตีความเป็นตัวเลขแล้วศูนย์หน้าหายตอนเปิดไฟล์
                        ws.Cells[r + 2, c + 1].Value = vals[c] ?? "";
                    }
                }

                // AutoFit ทั้งชีตเมื่อข้อมูลเยอะจะช้ามาก — วัดความกว้างจาก 200 แถวแรกก็พอ
                int fitRows = Math.Min(dt.Rows.Count + 1, 200);
                ws.Cells[1, 1, fitRows, schema.Count].AutoFitColumns();

                var fileStream = new MemoryStream();
                excel.SaveAs(fileStream);

                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "export",
                    action_info: string.Format("export ข้อมูล : {0} ({1} แถว)", Module.Config.TextBreadcrumb, dt.Rows.Count),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: Module.Config.Table,
                    old_value: _db.DataTableToJSONWithJSONNet(dt),
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text
                );

                string fileName = Module.Name + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_.xlsx";
                return File(fileStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _utility.writeLogs("AgentList Export error - " + ex.Message);
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect("/Admin/" + Module.Name);
            }
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var itemEdit = _db.ExecuteQuery(
                    string.Format("select * from {0} where id = @id and web_id = @web_id limit 1", Module.Config.Table),
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } }
                );
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                var updateFields = _admin.setFieldsUpdate(Module, collection, defaultFields: false);

                // title and approved in web_agent are integer columns, not text
                foreach (var intField in new[] { "title", "approved" })
                {
                    if (updateFields.ContainsKey(intField) && updateFields[intField] != DBNull.Value
                        && int.TryParse(updateFields[intField].ToString(), out int parsed))
                    {
                        updateFields[intField] = parsed;
                    }
                }

                // สถานะที่ส่งมาต้องเป็นรหัสที่ระบบรองรับเท่านั้น (0/1/3/4) — ค่าแปลกปลอมถือเป็น "กำลังตรวจสอบ"
                if (updateFields.ContainsKey("approved"))
                    updateFields["approved"] = Helpers.AgentStatus.Normalize(updateFields["approved"]);

                // จังหวัด/อำเภอ/ตำบล ต้องลง DB เป็น "code id" ของ web_data_* เสมอ
                // ปกติ dropdown ส่ง code มาอยู่แล้ว — ตรงนี้กันกรณีฟอร์มถูกแก้เอง/ค่าเก่าที่เป็นชื่อ
                // ค่าที่จับคู่กับตารางไม่ได้ = เก็บ NULL แทนที่จะปล่อยข้อความหลุดลง DB
                foreach (var grp in new[] { "base", "contact" })
                {
                    string pKey = $"cus_{grp}_province", aKey = $"cus_{grp}_amphur", tKey = $"cus_{grp}_tambol";
                    if (!updateFields.ContainsKey(pKey)) continue;

                    string V(string k) => updateFields.ContainsKey(k) && updateFields[k] != DBNull.Value
                        ? (updateFields[k]?.ToString() ?? "") : "";
                    object O(string code) => string.IsNullOrEmpty(code) ? DBNull.Value : code;

                    string pCode = Helpers.GeoHelper.ToProvinceCode(_db, V(pKey));
                    string aCode = Helpers.GeoHelper.ToDistrictCode(_db, V(aKey), pCode);
                    string tCode = Helpers.GeoHelper.ToSubdistrictCode(_db, V(tKey), aCode);

                    updateFields[pKey] = O(pCode);
                    if (updateFields.ContainsKey(aKey)) updateFields[aKey] = O(aCode);
                    if (updateFields.ContainsKey(tKey)) updateFields[tKey] = O(tCode);
                }

                var affected = _db.Update(
                    Module.Config.Table,
                    "WHERE id = @id and web_id = @web_id",
                    updateFields,
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } }
                );

                if (affected != 0)
                {
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "edit",
                        action_info: string.Format("แก้ไขข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: _db.DataTableToJSONWithJSONNet(itemEdit).TrimStart('[').TrimEnd(']'),
                        new_value: JsonConvert.SerializeObject(updateFields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );

                    // ผลข้างเคียงของสถานะ (อีเมล + member_type) — ใช้ตัวเดียวกับ ajax ในหน้า Detail
                    int oldStatus = Helpers.AgentStatus.Normalize(itemEdit.Rows[0]["approved"]);
                    int newStatus = updateFields.ContainsKey("approved")
                        ? Helpers.AgentStatus.Normalize(updateFields["approved"])
                        : oldStatus;
                    string statusNote = ApplyStatusSideEffects(id, itemEdit.Rows[0]["uid"], oldStatus, newStatus);

                    TempData["alert_message"] = string.Format("แก้ไขข้อมูลแล้ว {0:n0} รายการ", affected)
                        + (string.IsNullOrEmpty(statusNote) ? "" : " — " + statusNote);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถแก้ไขข้อมูลได้";
                    return RedirectToActionPermanent("Edit", new { id = id });
                }
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
        }

        /// <summary>
        /// อัปเดตสถานะ (approved) แบบ ajax จากหน้า Detail
        /// ผลข้างเคียง (อีเมล/member_type) ใช้ ApplyStatusSideEffects ตัวเดียวกับหน้า Edit
        /// </summary>
        [HttpPost]
        [ModuleCheck("edit")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateApproved(int id, int approved)
        {
            try
            {
                // ค่าที่ไม่อยู่ในชุดสถานะที่รองรับ = ปฏิเสธไปเลย (กันค่าหลุดลง DB จนหน้า list แสดงผิด)
                if (!Helpers.AgentStatus.IsValid(approved))
                    return Json(new { success = false, message = "สถานะไม่ถูกต้อง" });

                var itemEdit = _db.ExecuteQuery(
                    "select * from web_agent where id = @id and web_id = @web_id limit 1",
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } }
                );
                if (itemEdit.Rows.Count == 0)
                    return Json(new { success = false, message = "ไม่พบข้อมูล" });

                int oldStatus = Helpers.AgentStatus.Normalize(itemEdit.Rows[0]["approved"]);

                _db.Update(
                    "web_agent",
                    "WHERE id = @id and web_id = @web_id",
                    new Dictionary<string, object>() { { "approved", approved } },
                    new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } }
                );

                string note = ApplyStatusSideEffects(id, itemEdit.Rows[0]["uid"], oldStatus, approved);

                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "edit",
                    action_info: string.Format("แก้ไขสถานะ : {0} ({1}) => {2} ({3})",
                        Module.Config.TextBreadcrumb, id, approved, Helpers.AgentStatus.Label(approved)),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: "web_agent",
                    old_value: _db.DataTableToJSONWithJSONNet(itemEdit).TrimStart('[').TrimEnd(']'),
                    new_value: JsonConvert.SerializeObject(new { approved }),
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text
                );

                string message = string.Format("บันทึกสถานะ \"{0}\" แล้ว", Helpers.AgentStatus.Label(approved))
                    + (string.IsNullOrEmpty(note) ? "" : " — " + note);

                return Json(new { success = true, message, approved });
            }
            catch (Exception e)
            {
                _utility.writeLogs("UpdateApproved error - " + e.Message);
                return Json(new { success = false, message = "เกิดข้อผิดพลาด: " + e.Message });
            }
        }

        /// <summary>
        /// ผลข้างเคียงของการเปลี่ยนสถานะผู้สมัคร — จุดเดียวที่รวมกฎไว้ทั้งหมด
        /// (เรียกทั้งจากหน้า Edit ที่กดบันทึก และจาก dropdown ajax ในหน้า Detail)
        ///
        ///   ผ่าน (1)       → อีเมลยินดีต้อนรับ (ครั้งเดียวตาม flag welcome_mail_sent) + member_type = 3 (Agent)
        ///   ไม่ผ่าน (3)     → อีเมลแจ้งผลไม่ผ่าน (ส่งเมื่อ "เพิ่งเปลี่ยนมาเป็นไม่ผ่าน" เท่านั้น กันส่งซ้ำทุกครั้งที่กดบันทึก)
        ///   Black List (4) → ไม่ส่งอีเมลใด ๆ (ตามที่กำหนด) — สมัครตัวแทนไม่ได้อีก แต่ยังล็อกอินได้
        ///   กำลังตรวจสอบ(0) → ไม่ส่งอีเมล
        ///
        /// คืนข้อความสรุปสั้น ๆ ไว้ต่อท้าย alert (ว่าง = ไม่มีอะไรต้องบอกเพิ่ม)
        /// </summary>
        private string ApplyStatusSideEffects(int agentId, object uid, int oldStatus, int newStatus)
        {
            // member_type ต้องตรงกับสถานะเสมอ แม้สถานะไม่ได้เปลี่ยน (กันข้อมูลเก่าที่ไม่ตรงกัน)
            SyncMemberType(uid, newStatus);

            if (newStatus == Helpers.AgentStatus.Approved)
            {
                bool sent = TrySendWelcomeEmail(agentId);
                return sent
                    ? "ส่งอีเมลแจ้งผู้สมัครแล้ว"
                    : "ไม่ได้ส่งอีเมล (ส่งไปก่อนหน้านี้แล้ว หรือไม่มีอีเมลผู้รับ)";
            }

            if (newStatus == Helpers.AgentStatus.Rejected)
            {
                // ส่งเฉพาะตอนเปลี่ยนสถานะ "เข้ามา" เป็นไม่ผ่าน — กดบันทึกซ้ำทั้งที่เป็นไม่ผ่านอยู่แล้วจะไม่ส่งอีก
                if (oldStatus == Helpers.AgentStatus.Rejected) return "";
                bool sent = TrySendRejectEmail(agentId);
                return sent
                    ? "ส่งอีเมลแจ้งผลไม่ผ่านแล้ว"
                    : "ไม่ได้ส่งอีเมล (ไม่มีอีเมลผู้รับ หรือส่งไม่สำเร็จ)";
            }

            return "";
        }

        /// <summary>
        /// อัปเดต member_type ของ web_member ตามสถานะของตัวแทน
        /// uid ของ web_agent คือค่าเดียวกับ id ของ web_member
        /// "ผ่าน" (approved = 1) => member_type = 3 (Agency), สถานะอื่นทั้งหมด => member_type = 1 (สมาชิกทั่วไป)
        /// </summary>
        private void SyncMemberType(object uid, int approved)
        {
            try
            {
                if (uid == null || uid == DBNull.Value) return;
                if (!int.TryParse(uid.ToString(), out int memberId) || memberId <= 0) return;

                int memberType = approved == Helpers.AgentStatus.Approved ? 3 : 1;

                _db.Update(
                    "web_member",
                    "WHERE id = @id",
                    new Dictionary<string, object>() { { "member_type", memberType } },
                    new Dictionary<string, object>() { { "id", memberId } }
                );
            }
            catch (Exception e)
            {
                _utility.writeLogs("SyncMemberType error - " + e.Message);
            }
        }

        /// <summary>
        /// ส่งอีเมลยินดีต้อนรับให้ผู้สมัครตัวแทน (ส่งครั้งเดียว โดยเช็ค flag welcome_mail_sent)
        /// คืนค่า true เมื่อส่งสำเร็จในครั้งนี้
        /// </summary>
        private bool TrySendWelcomeEmail(int agentId)
        {
            try
            {
                var rows = _db.ExecuteQuery(
                    @"select a.id, a.uid, a.name, a.surname, a.welcome_mail_sent, m.email
                      from web_agent a
                      left join web_member m on m.id = a.uid
                      where a.id = @id and a.web_id = @web_id limit 1",
                    new Dictionary<string, object>() { { "id", agentId }, { "web_id", _currentWebID } }
                );
                if (rows.Rows.Count == 0) return false;
                var r = rows.Rows[0];

                // เคยส่งแล้ว -> ไม่ส่งซ้ำ
                int sentFlag = 0;
                int.TryParse(r["welcome_mail_sent"]?.ToString(), out sentFlag);
                if (sentFlag == 1) return false;

                string email = (r["email"]?.ToString() ?? "").Trim();
                if (string.IsNullOrEmpty(email) || !email.Contains("@")) return false;

                string firstname = (r["name"]?.ToString() ?? "").Trim();
                string surname = (r["surname"]?.ToString() ?? "").Trim();

                string subject = $"ยินดีต้อนรับสู่การเป็นตัวแทน SAM คุณ{firstname}";
                string body = BuildWelcomeEmailBody(firstname, surname);
                string ccList = string.Join(",", GetWelcomeCcEmails());

                bool ok = _utility.Email(MailTo: email, mailTitle: subject, mailBody: body, MailCC: ccList);

                if (ok)
                {
                    _db.Update(
                        "web_agent",
                        "WHERE id = @id and web_id = @web_id",
                        new Dictionary<string, object>() { { "welcome_mail_sent", 1 } },
                        new Dictionary<string, object>() { { "id", agentId }, { "web_id", _currentWebID } }
                    );
                }
                return ok;
            }
            catch (Exception e)
            {
                _utility.writeLogs("TrySendWelcomeEmail error - " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// ส่งอีเมลแจ้ง "ไม่ผ่านการตรวจสอบ" ให้ผู้สมัครตัวแทน
        /// ไม่ใช้ flag เก็บใน DB — คุมการส่งซ้ำที่ ApplyStatusSideEffects (ส่งเฉพาะตอนสถานะเปลี่ยนมาเป็นไม่ผ่าน)
        /// เพื่อให้ตรวจรอบใหม่แล้วตีกลับอีกครั้งยังแจ้งผู้สมัครได้
        /// คืนค่า true เมื่อส่งสำเร็จ
        /// </summary>
        private bool TrySendRejectEmail(int agentId)
        {
            try
            {
                var rows = _db.ExecuteQuery(
                    @"select a.id, a.uid, a.codeid, a.name, a.surname, m.email
                      from web_agent a
                      left join web_member m on m.id = a.uid
                      where a.id = @id and a.web_id = @web_id limit 1",
                    new Dictionary<string, object>() { { "id", agentId }, { "web_id", _currentWebID } }
                );
                if (rows.Rows.Count == 0) return false;
                var r = rows.Rows[0];

                string email = (r["email"]?.ToString() ?? "").Trim();
                if (string.IsNullOrEmpty(email) || !email.Contains("@")) return false;

                string firstname = (r["name"]?.ToString() ?? "").Trim();
                string surname = (r["surname"]?.ToString() ?? "").Trim();
                string codeid = (r["codeid"]?.ToString() ?? "").Trim();

                string subject = $"แจ้งผลการพิจารณาการสมัครเป็นตัวแทนขายทรัพย์ SAM คุณ{firstname}";
                string body = BuildRejectEmailBody(firstname, surname, codeid);
                string ccList = string.Join(",", GetWelcomeCcEmails());

                return _utility.Email(MailTo: email, mailTitle: subject, mailBody: body, MailCC: ccList);
            }
            catch (Exception e)
            {
                _utility.writeLogs("TrySendRejectEmail error - " + e.Message);
                return false;
            }
        }

        private string BuildWelcomeEmailBody(string firstname, string surname)
        {
            string loginUrl = _utility.frontURL("/th/login");
            string fullName = System.Net.WebUtility.HtmlEncode($"{firstname} {surname}".Trim());
            return $@"
<div style=""font-family:'Sarabun',Tahoma,Arial,sans-serif;max-width:600px;margin:0 auto;color:#333;line-height:1.7;"">
  <p>เรียน คุณ {fullName}</p>
  <p>ยินดีต้อนรับสู่การเป็น <strong>ตัวแทนขายทรัพย์ SAM</strong> 🎉</p>
  <p>การสมัครเป็นตัวแทนของคุณได้รับการ <strong>ยืนยัน</strong> เรียบร้อยแล้ว คุณสามารถเข้าสู่ระบบเพื่อใช้งานได้ทันที</p>
  <p style=""text-align:center;margin:32px 0;"">
    <a href=""{loginUrl}"" style=""background:#00b894;color:#fff;text-decoration:none;padding:14px 36px;border-radius:8px;font-size:16px;display:inline-block;"">เข้าสู่ระบบ</a>
  </p>
  <p style=""color:#888;font-size:13px;"">อีเมลฉบับนี้ส่งจากระบบอัตโนมัติ กรุณาอย่าตอบกลับ</p>
</div>";
        }

        /// <summary>
        /// เนื้อหาอีเมลแจ้ง "ไม่ผ่านการตรวจสอบ" — โครงเดียวกับอีเมลยินดีต้อนรับ (กล่องกลาง 600px, ฟอนต์ Sarabun)
        /// ต่างที่โทนสี (แดง = แจ้งผล) และปิดท้ายด้วยช่องทางติดต่อเจ้าหน้าที่แทนปุ่มเข้าสู่ระบบ
        /// </summary>
        private string BuildRejectEmailBody(string firstname, string surname, string codeid)
        {
            string fullName = System.Net.WebUtility.HtmlEncode($"{firstname} {surname}".Trim());
            string hotline = System.Net.WebUtility.HtmlEncode(GetHotlineNumber());
            string codeLine = string.IsNullOrEmpty(codeid)
                ? ""
                : $@"<p style=""margin:0 0 8px;"">หมายเลขใบสมัคร : <strong>{System.Net.WebUtility.HtmlEncode(codeid)}</strong></p>";

            return $@"
<div style=""font-family:'Sarabun',Tahoma,Arial,sans-serif;max-width:600px;margin:0 auto;color:#333;line-height:1.7;"">
  <p>เรียน คุณ {fullName}</p>
  {codeLine}
  <p>ตามที่ท่านได้สมัครเป็น <strong>ตัวแทนขายทรัพย์ SAM</strong> นั้น
     บริษัทฯ ขอเรียนให้ทราบว่า ผลการพิจารณาใบสมัครของท่าน
     <strong style=""color:#c0392b;"">ไม่ผ่านการตรวจสอบ</strong></p>
  <p>หากท่านต้องการทราบรายละเอียดเพิ่มเติม หรือประสงค์จะยื่นเอกสารเพิ่มเติมเพื่อขอรับการพิจารณาอีกครั้ง
     กรุณาติดต่อเจ้าหน้าที่ของบริษัทฯ ตามช่องทางด้านล่าง</p>
  <div style=""background:#f6f7f9;border-left:4px solid #c0392b;padding:14px 18px;margin:24px 0;"">
    <p style=""margin:0;"">ติดต่อเจ้าหน้าที่ : <strong>{hotline}</strong></p>
  </div>
  <p>บริษัทฯ ขอขอบพระคุณที่ท่านให้ความสนใจ</p>
  <p style=""color:#888;font-size:13px;"">อีเมลฉบับนี้ส่งจากระบบอัตโนมัติ กรุณาอย่าตอบกลับ</p>
</div>";
        }

        /// <summary>
        /// เบอร์ติดต่อที่แสดงในอีเมล — ใช้ค่าเดียวกับ "สายด่วน" ท้ายหน้าเว็บ (web_home_footer.pb_tel)
        /// เพื่อไม่ต้องมาแก้เบอร์ในโค้ดเวลาเปลี่ยน
        /// </summary>
        private string GetHotlineNumber()
        {
            try
            {
                var rows = _db.ExecuteQuery("SELECT pb_tel FROM web_home_footer WHERE id = 1 LIMIT 1");
                if (rows.Rows.Count > 0)
                {
                    string tel = (rows.Rows[0]["pb_tel"]?.ToString() ?? "").Trim();
                    if (!string.IsNullOrEmpty(tel)) return "โทร. " + tel;
                }
            }
            catch (Exception e)
            {
                _utility.writeLogs("GetHotlineNumber error - " + e.Message);
            }
            return "ฝ่ายบริหารทรัพย์สิน บริษัท บริหารสินทรัพย์สุขุมวิท จำกัด";
        }

        /// <summary>
        /// ดึงรายชื่ออีเมล CC สำหรับแจ้งเจ้าหน้าที่ จาก web_core_single (module_id = 26, คอลัมน์ pb_t2)
        /// </summary>
        private List<string> GetWelcomeCcEmails()
        {
            var list = new List<string>();
            try
            {
                var rows = _db.ExecuteQuery("SELECT pb_t2 FROM web_core_single WHERE module_id = '26' LIMIT 1");
                if (rows.Rows.Count > 0)
                {
                    string raw = rows.Rows[0]["pb_t2"]?.ToString() ?? "";
                    foreach (var e in raw.Split(new[] { ',', ';', ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var addr = e.Trim();
                        if (addr.Contains("@") && !list.Contains(addr)) list.Add(addr);
                    }
                }
            }
            catch (Exception e)
            {
                _utility.writeLogs("GetWelcomeCcEmails error - " + e.Message);
            }
            return list;
        }

        /// <summary>
        /// ajax: คืนรายการอำเภอ/เขต ตามรหัสจังหวัด (ใช้กับ dropdown ที่อยู่ในหน้า Edit)
        /// </summary>
        [HttpGet]
        public IActionResult GetDistricts(string provinceCode)
        {
            var results = new List<object>();
            if (Helpers.GeoHelper.IsCode(provinceCode))
            {
                var dt = _db.ExecuteQuery(
                    @"select d.code, d.name_in_thai
                      from web_data_districts d
                      join web_data_provinces p on p.id = d.province_id
                      where p.code = @code order by d.name_in_thai",
                    new Dictionary<string, object>() { { "code", int.Parse(provinceCode.Trim()) } }
                );
                foreach (DataRow r in dt.Rows)
                    results.Add(new { code = r["code"].ToString(), name = r["name_in_thai"].ToString() });
            }
            return Json(results);
        }

        /// <summary>
        /// ajax: คืนรายการตำบล/แขวง ตามรหัสอำเภอ (ใช้กับ dropdown ที่อยู่ในหน้า Edit)
        /// </summary>
        [HttpGet]
        public IActionResult GetSubdistricts(string districtCode)
        {
            var results = new List<object>();
            if (Helpers.GeoHelper.IsCode(districtCode))
            {
                var dt = _db.ExecuteQuery(
                    @"select s.code, s.name_in_thai, s.zip_code
                      from web_data_subdistricts s
                      join web_data_districts d on d.id = s.district_id
                      where d.code = @code order by s.name_in_thai",
                    new Dictionary<string, object>() { { "code", int.Parse(districtCode.Trim()) } }
                );
                foreach (DataRow r in dt.Rows)
                    results.Add(new { code = r["code"].ToString(), name = r["name_in_thai"].ToString(), zip = r["zip_code"]?.ToString() });
            }
            return Json(results);
        }
    }
}
