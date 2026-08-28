using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    // รายงานตัวแทน (Agent) — สรุปเป็นตารางแบบเดียวกับไฟล์ Excel ที่ฝ่ายงานใช้อยู่
    //   1) จำนวนตัวแทนแยกตามปีที่สมัคร            4) Top 10 จังหวัดที่มีนายหน้าลงทะเบียนขาย (ทรัพย์+มูลค่า)
    //   2) แยกตามเพศ/ประเภทผู้สมัคร               5) Top 10 รหัสทรัพย์ที่มีคนแจ้งซ้ำมากที่สุด
    //   3) Top 10 จังหวัดของตัวแทน                6) Top 10 ประเภททรัพย์ (ตัดซ้ำ)
    // ตรรกะการรวมยอดทั้งหมดอยู่ใน Helpers/AgentReportHelper.cs (ใช้ร่วมกับปุ่ม Export Excel)
    public class AgentReportController : AdminCoreController
    {
        // จำนวนแถวสูงสุดของตาราง Top N ในรายงาน
        public const int TopN = 10;

        public AgentReportController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AgentReport");
        }

        private string Q(string key) => Request.Query.ContainsKey(key) ? (Request.Query[key].ToString() ?? "").Trim() : "";

        public override IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);
            Module = _admin.setBreadcrumbCMSPage(Module);

            try
            {
                var prms  = new Dictionary<string, object>();
                var where = AgentReportHelper.BuildWhere(Q, prms, _currentWebID);
                var data  = AgentReportHelper.Build(_db, where, prms);

                int _access_id = _session.GetInt32("admin_access_id") ?? 0;
                Module.Config.CanAdd    = _admin.checkAccess(Module, _access_id, "add");
                Module.Config.CanEdit   = _admin.checkAccess(Module, _access_id, "edit");
                Module.Config.CanDelete = _admin.checkAccess(Module, _access_id, "delete");

                ViewBag._utility   = _utility;
                ViewBag._admin     = _admin;
                ViewBag._session   = _session;
                ViewBag._db        = _db;
                ViewBag.Module     = Module;
                ViewBag.Title      = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.searchInputVal = _admin.getSearchInputValue(Module);

                ViewBag.Report = data;
                ViewBag.TopN   = TopN;
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new thaicredit_hr_admin.Areas.Admin.Models.ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }

            return View("~/Areas/Admin/Views/AgentReport/Index.cshtml");
        }

        /// <summary>
        /// ดาวน์โหลดรายงานเป็น Excel — ใช้เงื่อนไขค้นหาชุดเดียวกับหน้าจอ (ส่งต่อผ่าน query string)
        /// 1 ตาราง = 1 sheet และใส่ครบทุกแถว (ไม่ตัดแค่ Top 10) เพื่อให้เอาไปทำ pivot ต่อได้
        /// </summary>
        [HttpGet]
        public IActionResult ExportExcel()
        {
            var prms  = new Dictionary<string, object>();
            var where = AgentReportHelper.BuildWhere(Q, prms, _currentWebID);
            var data  = AgentReportHelper.Build(_db, where, prms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excel = new ExcelPackage();

            AddSheet(excel, "จำนวน Agent ตามปี", new[] { "ปี", "จำนวน" }, data.ByYear,
                     r => new object[] { r.Label, r.Count }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2 });

            AddSheet(excel, "แยกตามเพศ-ประเภท", new[] { "ประเภท", "จำนวน" }, data.ByPersonType,
                     r => new object[] { r.Label, r.Count }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2 });

            AddSheet(excel, "Agent ตามจังหวัด", new[] { "จังหวัด", "จำนวน Agent" }, data.AgentByProvince,
                     r => new object[] { r.Label, r.Count }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2 });

            AddSheet(excel, "จังหวัดที่ลงทะเบียนขาย", new[] { "จังหวัด", "จำนวนทรัพย์", "ราคาประเมิน" }, data.AssetByProvince,
                     r => new object[] { r.Label, r.Count, r.Value }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2, 3 });

            AddSheet(excel, "รหัสทรัพย์ที่แจ้งซ้ำ", new[] { "รหัสทรัพย์", "จำนวน Agent", "มูลค่า" }, data.TopDuplicateAsset,
                     r => new object[] { r.Label, r.Count, r.Found ? (object)r.Value : "ไม่พบข้อมูลทรัพย์" }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2, 3 });

            AddSheet(excel, "แยกตามประเภททรัพย์", new[] { "ประเภททรัพย์", "จำนวน", "มูลค่า" }, data.AssetByType,
                     r => new object[] { r.Label, r.Count, r.Value }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2, 3 });

            // ค่าที่ Agent พิมพ์เป็นข้อความอิสระ (ไม่ตรงรหัสใน tb_product2) — แยก sheet ไม่นับรวมในสถิติทรัพย์
            AddSheet(excel, "ข้อความที่ Agent แจ้งเอง", new[] { "ข้อความที่แจ้ง", "จำนวน Agent" }, data.TopFreeText,
                     r => new object[] { r.Label, r.Count }, totalLabel: "รวมทั้งหมด", totalCols: new[] { 2 });

            var fileName  = $"agent_report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var fileBytes = excel.GetAsByteArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        //----- สร้าง sheet 1 ตาราง : หัวตารางสีเข้ม + แถวรวมท้ายตาราง
        private static void AddSheet(ExcelPackage excel, string sheetName, string[] headers,
                                     List<AgentReportRow> rows, Func<AgentReportRow, object[]> map,
                                     string totalLabel, int[] totalCols)
        {
            var ws = excel.Workbook.Worksheets.Add(sheetName);
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
            foreach (var r in rows)
            {
                var vals = map(r);
                for (int c = 0; c < vals.Length; c++)
                {
                    ws.Cells[row, c + 1].Value = vals[c];
                    if (vals[c] is long || vals[c] is decimal) ws.Cells[row, c + 1].Style.Numberformat.Format = "#,##0";
                }
                row++;
            }

            //----- แถวรวม (นับจากรายการเต็ม ไม่ใช่แค่ Top 10)
            if (rows.Count > 0)
            {
                ws.Cells[row, 1].Value = totalLabel;
                ws.Cells[row, 1].Style.Font.Bold = true;
                foreach (var c in totalCols)
                {
                    decimal sum = c == 2 ? rows.Sum(x => (decimal)x.Count) : rows.Sum(x => x.Value);
                    ws.Cells[row, c].Value = sum;
                    ws.Cells[row, c].Style.Font.Bold = true;
                    ws.Cells[row, c].Style.Numberformat.Format = "#,##0";
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
            }
        }
    }
}
