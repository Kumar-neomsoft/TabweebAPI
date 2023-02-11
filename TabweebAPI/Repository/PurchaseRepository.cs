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
    public class PurchaseRepository : IPurchaseRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public PurchaseRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBSysConnection"];
        }
        #endregion
        public async Task<MethodResult<List<VendorRes>>> GetVendor(VendorReq objPro)
        {
            MethodResult<List<VendorRes>> ObjRes = new MethodResult<List<VendorRes>>();
            List<VendorRes> SPresponseObject = new List<VendorRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_VendorDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetVendor", DbType.String));
                dbParam.Add(new DbParameter("V_CODE", objPro.V_CODE, DbType.String));
                dbParam.Add(new DbParameter("V_A_NAME", objPro.V_A_NAME, DbType.String));
                dbParam.Add(new DbParameter("V_A_CODE", objPro.V_A_CODE, DbType.String));
                dbParam.Add(new DbParameter("RecordFrom", objPro.RecordFrom, DbType.Int32));
                dbParam.Add(new DbParameter("RecordTo", objPro.RecordTo, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<VendorRes>>(Result);
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

        public async Task<MethodResult<List<CashBankRes>>> GetCashBankNo(CashBankReq objPro)
        {
            MethodResult<List<CashBankRes>> ObjRes = new MethodResult<List<CashBankRes>>();
            List<CashBankRes> SPresponseObject = new List<CashBankRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_VendorDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "CashBankNo", DbType.String));
                dbParam.Add(new DbParameter("CASH_TYPE_SAVE", objPro.CASH_TYPE_SAVE, DbType.Int32));
                dbParam.Add(new DbParameter("RecordFrom", objPro.RecordFrom, DbType.Int32));
                dbParam.Add(new DbParameter("RecordTo", objPro.RecordTo, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<CashBankRes>>(Result);
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

        public async Task<MethodResult<saveStatus>> InsertPurchaseOrder(string PODetails, string POItemData)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            JObject obj = JObject.Parse(PODetails);
            string sqlStr = "sp_PurchaseOrderDetails";
            List<DbParameter> dbParam = new List<DbParameter>();
            try
            {
                dbParam.Add(new DbParameter("Mode", "Insert"));
               
                object POGuid = await _commonRepository.ExecuteScalar(sqlStr, CommandType.StoredProcedure, dbParam);
                Guid PurchaseOrder_Guid = (Guid)POGuid;
                int ReturnVal = InsertPOItemDetail(PurchaseOrder_Guid, POItemData);
                ObjRes.ResultObject = await _commonRepository.GetValue<saveStatus>(ReturnVal);

            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

            }
            return ObjRes;
        }
        public int InsertPOItemDetail(Guid Invoice_Guid, string POItemDetails)
        {
            try
            {
                List<PurchaseOrderItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<PurchaseOrderItemDetails>>(POItemDetails);
                string sqlStr = "sp_PurchaseOrderDetails";

                foreach (PurchaseOrderItemDetails s in lstobjitemDetails)
                {

                    List<DbParameter> dbParam = new List<DbParameter>();

                    object RtnVal = DbHelper.ExecuteScalar(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);

                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;

            }

        }
    }
}
