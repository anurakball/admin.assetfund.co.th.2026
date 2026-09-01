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
        protected AssetPlusImportController(IWebHostEnvironment e, IConfiguration c, IHttpContextAccessor x) : base(e, c, x) { }

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

        /// <summary>ค่าคงที่ของคอลัมน์ระบบ ตอน import (ระบบเดิมตั้ง status/pb_status/show_front = 1 คือเผยแพร่ทันที)</summary>
        protected Dictionary<string, object> ImportAuditFields()
        {
            long now = UnixNow();
            string user = CurrentUser();
            return new Dictionary<string, object>()
            {
                { "lastcreate", now }, { "lastupdate", now }, { "sort", 0 },
                { "status", 1 }, { "pb_status", 1 },
                { "last_user", user }, { "pb_last_user", user }, { "show_front", 1 },
            };
        }

        /// <summary>คอลัมน์ที่มีจริงในตาราง (ใช้กรองก่อนเขียน กัน SQL error เมื่อ XML มี element เกินมา)</summary>
        protected HashSet<string> TableColumns(string table)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var dt = _db.ExecuteQuery("select column_name from information_schema.columns where table_schema = 'dbo' and table_name = @t",
                                      new Dictionary<string, object>() { { "t", table } });
            foreach (System.Data.DataRow r in dt.Rows) set.Add(r["column_name"] + "");
            return set;
        }
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
        /// ระบบเดิม: ถ้ามีแถวเดิม (IndexName + ValueDateFormat) ให้ตั้ง Flag=0 ก่อน แล้ว insert แถวใหม่ Flag=1
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
        {
            detail = "";
            string valueDate = AssetPlusWsClient.Child(data, "ValueDate");
            if (string.IsNullOrEmpty(valueDate)) valueDate = navDate;
            string valueDateFormat = AssetPlusWsClient.ToDateKey(valueDate);
            string valueDateIn = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

            int n = 0;
            foreach (var indexGroup in AssetPlusWsClient.Children(data, "Index"))
            {
                foreach (XmlNode item in indexGroup.ChildNodes)
                {
                    if (item is not XmlElement) continue;
                    string indexName = AssetPlusWsClient.Child(item, "IndexName");
                    if (string.IsNullOrEmpty(indexName)) continue;

                    _db.ExecuteNonQuery("update [tb_home_other_indices] set Flag = 0 where IndexName = @n and ValueDateFormat = @d",
                        new Dictionary<string, object>() { { "n", indexName }, { "d", valueDateFormat } });

                    var f = ImportAuditFields();
                    f["title"] = indexName;
                    f["IndexName"] = indexName;
                    f["IndexValue"] = AssetPlusWsClient.Child(item, "IndexValue");
                    f["Change"] = AssetPlusWsClient.Child(item, "Change");
                    f["PercentChange"] = AssetPlusWsClient.Child(item, "PercentChange");
                    f["ValueDate"] = valueDate;
                    f["ValueDateFormat"] = valueDateFormat;
                    f["ValueDateIn"] = valueDateIn;
                    f["Flag"] = 1;
                    _db.Insert("tb_home_other_indices", f);
                    n++;
                }
            }
            detail = string.Format("(ValueDate {0})", valueDate);
            return n;
        }
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

        /// <summary>โครงสร้าง XML: &lt;ArrayOfNAV&gt;&lt;NAV&gt;&lt;FundCode/&gt;&lt;FundNameTH/&gt;…&lt;/NAV&gt;…</summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
        {
            detail = "";
            string navDateIn = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            int n = 0;

            //----- payload อาจเป็น <ArrayOfNAV> (มีลูก NAV) หรือเป็น <NAV> ตัวเดียว -----
            var items = AssetPlusWsClient.Children(data, "NAV");
            if (items.Count == 0 && string.Equals(data.LocalName, "NAV", StringComparison.OrdinalIgnoreCase)) items.Add(data);

            foreach (var nav in items)
            {
                string fundCode = AssetPlusWsClient.Child(nav, "FundCode");
                if (string.IsNullOrEmpty(fundCode)) continue;

                string navDateVal = AssetPlusWsClient.Child(nav, "NAVDate");
                string navDateFormat = AssetPlusWsClient.ToDateKey(navDateVal);

                _db.ExecuteNonQuery("update [tb_fund_nav] set Flag = 0 where FundCode = @c and NAVDateFormat = @d",
                    new Dictionary<string, object>() { { "c", fundCode }, { "d", navDateFormat } });

                var f = ImportAuditFields();
                f["title"] = fundCode;
                f["FundCode"] = fundCode;
                f["FundNameTH"] = AssetPlusWsClient.Child(nav, "FundNameTH");
                f["FundNameEN"] = AssetPlusWsClient.Child(nav, "FundNameEN");
                f["NAVDate"] = navDateVal;
                f["TotalNAV"] = AssetPlusWsClient.Child(nav, "TotalNAV");
                f["NAVPerUnit"] = AssetPlusWsClient.Child(nav, "NAVPerUnit");
                f["Offer"] = AssetPlusWsClient.Child(nav, "Offer");
                f["Bid"] = AssetPlusWsClient.Child(nav, "Bid");
                f["BahtChange"] = AssetPlusWsClient.Child(nav, "BahtChange");
                f["Change"] = AssetPlusWsClient.Child(nav, "Change");
                f["NAVDateIn"] = navDateIn;
                f["NAVDateFormat"] = navDateFormat;
                f["Flag"] = 1;
                _db.Insert("tb_fund_nav", f);
                n++;
            }
            return n;
        }
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
        /// ระบบเดิม: ตั้ง Flag=0 ให้แถวเดิมของวันเดียวกันทั้งหมด แล้ว insert ชุดใหม่ Flag=1
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
        {
            detail = "";
            string returnDate = AssetPlusWsClient.Child(data, "ReturnPerformanceDate");
            if (string.IsNullOrEmpty(returnDate)) returnDate = navDate;
            string navDateFormat = AssetPlusWsClient.ToDateKey(returnDate);
            string navDateIn = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

            _db.ExecuteNonQuery("update [tb_fund_performance] set Flag = 0 where NAVDateFormat = @d",
                new Dictionary<string, object>() { { "d", navDateFormat } });

            int n = 0;
            foreach (var group in AssetPlusWsClient.Children(data, "PastPerformance"))
            {
                foreach (var perf in AssetPlusWsClient.Children(group, "Performance"))
                {
                    string fundCode = AssetPlusWsClient.Child(perf, "FundCode");
                    if (string.IsNullOrEmpty(fundCode)) continue;

                    var f = ImportAuditFields();
                    f["title"] = fundCode;
                    f["ReturnPerformanceDate"] = returnDate;
                    f["FundCode"] = fundCode;
                    f["FundNameTH"] = AssetPlusWsClient.Child(perf, "FundNameTH");
                    f["FundNameEN"] = AssetPlusWsClient.Child(perf, "FundNameEN");
                    f["InceptionDateTH"] = AssetPlusWsClient.Child(perf, "InceptionDateTH");
                    f["InceptionDateEN"] = AssetPlusWsClient.Child(perf, "InceptionDateEN");
                    f["NAVPerUnit"] = AssetPlusWsClient.Child(perf, "NAVPerUnit");
                    f["ThreeMonth"] = AssetPlusWsClient.Child(perf, "ThreeMonth");
                    f["SixMonth"] = AssetPlusWsClient.Child(perf, "SixMonth");
                    f["OneYear"] = AssetPlusWsClient.Child(perf, "OneYear");
                    f["ThreeYear"] = AssetPlusWsClient.Child(perf, "ThreeYear");
                    f["YTD"] = AssetPlusWsClient.Child(perf, "YTD");
                    f["InceptionPort"] = AssetPlusWsClient.Child(perf, "InceptionPort");
                    f["InceptionBM"] = AssetPlusWsClient.Child(perf, "InceptionBM");
                    f["NAVDateIn"] = navDateIn;
                    f["NAVDateFormat"] = navDateFormat;
                    f["FundCodeMark"] = AssetPlusWsClient.Child(perf, "FundCodeMark");
                    f["Flag"] = 1;
                    _db.Insert("tb_fund_performance", f);
                    n++;
                }
            }
            detail = string.Format("(ReturnPerformanceDate {0})", returnDate);
            return n;
        }
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
        /// โครงสร้าง XML: &lt;ArrayOfFundFact&gt;&lt;FundFact&gt;&lt;FundCode/&gt;&lt;…อีกกว่า 250 element…&gt;
        ///
        /// ระบบเดิมเขียนทีละคอลัมน์ตามชื่อ element (element name = ชื่อคอลัมน์ตรงตัว)
        /// ที่นี่จึง map แบบ generic: เอาเฉพาะ element ที่ชื่อ "ตรงกับคอลัมน์จริง" ในตาราง
        /// ทำให้รองรับ element ใหม่ที่ web service เพิ่มมาโดยไม่ต้องแก้โค้ด และไม่พังถ้ามี element เกิน
        /// </summary>
        protected override int ImportData(XmlElement data, string navDate, out string detail)
        {
            detail = "";
            var columns = TableColumns("tb_fund_fundfact");
            //----- คอลัมน์ระบบ : ห้ามให้ XML เขียนทับ -----
            var reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "id", "lastcreate", "lastupdate", "sort", "status", "pb_status", "last_user", "pb_last_user", "show_front", "Flag", "title" };

            string navDateIn = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            int n = 0;

            var items = AssetPlusWsClient.Children(data, "FundFact");
            if (items.Count == 0 && string.Equals(data.LocalName, "FundFact", StringComparison.OrdinalIgnoreCase)) items.Add(data);

            foreach (var ff in items)
            {
                string fundCode = AssetPlusWsClient.Child(ff, "FundCode");
                if (string.IsNullOrEmpty(fundCode)) continue;

                //----- แถวเดิมของกองทุนนี้ → Flag = 0 (ระบบเดิมทำแบบเดียวกัน) -----
                _db.ExecuteNonQuery("update [tb_fund_fundfact] set Flag = 0 where fundcode = @c",
                    new Dictionary<string, object>() { { "c", fundCode } });

                var f = ImportAuditFields();
                f["title"] = fundCode;
                f["fundcode"] = fundCode;
                f["NAVDateIn"] = navDateIn;
                f["Flag"] = "1";

                foreach (XmlNode c in ff.ChildNodes)
                {
                    if (c is not XmlElement el) continue;
                    string col = el.LocalName;
                    if (reserved.Contains(col) || !columns.Contains(col)) continue;
                    if (string.Equals(col, "fundcode", StringComparison.OrdinalIgnoreCase)) continue;
                    f[col] = el.InnerText;
                }

                _db.Insert("tb_fund_fundfact", f);
                n++;
            }
            detail = string.Format("(fundDate {0})", navDate);
            return n;
        }
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
