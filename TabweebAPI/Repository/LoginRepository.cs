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
    public class LoginRepository : ILoginRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public LoginRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
        }
        #endregion
        public async Task<MethodResult<List<LoginResponse>>> AuthenticateUser(LoginRequest LoginReq)
        {
            MethodResult<List<LoginResponse>> responseObject = new MethodResult<List<LoginResponse>>();
            List<LoginResponse> LresponseObject = new List<LoginResponse>();
            try
            {
                
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetUsers";
                var Result = "";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("UserName", (String)LoginReq.UserName, DbType.String, 100));
                dbParam.Add(new DbParameter("Password", (String)LoginReq.Password, DbType.String, 100));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                LresponseObject = JsonConvert.DeserializeObject<List<LoginResponse>>(Result);
                responseObject.ResultObject = LresponseObject;

            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return responseObject;
        }
        public  List<LoginResponse> ValidateUser(LoginRequest LoginReq)
        {
           
            List<LoginResponse> LresponseObject = new List<LoginResponse>();
            try
            {

                DataTable dt = new DataTable();
                string sqlStr = "sp_GetUsers";
               
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("UserName", (String)LoginReq.UserName, DbType.String, 100));
                dbParam.Add(new DbParameter("Password", (String)LoginReq.Password, DbType.String, 100));
                dt =  DbHelper.ExecuteDataTable(_dbconn,sqlStr, CommandType.StoredProcedure, dbParam);
                var Result  = JsonConvert.SerializeObject(dt, Formatting.Indented);
                LresponseObject = JsonConvert.DeserializeObject<List<LoginResponse>>(Result);
                return LresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                 _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return LresponseObject;
        }

        public List<string> ValidateUser1(LoginRequest LoginReq)
        {

            List<string> LresponseObject = new List<string>();
            try
            {

                DataTable dt = new DataTable();
                string sqlStr = "sp_GetUsers";

                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("UserName", (String)LoginReq.UserName, DbType.String, 100));
                dbParam.Add(new DbParameter("Password", (String)LoginReq.Password, DbType.String, 100));
                dt = DbHelper.ExecuteDataTable(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
                var Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                LresponseObject = JsonConvert.DeserializeObject<List<string>>(Result);
                return LresponseObject;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return LresponseObject;
        }
        public long UpdateLoginSession(int UserId,string Token)
        {
            long result = 0;
            try
            {
                string sqlStr = "sp_UpdateLoginToken";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Update", DbType.String));
                dbParam.Add(new DbParameter("UserID", UserId, DbType.Int32));
                dbParam.Add(new DbParameter("Token", Token, DbType.String, 5000));
                result = DbHelper.ExecuteNonQuery(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
            }
            catch (Exception ex)
            {
                result = -2;
            }
            return result;
        }
        public long RefreshLoginSession(int UserId)
        {
            long result = 0;
            try
            {
                string sqlStr = "sp_UpdateLoginToken";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Refresh", DbType.String));
                dbParam.Add(new DbParameter("UserID", UserId, DbType.Int32));
                result = DbHelper.ExecuteNonQuery(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
            }
            catch (Exception ex)
            {
                result = -2;
            }
            return result;
        }
        public SessionResponse SessionResponse(int vUserID)
        {
            SessionResponse sessionResponse = new SessionResponse();
            try
            {
                
                DataSet ds = new DataSet();
                string sqlStr = "sp_UpdateLoginToken";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("UserID", vUserID, DbType.Int32));
                ds = DbHelper.ExecuteDataSet(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    sessionResponse.UserId = Convert.ToInt32(dt.Rows[0]["UserID"]);
                    sessionResponse.Token = dt.Rows[0]["token"].ToString();
                    sessionResponse.SessionTimeOut = dt.Rows[0]["TOKEN_EXP_DATE"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sessionResponse;
        }
    }
}
