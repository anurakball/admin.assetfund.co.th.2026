using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Filters;
using thaicredit_hr_admin.Areas.Admin.Helpers;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    [AdminLogin(json_response: true)]
    [Route("el-finder/file-system")]
    public class FileSystemController : Controller
    {
        public readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _config; 
        private readonly IHttpContextAccessor _context;
        private readonly ISession _session;
        private Utility _utility;
        private DBHelper _db;
         
        public FileSystemController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(hostingEnvironment, iConfig);
            _db = new DBHelper(hostingEnvironment, iConfig); 
        }

        [Route("set-session-hash-path")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> SetSessionHashPath(string hash)
        {
            HttpContext.Session.SetString("admin_elfinder_root_hash", hash);
            return Content("ok", MediaTypeNames.Application.Json);
        }
         
        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            int access_id = HttpContext.Session.GetInt32("admin_access_id") ?? 0;  
            int webID = HttpContext.Session.GetInt32("admin_web_id") ?? 0;
            var access = _db.ExecuteQuery("select top 1 * from [2026_web_admin_module] where access_id = '"+ access_id + "' and web_id = '" + webID + "' and mod_name = 'FileManager'");  
            if (access.Rows.Count == 0)
            {
                return Content("access denied...", MediaTypeNames.Application.Json);
            }
             
            var connector = GetConnector();
 
            #region ----- custom logic -----
            if (Request.Method.ToLower() == "post" && !string.IsNullOrWhiteSpace(Request.Form["cmd"]) && Request.Form["cmd"].ToString().ToLower() == "upload")
            {
                foreach (var formFile in Request.Form.Files)
                {
                    var fileEx = System.IO.Path.GetExtension(formFile.FileName);
                    if (
                        fileEx.ToLower() == ".webp" ||
                        fileEx.ToLower() == ".svg" ||
                        fileEx.ToLower() == ".png" ||
                        fileEx.ToLower() == ".gif" ||
                        fileEx.ToLower() == ".jpg" ||
                        fileEx.ToLower() == ".jpeg" ||
                        fileEx.ToLower() == ".pdf" ||
                        fileEx.ToLower() == ".doc" ||
                        fileEx.ToLower() == ".docx" ||
                        fileEx.ToLower() == ".txt" ||
                        fileEx.ToLower() == ".ppt" ||
                        fileEx.ToLower() == ".pptx" ||
                        fileEx.ToLower() == ".xls" ||
                        fileEx.ToLower() == ".xlsx" ||
                        fileEx.ToLower() == ".csv" ||
                        fileEx.ToLower() == ".zip" ||
                        fileEx.ToLower() == ".rar" ||
                        fileEx.ToLower() == ".xml"
                        )
                    {
                        
                    }
                    else
                    {
                        return StatusCode(200, new { error = "Connot upload file. " + fileEx.ToLower() + " file is not valid." });
                    }
                }
            }
            #endregion

            return await connector.ProcessAsync(Request);
        }

        [Route("thumb/{hash}")]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            int webID = HttpContext.Session.GetInt32("admin_web_id") ?? 0;

            string folderPath = $"Files/Site{webID}";

            if (!Directory.Exists(PathHelper.MapPath($"~/{folderPath}")))
            {
                Directory.CreateDirectory(PathHelper.MapPath($"~/{folderPath}"));
            }

            string AdmID = HttpContext.Session.GetString("admin_id");
             
            if (!Directory.Exists(PathHelper.MapPath($"~/{folderPath}/{AdmID}")))
            {
                Directory.CreateDirectory(PathHelper.MapPath($"~/{folderPath}/{AdmID}"));
            }

            if (Directory.Exists(PathHelper.MapPath($"~/{folderPath}/{AdmID}/.tmb")))
            { 
                System.IO.File.SetAttributes(PathHelper.MapPath($"~/{folderPath}/{AdmID}/.tmb"), FileAttributes.Hidden);
            }

            var root = new RootVolume(PathHelper.MapPath($"~/{folderPath}/{AdmID}"),$"{uri.Scheme}://{uri.Authority}/{folderPath}/{AdmID}",$"{uri.Scheme}://{uri.Authority}/el-finder/file-system/thumb/")
            {
                //IsReadOnly = !User.IsInRole("Administrators")
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = folderPath, // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 51200, // Limit imposed to user uploaded file <= 50 MB
                AccessControlAttributes = new HashSet<NamedAccessControlAttributeSet>()
                {
                    /*
                    new NamedAccessControlAttributeSet(PathHelper.MapPath("~/Files/"+HttpContext.Session.GetString("admin_id").ToString()+"/file.txt"))
                    {
                        Read = false,
                        Write = false,
                        Locked = true
                    }, 
                    new NamedAccessControlAttributeSet(PathHelper.MapPath($"~/{folderPath}/{AdmID}/.tmb"))
                    {
                        Read = false,
                        Write = false,
                        Locked = true
                    }
                    */ 
                },
                // Upload file type constraints
                //UploadAllow = new[] { "image", "text" },
                //'uploadAllow' => array('image', 'text/plain', 'text/html','application/pdf','application/zip'),// Mimetype image and text/plain allowed to upload
                //UploadDeny = new[] { "text/csv" },
                //UploadOrder = new[] { "allow", "deny" }
            };

            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        } 
    }
}