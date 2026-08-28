using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Runtime;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ServerStatusController : AdminCoreController
    {
        public ServerStatusController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("ServerStatus");
        }

        [HttpGet]
        public IActionResult CheckWebServerStatus()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var proc        = Process.GetCurrentProcess();
                var uptime      = DateTime.Now - proc.StartTime;
                var memMb       = Math.Round(proc.WorkingSet64 / 1024.0 / 1024.0, 1);
                var threadCount = proc.Threads.Count;
                sw.Stop();

                string status = "healthy";
                string detail = "เว็บเซิร์ฟเวอร์ทำงานปกติ";

                if (sw.ElapsedMilliseconds > 500)
                {
                    status = "warning";
                    detail = "ตอบสนองช้ากว่าปกติ";
                }

                string uptimeStr = uptime.Days > 0
                    ? $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m"
                    : $"{uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";

                return Json(new
                {
                    status,
                    detail,
                    responseMs  = sw.ElapsedMilliseconds,
                    uptimeStr,
                    memMb,
                    threadCount,
                    environment = _hostingEnvironment.EnvironmentName
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                return Json(new
                {
                    status      = "critical",
                    detail      = "เว็บเซิร์ฟเวอร์ผิดปกติ: " + ex.Message,
                    responseMs  = sw.ElapsedMilliseconds,
                    uptimeStr   = "-",
                    memMb       = 0,
                    threadCount = 0,
                    environment = ""
                });
            }
        }

        [HttpGet]
        public IActionResult CheckDbStatus()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using var conn = new SqlConnection(_db.DefaultConnectionString);
                conn.Open();
                using var cmd = new SqlCommand("SELECT 1", conn);
                cmd.ExecuteScalar();
                sw.Stop();

                return Json(new
                {
                    status = "healthy",
                    detail = "เชื่อมต่อฐานข้อมูลสำเร็จ",
                    responseMs = sw.ElapsedMilliseconds
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                return Json(new
                {
                    status = "critical",
                    detail = "เชื่อมต่อฐานข้อมูลไม่สำเร็จ: " + ex.Message,
                    responseMs = sw.ElapsedMilliseconds
                });
            }
        }
    }
}
