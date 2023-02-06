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
        
        public async Task<MethodResult<List<BillType>>> GetBillType(int LangId, int DocType)
        {
            MethodResult<List<BillType>> ObjRes = new MethodResult<List<BillType>>();
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
                ObjRes.ResultObject = WresponseObject;
                return ObjRes;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<InvoiceRes>>> GetInvoiceList(InvoiceReq Invobj)
        {
            MethodResult<List<InvoiceRes>> ObjRes = new MethodResult<List<InvoiceRes>>();
            List<InvoiceRes> SPresponseObject = new List<InvoiceRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_InvoiceDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "Select", DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", Invobj.BILL_GUID, DbType.String));
                dbParam.Add(new DbParameter("BILL_SER", Invobj.BILL_SER, DbType.Decimal));
                dbParam.Add(new DbParameter("C_NAME", Invobj.C_NAME, DbType.String));
                dbParam.Add(new DbParameter("BILL_NO", Invobj.BILL_NO, DbType.Decimal));
                dbParam.Add(new DbParameter("BILL_DOC_TYPE", Invobj.BILL_DOC_TYPE, DbType.Int32));
                dbParam.Add(new DbParameter("LANG_NO", Invobj.LANG_NO, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                SPresponseObject = JsonConvert.DeserializeObject<List<InvoiceRes>>(Result);
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
        public async Task<MethodResult<List<CCodeRes>>> GetCCode(int BranchId)
        {
            MethodResult<List<CCodeRes>> ObjRes = new MethodResult<List<CCodeRes>>();
            List<CCodeRes> WresponseObject = new List<CCodeRes>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetCCode", DbType.String));
                dbParam.Add(new DbParameter("BranchId", BranchId, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                WresponseObject = JsonConvert.DeserializeObject<List<CCodeRes>>(Result);
                ObjRes.ResultObject = WresponseObject;
                return ObjRes;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<saveStatus>> InsertInvoice(string InvDetails,string InvItemData)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            JObject obj = JObject.Parse(InvDetails);
            string sqlStr = "sp_InvoiceDetails";
            List<DbParameter> dbParam = new List<DbParameter>();
            try
            {
                dbParam.Add(new DbParameter("Mode", "Insert"));
                dbParam.Add(new DbParameter("BILL_SER", (Decimal)obj["BILL_SER"], DbType.Decimal));
                dbParam.Add(new DbParameter("BILL_DOC_TYPE", (Int32?)obj["BILL_DOC_TYPE"], DbType.Int32));
              
                dbParam.Add(new DbParameter("BILL_NO", (Decimal?)obj["BILL_NO"], DbType.Decimal));
                dbParam.Add(new DbParameter("BILL_DATE", (DateTime?)obj["BILL_DATE"], DbType.DateTime));
                dbParam.Add(new DbParameter("A_CY", (String?)obj["A_CY"], DbType.String));
                dbParam.Add(new DbParameter("BILL_RATE", (Double?)obj["BILL_RATE"], DbType.Double));
                dbParam.Add(new DbParameter("STOCK_RATE", (Double?)obj["STOCK_RATE"], DbType.Double));
                dbParam.Add(new DbParameter("C_CODE", (String?)obj["C_CODE"], DbType.String));
                dbParam.Add(new DbParameter("C_NAME", (String?)obj["C_NAME"], DbType.String));
                dbParam.Add(new DbParameter("AC_DTL_TYP", (Int32?)obj["AC_DTL_TYP"], DbType.Int32));
                dbParam.Add(new DbParameter("A_CODE", (String?)obj["A_CODE"], DbType.String));
                dbParam.Add(new DbParameter("VAT_AMT", (Double?)obj["VAT_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("BILL_AMT", (Double?)obj["BILL_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("W_CODE", (Int32?)obj["W_CODE"], DbType.Int32));
                dbParam.Add(new DbParameter("A_DESC", (String?)obj["A_DESC"], DbType.String));
                dbParam.Add(new DbParameter("BRN_NO", (Int32?)obj["BRN_NO"], DbType.Int32));
                dbParam.Add(new DbParameter("DISC_AMT", (Double?)obj["DISC_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("DISC_AMT_MST", (Double?)obj["DISC_AMT_MST"], DbType.Double));
                dbParam.Add(new DbParameter("DISC_AMT_DTL", (Double?)obj["DISC_AMT_DTL"], DbType.Double));
                dbParam.Add(new DbParameter("BILL_HUNG", (Boolean?)obj["BILL_HUNG"], DbType.Boolean));
                dbParam.Add(new DbParameter("SOURC_BILL_NO", (Decimal?)obj["SOURC_BILL_NO"], DbType.Decimal));
                dbParam.Add(new DbParameter("SOURC_BILL_TYP", (Int32?)obj["SOURC_BILL_TYP"], DbType.Int32));
                dbParam.Add(new DbParameter("PUSH_AMT", (Double?)obj["PUSH_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("RETURN_AMT", (Double?)obj["RETURN_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("BILL_CASH", (Double?)obj["BILL_CASH"], DbType.Double));
                dbParam.Add(new DbParameter("CASH_NO", (Int32?)obj["CASH_NO"], DbType.Int32));
                dbParam.Add(new DbParameter("BILL_BANK", (Double?)obj["BILL_BANK"], DbType.Double));
                dbParam.Add(new DbParameter("BANK_NO", (Int32?)obj["BANK_NO"], DbType.Int32));
                dbParam.Add(new DbParameter("BILL_DR_ACCOUNT", (Double?)obj["BILL_DR_ACCOUNT"], DbType.Double));
                dbParam.Add(new DbParameter("BILL_RT_AMT", (Double?)obj["BILL_RT_AMT"], DbType.Double));
                dbParam.Add(new DbParameter("PRNT_NO", (Int32?)obj["PRNT_NO"], DbType.Int32));
                dbParam.Add(new DbParameter("OLD_DOC_SER", (Decimal?)obj["OLD_DOC_SER"], DbType.Decimal));
                dbParam.Add(new DbParameter("AR_TYPE", (Int32?)obj["AR_TYPE"], DbType.Int32));
                dbParam.Add(new DbParameter("PaymentDone", (Boolean?)obj["PaymentDone"], DbType.Boolean));
                dbParam.Add(new DbParameter("PaymentDate", (DateTime?)obj["PaymentDate"], DbType.DateTime));
                dbParam.Add(new DbParameter("THE_DRIVER", (String?)obj["THE_DRIVER"], DbType.String));
                dbParam.Add(new DbParameter("AD_U_ID", (Int32?)obj["AD_U_ID"], DbType.Int32));
                dbParam.Add(new DbParameter("AD_DATE", (DateTime?)obj["AD_DATE"], DbType.DateTime));
                dbParam.Add(new DbParameter("UP_U_ID", (Int32?)obj["UP_U_ID"], DbType.Int32));
                dbParam.Add(new DbParameter("UP_DATE", (DateTime?)obj["UP_DATE"], DbType.DateTime));
                dbParam.Add(new DbParameter("AD_TRMNL_NM", (String?)obj["AD_TRMNL_NM"], DbType.String));
                dbParam.Add(new DbParameter("UP_TRMNL_NM", (String?)obj["UP_TRMNL_NM"], DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", (Guid)obj["BILL_GUID"], DbType.Guid));
                dbParam.Add(new DbParameter("BILL_COUNTER", (Int32?)obj["BILL_COUNTER"], DbType.Int32));
                dbParam.Add(new DbParameter("RoundingAmount", (Double?)obj["RoundingAmount"], DbType.Double));
                dbParam.Add(new DbParameter("C_PHONE", (String?)obj["C_PHONE"], DbType.String));
                dbParam.Add(new DbParameter("SAL_MAN", (Int32?)obj["SAL_MAN"], DbType.Int32));
                dbParam.Add(new DbParameter("DUE_DATE", (DateTime?)obj["DUE_DATE"], DbType.DateTime));
                dbParam.Add(new DbParameter("CC_CODE", (String?)obj["CC_CODE"], DbType.String));
                object InvoiceGuid = await _commonRepository.ExecuteScalar(sqlStr, CommandType.StoredProcedure, dbParam);
                Guid Invoice_Guid = (Guid)InvoiceGuid;
                int ReturnVal= InsertItemDetail(Invoice_Guid, InvItemData);
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
        public  int InsertItemDetail(Guid Invoice_Guid, string InvItemDetails)
        {
            try
            {
                List<InvoiceItemDetails> lstobjitemDetails = JsonConvert.DeserializeObject<List<InvoiceItemDetails>>(InvItemDetails);
                string sqlStr = "sp_InvoiceDetails";

                foreach (InvoiceItemDetails s in lstobjitemDetails)
                {

                    List<DbParameter> dbParam = new List<DbParameter>();
                    dbParam.Add(new DbParameter("Mode", "InsertItem"));
                    dbParam.Add(new DbParameter("Item_BILL_SER", (Decimal?)s.BILL_SER, DbType.Decimal));
                    dbParam.Add(new DbParameter("Item_BILL_DOC_TYPE", (Int32?)s.BILL_DOC_TYPE, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_BILL_NO", (Decimal?)s.BILL_NO, DbType.Decimal));
                    dbParam.Add(new DbParameter("Item_I_CODE", (String?)s.I_CODE, DbType.String));
                    dbParam.Add(new DbParameter("Item_I_QTY", (Double?)s.I_QTY, DbType.Double));
                    dbParam.Add(new DbParameter("Item_ITM_UNT", (String?)s.ITM_UNT, DbType.String));
                    dbParam.Add(new DbParameter("Item_P_SIZE", (Double?)s.P_SIZE, DbType.Double));
                    dbParam.Add(new DbParameter("Item_P_QTY", (Double?)s.P_QTY, DbType.Double));
                    dbParam.Add(new DbParameter("Item_I_PRICE", (Double?)s.I_PRICE, DbType.Double));
                    dbParam.Add(new DbParameter("Item_STK_COST", (Double?)s.STK_COST, DbType.Double));
                    dbParam.Add(new DbParameter("Item_W_CODE", (Int32?)s.W_CODE, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_EXPIRE_DATE", (DateTime?)s.EXPIRE_DATE, DbType.DateTime));
                    dbParam.Add(new DbParameter("Item_VAT_PER", (Double?)s.VAT_PER, DbType.Double));
                    dbParam.Add(new DbParameter("Item_VAT_AMT", (Double?)s.VAT_AMT, DbType.Double));
                    dbParam.Add(new DbParameter("Item_RCRD_NO", (Int32?)s.RCRD_NO, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_DIS_PER", (Double?)s.DIS_PER, DbType.Double));
                    dbParam.Add(new DbParameter("Item_DIS_AMT", (Double?)s.DIS_AMT, DbType.Double));
                    dbParam.Add(new DbParameter("Item_FREE_QTY", (Double?)s.FREE_QTY, DbType.Double));
                    dbParam.Add(new DbParameter("Item_BARCODE", (String?)s.BARCODE, DbType.String));
                    dbParam.Add(new DbParameter("Item_AR_TYPE", (Int32?)s.AR_TYPE, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_BILL_GUID", Invoice_Guid, DbType.Guid));
                    //dbParam.Add(new DbParameter("Item_BILL_DTL_GUID", System.Guid, DbType.Guid));
                    dbParam.Add(new DbParameter("Item_SAL_MAN", (Int32?)s.SAL_MAN, DbType.Int32));
                    dbParam.Add(new DbParameter("Item_CC_CODE", (String?)s.CC_CODE, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field1", (String?)s.Field1, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field2", (String?)s.Field2, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field3", (String?)s.Field3, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field4", (String?)s.Field4, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field5", (String?)s.Field5, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field6", (String?)s.Field6, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field7", (String?)s.Field7, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field8", (String?)s.Field8, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field9", (String?)s.Field9, DbType.String));
                    dbParam.Add(new DbParameter("Item_Field10", (String?)s.Field10, DbType.String));
                    object RtnVal = DbHelper.ExecuteScalar(_dbconn,sqlStr, CommandType.StoredProcedure, dbParam);

                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
              
            }

        }

        public async Task<MethodResult<List<BillSource>>> GetBillSource(int LangId)
        {
            MethodResult<List<BillSource>> ObjRes = new MethodResult<List<BillSource>>();
            List<BillSource> BSresponseObject = new List<BillSource>();
            try
            {
                var Result = "";
                DataTable dt = new DataTable();
                string sqlStr = "sp_GetMastersNS";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetBillSource", DbType.String));
                dbParam.Add(new DbParameter("LangId", LangId, DbType.Int32));
                dt = await _commonRepository.ExecuteDataTable(sqlStr, CommandType.StoredProcedure, dbParam);
                Result = JsonConvert.SerializeObject(dt, Formatting.Indented);
                BSresponseObject = JsonConvert.DeserializeObject<List<BillSource>>(Result);
                ObjRes.ResultObject = BSresponseObject;
                return ObjRes;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception message " + ex.Message);
                _logger.Error("InnerException message " + ex.InnerException);
                await _commonRepository.InsertUpdateErrorLog<List<saveStatus>>(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            }
            return ObjRes;
        }
        public async Task<MethodResult<List<InvoiceDetails>>> EditInvoice(Guid BILL_GUID)
        {
            MethodResult<List<InvoiceDetails>> ObjInvRes = new MethodResult<List<InvoiceDetails>>();
            List<InvoiceDetails>? Objres = new List<InvoiceDetails>();
            try
            {
                var Result = "";
                DataSet ds = new DataSet();
                string sqlStr = "sp_InvoiceDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "GetInvoice", DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", BILL_GUID, DbType.Guid));
                ds = await _commonRepository.ExecuteDataSet(sqlStr, CommandType.StoredProcedure, dbParam);
                DataTable dtInvoiceData = new DataTable();
                DataTable dtInvoiceItemData = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtInvoiceData = ds.Tables[0];
                    dtInvoiceItemData = ds.Tables[1];
                    Result = JsonConvert.SerializeObject(dtInvoiceData, Formatting.Indented);
                    string strItemDetails = JsonConvert.SerializeObject(dtInvoiceItemData);
                    List<InvoiceItemDetails>? lstInvItemDetails = JsonConvert.DeserializeObject<List<InvoiceItemDetails>>(strItemDetails);
                    Objres = JsonConvert.DeserializeObject<List<InvoiceDetails>>(Result);
                    Objres[0].InvoiceItemList = lstInvItemDetails;
                    ObjInvRes.ResultObject = Objres;
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
     
    }
}
