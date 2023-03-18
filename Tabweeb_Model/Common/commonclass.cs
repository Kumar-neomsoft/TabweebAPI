using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace Tabweeb_Model.Common
{
    public class commonclass
    {
        public enum saveStatus : int
        {
            success = 0,
            Failure = -1,
        }

        public enum CRUDAction
        {
            Add,
            Insert,
            Update,
            Delete,
            Select,
            Edit,
            View,
            CommonInsert,
            CommonUpdate,
            CommonDelete,
            CheckIn,
            CheckOut,
            Approved,
            Rejected,
            LoggedOut,
            Notification
        }
        public static string Decrypt(string jsonData)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            var base64EncodedBytes = System.Convert.FromBase64String(jsonData);
            string DecryptedString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return DecryptedString;
        }
        public static string DecryptCommon(string jsonData)
        {
            string jsonBytes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(jsonData));
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            var base64EncodedBytes = System.Convert.FromBase64String(jsonBytes);
            string DecryptedString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return DecryptedString;
        }
        public class ListtoDataTable
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties by using reflection   
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names  
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {

                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
        }
        public class CommonStatus
        {
            [JsonIgnore]
            public bool IsActive { get; set; }
            public string status
            {
                get
                {
                    return IsActive == true ? "Active" : "InActive";
                }
            }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? Total { get; set; }
        }
        public class CommonKeys
        {
            public Int32 PageNumber { get; set; }
            public Int32 Size { get; set; }
            public string DbColumn { get; set; }
            public string Sort { get; set; }
        }
       
    }
}
