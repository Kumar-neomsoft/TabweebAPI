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
//using System.Data.Common;
//using System.Data.Common;
//using System.Data.Common;
//using TabweebAPI.DBHelper;

namespace TabweebAPI.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        // private IDbTransaction _transaction;
        private IDbConnection _connection;
        //private bool _disposed;
        #endregion

        #region "Constructor"
        public PurchaseRepository(IConfiguration config)
        {
           
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
            _connection = new SqlConnection(_dbconn);
            _connection.Open();
            //_transaction = _connection.BeginTransaction();
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
            DynamicParameters dbParam = new DynamicParameters();
            int ReturnVal = 1;
           
            try
            {
               
                       if (_connection.State == ConnectionState.Closed)
                           _connection.Open();
                        using var _transaction = _connection.BeginTransaction();
                        try
                        {
                        dbParam.Add("@Mode", "InsertPO");
                        dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj["BILL_DOC_TYPE"], DbType.Int32);
                        dbParam.Add("@BILL_DATE", (DateTime?)obj["BILL_DATE"], DbType.DateTime);
                        dbParam.Add("@A_CY", (String?)obj["A_CY"], DbType.String);
                        dbParam.Add("@BILL_RATE", (Double?)obj["BILL_RATE"], DbType.Double);
                        dbParam.Add("@STOCK_RATE", (Double?)obj["STOCK_RATE"], DbType.Double);
                        dbParam.Add("@V_CODE", (String?)obj["V_CODE"], DbType.String);
                        dbParam.Add("@V_NAME", (String?)obj["V_NAME"], DbType.String);
                        dbParam.Add("@AC_DTL_TYP", (Int32?)obj["AC_DTL_TYP"], DbType.Int32);
                        dbParam.Add("@A_CODE", (String?)obj["A_CODE"], DbType.String);
                        dbParam.Add("@W_CODE", (Int32?)obj["W_CODE"], DbType.Int32);
                        dbParam.Add("@A_DESC", (String?)obj["A_DESC"], DbType.String);
                        dbParam.Add("@BRN_NO", (Int32?)obj["BRN_NO"], DbType.Int32);
                        dbParam.Add("@BILL_HUNG", (Boolean?)obj["BILL_HUNG"], DbType.Boolean);
                        dbParam.Add("@SOURC_BILL_NO", (Decimal?)obj["SOURC_BILL_NO"], DbType.Decimal);
                        dbParam.Add("@SOURC_BILL_TYP", (Int32?)obj["SOURC_BILL_TYP"], DbType.Int32);
                        dbParam.Add("@BILL_CASH", (Double?)obj["BILL_CASH"], DbType.Double);
                        dbParam.Add("@CASH_NO", (Int32?)obj["CASH_NO"], DbType.Int32);
                        dbParam.Add("@BILL_BANK", (Double?)obj["BILL_BANK"], DbType.Double);
                        dbParam.Add("@BANK_NO", (Int32?)obj["BANK_NO"], DbType.Int32);
                        dbParam.Add("@BILL_DR_ACCOUNT", (Double?)obj["BILL_DR_ACCOUNT"], DbType.Double);
                        dbParam.Add("@BILL_RT_AMT", (Double?)obj["BILL_RT_AMT"], DbType.Double);
                        dbParam.Add("@PRNT_NO", (Int32?)obj["PRNT_NO"], DbType.Int32);
                        dbParam.Add("@OLD_DOC_SER", (Decimal?)obj["OLD_DOC_SER"], DbType.Decimal);
                        dbParam.Add("@AR_TYPE", (Int32?)obj["AR_TYPE"], DbType.Int32);
                        dbParam.Add("@CERTIFIED", (Int32?)obj["CERTIFIED"], DbType.Int32);
                        dbParam.Add("@CERTIFIED_U_ID", (Int32?)obj["CERTIFIED_U_ID"], DbType.Int32);
                        dbParam.Add("@CERTIFIED_DATE", (DateTime?)obj["CERTIFIED_DATE"], DbType.DateTime);
                        dbParam.Add("@CERTIFIED_NOTES", (String?)obj["CERTIFIED_NOTES"], DbType.String);
                        dbParam.Add("@CERTIFIED_USED", (Int32?)obj["CERTIFIED_USED"], DbType.Int32);
                        dbParam.Add("@AD_U_ID", (Int32?)obj["AD_U_ID"], DbType.Int32);
                        dbParam.Add("@AD_DATE", (DateTime?)obj["AD_DATE"], DbType.DateTime);
                        dbParam.Add("@UP_U_ID", (Int32?)obj["UP_U_ID"], DbType.Int32);
                        dbParam.Add("@UP_DATE", (DateTime?)obj["UP_DATE"], DbType.DateTime);
                        dbParam.Add("@AD_TRMNL_NM", (String?)obj["AD_TRMNL_NM"], DbType.String);
                        dbParam.Add("@UP_TRMNL_NM", (String?)obj["UP_TRMNL_NM"], DbType.String);
                        dbParam.Add("@AP_TYPE", (Int32?)obj["AP_TYPE"], DbType.Int32);
                        Guid POGuid = _connection.Query<Guid>(sqlStr, dbParam, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();
                          
                            if (POGuid != Guid.Empty)
                            {
                                List<PurchaseOrderItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<PurchaseOrderItemDetails>>(POItemData);
                                string sqlStritem = "sp_PurchaseOrderDetails";
                                foreach (PurchaseOrderItemDetails s in lstobjitemDetails)
                                {
                                    DynamicParameters dbParamItem = new DynamicParameters();
                                    dbParamItem.Add("@Mode", "ItemInsert");
                                    dbParamItem.Add("@Item_I_CODE", (String)s.I_CODE, DbType.String);
                                    dbParamItem.Add("@Item_I_QTY", (Double?)s.I_QTY, DbType.Double);
                                    dbParamItem.Add("@Item_ITM_UNT", (String?)s.ITM_UNT, DbType.String);
                                    dbParamItem.Add("@Item_P_SIZE", (Double?)s.P_SIZE, DbType.Double);
                                    dbParamItem.Add("@Item_I_PRICE", (Double?)s.I_PRICE, DbType.Double);
                                    dbParamItem.Add("@Item_STK_COST", (Double?)s.STK_COST, DbType.Double);
                                    dbParamItem.Add("@Item_W_CODE", (Int32?)s.W_CODE, DbType.Int32);
                                    dbParamItem.Add("@Item_VAT_PER", (Double?)s.VAT_PER, DbType.Double);
                                    dbParamItem.Add("@Item_DIS_PER", (Double?)s.DIS_PER, DbType.Double);
                                    dbParamItem.Add("@Item_DIS_AMT", (Double?)s.DIS_AMT, DbType.Double);
                                    dbParamItem.Add("@Item_FREE_QTY", (Double?)s.FREE_QTY, DbType.Double);
                                    dbParamItem.Add("@Item_BARCODE", (String?)s.BARCODE, DbType.String);
                                    dbParamItem.Add("@Item_BILL_GUID", POGuid, DbType.Guid);
                                    ReturnVal=_connection.Query<int>(sqlStritem, dbParamItem, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();
                             
                            }
                        }

                        _transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            ReturnVal = 0;
                            _transaction.Rollback();
                            throw ex;
                        }
                    
                    ObjRes.ResultObject = await _commonRepository.GetValue<saveStatus>(ReturnVal);

            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return ObjRes;
        }
        public int InsertPOItemDetail(Guid PurchaseOrder_Guid, string POItemDetails)
        {
            try
            {
                List<PurchaseOrderItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<PurchaseOrderItemDetails>>(POItemDetails);
                string sqlStr = "sp_PurchaseOrderDetails";

                foreach (PurchaseOrderItemDetails s in lstobjitemDetails)
                {

                    List<DbParameter> dbParam = new List<DbParameter>();
                    dbParam.Add(new DbParameter("Mode", "ItemInsert"));
                    dbParam.Add(new DbParameter("Item_I_CODE", (String)s.I_CODE, DbType.String));
                    dbParam.Add(new DbParameter("Item_I_QTY", (Double?)s.I_QTY, DbType.Double));
                    dbParam.Add(new DbParameter("Item_ITM_UNT", (String?)s.ITM_UNT, DbType.String));
                    dbParam.Add(new DbParameter("Item_P_SIZE", (Double?)s.P_SIZE, DbType.Double));
                    dbParam.Add(new DbParameter("Item_I_PRICE", (Double?)s.I_PRICE, DbType.Double));
                    dbParam.Add(new DbParameter("Item_STK_COST", (Double?)s.STK_COST, DbType.Double));
                    dbParam.Add(new DbParameter("Item_W_CODE", (Int32?)s.W_CODE, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_VAT_PER", (Double?)s.VAT_PER, DbType.Double));
                    dbParam.Add(new DbParameter("Item_DIS_PER", (Double?)s.DIS_PER, DbType.Double));
                    dbParam.Add(new DbParameter("Item_DIS_AMT", (Double?)s.DIS_AMT, DbType.Double));
                    dbParam.Add(new DbParameter("Item_FREE_QTY", (Double?)s.FREE_QTY, DbType.Double));
                    dbParam.Add(new DbParameter("Item_BARCODE", (String?)s.BARCODE, DbType.String));
                    dbParam.Add(new DbParameter("Item_BILL_GUID", PurchaseOrder_Guid, DbType.Guid));
                    object RtnVal = DbHelper.ExecuteScalar(_dbconn, sqlStr, CommandType.StoredProcedure, dbParam);

                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;

            }

        }

        public async Task<MethodResult<saveStatus>> UpdatePurchaseOrder(string PODetails, string POItemData)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            JObject obj = JObject.Parse(PODetails);
            string sqlStr = "sp_PurchaseOrderDetails";
            DynamicParameters dbParam = new DynamicParameters();
            int ReturnVal = 1;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                using var _transaction = _connection.BeginTransaction();
                try
                {
                    dbParam.Add("@Mode", "UpdatePO");
                    dbParam.Add("@BILL_GUID", (Guid)obj["BILL_GUID"], DbType.Guid);
                    dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj["BILL_DOC_TYPE"], DbType.Int32);
                    dbParam.Add("@BILL_DATE", (DateTime?)obj["BILL_DATE"], DbType.DateTime);
                    dbParam.Add("@A_CY", (String?)obj["A_CY"], DbType.String);
                    dbParam.Add("@BILL_RATE", (Double?)obj["BILL_RATE"], DbType.Double);
                    dbParam.Add("@STOCK_RATE", (Double?)obj["STOCK_RATE"], DbType.Double);
                    dbParam.Add("@V_CODE", (String?)obj["V_CODE"], DbType.String);
                    dbParam.Add("@V_NAME", (String?)obj["V_NAME"], DbType.String);
                    dbParam.Add("@AC_DTL_TYP", (Int32?)obj["AC_DTL_TYP"], DbType.Int32);
                    dbParam.Add("@A_CODE", (String?)obj["A_CODE"], DbType.String);
                    dbParam.Add("@W_CODE", (Int32?)obj["W_CODE"], DbType.Int32);
                    dbParam.Add("@A_DESC", (String?)obj["A_DESC"], DbType.String);
                    dbParam.Add("@BRN_NO", (Int32?)obj["BRN_NO"], DbType.Int32);
                    dbParam.Add("@BILL_HUNG", (Boolean?)obj["BILL_HUNG"], DbType.Boolean);
                    dbParam.Add("@SOURC_BILL_NO", (Decimal?)obj["SOURC_BILL_NO"], DbType.Decimal);
                    dbParam.Add("@SOURC_BILL_TYP", (Int32?)obj["SOURC_BILL_TYP"], DbType.Int32);
                    dbParam.Add("@BILL_CASH", (Double?)obj["BILL_CASH"], DbType.Double);
                    dbParam.Add("@CASH_NO", (Int32?)obj["CASH_NO"], DbType.Int32);
                    dbParam.Add("@BILL_BANK", (Double?)obj["BILL_BANK"], DbType.Double);
                    dbParam.Add("@BANK_NO", (Int32?)obj["BANK_NO"], DbType.Int32);
                    dbParam.Add("@BILL_DR_ACCOUNT", (Double?)obj["BILL_DR_ACCOUNT"], DbType.Double);
                    dbParam.Add("@BILL_RT_AMT", (Double?)obj["BILL_RT_AMT"], DbType.Double);
                    dbParam.Add("@PRNT_NO", (Int32?)obj["PRNT_NO"], DbType.Int32);
                    dbParam.Add("@OLD_DOC_SER", (Decimal?)obj["OLD_DOC_SER"], DbType.Decimal);
                    dbParam.Add("@AR_TYPE", (Int32?)obj["AR_TYPE"], DbType.Int32);
                    dbParam.Add("@CERTIFIED", (Int32?)obj["CERTIFIED"], DbType.Int32);
                    dbParam.Add("@CERTIFIED_U_ID", (Int32?)obj["CERTIFIED_U_ID"], DbType.Int32);
                    dbParam.Add("@CERTIFIED_DATE", (DateTime?)obj["CERTIFIED_DATE"], DbType.DateTime);
                    dbParam.Add("@CERTIFIED_NOTES", (String?)obj["CERTIFIED_NOTES"], DbType.String);
                    dbParam.Add("@CERTIFIED_USED", (Int32?)obj["CERTIFIED_USED"], DbType.Int32);
                    dbParam.Add("@AD_U_ID", (Int32?)obj["AD_U_ID"], DbType.Int32);
                    dbParam.Add("@AD_DATE", (DateTime?)obj["AD_DATE"], DbType.DateTime);
                    dbParam.Add("@UP_U_ID", (Int32?)obj["UP_U_ID"], DbType.Int32);
                    dbParam.Add("@UP_DATE", (DateTime?)obj["UP_DATE"], DbType.DateTime);
                    dbParam.Add("@AD_TRMNL_NM", (String?)obj["AD_TRMNL_NM"], DbType.String);
                    dbParam.Add("@UP_TRMNL_NM", (String?)obj["UP_TRMNL_NM"], DbType.String);
                    dbParam.Add("@AP_TYPE", (Int32?)obj["AP_TYPE"], DbType.Int32);
                    Guid POGuid = _connection.Query<Guid>(sqlStr, dbParam, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();
                    if(POItemData !=null)
                    {
                        List<EditPOItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<EditPOItemDetails>>(POItemData);
                        string sqlStritem = "sp_PurchaseOrderDetails";

                        foreach (EditPOItemDetails s in lstobjitemDetails)
                        {

                            DynamicParameters dbParamItem = new DynamicParameters();

                            dbParamItem.Add("@Mode", "ItemUpdate");
                            dbParamItem.Add("@Item_BILL_DTL_GUID", (Guid)s.BILL_DTL_GUID, DbType.Guid);
                            dbParamItem.Add("@Item_I_CODE", (String)s.I_CODE, DbType.String);
                            dbParamItem.Add("@Item_I_QTY", (Double?)s.I_QTY, DbType.Double);
                            dbParamItem.Add("@Item_ITM_UNT", (String?)s.ITM_UNT, DbType.String);
                            dbParamItem.Add("@Item_P_SIZE", (Double?)s.P_SIZE, DbType.Double);
                            dbParamItem.Add("@Item_I_PRICE", (Double?)s.I_PRICE, DbType.Double);
                            dbParamItem.Add("@Item_STK_COST", (Double?)s.STK_COST, DbType.Double);
                            dbParamItem.Add("@Item_W_CODE", (Int32?)s.W_CODE, DbType.Int32);
                            dbParamItem.Add("@Item_VAT_PER", (Double?)s.VAT_PER, DbType.Double);
                            dbParamItem.Add("@Item_DIS_PER", (Double?)s.DIS_PER, DbType.Double);
                            dbParamItem.Add("@Item_DIS_AMT", (Double?)s.DIS_AMT, DbType.Double);
                            dbParamItem.Add("@Item_FREE_QTY", (Double?)s.FREE_QTY, DbType.Double);
                            dbParamItem.Add("@Item_BARCODE", (String?)s.BARCODE, DbType.String);
                            dbParamItem.Add("@Item_BILL_GUID", (Guid)s.BILL_GUID, DbType.Guid);
                            ReturnVal = _connection.Query<int>(sqlStritem, dbParamItem, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();

                        }
                    }
                    _transaction.Commit();
                }
                catch (Exception ex)
                {
                    ReturnVal = 0;
                    _transaction.Rollback();
                    throw ex;
                }
                ObjRes.ResultObject = await _commonRepository.GetValue<saveStatus>(ReturnVal);

            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return ObjRes;
        }

        
        public async Task<MethodResult<saveStatus>> DeletePurchaseOrder(Guid BILL_GUID)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            string sqlStr = "sp_PurchaseOrderDetails";
            List<DbParameter> dbParam = new List<DbParameter>();
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            using var _transaction = _connection.BeginTransaction();
            try
            {
                dbParam.Add(new DbParameter("Mode", "DeletePO"));
                dbParam.Add(new DbParameter("BILL_GUID", BILL_GUID, DbType.Guid));
                object POGuid = DbHelper.ExecuteScalar(sqlStr, CommandType.StoredProcedure, dbParam);
                ObjRes.ResultObject = await _commonRepository.GetValue<saveStatus>(POGuid);
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<PurchaseOrderViewDetails>>> EditPurchaseOrder(Guid BILL_GUID)
        {
            MethodResult<List<PurchaseOrderViewDetails>> ObjInvRes = new MethodResult<List<PurchaseOrderViewDetails>>();
            List<PurchaseOrderViewDetails>? Objres = new List<PurchaseOrderViewDetails>();
            try
            {
                var Result = "";
                DataSet ds = new DataSet();
                string sqlStr = "sp_PurchaseOrderDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "POEdit", DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", BILL_GUID, DbType.Guid));
                ds = await _commonRepository.ExecuteDataSet(sqlStr, CommandType.StoredProcedure, dbParam);
                DataTable dtPOData = new DataTable();
                DataTable dtPOItemData = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dtPOData = ds.Tables[0];
                        dtPOItemData = ds.Tables[1];
                        Result = JsonConvert.SerializeObject(dtPOData, Formatting.Indented);
                        string strItemDetails = JsonConvert.SerializeObject(dtPOItemData);
                        List<PurchaseOrderItemViewDetails>? lstPOItemDetails = JsonConvert.DeserializeObject<List<PurchaseOrderItemViewDetails>>(strItemDetails);
                        Objres = JsonConvert.DeserializeObject<List<PurchaseOrderViewDetails>>(Result);
                        Objres[0].PurchaseOrderItemViewList = lstPOItemDetails;
                        ObjInvRes.ResultObject = Objres;
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjInvRes;
        }
        public async Task<MethodResult<List<PurchaseOrderGetRes>>> GetAllPurchaseOrderDetails(PurchaseOrderGetReq obj)
        {
            MethodResult<List<PurchaseOrderGetRes>> responseObject = new MethodResult<List<PurchaseOrderGetRes>>();
            try
            {
                DataTable dt = new DataTable();
                string sqlStr = "sp_PurchaseOrderDetails";
                DynamicParameters dbParam = new DynamicParameters();
                dbParam.Add("@Mode", "Select", DbType.String);
                dbParam.Add("@BILL_GUID", (Guid?)obj.BILL_GUID, DbType.Guid);
                dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj.BILL_DOC_TYPE, DbType.Int32);
                dbParam.Add("@PO_BILL_SER", (Double?)obj.BILL_SER, DbType.Double);
                dbParam.Add("@PO_BILL_NO", (Int32?)obj.BILL_NO, DbType.Int32);
                dbParam.Add("@RecordFrom", (Int32?)obj.RecordFrom, DbType.Int32);
                dbParam.Add("@RecordTo", (Int32?)obj.RecordTo, DbType.Int32);
                var Result = await _commonRepository.GetList<PurchaseOrderGetRes>(sqlStr, dbParam);
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
        public async Task<MethodResult<List<GetPOItemDetails>>> GetPOItemDetails(Guid BILL_GUID)
        {
            
            MethodResult<List<GetPOItemDetails>> responseObject = new MethodResult<List<GetPOItemDetails>>();
            try
            {
              
                DataTable dt = new DataTable();
                string sqlStr = "sp_PurchaseOrderDetails";
                DynamicParameters dbParam = new DynamicParameters();
                dbParam.Add("@Mode", "ItemDetail", DbType.String);
                dbParam.Add("@BILL_GUID", BILL_GUID, DbType.Guid);
                var Result = await _commonRepository.GetList<GetPOItemDetails>(sqlStr, dbParam);
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
