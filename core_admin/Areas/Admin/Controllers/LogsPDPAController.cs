using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class LogsPDPAController : AdminCoreController
    {
        public LogsPDPAController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsPDPA");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var qMember    = Q("q_member");
            var qCode      = Q("q_code");
            var qAccept    = Q("q_accept");
            var qVersion   = Q("q_version");
            var qIp        = Q("q_ip");
            var qDateStart = Q("q_date_start");
            var qDateEnd   = Q("q_date_end");

            var where = new System.Text.StringBuilder("WHERE 1=1");
            var prms  = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(qMember)) {
                where.Append(" AND (CAST(c.member_id AS VARCHAR) = @p_mid OR m.username ILIKE @p_mlike OR m.email ILIKE @p_mlike OR c.email ILIKE @p_mlike)");
                prms["p_mid"] = qMember; prms["p_mlike"] = "%" + qMember + "%";
            }
            if (!string.IsNullOrEmpty(qCode))    { where.Append(" AND c.consent_code = @p_code");      prms["p_code"]    = qCode; }
            if (!string.IsNullOrEmpty(qVersion)) { where.Append(" AND c.policy_version = @p_version"); prms["p_version"] = qVersion; }
            if (!string.IsNullOrEmpty(qIp))      { where.Append(" AND c.ip_address ILIKE @p_ip");      prms["p_ip"]      = "%" + qIp + "%"; }

            if (qAccept == "1")      { where.Append(" AND c.is_accept = TRUE"); }
            else if (qAccept == "0") { where.Append(" AND c.is_accept = FALSE"); }

            if (!string.IsNullOrEmpty(qDateStart)) {
                try { var p = qDateStart.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND c.consent_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            if (!string.IsNullOrEmpty(qDateEnd)) {
                try { var p = qDateEnd.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND c.consent_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            var dt = _db.ExecuteQuery($@"
                SELECT c.id, c.member_id, c.email,
                       c.consent_code, c.consent_title, c.is_accept,
                       c.consent_at, c.ip_address, c.user_agent,
                       c.policy_version, c.created_at,
                       m.username
                FROM web_pdpa_consent c
                LEFT JOIN web_member m ON c.member_id = m.id
                {where}
                ORDER BY c.consent_at DESC NULLS LAST, c.id DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("PDPA Consent Log");

            var headers = new[] {
                "#", "วันที่ให้ความยินยอม", "Member ID", "Username", "Email",
                "Consent Code", "Consent Title", "สถานะ", "Policy Version",
                "IP Address", "User Agent", "บันทึกเมื่อ"
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
                var at  = dr["consent_at"] != System.DBNull.Value ? Convert.ToDateTime(dr["consent_at"]) : DateTime.MinValue;
                var cat = dr["created_at"] != System.DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue;
                var isAccept = dr["is_accept"] != System.DBNull.Value && Convert.ToBoolean(dr["is_accept"]);

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at  != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss")  : "";
                ws.Cells[row, 3].Value  = dr["member_id"]?.ToString();
                ws.Cells[row, 4].Value  = dr["username"]?.ToString();
                ws.Cells[row, 5].Value  = dr["email"]?.ToString();
                ws.Cells[row, 6].Value  = dr["consent_code"]?.ToString();
                ws.Cells[row, 7].Value  = dr["consent_title"]?.ToString();
                ws.Cells[row, 8].Value  = isAccept ? "ยินยอม" : "ไม่ยินยอม";
                ws.Cells[row, 9].Value  = dr["policy_version"]?.ToString();
                ws.Cells[row, 10].Value = dr["ip_address"]?.ToString();
                ws.Cells[row, 11].Value = dr["user_agent"]?.ToString();
                ws.Cells[row, 12].Value = cat != DateTime.MinValue ? cat.ToString("dd/MM/yyyy HH:mm:ss") : "";
                row++;
            }

            if (dt.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileName  = $"pdpa_consent_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
