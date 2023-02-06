using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tabweeb_Model
{
    public class BillType
    {
        public int LangId { get; set; }
        public int FlagValue { get; set; }
        public string FlagCode { get; set; }
        public string FlagDesc { get; set; }
        public int DocType { get; set; }

    }
    public class InvoiceReq
    {
        public int? LANG_NO { get; set; }
        public string? BILL_GUID { get; set; }
        public decimal? BILL_SER { get; set; }
        public string? C_NAME { get; set; }
        public decimal? BILL_NO { get; set; }
        public int? BILL_DOC_TYPE { get; set; }
    }

    public class InvoiceRes
    {
        public decimal BILL_SER { get; set; }
        public int? BILL_DOC_TYPE { get; set; }
        public decimal BILL_NO { get; set; }
        public string? A_DESC { get; set; }
        public string? C_NAME { get; set; }
        public double? VAT_AMT { get; set; }
        public double? BILL_AMT { get; set; }
        public double? DISC_AMT { get; set; }
        public string? FLG_DESC { get; set; }
      
    }
    public class CCodeRes
    {
        public int CCode { get; set; }
        public string CName { get; set; }
        public int BranchId { get; set; }

    }
    public class BillSource
    {
        public int LANG_NO { get; set; }
        public int FORM_NO { get; set; }
        public string FORM_NAME { get; set; }
        public string FORM_NOTE { get; set; }

    }
    public class InvoiceDetails
    {
        public decimal BILL_SER { get; set; }
        public int? BILL_DOC_TYPE { get; set; }
        public decimal BILL_NO { get; set; }
        public DateTime? BILL_DATE { get; set; }
        public string? A_CY { get; set; }
        public double? BILL_RATE { get; set; }
        public double? STOCK_RATE { get; set; }
        public string? C_CODE { get; set; }
        public string? C_NAME { get; set; }
        public int? AC_DTL_TYP { get; set; }
        public string? A_CODE { get; set; }
        public double? VAT_AMT { get; set; }
        public double? BILL_AMT { get; set; }
        public int? W_CODE { get; set; }
        public string? A_DESC { get; set; }
        public int? BRN_NO { get; set; }
        public double? DISC_AMT { get; set; }
        public double? DISC_AMT_MST { get; set; }
        public double? DISC_AMT_DTL { get; set; }
        public bool? BILL_HUNG { get; set; }
        public decimal? SOURC_BILL_NO { get; set; }
        public int? SOURC_BILL_TYP { get; set; }
        public double? PUSH_AMT { get; set; }
        public double? RETURN_AMT { get; set; }
        public double? BILL_CASH { get; set; }
        public int? CASH_NO { get; set; }
        public double? BILL_BANK { get; set; }
        public int? BANK_NO { get; set; }
        public double? BILL_DR_ACCOUNT { get; set; }
        public double? BILL_RT_AMT { get; set; }
        public int? PRNT_NO { get; set; }
        public decimal? OLD_DOC_SER { get; set; }
        public int? AR_TYPE { get; set; }
        public bool? PaymentDone { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? THE_DRIVER { get; set; }
        public int? AD_U_ID { get; set; }
        public DateTime? AD_DATE { get; set; }
        public int? UP_U_ID { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string? AD_TRMNL_NM { get; set; }
        public string? UP_TRMNL_NM { get; set; }
        public System.Guid BILL_GUID { get; set; }
        public int? BILL_COUNTER { get; set; }
        public double? RoundingAmount { get; set; }
        public string? C_PHONE { get; set; }
        public int? SAL_MAN { get; set; }
        public DateTime? DUE_DATE { get; set; }
        public string? CC_CODE { get; set; }

        public List<InvoiceItemDetails> InvoiceItemList { get; set; }
      
    }
    public class InvoiceItemDetails
    {
        public decimal BILL_SER { get; set; }
        public int? BILL_DOC_TYPE { get; set; }
        public decimal BILL_NO { get; set; }
        public string I_CODE { get; set; }
        public double I_QTY { get; set; }
        public string? ITM_UNT { get; set; }
        public double? P_SIZE { get; set; }
        public double? P_QTY { get; set; }
        public double? I_PRICE { get; set; }
        public double? STK_COST { get; set; }
        public int? W_CODE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public double? VAT_PER { get; set; }
        public double? VAT_AMT { get; set; }
        public int RCRD_NO { get; set; }
        public double? DIS_PER { get; set; }
        public double? DIS_AMT { get; set; }
     
        public double FREE_QTY { get; set; } = 0;
        public string? BARCODE { get; set; }
        public int? AR_TYPE { get; set; }

        public Guid? BILL_GUID { get; set; }
        public Guid BILL_DTL_GUID { get; set; }
        public int? SAL_MAN { get; set; }
        public string? CC_CODE { get; set; }
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
        public string? Field3 { get; set; }
        public string? Field4 { get; set; }
        public string? Field5 { get; set; }
        public string? Field6 { get; set; }
        public string? Field7 { get; set; }
        public string? Field8 { get; set; }
        public string? Field9 { get; set; }
        public string? Field10 { get; set; }
    }

    
}
