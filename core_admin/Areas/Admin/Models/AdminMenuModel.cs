namespace thaicredit_hr_admin.Areas.Admin.Models
{
    public class AdminMenuModel
    {
        public string? Title { get; set; }
        public string? Link { get; set; } = "";
        public string? ModuleName { get; set; } = "";
        public string? Icon { get; set; } = "";

        public List<AdminMenuModel>? SubMenu { get; set; }
    }
}
