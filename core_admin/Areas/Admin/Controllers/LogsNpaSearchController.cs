using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // Logs/คำค้นหาทรัพย์ NPA — อ่านตาราง api_npa_search_log (DB:PG)
    // front-end (d:\Project\sam.or.th : HomeController.LogNpaSearch) บันทึกทุกครั้งที่มีการค้นหาทรัพย์
    // และล้าง log ที่เก่ากว่า 30 วันทิ้งอัตโนมัติ — หน้านี้จึงเห็นย้อนหลังได้ 30 วัน
    public class LogsNpaSearchController : AdminCoreController
    {
        public LogsNpaSearchController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("LogsNpaSearch");
        }

        [HttpGet]
        public IActionResult ExportExcel()
        {
            string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

            var prms = new Dictionary<string, object>();
            var where = NpaSearchLogHelper.BuildWhere(Q, prms);

            var dt = _db.ExecuteQuery($@"
                SELECT l.id, l.created_at, l.keyword, l.search_mode, l.lang,
                       l.filters, l.sort, l.page, l.result_count,
                       l.query_string, l.client_ip, l.user_agent, l.referer
                FROM [2026_api_npa_search_log] l
                {where}
                ORDER BY l.created_at DESC", prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();

            #region ----- Sheet 1 : รายการค้นหา -----
            var ws = excel.Workbook.Worksheets.Add("คำค้นหาทรัพย์ NPA");

            var headers = new[] {
                "#", "วันที่/เวลา", "คำค้นหา", "ประเภทการค้นหา", "ภาษา",
                "ตัวกรองที่ใช้", "การเรียง", "หน้า", "จำนวนผลลัพธ์",
                "Query String", "Client IP", "Referer", "User Agent"
            };

            var headerColor = System.Drawing.Color.FromArgb(31, 78, 121);
            for (int c = 0; c < headers.Length; c++)
            {
                var cell = ws.Cells[1, c + 1];
                cell.Value = headers[c];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(headerColor);
            }

            int row = 2;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                var at  = dr["created_at"]   != System.DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue;
                var cnt = dr["result_count"] != System.DBNull.Value ? (object)Convert.ToInt64(dr["result_count"]) : null;
                var pg  = dr["page"]         != System.DBNull.Value ? (object)Convert.ToInt32(dr["page"]) : null;

                ws.Cells[row, 1].Value  = row - 1;
                ws.Cells[row, 2].Value  = at != DateTime.MinValue ? at.ToString("dd/MM/yyyy HH:mm:ss") : "";
                ws.Cells[row, 3].Value  = dr["keyword"]?.ToString();
                ws.Cells[row, 4].Value  = dr["search_mode"]?.ToString();
                ws.Cells[row, 5].Value  = dr["lang"]?.ToString();
                ws.Cells[row, 6].Value  = NpaSearchLogHelper.Summary(dr["filters"]?.ToString());
                ws.Cells[row, 7].Value  = dr["sort"]?.ToString();
                ws.Cells[row, 8].Value  = pg;
                ws.Cells[row, 9].Value  = cnt;
                ws.Cells[row, 10].Value = dr["query_string"]?.ToString();
                ws.Cells[row, 11].Value = dr["client_ip"]?.ToString();
                ws.Cells[row, 12].Value = dr["referer"]?.ToString();
                ws.Cells[row, 13].Value = dr["user_agent"]?.ToString();
                row++;
            }

            if (dt.Rows.Count > 0)
            {
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                //----- คำค้นหาบางรายการยาวมาก (ผู้ใช้พิมพ์รัว) จำกัดความกว้างไม่ให้ไฟล์อ่านยาก
                if (ws.Column(3).Width > 60) ws.Column(3).Width = 60;
                if (ws.Column(10).Width > 60) ws.Column(10).Width = 60;
                if (ws.Column(12).Width > 50) ws.Column(12).Width = 50;
                if (ws.Column(13).Width > 50) ws.Column(13).Width = 50;
            }
            #endregion

            #region ----- Sheet 2 : สรุปคำค้นหายอดนิยม -----
            var dtTop = _db.ExecuteQuery($@"
                SELECT LTRIM(RTRIM(l.keyword)) AS kw,
                       COUNT(*) AS cnt,
                       CAST(COALESCE(AVG(l.result_count), 0) AS bigint) AS avg_result,
                       SUM(CASE WHEN COALESCE(l.result_count, 0) = 0 THEN 1 ELSE 0 END) AS cnt_zero,
                       MAX(l.created_at) AS last_at
                FROM [2026_api_npa_search_log] l
                {where}
                  AND l.keyword IS NOT NULL AND LTRIM(RTRIM(l.keyword)) <> ''
                GROUP BY LTRIM(RTRIM(l.keyword))
                ORDER BY cnt DESC, last_at DESC", prms);

            var ws2 = excel.Workbook.Worksheets.Add("คำค้นหายอดนิยม");
            var headers2 = new[] { "อันดับ", "คำค้นหา", "จำนวนครั้ง", "ผลลัพธ์เฉลี่ย", "ครั้งที่ไม่พบผลลัพธ์", "ค้นหาล่าสุด" };
            for (int c = 0; c < headers2.Length; c++)
            {
                var cell = ws2.Cells[1, c + 1];
                cell.Value = headers2[c];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(headerColor);
            }

            int row2 = 2;
            foreach (System.Data.DataRow dr in dtTop.Rows)
            {
                var lastAt = dr["last_at"] != System.DBNull.Value ? Convert.ToDateTime(dr["last_at"]) : DateTime.MinValue;
                ws2.Cells[row2, 1].Value = row2 - 1;
                ws2.Cells[row2, 2].Value = dr["kw"]?.ToString();
                ws2.Cells[row2, 3].Value = Convert.ToInt64(dr["cnt"]);
                ws2.Cells[row2, 4].Value = Convert.ToInt64(dr["avg_result"]);
                ws2.Cells[row2, 5].Value = Convert.ToInt64(dr["cnt_zero"]);
                ws2.Cells[row2, 6].Value = lastAt != DateTime.MinValue ? lastAt.ToString("dd/MM/yyyy HH:mm:ss") : "";
                row2++;
            }

            if (dtTop.Rows.Count > 0)
            {
                ws2.Cells[ws2.Dimension.Address].AutoFitColumns();
                if (ws2.Column(2).Width > 60) ws2.Column(2).Width = 60;
            }
            #endregion

            var fileName  = $"npa_search_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
