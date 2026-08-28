using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using thaicredit_hr_admin.Areas.Admin.Controllers;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    /// <summary>
    /// ไฟล์แนบ 1 ไฟล์ที่บันทึกลงดิสก์แล้ว
    /// </summary>
    public class SavedAttachment
    {
        public string StoredName { get; set; } = "";   // ชื่อไฟล์ที่บันทึกจริง (ใช้ลง DB / ทำลิงก์)
        public string FullPath { get; set; } = "";     // path เต็มบนดิสก์ (ใช้แนบอีเมล)
        public string OriginalName { get; set; } = ""; // ชื่อไฟล์เดิมที่ผู้ใช้อัปโหลด (ใช้เป็นชื่อไฟล์ในอีเมล)
    }

    /// <summary>
    /// รูปในเนื้อหาอีเมลที่จะฝังไปกับอีเมลแบบ inline (cid:)
    /// </summary>
    public class InlineImage
    {
        public string Cid { get; set; } = "";
        public byte[] Content { get; set; } = [];
        public string MimeType { get; set; } = "image/png";
        public string FileName { get; set; } = "image.png";
    }

    /// <summary>
    /// เนื้อหาอีเมลที่แปลงจาก CKEditor แล้ว พร้อมใส่ลง MimeMessage
    /// </summary>
    public class PreparedMailBody
    {
        public string Html { get; set; } = "";
        public string PlainText { get; set; } = "";
        public List<InlineImage> Images { get; set; } = new();

        /// <summary>ใส่เนื้อหา + รูป inline ลง BodyBuilder ของอีเมลแต่ละฉบับ</summary>
        public void ApplyTo(BodyBuilder builder)
        {
            builder.HtmlBody = Html;
            if (!string.IsNullOrWhiteSpace(PlainText))
            {
                builder.TextBody = PlainText;
            }
            foreach (var img in Images)
            {
                var resource = builder.LinkedResources.Add(img.FileName, img.Content, ContentType.Parse(img.MimeType));
                resource.ContentId = img.Cid;
            }
        }
    }

    /// <summary>ผลการส่งอีเมลทั้งรอบ</summary>
    public class BroadcastSendResult
    {
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public List<EmailSendResult> Results { get; set; } = new();
    }

    /// <summary>
    /// ตัวช่วยกลางของเมนู "ส่งอีเมลกระจาย" 3 เมนู (SubscriptionEmail / MemberNews / AgentNews)
    ///
    /// - <see cref="SaveAttachments"/>  : รับไฟล์จาก input file (สูงสุด 3 ไฟล์, ไม่บังคับ) บันทึกลงดิสก์
    /// - <see cref="PrepareBody"/>      : แปลง HTML จาก CKEditor ให้เป็น HTML ที่ Gmail/Outlook แสดงผลได้ตามที่ตกแต่งไว้
    ///                                    (inline style ทุกแท็ก + ฝังรูปในเนื้อหาแบบ cid: ให้เห็นรูปแม้เข้าเว็บ admin ไม่ได้)
    /// - <see cref="Send"/>             : ต่อ SMTP ครั้งเดียวแล้ววนส่งทีละฉบับ เก็บผลรายอีเมล
    /// </summary>
    public static class BroadcastMail
    {
        /// <summary>โฟลเดอร์เก็บไฟล์แนบ (ใต้ wwwroot/Files/) — ใช้ร่วมกันทั้ง 3 เมนู</summary>
        public const string AttachmentFolder = "subscription_news";

        /// <summary>จำนวนไฟล์แนบสูงสุด (ชื่อ input = attachment1..attachment3)</summary>
        public const int MaxAttachments = 3;

        /// <summary>รูปในเนื้อหาที่ใหญ่เกินนี้จะไม่ฝังแบบ inline (ปล่อยเป็น URL แทน) กันอีเมลบวมเกินไป</summary>
        private const long MaxInlineImageBytes = 5 * 1024 * 1024;

        #region ---------- ไฟล์แนบ ----------

        /// <summary>
        /// บันทึกไฟล์แนบจาก input file ชื่อ attachment1..attachment3
        /// คืนค่าเป็นอาร์เรย์ขนาด 3 โดยรักษาตำแหน่งช่องเดิมไว้ (ช่องที่ไม่ได้อัปโหลด = null)
        /// </summary>
        public static SavedAttachment?[] SaveAttachments(IFormCollection collection, string webRootPath)
        {
            var saved = new SavedAttachment?[MaxAttachments];
            string uploadDir = Path.Combine(webRootPath, "Files", AttachmentFolder);

            for (int i = 0; i < MaxAttachments; i++)
            {
                var file = collection.Files["attachment" + (i + 1)];
                if (file == null || file.Length <= 0) continue;

                Directory.CreateDirectory(uploadDir);

                string rawName = Path.GetFileNameWithoutExtension(file.FileName);
                string ext = Path.GetExtension(file.FileName);
                // sanitize ชื่อไฟล์: ตัดอักขระที่ไม่ปลอดภัยใน path/URL ให้เหลือ [a-zA-Z0-9._-] แทนที่ที่เหลือด้วย _
                string safeName = Regex.Replace(rawName, @"[^a-zA-Z0-9._-]+", "_").Trim('_');
                if (string.IsNullOrEmpty(safeName)) safeName = "file";
                if (safeName.Length > 60) safeName = safeName.Substring(0, 60);
                // ตั้งชื่อไฟล์ไม่ให้ชนกัน: <timestamp>_<guid8>_<ชื่อเดิม>
                string storedName = string.Format("{0}_{1}_{2}{3}",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Guid.NewGuid().ToString("N").Substring(0, 8),
                    safeName,
                    ext);

                string fullPath = Path.Combine(uploadDir, storedName);
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                saved[i] = new SavedAttachment
                {
                    StoredName = storedName,
                    FullPath = fullPath,
                    OriginalName = Path.GetFileName(file.FileName)
                };
            }

            return saved;
        }

        #endregion

        #region ---------- แปลงเนื้อหาจาก CKEditor เป็น HTML สำหรับอีเมล ----------

        /// <summary>
        /// แปลง HTML ที่ได้จาก CKEditor ให้เป็นเนื้อหาอีเมล
        /// - inline style ให้ทุกแท็ก (อีเมลไคลเอนต์ไม่โหลด stylesheet ของเว็บ)
        /// - แปลงแท็ก HTML5 (figure/figcaption/mark) เป็น div/span เพื่อให้ Outlook แสดงผลได้
        /// - แปลงสี hsl() เป็น hex
        /// - รูปในเนื้อหา: ถ้าเป็นไฟล์ในเครื่อง (elFinder) หรือ data:base64 จะฝังไปกับอีเมลแบบ cid:
        ///   ถ้าเป็นรูปจากภายนอกจะคงเป็น URL เดิม
        /// </summary>
        public static PreparedMailBody PrepareBody(string editorHtml, string webRootPath, string rootUrl)
        {
            var prepared = new PreparedMailBody();
            editorHtml ??= "";

            var parser = new HtmlParser();
            var doc = parser.ParseDocument("<html><body><div id=\"ck-mail-root\">" + editorHtml + "</div></body></html>");
            var root = doc.GetElementById("ck-mail-root");
            if (root == null)
            {
                prepared.Html = WrapEmailShell(editorHtml);
                prepared.PlainText = "";
                return prepared;
            }

            ConvertMediaEmbeds(doc, root);
            ConvertHtml5Elements(doc, root);
            InlineImagesAndStyles(doc, root, webRootPath, rootUrl, prepared.Images);
            ApplyDefaultStyles(root);
            NormalizeColors(root);

            prepared.Html = WrapEmailShell(root.InnerHtml);
            prepared.PlainText = BuildPlainText(root);
            return prepared;
        }

        /// <summary>วางเนื้อหาลงโครง HTML อีเมล (ตารางกลางหน้า กว้างสูงสุด 640px)</summary>
        private static string WrapEmailShell(string content)
        {
            const string fontStack = "'Kanit','Segoe UI',Tahoma,Arial,sans-serif";
            return
                "<!DOCTYPE html>" +
                "<html><head>" +
                "<meta charset=\"utf-8\" />" +
                "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\" />" +
                "</head>" +
                "<body style=\"margin:0;padding:0;background-color:#f4f5f7;\">" +
                "<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" " +
                    "style=\"margin:0;padding:0;background-color:#f4f5f7;\"><tr>" +
                "<td align=\"center\" style=\"padding:24px 12px;\">" +
                "<table role=\"presentation\" width=\"640\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" " +
                    "style=\"width:100%;max-width:640px;background-color:#ffffff;border-radius:8px;\"><tr>" +
                "<td style=\"padding:28px;font-family:" + fontStack + ";font-size:16px;line-height:1.7;" +
                    "color:#333333;word-break:break-word;\">" +
                content +
                "</td></tr></table>" +
                "</td></tr></table>" +
                "</body></html>";
        }

        /// <summary>mediaEmbed (YouTube ฯลฯ) — อีเมลไม่รองรับ iframe จึงแปลงเป็นลิงก์แทน</summary>
        private static void ConvertMediaEmbeds(IDocument doc, IElement root)
        {
            foreach (var media in root.QuerySelectorAll("oembed, iframe").ToList())
            {
                string url = media.GetAttribute("url") ?? media.GetAttribute("src") ?? "";
                var replacement = doc.CreateElement("p");
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var link = doc.CreateElement("a");
                    link.SetAttribute("href", url);
                    link.TextContent = url;
                    replacement.AppendChild(link);
                }
                // ถ้าอยู่ใน <figure class="media"> ให้แทนที่ทั้ง figure
                var target = (media.ParentElement != null && media.ParentElement.TagName.Equals("FIGURE", StringComparison.OrdinalIgnoreCase))
                    ? media.ParentElement
                    : media;
                target.Replace(replacement);
            }
        }

        /// <summary>
        /// แปลงแท็ก HTML5 ที่อีเมลไคลเอนต์เก่า (Outlook/Word engine) ไม่รู้จัก
        /// figure → div, figcaption → div, mark → span (พร้อมสีไฮไลต์)
        /// </summary>
        private static void ConvertHtml5Elements(IDocument doc, IElement root)
        {
            foreach (var mark in root.QuerySelectorAll("mark").ToList())
            {
                string css = MarkerCss(mark);
                var span = Rename(doc, mark, "span");
                PrependStyle(span, css);
            }

            foreach (var figure in root.QuerySelectorAll("figure").ToList())
            {
                bool isImage = figure.ClassList.Contains("image");
                string css = isImage ? "margin:16px 0;text-align:center;" : "margin:0 0 12px;";
                if (figure.ClassList.Contains("image-style-align-left")) css = "margin:16px 0;text-align:left;";
                if (figure.ClassList.Contains("image-style-align-right")) css = "margin:16px 0;text-align:right;";
                var div = Rename(doc, figure, "div");
                PrependStyle(div, css);
            }

            foreach (var caption in root.QuerySelectorAll("figcaption").ToList())
            {
                var div = Rename(doc, caption, "div");
                PrependStyle(div, "font-size:14px;color:#6b7280;padding-top:6px;");
            }
        }

        /// <summary>สีไฮไลต์ตามคลาสของปุ่ม highlight ใน CKEditor</summary>
        private static string MarkerCss(IElement mark)
        {
            if (mark.ClassList.Contains("marker-green")) return "background-color:#63f963;";
            if (mark.ClassList.Contains("marker-pink")) return "background-color:#fc7999;";
            if (mark.ClassList.Contains("marker-blue")) return "background-color:#72ccfd;";
            if (mark.ClassList.Contains("pen-red")) return "color:#e91313;background-color:transparent;";
            if (mark.ClassList.Contains("pen-green")) return "color:#118800;background-color:transparent;";
            return "background-color:#fdfd77;";
        }

        /// <summary>เปลี่ยนชื่อแท็กโดยคงลูก/แอตทริบิวต์เดิมไว้</summary>
        private static IElement Rename(IDocument doc, IElement element, string newTagName)
        {
            var replacement = doc.CreateElement(newTagName);
            foreach (var attr in element.Attributes.ToList())
            {
                replacement.SetAttribute(attr.Name, attr.Value);
            }
            while (element.FirstChild != null)
            {
                replacement.AppendChild(element.FirstChild);
            }
            element.Replace(replacement);
            return replacement;
        }

        /// <summary>ใส่ style ตั้งต้นไว้ "ข้างหน้า" style เดิม — ค่าที่ผู้ใช้ตั้งเองใน CKEditor จึงชนะเสมอ</summary>
        private static void PrependStyle(IElement element, string css)
        {
            if (string.IsNullOrEmpty(css)) return;
            string existing = (element.GetAttribute("style") ?? "").Trim();
            string merged = css.TrimEnd().TrimEnd(';') + ";";
            if (!string.IsNullOrEmpty(existing))
            {
                merged += existing.EndsWith(";") ? existing : existing + ";";
            }
            element.SetAttribute("style", merged);
        }

        /// <summary>ตารางกำหนด style ตั้งต้นของแต่ละแท็ก (เลียนแบบการแสดงผลใน CKEditor)</summary>
        private static readonly (string Selector, string Css)[] DefaultStyles =
        [
            ("p",          "margin:0 0 12px;"),
            ("h1",         "margin:0 0 12px;font-size:28px;line-height:1.35;font-weight:600;"),
            ("h2",         "margin:0 0 12px;font-size:24px;line-height:1.35;font-weight:600;"),
            ("h3",         "margin:0 0 10px;font-size:20px;line-height:1.4;font-weight:600;"),
            ("h4",         "margin:0 0 10px;font-size:18px;line-height:1.4;font-weight:600;"),
            ("h5",         "margin:0 0 10px;font-size:16px;line-height:1.4;font-weight:600;"),
            ("h6",         "margin:0 0 10px;font-size:15px;line-height:1.4;font-weight:600;"),
            ("ul",         "margin:0 0 12px;padding-left:24px;"),
            ("ol",         "margin:0 0 12px;padding-left:24px;"),
            ("li",         "margin:0 0 6px;"),
            ("a",          "color:#0047B6;text-decoration:underline;"),
            ("blockquote", "margin:0 0 12px;padding:8px 16px;border-left:4px solid #dfe2e5;color:#555555;"),
            ("hr",         "border:0;border-top:1px solid #e3e6ea;margin:20px 0;"),
            ("table",      "border-collapse:collapse;width:100%;margin:0 0 12px;"),
            ("th",         "border:1px solid #d6dae0;padding:8px 10px;background-color:#f2f4f7;text-align:left;"),
            ("td",         "border:1px solid #d6dae0;padding:8px 10px;"),
            ("pre",        "margin:0 0 12px;padding:12px;background-color:#f5f6f8;border-radius:4px;" +
                           "font-family:'Courier New',Courier,monospace;font-size:14px;white-space:pre-wrap;"),
            ("code",       "font-family:'Courier New',Courier,monospace;font-size:14px;"),
            ("img",        "max-width:100%;height:auto;border:0;outline:none;text-decoration:none;"),
            // ขนาดตัวอักษรแบบคลาสของ CKEditor (กรณีตั้งค่า fontSize เป็น named size)
            ("span.text-tiny",  "font-size:10px;"),
            ("span.text-small", "font-size:14px;"),
            ("span.text-big",   "font-size:20px;"),
            ("span.text-huge",  "font-size:28px;"),
        ];

        private static void ApplyDefaultStyles(IElement root)
        {
            foreach (var (selector, css) in DefaultStyles)
            {
                foreach (var element in root.QuerySelectorAll(selector).ToList())
                {
                    PrependStyle(element, css);
                }
            }
        }

        /// <summary>hsl()/hsla() ในค่า style → hex (Outlook ไม่รู้จัก hsl)</summary>
        private static void NormalizeColors(IElement root)
        {
            foreach (var element in root.QuerySelectorAll("[style]").ToList())
            {
                string style = element.GetAttribute("style") ?? "";
                string converted = HslRegex.Replace(style, m =>
                {
                    if (double.TryParse(m.Groups[1].Value, out double h) &&
                        double.TryParse(m.Groups[2].Value, out double s) &&
                        double.TryParse(m.Groups[3].Value, out double l))
                    {
                        return HslToHex(h, s, l);
                    }
                    return m.Value;
                });
                if (converted != style)
                {
                    element.SetAttribute("style", converted);
                }
            }
        }

        private static readonly Regex HslRegex = new(
            @"hsla?\(\s*([\d.]+)\s*,\s*([\d.]+)%\s*,\s*([\d.]+)%\s*(?:,\s*[\d.]+\s*)?\)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static string HslToHex(double h, double s, double l)
        {
            s /= 100.0;
            l /= 100.0;
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs(((h / 60.0) % 2) - 1));
            double m = l - c / 2;
            double r = 0, g = 0, b = 0;
            switch ((int)(h / 60) % 6)
            {
                case 0: r = c; g = x; break;
                case 1: r = x; g = c; break;
                case 2: g = c; b = x; break;
                case 3: g = x; b = c; break;
                case 4: r = x; b = c; break;
                default: r = c; b = x; break;
            }
            int R = (int)Math.Round((r + m) * 255);
            int G = (int)Math.Round((g + m) * 255);
            int B = (int)Math.Round((b + m) * 255);
            return string.Format("#{0:x2}{1:x2}{2:x2}",
                Math.Clamp(R, 0, 255), Math.Clamp(G, 0, 255), Math.Clamp(B, 0, 255));
        }

        /// <summary>
        /// รูปในเนื้อหา: ไฟล์ในเครื่อง (elFinder) และ data:base64 → ฝังไปกับอีเมลแบบ cid:
        /// รูปจากภายนอก → คง URL เดิม / รูปที่เป็น path สัมพัทธ์แต่หาไฟล์ไม่เจอ → เติมโดเมนให้เป็น URL เต็ม
        /// </summary>
        private static void InlineImagesAndStyles(IDocument doc, IElement root, string webRootPath, string rootUrl, List<InlineImage> images)
        {
            var cidBySource = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var img in root.QuerySelectorAll("img").ToList())
            {
                string src = (img.GetAttribute("src") ?? "").Trim();
                if (string.IsNullOrEmpty(src)) continue;

                // srcset ของ CKEditor ชี้ไปที่รูปย่อของเว็บ — อีเมลไม่ต้องใช้ ตัดทิ้งกัน client เลือกไฟล์ผิด
                img.RemoveAttribute("srcset");
                img.RemoveAttribute("sizes");

                if (cidBySource.TryGetValue(src, out string? existingCid))
                {
                    img.SetAttribute("src", "cid:" + existingCid);
                    continue;
                }

                InlineImage? inline = null;

                if (src.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                {
                    inline = FromDataUri(src);
                }
                else
                {
                    string? localPath = MapToLocalFile(src, webRootPath, rootUrl);
                    if (localPath != null)
                    {
                        try
                        {
                            var info = new FileInfo(localPath);
                            if (info.Exists && info.Length > 0 && info.Length <= MaxInlineImageBytes)
                            {
                                inline = new InlineImage
                                {
                                    Content = File.ReadAllBytes(localPath),
                                    MimeType = MimeTypeOf(Path.GetExtension(localPath)),
                                    FileName = Path.GetFileName(localPath)
                                };
                            }
                        }
                        catch
                        {
                            inline = null; // อ่านไฟล์ไม่ได้ → ปล่อยเป็น URL ต่อไป
                        }
                    }
                }

                if (inline != null)
                {
                    inline.Cid = MimeUtils.GenerateMessageId();
                    images.Add(inline);
                    cidBySource[src] = inline.Cid;
                    img.SetAttribute("src", "cid:" + inline.Cid);
                }
                else if (src.StartsWith("/") && !string.IsNullOrEmpty(rootUrl))
                {
                    // path สัมพัทธ์ที่ฝังรูปไม่ได้ → อย่างน้อยทำให้เป็น URL เต็มเพื่อให้เปิดจากอีเมลได้
                    img.SetAttribute("src", rootUrl.TrimEnd('/') + src);
                }
            }
        }

        private static InlineImage? FromDataUri(string src)
        {
            var match = Regex.Match(src, @"^data:(image/[a-z0-9.+-]+);base64,(.+)$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!match.Success) return null;
            try
            {
                byte[] bytes = Convert.FromBase64String(match.Groups[2].Value.Trim());
                if (bytes.Length == 0 || bytes.Length > MaxInlineImageBytes) return null;
                string mime = match.Groups[1].Value.ToLowerInvariant();
                string ext = mime.Replace("image/", "").Replace("jpeg", "jpg").Replace("svg+xml", "svg");
                return new InlineImage
                {
                    Content = bytes,
                    MimeType = mime,
                    FileName = "image_" + Guid.NewGuid().ToString("N").Substring(0, 8) + "." + ext
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// แปลง src ของรูปเป็น path ไฟล์จริงใต้ wwwroot (เฉพาะรูปที่อยู่บนโดเมน admin เอง)
        /// คืน null เมื่อไม่ใช่ไฟล์ในเครื่อง
        /// </summary>
        private static string? MapToLocalFile(string src, string webRootPath, string rootUrl)
        {
            if (string.IsNullOrEmpty(webRootPath)) return null;

            string relative;
            if (src.StartsWith("/"))
            {
                relative = src;
            }
            else if (Uri.TryCreate(src, UriKind.Absolute, out var absolute))
            {
                if (!Uri.TryCreate(rootUrl, UriKind.Absolute, out var root)) return null;
                if (!string.Equals(absolute.Host, root.Host, StringComparison.OrdinalIgnoreCase)) return null;
                if (absolute.Port != root.Port) return null;
                relative = absolute.AbsolutePath;
            }
            else
            {
                return null;
            }

            // ตัด query/hash แล้ว decode %xx (ชื่อไฟล์ภาษาไทยจาก elFinder)
            relative = relative.Split('?')[0].Split('#')[0];
            try
            {
                relative = Uri.UnescapeDataString(relative);
            }
            catch
            {
                return null;
            }

            relative = relative.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            if (relative.Contains("..")) return null;

            string candidate = Path.GetFullPath(Path.Combine(webRootPath, relative));
            string rootFull = Path.GetFullPath(webRootPath);
            // กัน path traversal — ต้องอยู่ใต้ wwwroot เท่านั้น
            if (!candidate.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase)) return null;

            return File.Exists(candidate) ? candidate : null;
        }

        private static string MimeTypeOf(string extension)
        {
            switch ((extension ?? "").ToLowerInvariant())
            {
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".webp": return "image/webp";
                case ".bmp": return "image/bmp";
                case ".svg": return "image/svg+xml";
                case ".ico": return "image/x-icon";
                default: return "image/jpeg";
            }
        }

        /// <summary>เนื้อหาแบบข้อความล้วน (multipart/alternative) — ช่วยเรื่อง deliverability และไคลเอนต์ที่ปิด HTML</summary>
        private static string BuildPlainText(IElement root)
        {
            var text = new StringBuilder();
            foreach (var block in root.QuerySelectorAll("p, li, h1, h2, h3, h4, h5, h6, td, blockquote, div"))
            {
                string line = (block.TextContent ?? "").Trim();
                if (line.Length > 0 && block.QuerySelector("p, li, h1, h2, h3, h4, h5, h6, td, blockquote, div") == null)
                {
                    text.AppendLine(line);
                }
            }
            if (text.Length == 0)
            {
                text.Append((root.TextContent ?? "").Trim());
            }
            return Regex.Replace(text.ToString(), @"[ \t]+", " ").Trim();
        }

        #endregion

        #region ---------- ส่งอีเมล ----------

        /// <summary>
        /// ต่อ SMTP ครั้งเดียวแล้ววนส่งทีละฉบับ (ผู้รับไม่เห็นกัน) เก็บผลสำเร็จ/ไม่สำเร็จรายอีเมล
        /// </summary>
        public static BroadcastSendResult Send(
            IConfiguration config,
            List<string> emailList,
            string subject,
            PreparedMailBody body,
            SavedAttachment?[] attachments)
        {
            var result = new BroadcastSendResult();
            if (emailList == null || emailList.Count == 0) return result;

            var mailSettings = config.GetSection("MailSettings");
            string fromMail = mailSettings["FromMail"] ?? "";
            string displayName = mailSettings["DisplayName"] ?? "";
            string smtpHost = mailSettings["Host"] ?? "";
            string smtpUser = mailSettings["UserName"] ?? "";
            string smtpPass = mailSettings["Password"] ?? "";
            int smtpPort = int.TryParse(mailSettings["Port"], out int p) ? p : 587;
            bool useSSL = bool.TryParse(mailSettings["UseSSL"], out bool ssl) && ssl;

            using var smtpClient = new SmtpClient();
            try
            {
                var socketOptions = useSSL
                    ? MailKit.Security.SecureSocketOptions.SslOnConnect
                    : MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;

                smtpClient.Connect(smtpHost, smtpPort, socketOptions);
                smtpClient.Authenticate(smtpUser, smtpPass);

                foreach (string email in emailList)
                {
                    try
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(displayName, fromMail));
                        message.To.Add(MailboxAddress.Parse(email));
                        message.Subject = subject;

                        var builder = new BodyBuilder();
                        body.ApplyTo(builder);

                        if (attachments != null)
                        {
                            foreach (var file in attachments)
                            {
                                if (file == null || string.IsNullOrEmpty(file.FullPath)) continue;
                                if (!File.Exists(file.FullPath)) continue;
                                var attachment = builder.Attachments.Add(file.FullPath);
                                // ให้ผู้รับเห็นชื่อไฟล์เดิมที่อัปโหลด ไม่ใช่ชื่อที่ตั้งใหม่กันไฟล์ชนกัน
                                if (!string.IsNullOrEmpty(file.OriginalName))
                                {
                                    attachment.ContentDisposition ??= new ContentDisposition(ContentDisposition.Attachment);
                                    attachment.ContentDisposition.FileName = file.OriginalName;
                                    attachment.ContentType.Name = file.OriginalName;
                                }
                            }
                        }

                        message.Body = builder.ToMessageBody();

                        smtpClient.Send(message);
                        result.SuccessCount++;
                        result.Results.Add(new EmailSendResult { Email = email, Success = true });
                    }
                    catch (Exception ex)
                    {
                        result.FailCount++;
                        result.Results.Add(new EmailSendResult { Email = email, Success = false, ErrorMessage = ex.Message });
                    }
                }

                smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                int alreadyProcessed = result.Results.Count;
                for (int i = alreadyProcessed; i < emailList.Count; i++)
                {
                    result.Results.Add(new EmailSendResult
                    {
                        Email = emailList[i],
                        Success = false,
                        ErrorMessage = "Connection error: " + ex.Message
                    });
                }
                result.FailCount = emailList.Count - result.SuccessCount;
            }

            return result;
        }

        #endregion
    }
}
