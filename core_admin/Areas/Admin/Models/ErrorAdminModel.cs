namespace thaicredit_hr_admin.Areas.Admin.Models
{
    public class ErrorAdminModel
    {
        public string? ErrorTitle { get; set; }
        public string? ErrorDetail { get; set; }
        public string? ErrorClass { get; set; } = "alert-danger";
    }
}
