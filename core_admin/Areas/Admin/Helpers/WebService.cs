using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class WebService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _context;
        private readonly ISession _session;
        private Utility _utility;

        public WebService(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig, IHttpContextAccessor iContext)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _context = iContext;
            _session = iContext.HttpContext.Session;
            _utility = new Utility(_hostingEnvironment, _config);
        }

        public string RequestData(string param_url, string json, string param_Method = "POST")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            string url = _config.GetSection("API").GetSection("URL").Value.ToString() + "/" + param_url.ToString().Trim();
            string access_token_txt = _session.GetString("Token").ToString();

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token_txt;
                    client.Encoding = Encoding.UTF8;
                    if (json != "")
                    {
                        var result = "";
                        if (param_Method.ToString().ToUpper() == "POST")
                        {
                            result = client.UploadString(url.ToString(), "POST", json);
                        }
                        else
                        {
                            result = client.DownloadString(url.ToString() + "?" + json.ToString());
                        }
                        return result;
                    }
                    else
                    {
                        var result = "";
                        if (param_Method.ToString().ToUpper() == "POST")
                        {
                            result = client.UploadString(url.ToString(), "POST");
                        }
                        else
                        {
                            result = client.DownloadString(url.ToString());
                        }
                        return result;
                    }
                }
            }
            catch (WebException e)
            {
                string jsonResponse = "response body is not json format.";
                try
                {
                    using (StreamReader sr = new StreamReader(e.Response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd();
                        if (IsValidJson(result))
                        {
                            jsonResponse = result;
                        }
                    }
                }
                catch
                {

                }

                var url_ar = url.Split('/');
                var f_name = url_ar[url_ar.Length - 1];

                string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                string TextError = "";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Request URL : " + url;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Json Body : " + json;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Function : " + f_name;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Json Response : " + jsonResponse;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Method : " + param_Method;
                _utility.writeLogs(TextError.ToString());

                var json_error = new Dictionary<string, object>() {
                    { "StatusCode", "40" },
                    { "Message", string.Format("เกิดข้อผิดพลาด ระหว่างการเรียกข้อมูล ({1}, {0})", e.Message, f_name) }
                };
                return JsonConvert.SerializeObject(json_error);
            }
        }

        public byte[] RequestDownload(string param_url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            param_url = param_url.TrimStart('/');
            string url = _config.GetSection("API").GetSection("URL").Value.ToString() + "/" + param_url.ToString().Trim();
            string access_token_txt = _session.GetString("Token").ToString();

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token_txt;
                    client.Encoding = Encoding.UTF8;
                    return client.DownloadData(url.ToString());
                }
            }
            catch (WebException e)
            {
                string jsonResponse = "response body is not json format.";
                try
                {
                    using (StreamReader sr = new StreamReader(e.Response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd();
                        if (IsValidJson(result))
                        {
                            jsonResponse = result;
                        }
                        else
                        {
                            jsonResponse = result;
                        }
                    }
                }
                catch
                {

                }

                var url_ar = url.Split('/');
                var f_name = url_ar[url_ar.Length - 1];

                string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                string TextError = "";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Request URL : " + url;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Function : " + f_name;
                TextError += "\r\n";
                TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Json Response : " + jsonResponse;
                TextError += "\r\n";
                _utility.writeLogs(TextError.ToString());

                /*var json_error = new Dictionary<string, object>() {
                    { "StatusCode", "40" },
                    { "Message", string.Format("เกิดข้อผิดพลาด ระหว่างการเรียกข้อมูล ({1}, {0})", e.Message, f_name) }
                };*/
                //return JsonConvert.SerializeObject(json_error);

                throw new Exception(e.Message);
            }
        }

        public string RequestToken()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            bool SetToken = false;
            if (string.IsNullOrEmpty(_session.GetString("Token")) == true || string.IsNullOrEmpty(_session.GetString("TokenExpiration")) == true)
            {
                SetToken = true;
            }
            else if (string.IsNullOrEmpty(_session.GetString("TokenExpiration")) != true)
            {
                DateTime TokenExpiration = Convert.ToDateTime(_session.GetString("TokenExpiration"));
                if (TokenExpiration <= System.DateTime.Now)
                {
                    SetToken = true;
                }
            }

            if (SetToken == true)
            {
                string url = _config.GetSection("API").GetSection("URL").Value.ToString() + "/Authenticate/login";

                string json = JsonConvert.SerializeObject(new Dictionary<string, object> {
                    { "username", _config.GetSection("API").GetSection("Username").Value.ToString() },
                    { "password", _config.GetSection("API").GetSection("Password").Value.ToString() }
                });

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Encoding = Encoding.UTF8;

                        var result = client.UploadString(url, "POST", json);
                        JObject dataToken = JObject.Parse(result);

                        _session.SetString("Token", (dataToken["token"] + "").ToString());
                        _session.SetString("TokenExpiration", Convert.ToDateTime(dataToken["expiration"] + "").ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"));

                        return (dataToken["token"] + "").ToString();
                    }
                }
                catch (WebException e)
                {
                    string jsonResponse = "response body is not json format.";
                    try
                    {
                        using (StreamReader sr = new StreamReader(e.Response.GetResponseStream()))
                        {
                            var result = sr.ReadToEnd();
                            if (IsValidJson(result))
                            {
                                jsonResponse = result;
                            }
                        }
                    }
                    catch
                    {

                    }

                    var url_ar = url.Split('/');
                    var f_name = url_ar[url_ar.Length - 1];

                    string eMessage = System.Text.RegularExpressions.Regex.Replace(e.Message, @"\t|\n|\r", "");
                    string TextError = "";
                    TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Request URL : " + url;
                    //TextError += "\r\n";
                    //TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Json Body : " + json;
                    TextError += "\r\n";
                    TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Function : " + f_name;
                    TextError += "\r\n";
                    TextError += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + eMessage + " | Json Response : " + jsonResponse;
                    _utility.writeLogs(TextError.ToString());

                    var json_error = new Dictionary<string, object>() {
                        { "StatusCode", "40" },
                        { "Message", string.Format("เกิดข้อผิดพลาด ระหว่างการเรียกข้อมูล ({1}, {0})", e.Message, f_name) }
                    };
                    return JsonConvert.SerializeObject(json_error);
                }
            }
            else
            {
                return "";
            }
        }

        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
