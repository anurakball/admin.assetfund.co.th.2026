using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsLINEController : AdminCoreController
    {
        public LogsLINEController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsLINE");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qEndpoint   = Q("q_endpoint");
            var qSuccess    = Q("q_success");
            var qUid        = Q("q_uid");
            var qIp         = Q("q_ip");
            var qErrorCode  = Q("q_error_code");
            var qDateStart  = Q("q_date_start");
            var qDateEnd    = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qEndpoint))  { where.Append(" AND endpoint ILIKE @p_ep");          prms["p_ep"]  = "%" + qEndpoint + "%"; }
            if (qSuccess == "true")                { where.Append(" AND success = TRUE"); }
            else if (qSuccess == "false")          { where.Append(" AND success = FALSE"); }
            if (!string.IsNullOrEmpty(qUid))       { where.Append(" AND CAST(uid AS VARCHAR) = @p_uid"); prms["p_uid"] = qUid; }
            if (!string.IsNullOrEmpty(qIp))        { where.Append(" AND ip_address ILIKE @p_ip");        prms["p_ip"]  = "%" + qIp + "%"; }
            if (!string.IsNullOrEmpty(qErrorCode)) { where.Append(" AND error_code ILIKE @p_ec");        prms["p_ec"]  = "%" + qErrorCode + "%"; }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND log_date >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND log_date < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT id, log_date, endpoint, success, uid, detail, ip_address, error_code
                FROM api_audit_log
                {where}
                ORDER BY log_date DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("LINE LIFF API Log");

            var headers = new[] { "#", "วันที่/เวลา", "Endpoint", "สถานะ", "UID", "Detail", "IP Address", "Error Code" };
            var headerColor = System.Drawing.Color.FromArgb(0, 114, 178);
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
                var at      = dr["log_date"] != System.DBNull.Value ? Convert.ToDateTime(dr["log_date"]) : DateTime.MinValue;
                var success = dr["success"] != System.DBNull.Value && Convert.ToBoolean(dr["success"]);

                ws.Cells[row, 1].Value = row - 1;
                ws.Cells[row, 2].Value = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value = dr["endpoint"]?.ToString();
                ws.Cells[row, 4].Value = success ? "Success" : "Failed";
                ws.Cells[row, 5].Value = dr["uid"] != System.DBNull.Value ? dr["uid"]?.ToString() : "";
                ws.Cells[row, 6].Value = dr["detail"]?.ToString();
                ws.Cells[row, 7].Value = dr["ip_address"]?.ToString();
                ws.Cells[row, 8].Value = dr["error_code"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName  = $"line_liff_api_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
