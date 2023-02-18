using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tabweeb_Model;
using Tabweeb_Model.Common;
using static Tabweeb_Model.Common.commonclass;
using System.Data;
using System.Data.SqlClient;
using TabweebAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using TabweebAPI.IRepository;
using TabweebAPI.DBHelper;
using System.Text.Json.Serialization;
using NLog;
using Dapper;
namespace TabweebAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IDbConnection _connection;
        //private bool _disposed;
        #endregion

        #region "Constructor"
        public CustomerRepository(IConfiguration config)
        {

            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
            _connection = new SqlConnection(_dbconn);
            _connection.Open();

        }
        #endregion

        public async Task<MethodResult<List<GetCustomerRes>>> GetCustomer(GetCustomerReq obj)
        {
            MethodResult<List<GetCustomerRes>> responseObject = new MethodResult<List<GetCustomerRes>>();
            try
            {
                
                string sqlStr = "sp_CustomerDetails";
                DynamicParameters dbParam = new DynamicParameters();
                dbParam.Add("@Mode", "GetCustomer", DbType.String);
                dbParam.Add("@C_CODE", (string?)obj.C_CODE, DbType.String);
                dbParam.Add("@C_A_NAME", (String?)obj.C_A_NAME, DbType.String);
                dbParam.Add("@C_E_NAME", (String?)obj.C_E_NAME, DbType.String);
                dbParam.Add("@C_A_CODE", (String?)obj.C_A_CODE, DbType.String);
                dbParam.Add("@C_BRN_NO", (Int32?)obj.C_BRN_NO, DbType.Int32);
                dbParam.Add("@RecordFrom", (Int32?)obj.RecordFrom, DbType.Int32);
                dbParam.Add("@RecordTo", (Int32?)obj.RecordTo, DbType.Int32);
                var Result = await _commonRepository.GetList<GetCustomerRes>(sqlStr, dbParam);
                responseObject.ResultObject = Result.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return responseObject;
        }
    }
}
