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
    public class MasterRepository : IMasterRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public MasterRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBSysConnection"];
        }
        #endregion
        public async Task<MethodResult<List<Language>>> GetLangList()
        {
            MethodResult<List<Language>> ObjRes = new MethodResult<List<Language>>();
            List<Language> lanresponseObject = new List<Language>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMasters";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Lang", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                lanresponseObject = JsonConvert.DeserializeObject<List<Language>>(Result);
                ObjRes.ResultObject = lanresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<Company>>> GetCompany()
        {
            MethodResult<List<Company>> ObjRes = new MethodResult<List<Company>>();
            List<Company> accresponseObject = new List<Company>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Company", DbType.String));
                dt = await _commonRepository.ExecuteDataTable( sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                accresponseObject = JsonConvert.DeserializeObject<List<Company>>(Result);
                ObjRes.ResultObject = accresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<BranchRes>>> GetBranchById(int CompanyId)
        {
            MethodResult<List<BranchRes>> responseObject = new MethodResult<List<BranchRes>>();
            List<BranchRes> accresponseObject = new List<BranchRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "SelectBranchId", DbType.String));
                dbParam.Add(new DbParameter("CompanyId", CompanyId, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                accresponseObject = JsonConvert.DeserializeObject<List<BranchRes>>(Result);
                responseObject.ResultObject = accresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return responseObject;
        }
        public async Task<MethodResult<List<BranchRes>>> GetBranch()
        {
            MethodResult<List<BranchRes>> responseObject = new MethodResult<List<BranchRes>>();
            List<BranchRes> BrresponseObject = new List<BranchRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "SelectBranch", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                BrresponseObject = JsonConvert.DeserializeObject<List<BranchRes>>(Result);
                responseObject.ResultObject = BrresponseObject;
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
        public async Task<MethodResult<List<AccountingYear>>> GetAccountingYear()
        {
            MethodResult<List<AccountingYear>> ObjRes = new MethodResult<List<AccountingYear>>();
            List<AccountingYear> accresponseObject = new List<AccountingYear>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMasters";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "AccountingYear", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                accresponseObject = JsonConvert.DeserializeObject<List<AccountingYear>>(Result);
                ObjRes.ResultObject = accresponseObject;
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
