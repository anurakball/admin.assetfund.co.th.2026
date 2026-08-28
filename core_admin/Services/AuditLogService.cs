using Microsoft.Data.SqlClient;
using thaicredit_hr_admin.Areas.Admin.Helpers;

public interface IAuditLogService
{
    void Log(string endpoint, bool success, int? uid = null, string? detail = null, string? ipAddress = null, string? errorCode = null);
}

public class AuditLogService : IAuditLogService
{
    private readonly string _connStr;
    private readonly ILogger<AuditLogService> _logger;

    public AuditLogService(IConfiguration configuration, ILogger<AuditLogService> logger)
    {
        var db = configuration.GetSection("DBConnection");
        _connStr = new SqlConnectionStringBuilder
        {
            DataSource = db["Server"],
            InitialCatalog = db["Database"],
            UserID = db["Username"],
            Password = db["Password"],
            Pooling = true,
            MinPoolSize = 1,
            MaxPoolSize = 5,
            CommandTimeout = 10,
            TrustServerCertificate = true
        }.ConnectionString;
        _logger = logger;
    }

    public void Log(string endpoint, bool success, int? uid = null, string? detail = null, string? ipAddress = null, string? errorCode = null)
    {
        try
        {
            using var conn = new SqlConnection(_connStr);
            conn.Open();
            const string sql = @"
                INSERT INTO [2026_api_audit_log] (endpoint, success, uid, detail, ip_address, error_code)
                VALUES (@endpoint, @success, @uid, @detail, @ip_address, @error_code)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@endpoint", endpoint);
            cmd.Parameters.AddWithValue("@success", success);
            cmd.Parameters.AddWithValue("@uid", (object?)uid ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@detail", (object?)detail ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ip_address", (object?)ipAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@error_code", (object?)errorCode ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write audit log for endpoint {Endpoint}", endpoint);
        }
    }
}
