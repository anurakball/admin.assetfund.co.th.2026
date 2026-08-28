using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsMemberController : AdminCoreController
    {
        public LogsMemberController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsMember");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qMember    = Q("q_member");
            var qCode      = Q("q_code");
            var qCategory  = Q("q_category");
            var qSeverity  = Q("q_severity");
            var qStatus    = Q("q_status");
            var qIp        = Q("q_ip");
            var qDateStart = Q("q_date_start");
            var qDateEnd   = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qMember)) {
                where.Append(" AND (CAST(l.member_id AS VARCHAR) = @p_mid OR m.username ILIKE @p_mlike OR m.email ILIKE @p_mlike)");
                prms["p_mid"] = qMember; prms["p_mlike"] = "%" + qMember + "%";
            }
            if (!string.IsNullOrEmpty(qCode))     { where.Append(" AND l.activity_code = @p_code");  prms["p_code"]   = qCode; }
            if (!string.IsNullOrEmpty(qCategory)) { where.Append(" AND t.category = @p_cat");        prms["p_cat"]    = qCategory; }
            if (!string.IsNullOrEmpty(qSeverity)) { where.Append(" AND t.severity = @p_sev");        prms["p_sev"]    = qSeverity; }
            if (!string.IsNullOrEmpty(qStatus))   { where.Append(" AND l.status = @p_status");       prms["p_status"] = qStatus; }
            if (!string.IsNullOrEmpty(qIp))       { where.Append(" AND l.ip_address ILIKE @p_ip");   prms["p_ip"]     = "%" + qIp + "%"; }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND l.activity_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND l.activity_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT l.id, l.member_id, l.activity_code,
                       t.name AS type_name, l.activity_name,
                       t.category, t.severity,
                       l.status, l.error_message,
                       l.ip_address, l.device_type, l.browser, l.os,
                       l.country, l.city,
                       l.duration_ms, l.request_url, l.request_method,
                       l.description, l.activity_at,
                       m.username, m.email
                FROM web_member_activity_log l
                LEFT JOIN web_member_activity_type t ON l.activity_type_id = t.id
                LEFT JOIN web_member m ON l.member_id = m.id
                {where}
                ORDER BY l.activity_at DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("Member Activity Log");

            var headers = new[] {
                "#", "วันที่/เวลา", "Member ID", "Username", "Email",
                "Activity Code", "ชื่อ Activity", "Category", "Severity", "สถานะ",
                "IP Address", "Device", "Browser", "OS", "Location",
                "ระยะเวลา (ms)", "Method", "URL", "คำอธิบาย", "Error"
            };

            var headerColor = System.Drawing.Color.FromArgb(31, 78, 121);
            for (int c = 0; c < headers.Length; c++) {
                var cell = ws.Cells[1, c + 1];
                cell.Value = headers[c];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(headerColor);
            }

            int row = 2;
            foreach (System.Data.DataRow dr in dt.Rows) {
                var at  = dr["activity_at"] != System.DBNull.Value ? Convert.ToDateTime(dr["activity_at"]) : DateTime.MinValue;
                var dur = dr["duration_ms"] != System.DBNull.Value ? (object)Convert.ToInt32(dr["duration_ms"]) : null;
                var city    = dr["city"]?.ToString() ?? "";
                var country = dr["country"]?.ToString() ?? "";
                var loc     = !string.IsNullOrEmpty(city) ? city + (!string.IsNullOrEmpty(country) ? ", " + country : "") : country;

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value  = dr["member_id"]?.ToString();
                ws.Cells[row, 4].Value  = dr["username"]?.ToString();
                ws.Cells[row, 5].Value  = dr["email"]?.ToString();
                ws.Cells[row, 6].Value  = dr["activity_code"]?.ToString();
                ws.Cells[row, 7].Value  = dr["type_name"]?.ToString() ?? dr["activity_name"]?.ToString();
                ws.Cells[row, 8].Value  = dr["category"]?.ToString();
                ws.Cells[row, 9].Value  = dr["severity"]?.ToString();
                ws.Cells[row, 10].Value = dr["status"]?.ToString();
                ws.Cells[row, 11].Value = dr["ip_address"]?.ToString();
                ws.Cells[row, 12].Value = dr["device_type"]?.ToString();
                ws.Cells[row, 13].Value = dr["browser"]?.ToString();
                ws.Cells[row, 14].Value = dr["os"]?.ToString();
                ws.Cells[row, 15].Value = loc;
                ws.Cells[row, 16].Value = dur;
                ws.Cells[row, 17].Value = dr["request_method"]?.ToString();
                ws.Cells[row, 18].Value = dr["request_url"]?.ToString();
                ws.Cells[row, 19].Value = dr["description"]?.ToString();
                ws.Cells[row, 20].Value = dr["error_message"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName = $"member_activity_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
