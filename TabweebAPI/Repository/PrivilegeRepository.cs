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
    public class PrivilegeRepository : IPrivilegeRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public PrivilegeRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBSysConnection"];
        }
        #endregion

        public async Task<MethodResult<List<PrivilegeRes>>> GetPrivilege(PrivilegeReq objPro)
        {
            MethodResult<List<PrivilegeRes>> ObjRes = new MethodResult<List<PrivilegeRes>>();
            List<PrivilegeRes> SPresponseObject = new List<PrivilegeRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_PrivilegeDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("U_ID", objPro.U_ID, DbType.Int32));
                dbParam.Add(new DbParameter("FORM_NO", objPro.FORM_NO, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<PrivilegeRes>>(Result);
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
