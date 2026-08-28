using System.Text.RegularExpressions;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    // ────────────────────────────────────────────────────────────────────────────
    // EFormSubmitHelper — ตัวช่วยตีความข้อมูลที่ผู้ใช้กรอกผ่านแบบฟอร์ม (web_eform_submit)
    //
    // รูปแบบจัดเก็บ (จากฝั่ง front-end): info = ป้ายกำกับ (label) ของแต่ละช่อง คั่นด้วย '|'
    //                                    en_info = ค่าที่กรอก คั่นด้วย '|' (align 1:1 กับ info)
    // ไฟล์แนบ: ค่าเป็นชื่อไฟล์รูปแบบ ef{formId}_{guid}.ext (หลายไฟล์คั่นด้วย ", ")
    //
    // ใช้ร่วมกันทั้งหน้า List (สรุปย่อ), Detail (ตารางเต็ม) และ Export Excel (pivot รายคอลัมน์)
    // ────────────────────────────────────────────────────────────────────────────
    public static class EFormSubmitHelper
    {
        // ชื่อไฟล์ที่ระบบ E-Form บันทึก: ef{formId}_{32 hex}.ext
        private static readonly Regex FileNameRe =
            new(@"^ef\d+_[0-9a-fA-F]{32}\.[A-Za-z0-9]+$", RegexOptions.Compiled);
        private static readonly Regex TagRe = new("<.*?>", RegexOptions.Compiled | RegexOptions.Singleline);

        public sealed record Pair(string Label, string Value)
        {
            public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
        }

        // แยก info|en_info เป็นคู่ (label, value) — ตัด pair ที่ label ว่างทิ้ง (กันช่องขยะจากตัวคั่น)
        public static List<Pair> ParsePairs(string? info, string? enInfo)
        {
            var list = new List<Pair>();
            if (string.IsNullOrEmpty(info)) return list;
            var labels = info.Split('|');
            var values = (enInfo ?? "").Split('|');
            for (int i = 0; i < labels.Length; i++)
            {
                string lbl = StripHtml(labels[i]).Trim();
                if (lbl == "") continue;
                string val = (i < values.Length ? values[i] : "") ?? "";
                list.Add(new Pair(lbl, val.Trim()));
            }
            return list;
        }

        public static bool IsFileValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            var tokens = value.Split(new[] { ", " }, StringSplitOptions.None);
            return tokens.Length > 0 && tokens.All(t => FileNameRe.IsMatch(t.Trim()));
        }

        public static IEnumerable<string> FileTokens(string value) =>
            value.Split(new[] { ", " }, StringSplitOptions.None).Select(t => t.Trim()).Where(t => t != "");

        // ตัดแท็ก HTML ออกจากค่า (ค่าที่ผู้ใช้กรอกอาจมี <b>..</b> จาก label ของฟอร์ม)
        public static string StripHtml(string? s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Net.WebUtility.HtmlDecode(TagRe.Replace(s, "")).Trim();
        }

        // สรุปย่อสำหรับหน้า List: หยิบเฉพาะช่องที่มีค่า มาแสดงแบบ "label: value" ไม่กี่ช่องแรก
        //   maxPairs = จำนวนช่องสูงสุดที่จะโชว์, maxLen = ความยาวสูงสุดของสตริงรวม
        public static string Summary(string? info, string? enInfo, int maxPairs = 3, int maxLen = 90)
        {
            var pairs = ParsePairs(info, enInfo).Where(p => !p.IsEmpty).ToList();
            if (pairs.Count == 0) return "";
            var parts = new List<string>();
            foreach (var p in pairs.Take(maxPairs))
            {
                string val = IsFileValue(p.Value) ? "📎 " + string.Join(", ", FileTokens(p.Value)) : StripHtml(p.Value);
                if (val.Length > 40) val = val.Substring(0, 40) + "…";
                parts.Add($"{p.Label}: {val}");
            }
            string text = string.Join("  •  ", parts);
            if (text.Length > maxLen) text = text.Substring(0, maxLen) + "…";
            if (pairs.Count > maxPairs) text += $"  (+{pairs.Count - maxPairs})";
            return text;
        }

        // จำนวนช่องที่กรอกจริง (มีค่า) — ใช้แสดง badge ในหน้า List
        public static int AnsweredCount(string? info, string? enInfo) =>
            ParsePairs(info, enInfo).Count(p => !p.IsEmpty);

        // แปลงค่าเป็น HTML ปลอดภัย (encode) โดยคง line break ของ textarea ไว้เป็น <br>
        public static string ValueToHtml(string? value)
        {
            string s = StripHtml(value);
            return System.Net.WebUtility.HtmlEncode(s)
                .Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
        }
    }
}
