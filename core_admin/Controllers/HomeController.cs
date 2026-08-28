using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using thaicredit_hr_admin.Models;

namespace thaicredit_hr_admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly Utility _utility;
        private readonly DBHelper _db;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, IConfiguration iConfig)
        {
            _logger = logger;
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig);
        }

        public IActionResult Index()
        {
            return Redirect("/Admin");
            /*var ss = _db.ExecuteQuery("select * from web_admin_logx where created_by = @val1 and admin_name = @val2 ", new Dictionary<string, object>() { { "val1", "test1" }, { "val2", "admin_name1" } });
            if (ss.Rows.Count > 0)
            {
                return Ok(ss.Rows[0]["admin_name"]);
            }*/
            /*var ss2 = _db.ExecuteQuery("select * from web_admin_log");
            if (ss2.Rows.Count > 0)
            {
                return Ok(ss2.Rows[0]["admin_name"]);
            }*/
            //return Ok(_db.DefaultConnectionString);
            //return Ok(_db.ExecuteNonQuery("update web_admin_log set admin_name = @admin_name where id = @id", new Dictionary<string, object>() { { "admin_name", "ทดสอบชื่อ admin" }, { "id", 8 } }));
            /*return Ok(_db.Update("web_admin_log", "where id = @id", 
                new Dictionary<string, object>() { { "admin_name", "ทดสอบชื่อ admin 999" } },
                new Dictionary<string, object>() { { "id", 8 } }
                ));*/
            //created_at, updated_at, created_by, updated_by
            /*return Ok(_db.Insert("web_admin_log",
                new Dictionary<string, object>() {
                    { "created_at", System.DateTime.Now },
                    { "updated_at", System.DateTime.Now },
                    { "created_by", "backend" },
                    { "updated_by", "code" }
                }));*/
            //return View();
        }

        /*public IActionResult Privacy()
        {
            //x
            return View();
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}