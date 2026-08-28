using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{ 
    public class WidgetAjaxController : Controller
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly ISession _session;
        public readonly Utility _utility;
        public readonly DBHelper _db;
        public readonly AdminHelpers _admin;

        public Module? Module { set; get; } = null;

        public WidgetAjaxController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig);
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
        }

        public virtual IActionResult Config()
        {
            return Json(new { module_name = Module.Name, module_config = Module.Config, session = _session });
        }

        //----- ชี้ path สัมพัทธ์ของไฟล์ front-end (/media, /uploads, /images) ไปที่โดเมน front-end
        //      ใช้กับการแสดงตัวอย่างใน page builder เท่านั้น
        private static readonly string[] FrontAssetFolders = new[] { "media", "uploads", "images" };
        private string RewriteFrontAssetUrl(string html)
        {
            if (string.IsNullOrEmpty(html)) { return html; }

            string frontBase = (_utility.frontURL() + "").TrimEnd('/');
            if (frontBase == "") { return html; }

            foreach (string folder in FrontAssetFolders)
            {
                //----- จับเฉพาะที่อยู่ใน attribute (src="/media/..", href='/media/..') และใน url(/media/..) ของ CSS
                html = Regex.Replace(
                    html,
                    "(?<prefix>(?:src|href|srcset|data-hover-img)\\s*=\\s*[\"']|url\\(\\s*[\"']?)/" + folder + "/",
                    "${prefix}" + frontBase + "/" + folder + "/",
                    RegexOptions.IgnoreCase);
            }

            return html;
        }
        
        //----- แทนที่ token |||pb_xxx||| ด้วยข้อมูลจริงของโมดูล CMS ที่ widget ผูกไว้ (คอลัมน์ mod_name)
        //      รองรับ 2 รูปแบบ
        //        1) โมดูลระเบียนเดียว (web_core_single) -> แทน token ตรง ๆ
        //        2) โมดูลการ์ดซ้ำ (web_core_item)      -> ใช้บล็อกวนซ้ำ |||REPEAT|||...|||/REPEAT|||
        //      ถ้าไม่พบโมดูล/ไม่มีข้อมูล จะคืน HTML เดิมโดยไม่แตะ token (กันตัวอย่างพัง)
        //----- อ่านข้อมูลของโมดูล CMS หนึ่งตัว (กรอง web_id ปัจจุบัน + module_id ตาม config)
        private DataTable? LoadModuleData(string modName)
        {
            var mod = _admin.GetModule(modName);
            if (mod == null || string.IsNullOrEmpty(mod.Config.Table)) { return null; }

            string sql = string.Format("select * from {0} where web_id = @web_id ", Db.T(mod.Config.Table));
            var para = new Dictionary<string, object>() { { "web_id", _admin._currentWebID } };
            int moduleId = mod.Config.TableModuleID ?? 0;
            if (moduleId > 0) { sql += " and module_id = @module_id "; para.Add("module_id", moduleId); }
            sql += " order by sort asc ";

            return _db.ExecuteQuery(sql, para);
        }

        private string ReplaceTokenByModule(string html, string modName)
        {
            if (string.IsNullOrEmpty(html)) { return html; }

            //----- 1) บล็อกวนซ้ำที่ระบุโมดูลเอง  |||REPEAT:ModuleName||| ... |||/REPEAT|||
            html = RepeatNamedRegex.Replace(html, m =>
            {
                var dtx = LoadModuleData(m.Groups["mod"].Value.Trim());
                return dtx == null ? "" : RenderRows(m.Groups["body"].Value, dtx);
            });

            //----- 2) token ที่ระบุโมดูลเอง  |||ModuleName:pb_col|||  (ใช้ค่าจากแถวแรก)
            html = QualifiedTokenRegex.Replace(html, m =>
            {
                var dtx = LoadModuleData(m.Groups["mod"].Value.Trim());
                if (dtx == null || dtx.Rows.Count == 0) { return m.Value; }
                string col = m.Groups["col"].Value.Trim();
                return dtx.Rows[0].Table.Columns.Contains(col) ? (dtx.Rows[0][col] + "") : m.Value;
            });

            var dt = LoadModuleData(modName);
            if (dt == null || dt.Rows.Count == 0) { return html; }

            //----- 3) บล็อกวนซ้ำของโมดูลที่ widget ผูกไว้ : ทำก่อน เพื่อไม่ให้ token ข้างในโดนแทนด้วยแถวแรก
            html = RepeatBlockRegex.Replace(html, m => RenderRows(m.Groups["body"].Value, dt));

            //----- 4) token นอกบล็อกวนซ้ำใช้ค่าจากแถวแรกเสมอ (โมดูลระเบียนเดียวมีแถวเดียวอยู่แล้ว)
            return ReplaceRowTokens(html, dt.Rows[0]);
        }

        private static readonly Regex RepeatBlockRegex =
            new Regex(@"\|\|\|REPEAT\|\|\|(?<body>[\s\S]*?)\|\|\|/REPEAT\|\|\|", RegexOptions.IgnoreCase);

        private static readonly Regex RepeatNamedRegex =
            new Regex(@"\|\|\|REPEAT:(?<mod>[A-Za-z0-9_]+)\|\|\|(?<body>[\s\S]*?)\|\|\|/REPEAT\|\|\|", RegexOptions.IgnoreCase);

        private static readonly Regex QualifiedTokenRegex =
            new Regex(@"\|\|\|(?<mod>[A-Za-z0-9_]+):(?<col>[A-Za-z0-9_]+)\|\|\|");

        private string RenderRows(string body, DataTable dt)
        {
            var sb = new System.Text.StringBuilder();
            foreach (DataRow r in dt.Rows)
            {
                //----- ข้ามแถวที่ปิดการแสดงผล ให้ตัวอย่างตรงกับหน้าเว็บจริง
                if (r.Table.Columns.Contains("show_front") && (r["show_front"] + "") == "0") { continue; }
                sb.Append(ReplaceRowTokens(body, r));
            }
            return sb.ToString();
        }

        private string ReplaceRowTokens(string html, DataRow row)
        {
            foreach (DataColumn col in row.Table.Columns)
            {
                string token = "|||" + col.ColumnName + "|||";
                if (html.Contains(token))
                {
                    html = html.Replace(token, row[col] + "");
                }
            }
            return html;
        }

        [AdminLogin]
        public virtual IActionResult Index(int id, string box_data = "", string box_data_sub = "")
        {
            //----- web_id = 0 อ่านจาก web_widget, web_id != 0 (microsite) อ่านจาก web_widget2 (เลือกชุดโดย WidgetTable())
            //      พาเลตต์เป็นค่าคงที่ชุดเดียว ไม่กรอง web_id (ตารางเหล่านี้ไม่ใช้ web_id เป็นเงื่อนไข)
            var This_Widget = _db.ExecuteQuery(
                string.Format("select * from {0} where id = @id", Db.T(_admin.WidgetTable())),
                new Dictionary<string, object>() { { "id", id } });
            if (This_Widget.Rows.Count == 0)
            {
                return Content("");
            }
            var this_resp = This_Widget.Rows[0]["pb_info"]+"";

            string q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '1' AND web_id = '"+ _admin._currentWebID + "'";

            //----- ตาราง map id -> module_id ด้านล่างเป็นของชุด widget เดิมเท่านั้น
            //      widget2 ใช้เลข id คนละชุด (ชนกัน) จึงข้าม ใช้ module_id = 1 เป็นค่าตั้งต้น
            if(_admin.UseWidget2())
            {
                //----- ข้าม mapping ของชุดเดิม
            }
            else if(id == 3 || id == 7 || id == 19)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '2' AND web_id = '"+ _admin._currentWebID + "'";
            }
            else if(id == 4 || id == 9 || id == 20)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '3' AND web_id = '"+ _admin._currentWebID + "'";
            }
            else if(id == 10 || id == 11 || id == 21)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '4' AND web_id = '"+ _admin._currentWebID + "'";
            }
            else if(id == 12 || id == 8 || id == 22)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '5' AND web_id = '"+ _admin._currentWebID + "'";
            }
            else if(id == 13 || id == 15 || id == 23)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '6' AND web_id = '"+ _admin._currentWebID + "'";
            }
            else if(id == 14 || id == 16 || id == 24)
            {
                q = "SELECT * FROM [2026_web_core_single] WHERE module_id = '7' AND web_id = '"+ _admin._currentWebID + "'";
            }

            //----- widget2 ผูกกับโมดูล CMS ผ่านคอลัมน์ mod_name (ชื่อโมดูลใน AdminMenu.AllModule())
            //      แทนการ hardcode id -> module_id แบบชุดเดิม ทำให้ทุก microsite ใช้ config เดียวกันได้
            string modName = (This_Widget.Rows[0].Table.Columns.Contains("pb_mod_name") ? This_Widget.Rows[0]["pb_mod_name"] + "" : "").Trim();
            if (modName != "")
            {
                this_resp = ReplaceTokenByModule(this_resp, modName);
            }
            else
            {
                var This_Data = _db.ExecuteQuery(q);

                string all_field = "pb_title,pb_en_title";
                for(int i=1;i<=50;i++)
                {
                    all_field = all_field + ",pb_t" + i + ",pb_en_t" + i + "";
                }

                string[] arr = all_field.Split(',');
                //----- ไม่มีระเบียนใน web_core_single ก็ปล่อย HTML ตามเดิม (กัน index out of range)
                if(This_Data.Rows.Count > 0)
                {
                    foreach(string item in arr)
                    {
                        if(this_resp.Contains("|||"+item+"|||") && This_Data.Rows[0].Table.Columns.Contains(item))
                        {
                            this_resp = this_resp.Replace("|||"+item+"|||", This_Data.Rows[0][item] + "");
                        }
                    }
                }
            }

            //----- รูป/ไฟล์ในเนื้อหาเขียนเป็น path สัมพัทธ์ (/media/..., /uploads/...) ซึ่งอยู่ฝั่ง front-end
            //      ถ้าไม่แปลง จะ 404 บนโดเมน admin ทำให้ตัวอย่างไม่เหมือนหน้าจริง
            //      แปลงเฉพาะตอนแสดงตัวอย่างเท่านั้น ไม่แตะข้อมูลที่บันทึกไว้
            this_resp = RewriteFrontAssetUrl(this_resp);

            ViewBag.Resp = this_resp;
           
            return View("~/Areas/Admin/Views/WidgetAjax/Index.cshtml");
        }

        [AdminLogin]
        public virtual IActionResult Setting(int id,string box_data, string box_data_sub) //ไม่ได้ใช้ เนื่องจาก แต่ละ Widget ผูกกับข้อมูลเลย ไม่ได้ผูก cat_id เพื่อให้มีหน้าจอในการเลือก 
        { 
            ViewBag._utility = _utility;
            ViewBag._admin = _admin;
            ViewBag._session = _session;
            ViewBag._db = _db;
            ViewBag.Module = Module; 
            ViewBag.CoreSystem = "";
            ViewBag.CatID = "";
            ViewBag.Setting = "";

            //เมื่อมีการกด ไอคอนเฟือง จะเข้ามาหน้าเลือก Cat Data ข้อมูลว่าเอาอันไหน จัดเรียงแบบไหน
            //ส่วนตรงนี้จะกำหนดค่า Default เฉพาะ Dropdown Core System เท่านั้น
            if (id == 1)      { ViewBag.CoreSystem = "1";   ViewBag.CatID = 1; }
            else if (id == 2) { ViewBag.CoreSystem = "1";   ViewBag.CatID = 1; }
              
            ViewBag.id = id;
            ViewBag.Setting = box_data;

            return View("~/Areas/Admin/Views/WidgetAjax/Setting.cshtml");
        }

        [AdminLogin]
        public virtual IActionResult Manage(int id,string box_data) //ในแต่ละ Widget กดรูปดินสอ เพื่อเปิด Modal และแสดงหน้า Manage ในแต่ละระบบ
        {
            ViewBag.Resp = "";

            string url_core = "";
            string url_cat_id = "";
            
            //กดปุ่มรูปดินสอ เพื่อแสดงหน้า Manage Data หลังบ้าน กำหนดตามชื่อ Module Name ที่หลังบ้าน
            if (id == 1)      { url_core = "HomeImageSlide";    url_cat_id = "1"; }
            else if (id == 2) { url_core = "HomeSamText";       url_cat_id = "1"; }
            else if (id == 3) { url_core = "HomeSamText2";      url_cat_id = "2"; }
            else if (id == 4) { url_core = "HomeSamText3";      url_cat_id = "3"; }
            else if (id == 11){ url_core = "HomeSamText4";      url_cat_id = "4"; }
            else if (id == 12){ url_core = "HomeSamText5";      url_cat_id = "5"; }
            else if (id == 13){ url_core = "HomeSamText6";      url_cat_id = "6"; }
            else if (id == 14){ url_core = "HomeSamText7";      url_cat_id = "7"; }
            else if (id == 5) { url_core = "HomeImageSlide";    url_cat_id = "1"; }
            else if (id == 6) { url_core = "HomeSamText";       url_cat_id = "1"; }
            else if (id == 7) { url_core = "HomeSamText2";      url_cat_id = "2"; }
            else if (id == 9) { url_core = "HomeSamText3";      url_cat_id = "3"; }
            else if (id == 10){ url_core = "HomeSamText4";      url_cat_id = "4"; }
            else if (id == 8) { url_core = "HomeSamText5";      url_cat_id = "5"; }
            else if (id == 15){ url_core = "HomeSamText6";      url_cat_id = "6"; }
            else if (id == 16){ url_core = "HomeSamText7";      url_cat_id = "7"; }
            else if (id == 17){ url_core = "HomeImageSlide";    url_cat_id = "1"; }
            else if (id == 18){ url_core = "HomeSamText";       url_cat_id = "1"; }
            else if (id == 19){ url_core = "HomeSamText2";      url_cat_id = "2"; }
            else if (id == 20){ url_core = "HomeSamText3";      url_cat_id = "3"; }
            else if (id == 21){ url_core = "HomeSamText4";      url_cat_id = "4"; }
            else if (id == 22){ url_core = "HomeSamText5";      url_cat_id = "5"; }
            else if (id == 23){ url_core = "HomeSamText6";      url_cat_id = "6"; }
            else if (id == 24){ url_core = "HomeSamText7";      url_cat_id = "7"; } 
            else if (id == 25){ url_core = "News";              url_cat_id = "1"; }
            else if (id == 26){ url_core = "News";              url_cat_id = "1"; }
            else if (id == 27){ url_core = "News";              url_cat_id = "1"; }

            /*
            if (box_data != null && box_data != "")
            {
                if (box_data.IndexOf(',') > -1)
                { 
                    string[] arr_box_data = box_data.Split(",");
                    for (var i = 0; i < arr_box_data.Length; i++)
                    {
                        if (arr_box_data[i] != "")
                        {
                            if (arr_box_data[i].IndexOf(':') > -1)
                            {
                                string[] arr_widget = arr_box_data[i].Split(":");

                                //ทำการเรียกข้อมูล Box Data ทั้งหมด มา Loop หาว่า Widget นี้ ที่ได้กดปุ่มดินสอ Manage ข้อมูล เป็นระบบ Core อะไร

                                if (arr_widget[0] == id.ToString())
                                {
                                    string[] arr_value = arr_widget[1].Split("|");
                                    string table_data = arr_value[0];

                                    if (table_data == "1") {      url_core = "Announce"; }
                                    else if (table_data == "2") { url_core = "Banner"; }
                                    else if (table_data == "3") { url_core = "BranchLocation"; }
                                    else if (table_data == "4") { url_core = "Download"; }
                                    else if (table_data == "5") { url_core = "EForm"; }
                                    else if (table_data == "6") { url_core = "Faq"; }
                                    else if (table_data == "7") { url_core = "Gallery"; }
                                    else if (table_data == "8") { url_core = "HomeImageSlide"; }
                                    else if (table_data == "9") { url_core = "News"; }
                                    else if (table_data == "10") { url_core = "Product"; }
                                    else if (table_data == "11") { url_core = "Promotion"; }
                                    else if (table_data == "12") { url_core = "VDO"; }
                                    else if (table_data == "13") { url_core = "HomeTextCenter"; }
                                    else if (table_data == "14") { url_core = "Board"; }
                                    else if (table_data == "15") { url_core = "Relate"; }
                                    else if (table_data == "16") { url_core = "TextEditor"; }

                                    url_cat_id = arr_value[1];
                                }
                            }
                        }
                    }
                }
            }
            */

            //เมื่อกด Manage จะเช็คว่า เปิดหน้า List Data แต่ละ Cat หรือ เปิดหน้า Edit ข้อมูล
            if (id == 2 || id == 3 || id == 4 || id == 11 || id == 12 || id == 13 || id == 14 || id == 6 || id == 7 || id == 10 || id == 15 || id == 16 || id == 8 || id == 9 || id == 18 || id == 19 || id == 21 || id == 20 || id == 22 || id == 23 || id == 24)
            {
                ViewBag.Resp = "/Admin/" + url_core + "/Edit/" + url_cat_id; 
            }
            else
            {
                ViewBag.Resp = "/Admin/" + url_core + "?text=";
            }
             
            return View("~/Areas/Admin/Views/WidgetAjax/Manage.cshtml");
        }
    }
}
