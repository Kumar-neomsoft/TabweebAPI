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

namespace TabweebAPI.Common
{
    public class CommonRepository
    {

        public static string connStr;
        public static string systemconnStr;
        private string providerName= "System.Data.SqlClient";

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
      

      

    }
}
