using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thaicredit_hr_admin.Areas.Admin.Filters;

namespace thaicredit_hr_admin.Areas.Admin.Controllers
{
    public class MemberListController : AdminCoreController
    {
        public MemberListController(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext) : base(hostingEnvironment, iConfig, iContext)
        {
            Module = _admin.GetModule("MemberList");
        }

        /*public override IActionResult Index()
        {
            return View();
        }*/

        [HttpGet]
        public IActionResult SearchMember(string q)
        {
            if (string.IsNullOrEmpty(q) || q.Length < 3)
                return Json(new List<object>());

            try
            {
                var dt = _db.ExecuteQuery(
                    "SELECT top 50 id, firstname, surname FROM [2026_web_member] WHERE firstname LIKE @q OR surname LIKE @q",
                    new Dictionary<string, object>() { { "q", "%" + q + "%" } }
                );

                var results = new List<object>();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string text = row["firstname"] + " : " + row["surname"];
                    results.Add(new { id = row["id"], text });
                }
                return Json(results);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        [HttpPost]
        [ModuleCheck("edit")]
        public override IActionResult Edit(int id, IFormCollection collection)
        {
            if (collection.ContainsKey("password") && !string.IsNullOrEmpty(collection["password"]))
            {
                var formDict = collection.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                formDict["password"] = BCrypt.Net.BCrypt.HashPassword(collection["password"], workFactor: 12);
                collection = new FormCollection(formDict);
            }
            else
            {
                // password ไม่ถูกส่งมา หรือส่งมาว่าง → กัน setFieldsUpdate ใส่ DBNull โดยเอาออกจาก FieldUpdate
                Module.Config.FieldUpdate = Module.Config.FieldUpdate?.Where(f => f != "password").ToList();
                if (collection.ContainsKey("password"))
                {
                    var formDict = collection.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    formDict.Remove("password");
                    collection = new FormCollection(formDict);
                }
            }
            return base.Edit(id, collection);
        }
    }
}
