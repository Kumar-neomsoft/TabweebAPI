using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Tabweeb_Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
//using System.Data.Common;
using System.Reflection;
using TabweebAPI.DBHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Net.Mail;

namespace TabweebAPI.Common
{
    public class CommonRepository
    {
        #region "Declarations"
        public static string connStr;
        public static string systemconnStr;
        private string providerName= "System.Data.SqlClient";
        #endregion
        public CommonRepository()
        {
            connStr = GetConnectionString();
            systemconnStr = GetSystemConnectionString();

        }
        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
           
        }
        public static string GetSystemConnectionString()
        {
            return Startup.SysConnectionString;
            
        }
       
        public async Task<DataTable> ExecuteDataTable(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            try
            {

                DataTable dt = new DataTable();
                dt =  DbHelper.ExecuteDataTable(connStr, query, CommandType.StoredProcedure, parameterlist);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
               
            }
        }
        public async Task<DataSet> ExecuteDataSet(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            try
            {

                DataSet ds = new DataSet();
                ds = DbHelper.ExecuteDataSet(connStr, query, CommandType.StoredProcedure, parameterlist);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<object> ExecuteScalar(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            try
            {
                return DbHelper.ExecuteScalar(connStr, query, CommandType.StoredProcedure, parameterlist);
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public async Task<T> GetValue<T>(object Result)
        {
            try
            {
                    T output =  (T)Enum.ToObject(typeof(T),Result);
                    return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<T>> GetList<T>(string query, DynamicParameters parms)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(connStr);
                var results = await connection.QueryAsync<T>(query, parms, commandType: CommandType.StoredProcedure);
                return results.ToList();
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public async Task<DataTable> ExecuteDataTable(string conn, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = DbHelper.ExecuteDataTable(conn, query, CommandType.StoredProcedure, parameterlist);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<string> InsertUpdateErrorLog<T>(Exception exception, string location)
        {

            var ErrorValue = string.Empty;
            ErrorLog errorlog = new ErrorLog();
            errorlog.UserName = "Data Access Layer";
            errorlog.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            errorlog.ErrorLocation = "Referer:" + location + "|Stack Trace:" + exception.ToString();
            errorlog.ErrorType = exception.Message;
            errorlog.ErrorDescription = exception.StackTrace;
            var results =  InsertUpdateErrorLog(errorlog);

            SqlException SqlExcep = default(SqlException);
            SqlExcep = (SqlException)exception;
            if ((SqlExcep != null))
            {
                List<sqlError> SqlErrorList = new List<sqlError>()
                {
                    new sqlError() { ErrorNumber = "42883", ErrorValue = "Undefined Function" } ,//42883- No Procedure Matches
                    //new sqlError() { ErrorNumber = 2601, ErrorValue = "DUPLICATE" } ,
                    new sqlError() { ErrorNumber = "23503", ErrorValue = "Foreign Violation" },//23503-Foreign Violation
                    new sqlError() { ErrorNumber = "23505", ErrorValue = "Unique Violation" }  //23505-unique_violation
                };
                ErrorValue = SqlErrorList.Where(a => a.ErrorNumber == SqlExcep.SqlState).Select(b => b.ErrorValue).FirstOrDefault();
                return ErrorValue;
            }

            return ErrorValue;
        }
        public  string InsertUpdateErrorLog(ErrorLog errorlog)
        {
            try
            {
                string sqlstr = "sp_ErrorLog";
                List<DbParameter> dbParam = new List<DbParameter>();
                if((String)errorlog.Id.ToString() == null || (String)errorlog.Id.ToString() == "" || (String)errorlog.Id.ToString() == "0")
                    dbParam.Add(new DbParameter("Mode", "Insert", DbType.String));
                else
                    dbParam.Add(new DbParameter("Mode", "Update", DbType.String));
                dbParam.Add(new DbParameter("Id", (Int32?) errorlog.Id, DbType.Int32));
                dbParam.Add(new DbParameter("UserName", (String)errorlog.UserName, DbType.String, 50));
                dbParam.Add(new DbParameter("DateTime", (String)errorlog.DateTime, DbType.DateTime));
                dbParam.Add(new DbParameter("ErrorLocation", (String)errorlog.ErrorLocation, DbType.String, 100));
                dbParam.Add(new DbParameter("ErrorType", (String)errorlog.ErrorType, DbType.String));
                dbParam.Add(new DbParameter("ErrorDescription", (String)errorlog.ErrorDescription, DbType.String));
                DbHelper.ExecuteNonQuery(connStr, sqlstr, CommandType.StoredProcedure, dbParam);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public  string ToUrlSafeBase64String(string Base64String)
        {
            // avoid any slashes, plus signs or equal signs
            // the following makes this base64 string url safe
            //Base64String = Base64String.Replace("/", "_");
            Base64String = Base64String.Replace("+", "-");
            return Base64String;
        }

        public  string FromUrlSafeBase64String(string Base64String)
        {
            // add back any slashes, plus signs or equal signs
            // the following makes this url safe string a base64 string
            //Base64String = Base64String.Replace("_", "/");
            Base64String = Base64String.Replace("-", "+");
            return Base64String;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKMNIV2SPBNIMNI992MNI12MNI";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKMNIV2SPBNIMNI992MNI12MNI";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public  bool SendMailContent(string Subject, string MailBody, string[] ToMail)
        {
            bool rtn = false;
            try
            {
                string FromMail = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["FromMail"];
                string Password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["Password"];
                string Smtpclient = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["smtp"];
                string Url = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["ProjectURl"];
                string Port = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["MailPort"];


                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(FromMail);
                foreach (string tomail in ToMail)
                {
                    Mail.To.Add(tomail);
                }

                Mail.Subject = Subject;
                Mail.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient(Smtpclient);
                SmtpServer.Port = Convert.ToInt32(Port);
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(FromMail, Password);
                string SSL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["SSL"];
                if (SSL.ToLower() == "true")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }
                Mail.Body = MailBody;
                SmtpServer.Send(Mail); 
             
                rtn = true;
            }
            catch (Exception)
            {
                rtn = false;
            }
            return rtn;
        }

    }
}
