using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace TabweebAPI.DBHelper
{
    public sealed class DbHelper
    {
        private DbHelper()
        {

        }

        static DbHelper()
        {
           string _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
             if (_dbconn != null)
             {
                string conString = _dbconn;
                string providerString = "System.Data.SqlClient";
                if (!string.IsNullOrEmpty(conString) && !string.IsNullOrEmpty(providerString))
                {
                    SetConnectionString(conString);
                    SetProviderString(providerString);
                }
            }
        }

        internal const string MySQLDatabase = "MySQL";
        internal const string SQLDatabase = "SQL";

        internal const string ODBCProvider = "System.Data.Odbc";
        internal const string SQLProvider = "System.Data.SqlClient";
        internal const string MySQLProvider = "MySql.Data.MySqlClient";
        //Shared db As IDB = GetDbInstance()

        private static IDB db;
        static internal IDB GetDbInstance()
        {
            // get these two from config file or somewhere
            var connectionString = GetConnectionString();
            var providerName = GetProviderString();

            // logic to decide which db is being used
            var driver = GetDbType(providerName);

#if DEBUG1
            BaDbHelperLog.AddDebugFormat("BaDbHelper Provider: {0} ", providerName);
            BaDbHelperLog.AddDebugFormat("BaDbHelper DBType: {0} ", driver);
#endif

            if (providerName == MySQLProvider)
            {
                //return new Db<MySqlClient>(connectionString, providerName);
                return null;
            }
            else if (driver == MySQLDatabase && providerName == ODBCProvider)
            {
                //return new Db<MySqlOdbc>(connectionString, providerName);
                return null;
            }
            else if (driver == SQLDatabase && providerName == SQLProvider)
            {
                return new Db<SqlClient>(connectionString, providerName);
            }
            else
            {
                return null;
            }
        }

        private static void SetDB()
        {
            db = GetDbInstance();
        }

        /// <summary>
        /// This method helps to retrieve database type such as MySQL, SQL or any other.
        /// </summary>
        /// <returns>DbType String</returns>
        static internal string GetDbType(string provideName)
        {
            string dbType = null;

            string conString = GetConnectionString();

            if (provideName == MySQLProvider)
            {
                dbType = MySQLDatabase;
            }
            else if (provideName == SQLProvider)
            {
                dbType = SQLDatabase;
            }
            else if (provideName == ODBCProvider && conString.Contains(MySQLDatabase))
            {
                dbType = MySQLDatabase;
            }
            else if (provideName == ODBCProvider && conString.Contains(SQLDatabase))
            {
                dbType = SQLDatabase;
            }

            return dbType;
        }

        /// <summary>
        /// Helps to get active connection string
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return _connectionString;
        }
        private static string _connectionString;
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            if (!string.IsNullOrEmpty(_connectionString) && !string.IsNullOrEmpty(_providerString))
            {
                SetDB();
            }
        }

        /// <summary>
        /// Helps to get database provider based on configured connection
        /// </summary>
        /// <returns></returns>
        private static string GetProviderString()
        {
            return _providerString;
        }

        private static string _providerString;
        public static void SetProviderString(string provider)
        {
            _providerString = provider;
            if (!string.IsNullOrEmpty(_connectionString) && !string.IsNullOrEmpty(_providerString))
            {
                SetDB();
            }
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.DataSet</returns>
        public static DataSet ExecuteDataSet(string conn, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteDataSet(conn,query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute DataSet using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.DataSet</returns>
        public static DataSet ExecuteDataSet(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteDataSet(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute DataTable
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.DataTable</returns>
        public static DataTable ExecuteDataTable(string connection,string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteDataTable(connection,query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute DataTable using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.DataTable</returns>
        public static DataTable ExecuteDataTable(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteDataTable(baDBConnection, query, cmdType, parameterlist);
        }
        /// <summary>
        /// Execute DataTable using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.DataTable</returns>
        public static DataTable ExecuteDataTable(IDbConn baDBConnection, string query, List<DbParameter> parameterlist = null, CommandType cmdType = CommandType.Text)
        {
            return db.ExecuteDataTable(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute NonQuery
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>integer type value for affected rows</returns>
        public static int ExecuteNonQuery(string conn,string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteNonQuery(conn,query, cmdType, parameterlist);
        }
        /// <summary>
        /// Execute NonQuery
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>integer type value for affected rows</returns>
        public static int ExecuteNonQuery(IDbConn baDBConnection, string query, List<DbParameter> parameterlist = null, CommandType cmdType = CommandType.Text)
        {
            return db.ExecuteNonQuery(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute NonQuery using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>integer type value for affected rows</returns>
        public static int ExecuteNonQuery(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteNonQuery(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Object</returns>
        public static object ExecuteScalar(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteScalar(query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute Scalar using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Object</returns>
        public static object ExecuteScalar(IDbConn baDBConnection, string query, List<DbParameter> parameterlist = null, CommandType cmdtype = CommandType.Text)
        {
            return db.ExecuteScalar(baDBConnection, query, cmdtype, parameterlist);
        }
        /// <summary>
        /// Execute Scalar using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Object</returns>
        public static object ExecuteScalar(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteScalar(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.IDataReader</returns>
        [Obsolete("This method should be used only with active / shared connection.")]
        private static IDataReader ExecuteReader(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteReader(query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute Reader using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.IDataReader</returns>
        public static IDataReader ExecuteReader(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            return db.ExecuteReader(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Execute Reader using shared connection
        /// </summary>
        /// <param name="baDBConnection">IBaDbConnection object</param>
        /// <param name="query">SQL query as a string</param>
        /// <param name="cmdType">System.Data.CommandType parameter</param>
        /// <param name="parameterlist">Collection of BaDbparameter</param>
        /// <returns>System.Data.IDataReader</returns>
        public static IDataReader ExecuteReader(IDbConn baDBConnection, string query, List<DbParameter> parameterlist = null, CommandType cmdType = CommandType.Text)
        {
            return db.ExecuteReader(baDBConnection, query, cmdType, parameterlist);
        }

        /// <summary>
        /// Returns IBaDbConnection object based on active connection string.
        /// </summary>
        /// <returns>Returns IDbConnection object</returns>
        public static IDbConn GetDBConnection()
        {
            return db.GetDBConnection();
        }

        /// <summary>
        /// Returns IDbCommand object based on IDbConnection
        /// </summary>
        /// <returns>Returns IDbCommand object</returns>
        public static IDbCommand GetDBCommand()
        {
            return db.GetDBCommand();
        }

        /// <summary>
        /// It will parse the comma seperated values in individual parameters
        /// It appends individual parameters into the passed parameter list
        /// Finally, it returns parameter value string that need to replace with actual parameter by formating sql command text.
        /// It is not applicable to stored procedure
        /// </summary>
        /// <param name="parametersValue">Existing parameter value string which contains comma or pipe </param>
        /// <param name="parameterlist">BaDbParameter list which is used in command</param>
        /// <returns>Updates BaDbParameter list collection based on parameter values found in passed parameter string and returns string with parameterValues</returns>
        /// <remarks></remarks>
        public static string ParseParametersForInClause(string parametersValue, List<DbParameter> parameterlist)
        {

            string commaValues = string.Empty;
            int I = 1;

            if ((parametersValue != null))
            {
                string[] strArray = parametersValue.Split(',');

                foreach (string objStr in strArray)
                {
                    //IN clause
                    string parameterName = String.Format("{0}{1}{2}", "@", I.ToString(), Guid.NewGuid().ToString().Substring(0, 8));

                    commaValues = commaValues + (parameterName + ",");
                    parameterlist.Add(new DbParameter(parameterName, objStr, DbType.String));
                    I = I + 1;
                }

                if (commaValues.Length > 1 && commaValues.Contains(","))
                {
                    commaValues = commaValues.Remove(commaValues.Length - 1, 1);
                }
            }

            return commaValues;
        }

    }

    public static class Extension
    {
        /// <summary>
        /// Initialize passed parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterlist"></param>
        public static void Parameterize(this IDbCommand command, List<DbParameter> parameterlist)
        {
            foreach (DbParameter obj in parameterlist)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = obj.Name;
                parameter.Value = obj.Value;

                if (obj.DBType.HasValue)
                {
                    parameter.DbType = obj.DBType.Value;
                }

                parameter.Direction = obj.DBDirection;

                parameter.Size = obj.Size;

                command.Parameters.Add(parameter);
            }
        }

    }
}
