using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace TabweebAPI.DBHelper
{
    public sealed class SqlClient : IDbConn, IDisposable
    {
        // Internal members
        private SqlConnection _conn = null;
        private SqlTransaction _trans = null;

        #region "Constructor"

        public SqlClient()
        {
            Connect();
        }

        public SqlClient(string connectionString)
        {
            this.ConnectionString = connectionString;
            Connect();
        }

        #endregion

        #region "Private Method"

        // Creates a OdbcConnection using the current connection string
        private void Connect()
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                this._conn = new SqlConnection();
            }
            else
            {
                this._conn = new SqlConnection(this.ConnectionString);
            }
        }

        #endregion

        #region "IDbConnection Implementation"

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            _trans = this._conn.BeginTransaction(il);
            return _trans;
        }

        public IDbTransaction BeginTransaction()
        {
            _trans = this._conn.BeginTransaction();
            return _trans;
        }

        public void ChangeDatabase(string databaseName)
        {
            this._conn.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            this._conn.Close();
        }

        public string ConnectionString
        {
            get
            {
                if (this._conn == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this._conn.ConnectionString;
                }
            }
            set { this._conn.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return this._conn.ConnectionTimeout; }
        }

        public IDbCommand CreateCommand()
        {
            return _conn.CreateCommand();
        }

        public string Database
        {
            get { return this._conn.Database; }
        }

        public void Open()
        {
            this._conn.Open();
        }

        public ConnectionState State
        {
            get { return this._conn.State; }
        }

        public void Dispose()
        {
            this._conn.Dispose();
        }

        #endregion

        #region "IBaDbConnection Custom Property Implementation"

        public IDbTransaction DBTransaction
        {
            get { return this._trans; }
        }

        private string _queryText;
        public string QueryText
        {
            get { return _queryText; }
            set { _queryText = value; }
        }

        #endregion

    }
}
