using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// โครงคอลัมน์ของ "ไฟล์ Excel ตัวแทนขายทรัพย์" — แหล่งความจริงที่เดียวของทั้ง 3 ฝั่ง
    ///
    ///   • AgentImport/DownloadTemplate — ไฟล์เปล่าให้กรอก (หัวคอลัมน์อย่างเดียว)
    ///   • AgentList/Export             — ไฟล์ข้อมูลจริง <b>รูปแบบเดียวกันเป๊ะ</b> เอากลับไป Import ต่อได้ทันที
    ///   • AgentImport/Import           — อ่านค่าจากไฟล์ตาม "ลำดับคอลัมน์" (index) ไม่ใช่ชื่อหัวตาราง
    ///
    /// ⚠️ ลำดับในลิสต์คือลำดับคอลัมน์ในไฟล์ — ห้ามสลับหรือแทรกกลาง ถ้าจะเพิ่มให้ต่อท้ายบล็อกของตัวเอง
    ///    และแก้ index ที่ Import อ่าน (AgentImportController.Import) ให้ตรงกันเสมอ
    ///
    /// ข้อมูล 1 แถว = ตัวแทน 1 ราย (web_agent) + สมาชิกที่ผูกอยู่ (web_member) โดยจับคู่ด้วย
    /// <c>web_agent.uid = web_member.id</c> — ฝั่ง Export จึงดึงช่องสมาชิกด้วย scalar subquery
    /// (แทน JOIN เพราะ SQL ของหน้า list ที่ Export ใช้ต่อเป็น "select * from web_agent ..." ตายตัว)
    /// </summary>
    public static class AgentImportSchema
    {
        /// <summary>1 คอลัมน์ในไฟล์: หัวตาราง + เป็นช่องของสมาชิกหรือตัวแทน (ใช้เลือกสีหัว) + นิพจน์ SQL ตอน Export</summary>
        public readonly record struct Column(string Header, bool IsMember, string Sql);

        /// <summary>คอลัมน์ "รหัส NPA 1..N" อยู่ขวาสุดของไฟล์เสมอ — เริ่มที่คอลัมน์นี้ (9 ช่องสมาชิก + 22 ช่องตัวแทน = 31)</summary>
        public const int NpaStartColumn = 32;

        /// <summary>จำนวนช่องรหัสทรัพย์ — มาจาก <see cref="AdminMenu.NpaSlotCount"/> ที่เดียว</summary>
        public static int NpaSlotCount => AdminMenu.NpaSlotCount;

        /// <summary>จำนวนคอลัมน์ทั้งไฟล์</summary>
        public static int TotalColumns => NpaStartColumn - 1 + NpaSlotCount;

        // ช่องของสมาชิก: ดึงจาก web_member ที่ id ตรงกับ web_agent.uid
        // (ต้องเขียน web_agent.uid แบบเต็ม — ถ้าใช้ uid เฉย ๆ จะไปชนคอลัมน์ชื่อเดียวกันใน web_member ได้)
        private static string M(string col)  => $"(SELECT m.{col} FROM web_member m WHERE m.id = web_agent.uid)";
        // ช่องตัวเลข → cast เป็น text ตั้งแต่ใน SQL เพื่อให้ Excel เขียนเป็น "ข้อความ" ทุกช่อง
        private static string Mi(string col) => $"(SELECT m.{col}::text FROM web_member m WHERE m.id = web_agent.uid)";
        private static string Ai(string col) => $"{col}::text";

        private static readonly System.Drawing.Color MemberColor = System.Drawing.Color.FromArgb(31, 78, 121);
        private static readonly System.Drawing.Color AgentColor   = System.Drawing.Color.FromArgb(56, 106, 40);

        public static readonly IReadOnlyList<Column> Columns = BuildColumns();

        private static List<Column> BuildColumns()
        {
            var list = new List<Column>
            {
                // ── ช่องสมาชิก (web_member) 1–9 ──
                // ประเภทสมาชิก: ค่าเดียวกับ radio "type" ของหน้าสมัคร /th/register (1=นิติบุคคล, 2=บุคคลธรรมดา)
                new("ประเภทสมาชิก * (1=นิติบุคคล, 2=บุคคลธรรมดา)", true, Mi("type")),
                new("คำนำหน้า (สมาชิก)",       true, M("title")),
                new("คำนำหน้า อื่นๆ (สมาชิก)",  true, M("titleoth")),
                new("ชื่อ (สมาชิก)",           true, M("firstname")),
                new("นามสกุล (สมาชิก)",        true, M("surname")),
                new("Username *",              true, M("username")),
                new("อีเมล *",                 true, M("email")),
                new("มือถือ * (ตัวเลข 10 หลัก ขึ้นต้น 0)", true, M("mobile")),
                new("LINE UUID",               true, M("lineuid")),

                // ── ช่องตัวแทน (web_agent) 10–31 ──
                new("คำนำหน้า ตัวแทน * (1=นาย 2=นาง 3=นางสาว 4=อื่นๆ)", false, Ai("title")),
                new("ชื่อ (ตัวแทน)",    false, "name"),
                new("นามสกุล (ตัวแทน)", false, "surname"),
                new("เลขบัตรประชาชน/เลขผู้เสียภาษี * (ตัวเลข 13 หลัก)", false, "idcard"),
                new("ที่อยู่ตามทะเบียนบ้าน", false, "cus_base_addr"),
                new("หมู่ที่ (ทะเบียนบ้าน)",  false, "cus_base_moo"),
                new("ซอย (ทะเบียนบ้าน)",     false, "cus_base_soi"),
                new("ถนน (ทะเบียนบ้าน)",     false, "cus_base_road"),
                // ที่อยู่: ระบบเก็บเป็นรหัส Master Data ของ web_data_* เหมือนหน้าสมัคร /th/apply-agent/step2
                //          → ต้องกรอก "รหัสตัวเลข" เท่านั้น กรอกเป็นชื่อจะถูกข้ามทั้งแถว
                new("รหัสตำบล/แขวง (ทะเบียนบ้าน) [ตัวเลข 6 หลัก]", false, "cus_base_tambol"),
                new("รหัสอำเภอ/เขต (ทะเบียนบ้าน) [ตัวเลข 4 หลัก]", false, "cus_base_amphur"),
                new("รหัสจังหวัด (ทะเบียนบ้าน) [ตัวเลข 2 หลัก]",   false, "cus_base_province"),
                new("รหัสไปรษณีย์ (ทะเบียนบ้าน) [5 หลัก]",        false, "cus_base_zipcode"),
                new("ที่อยู่ติดต่อ",          false, "cus_contact_arrr"),
                new("หมู่ที่ (ที่อยู่ติดต่อ)", false, "cus_contact_moo"),
                new("ซอย (ที่อยู่ติดต่อ)",    false, "cus_contact_soi"),
                new("ถนน (ที่อยู่ติดต่อ)",    false, "cus_contact_road"),
                new("รหัสตำบล/แขวง (ที่อยู่ติดต่อ) [ตัวเลข 6 หลัก]", false, "cus_contact_tambol"),
                new("รหัสอำเภอ/เขต (ที่อยู่ติดต่อ) [ตัวเลข 4 หลัก]", false, "cus_contact_amphur"),
                new("รหัสจังหวัด (ที่อยู่ติดต่อ) [ตัวเลข 2 หลัก]",   false, "cus_contact_province"),
                new("รหัสไปรษณีย์ (ที่อยู่ติดต่อ) [5 หลัก]",        false, "cus_contact_zipcode"),
                new("โทรศัพท์บ้าน (ตัวเลข 9-10 หลัก)",   false, "cus_base_phone"),
                new("โทรศัพท์ติดต่อ (ตัวเลข 9-10 หลัก)", false, "cus_contact_phone"),
            };

            // ── ช่องรหัสทรัพย์ NPA (ขวาสุดของไฟล์) 32.. ──
            list.AddRange(Enumerable.Range(1, NpaSlotCount)
                .Select(i => new Column($"รหัส NPA {i}", false, "npaid" + i)));

            return list;
        }

        /// <summary>ตำแหน่งคอลัมน์ (index เริ่ม 0) ของช่องที่ระบุด้วยนิพจน์ SQL — ใช้แทนการเขียนเลขคอลัมน์ตายตัว</summary>
        public static int IndexOf(string sql)
        {
            for (int i = 0; i < Columns.Count; i++)
                if (Columns[i].Sql == sql) return i;
            throw new ArgumentOutOfRangeException(nameof(sql), $"ไม่พบคอลัมน์ '{sql}' ใน AgentImportSchema");
        }

        /// <summary>เขียนแถวหัวตาราง (แถว 1) พร้อมสีประจำกลุ่ม — ให้ template กับไฟล์ export หน้าตาเหมือนกัน</summary>
        public static void WriteHeader(ExcelWorksheet sheet)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                var cell = sheet.Cells[1, i + 1];
                cell.Value = Columns[i].Header;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Columns[i].IsMember ? MemberColor : AgentColor);
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }
        }

        /// <summary>
        /// ตั้งทั้งคอลัมน์เป็นรูปแบบ "ข้อความ" — กัน Excel แปลง "0812345678" เป็นตัวเลข 812345678 (ศูนย์หน้าหาย)
        /// ตั้งที่ระดับคอลัมน์ ไม่ใช่ช่วงเซลล์ เพื่อไม่ให้ขอบเขตข้อมูล (Dimension) ของชีตบวมเป็นพันแถว
        /// </summary>
        public static void SetTextFormat(ExcelWorksheet sheet)
        {
            for (int i = 1; i <= Columns.Count; i++)
                sheet.Column(i).Style.Numberformat.Format = "@";
        }
    }
}
