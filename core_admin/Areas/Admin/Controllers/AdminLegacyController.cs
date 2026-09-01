using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    /// <summary>
    /// เครื่องยนต์กลางของ "เมนูที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus"
    /// (ASP WebForms ที่ <c>localhost:8099/assetplus/backoffice</c>)
    ///
    /// ทำงานบน **ตารางเดิม** (<c>tb_*</c> ใน database เดียวกัน — ไม่มี prefix <c>2026_</c> และห้ามแก้โครงสร้าง)
    /// จึงแยกจาก <see cref="AdminCoreController"/> เพราะสคีมาต่างกัน:
    ///
    /// | ระบบใหม่ (2026_web_*)              | ระบบเดิม (tb_*)                          |
    /// |------------------------------------|------------------------------------------|
    /// | web_id (แยก microsite)             | ไม่มี                                     |
    /// | created_at / updated_at (datetime) | lastcreate / lastupdate (unix seconds)   |
    /// | created_by / updated_by            | last_user                                |
    /// | approve_by                         | pb_last_user                             |
    /// | id = IDENTITY ทุกตาราง             | บางตารางไม่ใช่ IDENTITY (ต้องคำนวณ MAX(id)+1) |
    ///
    /// ส่วนที่ "เหมือนกัน" และใช้ซ้ำได้เลย: sort / status / pb_status / show_front / คู่คอลัมน์ pb_*
    /// และตรรกะ Approve (<c>pb_&lt;field&gt; = &lt;field&gt;, pb_status=1, show_front=1</c>) ตรงกันทั้งสองระบบ
    ///
    /// UI ทั้งหมดใช้ของระบบใหม่ (view / ปุ่ม / ค้นหา / แบ่งหน้า / สิทธิ์ / audit log)
    /// </summary>
    [AdminLogin]
    [ModuleCheck]
    [XssValidate]
    public class AdminLegacyController : AdminCoreController
    {
        public AdminLegacyController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
            : base(hostingEnvironment, iConfig, iContext) { }

        // ======================================================================
        //  helper
        // ======================================================================

        /// <summary>unix seconds ปัจจุบัน — ตรงกับ <c>class1.date_to_unix()</c> ของระบบเดิม</summary>
        protected static long UnixNow() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        /// <summary>ชื่อผู้ใช้ที่บันทึกลงคอลัมน์ <c>last_user</c> / <c>pb_last_user</c> (ระบบเดิมเก็บ username)</summary>
        protected string CurrentUser() => _session.GetString("admin_user") ?? "";

        /// <summary>ตารางเดิมบางตัว id ไม่ใช่ IDENTITY — ระบบเดิมใช้ MAX(id)+1 (include/new_id.aspx)</summary>
        protected int NextId()
        {
            var dt = _db.ExecuteQuery(string.Format("select coalesce(max(id),0) + 1 as next_id from {0}", Db.T(Module.Config.Table)));
            return (dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0]["next_id"]) : 1;
        }

        /// <summary>sort ถัดไป = MAX(sort)+10 (ถ้ามีกลุ่มก็นับเฉพาะในกลุ่ม) — ตรงกับ include/new_sort.aspx</summary>
        protected int NextSort(string cateValue = "")
        {
            string sql = string.Format("select coalesce(max(sort),0) + 10 as next_sort from {0}", Db.T(Module.Config.Table));
            var p = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Module.Config.TableCateField) && !string.IsNullOrEmpty(cateValue))
            {
                sql += string.Format(" where cast({0} as nvarchar(max)) = cast(@cate as nvarchar(max))", Module.Config.TableCateField);
                p.Add("cate", cateValue);
            }
            var dt = _db.ExecuteQuery(sql, p);
            return (dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0]["next_sort"]) : 10;
        }

        /// <summary>เรียงลำดับ sort ใหม่เป็น 10,20,30… (ตรงกับ class1.refresh_record_sort ของระบบเดิม)</summary>
        protected void LegacyReSort()
        {
            string sql = string.Format("select id from {0}", Db.T(Module.Config.Table));
            var p = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
            {
                string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                sql += string.Format(" where cast({0} as nvarchar(max)) = cast(@cate as nvarchar(max))", Module.Config.TableCateField);
                p.Add("cate", cate_val);
            }
            sql += " order by sort asc, id asc";

            var rows = _db.ExecuteQuery(sql, p);
            int sort = 10;
            foreach (System.Data.DataRow r in rows.Rows)
            {
                _db.Update(Module.Config.Table, " where id = @id ",
                    new Dictionary<string, object>() { { "sort", sort } },
                    new Dictionary<string, object>() { { "id", Convert.ToInt32(r["id"]) } });
                sort += 10;
            }
        }

        /// <summary>
        /// เขียนคิว "รออนุมัติ" ลง <c>tb_admin_approve</c> ให้เหมือนระบบเดิม (include/insert_approve.aspx)
        /// คอลัมน์: title, this_mod, this_id, last_user, lastcreate, lastupdate, status, pb_status, cat_id
        /// </summary>
        protected void ApproveQueueUpsert(int id, string title)
        {
            if (Module.Config.LegacyApproveQueue != true) return;
            try
            {
                ApproveQueueDelete(id);
                long now = UnixNow();
                string catId = "";
                if (!string.IsNullOrEmpty(Module.Config.TableCateField))
                {
                    catId = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                }
                //----- คอลัมน์จริงของ tb_admin_approve : title, this_mod, this_id, this_user, lastcreate, lastupdate, sort, status, this_cat_id
                //      (id เป็น identity) — ลำดับเดียวกับ INSERT ... VALUES ของระบบเดิม (include/insert_approve.aspx)
                _db.ExecuteNonQuery(
                    "insert into [tb_admin_approve] (title, this_mod, this_id, this_user, lastcreate, lastupdate, sort, status, this_cat_id) " +
                    "values (@title, @this_mod, @this_id, @this_user, @lastcreate, @lastupdate, 1, 1, @this_cat_id)",
                    new Dictionary<string, object>()
                    {
                        { "title", string.Format("{0} ( {1} )", Module.Config.TextBreadcrumb, title) },
                        { "this_mod", Module.Config.Table },
                        { "this_id", id.ToString() },
                        { "this_user", CurrentUser() },
                        { "lastcreate", now },
                        { "lastupdate", now },
                        { "this_cat_id", catId },
                    });
            }
            catch (Exception e)
            {
                // คิวอนุมัติของระบบเดิมเป็นข้อมูลเสริม — ถ้าเขียนไม่ได้ต้องไม่ทำให้การบันทึกหลักล้ม
                _utility.writeLogs("ApproveQueueUpsert (" + Module.Config.Table + ") - " + e.Message);
            }
        }

        protected void ApproveQueueDelete(int id)
        {
            if (Module.Config.LegacyApproveQueue != true) return;
            try
            {
                _db.ExecuteNonQuery("delete from [tb_admin_approve] where this_mod = @this_mod and this_id = @this_id",
                    new Dictionary<string, object>() { { "this_mod", Module.Config.Table }, { "this_id", id.ToString() } });
            }
            catch (Exception e)
            {
                _utility.writeLogs("ApproveQueueDelete (" + Module.Config.Table + ") - " + e.Message);
            }
        }

        /// <summary>ฟิลด์นี้เป็นช่องอัปโหลดไฟล์ของระบบเดิมหรือไม่ (img1 / en_img1 / file1 …)</summary>
        protected static bool IsLegacyFileField(string field)
        {
            string f = (field ?? "").ToLowerInvariant();
            return f.Contains("img") || f.StartsWith("file");
        }

        /// <summary>
        /// โฟลเดอร์ปลายทางของไฟล์อัปโหลด — ต้องเป็นโฟลเดอร์ upload ของ "เว็บระบบเดิม"
        /// เพราะ front-end เดิมอ่านไฟล์จากที่นั่นด้วยชื่อไฟล์เปล่า ๆ ที่เก็บไว้ในคอลัมน์
        /// ตั้งค่าที่ appsettings → LegacyUpload:Path (ไม่ตั้ง = เก็บลง wwwroot/Files/legacy_upload ของระบบใหม่)
        /// </summary>
        protected string LegacyUploadPath()
        {
            string p = _config["LegacyUpload:Path"] ?? "";
            if (string.IsNullOrWhiteSpace(p))
            {
                p = Path.Combine(_hostingEnvironment.WebRootPath, "Files", "legacy_upload");
            }
            return p;
        }

        /// <summary>URL สำหรับแสดงรูป/ไฟล์ที่อัปโหลด (ต้องลงท้ายด้วย /)</summary>
        protected string LegacyUploadUrl()
        {
            string u = _config["LegacyUpload:Url"] ?? "";
            if (string.IsNullOrWhiteSpace(u)) u = _utility.rootURL() + "/Files/legacy_upload/";
            if (!u.EndsWith("/")) u += "/";
            return u;
        }

        /// <summary>
        /// บันทึกไฟล์ที่อัปโหลด แล้วคืน "ชื่อไฟล์เปล่า" ตามรูปแบบเดิมของระบบเก่า
        /// (include/image_copy_add.aspx) : &lt;table&gt;_&lt;rand 0-999&gt;_&lt;unix&gt;_&lt;field&gt;.&lt;ext&gt;
        /// </summary>
        protected string SaveLegacyUpload(IFormFile file, string field)
        {
            string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
            var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "jpg","jpeg","png","gif","bmp","webp","pdf","csv","xls","xlsx","doc","docx","ppt","pptx" };
            if (!allow.Contains(ext)) throw new Exception("ไฟล์ผิดประเภท (รองรับ: " + string.Join(", ", allow) + ")");

            string dir = LegacyUploadPath();
            Directory.CreateDirectory(dir);

            string name = string.Format("{0}_{1}_{2}_{3}.{4}",
                Module.Config.Table, new Random().Next(0, 999), UnixNow(), field, ext);

            using (var fs = new FileStream(Path.Combine(dir, name), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return name;
        }

        /// <summary>ค่าที่จะเขียนลงตาราง — เอาเฉพาะฟิลด์ที่ประกาศไว้ใน FieldCreate / FieldUpdate เท่านั้น</summary>
        protected Dictionary<string, object> LegacyFields(IFormCollection f, bool isCreate)
        {
            var fields = new Dictionary<string, object>();
            var declared = isCreate ? Module.Config.FieldCreate : Module.Config.FieldUpdate;
            long now = UnixNow();

            if (declared != null)
            {
                foreach (string field in declared)
                {
                    if (field == "lastcreate" || field == "lastupdate")
                    {
                        fields[field] = now;
                    }
                    else if (field == "last_user")
                    {
                        fields[field] = CurrentUser();
                    }
                    else if (field == "pb_last_user")
                    {
                        fields[field] = "";
                    }
                    else if (field == "sort")
                    {
                        string cate = (!string.IsNullOrEmpty(Module.Config.TableCateField) && f.ContainsKey(Module.Config.TableCateField))
                            ? f[Module.Config.TableCateField].ToString() : "";
                        fields[field] = NextSort(cate);
                    }
                    else if (field == "status")
                    {
                        fields[field] = 1;
                    }
                    else if (field == "pb_status")
                    {
                        fields[field] = 0;      // แก้ไขแล้วต้องรออนุมัติใหม่เสมอ (ตรงกับระบบเดิม)
                    }
                    else if (field == "show_front")
                    {
                        fields[field] = 0;
                    }
                    else if (field.EndsWith("date") && f.ContainsKey(field))
                    {
                        // คอลัมน์ชนิด date ของระบบเดิม (เช่น holidaydate) — ฟอร์มส่งมาเป็น dd/MM/yyyy
                        string v = (f[field] + "").Trim();
                        //----- culture ของแอปคือ th-TH (ปีพุทธศักราช) — ใช้ culture ปัจจุบันให้ตรงกับที่หน้าเว็บแสดง
                        //      เช่น "31/12/2569" (พ.ศ.) ต้องถูกบันทึกเป็น 2026-12-31 ในคอลัมน์ date
                        if (string.IsNullOrEmpty(v)) { fields[field] = DBNull.Value; }
                        else if (DateTime.TryParseExact(v, "dd/MM/yyyy", null,
                                     System.Globalization.DateTimeStyles.None, out var d)) { fields[field] = d.Date; }
                        else if (DateTime.TryParse(v, out var d2)) { fields[field] = d2.Date; }
                        else { fields[field] = DBNull.Value; }
                    }
                    else if (IsLegacyFileField(field))
                    {
                        //----- ช่องอัปโหลดไฟล์ : มีไฟล์ใหม่ = เก็บไฟล์ใหม่, ไม่มี = ใช้ค่าเดิมจาก hidden &lt;field&gt;_old -----
                        var up = f.Files.GetFile(field);
                        if (up != null && up.Length > 0)
                        {
                            fields[field] = SaveLegacyUpload(up, field);
                        }
                        else if (f.ContainsKey(field + "_old"))
                        {
                            fields[field] = (f[field + "_old"] + "").Trim();
                        }
                        else if (isCreate)
                        {
                            fields[field] = "";
                        }
                    }
                    else if (f.ContainsKey(field))
                    {
                        fields[field] = (f[field] + "").Trim();
                    }
                }
            }

            //----- ฟิลด์ระบบที่ต้องมีเสมอ (เผื่อไม่ได้ประกาศไว้)
            if (isCreate)
            {
                if (!fields.ContainsKey("lastcreate")) fields["lastcreate"] = now;
                if (!fields.ContainsKey("status")) fields["status"] = 1;
                if (!fields.ContainsKey("pb_status")) fields["pb_status"] = 0;
                if (!fields.ContainsKey("show_front")) fields["show_front"] = 0;
                if (!fields.ContainsKey("pb_last_user")) fields["pb_last_user"] = "";
            }
            if (!fields.ContainsKey("lastupdate")) fields["lastupdate"] = now;
            if (!fields.ContainsKey("last_user")) fields["last_user"] = CurrentUser();
            if (!isCreate) fields["pb_status"] = 0;

            return fields;
        }

        /// <summary>ตรวจค่าซ้ำตาม <c>UniqueFields</c> (ระบบเดิมเช็ค datatype ของ tb_calendar_category)</summary>
        protected string? CheckUnique(IFormCollection f, int excludeId = 0)
        {
            if (Module.Config.UniqueFields == null) return null;
            foreach (string field in Module.Config.UniqueFields)
            {
                if (!f.ContainsKey(field)) continue;
                string val = (f[field] + "").Trim();
                if (string.IsNullOrEmpty(val)) continue;

                var dt = _db.ExecuteQuery(
                    string.Format("select top 1 id from {0} where cast({1} as nvarchar(max)) = cast(@v as nvarchar(max)) and id <> @id", Db.T(Module.Config.Table), field),
                    new Dictionary<string, object>() { { "v", val }, { "id", excludeId } });
                if (dt.Rows.Count > 0)
                {
                    return string.Format("\"{0}\" นี้มีอยู่ในระบบแล้ว ไม่สามารถใช้ซ้ำกันได้", val);
                }
            }
            return null;
        }

        // ======================================================================
        //  Index
        // ======================================================================
        public override IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);

            try
            {
                var sqlParam = new Dictionary<string, object>() { { "id", 0 } };

                string sqlFrom = string.Format(" from {0} ", Db.T(Module.Config.Table));
                string sqlWhere = " where id > @id ";
                string sqlDateSearch = "";
                string sqlFieldSearch = "";

                string sqlOrder = "";
                if (Module.Config.Sort.ToLower() == "asc" || Module.Config.Sort.ToLower() == "desc")
                {
                    string tie = System.Text.RegularExpressions.Regex.IsMatch(Module.Config.OrderBy ?? "", @"(^|[\s,])id($|[\s,])",
                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase) ? "" : ", id " + Module.Config.Sort;
                    sqlOrder = string.Format(" order by {0} {1}{2} ", Module.Config.OrderBy, Module.Config.Sort, tie);
                }

                #region ----- Date Search (lastcreate เป็น unix seconds) -----
                if (Module.Config.EnableDateSearch == true)
                {
                    string after = _session.GetString("admin_" + Module.Name + "_after") ?? "";
                    string before = _session.GetString("admin_" + Module.Name + "_before") ?? "";
                    if (!string.IsNullOrEmpty(after) && DateTime.TryParse(after, out var dAfter))
                    {
                        sqlDateSearch += " and lastcreate >= @after ";
                        sqlParam.Add("after", new DateTimeOffset(DateTime.SpecifyKind(dAfter.Date, DateTimeKind.Utc)).ToUnixTimeSeconds());
                    }
                    if (!string.IsNullOrEmpty(before) && DateTime.TryParse(before, out var dBefore))
                    {
                        sqlDateSearch += " and lastcreate <= @before ";
                        sqlParam.Add("before", new DateTimeOffset(DateTime.SpecifyKind(dBefore.Date.AddDays(1).AddSeconds(-1), DateTimeKind.Utc)).ToUnixTimeSeconds());
                    }
                }
                #endregion

                #region ----- Field Search -----
                if (Module.Config.FieldSearch != null && Module.Config.FieldSearch.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> fs in Module.Config.FieldSearch)
                    {
                        string val = _session.GetString("admin_" + Module.Name + "_search_" + fs.Key) ?? "";
                        if (string.IsNullOrEmpty(val) || fs.Value.Count == 0) continue;

                        bool isEqual = Module.Config.FieldSearchIsEqual != null && Module.Config.FieldSearchIsEqual.Contains(fs.Key);
                        var cond = new List<string>();
                        foreach (string fld in fs.Value)
                        {
                            cond.Add(isEqual
                                ? string.Format(" LOWER(cast({0} as nvarchar(max))) = LOWER(cast(@search_{1} as nvarchar(max))) ", fld, fs.Key)
                                : string.Format(" LOWER(cast({0} as nvarchar(max))) like LOWER(@search_{1}) ", fld, fs.Key));
                        }
                        sqlParam.TryAdd("search_" + fs.Key, isEqual ? val : "%" + val + "%");
                        sqlFieldSearch += " and ( " + string.Join(" or ", cond) + " ) ";
                    }
                }
                var searchInputVal = _admin.getSearchInputValue(Module);
                #endregion

                string sql = string.Format(" select * {0} {1} {2} {3} {4}", sqlFrom, sqlWhere, sqlDateSearch, sqlFieldSearch, sqlOrder);
                string sqlQuery = _utility.Encrypt(JsonConvert.SerializeObject(new { sql = sql, parameter = sqlParam }), _utility.appKey());

                #region ----- Pagination -----
                string currentURL = _utility.rootURL() + Request.Path.ToString() + Request.QueryString.ToString();
                var totalRecordDT = _db.ExecuteQuery(string.Format("select count(id) as total_rows {0} {1} {2} {3}", sqlFrom, sqlWhere, sqlDateSearch, sqlFieldSearch), sqlParam);
                int totalRecord = (totalRecordDT.Rows.Count > 0) ? Convert.ToInt32(totalRecordDT.Rows[0]["total_rows"]) : 0;
                var pager = new Pager(totalItems: totalRecord, currentPage: Module.Config.Page, pageSize: Module.Config.PerPage, 4);
                string pageHtml = (totalRecord > Module.Config.PerPage) ? pager.CreateHtml(currentURL, "link") : "";
                if (Module.Config.Page > pager.TotalPages) { Module.Config.Page = pager.TotalPages; }
                #endregion

                if (string.IsNullOrWhiteSpace(sqlOrder)) { sql += " order by (select null) "; }
                sql += " offset @start rows fetch next @perpage rows only ";
                sqlParam.Add("start", ((Module.Config.Page - 1) * Module.Config.PerPage) < 0 ? 0 : (Module.Config.Page - 1) * Module.Config.PerPage);
                sqlParam.Add("perpage", Module.Config.PerPage);

                var listData = _db.ExecuteQuery(sql, sqlParam);

                #region ----- Access Module -----
                int _access_id = _session.GetInt32("admin_access_id") ?? 0;
                Module.Config.CanAdd = _admin.checkAccess(Module, _access_id, "add");
                Module.Config.CanEdit = _admin.checkAccess(Module, _access_id, "edit");
                Module.Config.CanDelete = _admin.checkAccess(Module, _access_id, "delete");
                Module.Config.CanMove = _admin.checkAccess(Module, _access_id, "move");
                Module.Config.CanStatus = _admin.checkAccess(Module, _access_id, "status");
                Module.Config.CanExport = _admin.checkAccess(Module, _access_id, "export");
                Module.Config.CanApprove = _admin.checkAccess(Module, _access_id, "approve");
                ViewBag.showMoveTools = (totalRecord > 0) ? _admin.showMoveTools(Module) : false;
                #endregion

                #region ----- ViewBag -----
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.listData = listData.Rows.Count > 0 ? listData.Rows : null;
                ViewBag.sqlQuery = sqlQuery;
                ViewBag.totalRecord = totalRecord;
                ViewBag.pageHtml = pageHtml;
                ViewBag.currentURL = currentURL;
                ViewBag.startRows = (Module.Config.Page - 1) * Module.Config.PerPage + 1;
                ViewBag.after = (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_after"))) ? _utility.dateFormat(_session.GetString("admin_" + Module.Name + "_after")) : "";
                ViewBag.before = (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_before"))) ? _utility.dateFormat(_session.GetString("admin_" + Module.Name + "_before")) : "";
                ViewBag.searchInputVal = searchInputVal;
                #endregion
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }

            return View("~/Areas/Admin/Views/" + Module.Config.UseViewListFrom + "/Index.cshtml");
        }

        // ======================================================================
        //  Create
        // ======================================================================
        [ModuleCheck("add")]
        public override IActionResult Create()
        {
            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/เพิ่ม";
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.LegacyUploadUrl = LegacyUploadUrl();
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
            return View("~/Areas/Admin/Views/" + Module.Config.UseViewCreateFrom + "/Create.cshtml");
        }

        [HttpPost]
        [ModuleCheck("add")]
        public override IActionResult Create(IFormCollection collection)
        {
            try
            {
                #region ----- จำกลุ่มที่เลือกไว้ (ใช้ตอน redirect กลับหน้า list) -----
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                string? dup = CheckUnique(collection);
                if (dup != null)
                {
                    TempData["alert_message"] = dup;
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Create");
                }

                var fields = LegacyFields(collection, isCreate: true);

                int newId = 0;
                if (Module.Config.LegacyIdManual == true)
                {
                    newId = NextId();
                    fields["id"] = newId;
                }

                var affected = _db.Insert(Module.Config.Table, fields);

                if (affected != 0)
                {
                    if (newId == 0)
                    {
                        var last = _db.ExecuteQuery(string.Format("select coalesce(max(id),0) as last_id from {0}", Db.T(Module.Config.Table)));
                        if (last.Rows.Count > 0) newId = Convert.ToInt32(last.Rows[0]["last_id"]);
                    }

                    ApproveQueueUpsert(newId, collection.ContainsKey("title") ? collection["title"] + "" : "");

                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "add",
                        action_info: string.Format("เพิ่มข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, newId),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(fields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text);

                    TempData["alert_message"] = string.Format("เพิ่มข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }

                TempData["alert_message"] = "ไม่สามารถเพิ่มข้อมูลได้";
                TempData["alert_class"] = "alert-warning";
                return RedirectToAction("Create");
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

        // ======================================================================
        //  Edit
        // ======================================================================
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id)
        {
            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/แก้ไข";

                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id", Db.T(Module.Config.Table)),
                                                new Dictionary<string, object>() { { "id", id } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.itemEdit = itemEdit.Rows[0];
                ViewBag.itemJSONEdit = _db.DataTableToScriptJSON(itemEdit);
                ViewBag.LegacyUploadUrl = LegacyUploadUrl();
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }
            return View("~/Areas/Admin/Views/" + Module.Config.UseViewEditFrom + "/Edit.cshtml");
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id", Db.T(Module.Config.Table)),
                                                new Dictionary<string, object>() { { "id", id } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                string? dup = CheckUnique(collection, id);
                if (dup != null)
                {
                    TempData["alert_message"] = dup;
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Edit", new { id = id });
                }

                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }

                var fields = LegacyFields(collection, isCreate: false);

                //----- hook ให้เมนูลูกทำงานเพิ่มก่อนบันทึก (เช่น tb_calendar_category ต้องอัปเดต datatype ใน tb_calendar ตาม) -----
                BeforeLegacyUpdate(id, itemEdit.Rows[0], collection, fields);

                var affected = _db.Update(Module.Config.Table, " where id = @id ", fields, new Dictionary<string, object>() { { "id", id } });

                if (affected != 0)
                {
                    AfterLegacyUpdate(id, itemEdit.Rows[0], collection, fields);
                    ApproveQueueUpsert(id, collection.ContainsKey("title") ? collection["title"] + "" : "");

                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "edit",
                        action_info: string.Format("แก้ไขข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: _db.DataTableToJSONWithJSONNet(itemEdit).TrimStart('[').TrimEnd(']'),
                        new_value: JsonConvert.SerializeObject(fields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text);

                    TempData["alert_message"] = string.Format("แก้ไขข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }

                TempData["alert_message"] = "ไม่สามารถแก้ไขข้อมูลได้";
                TempData["alert_class"] = "alert-warning";
                return RedirectToAction("Edit", new { id = id });
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

        /// <summary>hook ก่อน UPDATE — ให้เมนูลูก override ได้</summary>
        protected virtual void BeforeLegacyUpdate(int id, System.Data.DataRow oldRow, IFormCollection f, Dictionary<string, object> fields) { }
        /// <summary>hook หลัง UPDATE — ให้เมนูลูก override ได้</summary>
        protected virtual void AfterLegacyUpdate(int id, System.Data.DataRow oldRow, IFormCollection f, Dictionary<string, object> fields) { }

        // ======================================================================
        //  Delete
        // ======================================================================
        [HttpPost]
        [ModuleCheck("delete")]
        public override IActionResult Delete(IFormCollection f)
        {
            try
            {
                if (!f.ContainsKey("id") || string.IsNullOrEmpty(f["id"]))
                {
                    TempData["alert_message"] = "กรุณาระบุรายการที่ต้องการลบ";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                int rowDel = 0;
                int rowBlocked = 0;
                foreach (string sid in f["id"].ToString().Split(','))
                {
                    if (!_utility.isInt(sid)) continue;
                    int id = Convert.ToInt32(sid);

                    var itemDelete = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id", Db.T(Module.Config.Table)),
                                                      new Dictionary<string, object>() { { "id", id } });
                    if (itemDelete.Rows.Count == 0) continue;

                    //----- กันลบกลุ่มที่ยังมีลูกอยู่ (แนวคิดเดียวกับ table_sub ของระบบเดิม) -----
                    //      เทียบค่าใน "คอลัมน์ที่ผูกกัน" ไม่ใช่ id เสมอไป
                    //      เช่น tb_calendar ผูกกับหมวดหมู่ด้วยคอลัมน์ datatype (ไม่ใช่ cat_id)
                    if (!string.IsNullOrEmpty(LegacyChildTable))
                    {
                        object parentVal = itemDelete.Rows[0].Table.Columns.Contains(LegacyParentField)
                            ? itemDelete.Rows[0][LegacyParentField] : id;
                        if (parentVal == null || parentVal == DBNull.Value || (parentVal + "").Trim() == "")
                        {
                            parentVal = id;
                        }
                        var child = _db.ExecuteQuery(string.Format("select count(id) as c from {0} where cast({1} as nvarchar(max)) = cast(@pv as nvarchar(max))", Db.T(LegacyChildTable), LegacyChildField),
                                                     new Dictionary<string, object>() { { "pv", parentVal } });
                        if (child.Rows.Count > 0 && Convert.ToInt32(child.Rows[0]["c"]) > 0) { rowBlocked++; continue; }
                    }

                    var affected = _db.ExecuteNonQuery(string.Format("delete from {0} where id = @id", Db.T(Module.Config.Table)),
                                                       new Dictionary<string, object>() { { "id", id } });
                    rowDel += affected;

                    if (affected > 0)
                    {
                        ApproveQueueDelete(id);
                        if (Module.Config.CanMove == true) LegacyReSort();

                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "delete",
                            action_info: string.Format("ลบข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: Module.Config.Table,
                            old_value: _db.DataTableToJSONWithJSONNet(itemDelete).TrimStart('[').TrimEnd(']'),
                            mod_name: Module.Name,
                            mod_name_txt: Module.Config.Text);
                    }
                }

                TempData["alert_message"] = string.Format("ลบข้อมูลแล้ว {0:n0} รายการ", rowDel)
                                          + (rowBlocked > 0 ? string.Format(" (ข้าม {0:n0} รายการ เพราะยังมีข้อมูลย่อยอยู่)", rowBlocked) : "");
                TempData["alert_class"] = rowBlocked > 0 ? "alert-warning" : "alert-success";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }

        /// <summary>ตารางลูกที่อ้างถึงเมนูนี้ (กันลบกลุ่มที่ยังถูกใช้งาน) — ตั้งค่าที่ controller ของเมนู</summary>
        protected string LegacyChildTable { get; set; } = "";
        /// <summary>คอลัมน์ฝั่ง "ลูก" ที่เก็บค่าอ้างถึงแถวนี้</summary>
        protected string LegacyChildField { get; set; } = "cat_id";
        /// <summary>คอลัมน์ฝั่ง "แม่" ที่ถูกอ้างถึง (ปกติคือ id แต่ tb_calendar ผูกด้วย datatype)</summary>
        protected string LegacyParentField { get; set; } = "id";

        // ======================================================================
        //  Status
        // ======================================================================
        [HttpPost]
        [ModuleCheck("status")]
        public override IActionResult Status(IFormCollection f)
        {
            try
            {
                if (!f.ContainsKey("status") || string.IsNullOrEmpty(f["status"]) || !_utility.isInt(f["status"].ToString()))
                {
                    TempData["alert_message"] = "สถานะ เปิด-ปิด ไม่ถูกต้อง";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
                string status_row = f["status"].ToString();
                string status_text = status_row == "1" ? "เปิด" : "ปิด";

                if (!f.ContainsKey("id") || string.IsNullOrEmpty(f["id"]))
                {
                    TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการ {0}", status_text);
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                int rowAffected = 0;
                foreach (string sid in f["id"].ToString().Split(','))
                {
                    if (!_utility.isInt(sid)) continue;
                    int id = Convert.ToInt32(sid);

                    var affected = _db.Update(Module.Config.Table, " where id = @id ",
                        new Dictionary<string, object>() { { "status", Convert.ToInt32(status_row) }, { "lastupdate", UnixNow() }, { "last_user", CurrentUser() } },
                        new Dictionary<string, object>() { { "id", id } });
                    rowAffected += affected;

                    if (affected > 0)
                    {
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "status",
                            action_info: string.Format("{2}การแสดงผล : {0} ({1})", Module.Config.TextBreadcrumb, id, status_text),
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: Module.Config.Table,
                            mod_name: Module.Name,
                            mod_name_txt: Module.Config.Text);
                    }
                }

                TempData["alert_message"] = status_text + string.Format("การแสดงผลแล้ว {0:n0} รายการ", rowAffected);
                TempData["alert_class"] = "alert-success";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }

        // ======================================================================
        //  Approve  (pb_<field> = <field>, pb_status = 1, show_front = 1)
        // ======================================================================
        [HttpPost]
        [ModuleCheck("approve")]
        public override IActionResult Approve(int id, IFormCollection f)
        {
            try
            {
                var item = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id", Db.T(Module.Config.Table)),
                                            new Dictionary<string, object>() { { "id", id } });
                if (item.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการอนุมัติ";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                if (!f.ContainsKey("approve") || (f["approve"].ToString() != "1" && f["approve"].ToString() != "0"))
                {
                    TempData["alert_message"] = "สถานะ อนุมัติ ไม่ถูกต้อง";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
                string approve_status = f["approve"].ToString();

                if (Module.Config.FieldApprove == null || Module.Config.FieldApprove.Count == 0)
                {
                    TempData["alert_message"] = "FieldApprove is not set";
                    TempData["alert_class"] = "alert-danger";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                var set = new List<string>();
                set.Add(" pb_last_user = @pb_last_user ");
                foreach (string field in Module.Config.FieldApprove)
                {
                    set.Add(approve_status == "1"
                        ? string.Format(" pb_{0} = {0} ", field)     // อนุมัติ : เอาค่าที่แก้ไปเป็นค่าเผยแพร่
                        : string.Format(" {0} = pb_{0} ", field));   // ไม่อนุมัติ : ย้อนกลับเป็นค่าที่เคยเผยแพร่
                }
                set.Add(" pb_status = 1 ");
                if (approve_status == "1") set.Add(" show_front = 1 ");

                string sql = string.Format("update {0} set {1} where id = @id", Db.T(Module.Config.Table), string.Join(",", set));
                var affected = _db.ExecuteNonQuery(sql, new Dictionary<string, object>() { { "id", id }, { "pb_last_user", CurrentUser() } });

                if (affected != 0)
                {
                    ApproveQueueDelete(id);
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: approve_status == "1" ? "approve" : "unapprove",
                        action_info: string.Format("{2} : {0} ({1})", Module.Config.TextBreadcrumb, id, approve_status == "1" ? "อนุมัติ" : "ไม่อนุมัติ"),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text);

                    TempData["alert_message"] = approve_status == "1"
                        ? string.Format("อนุมัติข้อมูลแล้ว {0:n0} รายการ", affected)
                        : string.Format("ยกเลิกแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }

                TempData["alert_message"] = "ไม่สามารถอนุมัติข้อมูลได้";
                TempData["alert_class"] = "alert-warning";
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

        // ======================================================================
        //  Move  (sort ±15 แล้วเรียงใหม่เป็น 10,20,30…)
        // ======================================================================
        [HttpPost]
        [ModuleCheck("move")]
        public override IActionResult Move(IFormCollection f)
        {
            Module = _admin.setSessionRequest(Module, Request);

            if (Module.Config.OrderBy.ToLower() != "sort" || Module.Config.Sort.ToLower() != "asc")
            {
                TempData["alert_message"] = "ไม่สามารถย้ายตำแหน่งได้ เนื่องจากการจัดเรียงไม่ถูกต้อง";
                TempData["alert_class"] = "alert-warning";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
            if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
            {
                string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                if (cate_val == "")
                {
                    TempData["alert_message"] = "ไม่สามารถย้ายตำแหน่งได้ เนื่องจากยังไม่ได้เลือกกลุ่ม";
                    TempData["alert_class"] = "alert-warning";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }

            try
            {
                if (!f.ContainsKey("move") || (f["move"].ToString().ToLower() != "up" && f["move"].ToString().ToLower() != "down"))
                {
                    TempData["alert_message"] = "สถานะ ย้าย ไม่ถูกต้อง";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
                string move_row = f["move"].ToString().ToLower();

                int move_step = 1;
                if (f.ContainsKey("step") && _utility.isInt(f["step"].ToString()))
                {
                    move_step = Convert.ToInt32(f["step"]);
                    if (move_step <= 0)
                    {
                        TempData["alert_message"] = "จำนวน Step ไม่ถูกต้อง";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                }
                move_step = (move_step * 10) - 10;

                if (!f.ContainsKey("id") || string.IsNullOrEmpty(f["id"]))
                {
                    TempData["alert_message"] = "กรุณาระบุรายการที่ต้องการย้าย";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                int rowAffected = 0;
                foreach (string sid in f["id"].ToString().Split(','))
                {
                    if (!_utility.isInt(sid)) continue;
                    int id = Convert.ToInt32(sid);

                    var affected = _db.ExecuteNonQuery(
                        string.Format("update {0} set sort = (sort{1}15){1}{2} where id = @id", Db.T(Module.Config.Table), move_row == "up" ? "-" : "+", move_step),
                        new Dictionary<string, object>() { { "id", id } });
                    rowAffected += affected;
                    LegacyReSort();

                    if (affected > 0)
                    {
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "move",
                            action_info: string.Format("เลื่อน{2} : {0} ({1})", Module.Config.TextBreadcrumb, id, move_row == "up" ? "ขึ้น" : "ลง"),
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: Module.Config.Table,
                            mod_name: Module.Name,
                            mod_name_txt: Module.Config.Text);
                    }
                }

                TempData["alert_message"] = string.Format("ย้ายตำแหน่งแล้ว {0:n0} รายการ", rowAffected);
                TempData["alert_class"] = "alert-success";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }
    }
}
