using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;
using static System.Net.WebRequestMethods;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    [AdminLogin]
    [ModuleCheck]
    [XssValidate]
    public class AdminCoreController : Controller
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IHttpContextAccessor _context;
        public readonly ISession _session;
        public readonly Utility _utility;
        public readonly DBHelper _db;
        public readonly AdminHelpers _admin;
        public int _currentWebID = -1;

        public Module? Module { set; get; } = null;

        public AdminCoreController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig);
            _admin = new AdminHelpers(hostingEnvironment, _config, _context);
            _currentWebID = _admin._currentWebID;
        }

        public virtual IActionResult Config()
        {
            return Json(new { module_name = Module.Name, module_config = Module.Config, session = _session });
        }

        public virtual IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);
            Module = _admin.setBreadcrumbCMSPage(Module);

            try
            {
                #region ----- List Data -----

                var sqlParam = new Dictionary<string, object>() { { "id", 0 }, { "web_id", _currentWebID } };

                string sqlSelect = string.Format(" select * ");
                string sqlFrom = string.Format(" from {0} ", Db.T(Module.Config.Table));
                string sqlWhere = string.Format(" where id > @id and web_id = @web_id ");
                string sqlDateSearch = "";
                string sqlFieldSearch = ""; 
                // ORDER BY: เติม tiebreaker ", id <dir>" อัตโนมัติเมื่อการเรียงยังไม่อ้าง id
                //   - ให้ลำดับ deterministic และตรงกับ convention ของ front-end ที่ใช้ "... , id ASC" (เช่น LoadFaqItems, web_core_item, web_footer_link)
                //   - กันเคส sort ซ้ำ (tie) แล้ว Postgres คืนลำดับสุ่มจนไม่ตรง front-end
                //   - ไม่แตะ Config.OrderBy จึงไม่กระทบเงื่อนไขปุ่มจัดเรียง (Move ตรวจ OrderBy == "sort")
                string sqlOrder = "";
                if (Module.Config.Sort.ToLower() == "asc" || Module.Config.Sort.ToLower() == "desc")
                {
                    string _orderTie = System.Text.RegularExpressions.Regex.IsMatch(Module.Config.OrderBy ?? "", @"(^|[\s,])id($|[\s,])", System.Text.RegularExpressions.RegexOptions.IgnoreCase) ? "" : ", id " + Module.Config.Sort;
                    sqlOrder = string.Format(" order by {0} {1}{2} ", Module.Config.OrderBy, Module.Config.Sort, _orderTie);
                }

                if (Module.Config.TableModuleID != null && Module.Config.TableModuleID > 0)
                {
                    sqlWhere = sqlWhere + string.Format(" and module_id = '" + Module.Config.TableModuleID.ToString() + "' ");
                }

                #region ----- Date Search -----
                if (Module.Config.EnableDateSearch == true)
                {
                    if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_after")))
                    {
                        sqlDateSearch += " and created_at >= (CAST(@after AS datetime2)) ";
                        sqlParam.Add("after", _session.GetString("admin_" + Module.Name + "_after") + " 00:00:00");
                    }
                    if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_before")))
                    {
                        sqlDateSearch += " and created_at <= (CAST(@before AS datetime2)) ";
                        sqlParam.Add("before", _session.GetString("admin_" + Module.Name + "_before") + " 23:59:59");
                    }
                }
                #endregion

                #region ----- Field Search -----
                if (Module.Config.FieldSearch != null && Module.Config.FieldSearch.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> FieldSearch in Module.Config.FieldSearch)
                    {
                        if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key)) && FieldSearch.Value.Count > 0)
                        {
                            List<string> arCondition = new List<string>();
                            foreach (string Fields in FieldSearch.Value)
                            {
                                if (Module.Config.FieldSearchIsEqual != null && Module.Config.FieldSearchIsEqual.Where(c => c == FieldSearch.Key).Count() == 1)
                                {
                                    arCondition.Add(string.Format(" LOWER(cast({0} as nvarchar(max))) = LOWER(cast({1} as nvarchar(max))) ", Fields, "@search_" + FieldSearch.Key));
                                }
                                else
                                {
                                    arCondition.Add(string.Format(" LOWER(cast({0} as nvarchar(max))) like LOWER({1}) ", Fields, "@search_" + FieldSearch.Key));
                                }
                            }

                            if (Module.Config.FieldSearchIsEqual != null && Module.Config.FieldSearchIsEqual.Where(c => c == FieldSearch.Key).Count() == 1)
                            {
                                sqlParam.TryAdd("search_" + FieldSearch.Key, _session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key).ToString());
                            }
                            else
                            {
                                sqlParam.TryAdd("search_" + FieldSearch.Key, "%" + _session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key).ToString() + "%");
                            }

                            sqlFieldSearch += " and ( " + string.Join(" or ", arCondition) + " ) ";

                        }
                    }
                }

                var searchInputVal = _admin.getSearchInputValue(Module);
                #endregion

                string sql = string.Format("{0} {1} {2} {3} {4} {5}", sqlSelect, sqlFrom, sqlWhere, sqlDateSearch, sqlFieldSearch, sqlOrder);

                #region ----- Encrypt SQL Query (for export) -----
                string sqlQuery = _utility.Encrypt(JsonConvert.SerializeObject(new { sql = sql, parameter = sqlParam }), _utility.appKey());
                #endregion

                #region ----- Pagination -----
                string currentURL = _utility.rootURL() + Request.Path.ToString() + Request.QueryString.ToString();
                //return Json(new { currentURL, requestPath = Request.Path.ToString(), requestQueryString = Request.QueryString.ToString() });
                string sqlPage = string.Format("select count(id) as total_rows {0} {1} {2} {3}", sqlFrom, sqlWhere, sqlDateSearch, sqlFieldSearch);
                if (Module.Name == "BranchMain")
                {
                    sqlPage = string.Format("select count(*) as total_rows FROM web_branch_translation WHERE id > 0 " + sqlFieldSearch + "");
                }
                var paramPage = sqlParam;
                var totalRecordDT = _db.ExecuteQuery(sqlPage, paramPage);
                int totalRecord = (totalRecordDT.Rows.Count > 0) ? Convert.ToInt32(totalRecordDT.Rows[0]["total_rows"]) : 0;
                var pager = new Pager(totalItems: totalRecord, currentPage: Module.Config.Page, pageSize: Module.Config.PerPage, 4);
                string pageHtml = (totalRecord > Module.Config.PerPage) ? pager.CreateHtml(currentURL, "link") : "";

                if (Module.Config.Page > pager.TotalPages)
                {
                    Module.Config.Page = pager.TotalPages;
                }
                #endregion

                // T-SQL แบ่งหน้าด้วย OFFSET/FETCH ซึ่งบังคับว่าคำสั่งต้องมี ORDER BY
                // โมดูลที่ไม่ได้ตั้งค่าการเรียงจะได้ sqlOrder ว่าง จึงต้องเติม ORDER BY กลาง ๆ ให้
                if (string.IsNullOrWhiteSpace(sqlOrder)) { sql += " order by (select null) "; }
                sql += " offset @start rows fetch next @perpage rows only ";
                sqlParam.Add("start", ((Module.Config.Page - 1) * Module.Config.PerPage) < 0 ? 0 : (Module.Config.Page - 1) * Module.Config.PerPage);
                sqlParam.Add("perpage", Module.Config.PerPage);

                var listData = _db.ExecuteQuery(sql, sqlParam);

                #endregion

                #region ----- Access Module -----
                int _access_id = _session.GetInt32("admin_access_id") ?? 0;
                Module.Config.CanAdd = _admin.checkAccess(Module, _access_id, "add");
                Module.Config.CanEdit = _admin.checkAccess(Module, _access_id, "edit");
                Module.Config.CanDelete = _admin.checkAccess(Module, _access_id, "delete");
                Module.Config.CanMove = _admin.checkAccess(Module, _access_id, "move");
                Module.Config.CanStatus = _admin.checkAccess(Module, _access_id, "status");
                Module.Config.CanExport = _admin.checkAccess(Module, _access_id, "export");
                Module.Config.CanApprove = _admin.checkAccess(Module, _access_id, "approve");

                #region ----- Move condition -----
                ViewBag.showMoveTools = (totalRecord > 0) ? _admin.showMoveTools(Module) : false;
                #endregion

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
                //return Json(Module.Config);
                #endregion

                if (Module.Name == "CMSPage" || Module.Name == "CMSPageFooter1" || Module.Name == "CMSPageFooter2" || Module.Name == "FooterLink")
                {
                    if (_session.GetString("admin_" + Module.Name + "_search_cat_id") == null)
                    {
                        _session.SetString("admin_" + Module.Name + "_search_cat_id", "0");
                        ViewBag.showMoveTools = true;
                    }
                }
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }

            #region ---------- Views ------------
            try
            {
                return View("~/Areas/Admin/Views/" + Module.Config.UseViewListFrom + "/Index.cshtml");
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}", e.Message)
                });
            }
            #endregion
        }

        public virtual IActionResult Detail(int id)
        {
            if (Module.Config.EnableViewDetail == false)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Page not found",
                    ErrorDetail = "The page you are looking for no longer exists."
                });
            }

            Module = _admin.setBreadcrumbCMSPage(Module);

            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/รายละเอียด";

                #region ----- Select Item -----
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการดู";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                #endregion

                #region -------- ViewBag -----------
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.itemEdit = itemEdit.Rows[0];
                #endregion

                #region Logs Action
                _admin.ActionLogs(
                    admin_user_id: (int)_session.GetInt32("admin_user_id"),
                    admin_username: _session.GetString("admin_user"),
                    action: "view_detail",
                    action_info: string.Format("ดูรายละเอียดข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                    action_url: Request.Host.Value + Request.Path.Value,
                    action_table: Module.Config.Table,
                    mod_name: Module.Name,
                    mod_name_txt: Module.Config.Text
                );
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

            #region ---------- Views ------------
            try
            {
                /*if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/" + Module.Name + "/Detail.cshtml"))
                {
                    return View("~/Areas/Admin/Views/" + Module.Name + "/Detail.cshtml");
                }
                else if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/AdminCore/Detail.cshtml"))
                {
                    return View("~/Areas/Admin/Views/AdminCore/Detail.cshtml");
                }
                else
                {
                    return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                    {
                        ErrorTitle = "Views not found",
                        ErrorDetail = string.Format("The view '{0}' was not found.", "~/Areas/Admin/Views/" + Module.Name + "/Detail.cshtml")
                    });
                }*/
                return View("~/Areas/Admin/Views/" + Module.Config.UseViewDetailFrom + "/Detail.cshtml");
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}", e.Message)
                });
            }
            #endregion
        }

        [ModuleCheck("add")]
        public virtual IActionResult Create()
        {
            Module = _admin.setBreadcrumbCMSPage(Module);
            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/เพิ่ม";

                #region -------- ViewBag -----------
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
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

            #region ---------- Views ------------
            try
            {
                if (Module.Name == "EForm")
                {
                    var EForm_Group_Rows = _db.ExecuteQuery(string.Format("select top 999 * from {0} where status = 1 AND ( ( pb_issue_date_config = '1' ) OR ( pb_issue_date_config = '2' AND ( pb_issue_date < SYSDATETIMEOFFSET() AND pb_expiry_date > SYSDATETIMEOFFSET() ) ) ) and web_id = @web_id order by sort asc", Db.T("web_eform_group")), new Dictionary<string, object>() { { "status", 1 }, { "web_id", _currentWebID } });
                    ViewBag.EForm_Group_Rows = EForm_Group_Rows;
                }

                return View("~/Areas/Admin/Views/" + Module.Config.UseViewCreateFrom + "/Create.cshtml");
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}", e.Message)
                });
            }
            #endregion
        }

        [HttpPost]
        [ModuleCheck("add")]
        public virtual IActionResult Create(IFormCollection collection)
        {
            try
            {
                #region Set Cate session 
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }
                #endregion

                bool is_default_field = true;
                var insertFields = _admin.setFieldsCreate(Module, collection, is_default_field);
                var affected = _db.Insert(Module.Config.Table, insertFields);

                if (affected != 0)
                {
                    int last_create_id = 0;
                    var lastInsert = _db.ExecuteQuery(string.Format("SELECT top 1 MAX(id) as last_id from {0} WHERE web_id = @web_id GROUP BY id ORDER BY id desc", Db.T(Module.Config.Table)), new() { { "web_id", _currentWebID } });

                    if (lastInsert.Rows.Count > 0 && !string.IsNullOrEmpty(lastInsert.Rows[0]["last_id"].ToString()) && _utility.isInt(lastInsert.Rows[0]["last_id"] + ""))
                    {
                        last_create_id = Convert.ToInt32(lastInsert.Rows[0]["last_id"].ToString());
                    }

                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "add",
                        action_info: string.Format("เพิ่มข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, last_create_id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: "",
                        new_value: JsonConvert.SerializeObject(insertFields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    TempData["alert_message"] = string.Format("เพิ่มข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถเพิ่มข้อมูลได้";
                    return Redirect("Create");
                }
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

        [ModuleCheck("edit")]
        public virtual IActionResult Edit(int id)
        {
            Module = _admin.setBreadcrumbCMSPage(Module);
            try
            {
                Module.Config.TextBreadcrumb = Module.Config.TextBreadcrumb + "/แก้ไข";

                #region Select Item Edit #########
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                #endregion

                #region -------- ViewBag -----------
                ViewBag._utility = _utility;
                ViewBag._admin = _admin;
                ViewBag._session = _session;
                ViewBag._db = _db;
                ViewBag.Module = Module;
                ViewBag.Title = Module.Config.Text;
                ViewBag.ModuleName = Module.Name;
                ViewBag.itemEdit = itemEdit.Rows[0];
                // ค่านี้ถูกฝังลงใน <script> ของ View (var jsonRow = @Html.Raw(...)) → ต้อง escape HTML กัน stored XSS
                ViewBag.itemJSONEdit = _db.DataTableToScriptJSON(itemEdit);
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

            #region ---------- Views ------------
            try
            {
                /*if (Module.Config.UseViewCreateFrom != "" && System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/" + Module.Config.UseViewCreateFrom + "/Edit.cshtml"))
                {
                    return View("~/Areas/Admin/Views/" + Module.Config.UseViewCreateFrom + "/Edit.cshtml");
                }
                else if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/" + Module.Name + "/Edit.cshtml"))
                {
                    return View("~/Areas/Admin/Views/" + Module.Name + "/Edit.cshtml");
                }
                else if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/AdminCore/Edit.cshtml"))
                {
                    return View("~/Areas/Admin/Views/AdminCore/Edit.cshtml");
                }
                else
                {
                    return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                    {
                        ErrorTitle = "Views not found",
                        ErrorDetail = string.Format("The view '{0}' was not found.", "~/Areas/Admin/Views/" + Module.Name + "/Edit.cshtml")
                    });
                }*/
                if (Module.Name == "EForm")
                {
                    var EForm_Group_Rows = _db.ExecuteQuery(string.Format("select top 999 * from {0} where status = 1 AND ( ( pb_issue_date_config = '1' ) OR ( pb_issue_date_config = '2' AND ( pb_issue_date < SYSDATETIMEOFFSET() AND pb_expiry_date > SYSDATETIMEOFFSET() ) ) ) and web_id = @web_id order by sort asc", Db.T("web_eform_group")), new Dictionary<string, object>() { { "status", 1 }, { "web_id", _currentWebID } });
                    ViewBag.EForm_Group_Rows = EForm_Group_Rows;
                }

                return View("~/Areas/Admin/Views/" + Module.Config.UseViewEditFrom + "/Edit.cshtml");
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}", e.Message)
                });
            }
            #endregion
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public virtual IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                #region Select Item Edit #########
                var itemEdit = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (itemEdit.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการแก้ไข";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                else
                {
                    //----- check locked row (waiting for approve) -----//
                    if (Module.Config.CanApprove == true && !string.IsNullOrEmpty(itemEdit.Rows[0]["pb_status"].ToString()) && itemEdit.Rows[0]["pb_status"] + "" == "0")
                    {
                        TempData["alert_message"] = "รายการที่รออนุมัติ ไม่สามารถแก้ไขได้";
                        TempData["alert_class"] = "alert-warning";
                        return RedirectToActionPermanent("Edit", new { id = id });
                    }
                }
                #endregion

                #region Set Cate session ############
                if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField) && collection.ContainsKey(Module.Config.TableCateField))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField, collection[Module.Config.TableCateField] + "");
                }

                // CMSPage (nested by parent_id): after saving, return the list to the edited item's OWN
                // level. Use the item's real parent from the DB so the redirect is correct regardless of
                // whatever level the session was last left at (e.g. when editing via a direct URL).
                if (Module.Config.IsCMSPage == true && !string.IsNullOrEmpty(Module.Config.FieldCMSPage) && itemEdit.Columns.Contains(Module.Config.FieldCMSPage))
                {
                    _session.SetString("admin_" + Module.Name + "_search_" + Module.Config.FieldCMSPage, itemEdit.Rows[0][Module.Config.FieldCMSPage] + "");
                }
                #endregion

                bool is_default_field = true;

                if (Module.Name == "Branch")
                {
                    is_default_field = false;
                }
                else if (Module.Name == "HomeHeader")
                {
                    // เลือก Template (this_type) แล้ว re-order layout ของหน้าแรก
                    string this_type = collection["this_type"] + "";
                    string box_layout;

                    if (_currentWebID == 0)
                    {
                        // ----- เว็บไซต์หลัก (web_id=0): คงพฤติกรรมเดิมทุกอย่าง -----
                        // token wg_* ชุดนี้ถูกตีความโดย Views/Home/Index.cshtml ของเว็บหลัก (เทียบ literal เป็น flag ไม่แตะ web_widget2)
                        box_layout = "wg_2,wg_3,wg_4,wg_11,wg_12,wg_13,wg_25,wg_14"; // Template 1 (ค่าเริ่มต้น)
                        if (this_type == "2")
                        {
                            box_layout = "wg_6,wg_7,wg_9,wg_10,wg_8,wg_15,wg_26,wg_16";
                        }
                        else if (this_type == "3")
                        {
                            box_layout = "wg_18,wg_19,wg_20,wg_21,wg_22,wg_23,wg_27,wg_24";
                        }
                    }
                    else
                    {
                        // ----- microsite (web_id != 0): สร้าง box_layout จาก web_widget2 ของ microsite เอง -----
                        // front-end (Microsite1.cshtml) ตีความ token เป็น wg_<id ของ web_widget2>
                        // จึง map ลำดับ section ตาม Template โดยอ้าง "title" (คีย์ที่เหมือนกันทุก microsite)
                        // แล้วแปลงเป็น wg_<id> ของ microsite นั้น ๆ
                        box_layout = BuildMicrositeBoxLayout(_currentWebID, this_type);
                    }

                    // microsite ที่ไม่มี widget (box_layout ว่าง) จะไม่เขียนทับ เพื่อไม่ให้ล้าง layout เดิมโดยไม่ตั้งใจ
                    if (!string.IsNullOrEmpty(box_layout))
                    {
                        _db.ExecuteNonQuery("UPDATE [2026_web_cms_page] SET box_layout = @box_layout, pb_box_layout = @box_layout WHERE is_home = '1' AND web_id = @web_id ", new() { { "box_layout", box_layout }, { "web_id", _currentWebID } });
                    }
                }

                var updateFields = _admin.setFieldsUpdate(Module, collection, is_default_field);
                //return Json(updateFields);
                var affected = _db.Update(Module.Config.Table, "WHERE id = @id and web_id = @web_id", updateFields, new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });

                if (affected != 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: "edit",
                        action_info: string.Format("แก้ไขข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        old_value: _db.DataTableToJSONWithJSONNet(itemEdit).TrimStart('[').TrimEnd(']'),
                        new_value: JsonConvert.SerializeObject(updateFields),
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    TempData["alert_message"] = string.Format("แก้ไขข้อมูลแล้ว {0:n0} รายการ", affected);
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถแก้ไขข้อมูลได้";
                    return RedirectToActionPermanent("Edit", new { id = id });
                }
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

        /// <summary>
        /// สร้างค่า box_layout ของหน้าแรก microsite (web_id != 0) ตาม Template (this_type) ที่เลือก
        /// โดยอ้างลำดับ section ด้วย "title" ของ web_widget2 (title เหมือนกันทุก microsite ส่วน id ต่างกันต่อไซต์)
        /// แล้วแปลงเป็น token wg_&lt;id&gt; ที่ front-end (Microsite1.cshtml) เข้าใจ
        /// ลำดับแต่ละ Template อ้างอิงจากภาพต้นแบบ T1/T2/T3
        /// </summary>
        private string BuildMicrositeBoxLayout(int webId, string thisType)
        {
            // ลำดับ section (อ้างด้วย title) ต่อ Template
            string[] order = thisType switch
            {
                "2" => new[]
                {
                    "แบนเนอร์หลัก",
                    "จุดเด่นบริการ 5 ข้อ",
                    "ทรัพย์ที่ร่วมรายการ",
                    "ตารางแผนผ่อนชำระ",
                    "เงื่อนไขโครงการ",
                    "สอบถามเพิ่มเติม (QR)",
                },
                "3" => new[]
                {
                    "แบนเนอร์หลัก",
                    "ตัวนับสถิติ",
                    "แนวทางที่เราช่วยได้",
                    "เอกสารที่ต้องเตรียม",
                    "ฟอร์มติดต่อกลับ",
                    "คำถามที่พบบ่อย",
                },
                // "1" และค่าอื่น ๆ → Template 1
                _ => new[]
                {
                    "แบนเนอร์หลัก",
                    "มากกว่าการบริหารสินทรัพย์",
                    "จุดมุ่งหมาย 4 ข้อ",
                    "แบนเนอร์ครึ่งจอ",
                    "เอกสารที่เกี่ยวข้อง",
                },
            };

            // map title -> id ของ web_widget2 (พาเลตต์ค่าคงที่ ไม่ใช้ web_id เป็นเงื่อนไข)
            var rows = _db.ExecuteQuery("SELECT id, title FROM [2026_web_widget2]");
            var titleToId = new Dictionary<string, string>();
            foreach (System.Data.DataRow r in rows.Rows)
            {
                string t = (r["title"] + "").Trim();
                if (!string.IsNullOrEmpty(t) && !titleToId.ContainsKey(t))
                {
                    titleToId[t] = r["id"] + "";
                }
            }

            // ต่อเป็น wg_<id> ตามลำดับ Template (title ที่หาไม่เจอจะข้ามไป)
            var tokens = new List<string>();
            foreach (string title in order)
            {
                if (titleToId.TryGetValue(title, out var wid) && !string.IsNullOrEmpty(wid))
                {
                    tokens.Add("wg_" + wid);
                }
            }
            return string.Join(",", tokens);
        }

        [HttpPost]
        [ModuleCheck("delete")]
        public virtual IActionResult Delete(IFormCollection f)
        {
            try
            {
                // TODO: Add delete logic here
                if (f.ContainsKey("id") && !string.IsNullOrEmpty(f["id"]))
                {
                    string allID = f["id"].ToString();
                    var allIDar = allID.Split(',');
                    if (allIDar.Length == 0)
                    {
                        TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                    else
                    {
                        int rowDel = 0;
                        int rowFail = 0;
                        foreach (string id in allIDar)
                        {
                            #region ----- select item delete -----
                            var itemDelete = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                            #endregion

                            if (_utility.isInt(id) && itemDelete.Rows.Count > 0)
                            { 
                                if(Module.Config.Table == "web_microsite")
                                {
                                    //----- ลบข้อมูลทุก table ที่ผูกกับ microsite นี้ เพื่อไม่ให้เหลือ orphan (table clean)
                                    //      ดึงรายชื่อ table ที่มีคอลัมน์ web_id จาก information_schema แบบไดนามิก
                                    //      -> ครอบคลุมทุก table ปัจจุบัน + table ที่เพิ่มในอนาคตอัตโนมัติ (ไม่ต้องมาแก้ list เอง)
                                    //      ยกเว้น web_microsite เอง (แถวของ microsite มี web_id=0 จะถูกลบด้วย id ด้านล่าง)
                                    //----- และยกเว้น 4 ตาราง widget ที่เป็น "ค่าคงที่" (พาเลตต์ Drag-Drop, ไม่มี CRUD)
                                    //      ห้ามลบเด็ดขาด ไม่ว่ากรณีใด — เป็น reference data ที่ทุก microsite ใช้ร่วมกัน
                                    var constWidgetTables = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                                    {
                                        "web_widget", "web_widget_group", "web_widget2", "web_widget_group2"
                                    };
                                    //----- ค้นเฉพาะตารางของระบบนี้ (ขึ้นต้นด้วย prefix 2026_) จะได้ไม่ไปแตะตารางเดิมของ asset_plus_uat
                                    var tablesWithWebId = _db.ExecuteQuery(
                                        "select table_name from information_schema.columns " +
                                        "where column_name = 'web_id' and table_schema = 'dbo' " +
                                        "  and table_name like '" + Db.Prefix + "%' and table_name <> '" + Db.Prefix + "web_microsite'");

                                    foreach (System.Data.DataRow tRow in tablesWithWebId.Rows)
                                    {
                                        string physicalTable = tRow["table_name"] + "";
                                        //----- เทียบกับรายชื่อตารางคงที่ด้วย "ชื่อตรรกะ" (ตัด prefix ออกก่อน)
                                        string cleanTable = physicalTable.StartsWith(Db.Prefix, StringComparison.OrdinalIgnoreCase)
                                            ? physicalTable.Substring(Db.Prefix.Length) : physicalTable;
                                        if (constWidgetTables.Contains(cleanTable)) continue;   //----- ข้ามตารางคงที่
                                        //----- ชื่อ table มาจาก information_schema (ไม่ใช่ user input) จึงต่อ string ได้อย่างปลอดภัย
                                        _db.ExecuteNonQuery("delete from " + Db.T(cleanTable) + " where web_id = @id", new() { { "id", Convert.ToInt32(id) } });
                                    }
                                }
                                else if (Module.Config.Table == "web_admin_access")
                                {
                                    _db.ExecuteNonQuery("delete from [2026_web_admin_module] where access_id = @id", new() { { "id", Convert.ToInt32(id) } });
                                }

                                var affected = _db.ExecuteNonQuery(string.Format("delete from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                                rowDel += affected;

                                if (affected > 0)
                                {
                                    if (Module.Config.CanMove == true)
                                    {
                                        _admin.reSort(Module);
                                    }
                                    #region Logs Action
                                    _admin.ActionLogs(
                                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                                        admin_username: _session.GetString("admin_user"),
                                        action: "delete",
                                        action_info: string.Format("ลบข้อมูล : {0} ({1})", Module.Config.TextBreadcrumb, id),
                                        action_url: Request.Host.Value + Request.Path.Value,
                                        action_table: Module.Config.Table,
                                        old_value: _db.DataTableToJSONWithJSONNet(itemDelete).TrimStart('[').TrimEnd(']'),
                                        mod_name: Module.Name,
                                        mod_name_txt: Module.Config.Text
                                    );
                                    #endregion
                                }
                            }
                        }

                        TempData["alert_message"] = string.Format("ลบข้อมูลแล้ว {0:n0} รายการ", rowDel);
                        TempData["alert_class"] = "alert-success";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));

                    }
                }
                else
                {
                    TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }

        [HttpPost]
        [ModuleCheck("status")]
        public virtual IActionResult Status(IFormCollection f)
        {
            try
            {
                string status_row = "";
                if (f.ContainsKey("status") && !string.IsNullOrEmpty(f["status"]) && _utility.isInt(f["status"].ToString()))
                {
                    status_row = f["status"].ToString();
                }
                else
                {
                    TempData["alert_message"] = string.Format("สถานะ เปิด-ปิด ไม่ถูกต้อง");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                // ป้ายกำกับสถานะ — คอลัมน์ status มี 3 ค่า ไม่ใช่ 2
                //   0 = ปิด (ซ่อน), 1 = เปิด (แสดงปกติ), 2 = ปักหมุด (เมนูรูปสไลด์หน้าแรก — แสดงเป็นอันดับแรก)
                // เดิมใช้ (status_row == "1" ? "เปิด" : "ปิด") ทำให้ค่า 2 ตกไปเข้าเคส "ปิด"
                // ข้อความแจ้งเตือนและ audit log จึงขึ้นว่า "ปิดการแสดงผล" ทั้งที่กดปักหมุด
                string status_text = status_row switch
                {
                    "2" => "ปักหมุด",
                    "1" => "เปิด",
                    _   => "ปิด"
                };

                if (f.ContainsKey("id") && !string.IsNullOrEmpty(f["id"]))
                {
                    string allID = f["id"].ToString();
                    var allIDar = allID.Split(',');
                    if (allIDar.Length == 0)
                    {
                        TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการ {0}", status_text);
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                    else
                    {
                        int rowAffected = 0;
                        foreach (string id in allIDar)
                        {
                            if (_utility.isInt(id))
                            {
                                #region ----- check wating approve and public -----
                                if (Convert.ToInt32(status_row) == 0)
                                {
                                    var item = _db.ExecuteQuery($"select top 1 * from {Db.T(Module.Config.Table)} where id = @id and web_id = @web_id", new Dictionary<string, object>() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                                    if (item.Rows.Count > 0 && item.Rows[0]["pb_status"] + "" == "0" && item.Rows[0]["show_front"] + "" == "0")
                                    {
                                        TempData["alert_message"] = string.Format("ไม่สามารถ ปิด การแสดงผลได้ เนื่องจากเป็นรายการ รอการเผยแพร่");
                                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                                    }
                                }
                                #endregion

                                // เขียน updated_at ด้วย — การเปลี่ยนสถานะคือการแก้ไขข้อมูลเช่นกัน
                                // เดิมอัปเดตเฉพาะ status ทำให้คอลัมน์ "วันที่แก้ไข" ในหน้า list ไม่ขยับ
                                var affected = _db.Update(Module.Config.Table, "WHERE id = @id and web_id = @web_id ", new Dictionary<string, object>() { { "status", Convert.ToInt32(status_row) }, { "updated_at", System.DateTime.Now } }, new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                                rowAffected += affected;

                                if (affected > 0)
                                {
                                    #region Logs Action
                                    _admin.ActionLogs(
                                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                                        admin_username: _session.GetString("admin_user"),
                                        action: "status",
                                        action_info: string.Format("{2}การแสดงผล : {0} ({1})", Module.Config.TextBreadcrumb, id, status_text),
                                        action_url: Request.Host.Value + Request.Path.Value,
                                        action_table: Module.Config.Table,
                                        mod_name: Module.Name,
                                        mod_name_txt: Module.Config.Text
                                    );
                                    #endregion
                                }
                            }
                        }

                        TempData["alert_message"] = status_text + string.Format("การแสดงผลแล้ว {0:n0} รายการ", rowAffected);
                        TempData["alert_class"] = "alert-success";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));

                    }
                }
                else
                {
                    TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }

        [HttpPost]
        [ModuleCheck("approve")]
        public virtual IActionResult Approve(int id, IFormCollection f)
        {
            try
            {
                #region Select Item #########
                var item = _db.ExecuteQuery(string.Format("select top 1 * from {0} where id = @id and web_id = @web_id", Db.T(Module.Config.Table)), new Dictionary<string, object>() { { "id", id }, { "web_id", _currentWebID } });
                if (item.Rows.Count == 0)
                {
                    TempData["alert_message"] = "ไม่พบข้อมูลที่ต้องการอนุมัติ";
                    TempData["alert_class"] = "alert-warning";
                    return RedirectToAction("Index");
                }
                #endregion

                #region Approve Status #########
                string approve_status = "";
                if (f.ContainsKey("approve") && !string.IsNullOrEmpty(f["approve"]) && (f["approve"].ToString() == "1" || f["approve"].ToString() == "0"))
                {
                    approve_status = f["approve"].ToString();
                }
                else
                {
                    TempData["alert_message"] = string.Format("สถานะ อนุมัติ ไม่ถูกต้อง");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
                #endregion

                #region Check field approve #######
                if (Module.Config.FieldApprove == null || Module.Config.FieldApprove.Count == 0)
                {
                    TempData["alert_message"] = string.Format("FieldApprove is not set");
                    TempData["alert_class"] = "alert-danger";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
                #endregion

                string sqlApprove = "";

                sqlApprove = string.Format(" UPDATE {0} set ", Db.T(Module.Config.Table));
                var fieldApprove = new List<string>();
                fieldApprove.Add(" approve_by = @approve_by ");
                foreach (string field in Module.Config.FieldApprove)
                {
                    if (approve_status == "1")
                    {
                        fieldApprove.Add(string.Format(" pb_{0} = {0} ", field));
                    }
                    else if (approve_status == "0")
                    {
                        fieldApprove.Add(string.Format(" {0} = pb_{0} ", field));
                    }
                }

                if (approve_status == "1")
                {
                    fieldApprove.Add(" pb_status = 1 ");
                    fieldApprove.Add(" show_front = 1 ");
                }
                else
                {
                    fieldApprove.Add(" pb_status = 1 ");
                }

                sqlApprove += string.Join(",", fieldApprove);
                sqlApprove += " WHERE id = @id ";

                var affected = _db.ExecuteNonQuery(sqlApprove, new Dictionary<string, object>() { { "id", id }, { "approve_by", _session.GetString("admin_user") ?? "" } });

                if (affected != 0)
                {
                    #region Logs Action
                    _admin.ActionLogs(
                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                        admin_username: _session.GetString("admin_user"),
                        action: approve_status == "1" ? "approve" : "unapprove",
                        action_info: string.Format("{2} : {0} ({1})", Module.Config.TextBreadcrumb, id, approve_status == "1" ? "อนุมัติ" : "ไม่อนุมัติ"),
                        action_url: Request.Host.Value + Request.Path.Value,
                        action_table: Module.Config.Table,
                        mod_name: Module.Name,
                        mod_name_txt: Module.Config.Text
                    );
                    #endregion

                    string alert_message = "";
                    if (approve_status == "1")
                    {
                        alert_message = string.Format("อนุมัติข้อมูลแล้ว {0:n0} รายการ", affected);
                    }
                    else
                    {
                        alert_message = string.Format("ยกเลิกแล้ว {0:n0} รายการ", affected);
                    }
                    TempData["alert_message"] = alert_message;
                    TempData["alert_class"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["alert_message"] = "ไม่สามารถอนุมัติข้อมูลได้";
                    return RedirectToActionPermanent("Edit", new { id = id });
                }
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

        [HttpPost]
        [ModuleCheck("move")]
        public virtual IActionResult Move(IFormCollection f)
        {
            Module = _admin.setSessionRequest(Module, Request);

            #region Check sort 
            if (Module.Config.OrderBy.ToLower() != "sort" || Module.Config.Sort.ToLower() != "asc")
            {
                TempData["alert_message"] = string.Format("ไม่สามารถย้ายตำแหน่งได้ เนื่องจากการจัดเรียงไม่ถูกต้อง");
                TempData["alert_class"] = "alert-warning";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
            if (!string.IsNullOrEmpty(Module.Config.TableCate) && !string.IsNullOrEmpty(Module.Config.TableCateField))
            {
                string cate_val = _session.GetString("admin_" + Module.Name + "_search_" + Module.Config.TableCateField) ?? "";
                if (cate_val == "")
                {
                    TempData["alert_message"] = string.Format("ไม่สามารถย้ายตำแหน่งได้ เนื่องจากยังไม่ได้เลือกกลุ่ม ({0}='{1}')", Module.Config.TableCateField, cate_val);
                    TempData["alert_class"] = "alert-warning";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            #endregion

            try
            {
                string move_row = "";
                int move_step = 1;
                #region ----- check post data -----
                if (f.ContainsKey("move") && !string.IsNullOrEmpty(f["move"]) && (f["move"].ToString().ToLower() == "up" || f["move"].ToString().ToLower() == "down"))
                {
                    move_row = f["move"].ToString();
                }
                else
                {
                    TempData["alert_message"] = string.Format("สถานะ ย้าย ไม่ถูกต้อง");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }

                if (f.ContainsKey("step") && !string.IsNullOrEmpty(f["step"]) && _utility.isInt(f["step"].ToString()))
                {
                    move_step = Convert.ToInt32(f["step"]);
                    if (move_step <= 0)
                    {
                        TempData["alert_message"] = string.Format("จำนวน Step ไม่ถูกต้อง");
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                }
                #endregion

                move_step = (move_step * 10) - 10;

                if (f.ContainsKey("id") && !string.IsNullOrEmpty(f["id"]))
                {
                    string allID = f["id"].ToString();
                    var allIDar = allID.Split(',');
                    if (allIDar.Length == 0)
                    {
                        TempData["alert_message"] = "กรุณาระบุรายการที่ต้องการย้าย";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }
                    else
                    {
                        int rowAffected = 0;
                        foreach (string id in allIDar)
                        {
                            if (_utility.isInt(id))
                            {
                                var affected = _db.ExecuteNonQuery(string.Format("update {0} set sort = (sort{1}15){1}{2} where id = @id and web_id = @web_id ", Db.T(Module.Config.Table), (move_row == "up") ? "-" : "+", move_step), new() { { "id", Convert.ToInt32(id) }, { "web_id", _currentWebID } });
                                rowAffected += affected;

                                _admin.reSort(Module);

                                if (affected > 0)
                                {
                                    #region Logs Action
                                    _admin.ActionLogs(
                                        admin_user_id: (int)_session.GetInt32("admin_user_id"),
                                        admin_username: _session.GetString("admin_user"),
                                        action: "move",
                                        action_info: string.Format("เลื่อน{2} : {0} ({1})", Module.Config.TextBreadcrumb, id, move_row == "up" ? "ขึ้น" : "ลง"),
                                        action_url: Request.Host.Value + Request.Path.Value,
                                        action_table: Module.Config.Table,
                                        mod_name: Module.Name,
                                        mod_name_txt: Module.Config.Text
                                    );
                                    #endregion
                                }
                            }
                        }

                        TempData["alert_message"] = string.Format("เลื่อน{1}แล้ว {0:n0} รายการ", rowAffected, move_row == "up" ? "ขึ้น" : "ลง");
                        TempData["alert_class"] = "alert-success";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));

                    }
                }
                else
                {
                    TempData["alert_message"] = string.Format("กรุณาระบุรายการที่ต้องการลบ");
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            catch (Exception ex)
            {
                TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                TempData["alert_class"] = "alert-danger";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }

        [HttpPost]
        [ModuleCheck("export")]
        public virtual IActionResult Export(IFormCollection f)
        {
            if (f.ContainsKey("export") && !string.IsNullOrEmpty(f["export"]))
            {
                string data_decrypt = _utility.Decrypt(f["export"].ToString(), _utility.appKey());

                if (data_decrypt != "")
                {
                    if (true)//try
                    {
                        JObject sql_object = JObject.Parse(data_decrypt);

                        string sql = sql_object["sql"].ToString();
                        var sql_parameter = JsonConvert.DeserializeObject<Dictionary<string, object>>(sql_object["parameter"].ToString());

                        var field_as = new List<string>();
                        foreach (var item in Module.Config.ExportData)
                        {
                            string field_title = item.Value ?? item.Key;
                            field_as.Add(string.Format("{0} as \"{1}\"", item.Key, field_title.Replace("\"", " ")));
                        }

                        #region ----- where id in (....) -----
                        string conditionID = "";
                        if (f.ContainsKey("export_id") && !string.IsNullOrEmpty(f["export_id"]))
                        {
                            bool check_verify_pass = false;
                            if (f["export_id"].ToString().IndexOf(',') > -1)
                            {
                                check_verify_pass = true;
                                string[] aaa;
                                aaa = f["export_id"].ToString().Split(',');
                                foreach (string aaa_item in aaa)
                                {
                                    Int32 a;
                                    if (!Int32.TryParse(aaa_item, out a))
                                    {
                                        check_verify_pass = false;
                                    }
                                }
                            }
                            else
                            {
                                Int32 a;
                                if (Int32.TryParse(f["export_id"], out a))
                                {
                                    check_verify_pass = true;
                                }
                            }

                            if (check_verify_pass)
                            {
                                conditionID = " id IN (" + f["export_id"] + ") ";

                                var sql_arr = sql.Split("where");
                                if (sql_arr.Length > 1)
                                {
                                    sql = string.Format(" {0} where {1} and {2} ", sql_arr[0], conditionID, sql_arr[1]);
                                }
                            }

                        }
                        #endregion

                        #region ----- web_id -----
                        conditionID += " and web_id = @web_id ";
                        if (!sql_parameter.ContainsKey("web_id")) { sql_parameter.Add("web_id", _currentWebID); }                        
                        #endregion



                        var command_sql = sql.Replace("select *", "select " + string.Join(",", field_as));

                        if (Module.Config.TableModuleID != null && Module.Config.TableModuleID > 0)
                        {
                            command_sql = string.Format(" where module_id = '" + Module.Config.TableModuleID.ToString() + "' ");
                        }

                        if (Module.Config.Table == "web_admin_log")
                        {
                            // T-SQL วางจำนวนแถวไว้หน้ารายการคอลัมน์ (TOP) ไม่ใช่ท้ายคำสั่งแบบ LIMIT ของ PostgreSQL
                            command_sql = System.Text.RegularExpressions.Regex.Replace(
                                command_sql, @"^\s*select\s+", "select top 1000 ",
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        }

                        var dt = _db.ExecuteQuery(command_sql, sql_parameter);

                        ExcelPackage excel = new ExcelPackage();
                        var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                        int report_rows = 1;

                        #region ----- header style -----
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            int c_xls = 1;
                            foreach (System.Data.DataColumn column in dt.Columns)
                            {
                                workSheet.Cells[report_rows, c_xls].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[report_rows, c_xls].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                                workSheet.Cells[report_rows, c_xls].Style.Font.Bold = true;
                                workSheet.Cells[report_rows, c_xls].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                c_xls++;
                            }
                        }
                        #endregion

                        workSheet.Cells[report_rows, 1].LoadFromDataTable(dt, true);

                        // เขียนหัวคอลัมน์จากชื่อที่ตั้งไว้ใน ExportData โดยตรง (กัน PostgreSQL ตัดชื่อ alias ที่ยาวเกิน 63 ไบต์
                        // ซึ่งทำให้หัวตารางภาษาไทยยาว ๆ ถูกตัดหรือมีเลขต่อท้ายเพื่อกันชื่อซ้ำ)
                        for (int i = 0; i < Module.Config.ExportData.Count && i < dt.Columns.Count; i++)
                        {
                            workSheet.Cells[1, i + 1].Value = Module.Config.ExportData[i].Value ?? Module.Config.ExportData[i].Key;
                        }

                        var fileStream = new MemoryStream();
                        excel.SaveAs(fileStream);
                        fileStream.Position = 0;

                        #region Logs Action
                        _admin.ActionLogs(
                            admin_user_id: (int)_session.GetInt32("admin_user_id"),
                            admin_username: _session.GetString("admin_user"),
                            action: "export",
                            action_info: string.Format("export ข้อมูล : {0} ({1} แถว)", Module.Config.TextBreadcrumb, dt.Rows.Count),
                            action_url: Request.Host.Value + Request.Path.Value,
                            action_table: Module.Config.Table,
                            old_value: _db.DataTableToJSONWithJSONNet(dt),
                            mod_name: Module.Name,
                            mod_name_txt: Module.Config.Text
                        );
                        #endregion

                        string file_name = Module.Name + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_.xlsx";
                        return File(fileStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name);
                    }
                    /*catch (Exception ex)
                    {
                        TempData["alert_message"] = string.Format("เกิดข้อผิดพลาด, {0}", ex.Message);
                        TempData["alert_class"] = "alert-danger";
                        return Redirect(string.Format("/Admin/{0}", Module.Name));
                    }*/
                }
                else
                {
                    TempData["alert_message"] = string.Format("ไม่สามารถ export ข้อมูลได้ (decrypt)");
                    TempData["alert_class"] = "alert-error";
                    return Redirect(string.Format("/Admin/{0}", Module.Name));
                }
            }
            else
            {
                TempData["alert_message"] = string.Format("ไม่สามารถ export ข้อมูลได้ (query not set)");
                TempData["alert_class"] = "alert-error";
                return Redirect(string.Format("/Admin/{0}", Module.Name));
            }
        }
    }
}
