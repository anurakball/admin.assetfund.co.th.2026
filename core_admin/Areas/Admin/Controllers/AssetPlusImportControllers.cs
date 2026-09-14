using System.Xml;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    /// <summary>
    /// ฐานของเมนู "Get ..." ที่พอร์ตมาจากหลังบ้านเดิม
    /// ระบบเดิมทุกหน้าทำงานเหมือนกัน: เลือกวันที่ → เรียก web service → แปลง XML → เขียนลงตารางเดิม
    /// ระบบใหม่เพิ่มทางเลือก "อัปโหลดไฟล์ XML" ไว้ใช้ตอน web service เข้าไม่ถึง (โครงสร้างไฟล์เดียวกัน)
    /// </summary>
    public abstract class AssetPlusImportController : AdminLegacyController
    {
        /// <summary>
        /// เครื่องยนต์นำเข้าตัวเดียวกับที่ scheduler ใช้ (<see cref="AssetPlusImporter"/>)
        /// — กดเองจากหลังบ้าน กับให้ ws_schedule เรียก จึงได้ผลลัพธ์เหมือนกันเป๊ะ
        /// </summary>
        protected readonly AssetPlusImporter _importer;

        protected AssetPlusImportController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            _importer = new AssetPlusImporter(_db);
        }

        /// <summary>ชื่อ operation ของ web service</summary>
        protected abstract string WsOperation { get; }
        /// <summary>ชื่อพารามิเตอร์วันที่ของ operation (null = ไม่มีพารามิเตอร์)</summary>
        protected abstract string? WsDateParam { get; }

        [ModuleCheck("add")]
        public override IActionResult Create()
        {
            Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/ดึงข้อมูล";
            var ws = new AssetPlusWsClient(_config);
            ViewBag._utility = _utility;
            ViewBag._admin = _admin;
            ViewBag._session = _session;
            ViewBag._db = _db;
            ViewBag.Module = Module;
            ViewBag.Title = Module.Config.Text;
            ViewBag.ModuleName = Module.Name;
            ViewBag.WsUrl = ws.Url;
            ViewBag.WsOperation = WsOperation;
            return View("~/Areas/Admin/Views/" + Module.Config.UseViewCreateFrom + "/Create.cshtml");
        }

        [HttpPost]
        [ModuleCheck("add")]
        [RequestSizeLimit(52428800)]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                string navDate = (collection["nav_date"] + "").Trim();       // dd/MM/yyyy
                if (WsDateParam != null && string.IsNullOrEmpty(navDate))
                {
                    TempData["alert_message"] = "กรุณาระบุวันที่";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Create");
                }

                XmlElement? data = null;
                string error = "";
                string source = "";

                //----- 1) ถ้าอัปโหลดไฟล์ XML มา ให้ใช้ไฟล์ก่อน (ทางเลือกสำรองเมื่อ web service เข้าไม่ถึง) -----
                var file = collection.Files.GetFile("xml_file");
                if (file != null && file.Length > 0)
                {
                    using var sr = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8, true);
                    string xmlText = sr.ReadToEnd();
                    data = AssetPlusWsClient.FromXmlText(xmlText, WsOperation + "Result", out error);
                    source = "ไฟล์ " + file.FileName;
                }
                else
                {
                    //----- 2) เรียก web service ตามระบบเดิม -----
                    var ws = new AssetPlusWsClient(_config);
                    data = ws.Call(WsOperation, WsDateParam, navDate.Replace("-", "/"), out error);
                    source = "web service (" + ws.Url + ")";
                }

                if (data == null)
                {
                    TempData["alert_message"] = string.Format("ดึงข้อมูลไม่สำเร็จจาก {0} : {1}<br/>สามารถอัปโหลดไฟล์ XML แทนได้", source, error);
                    TempData["alert_class"] = "alert-danger";
                    return RedirectToAction("Create");
                }

                int affected = ImportData(data, navDate, out string detail);

                TempData["alert_message"] = string.Format("นำเข้าข้อมูลจาก {0} แล้ว {1:n0} รายการ{2}", source, affected, string.IsNullOrEmpty(detail) ? "" : " " + detail);
                TempData["alert_class"] = affected > 0 ? "alert-success" : "alert-warning";

                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "add",
                    action_info: string.Format("นำเข้าข้อมูล : {0} ({1} รายการ, วันที่ {2})", Module.Config.TextBreadcrumb, affected, navDate),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: Module.Config.Table,
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text);

                return RedirectToAction("Index");
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

        /// <summary>แปลง XML → เขียนลงตารางเดิม (แต่ละเมนู implement เอง) คืนจำนวนแถวที่นำเข้า</summary>
        protected abstract int ImportData(XmlElement data, string navDate, out string detail);

    }

    // =====================================================================================
    //  หน้าหลัก / Get Other Indices   (ระบบเดิม mod_tb_home_other_indices)
    // =====================================================================================
    public class ApOtherIndicesController : AssetPlusImportController
    {
        public ApOtherIndicesController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApOtherIndices");
        }

        protected override string WsOperation => "MartketOtherIndices";
        protected override string? WsDateParam => "date";

        /// <summary>
        /// โครงสร้าง XML: &lt;OtherIndices&gt;&lt;ValueDate&gt;dd/MM/yyyy&lt;/ValueDate&gt;&lt;Index&gt;&lt;…&gt;&lt;IndexName/&gt;&lt;IndexValue/&gt;&lt;Change/&gt;&lt;PercentChange/&gt;…
        /// (ตรรกะจริงอยู่ที่ <see cref="AssetPlusImporter.ImportOtherIndices"/> — ใช้ร่วมกับ ws_schedule)
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
            => _importer.ImportOtherIndices(data, navDate, CurrentUser(), out detail);
    }

    // =====================================================================================
    //  ข้อมูลกองทุน / Get NAV   (ระบบเดิม mod_tb_fund_nav)
    // =====================================================================================
    public class ApFundNavController : AssetPlusImportController
    {
        public ApFundNavController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundNav");
        }

        protected override string WsOperation => "NAVAnnounce";
        protected override string? WsDateParam => null;   // ระบบเดิมเรียก NAVAnnounce() ไม่ส่งพารามิเตอร์

        /// <summary>
        /// โครงสร้าง XML: &lt;ArrayOfNAV&gt;&lt;NAV&gt;&lt;FundCode/&gt;&lt;FundNameTH/&gt;…&lt;/NAV&gt;…
        /// (ตรรกะจริงอยู่ที่ <see cref="AssetPlusImporter.ImportNav"/> — ใช้ร่วมกับ ws_schedule)
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
            => _importer.ImportNav(data, CurrentUser(), out detail);
    }

    // =====================================================================================
    //  ข้อมูลกองทุน / Get Performance   (ระบบเดิม mod_tb_fund_performance)
    // =====================================================================================
    public class ApFundPerformanceController : AssetPlusImportController
    {
        public ApFundPerformanceController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundPerformance");
        }

        protected override string WsOperation => "FundReturnPerformance";
        protected override string? WsDateParam => "date";

        /// <summary>
        /// โครงสร้าง XML: &lt;ReturnPerformance&gt;&lt;ReturnPerformanceDate/&gt;…&lt;PastPerformance&gt;&lt;Performance&gt;&lt;FundCode/&gt;…
        /// (ตรรกะจริงอยู่ที่ <see cref="AssetPlusImporter.ImportPerformance"/> — ใช้ร่วมกับ ws_schedule
        ///  รวมถึงการคำนวณ FundCodeMark และอัปเดตหัวตาราง tb_fund_performance_hd)
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
            => _importer.ImportPerformance(data, navDate, CurrentUser(), out detail);
    }

    // =====================================================================================
    //  ข้อมูลกองทุน / Get Fund Fact Sheet   (ระบบเดิม mod_tb_fund_fundfact)
    // =====================================================================================
    public class ApFundFactSheetController : AssetPlusImportController
    {
        public ApFundFactSheetController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundFactSheet");
        }

        protected override string WsOperation => "FundFactSheet";
        protected override string? WsDateParam => "fundDate";

        /// <summary>
        /// โครงสร้าง XML: &lt;ArrayOfFundFact&gt;&lt;FundFact&gt;&lt;FundCode/&gt;&lt;…element เดี่ยวอีกกว่า 250 ตัว…&gt;
        ///   + ชุดรายการอีก 11 ชุด (NAVHistory, FundPerformanceData, …) ที่แตกลงตารางลูก tb_fund_fundfact_*
        /// (ตรรกะจริงอยู่ที่ <see cref="AssetPlusImporter.ImportFundFact"/> — ใช้ร่วมกับ ws_schedule)
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
            => _importer.ImportFundFact(data, CurrentUser(), out detail);
    }

    // =====================================================================================
    //  ข้อมูลกองทุน / Delete NAV   (ระบบเดิม mod_tb_fund_nav_del)
    //  เลือกวันที่ + กองทุน แล้วลบแถวใน tb_fund_nav ที่ตรงเงื่อนไข
    // =====================================================================================
    public class ApFundNavDeleteController : AdminLegacyController
    {
        public ApFundNavDeleteController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x)
        {
            Module = _admin.GetModule("ApFundNavDelete");
        }

        public override IActionResult Index()
        {
            try
            {
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;

                //----- รายชื่อกองทุนทั้งหมดที่มี NAV อยู่ (ระบบเดิม: select distinct FundCode from tb_fund_nav order by FundCode) -----
                ViewBag.FundCodes = _db.ExecuteQuery("select distinct FundCode from [tb_fund_nav] where FundCode is not null and FundCode <> '' order by FundCode asc");

                int _access_id = _session.GetInt32("admin_access_id") ?? 0;
                Module.Config.CanDelete = _admin.checkAccess(Module, _access_id, "delete");
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
            return View("~/Areas/Admin/Views/ApFundNavDelete/Index.cshtml");
        }

        /// <summary>ลบ NAV ตามวันที่ + กองทุนที่เลือก (ตรงกับ mod_tb_fund_nav_del/export.aspx)</summary>
        [HttpPost]
        [ModuleCheck("delete")]
        public IActionResult DeleteNav(IFormCollection f)
        {
            try
            {
                string navDate = (f["nav_date"] + "").Trim();                  // dd/MM/yyyy
                string dateKey = AssetPlusWsClient.ToDateKey(navDate);
                var funds = (f["Fund"] + "").Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim()).Where(s => s.Length > 0).ToList();

                if (string.IsNullOrEmpty(dateKey))
                {
                    TempData["alert_message"] = "กรุณาระบุวันที่ NAV ให้ถูกต้อง (วว/ดด/ปปปป)";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                if (funds.Count == 0)
                {
                    TempData["alert_message"] = "กรุณาเลือกกองทุนอย่างน้อย 1 รายการ";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                int total = 0;
                foreach (string fund in funds)
                {
                    total += _db.ExecuteNonQuery("delete from [tb_fund_nav] where NAVDateFormat = @d and FundCode = @c",
                        new Dictionary<string, object>() { { "d", dateKey }, { "c", fund } });
                }

                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "delete",
                    action_info: string.Format("ลบ NAV : {0} (วันที่ {1}, {2} กองทุน, {3} แถว)", Module.Config.TextBreadcrumb, navDate, funds.Count, total),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: Module.Config.Table,
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text);

                TempData["alert_message"] = string.Format("ลบข้อมูล NAV วันที่ {0} แล้ว {1:n0} รายการ ({2} กองทุน)", navDate, total, funds.Count);
                TempData["alert_class"] = total > 0 ? "alert-success" : "alert-warning";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }
    }
}
