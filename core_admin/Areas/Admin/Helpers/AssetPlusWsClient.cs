using System.Text;
using System.Xml;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// ตัวเรียก web service ของระบบเดิม Asset Plus (<c>ASPWS.asmx</c>)
    /// ที่หน้า "Get ..." ในหลังบ้านเดิมใช้ดึงข้อมูล NAV / Performance / Fund Fact Sheet / Other Indices
    ///
    /// endpoint เดิมอยู่ใน <c>assetplus/web.config</c> :
    ///   <c>&lt;add key="ASPWSService.ASPWS" value="http://167.179.243.42:53556/ws/ASPWS.asmx"/&gt;</c>
    /// ในระบบใหม่ตั้งค่าได้ที่ <c>appsettings.json → AssetPlusWS:URL</c> (ปรับตามปลายทางจริงได้)
    ///
    /// operation ที่ใช้ (จาก ASPWS.wsdl):
    ///   NAVAnnounce()                     → &lt;ArrayOfNAV&gt;&lt;NAV&gt;…
    ///   MartketOtherIndices(date)         → &lt;OtherIndices&gt;&lt;ValueDate&gt;…&lt;Index&gt;…
    ///   FundReturnPerformance(date)       → &lt;ReturnPerformance&gt;…&lt;PastPerformance&gt;&lt;Performance&gt;…
    ///   FundFactSheet(fundDate)           → &lt;ArrayOfFundFact&gt;&lt;FundFact&gt;…
    /// </summary>
    public class AssetPlusWsClient
    {
        public const string Namespace = "http://tempuri.org/";
        private readonly string _url;
        private readonly int _timeoutSeconds;

        public AssetPlusWsClient(IConfiguration config)
        {
            _url = config["AssetPlusWS:URL"] ?? "";
            _timeoutSeconds = int.TryParse(config["AssetPlusWS:TimeoutSeconds"], out var t) ? t : 60;
        }

        public string Url => _url;
        public bool IsConfigured => !string.IsNullOrWhiteSpace(_url);

        /// <summary>
        /// เรียก operation แล้วคืน "เนื้อข้อมูล" ที่อยู่ใน &lt;xxxResult&gt; เป็น XmlElement
        /// คืน null พร้อม <paramref name="error"/> เมื่อเรียกไม่ได้ (เช่น web service ปิด / เครือข่ายเข้าไม่ถึง)
        /// </summary>
        public XmlElement? Call(string operation, string? paramName, string? paramValue, out string error)
        {
            error = "";
            if (!IsConfigured)
            {
                error = "ยังไม่ได้ตั้งค่า AssetPlusWS:URL ใน appsettings";
                return null;
            }

            string body = string.IsNullOrEmpty(paramName)
                ? string.Format("<{0} xmlns=\"{1}\" />", operation, Namespace)
                : string.Format("<{0} xmlns=\"{1}\"><{2}>{3}</{2}></{0}>", operation, Namespace, paramName,
                                System.Security.SecurityElement.Escape(paramValue ?? ""));

            string envelope =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body>" + body + "</soap:Body></soap:Envelope>";

            try
            {
                using var http = new HttpClient() { Timeout = TimeSpan.FromSeconds(_timeoutSeconds) };
                var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "\"" + Namespace + operation + "\"");

                var res = http.PostAsync(_url, content).GetAwaiter().GetResult();
                string text = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (!res.IsSuccessStatusCode)
                {
                    error = string.Format("web service ตอบกลับ HTTP {0}", (int)res.StatusCode);
                    return null;
                }

                var doc = new XmlDocument();
                doc.LoadXml(text);
                return FindResult(doc, operation + "Result");
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>อ่านไฟล์ XML ที่ผู้ใช้อัปโหลด (ใช้แทน web service ตอนเครือข่ายเข้าไม่ถึง — โครงสร้างเดียวกัน)</summary>
        public static XmlElement? FromXmlText(string xmlText, string resultElementName, out string error)
        {
            error = "";
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlText);
                //----- ไฟล์อาจเป็น SOAP envelope หรือเป็น payload ตรง ๆ (เช่น NAVAnnounce.xml ที่ระบบเดิมเซฟไว้)
                var inResult = FindResult(doc, resultElementName);
                if (inResult != null) return inResult;
                return doc.DocumentElement;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>หา element ชื่อ <c>&lt;xxxResult&gt;</c> แบบไม่สนใจ namespace แล้วคืน "ลูกตัวแรกที่เป็น element"</summary>
        private static XmlElement? FindResult(XmlDocument doc, string resultElementName)
        {
            foreach (XmlNode n in doc.GetElementsByTagName("*"))
            {
                if (n is XmlElement el && string.Equals(el.LocalName, resultElementName, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (XmlNode c in el.ChildNodes)
                    {
                        if (c is XmlElement ce) return ce;
                    }
                    return el;
                }
            }
            return null;
        }

        /// <summary>ค่าใน element ลูกชื่อ <paramref name="name"/> (ไม่สนใจ namespace) — ไม่เจอคืนค่าว่าง</summary>
        public static string Child(XmlNode parent, string name)
        {
            foreach (XmlNode c in parent.ChildNodes)
            {
                if (c is XmlElement el && string.Equals(el.LocalName, name, StringComparison.OrdinalIgnoreCase))
                    return el.InnerText.Trim();
            }
            return "";
        }

        /// <summary>element ลูกทั้งหมดที่ชื่อ <paramref name="name"/> (ไม่สนใจ namespace)</summary>
        public static List<XmlElement> Children(XmlNode parent, string name)
        {
            var list = new List<XmlElement>();
            foreach (XmlNode c in parent.ChildNodes)
            {
                if (c is XmlElement el && string.Equals(el.LocalName, name, StringComparison.OrdinalIgnoreCase))
                    list.Add(el);
            }
            return list;
        }

        /// <summary>dd/MM/yyyy → yyyyMMdd (รูปแบบที่ระบบเดิมเก็บในคอลัมน์ *DateFormat)</summary>
        public static string ToDateKey(string ddMMyyyy)
        {
            string v = (ddMMyyyy ?? "").Trim();
            if (v.Length < 10) return "";
            return v.Substring(6, 4) + v.Substring(3, 2) + v.Substring(0, 2);
        }
    }
}
