using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsGoogleMapController : AdminCoreController
    {
        public LogsGoogleMapController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsGoogleMap");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qApi       = Q("q_api");
            var qMethod    = Q("q_method");
            var qSuccess   = Q("q_success");
            var qIp        = Q("q_ip");
            var qDateStart = Q("q_date_start");
            var qDateEnd   = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qApi))    { where.Append(" AND l.api_name LIKE @p_api");    prms["p_api"]    = "%" + qApi + "%"; }
            if (!string.IsNullOrEmpty(qMethod)) { where.Append(" AND l.http_method = @p_method");  prms["p_method"] = qMethod; }
            if (!string.IsNullOrEmpty(qIp))     { where.Append(" AND l.client_ip LIKE @p_ip");    prms["p_ip"]     = "%" + qIp + "%"; }
            if (qSuccess == "true")             { where.Append(" AND l.success = 1"); }
            else if (qSuccess == "false")       { where.Append(" AND l.success = 0"); }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND l.created_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND l.created_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT l.id, l.created_at,
                       l.api_name, l.endpoint, l.http_method,
                       l.response_status, l.success, l.response_time_ms,
                       l.error_message, l.client_ip, l.user_agent
                FROM [2026_api_google_map_logs] l
                {where}
                ORDER BY l.created_at DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("Google Map API Log");

            var headers = new[] {
                "#", "วันที่/เวลา", "API Name", "Endpoint", "Method",
                "HTTP Status", "สถานะ", "Response Time (ms)",
                "Client IP", "User Agent", "Error Message"
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
                var at        = dr["created_at"]       != System.DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue;
                var httpSt    = dr["response_status"]  != System.DBNull.Value ? (object)Convert.ToInt32(dr["response_status"]) : null;
                var dur       = dr["response_time_ms"] != System.DBNull.Value ? (object)Convert.ToInt32(dr["response_time_ms"]) : null;
                var isSuccess = dr["success"]          != System.DBNull.Value && Convert.ToBoolean(dr["success"]);

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value  = dr["api_name"]?.ToString();
                ws.Cells[row, 4].Value  = dr["endpoint"]?.ToString();
                ws.Cells[row, 5].Value  = dr["http_method"]?.ToString();
                ws.Cells[row, 6].Value  = httpSt;
                ws.Cells[row, 7].Value  = isSuccess ? "Success" : "Error";
                ws.Cells[row, 8].Value  = dur;
                ws.Cells[row, 9].Value  = dr["client_ip"]?.ToString();
                ws.Cells[row, 10].Value = dr["user_agent"]?.ToString();
                ws.Cells[row, 11].Value = dr["error_message"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName  = $"google_map_api_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
