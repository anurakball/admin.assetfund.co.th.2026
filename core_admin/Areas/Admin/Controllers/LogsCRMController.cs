using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsCRMController : AdminCoreController
    {
        public LogsCRMController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsCRM");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qService   = Q("q_service");
            var qModule    = Q("q_module");
            var qMethod    = Q("q_method");
            var qSuccess   = Q("q_success");
            var qEnv       = Q("q_env");
            var qTrace     = Q("q_trace");
            var qIp        = Q("q_ip");
            var qDateStart = Q("q_date_start");
            var qDateEnd   = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qService)) { where.Append(" AND l.service_name LIKE @p_svc");    prms["p_svc"]    = "%" + qService + "%"; }
            if (!string.IsNullOrEmpty(qModule))  { where.Append(" AND l.module_name LIKE @p_mod");     prms["p_mod"]    = "%" + qModule + "%"; }
            if (!string.IsNullOrEmpty(qMethod))  { where.Append(" AND l.http_method = @p_method");      prms["p_method"] = qMethod; }
            if (!string.IsNullOrEmpty(qEnv))     { where.Append(" AND l.environment_name = @p_env");    prms["p_env"]    = qEnv; }
            if (!string.IsNullOrEmpty(qTrace))   { where.Append(" AND l.trace_id LIKE @p_trace");      prms["p_trace"]  = "%" + qTrace + "%"; }
            if (!string.IsNullOrEmpty(qIp))      { where.Append(" AND l.client_ip LIKE @p_ip");        prms["p_ip"]     = "%" + qIp + "%"; }

            if (qSuccess == "1")      { where.Append(" AND l.is_success = 1"); }
            else if (qSuccess == "0") { where.Append(" AND l.is_success = 0"); }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND l.created_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND l.created_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT l.id, l.created_at,
                       l.service_name, l.module_name,
                       l.http_method, l.endpoint_url,
                       l.response_status_code,
                       l.is_success, l.error_message,
                       l.duration_ms, l.client_ip, l.user_agent,
                       l.member_id, l.created_by,
                       l.trace_id, l.correlation_id,
                       l.environment_name, l.remark
                FROM [2026_api_crm_logs] l
                {where}
                ORDER BY l.created_at DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("CRM API Logs");

            var headers = new[] {
                "#", "วันที่/เวลา", "Service Name", "Module / Function",
                "HTTP Method", "Endpoint URL", "Status Code", "ผลลัพธ์",
                "Duration (ms)", "Client IP", "User Agent",
                "Member ID", "Created By", "Trace ID", "Correlation ID",
                "Environment", "Error Message", "Remark"
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
                var at       = dr["created_at"] != System.DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue;
                var dur      = dr["duration_ms"] != System.DBNull.Value ? (object)Convert.ToInt32(dr["duration_ms"]) : null;
                var isSuccess = dr["is_success"] != System.DBNull.Value && Convert.ToBoolean(dr["is_success"]);

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value  = dr["service_name"]?.ToString();
                ws.Cells[row, 4].Value  = dr["module_name"]?.ToString();
                ws.Cells[row, 5].Value  = dr["http_method"]?.ToString();
                ws.Cells[row, 6].Value  = dr["endpoint_url"]?.ToString();
                ws.Cells[row, 7].Value  = dr["response_status_code"] != System.DBNull.Value ? Convert.ToInt32(dr["response_status_code"]) : (object?)null;
                ws.Cells[row, 8].Value  = isSuccess ? "Success" : "Failed";
                ws.Cells[row, 9].Value  = dur;
                ws.Cells[row, 10].Value = dr["client_ip"]?.ToString();
                ws.Cells[row, 11].Value = dr["user_agent"]?.ToString();
                ws.Cells[row, 12].Value = dr["member_id"]?.ToString();
                ws.Cells[row, 13].Value = dr["created_by"]?.ToString();
                ws.Cells[row, 14].Value = dr["trace_id"]?.ToString();
                ws.Cells[row, 15].Value = dr["correlation_id"]?.ToString();
                ws.Cells[row, 16].Value = dr["environment_name"]?.ToString();
                ws.Cells[row, 17].Value = dr["error_message"]?.ToString();
                ws.Cells[row, 18].Value = dr["remark"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName  = $"crm_api_logs_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
