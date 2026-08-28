using System.Text.Json;
using System.Text.RegularExpressions;

/// <summary>
/// ผลการตรวจสอบ Username/Password กับ Azure AD
/// </summary>
public enum AzureAdAuthStatus
{
    /// <summary>ตรวจผ่าน — Username/Password ถูกต้อง</summary>
    Success,
    /// <summary>Username หรือ Password ไม่ถูกต้อง / ไม่มีบัญชีนี้ใน tenant</summary>
    InvalidCredentials,
    /// <summary>รหัสถูก แต่บัญชีเข้าไม่ได้ (ถูกล็อค / ปิดใช้งาน / รหัสหมดอายุ / ต้องทำ MFA)</summary>
    AccountBlocked,
    /// <summary>รหัสถูก แต่ยังไม่ได้ Grant admin consent ให้แอป (AADSTS65001)</summary>
    ConsentRequired,
    /// <summary>ยังไม่ได้ตั้งค่า AzureAd ใน appsettings หรือปิดใช้งานอยู่</summary>
    NotConfigured,
    /// <summary>ติดต่อ Azure ไม่ได้ / ตั้งค่า client id หรือ secret ผิด / error อื่น</summary>
    ServiceError
}

public class AzureAdAuthResult
{
    public AzureAdAuthStatus Status { get; set; } = AzureAdAuthStatus.ServiceError;
    public bool IsSuccess => Status == AzureAdAuthStatus.Success;

    /// <summary>รหัส error ของ Azure เช่น AADSTS50126 (ว่างถ้าสำเร็จ)</summary>
    public string ErrorCode { get; set; } = "";
    /// <summary>ข้อความภาษาไทยสำหรับแสดงผู้ใช้</summary>
    public string Message { get; set; } = "";
    /// <summary>ข้อความดิบจาก Azure สำหรับเก็บ log (ไม่แสดงผู้ใช้)</summary>
    public string RawError { get; set; } = "";

    //----- ข้อมูลผู้ใช้จาก id_token (มีเฉพาะกรณีสำเร็จและได้ token กลับมา)
    public string DisplayName { get; set; } = "";
    public string Email { get; set; } = "";
    public string ObjectId { get; set; } = "";
}

public interface IAzureAdAuthService
{
    /// <summary>เปิดใช้งาน + ตั้งค่าครบหรือไม่</summary>
    bool IsEnabled { get; }
    /// <summary>โดเมนอีเมลพนักงานที่อนุญาต (ว่าง = ไม่จำกัดโดเมน)</summary>
    IReadOnlyList<string> AllowedEmailDomains { get; }
    /// <summary>ตรวจว่า Username เข้าเกณฑ์ "อีเมลพนักงาน" หรือไม่ (ใช้ทั้งหน้า Create/Edit และตอน Login)</summary>
    bool IsValidEmployeeUsername(string username, out string errorMessage);
    /// <summary>ตรวจ Username/Password กับ Azure AD (ROPC — grant_type=password)</summary>
    AzureAdAuthResult ValidateCredential(string username, string password);
}

/// <summary>
/// ตรวจ Username/Password ของผู้ดูแลระบบ "ประเภทการ Login = แบบพนักงาน" กับ Azure AD
/// ด้วย OAuth2 Resource Owner Password Credentials (ROPC) บน endpoint v2.0
///
/// ค่าเชื่อมต่อทั้งหมดอ่านจาก appsettings(.Development).json section "AzureAd" — ไม่ hardcode
/// คลาสนี้ "ไม่ยุ่ง" กับผู้ใช้ประเภทปกติเลย ถูกเรียกเฉพาะเมื่อ web_admin.login_type = 2
/// </summary>
public class AzureAdAuthService : IAzureAdAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AzureAdAuthService> _logger;

    private readonly bool _enabled;
    private readonly string _instance;
    private readonly string _tenantId;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _scope;
    private readonly int _timeoutSeconds;
    private readonly bool _allowConsentPending;
    private readonly List<string> _allowedDomains = new();

    private static readonly Regex EmailRegex = new(
        @"^[A-Za-z0-9!#$%&'*+/=?^_`{|}~.-]+@[A-Za-z0-9]([A-Za-z0-9-]*[A-Za-z0-9])?(\.[A-Za-z0-9]([A-Za-z0-9-]*[A-Za-z0-9])?)+$",
        RegexOptions.Compiled);

    public AzureAdAuthService(IConfiguration config, IHttpClientFactory httpClientFactory, ILogger<AzureAdAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        var s = config.GetSection("AzureAd");
        _enabled = s.GetValue<bool>("Enabled");
        _instance = (s["Instance"] ?? "https://login.microsoftonline.com/").Trim();
        _tenantId = (s["TenantId"] ?? "").Trim();
        _clientId = (s["ClientId"] ?? "").Trim();
        _clientSecret = (s["ClientSecret"] ?? "").Trim();
        _scope = string.IsNullOrWhiteSpace(s["Scope"]) ? "openid profile email" : s["Scope"]!.Trim();
        _timeoutSeconds = s.GetValue<int?>("TimeoutSeconds") ?? 20;
        _allowConsentPending = s.GetValue<bool>("AllowConsentPending");

        if (!_instance.EndsWith("/")) _instance += "/";
        if (_timeoutSeconds <= 0) _timeoutSeconds = 20;

        foreach (var d in (s["AllowedEmailDomains"] ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            _allowedDomains.Add(d.TrimStart('@').ToLowerInvariant());
        }
    }

    public bool IsEnabled =>
        _enabled &&
        !string.IsNullOrWhiteSpace(_tenantId) &&
        !string.IsNullOrWhiteSpace(_clientId) &&
        !string.IsNullOrWhiteSpace(_clientSecret);

    public IReadOnlyList<string> AllowedEmailDomains => _allowedDomains;

    public bool IsValidEmployeeUsername(string username, out string errorMessage)
    {
        errorMessage = "";
        username = (username ?? "").Trim();

        if (username == "")
        {
            errorMessage = "กรุณาระบุ Username (อีเมลพนักงาน)";
            return false;
        }
        if (username.Length > 255 || !EmailRegex.IsMatch(username))
        {
            errorMessage = "ประเภทการ Login แบบพนักงาน : Username ต้องอยู่ในรูปแบบอีเมล เช่น name@" + (_allowedDomains.Count > 0 ? _allowedDomains[0] : "example.com");
            return false;
        }
        if (_allowedDomains.Count > 0)
        {
            string domain = username.Substring(username.LastIndexOf('@') + 1).ToLowerInvariant();
            if (!_allowedDomains.Contains(domain))
            {
                errorMessage = "ประเภทการ Login แบบพนักงาน : Username ต้องเป็นอีเมลโดเมน " + string.Join(" หรือ ", _allowedDomains.Select(d => "@" + d));
                return false;
            }
        }
        return true;
    }

    public AzureAdAuthResult ValidateCredential(string username, string password)
    {
        if (!IsEnabled)
        {
            return new AzureAdAuthResult
            {
                Status = AzureAdAuthStatus.NotConfigured,
                Message = "ระบบยังไม่ได้ตั้งค่าการเชื่อมต่อ Azure AD กรุณาติดต่อผู้ดูแลระบบ",
                RawError = "AzureAd section is disabled or incomplete (Enabled/TenantId/ClientId/ClientSecret)"
            };
        }

        username = (username ?? "").Trim();
        if (username == "" || string.IsNullOrEmpty(password))
        {
            return new AzureAdAuthResult
            {
                Status = AzureAdAuthStatus.InvalidCredentials,
                Message = "Username / Password ไม่ถูกต้อง",
                RawError = "empty username or password"
            };
        }

        string tokenUrl = $"{_instance}{_tenantId}/oauth2/v2.0/token";

        try
        {
            var client = _httpClientFactory.CreateClient("AzureAd");
            client.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);

            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "scope", _scope },
                { "username", username },
                { "password", password },
                { "grant_type", "password" }
            });

            using var response = client.PostAsync(tokenUrl, form).GetAwaiter().GetResult();
            string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var ok = new AzureAdAuthResult { Status = AzureAdAuthStatus.Success, Message = "" };
                ReadIdToken(body, ok);
                return ok;
            }

            return MapError(body, (int)response.StatusCode);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Azure AD ROPC timeout for {User}", username);
            return new AzureAdAuthResult
            {
                Status = AzureAdAuthStatus.ServiceError,
                Message = "ไม่สามารถติดต่อระบบยืนยันตัวตน Azure AD ได้ (หมดเวลา) กรุณาลองใหม่อีกครั้ง",
                RawError = "timeout: " + ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Azure AD ROPC error for {User}", username);
            return new AzureAdAuthResult
            {
                Status = AzureAdAuthStatus.ServiceError,
                Message = "ไม่สามารถติดต่อระบบยืนยันตัวตน Azure AD ได้ กรุณาติดต่อผู้ดูแลระบบ",
                RawError = ex.Message
            };
        }
    }

    /// <summary>แปลง error ของ Azure (AADSTSxxxxx) เป็นสถานะ + ข้อความไทย</summary>
    private AzureAdAuthResult MapError(string body, int httpStatus)
    {
        string error = "";
        string description = "";
        string firstCode = "";

        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            if (root.TryGetProperty("error", out var e)) error = e.GetString() ?? "";
            if (root.TryGetProperty("error_description", out var d)) description = d.GetString() ?? "";
            if (root.TryGetProperty("error_codes", out var c) && c.ValueKind == JsonValueKind.Array && c.GetArrayLength() > 0)
            {
                firstCode = c[0].ToString();
            }
        }
        catch
        {
            description = body;
        }

        //----- ตัด Trace ID / Correlation ID ออกจากข้อความ log ให้อ่านง่าย
        int cut = description.IndexOf("Trace ID", StringComparison.OrdinalIgnoreCase);
        string shortDesc = (cut > 0 ? description.Substring(0, cut) : description).Trim();

        var result = new AzureAdAuthResult
        {
            ErrorCode = firstCode != "" ? "AADSTS" + firstCode : (error != "" ? error : "http_" + httpStatus),
            RawError = $"http {httpStatus} / {error} / {shortDesc}"
        };

        switch (firstCode)
        {
            //----- รหัสผ่านไม่ถูกต้อง / ไม่มีบัญชีนี้ใน directory
            case "50126":
            case "50034":
            case "50056":
                result.Status = AzureAdAuthStatus.InvalidCredentials;
                result.Message = "Username / Password ไม่ถูกต้อง";
                break;

            //----- รหัสถูกต้อง แต่บัญชีเข้าไม่ได้
            case "50053": // ถูกล็อคจากการกรอกผิดหลายครั้ง (smart lockout)
                result.Status = AzureAdAuthStatus.AccountBlocked;
                result.Message = "บัญชีพนักงานถูกล็อคชั่วคราวจากการกรอกรหัสผ่านผิดหลายครั้ง กรุณาลองใหม่ภายหลัง";
                break;
            case "50055": // รหัสผ่านหมดอายุ
                result.Status = AzureAdAuthStatus.AccountBlocked;
                result.Message = "รหัสผ่านของบัญชีพนักงานหมดอายุ กรุณาเปลี่ยนรหัสผ่านที่ระบบขององค์กรก่อน";
                break;
            case "50057": // บัญชีถูกปิดใช้งาน
                result.Status = AzureAdAuthStatus.AccountBlocked;
                result.Message = "บัญชีพนักงานนี้ถูกปิดใช้งาน กรุณาติดต่อผู้ดูแลระบบ";
                break;
            case "50072":
            case "50074":
            case "50076":
            case "50079":
            case "50158":
            case "53003": // Conditional Access
                result.Status = AzureAdAuthStatus.AccountBlocked;
                result.Message = "บัญชีพนักงานนี้ต้องยืนยันตัวตนเพิ่มเติม (MFA/Conditional Access) จึงเข้าสู่ระบบด้วยวิธีนี้ไม่ได้ กรุณาติดต่อผู้ดูแลระบบ";
                break;

            //----- ยังไม่ได้ Grant admin consent ให้แอป (Azure ตรวจรหัสผ่านผ่านแล้วถึงจะมาถึงขั้นนี้)
            case "65001":
                result.Status = AzureAdAuthStatus.ConsentRequired;
                result.Message = "แอปยังไม่ได้รับอนุญาต (admin consent) จาก Azure AD กรุณาติดต่อผู้ดูแลระบบ";
                break;

            //----- ตั้งค่าฝั่งระบบผิด
            case "7000215":
            case "7000222":
                result.Status = AzureAdAuthStatus.ServiceError;
                result.Message = "การตั้งค่าเชื่อมต่อ Azure AD ไม่ถูกต้อง (Client Secret) กรุณาติดต่อผู้ดูแลระบบ";
                break;
            case "700016":
            case "90002":
                result.Status = AzureAdAuthStatus.ServiceError;
                result.Message = "การตั้งค่าเชื่อมต่อ Azure AD ไม่ถูกต้อง (Client ID / Tenant ID) กรุณาติดต่อผู้ดูแลระบบ";
                break;
            case "7000218":
                result.Status = AzureAdAuthStatus.ServiceError;
                result.Message = "การตั้งค่าแอปบน Azure AD ไม่รองรับการเข้าสู่ระบบด้วยวิธีนี้ กรุณาติดต่อผู้ดูแลระบบ";
                break;

            default:
                //----- error อื่นที่ยังไม่ได้ map: ถ้าเป็น invalid_grant ให้ถือว่ารหัสผ่านไม่ถูกต้อง
                if (error == "invalid_grant")
                {
                    result.Status = AzureAdAuthStatus.InvalidCredentials;
                    result.Message = "Username / Password ไม่ถูกต้อง";
                }
                else
                {
                    result.Status = AzureAdAuthStatus.ServiceError;
                    result.Message = "ไม่สามารถยืนยันตัวตนกับ Azure AD ได้ กรุณาติดต่อผู้ดูแลระบบ";
                }
                break;
        }

        //----- AADSTS65001 เกิดขึ้น "หลัง" Azure ตรวจ Username/Password ผ่านแล้วเท่านั้น
        //      (รหัสผิดจะได้ AADSTS50126 / ไม่มีบัญชีจะได้ AADSTS50034 ตั้งแต่ก่อนถึงขั้นนี้)
        //      จึงเปิด switch ไว้ให้ใช้งานได้ระหว่างรอ Grant admin consent — ปิดเป็น false ใน production
        if (result.Status == AzureAdAuthStatus.ConsentRequired && _allowConsentPending)
        {
            _logger.LogWarning("Azure AD: accepted credential with pending admin consent (AzureAd:AllowConsentPending = true). {Raw}", result.RawError);
            return new AzureAdAuthResult
            {
                Status = AzureAdAuthStatus.Success,
                ErrorCode = result.ErrorCode,
                RawError = result.RawError
            };
        }

        return result;
    }

    /// <summary>อ่าน claim จาก id_token (ไม่ verify signature — ใช้แค่แสดงผล/บันทึก log เท่านั้น)</summary>
    private void ReadIdToken(string body, AzureAdAuthResult result)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("id_token", out var idTokenEl)) return;

            string idToken = idTokenEl.GetString() ?? "";
            var parts = idToken.Split('.');
            if (parts.Length < 2) return;

            string payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            using var claims = JsonDocument.Parse(Convert.FromBase64String(payload));
            var root = claims.RootElement;
            if (root.TryGetProperty("name", out var n)) result.DisplayName = n.GetString() ?? "";
            if (root.TryGetProperty("preferred_username", out var u)) result.Email = u.GetString() ?? "";
            if (root.TryGetProperty("oid", out var o)) result.ObjectId = o.GetString() ?? "";
        }
        catch
        {
            //----- อ่าน claim ไม่ได้ไม่ถือเป็นความล้มเหลวของการ login
        }
    }
}
