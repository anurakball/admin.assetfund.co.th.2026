using System.Text.Json;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// Google reCAPTCHA v2 แบบกล่องติ๊ก "I'm not a robot" — ใช้กับหน้า Login และ modal re-login (endpoint เดียวกัน)
    ///
    /// ตั้งค่าที่ appsettings → "GoogleReCaptcha":
    ///   SiteKey   : ใส่ในหน้าเว็บ (ไม่ใช่ความลับ)
    ///   SecretKey : ใช้ตรวจฝั่ง server (ความลับ)
    ///   VerifyUrl : ค่าปกติ https://www.google.com/recaptcha/api/siteverify
    ///
    /// ⚠ ตอนนี้ทั้ง appsettings.json และ appsettings.Development.json ใส่ "คีย์ทดสอบของ Google" ไว้
    ///   (ใช้ได้บน localhost, ติ๊กแล้วผ่านทุกครั้ง, กล่องขึ้นข้อความเตือนสีแดงว่าใช้ทดสอบเท่านั้น)
    ///   ขึ้นเซิร์ฟเวอร์จริงต้องสมัครคีย์ของโดเมนจริงที่ https://www.google.com/recaptcha/admin แล้วแทนที่ทั้ง 2 ค่า
    ///
    /// ถ้าไม่ได้ตั้ง SiteKey/SecretKey → ปฏิเสธการ login (fail closed) และแจ้งข้อความให้ตั้งค่า
    /// ไม่ปล่อยผ่านเงียบ ๆ เพราะจะกลายเป็นหลังบ้านที่ไม่มีกันบอทโดยไม่มีใครรู้
    /// </summary>
    public static class ReCaptcha
    {
        public const string FormField = "g-recaptcha-response";

        // HttpClient ตัวเดียวใช้ทั้งแอป (สร้างใหม่ทุก request จะทำ socket หมด)
        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };

        public static string SiteKey(IConfiguration config) => (config["GoogleReCaptcha:SiteKey"] ?? "").Trim();

        public static bool IsConfigured(IConfiguration config) =>
            SiteKey(config) != "" && (config["GoogleReCaptcha:SecretKey"] ?? "").Trim() != "";

        /// <summary>
        /// ข้าม reCAPTCHA ตอนพัฒนาบนเครื่อง (ผู้ใช้สั่ง 18 ก.ย. 2569 — login ซ้ำหลายรอบตอนทดสอบ) ต้องครบทั้ง 3 ข้อ:
        ///   1) ASPNETCORE_ENVIRONMENT = Development
        ///   2) appsettings → GoogleReCaptcha:SkipOnLocalhost = true (ใส่ไว้ใน appsettings.Development.json เท่านั้น)
        ///   3) IP ของ connection เป็น loopback (127.0.0.1 / ::1) — ใช้ IP ของ socket จริง ไม่ใช่ Host/X-Forwarded-For ที่ปลอมได้
        /// เซิร์ฟเวอร์จริงไม่เข้าข้อ 1 และไม่มีไฟล์ Development.json จึงบังคับ reCAPTCHA ตามปกติ
        /// ใช้ทั้ง Login [HttpPost] (ไม่ตรวจ token) หน้า Login และ modal re-login (ไม่แสดงกล่อง)
        /// </summary>
        public static bool SkipForLocal(HttpContext? http, IConfiguration config)
        {
            if (http == null) return false;
            if (!string.Equals((config["GoogleReCaptcha:SkipOnLocalhost"] ?? "").Trim(), "true", StringComparison.OrdinalIgnoreCase)) return false;
            var env = http.RequestServices.GetService<IWebHostEnvironment>();
            if (env == null || !env.IsDevelopment()) return false;
            var ip = http.Connection.RemoteIpAddress;
            if (ip == null) return false;
            if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
            return System.Net.IPAddress.IsLoopback(ip);
        }

        public enum Result { Ok, Missing, Invalid, NotConfigured, Unreachable }

        /// <summary>ตรวจ token ที่ browser ส่งมากับ Google — เรียกก่อนเช็ก username/password ทุกครั้ง</summary>
        public static Result Verify(IConfiguration config, string? token, string? remoteIp)
        {
            if (!IsConfigured(config)) return Result.NotConfigured;
            token = (token ?? "").Trim();
            if (token == "") return Result.Missing;

            var url = (config["GoogleReCaptcha:VerifyUrl"] ?? "").Trim();
            if (url == "") url = "https://www.google.com/recaptcha/api/siteverify";

            var form = new Dictionary<string, string>
            {
                ["secret"] = config["GoogleReCaptcha:SecretKey"]!.Trim(),
                ["response"] = token,
            };
            if (!string.IsNullOrWhiteSpace(remoteIp)) form["remoteip"] = remoteIp!;

            try
            {
                using var res = _http.PostAsync(url, new FormUrlEncodedContent(form)).GetAwaiter().GetResult();
                var body = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                using var doc = JsonDocument.Parse(body);
                return doc.RootElement.TryGetProperty("success", out var ok) && ok.ValueKind == JsonValueKind.True
                    ? Result.Ok
                    : Result.Invalid;
            }
            catch
            {
                return Result.Unreachable;
            }
        }

        /// <summary>ข้อความที่แสดงผู้ใช้ตามผลตรวจ (Ok = null)</summary>
        public static string? Message(Result r) => r switch
        {
            Result.Ok => null,
            Result.Missing => "กรุณายืนยันว่าคุณไม่ใช่โปรแกรมอัตโนมัติ (ติ๊ก I'm not a robot)",
            Result.Invalid => "การยืนยัน reCAPTCHA ไม่ผ่านหรือหมดอายุ กรุณาติ๊กใหม่อีกครั้ง",
            Result.NotConfigured => "ระบบยังไม่ได้ตั้งค่า reCAPTCHA (GoogleReCaptcha:SiteKey / SecretKey) กรุณาติดต่อผู้ดูแลระบบ",
            _ => "ไม่สามารถตรวจสอบ reCAPTCHA กับ Google ได้ในขณะนี้ กรุณาลองใหม่อีกครั้ง",
        };
    }
}
