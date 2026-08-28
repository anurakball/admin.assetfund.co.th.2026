using System.Data;
using System.Text;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    // ตัวช่วยของเมนู "รายงานตัวแทน" (AgentReport)
    //
    // รวมตรรกะการประกอบตัวเลขของรายงานไว้ที่เดียว เพื่อให้หน้าจอ (Index) กับไฟล์ Excel (ExportExcel)
    // ได้ตัวเลขชุดเดียวกันเสมอ
    //
    // แหล่งข้อมูล 2 ฐาน:
    //   - PostgreSQL `web_agent` (+ `web_member` เพื่อดูประเภทสมาชิก) = ตัวตัวแทน
    //   - MySQL `sam_npa.tb_product2` = ทรัพย์ NPA ที่ตัวแทนลงทะเบียนขาย (เก็บเป็นรหัสใน npaid1..npaidN)
    // ทั้งสองฐานคนละเครื่อง join กันใน SQL ไม่ได้ จึงดึงคู่ (ตัวแทน, รหัสทรัพย์) จาก PG แล้วไป lookup
    // รายละเอียดทรัพย์จาก MySQL ทีเดียวด้วย IN (...) แล้วค่อยสรุปยอดในหน่วยความจำ
    public static class AgentReportHelper
    {
        // ชื่อ query string ของช่องค้นหาทั้งหมดในหน้านี้ (ใช้ตอนประกอบลิงก์ปุ่ม Export ด้วย)
        public static readonly string[] QueryKeys = { "q_date_start", "q_date_end", "q_approved", "q_status" };

        //----- ป้ายกำกับกลุ่ม "เพศ/ประเภทผู้สมัคร" เรียงตามลำดับที่ต้องการให้แสดง
        public const string LabelMale     = "ชาย";
        public const string LabelFemale   = "หญิง";
        public const string LabelJuristic = "นิติบุคคล";
        public const string LabelUnknown  = "ไม่ระบุ";
        public static readonly string[] PersonTypeOrder = { LabelMale, LabelJuristic, LabelFemale, LabelUnknown };

        /// <summary>
        /// เงื่อนไข WHERE ของตาราง web_agent (alias = a) จากค่าบน query string
        /// ใช้ร่วมกันทุกคิวรีในรายงาน เพื่อให้ทุกตารางกรองด้วยเงื่อนไขชุดเดียวกัน
        /// </summary>
        public static string BuildWhere(Func<string, string> q, Dictionary<string, object> prms, int webId)
        {
            var where = new StringBuilder("WHERE a.web_id = @p_web_id");
            prms["p_web_id"] = webId;

            //----- สถานะ : รหัสชุดเดียวกับหน้า AgentList (ดู AgentStatus)
            //      0 = กำลังตรวจสอบ (นับค่าเก่า 2 และ null/ค่าแปลกปลอมเข้าพวกนี้ด้วย ให้ตรงกับที่หน้าเว็บแสดง)
            //      1 = ผ่าน, 3 = ไม่ผ่าน, 4 = Black List ; ค่าว่าง = ไม่กรอง
            var approved = q("q_approved");
            if (int.TryParse(approved, out int approvedCode) && AgentStatus.IsValid(approvedCode))
            {
                if (approvedCode == AgentStatus.Pending)
                    where.Append($" AND COALESCE(a.approved, 0) NOT IN ({AgentStatus.Approved},{AgentStatus.Rejected},{AgentStatus.BlackList})");
                else
                    where.Append($" AND COALESCE(a.approved, 0) = {approvedCode}");
            }

            //----- สถานะเปิด/ปิดใช้งาน
            var status = q("q_status");
            if (status == "1")      where.Append(" AND COALESCE(a.status, 0) = 1");
            else if (status == "0") where.Append(" AND COALESCE(a.status, 0) <> 1");

            //----- ช่วงวันที่สมัคร (adddate) รับรูปแบบ dd/mm/yyyy จาก datetimepicker
            var ds = ParseThaiDate(q("q_date_start"));
            if (ds.HasValue) { where.Append(" AND a.adddate >= @p_ds"); prms["p_ds"] = ds.Value; }

            var de = ParseThaiDate(q("q_date_end"));
            if (de.HasValue) { where.Append(" AND a.adddate < @p_de"); prms["p_de"] = de.Value.AddDays(1); }

            return where.ToString();
        }

        // "31/12/2026" -> DateTime ; ค่าว่าง/รูปแบบผิด -> null (ไม่ใส่เงื่อนไข)
        public static DateTime? ParseThaiDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var p = value.Split('/');
            if (p.Length != 3) return null;
            if (!int.TryParse(p[0], out int d) || !int.TryParse(p[1], out int m) || !int.TryParse(p[2], out int y)) return null;
            try { return new DateTime(y, m, d); } catch { return null; }
        }

        /// <summary>
        /// ประกอบข้อมูลรายงานทั้งหมดตามเงื่อนไขที่ได้จาก BuildWhere
        /// </summary>
        public static AgentReportData Build(DBHelper db, string where, Dictionary<string, object> prms)
        {
            var data = new AgentReportData();

            //================= 1) จำนวนตัวแทนแยกตามปีที่สมัคร =================
            var dtYear = db.ExecuteQuery($@"
                SELECT YEAR(a.adddate AT TIME ZONE 'SE Asia Standard Time') AS yr,
                       COUNT(*) AS cnt
                FROM [2026_web_agent] a
                {where}
                GROUP BY YEAR(a.adddate AT TIME ZONE 'SE Asia Standard Time')
                ORDER BY YEAR(a.adddate AT TIME ZONE 'SE Asia Standard Time')", prms);

            foreach (DataRow r in dtYear.Rows)
            {
                data.ByYear.Add(new AgentReportRow
                {
                    Label = r["yr"] == DBNull.Value ? LabelUnknown : Convert.ToInt32(r["yr"]).ToString(),
                    Count = Convert.ToInt64(r["cnt"])
                });
            }
            data.AgentTotal = data.ByYear.Sum(x => x.Count);

            //================= 2) แยกตามเพศ / ประเภทผู้สมัคร =================
            // คำนำหน้า (web_agent.title) 1=นาย 2=นาง 3=นางสาว — ผู้สมัครนิติบุคคลไม่มีคำนำหน้า
            // จึงดูจากประเภทสมาชิก (web_member.type) 1=นิติบุคคล 2=บุคคลธรรมดา แทน
            var dtPerson = db.ExecuteQuery($@"
                SELECT CASE
                         WHEN a.title = 1                  THEN '{LabelMale}'
                         WHEN a.title IN (2, 3)            THEN '{LabelFemale}'
                         WHEN COALESCE(m.type, 0) = 1      THEN '{LabelJuristic}'
                         ELSE '{LabelUnknown}'
                       END AS label,
                       COUNT(*) AS cnt
                FROM [2026_web_agent] a
                LEFT JOIN [2026_web_member] m ON m.id = a.uid
                {where}
                GROUP BY CASE WHEN a.title = 1                  THEN '{LabelMale}'
                              WHEN a.title IN (2, 3)            THEN '{LabelFemale}'
                              WHEN COALESCE(m.type, 0) = 1      THEN '{LabelJuristic}'
                              ELSE '{LabelUnknown}'
                         END", prms);

            var personMap = new Dictionary<string, long>();
            foreach (DataRow r in dtPerson.Rows)
                personMap[r["label"].ToString()!] = Convert.ToInt64(r["cnt"]);

            foreach (var label in PersonTypeOrder)
            {
                if (!personMap.TryGetValue(label, out long cnt)) continue;
                if (cnt == 0) continue;
                data.ByPersonType.Add(new AgentReportRow { Label = label, Count = cnt });
            }

            //================= 3) จำนวนตัวแทนแยกตามจังหวัด =================
            // cus_base_province เก็บได้ 2 แบบ: "รหัสจังหวัด" (สมัครผ่านหน้าเว็บ) หรือ "ชื่อจังหวัด" (นำเข้าจาก Excel)
            // จึงแปลงรหัส -> ชื่อก่อน ถ้าไม่ใช่ตัวเลขให้ใช้ข้อความเดิม
            const string ProvinceExpr = @"
                COALESCE(
                    (SELECT p.name_in_thai FROM [2026_web_data_provinces] p
                      WHERE p.code = TRY_CAST(NULLIF(LTRIM(RTRIM(COALESCE(a.cus_base_province, ''))), '') AS int)),
                    NULLIF(LTRIM(RTRIM(COALESCE(a.cus_base_province, ''))), ''),
                    (SELECT p.name_in_thai FROM [2026_web_data_provinces] p
                      WHERE p.code = TRY_CAST(NULLIF(LTRIM(RTRIM(COALESCE(a.cus_contact_province, ''))), '') AS int)),
                    NULLIF(LTRIM(RTRIM(COALESCE(a.cus_contact_province, ''))), ''),
                    'ไม่ระบุ')";

            var dtAgentProvince = db.ExecuteQuery($@"
                SELECT {ProvinceExpr} AS province, COUNT(*) AS cnt
                FROM [2026_web_agent] a
                {where}
                GROUP BY {ProvinceExpr}
                ORDER BY cnt DESC, province", prms);

            foreach (DataRow r in dtAgentProvince.Rows)
                data.AgentByProvince.Add(new AgentReportRow { Label = r["province"].ToString()!, Count = Convert.ToInt64(r["cnt"]) });

            //================= 4-6) ทรัพย์ NPA ที่ตัวแทนลงทะเบียนขาย =================
            // 4.1 : กาง npaid1..npaidN ออกเป็นคู่ (ตัวแทน, ค่าที่แจ้ง)
            //       ค่าที่แจ้งเป็นได้ 2 แบบ: "รหัสทรัพย์" (ตรงกับ tb_product2.product_code) หรือ
            //       "ข้อความอิสระ" ที่ผู้สมัครพิมพ์เอง (ฟอร์มฝั่ง front-end เปิดให้พิมพ์เองได้)
            //       ห้าม upper() ทั้งก้อนเหมือนเดิม เพราะจะทำให้ข้อความอิสระเพี้ยน — ตัดซ้ำแบบไม่สนตัวพิมพ์
            //       ด้วย Dictionary(OrdinalIgnoreCase) ในหน่วยความจำแทน
            string values = string.Join(",", Enumerable.Range(1, AdminMenu.NpaSlotCount).Select(i => $"(a.npaid{i})"));

            var dtPair = db.ExecuteQuery($@"
                SELECT DISTINCT a.id AS agent_id, LTRIM(RTRIM(v.code)) AS code
                FROM [2026_web_agent] a
                CROSS APPLY (VALUES {values}) AS v(code)
                {where}
                  AND v.code IS NOT NULL AND LTRIM(RTRIM(v.code)) <> ''", prms);

            // ค่าที่แจ้ง -> รายชื่อตัวแทนที่แจ้งค่านั้น (ตัดซ้ำด้วย DISTINCT ตั้งแต่ใน SQL แล้ว)
            var agentsByValue = new Dictionary<string, HashSet<long>>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow r in dtPair.Rows)
            {
                string val = r["code"].ToString()!;
                long agentId = Convert.ToInt64(r["agent_id"]);
                if (!agentsByValue.TryGetValue(val, out var set))
                {
                    set = new HashSet<long>();
                    agentsByValue[val] = set;
                }
                set.Add(agentId);
            }

            // 4.2 : lookup รายละเอียดทรัพย์จาก MySQL ทีละชุด (กัน IN (...) ยาวเกินไป)
            //       ต่อ MySQL ไม่ได้ = ไม่ทำให้ทั้งหน้าพัง แต่ต้องบอกให้ชัดว่าข้อมูลทรัพย์ใช้ไม่ได้
            //       (ตารางฝั่งตัวแทนยังถูกต้อง ส่วนตารางทรัพย์จะไม่มีจังหวัด/ประเภท/มูลค่า)
            Dictionary<string, AgentAssetInfo> assets;
            try
            {
                assets = LoadAssets(db, agentsByValue.Keys);
            }
            catch (Exception ex)
            {
                data.AssetSourceError = ex.Message;
                assets = new Dictionary<string, AgentAssetInfo>(StringComparer.OrdinalIgnoreCase);
            }

            // 4.3 : สรุปยอด — ทุกตารางนับ "ทรัพย์ตัดซ้ำ" (1 รหัส = 1 ทรัพย์ ต่อให้มีตัวแทนแจ้งหลายคน)
            //       ค่าที่ไม่ตรงรหัสใน tb_product2 = ข้อความอิสระ -> แยกไปตาราง/ตัวนับของตัวเอง
            //       ไม่นับรวมในสถิติทรัพย์ (จำนวนทรัพย์/มูลค่า/จังหวัด/ประเภท) เพื่อไม่ให้ตัวเลขทรัพย์เพี้ยน
            //       ยกเว้นกรณีอ่าน MySQL ไม่สำเร็จ = ยังไม่รู้ว่าเป็นรหัสจริงหรือไม่ -> คงพฤติกรรมเดิม
            //       (นับเป็นทรัพย์ทั้งหมด) ไม่งั้นตัวเลขจะหายไปทั้งรายงานเวลาฐานทรัพย์ล่ม
            bool canClassify = data.AssetSourceError == null;
            var byProvince = new Dictionary<string, AgentReportRow>();
            var byType     = new Dictionary<string, AgentReportRow>();
            var agentsWithAsset    = new HashSet<long>();
            var agentsWithFreeText = new HashSet<long>();

            foreach (var kv in agentsByValue)
            {
                string val = kv.Key;
                assets.TryGetValue(val, out var asset);
                bool isFreeText = canClassify && asset == null;

                if (isFreeText)
                {
                    foreach (var id in kv.Value) agentsWithFreeText.Add(id);
                    data.FreeTextDistinct++;
                    data.FreeTextClaimTotal += kv.Value.Count;
                    data.TopFreeText.Add(new AgentReportRow
                    {
                        Label = val,
                        Count = kv.Value.Count,
                        Found = false            // ไม่ใช่รหัสทรัพย์ -> ไม่มีมูลค่า/จังหวัด/ประเภท
                    });
                    continue;
                }

                foreach (var id in kv.Value) agentsWithAsset.Add(id);
                data.AssetDistinct++;
                data.AssetClaimTotal += kv.Value.Count;

                decimal price = asset?.Price ?? 0;
                data.AssetValueTotal += price;

                string province = string.IsNullOrWhiteSpace(asset?.ProvinceName) ? LabelUnknown : asset!.ProvinceName!;
                string type     = string.IsNullOrWhiteSpace(asset?.TypeName)     ? LabelUnknown : asset!.TypeName!;

                Accumulate(byProvince, province, price);
                Accumulate(byType, type, price);

                data.TopDuplicateAsset.Add(new AgentReportRow
                {
                    // รหัสจริง: แสดงตามที่บันทึกใน tb_product2 (ตัวแทนอาจพิมพ์ตัวเล็ก) / ข้อความอิสระ: ตามที่พิมพ์
                    Label = string.IsNullOrEmpty(asset?.Code) ? val : asset!.Code,
                    Count = kv.Value.Count,
                    Value = price,
                    Found = asset != null
                });
            }

            data.AgentWithAsset    = agentsWithAsset.Count;
            data.AgentWithFreeText = agentsWithFreeText.Count;

            //----- ต่อฐานข้อมูลทรัพย์ไม่ได้ = ไม่ต้องแสดงตารางจังหวัด/ประเภท (จะได้ไม่ขึ้น "ไม่ระบุ" ทั้งตาราง)
            if (data.AssetSourceError == null)
            {
                data.AssetByProvince = byProvince.Values.OrderByDescending(x => x.Count).ThenByDescending(x => x.Value).ThenBy(x => x.Label).ToList();
                data.AssetByType     = byType.Values.OrderByDescending(x => x.Count).ThenByDescending(x => x.Value).ThenBy(x => x.Label).ToList();
            }
            data.TopDuplicateAsset = data.TopDuplicateAsset.OrderByDescending(x => x.Count).ThenByDescending(x => x.Value).ThenBy(x => x.Label).ToList();
            data.TopFreeText       = data.TopFreeText.OrderByDescending(x => x.Count).ThenBy(x => x.Label).ToList();

            return data;
        }

        private static void Accumulate(Dictionary<string, AgentReportRow> map, string key, decimal price)
        {
            if (!map.TryGetValue(key, out var row))
            {
                row = new AgentReportRow { Label = key };
                map[key] = row;
            }
            row.Count++;
            row.Value += price;
        }

        /// <summary>
        /// อ่านรายละเอียดทรัพย์ (จังหวัด/ประเภท/ราคา) จาก MySQL ตามรหัสทรัพย์ที่ส่งเข้ามา
        /// รหัสที่ไม่พบจะไม่มี key ใน Dictionary ที่คืนกลับ
        /// </summary>
        public static Dictionary<string, AgentAssetInfo> LoadAssets(DBHelper db, IEnumerable<string> codes)
        {
            var result = new Dictionary<string, AgentAssetInfo>(StringComparer.OrdinalIgnoreCase);

            var list = (codes ?? Enumerable.Empty<string>())
                .Select(c => (c ?? "").Trim())
                .Where(c => c.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (list.Count == 0) return result;

            const int chunkSize = 500;
            for (int start = 0; start < list.Count; start += chunkSize)
            {
                var chunk = list.Skip(start).Take(chunkSize).ToList();
                var pars = new Dictionary<string, object>();
                var placeholders = new List<string>();
                for (int i = 0; i < chunk.Count; i++)
                {
                    placeholders.Add("@c" + i);
                    pars["@c" + i] = chunk[i];
                }

                var dt = db.ExecuteQueryMySQL($@"
                    SELECT p.id, p.product_code, p.price, p.is_deleted,
                           g.product_group AS type_name,
                           prov.thai_name  AS province_name
                    FROM tb_product2 p
                    LEFT JOIN product_group g ON g.id = p.product_type
                    LEFT JOIN provinces prov  ON prov.province_id = p.province
                    WHERE p.product_code IN ({string.Join(",", placeholders)})", pars);

                foreach (DataRow r in dt.Rows)
                {
                    string code = (r["product_code"]?.ToString() ?? "").Trim();
                    if (code.Length == 0) continue;

                    var info = new AgentAssetInfo
                    {
                        Id           = r["id"] == DBNull.Value ? 0 : Convert.ToInt64(r["id"]),
                        Code         = code,
                        Price        = r["price"] == DBNull.Value ? 0 : Convert.ToDecimal(r["price"]),
                        IsDeleted    = r["is_deleted"] != DBNull.Value && Convert.ToInt32(r["is_deleted"]) == 1,
                        TypeName     = r["type_name"]?.ToString(),
                        ProvinceName = r["province_name"]?.ToString()
                    };

                    // tb_product2 มีรหัสทรัพย์ซ้ำได้ (ข้อมูลเก่า) — เลือกแถวที่ยังไม่ถูกลบก่อน แล้วค่อยเอา id ใหม่สุด
                    if (result.TryGetValue(code, out var exist))
                    {
                        bool better = (exist.IsDeleted && !info.IsDeleted)
                                   || (exist.IsDeleted == info.IsDeleted && info.Id > exist.Id);
                        if (!better) continue;
                    }
                    result[code] = info;
                }
            }

            return result;
        }
    }

    //----- 1 บรรทัดของตารางในรายงาน (ใช้ร่วมกันทุกตาราง — บางตารางไม่ใช้ Value)
    public class AgentReportRow
    {
        public string Label { get; set; } = "";
        public long Count { get; set; }
        public decimal Value { get; set; }
        public bool Found { get; set; } = true;   // false = รหัสทรัพย์ที่หาไม่พบใน tb_product2
    }

    //----- รายละเอียดทรัพย์ 1 รายการจาก MySQL
    public class AgentAssetInfo
    {
        public long Id { get; set; }
        public string Code { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public string? TypeName { get; set; }
        public string? ProvinceName { get; set; }
    }

    //----- ชุดข้อมูลทั้งหมดของรายงาน (เก็บรายการเต็ม — หน้าจอ/Excel ค่อยตัด Top 10 เอง)
    public class AgentReportData
    {
        public List<AgentReportRow> ByYear { get; set; } = new();            // ปี -> จำนวนตัวแทน
        public List<AgentReportRow> ByPersonType { get; set; } = new();      // ชาย/หญิง/นิติบุคคล -> จำนวนตัวแทน
        public List<AgentReportRow> AgentByProvince { get; set; } = new();   // จังหวัดของตัวแทน -> จำนวนตัวแทน
        public List<AgentReportRow> AssetByProvince { get; set; } = new();   // จังหวัดของทรัพย์ -> จำนวนทรัพย์ + มูลค่า
        public List<AgentReportRow> AssetByType { get; set; } = new();       // ประเภททรัพย์ -> จำนวนทรัพย์ + มูลค่า
        public List<AgentReportRow> TopDuplicateAsset { get; set; } = new(); // รหัสทรัพย์ -> จำนวนตัวแทนที่แจ้งซ้ำ
        public List<AgentReportRow> TopFreeText { get; set; } = new();       // ข้อความอิสระ (ไม่ใช่รหัสทรัพย์) -> จำนวนตัวแทนที่แจ้ง

        public long AgentTotal { get; set; }        // ตัวแทนทั้งหมดตามเงื่อนไข
        public long AgentWithAsset { get; set; }    // ตัวแทนที่ลงทะเบียนทรัพย์ (รหัสที่พบใน tb_product2) อย่างน้อย 1 รายการ
        public long AssetDistinct { get; set; }     // จำนวนทรัพย์ตัดซ้ำ (นับเฉพาะรหัสที่พบใน tb_product2)
        public long AssetClaimTotal { get; set; }   // จำนวนครั้งที่มีการลงทะเบียนทรัพย์ (ไม่ตัดซ้ำ)
        public decimal AssetValueTotal { get; set; }// มูลค่ารวมของทรัพย์ตัดซ้ำ

        // ค่าที่ตัวแทนพิมพ์เป็น "ข้อความอิสระ" (ไม่ตรงรหัสใน tb_product2) — แยกออกจากสถิติทรัพย์ทั้งหมด
        public long AgentWithFreeText { get; set; } // ตัวแทนที่แจ้งเป็นข้อความอย่างน้อย 1 รายการ
        public long FreeTextDistinct { get; set; }  // จำนวนข้อความตัดซ้ำ
        public long FreeTextClaimTotal { get; set; }// จำนวนครั้งที่แจ้งเป็นข้อความ (ไม่ตัดซ้ำ)

        // ไม่ null = อ่านฐานข้อมูลทรัพย์ NPA (MySQL) ไม่สำเร็จ — ตารางฝั่งทรัพย์จึงไม่มีจังหวัด/ประเภท/มูลค่า
        public string? AssetSourceError { get; set; }
    }
}
