using System.Data;
using System.Data.Common;

namespace TabweebAPI.DBHelper
{
    public interface IDbConn : IDbConnection
    {

        string QueryText { get; set; }

        IDbTransaction DBTransaction { get; }
    }

}
