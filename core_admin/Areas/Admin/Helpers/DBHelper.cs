using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class DBHelper
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private Utility _utility;
        private static string defaultConnectionString = "";
        private readonly bool _enableSqlDebugLog;
        public DBHelper(IWebHostEnvironment hostingEnvironment, IConfiguration iConfig)
        {
            _config = iConfig;
            _hostingEnvironment = hostingEnvironment;
            _utility = new Utility(hostingEnvironment, iConfig);
            _enableSqlDebugLog = _config.GetValue<bool>("EnableSqlDebugLog");

            var DBConnection = _config.GetSection("DBConnection");
            defaultConnectionString = string.Format(
                "Server={0};Database={1};User ID={2};Password={3};TrustServerCertificate=True;MultipleActiveResultSets=True;",
                DBConnection.GetSection("Server").Value,
                DBConnection.GetSection("Database").Value,
                DBConnection.GetSection("Username").Value,
                DBConnection.GetSection("Password").Value);
        }
        public string DefaultConnectionString
        {
            get
            {
                return defaultConnectionString;
            }
            /*set
            {
                defaultConnectionString = value;
            }*/
        }
        /// <summary>
        /// ทำให้คอลัมน์เวลาหน้าตาเหมือนเดิมสมัยใช้ PostgreSQL
        ///
        /// คอลัมน์ <c>datetimeoffset</c> ของ SQL Server อ่านออกมาเป็น <see cref="DateTimeOffset"/>
        /// แต่โค้ดทั้งระบบ (View/Controller) รับเป็น <see cref="DateTime"/> แบบ UTC แล้วค่อยเรียก
        /// <c>.ToLocalTime()</c> เอง ซึ่งเป็นพฤติกรรมที่ Npgsql เคยให้ไว้
        /// จึงแปลงกลับตรงนี้จุดเดียว แทนการไล่แก้ทุกจุดที่ cast เป็น DateTime
        /// </summary>
        private static void NormalizeDateTimeColumns(DataTable dt)
        {
            var targets = dt.Columns.Cast<DataColumn>()
                            .Where(c => c.DataType == typeof(DateTimeOffset))
                            .ToList();

            foreach (var col in targets)
            {
                int ordinal = col.Ordinal;
                string name = col.ColumnName;
                string temp = name + "__utc";

                var replacement = new DataColumn(temp, typeof(DateTime));
                dt.Columns.Add(replacement);

                foreach (DataRow row in dt.Rows)
                {
                    row[temp] = row[name] == DBNull.Value
                        ? (object)DBNull.Value
                        : ((DateTimeOffset)row[name]).UtcDateTime;
                }

                dt.Columns.Remove(col);
                replacement.ColumnName = name;
                replacement.SetOrdinal(ordinal);
            }

            if (targets.Count > 0) dt.AcceptChanges();
        }

        /// <summary>
        /// เตรียมค่าก่อนส่งเป็นพารามิเตอร์
        ///
        /// คอลัมน์เวลาของระบบเป็น <c>datetimeoffset</c> (แปลงมาจาก <c>timestamptz</c> ของ PostgreSQL)
        /// ถ้าส่ง <see cref="System.DateTime"/> เข้าไปตรง ๆ SQL Server จะถือว่า offset เป็น +00:00
        /// ทำให้เวลาที่บันทึกเพี้ยนไป 7 ชั่วโมงเทียบกับข้อมูลเดิม จึงผูก offset ของเครื่องให้ก่อน
        /// (PostgreSQL เดิมใช้ TimeZone ของ session เติมให้อัตโนมัติ)
        /// </summary>
        private static object? ToParamValue(object? value)
        {
            if (value is DateTime dt)
            {
                return dt.Kind == DateTimeKind.Utc
                    ? new DateTimeOffset(dt)
                    : new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Unspecified),
                                         TimeZoneInfo.Local.GetUtcOffset(dt));
            }
            return value;
        }

        /// <summary>ชื่อตารางสำหรับ T-SQL — เติม prefix 2026_ และครอบ [] (ดู <see cref="Db.T"/>)</summary>
        private static string QuoteTable(string table) => Db.T(table);

        public DataTable ExecuteQuery(string query)
        {
            var parameters = new Dictionary<string, object>();
            return ExecuteQuery(query, parameters);
        }
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            #region ----- debug log -----
            if (_enableSqlDebugLog && _hostingEnvironment.IsDevelopment() && !query.Contains("web_admin_module") && !query.Contains("web_admin_access"))
            {
                Console.WriteLine("==============================================================");
                Console.WriteLine($"{System.DateTime.Now}");
                Console.WriteLine($"ExecuteQuery:{query}");
                Console.WriteLine(JsonConvert.SerializeObject(parameters));
                Console.WriteLine("==============================================================");
            }
            #endregion


            if (parameters == null)
                parameters = new Dictionary<string, object>();
            try
            {
                DataTable a = new DataTable();
                List<SqlParameter> filters = new List<SqlParameter>();
                if (parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        filters.Add(new SqlParameter(item.Key, ToParamValue(item.Value) ?? DBNull.Value));
                    }
                }
                a = Query(query, filters);

                string eMessage = System.Text.RegularExpressions.Regex.Replace(query, @"\t|\n|\r", "");

                string kv = "";
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }

                return a;
            }
            catch (Exception ex)
            {
                #region Log error
                string kv = "";
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }

                string eMessage = System.Text.RegularExpressions.Regex.Replace(ex.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage + " - " + query + "\r\n" + "parameter - " + kv);
                #endregion

                throw ex;
            }
        }
        public int ExecuteNonQuery(string query)
        {
            var parameters = new Dictionary<string, object>();
            return ExecuteNonQuery(query, parameters);
        }
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                List<SqlParameter> filters = new List<SqlParameter>();

                if (parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        filters.Add(new SqlParameter(item.Key, ToParamValue(item.Value) ?? DBNull.Value));
                    }
                }

                return NonQuery(query, filters);
            }
            catch (Exception ex)
            {
                #region Log error
                string kv = "";
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }

                string eMessage = System.Text.RegularExpressions.Regex.Replace(ex.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage + " - " + query + "\r\n" + "parameter - " + kv);
                #endregion

                throw ex;
            }
        }
        public int Insert(string table, Dictionary<string, object> parameters)
        {
            string query = "";
            try
            {
                if (parameters.Count == 0)
                    throw new ArgumentException("Insert is required parameters.");

                string escapedTableName = QuoteTable(table);

                #region Create sql insert command
                string field = "";
                string val = "";
                
                if (parameters.Count > 0)
                {
                    field += "(";
                    int i = 0;
                    foreach (var item in parameters)
                    {
                        if (i != 0) { field += ","; }
                        field += "" + item.Key + "";
                        i++;
                    }
                    field += ")";

                    val += "(";
                    int v = 0;
                    foreach (var item in parameters)
                    {
                        if (v != 0) { val += ","; }
                        val += "@" + item.Key + "";
                        v++;
                    }
                    val += " ) ";

                    query += string.Format("INSERT INTO {0} {1} VALUES {2}", escapedTableName, field, val);

                }
                #endregion

                List<SqlParameter> filters = new List<SqlParameter>();


                if (parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        filters.Add(new SqlParameter(item.Key, ToParamValue(item.Value) ?? DBNull.Value));
                    }
                }

                #region ----- debug log -----
                if (_enableSqlDebugLog && _hostingEnvironment.IsDevelopment() && !query.Contains("web_admin_module") && !query.Contains("web_admin_access"))
                {
                    Console.WriteLine("==============================================================");
                    Console.WriteLine($"{System.DateTime.Now}");
                    Console.WriteLine($"Insert:{query}");
                    Console.WriteLine(JsonConvert.SerializeObject(parameters));
                    Console.WriteLine("==============================================================");
                }
                #endregion

                return NonQuery(query, filters);
            }
            catch (Exception ex)
            {
                #region Log error
                string kv = "";
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }

                string eMessage = System.Text.RegularExpressions.Regex.Replace(ex.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage + " - " + query + "\r\n" + "parameter - " + kv);
                #endregion

                throw ex;
            }
        }
        public int Update(string table, string condition, Dictionary<string, object> parametersFieldValue, Dictionary<string, object> parametersCondition)
        {
            string query = "";
            try
            {
                string escapedTableName = QuoteTable(table);

                #region Create sql update command
                string field_val = "";

                if (parametersFieldValue.Count > 0)
                {
                    int v = 0;
                    foreach (var item in parametersFieldValue)
                    {
                        if (v != 0) { field_val += ","; }
                        field_val += "" + item.Key + " = @" + item.Key + "";
                        v++;
                    }

                    query += string.Format("UPDATE {0} SET {1} {2}", escapedTableName, field_val, condition);

                }
                #endregion

                List<SqlParameter> filters = new List<SqlParameter>();


                if (parametersFieldValue.Count > 0)
                {
                    foreach (var item in parametersFieldValue)
                    {
                        filters.Add(new SqlParameter(item.Key, ToParamValue(item.Value) ?? DBNull.Value));
                    }
                }

                if (parametersCondition.Count > 0)
                {
                    foreach (var item in parametersCondition)
                    {
                        filters.Add(new SqlParameter(item.Key, ToParamValue(item.Value) ?? DBNull.Value));
                    }
                }

                #region Logs test
                if (false)
                {
                    string kv = "";
                    if (parametersFieldValue != null && parametersFieldValue.Count > 0)
                    {
                        foreach (var item in parametersFieldValue)
                        {
                            kv += item.Key + "=" + item.Value + ",";
                        }
                    }
                    _utility.writeLogs("log test UPDATE - " + query + "\r\n" + "parameter - " + kv);
                }
                #endregion

                #region ----- debug log -----
                if (_enableSqlDebugLog && _hostingEnvironment.IsDevelopment() && !query.Contains("web_admin_module") && !query.Contains("web_admin_access"))
                {
                    Console.WriteLine("==============================================================");
                    Console.WriteLine($"{System.DateTime.Now}");
                    Console.WriteLine($"Update:{query}");
                    Console.WriteLine(JsonConvert.SerializeObject(parametersFieldValue));
                    Console.WriteLine(JsonConvert.SerializeObject(parametersCondition));
                    Console.WriteLine("==============================================================");
                }
                #endregion

                return NonQuery(query, filters);
            }
            catch (Exception ex)
            {
                #region Log error
                string kv = "";
                if (parametersFieldValue != null && parametersFieldValue.Count > 0)
                {
                    foreach (var item in parametersFieldValue)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }
                if (parametersCondition != null && parametersCondition.Count > 0)
                {
                    kv += "\r\n";
                    foreach (var item in parametersCondition)
                    {
                        kv += item.Key + "=" + item.Value + ",";
                    }
                }

                string eMessage = System.Text.RegularExpressions.Regex.Replace(ex.Message, @"\t|\n|\r", "");
                _utility.writeLogs(eMessage + " - " + query + "\r\n" + "parameter - " + kv);
                #endregion

                throw ex;
            }
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return JSONString;
        }

        /// <summary>
        /// JSON สำหรับฝังใน &lt;script&gt; ของ View โดยตรง (เช่น <c>var jsonRow = @Html.Raw(...)</c>)
        ///
        /// ⚠️ ห้ามใช้ <see cref="DataTableToJSONWithJSONNet"/> กับกรณีนี้ — ค่าจาก DB ที่มี <c>&lt;/script&gt;</c>
        /// (มาจากฟอร์มหน้าเว็บ / ไฟล์ Import) จะปิด tag แล้วรันสคริปต์ที่ผู้โจมตีแทรกมาในเบราว์เซอร์ของแอดมิน
        /// (stored XSS) — <c>StringEscapeHandling.EscapeHtml</c> แปลง &lt; &gt; &amp; ' " เป็น \uXXXX
        /// ซึ่งยังเป็น JSON ที่ถูกต้องและ decode กลับได้ค่าเดิมทุกตัวอักษร
        /// </summary>
        public string DataTableToScriptJSON(DataTable table)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(table,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml
                });
        }

        /// <summary>
        /// Query the secondary MySQL database (sam_npa) using the MySQLConnection config section.
        /// Parameters use @name placeholders, same as ExecuteQuery.
        /// </summary>
        public DataTable ExecuteQueryMySQL(string query, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            var mysqlConfig = _config.GetSection("MySQLConnection");
            string connStr = string.Format(
                "Server={0};User ID={1};Password={2};Database={3}",
                mysqlConfig["Server"], mysqlConfig["UserID"], mysqlConfig["Password"], mysqlConfig["Database"]
            );

            var dt = new DataTable();
            try
            {
                using var conn = new MySqlConnector.MySqlConnection(connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
                }
                using var reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            catch (Exception ex)
            {
                string eMessage = System.Text.RegularExpressions.Regex.Replace(ex.Message, @"\t|\n|\r", "");
                _utility.writeLogs("MySQL - " + eMessage + " - " + query);
                throw;
            }
            return dt;
        }

        /// <summary>
        /// แปลงรหัสทรัพย์ NPA (product_code) → ชื่อโครงการ แบบยิงคิวรีครั้งเดียวด้วย IN (...)
        /// ใช้ในหน้า AgentList Edit/Detail ที่มีช่องรหัสทรัพย์หลายสิบช่อง — ถ้า lookup ทีละช่อง
        /// จะยิง MySQL เท่ากับจำนวนช่อง (30 ครั้งต่อการเปิดหน้า 1 ครั้ง)
        /// คืน Dictionary ที่ key = product_code (รหัสที่ไม่พบจะไม่มี key)
        /// </summary>
        public Dictionary<string, string> GetNpaProjectNames(IEnumerable<string> codes)
        {
            var map = new Dictionary<string, string>();
            var list = (codes ?? Enumerable.Empty<string>())
                .Select(c => (c ?? "").Trim())
                .Where(c => c.Length > 0)
                .Distinct()
                .ToList();
            if (list.Count == 0) return map;

            var pars = new Dictionary<string, object>();
            var placeholders = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                placeholders.Add("@c" + i);
                pars["@c" + i] = list[i];
            }

            try
            {
                var dt = ExecuteQueryMySQL(
                    "select product_code, project from tb_product2 where product_code in (" +
                    string.Join(",", placeholders) + ")", pars);
                foreach (DataRow r in dt.Rows)
                {
                    var code = (r["product_code"]?.ToString() ?? "").Trim();
                    var proj = (r["project"]?.ToString() ?? "").Trim();
                    if (code.Length > 0 && proj.Length > 0) map[code] = proj;
                }
            }
            catch { }   // MySQL ล่ม = ไม่แสดงชื่อโครงการ แต่หน้า admin ต้องเปิดได้ตามปกติ
            return map;
        }
        #region Private Methods
        private static DataTable Query(string consulta, IList<SqlParameter> parametros)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(defaultConnectionString);
                SqlCommand command = new SqlCommand();
                SqlDataAdapter da;
                try
                {
                    command.Connection = connection;
                    command.CommandText = consulta;
                    if (parametros != null && parametros.Count() > 0)
                    {
                        command.Parameters.AddRange(parametros.ToArray());
                    }
                    da = new SqlDataAdapter(command);
                    da.Fill(dt);
                    NormalizeDateTimeColumns(dt);
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static int NonQuery(string query, IList<SqlParameter> parametros)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlConnection connection = new SqlConnection(defaultConnectionString);
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteNonQuery();

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
