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
namespace TabweebAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public ProductRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
        }
        #endregion

        public async Task<MethodResult<List<ProductSearch>>> SearchProduct(ProductSearchReq objPro)
        {
            MethodResult<List<ProductSearch>> ObjRes = new MethodResult<List<ProductSearch>>();
            List<ProductSearch> SPresponseObject = new List<ProductSearch>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_ProductDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetProduct", DbType.String));
                dbParam.Add(new DbParameter("I_CODE", objPro.I_CODE, DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<ProductSearch>>(Result);
                ObjRes.ResultObject = SPresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }

        public async Task<MethodResult<List<ProductGetRes>>> GetProductCode(ProductGetReq objPro)
        {
            MethodResult<List<ProductGetRes>> ObjRes = new MethodResult<List<ProductGetRes>>();
            List<ProductGetRes> GPresponseObject = new List<ProductGetRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_ProductDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetProductCode", DbType.String));
                dbParam.Add(new DbParameter("I_CODE", objPro.I_CODE, DbType.String));
                dbParam.Add(new DbParameter("RecordFrom", objPro.RecordFrom, DbType.Int32));
                dbParam.Add(new DbParameter("RecordTo", objPro.RecordTo, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                GPresponseObject = JsonConvert.DeserializeObject<List<ProductGetRes>>(Result);
                ObjRes.ResultObject = GPresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }

        public async Task<MethodResult<List<BarcodeGetRes>>> GetBarCode(BarcodeGetReq objPro)
        {
            MethodResult<List<BarcodeGetRes>> ObjRes = new MethodResult<List<BarcodeGetRes>>();
            List<BarcodeGetRes> GPresponseObject = new List<BarcodeGetRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_ProductDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetProBarCode", DbType.String));
                dbParam.Add(new DbParameter("I_CODE", (String)objPro.I_CODE, DbType.String));
                dbParam.Add(new DbParameter("ITM_UNT", (String?)objPro.ITM_UNT, DbType.String));
                dbParam.Add(new DbParameter("RecordFrom", (Int32?)objPro.RecordFrom, DbType.Int32));
                dbParam.Add(new DbParameter("RecordTo", (Int32?)objPro.RecordTo, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                GPresponseObject = JsonConvert.DeserializeObject<List<BarcodeGetRes>>(Result);
                ObjRes.ResultObject = GPresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<ProductSearch>>> GetAllProduct()
        {
            MethodResult<List<ProductSearch>> ObjRes = new MethodResult<List<ProductSearch>>();
            List<ProductSearch> SPresponseObject = new List<ProductSearch>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_ProductDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetProduct", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<ProductSearch>>(Result);
                ObjRes.ResultObject = SPresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
    }
}
