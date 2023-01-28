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
using Tabweeb_Model.Sales;

namespace TabweebAPI.Repository
{
    public class SalesRepository :ISalesRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public SalesRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
        }
        #endregion
        public async Task<MethodResult<List<Currency>>> GetCurrencyDetails()
        {
            MethodResult<List<Currency>> responseObject = new MethodResult<List<Currency>>();
            List<Currency> CresponseObject = new List<Currency>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetCurrency", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                CresponseObject = JsonConvert.DeserializeObject<List<Currency>>(Result);
                responseObject.ResultObject = CresponseObject;
                return responseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return responseObject;
        }

        public async Task<MethodResult<List<WareHouse>>> GetWareHouseDetails()
        {
            MethodResult<List<WareHouse>> responseObject = new MethodResult<List<WareHouse>>();
            List<WareHouse> WresponseObject = new List<WareHouse>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetWareHouse", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                WresponseObject = JsonConvert.DeserializeObject<List<WareHouse>>(Result);
                responseObject.ResultObject = WresponseObject;
                return responseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return responseObject;
        }

        public async Task<MethodResult<List<BillType>>> GetBillType(int LangId, int DocType)
        {
            MethodResult<List<BillType>> responseObject = new MethodResult<List<BillType>>();
            List<BillType> WresponseObject = new List<BillType>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetBillType", DbType.String));
                dbParam.Add(new DbParameter("LangId", LangId, DbType.Int32));
                dbParam.Add(new DbParameter("DocType", DocType, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                WresponseObject = JsonConvert.DeserializeObject<List<BillType>>(Result);
                responseObject.ResultObject = WresponseObject;
                return responseObject;
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
