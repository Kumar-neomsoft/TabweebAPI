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
using Microsoft.AspNetCore.Hosting;
namespace TabweebAPI.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        #region "Declarations"
        private readonly CommonRepository _commonRepository;
        private readonly IConfiguration _config;
        private string _dbconn = string.Empty;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        //private IHostingEnvironment Environment;
        #endregion

        #region "Constructor"
        public NotificationRepository(IConfiguration config)
        {
            _config = config;
            _commonRepository = new CommonRepository();
            _dbconn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DBConnection"];
        }
        #endregion


        public async Task<MethodResult<saveStatus>> SendNotification(QuotationMailSend obj, string AppPath)
        {
            MethodResult<saveStatus> ObjRes = new MethodResult<saveStatus>();
            MethodResult<List<ViewQuotationDetails>> ObjQuotationRes = new MethodResult<List<ViewQuotationDetails>>();
            List<ViewQuotationDetails>? Objres = new List<ViewQuotationDetails>();
            try
            {
               
                int ReturnVal = -1;
            
                DataSet ds = new DataSet();
                string sqlStr = "sp_QuotationDetails";
                List<DbParameter> dbParam = new List<DbParameter>();
                dbParam.Add(new DbParameter("Mode", "MailQuo", DbType.String));
                dbParam.Add(new DbParameter("BILL_GUID", obj.BILL_GUID, DbType.Guid));
                ds = await _commonRepository.ExecuteDataSet(sqlStr, CommandType.StoredProcedure, dbParam);
                if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtQuoData = ds.Tables[0];
                    DataTable dtQuoItemData = ds.Tables[1];
                    DataTable dtCompany = ds.Tables[2];
                    string QuotationDataJson = JsonConvert.SerializeObject(dtQuoData);
                    string QuotationCostDetails = JsonConvert.SerializeObject(dtQuoItemData);
                    List<PreviewDataDetailModel> lstQuotationData = JsonConvert.DeserializeObject<List<PreviewDataDetailModel>>(QuotationDataJson);
                    List<PreviewDataDetailListModel> lstQuotationDataDetails = JsonConvert.DeserializeObject<List<PreviewDataDetailListModel>>(QuotationCostDetails);

                    string encrAccept = _commonRepository.ToUrlSafeBase64String(_commonRepository.Encrypt("true"));
                    string encrReject = _commonRepository.ToUrlSafeBase64String(_commonRepository.Encrypt("false"));
                    string rtn = "";
                    string Subject = "";
                    string ReturnValue = string.Empty;
                    string Url = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["ProjectURl"];

                    string email = obj.ToEmail;
                    string AdminEmail = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("KeyValues")["AdminEmail"];
                    string[] ToMail = { email };
                    string[] ToAdminEmail = { AdminEmail };
                    //string QuotationId = "1";
                    Guid QuotationId = obj.BILL_GUID;
                    string CompanyName = "";
                    
                    string MailContentPath = "";
                    if(obj.LANG_NO==2)
                    {
                        MailContentPath = AppPath + "//Content/MailContent/quotation-eng.html";
                        CompanyName = dtCompany.Rows[0]["CMP_FNAME"].ToString();
                        Subject = CompanyName+ " - Quotation Details";
                    }
                    else
                    {
                        MailContentPath = AppPath + "//Content/MailContent/quotation-ara.html";
                        CompanyName = dtCompany.Rows[0]["CMP_LNAME"].ToString();
                        Subject = CompanyName + " - Quotation Details";
                    }
                        
                    StreamReader FileContent = new StreamReader(MailContentPath);
                    rtn = FileContent.ReadToEnd();

                    string encrQuotationId = _commonRepository.ToUrlSafeBase64String(_commonRepository.Encrypt(QuotationId.ToString().Trim()));
                    string link_accept = "<a href ='" + Url + "/Quotation/UpdateQuotationStatus?QuotationId=" + encrQuotationId + "&QuotationStatus=" + encrAccept + "'>Accept Quotation</a>";
                    string link_reject = "<a href ='" + Url + "/Quotation/UpdateQuotationStatus?QuotationId=" + encrQuotationId + "&QuotationStatus=" + encrReject + "'>Reject Quotation</a>";


                    string trreplace = "";

                    for (int quocount = 0; quocount < lstQuotationDataDetails.Count; quocount++)
                    {
                        trreplace += "<tr><td>" + lstQuotationDataDetails[quocount].I_CODE + "</td><td> " + lstQuotationDataDetails[quocount].I_NAME + "</td><td> " + lstQuotationDataDetails[quocount].W_NAME + "</td><td> "
                            + lstQuotationDataDetails[quocount].ITM_UNT + "</td><td> " + lstQuotationDataDetails[quocount].I_QTY + "</td><td> " + lstQuotationDataDetails[quocount].I_PRICE + "</td><td> " + lstQuotationDataDetails[quocount].VAT_AMT + "</td><td> " + lstQuotationDataDetails[quocount].DIS_AMT + "</td><td> " + lstQuotationDataDetails[quocount].Total + "</td> </tr>";

                    }


                    decimal? addcha = 0, dischar = 0, total = 0, tax = 0;
                    if (lstQuotationData.Count > 0)
                    {

                        rtn = rtn.Replace("{Fullname}", lstQuotationData[0].C_NAME);
                        rtn = rtn.Replace("{BillDate}", lstQuotationData[0].BILL_DATE);
                        rtn = rtn.Replace("{InvoiceType}", lstQuotationData[0].BILL_DOC_TYPE);
                        rtn = rtn.Replace("{displaynumber}",  lstQuotationData[0].BILL_NO.ToString());
                        rtn = rtn.Replace("{CustomerNumber}", lstQuotationData[0].C_CODE);
                        rtn = rtn.Replace("{Statement}", "");
                        total = lstQuotationData[0].BILL_AMT;
                        dischar = lstQuotationData[0].DISC_AMT;
                        tax = lstQuotationData[0].VAT_AMT;

                    }
                    rtn = rtn.Replace("{trreplace}", trreplace);

                    rtn = rtn.Replace("{Discount}", Convert.ToString(dischar));
                    rtn = rtn.Replace("{TotalBill}", Convert.ToString(total));
                    rtn = rtn.Replace("{Tax}", Convert.ToString(tax));
                    rtn = rtn.Replace("{link_accept}", link_accept);
                    rtn = rtn.Replace("{link_reject}", link_reject);

                    rtn = rtn.Replace("{CLIENTCOMPANYNAME}", CompanyName);
                    rtn = rtn.Replace("{CLIENTEMAILID}", "companymail@gmail.com");
                    rtn = rtn.Replace("{CLIENTECONTACTNUMBER}", "3622372673");
                    string MailBody = rtn;
                    bool Ismailsend = false;
                    Ismailsend = _commonRepository.SendMailContent(Subject, MailBody, ToMail);

                    if (Ismailsend == true)
                        ReturnVal = 0;
                }
                else
                {
                    ReturnVal = 1;
                }

                ObjRes.ResultCode = saveStatus.success;
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
    }
}
