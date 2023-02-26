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
    public class QuotationRepository : IQuotationRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IDbConnection _connection;
        #endregion

        #region "Constructor"
        public QuotationRepository(IConfiguration config)
        {

            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
            _connection = new SqlConnection(_dbconn);
            _connection.Open();
        }
        #endregion

        public async Task<MethodResult<saveStatus>> InsertQuotation(string JsonQDetails, string JsonQItemData)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            JObject obj = JObject.Parse(JsonQDetails);
            string sqlStr = "sp_QuotationDetails";
            DynamicParameters dbParam = new DynamicParameters();
            int ReturnVal = 1;

            try
            {

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                using var _transaction = _connection.BeginTransaction();
                try
                {
                    dbParam.Add("@Mode", "InsertQuo");
                    dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj["BILL_DOC_TYPE"], DbType.Int32);
                    dbParam.Add("@BILL_DATE", (DateTime?)obj["BILL_DATE"], DbType.DateTime);
                    dbParam.Add("@A_CY", (String?)obj["A_CY"], DbType.String);
                    dbParam.Add("@BILL_RATE", (Double?)obj["BILL_RATE"], DbType.Double);
                    dbParam.Add("@STOCK_RATE", (Double?)obj["STOCK_RATE"], DbType.Double);
                    dbParam.Add("@C_CODE", (String?)obj["C_CODE"], DbType.String);
                    dbParam.Add("@C_NAME", (String?)obj["C_NAME"], DbType.String);
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
                    dbParam.Add("@SAL_MAN", (Int32?)obj["SAL_MAN"], DbType.Int32);
                    Guid QuotationGuid = _connection.Query<Guid>(sqlStr, dbParam, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();

                    if (QuotationGuid != Guid.Empty)
                    {
                        List<QuotationItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<QuotationItemDetails>>(JsonQItemData);
                        string sqlStritem = "sp_QuotationDetails";
                        foreach (QuotationItemDetails s in lstobjitemDetails)
                        {
                            DynamicParameters dbParamItem = new DynamicParameters();
                            dbParamItem.Add("@Mode", "InsertItem");
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
                            dbParamItem.Add("@Item_SAL_MAN", (Int32?)s.AR_TYPE, DbType.Int32);
                            dbParamItem.Add("@Item_Field1", (String?)s.Field1, DbType.String);
                            dbParamItem.Add("@Item_Field2", (String?)s.Field2, DbType.String);
                            dbParamItem.Add("@Item_Field3", (String?)s.Field3, DbType.String);
                            dbParamItem.Add("@Item_Field4", (String?)s.Field4, DbType.String);
                            dbParamItem.Add("@Item_Field5", (String?)s.Field5, DbType.String);
                            dbParamItem.Add("@Item_Field6", (String?)s.Field6, DbType.String);
                            dbParamItem.Add("@Item_Field7", (String?)s.Field7, DbType.String);
                            dbParamItem.Add("@Item_Field8", (String?)s.Field8, DbType.String);
                            dbParamItem.Add("@Item_Field9", (String?)s.Field9, DbType.String);
                            dbParamItem.Add("@Item_Field10", (String?)s.Field10, DbType.String);
                            dbParamItem.Add("@Item_BILL_GUID", QuotationGuid, DbType.Guid);
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
        public async Task<MethodResult<List<QuotationGetRes>>> GetQuotationDetails(QuotationGetReq obj)
        {
            MethodResult<List<QuotationGetRes>> responseObject = new MethodResult<List<QuotationGetRes>>();
            try
            {

                string sqlStr = "sp_QuotationDetails";
                DynamicParameters dbParam = new DynamicParameters();
                dbParam.Add("@Mode", "Select", DbType.String);
                dbParam.Add("@BILL_GUID", (Guid?)obj.BILL_GUID, DbType.Guid);
                dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj.BILL_DOC_TYPE, DbType.Int32);
                dbParam.Add("@PO_BILL_SER", (Double?)obj.BILL_SER, DbType.Double);
                dbParam.Add("@PO_BILL_NO", (Int32?)obj.BILL_NO, DbType.Int32);
                dbParam.Add("@RecordFrom", (Int32?)obj.RecordFrom, DbType.Int32);
                dbParam.Add("@RecordTo", (Int32?)obj.RecordTo, DbType.Int32);
                var Result = await _commonRepository.GetList<QuotationGetRes>(sqlStr, dbParam);
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
        public async Task<MethodResult<saveStatus>> UpdateQuotation(string JsonQuoData, string jsonQuoItemDatta)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            JObject obj = JObject.Parse(JsonQuoData);
            string sqlStr = "sp_QuotationDetails";
            DynamicParameters dbParam = new DynamicParameters();
            int ReturnVal = 1;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                using var _transaction = _connection.BeginTransaction();
                try
                {
                    dbParam.Add("@Mode", "UpdateQuo");
                    dbParam.Add("@BILL_GUID", (Guid)obj["BILL_GUID"], DbType.Guid);
                    dbParam.Add("@BILL_DOC_TYPE", (Int32?)obj["BILL_DOC_TYPE"], DbType.Int32);
                    dbParam.Add("@BILL_DATE", (DateTime?)obj["BILL_DATE"], DbType.DateTime);
                    dbParam.Add("@A_CY", (String?)obj["A_CY"], DbType.String);
                    dbParam.Add("@BILL_RATE", (Double?)obj["BILL_RATE"], DbType.Double);
                    dbParam.Add("@STOCK_RATE", (Double?)obj["STOCK_RATE"], DbType.Double);
                    dbParam.Add("@C_CODE", (String?)obj["C_CODE"], DbType.String);
                    dbParam.Add("@C_NAME", (String?)obj["C_NAME"], DbType.String);
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
                    dbParam.Add("@SAL_MAN", (Int32?)obj["SAL_MAN"], DbType.Int32);
                    Guid QuotationGuid = _connection.Query<Guid>(sqlStr, dbParam, commandType: CommandType.StoredProcedure, transaction: _transaction).FirstOrDefault();
                    if (jsonQuoItemDatta != null)
                    {
                        List<UpdateQuotationItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<UpdateQuotationItemDetails>>(jsonQuoItemDatta);
                        string sqlStritem = "sp_QuotationDetails";

                        foreach (UpdateQuotationItemDetails s in lstobjitemDetails)
                        {

                            DynamicParameters dbParamItem = new DynamicParameters();

                            dbParamItem.Add("@Mode", "ItemUpdate");
                            dbParamItem.Add("@Item_BILL_DTL_GUID", (Guid)s.BILL_DTL_GUID, DbType.Guid);
                            dbParamItem.Add("@Item_BILL_GUID", (Guid)s.BILL_GUID, DbType.Guid);
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
                            dbParamItem.Add("@Item_Field1", (String?)s.Field1, DbType.String);
                            dbParamItem.Add("@Item_Field2", (String?)s.Field2, DbType.String);
                            dbParamItem.Add("@Item_Field3", (String?)s.Field3, DbType.String);
                            dbParamItem.Add("@Item_Field4", (String?)s.Field4, DbType.String);
                            dbParamItem.Add("@Item_Field5", (String?)s.Field5, DbType.String);
                            dbParamItem.Add("@Item_Field6", (String?)s.Field6, DbType.String);
                            dbParamItem.Add("@Item_Field7", (String?)s.Field7, DbType.String);
                            dbParamItem.Add("@Item_Field8", (String?)s.Field8, DbType.String);
                            dbParamItem.Add("@Item_Field9", (String?)s.Field9, DbType.String);
                            dbParamItem.Add("@Item_Field10", (String?)s.Field10, DbType.String);
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
        public async Task<MethodResult<List<ViewQuotationDetails>>> GetQuotationItemDetails(Guid BILL_GUID)
        {
            MethodResult<List<ViewQuotationDetails>> responseObject = new MethodResult<List<ViewQuotationDetails>>();
            try
            {
                //var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_QuotationDetails";
                DynamicParameters dbParam = new DynamicParameters();
                dbParam.Add("@Mode", "ItemDetail", DbType.String);
                dbParam.Add("@BILL_GUID", BILL_GUID, DbType.Guid);
                var Result = await _commonRepository.GetList<ViewQuotationDetails>(sqlStr, dbParam);
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
        public async Task<MethodResult<List<ViewQuotationDetails>>> GetQuotationDetails(Guid BILL_GUID)
        {
            MethodResult<List<ViewQuotationDetails>> ObjQuotationRes = new MethodResult<List<ViewQuotationDetails>>();
            List<ViewQuotationDetails>? Objres = new List<ViewQuotationDetails>();
            try
            {
                var Result = "";
                DataSet ds = new DataSet();
                string sqlStr = "sp_QuotationDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "EditQuo", DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", BILL_GUID, DbType.Guid));
                ds = await _commonRepository.ExecuteDataSet(sqlStr, CommandType.StoredProcedure, dbParam);
                DataTable dtQuoData = new DataTable();
                DataTable dtQuoItemData = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dtQuoData = ds.Tables[0];
                        dtQuoItemData = ds.Tables[1];
                        Result = JsonConvert.SerializeObject(dtQuoData, Formatting.Indented);
                        string strItemDetails = JsonConvert.SerializeObject(dtQuoItemData);
                        List<ViewQuotationItemDetails>? lstQuoItemDetails = JsonConvert.DeserializeObject<List<ViewQuotationItemDetails>>(strItemDetails);
                        Objres = JsonConvert.DeserializeObject<List<ViewQuotationDetails>>(Result);
                        Objres[0].QuotationItemList = lstQuoItemDetails;
                        ObjQuotationRes.ResultObject = Objres;
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjQuotationRes;
        }
        public async Task<MethodResult<saveStatus>> DeleteQuotation(Guid BILL_GUID)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            string sqlStr = "sp_QuotationDetails";
            List<DbParameter> dbParam = new List<DbParameter>();
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            using var _transaction = _connection.BeginTransaction();
            try
            {
                dbParam.Add(new DbParameter("Mode", "DeleteQuo"));
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
    }
}
