using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MemberReportController : AdminCoreController
    {
        public MemberReportController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MemberReport");
        }

        public override IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);
            Module = _admin.setBreadcrumbCMSPage(Module);

            try
            {
                var p = new Dictionary<string, object>() { { "web_id", _currentWebID } };

                // ---- web_member stats ----
                var dtMemberTotal = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE web_id = @web_id", p);
                var dtMemberActive = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE status = 1 AND web_id = @web_id", p);
                var dtMemberEmailConfirmed = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE confirm_email = 1 AND web_id = @web_id", p);
                var dtMemberType1 = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE type = 1 AND web_id = @web_id", p);
                var dtMemberType2 = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE type = 2 AND web_id = @web_id", p);
                var dtMemberToday = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE DATE(reg_date AT TIME ZONE 'Asia/Bangkok') = CURRENT_DATE AND web_id = @web_id", p);
                var dtMemberMonth = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_member WHERE DATE_TRUNC('month', reg_date) = DATE_TRUNC('month', NOW()) AND web_id = @web_id", p);
                var dtMemberWithAgent = _db.ExecuteQuery("SELECT COUNT(DISTINCT m.id) AS cnt FROM web_member m INNER JOIN web_agent a ON m.id = a.uid WHERE m.web_id = @web_id", p);

                // ---- web_agent stats ----
                var dtAgentTotal = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE web_id = @web_id", p);
                var dtAgentApproved = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE approved = 1 AND web_id = @web_id", p);
                var dtAgentPending = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE approved = 0 AND web_id = @web_id", p);
                var dtAgentActive = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE status = 1 AND web_id = @web_id", p);
                var dtAgentToday = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE DATE(adddate AT TIME ZONE 'Asia/Bangkok') = CURRENT_DATE AND web_id = @web_id", p);
                var dtAgentMonth = _db.ExecuteQuery("SELECT COUNT(*) AS cnt FROM web_agent WHERE DATE_TRUNC('month', adddate) = DATE_TRUNC('month', NOW()) AND web_id = @web_id", p);

                // ---- monthly trend last 6 months (member + agent) ----
                var dtMemberTrend = _db.ExecuteQuery(@"
                    SELECT TO_CHAR(DATE_TRUNC('month', reg_date), 'YYYY-MM') AS ym,
                           COUNT(*) AS cnt
                    FROM web_member
                    WHERE web_id = @web_id AND reg_date >= NOW() - INTERVAL '6 months'
                    GROUP BY ym ORDER BY ym", p);

                var dtAgentTrend = _db.ExecuteQuery(@"
                    SELECT TO_CHAR(DATE_TRUNC('month', adddate), 'YYYY-MM') AS ym,
                           COUNT(*) AS cnt
                    FROM web_agent
                    WHERE web_id = @web_id AND adddate >= NOW() - INTERVAL '6 months'
                    GROUP BY ym ORDER BY ym", p);

                // ---- ViewBag ----
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

                ViewBag.MemberTotal        = dtMemberTotal.Rows.Count > 0 ? Convert.ToInt64(dtMemberTotal.Rows[0]["cnt"]) : 0;
                ViewBag.MemberActive       = dtMemberActive.Rows.Count > 0 ? Convert.ToInt64(dtMemberActive.Rows[0]["cnt"]) : 0;
                ViewBag.MemberEmailConfirm = dtMemberEmailConfirmed.Rows.Count > 0 ? Convert.ToInt64(dtMemberEmailConfirmed.Rows[0]["cnt"]) : 0;
                ViewBag.MemberType1        = dtMemberType1.Rows.Count > 0 ? Convert.ToInt64(dtMemberType1.Rows[0]["cnt"]) : 0;
                ViewBag.MemberType2        = dtMemberType2.Rows.Count > 0 ? Convert.ToInt64(dtMemberType2.Rows[0]["cnt"]) : 0;
                ViewBag.MemberToday        = dtMemberToday.Rows.Count > 0 ? Convert.ToInt64(dtMemberToday.Rows[0]["cnt"]) : 0;
                ViewBag.MemberMonth        = dtMemberMonth.Rows.Count > 0 ? Convert.ToInt64(dtMemberMonth.Rows[0]["cnt"]) : 0;
                ViewBag.MemberWithAgent    = dtMemberWithAgent.Rows.Count > 0 ? Convert.ToInt64(dtMemberWithAgent.Rows[0]["cnt"]) : 0;

                ViewBag.AgentTotal    = dtAgentTotal.Rows.Count > 0 ? Convert.ToInt64(dtAgentTotal.Rows[0]["cnt"]) : 0;
                ViewBag.AgentApproved = dtAgentApproved.Rows.Count > 0 ? Convert.ToInt64(dtAgentApproved.Rows[0]["cnt"]) : 0;
                ViewBag.AgentPending  = dtAgentPending.Rows.Count > 0 ? Convert.ToInt64(dtAgentPending.Rows[0]["cnt"]) : 0;
                ViewBag.AgentActive   = dtAgentActive.Rows.Count > 0 ? Convert.ToInt64(dtAgentActive.Rows[0]["cnt"]) : 0;
                ViewBag.AgentToday    = dtAgentToday.Rows.Count > 0 ? Convert.ToInt64(dtAgentToday.Rows[0]["cnt"]) : 0;
                ViewBag.AgentMonth    = dtAgentMonth.Rows.Count > 0 ? Convert.ToInt64(dtAgentMonth.Rows[0]["cnt"]) : 0;

                ViewBag.MemberTrend   = dtMemberTrend;
                ViewBag.AgentTrend    = dtAgentTrend;
            }
            catch (Exception e)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml", new thaicredit_hr_admin.Areas.Admin.Models.ErrorAdminModel
                {
                    ErrorTitle = "Internal Server Error",
                    ErrorDetail = string.Format("{0}<br/>{1}", e.Message, e.StackTrace)
                });
            }

            return View("~/Areas/Admin/Views/MemberReport/Index.cshtml");
        }
    }
}
