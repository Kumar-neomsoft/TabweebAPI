using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace TabweebAPI.DBHelper
{
    public class Db<T> : IDB where T : IDbConn, new()
    {

        #region "Private Variable"
        private string connectionString;
        #endregion
        private string providerName;

        #region "Constructor"

        public Db(string _connectionString, string _providerName)
        {
            this.connectionString = _connectionString;
            this.providerName = _providerName;
        }

        #endregion

        #region "Executing Data Reader"

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public IDataReader ExecuteReader(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            using (var conn = new T())
            {
                //Assiging Query to native implementor to change as per database requirement.
                conn.QueryText = query;
                var cmd = conn.CreateCommand();

                if (parameterlist != null && parameterlist.Count > 0)
                {
                    cmd.Parameterize(parameterlist);
                }

                //Assigning Query Text According to Database Type
                cmd.CommandText = conn.QueryText;

                cmd.CommandType = cmdType;

                //To handle stored procedure 
                //ProviderCustomization(cmd);

                cmd.Connection.ConnectionString = connectionString;
                cmd.Connection.Open();

                return cmd.ExecuteReader();
            }
        }

        private static IEnumerable<IDataRecord> ExecuteReader(IDbCommand cmd)
        {
            List<IDataReader> lstIDataRecord = new List<IDataReader>();
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    lstIDataRecord.Add(r);
                }
            }
            return lstIDataRecord;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public IDataReader ExecuteReader(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            //Assiging Query to native implementor to change as per database requirement.
            baDBConnection.QueryText = query;

            var cmd = baDBConnection.CreateCommand();

            if (parameterlist != null && parameterlist.Count > 0)
            {
                cmd.Parameterize(parameterlist);
            }

            //Assigning Query Text According to Database Type
            cmd.CommandText = baDBConnection.QueryText;

            cmd.CommandType = cmdType;

            //To handle stored procedure 
            //ProviderCustomization(cmd);

            if ((baDBConnection.DBTransaction != null))
            {
                cmd.Transaction = baDBConnection.DBTransaction;
            }

            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }

            return cmd.ExecuteReader();
        }

        #endregion

        #region "Executing Scalar"

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar(string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            object result = null;

            using (var conn = new T())
            {
                using (var cmd = conn.CreateCommand())
                {
                    if (parameterlist != null && parameterlist.Count > 0)
                    {
                        cmd.Parameterize(parameterlist);
                    }

                    //Assiging Query to native implementor to change as per database requirement.
                    conn.QueryText = query;

                    //Assigning Query Text According to Database Type
                    cmd.CommandText = conn.QueryText;

                    cmd.CommandType = cmdType;

                    //To handle stored procedure 
                    //ProviderCustomization(cmd);

                    cmd.Connection.ConnectionString = connectionString;
                    cmd.Connection.Open();
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            object result = null;

            using (var cmd = baDBConnection.CreateCommand())
            {
                if (parameterlist != null && parameterlist.Count > 0)
                {
                    cmd.Parameterize(parameterlist);
                }

                //Assiging Query to native implementor to change as per database requirement.
                baDBConnection.QueryText = query;

                //Assigning Query Text According to Database Type
                cmd.CommandText = baDBConnection.QueryText;

                cmd.CommandType = cmdType;

                //To handle stored procedure 
                //ProviderCustomization(cmd);

                if ((baDBConnection.DBTransaction != null))
                {
                    cmd.Transaction = baDBConnection.DBTransaction;
                }

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                result = cmd.ExecuteScalar();
            }

            return result;
        }

        #endregion

        #region "Executing NonQuery"

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQuery(string connection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            int result = 0;
            using (var conn = new T())
            {

                //Assiging Query to native implementor to change as per database requirement.
                conn.QueryText = query;
                conn.ConnectionString = connection;
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {

                    if (parameterlist != null && parameterlist.Count > 0)
                    {
                        cmd.Parameterize(parameterlist);
                    }

                    //Assigning Query Text According to Database Type
                    cmd.CommandText = conn.QueryText;

                    cmd.CommandType = cmdType;

                    //To handle stored procedure 
                   // ProviderCustomization(cmd);

                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQuery(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            int result = 0;

            //Assiging Query to native implementor to change as per database requirement.
            baDBConnection.QueryText = query;

            using (var cmd = baDBConnection.CreateCommand())
            {

                if (parameterlist != null && parameterlist.Count > 0)
                {
                    cmd.Parameterize(parameterlist);
                }

                //Assigning Query Text According to Database Type
                cmd.CommandText = baDBConnection.QueryText;

                cmd.CommandType = cmdType;

                //To handle stored procedure 
               // ProviderCustomization(cmd);

                if ((baDBConnection.DBTransaction != null))
                {
                    cmd.Transaction = baDBConnection.DBTransaction;
                }

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        #endregion

        #region "Executing DataSet"

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet ExecuteDataSet(string connection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            DataSet ds = new DataSet();

            using (var conn = new T())
            {
                //Assiging Query to native implementor to change as per database requirement.
                conn.QueryText = query;

                using (var cmd = conn.CreateCommand())
                {

                    cmd.Connection.ConnectionString = connection;

                    if (parameterlist != null && parameterlist.Count > 0)
                    {
                        cmd.Parameterize(parameterlist);
                    }

                    //Assigning Query Text According to Database Type
                    cmd.CommandText = conn.QueryText;

                    cmd.CommandType = cmdType;

                    //To handle stored procedure 
                   // ProviderCustomization(cmd);

                    cmd.Connection.Open();

                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    DbDataAdapter da = factory.CreateDataAdapter();
                    da.SelectCommand = (DbCommand)cmd;

                    da.Fill(ds);
                }
            }
            return ds;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable ExecuteDataTable(string connection,string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            DataTable dt = new DataTable();

            using (var conn = new T())
            {
                //Assiging Query to native implementor to change as per database requirement.
                conn.QueryText = query;

                using (var cmd = conn.CreateCommand())
                {

                    cmd.Connection.ConnectionString = connection;// connectionString;

                    if (parameterlist != null && parameterlist.Count > 0)
                    {
                        cmd.Parameterize(parameterlist);
                    }

                    //Assigning Query Text According to Database Type
                    cmd.CommandText = conn.QueryText;

                    cmd.CommandType = cmdType;

                    //To handle stored procedure 
                   //ProviderCustomization(cmd);

                    cmd.Connection.Open();

                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    DbDataAdapter da = factory.CreateDataAdapter();
                    da.SelectCommand = (DbCommand)cmd;

                    da.Fill(dt);

                }
            }
            return dt;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet ExecuteDataSet(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {
            DataSet ds = new DataSet();

            //Assiging Query to native implementor to change as per database requirement.
            baDBConnection.QueryText = query;

            if (baDBConnection.State == ConnectionState.Closed)
            {
                baDBConnection.Open();
            }

            using (var cmd = baDBConnection.CreateCommand())
            {
                if (parameterlist != null && parameterlist.Count > 0)
                {
                    cmd.Parameterize(parameterlist);
                }

                //Assigning Query Text According to Database Type
                cmd.CommandText = baDBConnection.QueryText;

                cmd.CommandType = cmdType;

                //To handle stored procedure 
               // ProviderCustomization(cmd);

                if ((baDBConnection.DBTransaction != null))
                {
                    cmd.Transaction = baDBConnection.DBTransaction;
                }

                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                DbDataAdapter da = factory.CreateDataAdapter();
                da.SelectCommand = (DbCommand)cmd;

                da.Fill(ds);

            }

            return ds;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable ExecuteDataTable(IDbConn baDBConnection, string query, CommandType cmdType, List<DbParameter> parameterlist = null)
        {

            DataTable dt = new DataTable();

            //Assiging Query to native implementor to change as per database requirement.
            baDBConnection.QueryText = query;

            if (baDBConnection.State == ConnectionState.Closed)
            {
                baDBConnection.Open();
            }

            using (var cmd = baDBConnection.CreateCommand())
            {
                if (parameterlist != null && parameterlist.Count > 0)
                {
                    cmd.Parameterize(parameterlist);
                }

                //Assigning Query Text According to Database Type
                cmd.CommandText = baDBConnection.QueryText;

                cmd.CommandType = cmdType;

                //To handle stored procedure 
                //ProviderCustomization(cmd);

                if ((baDBConnection.DBTransaction != null))
                {
                    cmd.Transaction = baDBConnection.DBTransaction;
                }

                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                DbDataAdapter da = factory.CreateDataAdapter();
                da.SelectCommand = (DbCommand)cmd;

                da.Fill(dt);

            }

            return dt;
        }

        #endregion

        #region "Native Interface"


        private IDbConn _baDBConnection = null;

        public IDbConn GetDBConnection()
        {
            if (_baDBConnection == null)
            {
                _baDBConnection = new T();
            }

            if (_baDBConnection.State == ConnectionState.Closed)
            {
                if (string.IsNullOrEmpty(_baDBConnection.ConnectionString))
                {
                    _baDBConnection.ConnectionString = connectionString;
                }

                _baDBConnection.Open();
            }

            return _baDBConnection;

        }

        public IDbCommand GetDBCommand()
        {
            if (_baDBConnection == null)
            {
                return GetDBConnection().CreateCommand();
            }
            else
            {
                return _baDBConnection.CreateCommand();
            }
        }

        #endregion

        #region "Provider Customization"

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]

        private void ProviderCustomization(IDbCommand cmd)
        {
            //To handle stored procedure 
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                var dbType = DbHelper.GetDbType(providerName);

                if (providerName == DbHelper.MySQLProvider)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("{0} {1}", "call ", cmd.CommandText);
                }
                else if (providerName == DbHelper.ODBCProvider && dbType == DbHelper.MySQLDatabase)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("{0} {1}", "call ", cmd.CommandText);
                }
                else if (providerName == DbHelper.SQLProvider && dbType == DbHelper.SQLDatabase)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("{0} {1}", "exec ", cmd.CommandText);
                }

            }

#if DEBUG1
            StringBuilder strCommandParameter = new StringBuilder();
            foreach (IDbDataParameter obj in cmd.Parameters)
            {
                if ((obj != null))
                {
                    strCommandParameter.AppendLine(string.Format("{0} : {1} : {2} : {3}", obj.ParameterName, obj.Value, obj.Size, obj.DbType.ToString()));
                }
            }

            BaDbHelperLog.AddDebugFormat("BaDbHelper Execution: {0} Query Command Type : {1} {0} SQL Query: {2} {0} SQL Parameters: {3} {0}", Environment.NewLine, cmd.CommandType, cmd.CommandText, strCommandParameter);
#endif

        }

        #endregion


    }
}
