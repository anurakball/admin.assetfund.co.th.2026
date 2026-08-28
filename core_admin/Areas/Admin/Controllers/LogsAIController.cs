using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsAIController : AdminCoreController
    {
        public LogsAIController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsAI");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qMember    = Q("q_member");
            var qService   = Q("q_service");
            var qApi       = Q("q_api");
            var qModel     = Q("q_model");
            var qSuccess   = Q("q_success");
            var qIp        = Q("q_ip");
            var qDateStart = Q("q_date_start");
            var qDateEnd   = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qMember)) {
                where.Append(" AND (CAST(l.member_id AS VARCHAR) = @p_mid OR l.username LIKE @p_mlike)");
                prms["p_mid"] = qMember; prms["p_mlike"] = "%" + qMember + "%";
            }
            if (!string.IsNullOrEmpty(qService)) { where.Append(" AND l.service_name = @p_svc");    prms["p_svc"]   = qService; }
            if (!string.IsNullOrEmpty(qApi))     { where.Append(" AND l.api_name LIKE @p_api");    prms["p_api"]   = "%" + qApi + "%"; }
            if (!string.IsNullOrEmpty(qModel))   { where.Append(" AND l.model_name LIKE @p_model");prms["p_model"] = "%" + qModel + "%"; }
            if (!string.IsNullOrEmpty(qIp))      { where.Append(" AND l.request_ip LIKE @p_ip");   prms["p_ip"]    = "%" + qIp + "%"; }
            if (qSuccess == "true")              { where.Append(" AND l.is_success = 1"); }
            else if (qSuccess == "false")        { where.Append(" AND l.is_success = 0"); }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND l.created_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND l.created_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT l.id, l.created_at,
                       l.service_name, l.api_name,
                       l.member_id, l.username,
                       l.request_method, l.request_ip,
                       l.response_status,
                       l.is_success, l.error_message,
                       l.duration_ms,
                       l.model_name, l.prompt_tokens, l.completion_tokens, l.total_tokens,
                       l.reference_code, l.remark
                FROM [2026_api_service_logs] l
                {where}
                ORDER BY l.created_at DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("AI Service API Log");

            var headers = new[] {
                "#", "วันที่/เวลา", "Service", "API Name",
                "Member ID", "Username", "Model",
                "Prompt Tokens", "Completion Tokens", "Total Tokens",
                "Method", "HTTP Status", "IP Address",
                "ระยะเวลา (ms)", "สถานะ", "Error Message",
                "Reference Code", "Remark"
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
                var at       = dr["created_at"]   != System.DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue;
                var dur      = dr["duration_ms"]  != System.DBNull.Value ? (object)Convert.ToInt32(dr["duration_ms"])  : null;
                var httpSt   = dr["response_status"] != System.DBNull.Value ? (object)Convert.ToInt32(dr["response_status"]) : null;
                var ptok     = dr["prompt_tokens"]     != System.DBNull.Value ? (object)Convert.ToInt32(dr["prompt_tokens"])     : null;
                var ctok     = dr["completion_tokens"] != System.DBNull.Value ? (object)Convert.ToInt32(dr["completion_tokens"]) : null;
                var ttok     = dr["total_tokens"]      != System.DBNull.Value ? (object)Convert.ToInt32(dr["total_tokens"])      : null;
                var isSuccess = dr["is_success"] != System.DBNull.Value && Convert.ToBoolean(dr["is_success"]);

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value  = dr["service_name"]?.ToString();
                ws.Cells[row, 4].Value  = dr["api_name"]?.ToString();
                ws.Cells[row, 5].Value  = dr["member_id"]?.ToString();
                ws.Cells[row, 6].Value  = dr["username"]?.ToString();
                ws.Cells[row, 7].Value  = dr["model_name"]?.ToString();
                ws.Cells[row, 8].Value  = ptok;
                ws.Cells[row, 9].Value  = ctok;
                ws.Cells[row, 10].Value = ttok;
                ws.Cells[row, 11].Value = dr["request_method"]?.ToString();
                ws.Cells[row, 12].Value = httpSt;
                ws.Cells[row, 13].Value = dr["request_ip"]?.ToString();
                ws.Cells[row, 14].Value = dur;
                ws.Cells[row, 15].Value = isSuccess ? "Success" : "Error";
                ws.Cells[row, 16].Value = dr["error_message"]?.ToString();
                ws.Cells[row, 17].Value = dr["reference_code"]?.ToString();
                ws.Cells[row, 18].Value = dr["remark"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName  = $"ai_service_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
