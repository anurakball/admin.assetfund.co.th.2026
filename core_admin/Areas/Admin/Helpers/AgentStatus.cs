namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// สถานะการตรวจสอบผู้สมัครตัวแทน — คอลัมน์ <c>web_agent.approved</c> (integer)
    ///
    /// ⚠️ นี่คือ "แหล่งความจริงเดียว" ของสถานะนี้ทั้งระบบหลังบ้าน
    ///    (หน้า list / หน้า Detail / หน้า Edit / ไฟล์ export ต้องอ่านจากที่นี่เท่านั้น
    ///     ไม่งั้นแต่ละหน้าจะใช้ตัวเลขคนละชุดแล้วสถานะไม่ตรงกันเวลาเปิดดูซ้ำ)
    ///
    /// ค่าที่ใช้จริง:
    ///   0 = กำลังตรวจสอบ (ค่าเริ่มต้นตอน front-end บันทึกใบสมัคร)
    ///   1 = ผ่าน        → ส่งอีเมลยินดีต้อนรับ + ปรับ web_member.member_type = 3 (Agency)
    ///   3 = ไม่ผ่าน      → ส่งอีเมลแจ้งผลไม่ผ่าน + member_type กลับเป็น 1
    ///   4 = Black List  → ไม่ส่งอีเมลใด ๆ + member_type กลับเป็น 1 (ล็อกอินได้ แต่สมัครตัวแทนไม่ได้)
    ///
    /// ค่า 2 = ค่าเดิมของ "อยู่ระหว่างตรวจสอบ" ที่หน้า Edit เคยบันทึกไว้ — ถือเป็น "กำลังตรวจสอบ" เหมือน 0
    /// (Normalize() แปลงให้อัตโนมัติ จึงไม่ต้องไล่แก้ข้อมูลเก่าใน DB)
    ///
    /// ฝั่ง front-end (d:\Project\sam.or.th) ใช้เลขชุดเดียวกัน — ดู Helpers/Utility.cs (UHelper.AgentBlackList)
    /// ถ้าเพิ่ม/เปลี่ยนรหัสสถานะที่นี่ ต้องไปแก้ฝั่งนั้นด้วยเสมอ
    /// </summary>
    public static class AgentStatus
    {
        public const int Pending = 0;    // กำลังตรวจสอบ
        public const int Approved = 1;   // ผ่าน
        public const int Rejected = 3;   // ไม่ผ่าน
        public const int BlackList = 4;  // Black List

        /// ค่าเดิมของ "อยู่ระหว่างตรวจสอบ" (หน้า Edit รุ่นก่อนบันทึกเลขนี้) — ยังเจอในข้อมูลเก่า
        public const int LegacyPending = 2;

        /// ตัวเลือกทั้งหมดที่แอดมินเลือกได้ (ลำดับนี้คือลำดับที่แสดงในหน้าเว็บ)
        public static readonly IReadOnlyList<KeyValuePair<int, string>> Options = new List<KeyValuePair<int, string>>
        {
            new (Pending,   "กำลังตรวจสอบ"),
            new (Approved,  "ผ่าน"),
            new (Rejected,  "ไม่ผ่าน"),
            new (BlackList, "Black List"),
        };

        /// แปลงค่าดิบจาก DB/ฟอร์ม → รหัสสถานะที่รองรับ (ค่าที่ไม่รู้จักและ 2 = กำลังตรวจสอบ)
        public static int Normalize(object? raw)
        {
            if (raw == null || raw == DBNull.Value) return Pending;
            if (!int.TryParse(raw.ToString()?.Trim(), out int code)) return Pending;
            return IsValid(code) ? code : Pending;
        }

        /// true เมื่อเป็นรหัสที่ระบบรองรับ (ใช้ตรวจค่าที่ส่งมาจากฟอร์ม/ajax ก่อนบันทึก)
        public static bool IsValid(int code) =>
            code == Pending || code == Approved || code == Rejected || code == BlackList;

        /// ข้อความภาษาไทยของสถานะ (ใช้ทุกหน้าให้ตรงกัน)
        public static string Label(object? raw)
        {
            int code = Normalize(raw);
            foreach (var o in Options)
                if (o.Key == code) return o.Value;
            return Options[0].Value;
        }

        /// class ของ badge สำหรับหน้า list (bootstrap)
        public static string BadgeClass(object? raw) => Normalize(raw) switch
        {
            Approved => "bg-success",
            Rejected => "bg-danger",
            BlackList => "bg-dark",
            _ => "bg-secondary",
        };

        /// นิพจน์ SQL แปลง approved → ข้อความ (ใช้ในไฟล์ export เพื่อไม่ให้ออกมาเป็นตัวเลขดิบ)
        public static string SqlCaseExpr(string column = "approved") =>
            $"CASE {column} WHEN {Approved} THEN '{Options[1].Value}'"
            + $" WHEN {Rejected} THEN '{Options[2].Value}'"
            + $" WHEN {BlackList} THEN '{Options[3].Value}'"
            + $" ELSE '{Options[0].Value}' END";
    }
}
