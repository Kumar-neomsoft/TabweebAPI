using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

namespace TabweebAPI.DBHelper
{
    public interface IDB
    {

        DataSet ExecuteDataSet(string conn, string query, CommandType cmdType, List<DbParameter> parameterlist);

        DataTable ExecuteDataTable(string conn, string query, CommandType cmdType, List<DbParameter> parameterlist);

        int ExecuteNonQuery(string conn, string query, CommandType cmdType, List<DbParameter> parameterlist);

        object ExecuteScalar(string query, CommandType cmdType, List<DbParameter> parameterlist);

        IDataReader ExecuteReader(string query, CommandType cmdType, List<DbParameter> parameterlist);

        IDbConn GetDBConnection();

        IDbCommand GetDBCommand();

        DataSet ExecuteDataSet(IDbConn dbConnection, string query, CommandType cmdType, List<DbParameter> parameterlist);

        DataTable ExecuteDataTable(IDbConn dbConnection, string query, CommandType cmdType, List<DbParameter> parameterlist);

        int ExecuteNonQuery(IDbConn dbConnection, string query, CommandType cmdType, List<DbParameter> parameterlist);

        object ExecuteScalar(IDbConn dbConnection, string query, CommandType cmdType, List<DbParameter> parameterlist);

        IDataReader ExecuteReader(IDbConn dbConnection, string query, CommandType cmdType, List<DbParameter> parameterlist);


    }
}
