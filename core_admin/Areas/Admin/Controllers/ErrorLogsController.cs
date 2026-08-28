using Microsoft.AspNetCore.Mvc;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class ErrorLogsController : AdminCoreController
    {
        public ErrorLogsController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("ErrorLogs");
        }

        public override IActionResult Index()
        {
            Module = _admin.setSessionRequest(Module, Request);
            Module = _admin.setBreadcrumbCMSPage(Module);

            string logsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");
            var files = new List<object>();

            if (Directory.Exists(logsPath))
            {
                var fileInfos = new DirectoryInfo(logsPath)
                    .GetFiles("*.*")
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();

                foreach (var fi in fileInfos)
                {
                    files.Add(new
                    {
                        name = fi.Name,
                        size = fi.Length,
                        lastModified = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
            }

            ViewBag.Module = Module;
            ViewBag.Title = Module.Config.Text;
            ViewBag._utility = _utility;
            ViewBag._admin = _admin;
            ViewBag._session = _session;
            ViewBag._db = _db;
            ViewBag.searchInputVal = new Dictionary<string, string>();
            ViewBag.LogFiles = files;

            return View("~/Areas/Admin/Views/ErrorLogs/Index.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> GetFileContent(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Json(new { success = false, message = "ไม่พบชื่อไฟล์" });

            // Security: prevent path traversal
            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                return Json(new { success = false, message = "ชื่อไฟล์ไม่ถูกต้อง" });

            string logsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");
            string filePath = Path.Combine(logsPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return Json(new { success = false, message = "ไม่พบไฟล์" });

            try
            {
                const long maxBytes = 5 * 1024 * 1024; // 5 MB
                var fi = new FileInfo(filePath);

                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                long readLength = Math.Min(fs.Length, maxBytes);
                var buffer = new byte[readLength];
                _ = await fs.ReadAsync(buffer, 0, (int)readLength);
                string content = System.Text.Encoding.UTF8.GetString(buffer);

                bool truncated = fs.Length > maxBytes;
                return Json(new { success = true, content, fileName, truncated, totalSize = fi.Length });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "ไม่สามารถอ่านไฟล์ได้: " + ex.Message });
            }
        }
    }
}
