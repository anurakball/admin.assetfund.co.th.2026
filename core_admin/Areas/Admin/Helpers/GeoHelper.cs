using System.Data;
using System.Text.RegularExpressions;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// ตัวกลางจัดการ "จังหวัด / อำเภอ / ตำบล" ให้ทั้งระบบเก็บเป็น code id อ้างอิงตาราง
    /// web_data_provinces / web_data_districts / web_data_subdistricts (คอลัมน์ code) เหมือนกันหมด
    ///
    ///   • CodeExpr()  — SQL fragment แปลงคอลัมน์ที่เก็บเป็น text ให้เป็น int อย่างปลอดภัย
    ///                   (ข้อมูลเก่าที่เผลอเก็บเป็น "ชื่อ" จะได้ NULL แทนที่จะ error ทั้งหน้า)
    ///   • NameExpr()  — SQL fragment คืน "ชื่อไทย" จาก code (ใช้ใน Export Excel)
    ///   • *Name()     — แปลง code → ชื่อ ตอน render (หน้า View/Detail)
    ///   • To*Code()   — แปลง "ชื่อ หรือ code" → code (ใช้ตอน Import Excel)
    ///
    /// ⚠️ ค่าใน DB เป็น varchar → ห้าม cast ตรง ๆ ด้วย @code::int เพราะข้อมูลเก่าที่เป็นชื่อไทย
    ///    จะทำให้ PostgreSQL โยน invalid input syntax for type integer แล้วหน้า admin พังทั้งหน้า
    /// </summary>
    public static class GeoHelper
    {
        public const string ProvinceTable    = "web_data_provinces";
        public const string DistrictTable    = "web_data_districts";
        public const string SubdistrictTable = "web_data_subdistricts";

        // คำนำหน้าที่คนกรอกใน Excel มักติดมาด้วย — ตัดทิ้งก่อนจับคู่ชื่อ
        private static readonly Regex PrefixRx = new(
            @"^(จังหวัด|จ\.|อำเภอ|อ\.|เขต|ตำบล|ต\.|แขวง)\s*", RegexOptions.Compiled);
        private static readonly Regex DigitsRx = new(@"^\d+$", RegexOptions.Compiled);

        /// <summary>ค่านี้เป็น code (ตัวเลขล้วน) หรือไม่</summary>
        public static bool IsCode(string? value) =>
            !string.IsNullOrWhiteSpace(value) && DigitsRx.IsMatch(value.Trim());

        /// <summary>ตัดคำนำหน้า/ช่องว่าง/ไม้ยมก เพื่อเทียบชื่อแบบหลวม ๆ ("อ. เมือง" = "เมือง")</summary>
        public static string Normalize(string? value)
        {
            var s = (value ?? "").Trim();
            if (s.Length == 0) return "";
            s = PrefixRx.Replace(s, "");
            s = s.Replace(" ", "").Replace(" ", "").Replace("ฯ", "");
            // "กรุงเทพ" / "กรุงเทพมหานคร" / "กทม" → ชื่อเดียวกัน
            if (s.StartsWith("กรุงเทพ") || s == "กทม") return "กรุงเทพมหานคร";
            return s;
        }

        /// <summary>
        /// SQL: แปลงคอลัมน์ text ที่เก็บ code → int (ไม่ใช่ตัวเลข = NULL)
        /// ใช้แทนการเขียน @code::int ตรง ๆ
        /// </summary>
        public static string CodeExpr(string column) =>
            $"NULLIF(regexp_replace(COALESCE({column}, ''), '[^0-9]', '', 'g'), '')::int";

        /// <summary>
        /// SQL: คืน "ชื่อไทย (code)" ของคอลัมน์ที่เก็บ code — ใช้ใน SELECT ของ Export Excel
        /// ค่าที่หา code ไม่เจอ (เช่นข้อมูลเก่าที่เป็นชื่อ) คืนค่าเดิมกลับไปเพื่อไม่ให้ข้อมูลหาย
        /// </summary>
        public static string NameExpr(string table, string column) =>
            $@"COALESCE((SELECT g.name_in_thai FROM {table} g WHERE g.code = {CodeExpr(column)}),
                        NULLIF(btrim(COALESCE({column}, '')), ''))";

        // ── code → ชื่อ (ตอน render หน้า View/Detail) ────────────────────────────
        private static string NameByCode(DBHelper db, string table, string? value)
        {
            var code = (value ?? "").Trim();
            if (!IsCode(code)) return "";
            try
            {
                var dt = db.ExecuteQuery(
                    $"select name_in_thai from {table} where code = @code limit 1",
                    new Dictionary<string, object> { { "code", int.Parse(code) } });
                return dt.Rows.Count > 0 ? (dt.Rows[0]["name_in_thai"]?.ToString() ?? "") : "";
            }
            catch { return ""; }
        }

        public static string ProvinceName(DBHelper db, string? value)    => NameByCode(db, ProvinceTable, value);
        public static string DistrictName(DBHelper db, string? value)    => NameByCode(db, DistrictTable, value);
        public static string SubdistrictName(DBHelper db, string? value) => NameByCode(db, SubdistrictTable, value);

        /// <summary>ข้อความแสดงผล: "เชียงใหม่ (50)" — ไม่พบ code คืนค่าดิบที่เก็บไว้</summary>
        public static string Display(DBHelper db, string table, string? value)
        {
            var raw  = (value ?? "").Trim();
            if (raw.Length == 0) return "";
            var name = NameByCode(db, table, raw);
            return name.Length > 0
                ? $"{name} <span class='text-muted small'>({raw})</span>"
                : System.Net.WebUtility.HtmlEncode(raw);
        }

        // ── ชื่อ/code → code (ตอน Import Excel) ─────────────────────────────────
        // คืน "" เมื่อหาไม่เจอ เพื่อให้ผู้เรียกแจ้ง error รายแถวได้

        /// <summary>จังหวัด: รับได้ทั้ง code ("50") และชื่อ ("เชียงใหม่", "จ.เชียงใหม่")</summary>
        public static string ToProvinceCode(DBHelper db, string? value)
        {
            var raw = (value ?? "").Trim();
            if (raw.Length == 0) return "";
            if (IsCode(raw))
                return Exists(db, ProvinceTable, raw) ? raw : "";

            return LookupByName(db, ProvinceTable, Normalize(raw), null, null);
        }

        /// <summary>อำเภอ/เขต: จำกัดขอบเขตด้วยรหัสจังหวัดที่ resolve ได้แล้ว (ชื่ออำเภอซ้ำข้ามจังหวัดได้)</summary>
        public static string ToDistrictCode(DBHelper db, string? value, string provinceCode)
        {
            var raw = (value ?? "").Trim();
            if (raw.Length == 0) return "";
            if (IsCode(raw))
                return Exists(db, DistrictTable, raw) ? raw : "";

            return LookupByName(db, DistrictTable, Normalize(raw),
                $"d.province_id = (SELECT id FROM {ProvinceTable} WHERE code = @parent)", provinceCode);
        }

        /// <summary>ตำบล/แขวง: จำกัดขอบเขตด้วยรหัสอำเภอที่ resolve ได้แล้ว</summary>
        public static string ToSubdistrictCode(DBHelper db, string? value, string districtCode)
        {
            var raw = (value ?? "").Trim();
            if (raw.Length == 0) return "";
            if (IsCode(raw))
                return Exists(db, SubdistrictTable, raw) ? raw : "";

            return LookupByName(db, SubdistrictTable, Normalize(raw),
                $"d.district_id = (SELECT id FROM {DistrictTable} WHERE code = @parent)", districtCode);
        }

        /// <summary>รหัสไปรษณีย์ตามตำบล — ใช้เติมให้อัตโนมัติเมื่อไฟล์ Import ไม่ได้กรอกมา</summary>
        public static string ZipBySubdistrictCode(DBHelper db, string? subdistrictCode)
        {
            var code = (subdistrictCode ?? "").Trim();
            if (!IsCode(code)) return "";
            try
            {
                var dt = db.ExecuteQuery(
                    $"select zip_code from {SubdistrictTable} where code = @code limit 1",
                    new Dictionary<string, object> { { "code", int.Parse(code) } });
                return dt.Rows.Count > 0 ? (dt.Rows[0]["zip_code"]?.ToString() ?? "") : "";
            }
            catch { return ""; }
        }

        /// <summary>
        /// ตัวแปลง "ค่าที่อยู่ที่เก็บใน DB" → รหัส Master Data แบบทำงานในหน่วยความจำ
        ///
        /// ใช้ตอน Export Excel ที่ต้องแปลงหลายพันแถว — โหลดตารางอ้างอิงทั้ง 3 ระดับ "ครั้งเดียว"
        /// แทนการเรียก <see cref="ToProvinceCode"/> รายแถว (ซึ่งยิง query 6 ครั้งต่อแถว)
        /// กฎการจับคู่เหมือน To*Code() ทุกประการ (รวมการตัดคำนำหน้าด้วย <see cref="Normalize"/>)
        ///
        /// ต่างกันที่ "ค่าที่แปลงไม่ได้จะคืนค่าเดิม" ไม่ใช่คืนค่าว่าง — ไฟล์ export ต้องไม่ทำข้อมูลหาย
        /// ปล่อยให้ฝั่ง Import เป็นคนรายงานว่าแถวนั้นกรอกที่อยู่ไม่ถูกต้อง
        /// </summary>
        public sealed class CodeResolver
        {
            private readonly HashSet<string> _provinceCodes = new();
            private readonly HashSet<string> _districtCodes = new();
            private readonly HashSet<string> _subdistrictCodes = new();
            // ชื่อ (normalize แล้ว) → code  ; ระดับอำเภอ/ตำบลผูกกับรหัสแม่ด้วย เพราะชื่อซ้ำข้ามพื้นที่ได้
            private readonly Dictionary<string, string> _provinceByName = new();
            private readonly Dictionary<(string parent, string name), string> _districtByName = new();
            private readonly Dictionary<(string parent, string name), string> _subdistrictByName = new();

            public CodeResolver(DBHelper db)
            {
                foreach (DataRow r in db.ExecuteQuery(
                    $"select code, name_in_thai from {ProvinceTable}").Rows)
                {
                    string code = r["code"]?.ToString() ?? "";
                    if (code.Length == 0) continue;
                    _provinceCodes.Add(code);
                    _provinceByName.TryAdd(Normalize(r["name_in_thai"]?.ToString()), code);
                }

                foreach (DataRow r in db.ExecuteQuery(
                    $@"select d.code, d.name_in_thai, p.code as parent
                       from {DistrictTable} d join {ProvinceTable} p on p.id = d.province_id").Rows)
                {
                    string code = r["code"]?.ToString() ?? "";
                    if (code.Length == 0) continue;
                    _districtCodes.Add(code);
                    _districtByName.TryAdd((r["parent"]?.ToString() ?? "", Normalize(r["name_in_thai"]?.ToString())), code);
                }

                foreach (DataRow r in db.ExecuteQuery(
                    $@"select s.code, s.name_in_thai, d.code as parent
                       from {SubdistrictTable} s join {DistrictTable} d on d.id = s.district_id").Rows)
                {
                    string code = r["code"]?.ToString() ?? "";
                    if (code.Length == 0) continue;
                    _subdistrictCodes.Add(code);
                    _subdistrictByName.TryAdd((r["parent"]?.ToString() ?? "", Normalize(r["name_in_thai"]?.ToString())), code);
                }
            }

            public string Province(string? value) =>
                Resolve(value, _provinceCodes, _provinceByName, "");

            public string District(string? value, string provinceCode) =>
                Resolve(value, _districtCodes, _districtByName, provinceCode);

            public string Subdistrict(string? value, string districtCode) =>
                Resolve(value, _subdistrictCodes, _subdistrictByName, districtCode);

            private static string Resolve(string? value, HashSet<string> codes,
                Dictionary<string, string> byName, string _)
            {
                var raw = (value ?? "").Trim();
                if (raw.Length == 0) return "";
                if (codes.Contains(raw)) return raw;
                return byName.TryGetValue(Normalize(raw), out var code) ? code : raw;
            }

            private static string Resolve(string? value, HashSet<string> codes,
                Dictionary<(string, string), string> byName, string parentCode)
            {
                var raw = (value ?? "").Trim();
                if (raw.Length == 0) return "";
                if (codes.Contains(raw)) return raw;
                if (!IsCode(parentCode)) return raw;   // ไม่รู้รหัสแม่ = ไม่เดา คืนค่าเดิม
                return byName.TryGetValue((parentCode, Normalize(raw)), out var code) ? code : raw;
            }
        }

        private static bool Exists(DBHelper db, string table, string code)
        {
            try
            {
                var dt = db.ExecuteQuery(
                    $"select 1 from {table} where code = @code limit 1",
                    new Dictionary<string, object> { { "code", int.Parse(code) } });
                return dt.Rows.Count > 0;
            }
            catch { return false; }
        }

        // เทียบชื่อแบบตัดคำนำหน้า/ช่องว่างทั้งสองฝั่ง (ฝั่งตารางใช้ regexp_replace ให้ตรงกับ Normalize())
        private static string LookupByName(DBHelper db, string table, string normalized, string? parentWhere, string? parentCode)
        {
            if (normalized.Length == 0) return "";
            try
            {
                var pars = new Dictionary<string, object> { { "name", normalized } };
                string where = "";
                if (!string.IsNullOrEmpty(parentWhere))
                {
                    if (!IsCode(parentCode)) return "";   // ไม่รู้จังหวัด/อำเภอแม่ = ไม่ต้องเดา
                    where = " and " + parentWhere;
                    pars["parent"] = int.Parse(parentCode!.Trim());
                }

                var dt = db.ExecuteQuery(
                    $@"select d.code from {table} d
                       where replace(regexp_replace(d.name_in_thai, '^(จังหวัด|จ\.|อำเภอ|อ\.|เขต|ตำบล|ต\.|แขวง)\s*', ''), ' ', '') = @name
                       {where}
                       order by d.code limit 1", pars);
                return dt.Rows.Count > 0 ? (dt.Rows[0]["code"]?.ToString() ?? "") : "";
            }
            catch { return ""; }
        }
    }
}
