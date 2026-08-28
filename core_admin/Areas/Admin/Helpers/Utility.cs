using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;
using System.Text;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class Utility
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Utility(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
        }

        public void writeLogs(string txt, string path = "")
        {
            if (path == null || path.Trim() == "")
            {
                path = _hostingEnvironment.ContentRootPath + "/Logs/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            }
            else
            {
                path = _hostingEnvironment.ContentRootPath + "/Logs/" + path;
            }

            System.IO.DirectoryInfo di = Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + "/Logs/");

            using (System.IO.StreamWriter w = new System.IO.StreamWriter(path, true))
            {
                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ":" + txt);
                w.Close();
            }
        }

        public string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            //return result.ToString();
            return result.ToString().ToLower();
        }

        public bool isInt(string n = "")
        {
            int value;
            return int.TryParse(n, out value);
        }

        public string rootURL()
        {
            return _config.GetSection("RootURL").Value;
        }

        public string frontURL()
        {
            return _config.GetSection("FrontURL").Value;
        }

        public string frontURL(string path = "")
        {
            if (path == null)
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/";
            }
            else if (path.Trim() != "")
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/" + (path.TrimStart('/'));
            }
            else
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/";
            }
        }

        public string frontURLMicrosite(string path = "")
        {
            if (path == null)
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/";
            }
            else if (path.Trim() != "")
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/web/" + (path.TrimStart('/'));
            }
            else
            {
                return _config.GetSection("FrontURL").Value.TrimEnd('/') + "/";
            }
        }


        public string appKey()
        {
            return _config.GetSection("AppKey").Value;
        }

        public string dateFormat(string date = "")
        {
            var dateArr = date.Split('-');
            if (dateArr.Length == 3 && isInt(dateArr[0]) && isInt(dateArr[1]) && isInt(dateArr[2]))
            {
                return string.Format("{0}/{1}/{2}", dateArr[2], dateArr[1], dateArr[0]);
            }
            else
            {
                return date;
            }
        }

        public string strYMD2DMY(string ymd, string split = "-", bool buddhaYears = false)
        {
            try
            {
                var ar = ymd.Split(split);
                int year = Convert.ToInt32(ar[0]);
                if (buddhaYears == true)
                {
                    year = year + 543;
                }
                return string.Format("{1}{0}{2}{0}{3}", split, ar[2], ar[1], year);
            }
            catch
            {
                return ymd;
            }
        }

        public string Encrypt(string value, string keyString)
        {
            if (string.IsNullOrEmpty(value)) return value;
            try
            {
                var key = Encoding.UTF8.GetBytes(keyString);

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(value);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            var str = Convert.ToBase64String(result);
                            var fullCipher = Convert.FromBase64String(str);
                            return str;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string Decrypt(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public bool Email(string MailTo = "", string mailTitle = "", string mailBody = "", List<string> fileArray = null, string MailCC = "")
        {
            #region ############ Mail Setting ################
            var Data_MailSettings = _config.GetSection("MailSettings");
            string FromMail = Data_MailSettings.GetSection("FromMail").Value.ToString();
            string DisplayName = Data_MailSettings.GetSection("DisplayName").Value.ToString();
            string Host = Data_MailSettings.GetSection("Host").Value.ToString();
            string UserName = Data_MailSettings.GetSection("UserName").Value.ToString();
            string Password = Data_MailSettings.GetSection("Password").Value.ToString();
            int Port = 587;
            bool UseSSL = false;
            if (Data_MailSettings.GetSection("Port").Value.ToString().Trim() != "")
            {
                Port = Convert.ToInt32(Data_MailSettings.GetSection("Port").Value.ToString());
            }
            if (Data_MailSettings.GetSection("UseSSL").Value.ToString().Trim() != "")
            {
                UseSSL = Convert.ToBoolean(Data_MailSettings.GetSection("UseSSL").Value.ToString());
            }
            #endregion

            try
            {
                var message = new MimeMessage();
                message.Subject = mailTitle.ToString();

                message.From.Add(new MailboxAddress(DisplayName.ToString(), FromMail.ToString()));

                #region ############ Mail To ################
                if (MailTo.ToString().Trim() != "")
                {
                    var Mail_To_Array = MailTo.Split(',');
                    for (int i = 0; i < Mail_To_Array.Length; i++)
                    {
                        if ((Mail_To_Array[i] + "").ToString().Trim() != "")
                        {
                            message.To.Add(new MailboxAddress((Mail_To_Array[i] + "").ToString(), (Mail_To_Array[i] + "").ToString()));
                        }
                    }
                }
                #endregion

                #region ############ Mail CC ################
                if (MailCC.ToString().Trim() != "")
                {
                    var Mail_CC_Array = MailCC.Split(',');
                    for (int i = 0; i < Mail_CC_Array.Length; i++)
                    {
                        if ((Mail_CC_Array[i] + "").ToString().Trim() != "")
                        {
                            message.Cc.Add(new MailboxAddress((Mail_CC_Array[i] + "").ToString(), (Mail_CC_Array[i] + "").ToString()));
                        }
                    }
                }
                #endregion

                #region ############ Mail Body ################
                /*message.Body = new TextPart("plain")
                {
                    Text = "Hello,My First Demo Mail it is.Thanks",
                };*/
                var builder = new BodyBuilder();
                builder.HtmlBody = mailBody.ToString().Trim();
                //builder.TextBody = "-";

                #region ############ File Attachments ################
                if (fileArray != null)
                {
                    foreach (string r_File_Attachment in fileArray)
                    {
                        if (r_File_Attachment.ToString().Trim() != "")
                        {
                            string Data_File_Attachment = r_File_Attachment.ToString();
                            string File_Attachment_Name = Data_File_Attachment.ToString().Split("/").Last();
                            byte[] File_Attachment_Bytes = System.IO.File.ReadAllBytes(Data_File_Attachment.ToString());
                            builder.Attachments.Add(File_Attachment_Name.ToString(), File_Attachment_Bytes);
                        }
                    }
                }
                #endregion

                message.Body = builder.ToMessageBody();
                #endregion

                using (var client = new SmtpClient())
                {
                    client.Connect(Host.ToString(), Port, UseSSL);
                    client.Authenticate(UserName.ToString(), Password.ToString());

                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception e)
            {
                string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                writeLogs(eMessage.ToString(), "Logs_EMail_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

                return false;
            }
        }

        public string GenerateSlugURL(int length)
        {
            long currentTimeTicks = DateTime.UtcNow.Ticks;
            string cleanNumber = new string(currentTimeTicks.ToString().Where(char.IsDigit).ToArray());
            long numericValue = long.Parse(cleanNumber);
            string id = BaseConvert(numericValue, 10, 36);

            if (length > id.Length)
            {
                id = id.PadLeft(length, '0');
            }
            else if (length < id.Length)
            {
                // If the specified length is less than the actual slug length,
                // take the rightmost characters to match the specified length.
                id = id.Substring(id.Length - length);
            }

            return id;
        }

        private string BaseConvert(long number, int fromBase, int toBase)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            string result = string.Empty;
            bool isNegative = number < 0;
            number = Math.Abs(number);

            while (number > 0)
            {
                result = chars[(int)(number % toBase)] + result;
                number /= toBase;
            }

            return isNegative ? "-" + result : result;
        }

    }
}
