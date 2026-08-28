namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// ตัวช่วยเรื่องชื่อตารางใน SQL Server
    ///
    /// ตารางทั้งหมดของระบบนี้ถูกย้ายจาก PostgreSQL <c>asset_fund_temp</c> มาไว้ใน
    /// SQL Server <c>asset_plus_uat</c> ซึ่งมีตารางของระบบเดิมอยู่ก่อนแล้ว 127 ตัว
    /// (มีชื่อชนกันหลายตัว เช่น <c>web_admin</c>) จึงเติม prefix <c>2026_</c> ให้ทุกตาราง
    ///
    /// ชื่อจริงขึ้นต้นด้วยตัวเลข T-SQL จึงบังคับให้ครอบ <c>[ ]</c> เสมอ
    ///
    /// โค้ดส่วนอื่นยังอ้าง "ชื่อตรรกะ" เดิม (เช่น <c>Module.Config.Table = "web_admin"</c>)
    /// เพราะค่านั้นถูกใช้เป็นข้อความใน audit log และใช้เทียบเงื่อนไขในโค้ดด้วย
    /// การแปลงเป็นชื่อจริงจึงทำที่จุดประกอบ SQL เท่านั้น ผ่าน <see cref="T"/>
    /// </summary>
    public static class Db
    {
        public const string Prefix = "2026_";

        /// <summary>ชื่อตรรกะ → ชื่อจริงพร้อมวงเล็บ เช่น <c>web_admin</c> → <c>[2026_web_admin]</c></summary>
        public static string T(string? logicalName)
        {
            var name = (logicalName ?? "").Trim();
            if (name.Length == 0) return name;

            name = name.Trim('[', ']').Trim();          // เผื่อถูกส่งมาแบบครอบวงเล็บแล้ว
            if (!name.StartsWith(Prefix, System.StringComparison.OrdinalIgnoreCase))
                name = Prefix + name;

            return "[" + name + "]";
        }
    }
}
