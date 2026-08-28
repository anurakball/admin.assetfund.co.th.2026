using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class EFormSubmitController : AdminCoreController
    {
        public EFormSubmitController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("EFormSubmit");

            Module = _admin.setModuleCMSPage(Module);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Export แบบ "pivot" — แต่ละ "ช่องของฟอร์ม" กลายเป็น 1 คอลัมน์ใน Excel
        //
        // ปัญหาเดิม: export ทั่วไปทิ้ง info/en_info เป็นสตริงยาว ๆ คั่นด้วย '|' ในเซลล์เดียว
        //           → กรอง/เรียง/สรุปผลไม่ได้เลย
        // ของใหม่:  คอลัมน์ = [รหัส, วันที่ส่งข้อมูล, IP, แบบฟอร์ม, <คำถามที่ 1>, <คำถามที่ 2>, ...]
        //           ค่าของแต่ละแถวถูกจับคู่ label→value ; ไฟล์แนบแสดงเป็น URL ดาวน์โหลด
        //           (union ของ label ทุกฟอร์มในผลลัพธ์ โดยคงลำดับที่พบครั้งแรก — ปกติกรองทีละฟอร์มจะได้คอลัมน์ตรง ๆ)
        // ─────────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ModuleCheck("export")]
        public override IActionResult Export(IFormCollection f)
        {
            if (!f.ContainsKey("export") || string.IsNullOrEmpty(f["export"]))
            {
                TempData["alert_message"] = "ไม่สามารถ export ข้อมูลได้ (query not set)";
                TempData["alert_class"] = "alert-error";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }

            string data_decrypt = _utility.Decrypt(f["export"].ToString(), _utility.appKey());
            if (string.IsNullOrEmpty(data_decrypt))
            {
                TempData["alert_message"] = "ไม่สามารถ export ข้อมูลได้ (decrypt)";
                TempData["alert_class"] = "alert-error";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }

            try
            {
                JObject sql_object = JObject.Parse(data_decrypt);
                string sql = sql_object["sql"].ToString();
                var sql_parameter = JsonConvert.DeserializeObject<Dictionary<string, object>>(sql_object["parameter"].ToString())
                                    ?? new Dictionary<string, object>();

                #region ----- filter เฉพาะ id ที่เลือก (checkbox) -----
                if (f.ContainsKey("export_id") && !string.IsNullOrEmpty(f["export_id"]))
                {
                    var ids = f["export_id"].ToString().Split(',');
                    bool allInt = ids.Length > 0 && ids.All(x => int.TryParse(x, out _));
                    if (allInt)
                    {
                        var sql_arr = sql.Split("where");
                        if (sql_arr.Length > 1)
                        {
                            sql = string.Format(" {0} where id IN ({1}) and {2} ", sql_arr[0], f["export_id"], sql_arr[1]);
                        }
                    }
                }
                #endregion

                if (!sql_parameter.ContainsKey("web_id")) { sql_parameter.Add("web_id", _currentWebID); }

                var dt = _db.ExecuteQuery(sql, sql_parameter);

                #region ----- แปลงเป็นตาราง pivot -----
                var labelOrder = new List<string>();          // ลำดับคอลัมน์คำถาม (คงลำดับที่พบครั้งแรก)
                var labelSet = new HashSet<string>();
                var formCache = new Dictionary<string, string>();
                var records = new List<(string id, string date, string ip, string form, Dictionary<string, string> vals)>();

                foreach (DataRow r in dt.Rows)
                {
                    var pairs = EFormSubmitHelper.ParsePairs(r["info"] + "", r["en_info"] + "");
                    var vals = new Dictionary<string, string>();
                    foreach (var p in pairs)
                    {
                        if (!labelSet.Contains(p.Label)) { labelSet.Add(p.Label); labelOrder.Add(p.Label); }
                        string v = EFormSubmitHelper.IsFileValue(p.Value)
                            ? string.Join("\n", EFormSubmitHelper.FileTokens(p.Value).Select(fn => _utility.frontURL("/uploads/eform/" + fn)))
                            : EFormSubmitHelper.StripHtml(p.Value);
                        vals[p.Label] = v;   // label ซ้ำ (กรณีหลายฟอร์ม) → ค่าหลังทับ
                    }

                    string catId = r["cat_id"] + "";
                    if (!formCache.TryGetValue(catId, out var formName))
                    {
                        formName = _admin.GetCatTitle("web_eform", catId);
                        if (string.IsNullOrWhiteSpace(formName)) { formName = "#" + catId; }
                        formCache[catId] = formName;
                    }

                    string date = "";
                    try { date = ((DateTime)r["created_at"]).ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"); } catch { }

                    records.Add((r["id"] + "", date, r["des"] + "", formName, vals));
                }
                #endregion

                #region ----- เขียน Excel -----
                var headers = new List<string> { "รหัส", "วันที่ส่งข้อมูล", "IP Address", "แบบฟอร์ม" };
                headers.AddRange(labelOrder);

                using var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Sheet1");

                for (int c = 0; c < headers.Count; c++)
                {
                    var cell = ws.Cells[1, c + 1];
                    cell.Value = headers[c];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(11, 107, 79));
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rr = 2;
                foreach (var rec in records)
                {
                    ws.Cells[rr, 1].Value = rec.id;
                    ws.Cells[rr, 2].Value = rec.date;
                    ws.Cells[rr, 3].Value = rec.ip;
                    ws.Cells[rr, 4].Value = rec.form;
                    for (int li = 0; li < labelOrder.Count; li++)
                    {
                        if (rec.vals.TryGetValue(labelOrder[li], out var v) && !string.IsNullOrEmpty(v))
                        {
                            ws.Cells[rr, 5 + li].Value = v;
                        }
                    }
                    rr++;
                }

                // จัดความกว้างคอลัมน์ (จำกัดไม่ให้กว้างเกินไป) + ตีกรอบหัวตาราง
                if (ws.Dimension != null)
                {
                    ws.Cells[ws.Dimension.Address].Style.WrapText = false;
                    for (int c = 1; c <= headers.Count; c++)
                    {
                        ws.Column(c).AutoFit();
                        if (ws.Column(c).Width > 55) { ws.Column(c).Width = 55; }
                        if (ws.Column(c).Width < 12) { ws.Column(c).Width = 12; }
                    }
                    ws.View.FreezePanes(2, 1);   // ตรึงหัวตาราง
                }
                #endregion

                #region Logs Action
                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "export",
                    action_info: string.Format("export ข้อมูล (pivot) : {0} ({1} แถว, {2} คอลัมน์คำถาม)", Module.Config.TextBreadcrumb, records.Count, labelOrder.Count),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: Module.Config.Table,
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text
                );
                #endregion

                var ms = new MemoryStream();
                excel.SaveAs(ms);
                ms.Position = 0;
                string file_name = Module.Name + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_.xlsx";
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name);
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาดในการ export, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }
    }
}
