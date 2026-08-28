using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class AdminUserNotLoginController : AdminCoreController
    {
        public AdminUserNotLoginController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("AdminUserNotLogin");
        }

        public override IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);
            Module = _admin.setBreadcrumbCMSPage(Module);

            try
            {
                #region ----- List Data -----

                var sqlParam = new Dictionary<string, object>() { { "id", 0 }, { "web_id", _currentWebID } };
                string sqlSelect = string.Format(" select * ");
                string sqlFrom = string.Format(" from {0} ", Module.Config.Table);
                string sqlWhere = string.Format(" where id > @id and web_id = @web_id ");
                string sqlDateSearch = "";
                string sqlFieldSearch = "";
                string sqlOrder = string.Format(" order by {0} {1} ", Module.Config.OrderBy, Module.Config.Sort);

                #region ----- Date Search -----
                if (Module.Config.EnableDateSearch == true)
                {
                    if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_after")))
                    {
                        sqlDateSearch += " and created_at >= (@after::timestamp) ";
                        sqlParam.Add("after", _session.GetString("admin_" + Module.Name + "_after") + " 00:00:00");
                    }
                    if (!string.IsNullOrEmpty(_session.GetString("admin_" + Module.Name + "_before")))
                    {
                        sqlDateSearch += " and created_at <= (@before::timestamp) ";
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
                                    arCondition.Add(string.Format(" LOWER(cast({0} as text)) = LOWER(cast({1} as text)) ", Fields, "@search_" + FieldSearch.Key));
                                }
                                else
                                {
                                    arCondition.Add(string.Format(" LOWER(cast({0} as text)) like LOWER({1}) ", Fields, "@search_" + FieldSearch.Key));
                                }
                            }

                            if (Module.Config.FieldSearchIsEqual != null && Module.Config.FieldSearchIsEqual.Where(c => c == FieldSearch.Key).Count() == 1)
                            {
                                sqlParam.Add("search_" + FieldSearch.Key, _session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key).ToString());
                            }
                            else
                            {
                                sqlParam.Add("search_" + FieldSearch.Key, "%" + _session.GetString("admin_" + Module.Name + "_search_" + FieldSearch.Key).ToString() + "%");
                            }

                            sqlFieldSearch += " and ( " + string.Join(" or ", arCondition) + " ) ";

                        }
                    }
                }

                var searchInputVal = _admin.getSearchInputValue(Module);
                #endregion

                string sql2 = $"select admin_user_id from web_admin_log where action in ('login', 'login_2fa') {sqlDateSearch} group by admin_user_id ";
                string sql = string.Format("{0} {1} {2} {3} {4}", sqlSelect, sqlFrom, sqlWhere, $" and id not in ({sql2}) ", sqlOrder);
                
                #region ----- Encrypt SQL Query (for export) -----
                string sqlQuery = _utility.Encrypt(JsonConvert.SerializeObject(new { sql = sql, parameter = sqlParam }), _utility.appKey());
                #endregion

                #region ----- Pagination -----
                string currentURL = _utility.rootURL() + Request.Path.ToString() + Request.QueryString.ToString();
                string sqlPage = string.Format("select count(id) as total_rows {0} {1} {2}", sqlFrom, sqlWhere, $" and id not in ({sql2}) ");
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

                sql += " limit @perpage offset @start ";
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
                /*if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "Areas/Admin/Views/" + Module.Name + "/Index.cshtml"))
                {
                    return View("~/Areas/Admin/Views/" + Module.Name + "/Index.cshtml");
                }
                else
                {
                    return View("~/Areas/Admin/Views/AdminCore/Index.cshtml");
                }*/
                if (!string.IsNullOrEmpty(Request.Query["json"].ToString())) { return Ok(_utility.Decrypt(ViewBag.sqlQuery, _utility.appKey())); }
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
    }
}
