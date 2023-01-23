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
namespace TabweebAPI.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        public MasterRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBSysConnection"];
        }
        public async Task<MethodResult<List<Language>>> GetLangList()
        {
            MethodResult<List<Language>> responseObject = new MethodResult<List<Language>>();
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
                responseObject.ResultObject = lanresponseObject;
            }
            catch (Exception ex)
            {
            }
            return responseObject;
        }
        public async Task<MethodResult<List<Company>>> GetCompany()
        {
            MethodResult<List<Company>> responseObject = new MethodResult<List<Company>>();
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
                responseObject.ResultObject = accresponseObject;
            }
            catch (Exception ex)
            {
            }
            return responseObject;
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
            }
            return responseObject;
        }
        public async Task<MethodResult<List<BranchRes>>> GetBranch()
        {
            MethodResult<List<BranchRes>> responseObject = new MethodResult<List<BranchRes>>();
            List<BranchRes> accresponseObject = new List<BranchRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "SelectBranch", DbType.String));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                accresponseObject = JsonConvert.DeserializeObject<List<BranchRes>>(Result);
                responseObject.ResultObject = accresponseObject;
            }
            catch (Exception ex)
            {
            }
            return responseObject;
        }
        public async Task<MethodResult<List<AccountingYear>>> GetAccountingYear()
        {
            MethodResult<List<AccountingYear>> responseObject = new MethodResult<List<AccountingYear>>();
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
                responseObject.ResultObject = accresponseObject;
            }
            catch (Exception ex)
            {
            }
            return responseObject;
        }
    }
}
