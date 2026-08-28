namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    // ตัวช่วยของเมนู Logs/คำค้นหาทรัพย์ NPA (ตาราง api_npa_search_log)
    //
    // คอลัมน์ filters เป็น jsonb ที่ front-end เก็บพารามิเตอร์ตัวกรองของการค้นหาไว้
    // (HomeController.LogNpaSearch ฝั่ง d:\Project\sam.or.th) — ที่นี่เก็บเฉพาะ "ป้ายภาษาไทย"
    // ของคีย์เหล่านั้นไว้ใช้ร่วมกันทั้งหน้า list, modal รายละเอียด และไฟล์ Excel
    //
    // ⚠ ถ้าฝั่ง front-end เพิ่ม/ตัดคีย์ใน filterKeys ให้ปรับรายการนี้ตาม
    //   (คีย์ที่ไม่รู้จักยังแสดงได้ปกติ — Label() จะคืนชื่อคีย์ดิบกลับไป)
    public static class NpaSearchLogHelper
    {
        public static readonly List<KeyValuePair<string, string>> FilterKeys = new()
        {
            new("types",        "ประเภททรัพย์"),
            new("provinces",    "จังหวัด"),
            new("district",     "อำเภอ/เขต"),
            new("minPrice",     "ราคาต่ำสุด"),
            new("maxPrice",     "ราคาสูงสุด"),
            new("minArea",      "พื้นที่ใช้สอยต่ำสุด"),
            new("maxArea",      "พื้นที่ใช้สอยสูงสุด"),
            new("minLand",      "เนื้อที่ดินต่ำสุด"),
            new("maxLand",      "เนื้อที่ดินสูงสุด"),
            new("bedroom",      "ห้องนอน"),
            new("restroom",     "ห้องน้ำ"),
            new("status",       "สถานะทรัพย์"),
            new("zones",        "ย่าน"),
            new("nearby",       "สถานที่ใกล้เคียง"),
            new("promotion_id", "โปรโมชัน"),
            new("is_highlight", "ทรัพย์ไฮไลต์"),
            new("is_recommend", "ทรัพย์แนะนำ")
        };

        // ชื่อภาษาไทยของคีย์ตัวกรอง — ไม่รู้จักก็คืนคีย์เดิม
        public static string Label(string key)
        {
            var found = FilterKeys.FirstOrDefault(c => c.Key == key);
            return string.IsNullOrEmpty(found.Key) ? key : found.Value;
        }

        // แปลง filters (jsonb) เป็นคู่ (ป้ายไทย, ค่า) เรียงตามลำดับใน FilterKeys
        // คืนลิสต์ว่างเมื่อ json ว่าง/ผิดรูป — หน้าจอจะได้ไม่พังเพราะ log แถวเดียว
        public static List<KeyValuePair<string, string>> Parse(string? filtersJson)
        {
            var result = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(filtersJson) || filtersJson.Trim() == "{}" || filtersJson.Trim() == "null")
                return result;

            try
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(filtersJson);
                foreach (var kv in FilterKeys)
                {
                    var v = obj[kv.Key]?.ToString();
                    if (!string.IsNullOrWhiteSpace(v)) result.Add(new(kv.Value, v));
                }
                //----- คีย์ที่ไม่อยู่ในทะเบียน (front-end เพิ่มใหม่) ต่อท้ายด้วยชื่อคีย์ดิบ
                foreach (var p in obj.Properties())
                {
                    if (FilterKeys.Any(c => c.Key == p.Name)) continue;
                    var v = p.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(v)) result.Add(new(p.Name, v));
                }
            }
            catch { }

            return result;
        }

        // ข้อความสรุปตัวกรองแบบบรรทัดเดียว ("จังหวัด: 10, ราคาสูงสุด: 99990000") ใช้ในไฟล์ Excel
        public static string Summary(string? filtersJson)
        {
            return string.Join(", ", Parse(filtersJson).Select(c => c.Key + ": " + c.Value));
        }

        // ชื่อ query string ของช่องค้นหาทั้งหมดในหน้านี้ (เรียงตามที่แสดงบนฟอร์ม)
        // ใช้ทั้งตอนประกอบลิงก์ pagination และตอนส่งต่อไปหน้า ExportExcel
        public static readonly string[] QueryKeys =
        {
            "q_kw", "q_haskw", "q_mode", "q_lang", "q_result", "q_fkey", "q_ip", "q_date_start", "q_date_end"
        };

        // ประกอบเงื่อนไข WHERE จากค่าค้นหาบน query string — ใช้ร่วมกันระหว่างหน้า list กับ ExportExcel
        // เพื่อให้ผลลัพธ์ที่ export ตรงกับที่เห็นบนหน้าจอเสมอ (alias ตารางคือ l)
        public static string BuildWhere(Func<string, string> q, Dictionary<string, object> prms)
        {
            var where = new System.Text.StringBuilder("WHERE 1=1");

            var kw = q("q_kw");
            if (!string.IsNullOrEmpty(kw)) { where.Append(" AND l.keyword ILIKE @p_kw"); prms["p_kw"] = "%" + kw + "%"; }

            var hasKw = q("q_haskw");
            if (hasKw == "1")      where.Append(" AND l.keyword IS NOT NULL AND btrim(l.keyword) <> ''");
            else if (hasKw == "0") where.Append(" AND (l.keyword IS NULL OR btrim(l.keyword) = '')");

            var mode = q("q_mode");
            if (mode == "__none__")            where.Append(" AND (l.search_mode IS NULL OR l.search_mode = '')");
            else if (!string.IsNullOrEmpty(mode)) { where.Append(" AND l.search_mode = @p_mode"); prms["p_mode"] = mode; }

            var lang = q("q_lang");
            if (!string.IsNullOrEmpty(lang)) { where.Append(" AND l.lang = @p_lang"); prms["p_lang"] = lang; }

            var result = q("q_result");
            if (result == "found")         where.Append(" AND COALESCE(l.result_count, 0) > 0");
            else if (result == "notfound") where.Append(" AND COALESCE(l.result_count, 0) = 0");

            var fkey = q("q_fkey");
            if (fkey == "__any__")             where.Append(" AND l.filters IS NOT NULL AND l.filters <> '{}'::jsonb");
            else if (fkey == "__none__")       where.Append(" AND (l.filters IS NULL OR l.filters = '{}'::jsonb)");
            else if (!string.IsNullOrEmpty(fkey)) { where.Append(" AND jsonb_exists(l.filters, @p_fkey)"); prms["p_fkey"] = fkey; }

            var ip = q("q_ip");
            if (!string.IsNullOrEmpty(ip)) { where.Append(" AND l.client_ip ILIKE @p_ip"); prms["p_ip"] = "%" + ip + "%"; }

            var ds = q("q_date_start");
            if (!string.IsNullOrEmpty(ds))
            {
                try { var p = ds.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])); where.Append(" AND l.created_at >= @p_ds"); prms["p_ds"] = d; } catch { }
            }
            var de = q("q_date_end");
            if (!string.IsNullOrEmpty(de))
            {
                try { var p = de.Split('/'); var d = new DateTime(int.Parse(p[2]), int.Parse(p[1]), int.Parse(p[0])).AddDays(1); where.Append(" AND l.created_at < @p_de"); prms["p_de"] = d; } catch { }
            }

            return where.ToString();
        }
    }
}
