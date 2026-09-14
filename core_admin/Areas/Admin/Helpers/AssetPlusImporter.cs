using System.Xml;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// เครื่องยนต์ "นำเข้าข้อมูลจาก web service ของ Asset Plus" ที่ใช้ร่วมกัน 2 ทาง :
    ///
    ///   1. เมนู "Get ..." ในหลังบ้าน (ผู้ใช้กดเอง)  → <see cref="Controllers.AssetPlusImportController"/>
    ///   2. ตัวจับเวลาเรียกอัตโนมัติ (scheduler)      → <see cref="Controllers.WsScheduleController"/>
    ///
    /// ระบบเดิมเขียนตรรกะเดียวกันไว้ 2 ที่ (mod_*/add.aspx กับ ws_schedule/*.aspx) แล้วค่อย ๆ ต่างกัน
    /// ที่นี่จึงรวมไว้จุดเดียว เพื่อให้กดเองกับให้ scheduler เรียก ได้ผลลัพธ์เหมือนกันเป๊ะ
    ///
    /// อ้างอิงต้นฉบับ (D:\Project\assetfund.co.th.old\assetplus\backoffice\ws_schedule\) :
    ///   ws_get_other_indices.aspx · ws_get_nav.aspx · ws_get_performance.aspx · ws_get_fundfact.aspx
    ///
    /// กฎที่ต้องรักษาไว้ให้ตรงกับระบบเดิม (front-end เดิมอ่านข้อมูลตามนี้) :
    ///   • แถวที่นำเข้าใหม่ = <c>Flag = 1</c> · แถวเดิมของคีย์เดียวกันถูกตั้ง <c>Flag = 0</c> (ไม่ลบทิ้ง)
    ///   • <c>status = 1, pb_status = 1, show_front = 1</c> → เผยแพร่ทันทีโดยไม่ต้องรออนุมัติ
    ///   • <c>sort</c> = MAX(sort) + 10 ไล่ขึ้นทีละแถว
    ///   • คอลัมน์ <c>*DateFormat</c> เก็บ <c>yyyyMMdd</c> · <c>*DateIn</c> เก็บ <c>yyyyMMdd HH:mm:ss</c>
    ///
    /// ต่างจากระบบเดิมตรงที่ใช้ SQL แบบ parameterized ทั้งหมด (ระบบเดิมต่อสตริงแล้วใช้ <c>class1.o()</c>
    /// ซึ่ง "ตัดเครื่องหมาย ' ทิ้ง" — ชื่อกองทุนที่มี apostrophe จึงเพี้ยน) ที่นี่เก็บค่าจริงครบถ้วน
    /// </summary>
    public class AssetPlusImporter
    {
        private readonly DBHelper _db;

        /// <summary>ชื่อผู้ใช้ที่ scheduler ของระบบเดิมบันทึกลง last_user / pb_last_user</summary>
        public const string ScheduleUser = "ws_auto";

        public AssetPlusImporter(DBHelper db) { _db = db; }

        // =================================================================================
        //  helper
        // =================================================================================

        /// <summary>ค่าใน element ลูก แบบ "ดิบ" ไม่ตัดช่องว่าง (ระบบเดิมใช้ InnerText ตรง ๆ)</summary>
        private static string Raw(XmlNode parent, string name)
        {
            foreach (XmlNode c in parent.ChildNodes)
            {
                if (c is XmlElement el && string.Equals(el.LocalName, name, StringComparison.OrdinalIgnoreCase))
                    return el.InnerText;
            }
            return "";
        }

        private static List<XmlElement> Elements(XmlNode parent)
        {
            var list = new List<XmlElement>();
            foreach (XmlNode c in parent.ChildNodes) if (c is XmlElement el) list.Add(el);
            return list;
        }

        private static List<XmlElement> Named(XmlNode parent, string name) => AssetPlusWsClient.Children(parent, name);

        /// <summary>
        /// เวลาปัจจุบันในรูปแบบที่ระบบเดิมเก็บลงคอลัมน์ <c>*DateIn</c> (<c>yyyyMMdd HH:mm:ss</c>)
        ///
        /// ⚠ ต้องเป็นปี **ค.ศ.** — ข้อมูลเดิมในตารางเป็น ค.ศ. ทั้งหมด (20180209, 20251016, 20260120 …)
        /// culture ของแอปนี้คือ th-TH ซึ่งใช้ปฏิทินพุทธ ถ้าเรียก ToString เฉย ๆ จะได้ 2569 แล้วเรียงลำดับ
        /// กับข้อมูลเดิมไม่ตรงกัน จึงบังคับ InvariantCulture
        /// </summary>
        private static string DateIn() =>
            DateTime.Now.ToString("yyyyMMdd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

        private static long UnixNow() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        /// <summary>
        /// ตัวไล่เลข <c>sort</c> แบบเดียวกับ <c>include/new_sort.aspx</c> (MAX(sort) + 10)
        /// ระบบเดิม query ใหม่ทุกแถว — ที่นี่อ่านครั้งเดียวแล้วบวกทีละ 10 ในหน่วยความจำ
        /// ได้ค่าชุดเดียวกันแต่ไม่ต้องยิง MAX() ซ้ำนับร้อยครั้งต่อการนำเข้าหนึ่งรอบ
        /// </summary>
        private long NextSortSeed(string table)
        {
            var dt = _db.ExecuteQuery(string.Format("select coalesce(max(sort), 0) as s, count(id) as c from {0}", Db.T(table)));
            if (dt.Rows.Count == 0) return 10;
            int c = Convert.ToInt32(dt.Rows[0]["c"]);
            return c > 0 ? Convert.ToInt64(dt.Rows[0]["s"]) + 10 : 10;
        }

        /// <summary>คอลัมน์ระบบที่ทุกตารางนำเข้าใช้ค่าเหมือนกัน (ตรงกับ ws_schedule ของระบบเดิม)</summary>
        private static Dictionary<string, object> AuditFields(string user, long sort)
        {
            long now = UnixNow();
            return new Dictionary<string, object>()
            {
                { "lastcreate", now }, { "lastupdate", now }, { "sort", sort },
                { "status", 1 }, { "pb_status", 1 },
                { "last_user", user }, { "pb_last_user", user }, { "show_front", 1 },
            };
        }

        /// <summary>ชื่อคอลัมน์จริงของตาราง (ใช้กรอง element ที่ web service ส่งเกินมา ไม่ให้ SQL พัง)</summary>
        public HashSet<string> TableColumns(string table)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var dt = _db.ExecuteQuery(
                "select column_name from information_schema.columns where table_schema = 'dbo' and table_name = @t",
                new Dictionary<string, object>() { { "t", table } });
            foreach (System.Data.DataRow r in dt.Rows) set.Add(r["column_name"] + "");
            return set;
        }

        // =================================================================================
        //  1) Other Indices — ws_get_other_indices.aspx  (MartketOtherIndices)
        // =================================================================================

        /// <summary>
        /// &lt;OtherIndices&gt;&lt;ValueDate&gt;dd/MM/yyyy&lt;/ValueDate&gt;
        ///   &lt;Index&gt;&lt;IndexItem&gt;&lt;IndexName/&gt;&lt;IndexValue/&gt;&lt;Change/&gt;&lt;PercentChange/&gt;&lt;/IndexItem&gt;…&lt;/Index&gt;
        /// </summary>
        public int ImportOtherIndices(XmlElement data, string fallbackDate, string user, out string detail)
        {
            string valueDate = AssetPlusWsClient.Child(data, "ValueDate");
            if (string.IsNullOrEmpty(valueDate)) valueDate = fallbackDate;
            string valueDateFormat = AssetPlusWsClient.ToDateKey(valueDate);
            string valueDateIn = DateIn();
            long sort = NextSortSeed("tb_home_other_indices");

            int n = 0;
            foreach (var indexGroup in Named(data, "Index"))
            {
                foreach (var item in Elements(indexGroup))
                {
                    string indexName = Raw(item, "IndexName");
                    if (string.IsNullOrEmpty(indexName)) continue;

                    //----- แถวเดิมของ (IndexName + วันที่) → Flag = 0 แล้วค่อย insert แถวใหม่ -----
                    _db.ExecuteNonQuery("update [tb_home_other_indices] set Flag = 0 where IndexName = @n and ValueDateFormat = @d",
                        new Dictionary<string, object>() { { "n", indexName }, { "d", valueDateFormat } });

                    var f = AuditFields(user, sort);
                    f["title"] = indexName;
                    f["IndexName"] = indexName;
                    f["IndexValue"] = Raw(item, "IndexValue");
                    f["Change"] = Raw(item, "Change");
                    f["PercentChange"] = Raw(item, "PercentChange");
                    f["ValueDate"] = valueDate;
                    f["ValueDateFormat"] = valueDateFormat;
                    f["ValueDateIn"] = valueDateIn;
                    f["Flag"] = 1;
                    _db.Insert("tb_home_other_indices", f);
                    sort += 10;
                    n++;
                }
            }
            detail = string.Format("ValueDate {0}", valueDate);
            return n;
        }

        // =================================================================================
        //  2) NAV — ws_get_nav.aspx  (NAVAnnounce)
        // =================================================================================

        /// <summary>&lt;ArrayOfNAV&gt;&lt;NAV&gt;&lt;FundCode/&gt;&lt;FundNameTH/&gt;…&lt;/NAV&gt;…</summary>
        public int ImportNav(XmlElement data, string user, out string detail)
        {
            string navDateIn = DateIn();
            long sort = NextSortSeed("tb_fund_nav");

            var items = Named(data, "NAV");
            if (items.Count == 0 && string.Equals(data.LocalName, "NAV", StringComparison.OrdinalIgnoreCase)) items.Add(data);

            var dates = new HashSet<string>();
            int n = 0;
            foreach (var nav in items)
            {
                string fundCode = Raw(nav, "FundCode");
                if (string.IsNullOrEmpty(fundCode)) continue;

                string navDateVal = Raw(nav, "NAVDate");
                string navDateFormat = AssetPlusWsClient.ToDateKey(navDateVal);
                if (navDateFormat != "") dates.Add(navDateFormat);

                _db.ExecuteNonQuery("update [tb_fund_nav] set Flag = 0 where FundCode = @c and NAVDateFormat = @d",
                    new Dictionary<string, object>() { { "c", fundCode }, { "d", navDateFormat } });

                var f = AuditFields(user, sort);
                f["title"] = fundCode;
                f["FundCode"] = fundCode;
                f["FundNameTH"] = Raw(nav, "FundNameTH");
                f["FundNameEN"] = Raw(nav, "FundNameEN");
                f["NAVDate"] = navDateVal;
                f["TotalNAV"] = Raw(nav, "TotalNAV");
                f["NAVPerUnit"] = Raw(nav, "NAVPerUnit");
                f["Offer"] = Raw(nav, "Offer");
                f["Bid"] = Raw(nav, "Bid");
                f["BahtChange"] = Raw(nav, "BahtChange");
                f["Change"] = Raw(nav, "Change");
                f["NAVDateIn"] = navDateIn;
                f["NAVDateFormat"] = navDateFormat;
                f["Flag"] = 1;
                _db.Insert("tb_fund_nav", f);
                sort += 10;
                n++;
            }
            detail = dates.Count > 0 ? "NAVDate " + string.Join(", ", dates.OrderBy(x => x)) : "";
            return n;
        }

        // =================================================================================
        //  3) Performance — ws_get_performance.aspx  (FundReturnPerformance)
        // =================================================================================

        /// <summary>
        /// &lt;ReturnPerformance&gt;
        ///   &lt;ReturnPerformanceDate/&gt;&lt;TitleFundCode/&gt;… (หัวตาราง → tb_fund_performance_hd แถว id = 1)
        ///   &lt;PastPerformance&gt;&lt;Performance&gt;&lt;FundCode/&gt;…&lt;/Performance&gt;… (→ tb_fund_performance)
        /// </summary>
        public int ImportPerformance(XmlElement data, string fallbackDate, string user, out string detail)
        {
            string returnDate = AssetPlusWsClient.Child(data, "ReturnPerformanceDate");
            if (string.IsNullOrEmpty(returnDate)) returnDate = fallbackDate;
            string navDateFormat = AssetPlusWsClient.ToDateKey(returnDate);
            string navDateIn = DateIn();

            //----- แถวเดิมของ "วันเดียวกัน" ทั้งหมด → Flag = 0 (ทำครั้งเดียวก่อนใส่ชุดใหม่) -----
            _db.ExecuteNonQuery("update [tb_fund_performance] set Flag = 0 where NAVDateFormat = @d",
                new Dictionary<string, object>() { { "d", navDateFormat } });

            long sort = NextSortSeed("tb_fund_performance");
            int n = 0;

            //----- FundCodeMark : ผูกแถว "เกณฑ์มาตรฐาน" เข้ากับกองทุนที่อยู่เหนือมัน -----
            //      แถวที่ NAVPerUnit / InceptionDateTH / InceptionDateEN ว่างทั้งสามช่อง = แถว benchmark
            //      → ใช้ FundCode ของกองทุนก่อนหน้า แล้วล้างตัวจำ (ตรงกับ ws_get_performance.aspx ทุกบรรทัด
            //        รวมถึงกรณี benchmark ติดกัน 2 แถว ที่แถวหลังจะได้ค่าว่าง)
            string fundCodeMarkChk = "";

            foreach (var group in Named(data, "PastPerformance"))
            {
                foreach (var perf in Named(group, "Performance"))
                {
                    string fundCode = Raw(perf, "FundCode");
                    string navPerUnit = Raw(perf, "NAVPerUnit");
                    string inceptionTH = Raw(perf, "InceptionDateTH");
                    string inceptionEN = Raw(perf, "InceptionDateEN");

                    string fundCodeMark;
                    if (navPerUnit.Trim() == "" && inceptionTH.Trim() == "" && inceptionEN.Trim() == "")
                    {
                        fundCodeMark = fundCodeMarkChk;
                        fundCodeMarkChk = "";
                    }
                    else
                    {
                        fundCodeMarkChk = fundCode;
                        fundCodeMark = fundCode;
                    }

                    if (string.IsNullOrEmpty(fundCode)) continue;

                    var f = AuditFields(user, sort);
                    f["title"] = fundCode;
                    f["ReturnPerformanceDate"] = returnDate;
                    f["FundCode"] = fundCode;
                    f["FundNameTH"] = Raw(perf, "FundNameTH");
                    f["FundNameEN"] = Raw(perf, "FundNameEN");
                    f["InceptionDateTH"] = inceptionTH;
                    f["InceptionDateEN"] = inceptionEN;
                    f["NAVPerUnit"] = navPerUnit;
                    f["ThreeMonth"] = Raw(perf, "ThreeMonth");
                    f["SixMonth"] = Raw(perf, "SixMonth");
                    f["OneYear"] = Raw(perf, "OneYear");
                    f["ThreeYear"] = Raw(perf, "ThreeYear");
                    f["YTD"] = Raw(perf, "YTD");
                    f["InceptionPort"] = Raw(perf, "InceptionPort");
                    f["InceptionBM"] = Raw(perf, "InceptionBM");
                    f["NAVDateIn"] = navDateIn;
                    f["NAVDateFormat"] = navDateFormat;
                    f["FundCodeMark"] = fundCodeMark;
                    f["Flag"] = 1;
                    _db.Insert("tb_fund_performance", f);
                    sort += 10;
                    n++;
                }
            }

            //----- หัวตาราง : ทุก element ชั้นบนที่ไม่ใช่ PastPerformance → tb_fund_performance_hd แถว id = 1 -----
            int hd = UpdatePerformanceHeader(data, navDateFormat, navDateIn);

            //----- ⚠ อย่าใส่อักขระนอกโค้ดเพจไทย (เช่น ·) — tb_admin_log.action_info เป็นชนิด text
            //      ที่เก็บได้เฉพาะ codepage 874 ข้อความจะกลายเป็น ? ตอนบันทึก log
            detail = string.Format("ReturnPerformanceDate {0}{1}", returnDate, hd > 0 ? " + อัปเดตหัวตาราง (tb_fund_performance_hd)" : "");
            return n;
        }

        /// <summary>
        /// อัปเดตหัวตารางผลการดำเนินงาน — ระบบเดิม:
        /// <c>update tb_fund_performance_hd set &lt;ทุก element ที่ไม่ใช่ PastPerformance&gt; = '…', NAVDateIn = …, NAVDateFormat = … where id = '1'</c>
        /// </summary>
        private int UpdatePerformanceHeader(XmlElement data, string navDateFormat, string navDateIn)
        {
            var columns = TableColumns("tb_fund_performance_hd");
            var reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "id", "Flag", "lastcreate", "lastupdate", "sort", "status", "pb_status", "last_user", "pb_last_user", "show_front" };

            var set = new List<string>();
            var p = new Dictionary<string, object>();
            int i = 0;

            foreach (var el in Elements(data))
            {
                if (string.Equals(el.LocalName, "PastPerformance", StringComparison.OrdinalIgnoreCase)) continue;
                if (reserved.Contains(el.LocalName) || !columns.Contains(el.LocalName)) continue;

                string key = "h" + (i++);
                set.Add(string.Format("{0} = @{1}", el.LocalName, key));
                p[key] = el.InnerText;
            }

            if (set.Count == 0) return 0;

            set.Add("NAVDateIn = @navdatein");
            p["navdatein"] = navDateIn;
            set.Add("NAVDateFormat = @navdateformat");
            p["navdateformat"] = navDateFormat;

            return _db.ExecuteNonQuery(
                string.Format("update {0} set {1} where id = 1", Db.T("tb_fund_performance_hd"), string.Join(", ", set)), p);
        }

        // =================================================================================
        //  4) Fund Fact Sheet — ws_get_fundfact.aspx  (FundFactSheet)
        // =================================================================================

        /// <summary>
        /// ตารางลูกของ Fund Fact Sheet : ชื่อ element ที่ห่อรายการ → (ตารางปลายทาง, คอลัมน์ที่เขียน)
        /// คีย์ของ dictionary คือชื่อ element ระดับ 2 ใน &lt;FundFact&gt; (ระดับ 3 คือ element ห่อแต่ละแถว)
        ///
        /// ค่าใน list คือคู่ "ชื่อคอลัมน์ในตาราง" → "ชื่อ element ใน XML"
        /// (บางคู่ชื่อไม่ตรงกัน เช่น คอลัมน์ OneYears มาจาก element OneYear
        ///  และคอลัมน์ PaymentYear/PaymentYearTH ต่างก็มาจาก element PaymentYearTH ตัวเดียวกัน — ตามระบบเดิม)
        /// </summary>
        private static readonly (string Wrapper, string Table, (string Col, string Xml)[] Map)[] FundFactChildTables = new[]
        {
            ("NAVHistory",             "tb_fund_fundfact_nav",              new[] { ("navdate", "NAVDateEN"), ("NAVDateTH", "NAVDateTH"), ("NAVDateEN", "NAVDateEN"), ("navunit", "NAVPerUnit") }),
            ("Benchmark1History",      "tb_fund_fundfact_benchmark_1",      new[] { ("BenchmarkDateTH", "BenchmarkDateTH"), ("BenchmarkDateEN", "BenchmarkDateEN"), ("BenchmarkValue", "BenchmarkValue") }),
            ("Benchmark2History",      "tb_fund_fundfact_benchmark_2",      new[] { ("BenchmarkDateTH", "BenchmarkDateTH"), ("BenchmarkDateEN", "BenchmarkDateEN"), ("BenchmarkValue", "BenchmarkValue") }),
            ("FundPerformanceData",    "tb_fund_fundfact_performance",      new[] { ("PerformanceItemTH", "PerformanceItemTH"), ("PerformanceItemEN", "PerformanceItemEN"), ("ThreeMonths", "ThreeMonths"), ("SixMonths", "SixMonths"), ("OneYears", "OneYear"), ("ThreeYears", "ThreeYears"), ("Inception", "Inception"), ("Decription", "Decription"), ("DescriptionTH", "DescriptionTH"), ("DescriptionEN", "DescriptionEN") }),
            ("AssetAllocationData",    "tb_fund_fundfact_asset_allocation", new[] { ("InvestmentTH", "InvestmentTH"), ("InvestmentEN", "InvestmentEN"), ("InvRatio", "InvRatio") }),
            ("FixedIncomeData",        "tb_fund_fundfact_fixincome",        new[] { ("SecurityCode", "SecurityCode"), ("SecurityCodeTH", "SecurityCodeTH"), ("SecurityCodeEN", "SecurityCodeEN"), ("IssueRating", "IssueRating"), ("InvRatio", "InvRatio") }),
            ("EquitySectorData",       "tb_fund_fundfact_equity_sector",    new[] { ("SectorCode", "SectorCode"), ("SectorCodeTH", "SectorCodeTH"), ("SectorCodeEN", "SectorCodeEN"), ("InvRatio", "InvRatio") }),
            ("EquityStockData",        "tb_fund_fundfact_equity_stock",     new[] { ("SecurityCode", "SecurityCode"), ("SecurityCodeTH", "SecurityCodeTH"), ("SecurityCodeEN", "SecurityCodeEN"), ("InvRatio", "InvRatio") }),
            ("CurrencyBreakdownData",  "tb_fund_fundfact_currency",         new[] { ("CurrencyCode", "CurrencyCode"), ("CurrencyCodeTH", "CurrencyCodeTH"), ("CurrencyCodeEN", "CurrencyCodeEN"), ("Month1TH", "Month1TH"), ("Month1EN", "Month1EN"), ("Month2TH", "Month2TH"), ("Month2EN", "Month2EN"), ("Change", "Change"), ("NAV", "NAV") }),
            ("TopHoldingData",         "tb_fund_fundfact_topholding",       new[] { ("SecurityCodeTH", "SecurityCodeTH"), ("SecurityCodeEN", "SecurityCodeEN"), ("Month1TH", "Month1TH"), ("Month1EN", "Month1EN"), ("Month2TH", "Month2TH"), ("Month2EN", "Month2EN"), ("Change", "Change") }),
            ("DividendPaymentData",    "tb_fund_fundfact_dividend",         new[] { ("PaymentYear", "PaymentYearTH"), ("PaymentYearTH", "PaymentYearTH"), ("PaymentYearEN", "PaymentYearEN"), ("Amount", "Amount") }),
        };

        /// <summary>
        /// &lt;ArrayOfFundFact&gt;&lt;FundFact&gt;&lt;FundCode/&gt;&lt;…element เดี่ยวอีกกว่า 250 ตัว…&gt;
        ///   &lt;NAVHistory/&gt;&lt;FundPerformanceData/&gt;… (11 ชุดที่แตกลงตารางลูก)
        ///
        /// ลำดับการทำงานตรงกับ ws_get_fundfact.aspx :
        ///   1. แถวเดิมของ fundcode นี้ → Flag = 0
        ///   2. insert แถวใหม่ (มีแค่คอลัมน์ระบบ + fundcode) Flag = 1
        ///   3. ตารางลูกทั้ง 11 ตัว : ลบของ fundcode นี้ทิ้ง แล้วใส่ชุดใหม่
        ///   4. UPDATE ค่าทุก element เดี่ยวลง tb_fund_fundfact ของ fundcode นี้
        ///      (ระบบเดิมไม่กรอง Flag — แถวเก่าที่เพิ่งตั้ง Flag = 0 จึงถูกอัปเดตด้วย ที่นี่ทำเหมือนกัน)
        /// </summary>
        public int ImportFundFact(XmlElement data, string user, out string detail)
        {
            var mainColumns = TableColumns("tb_fund_fundfact");
            var reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "id", "Flag", "lastcreate", "lastupdate", "sort", "status", "pb_status", "last_user", "pb_last_user", "show_front", "title", "fundcode", "NAVDateIn" };

            //----- element ที่เป็น "ชุดรายการ" ไม่ใช่ค่าเดี่ยว — ห้ามเอาไปใส่ UPDATE ของตารางหลัก -----
            var listElements = new HashSet<string>(FundFactChildTables.Select(t => t.Wrapper), StringComparer.OrdinalIgnoreCase);

            var childColumns = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var t in FundFactChildTables) childColumns[t.Table] = TableColumns(t.Table);

            string navDateIn = DateIn();
            long sort = NextSortSeed("tb_fund_fundfact");

            var items = Named(data, "FundFact");
            if (items.Count == 0 && string.Equals(data.LocalName, "FundFact", StringComparison.OrdinalIgnoreCase)) items.Add(data);

            int n = 0, childRows = 0;
            foreach (var ff in items)
            {
                string fundCode = Raw(ff, "FundCode");
                if (string.IsNullOrEmpty(fundCode)) continue;

                //----- 1) แถวเดิม → Flag = 0 -----
                _db.ExecuteNonQuery("update [tb_fund_fundfact] set Flag = 0 where fundcode = @c",
                    new Dictionary<string, object>() { { "c", fundCode } });

                //----- 2) แถวใหม่ (ระบบเดิม insert FundNameTH เป็นค่าว่างก่อน แล้วค่อย UPDATE ทีหลัง) -----
                var f = AuditFields(user, sort);
                f["title"] = fundCode;
                f["fundcode"] = fundCode;
                f["FundNameTH"] = "";
                f["NAVDateIn"] = navDateIn;
                f["Flag"] = 1;
                _db.Insert("tb_fund_fundfact", f);
                sort += 10;
                n++;

                //----- 3) ตารางลูก -----
                foreach (var (wrapper, table, map) in FundFactChildTables)
                {
                    var groups = Named(ff, wrapper);
                    if (groups.Count == 0) continue;

                    _db.ExecuteNonQuery(string.Format("delete from {0} where fundcode = @c", Db.T(table)),
                        new Dictionary<string, object>() { { "c", fundCode } });

                    var cols = childColumns[table];
                    foreach (var g in groups)
                    {
                        foreach (var rowEl in Elements(g))
                        {
                            var row = new Dictionary<string, object>() { { "fundcode", fundCode } };
                            foreach (var (col, xml) in map)
                            {
                                if (!cols.Contains(col)) continue;
                                row[col] = Raw(rowEl, xml);
                            }
                            _db.Insert(table, row);
                            childRows++;
                        }
                    }
                }

                //----- 4) UPDATE ค่าทุก element เดี่ยวลงตารางหลัก -----
                var set = new List<string>();
                var p = new Dictionary<string, object>();
                int i = 0;
                foreach (var el in Elements(ff))
                {
                    string col = el.LocalName;
                    if (listElements.Contains(col)) continue;
                    if (string.Equals(col, "FundNameTH", StringComparison.OrdinalIgnoreCase)) continue;   // ใส่ทีหลังแบบ HtmlEncode
                    if (reserved.Contains(col) || !mainColumns.Contains(col)) continue;

                    string key = "v" + (i++);
                    set.Add(string.Format("{0} = @{1}", col, key));
                    p[key] = el.InnerText;
                }

                //----- ระบบเดิมเก็บ FundNameTH แบบ HTML-encode (HttpUtility.HtmlEncode) ต่างจากคอลัมน์อื่น -----
                if (mainColumns.Contains("FundNameTH"))
                {
                    set.Add("FundNameTH = @fundnameth");
                    p["fundnameth"] = System.Web.HttpUtility.HtmlEncode(Raw(ff, "FundNameTH"));
                }

                if (set.Count > 0)
                {
                    p["c"] = fundCode;
                    _db.ExecuteNonQuery(
                        string.Format("update {0} set {1} where fundcode = @c", Db.T("tb_fund_fundfact"), string.Join(", ", set)), p);
                }
            }

            detail = string.Format("{0:n0} กองทุน + ตารางลูก {1:n0} แถว", n, childRows);
            return n;
        }
    }
}
