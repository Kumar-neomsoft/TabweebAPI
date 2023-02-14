using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Tabweeb_Model
{
    public class VendorReq
    {
        public string? V_CODE { get; set; }
        public string? V_A_NAME { get; set; }
        public string? V_A_CODE { get; set; }
        public int? RecordFrom { get; set; }
        public int? RecordTo { get; set; }
    }
    public class VendorRes
    {
        public string V_CODE { get; set; }
        public string V_A_NAME { get; set; }
        public string V_A_CODE { get; set; }
        public int TotalRowCount { get; set; }
    }
    public class CashBankReq
    {
        public int? CASH_TYPE_SAVE { get; set; }
        public int? RecordFrom { get; set; }
        public int? RecordTo { get; set; }
    }
    public class CashBankRes
    {
        public string CASH_NO { get; set; }
        public string CASH_NAME { get; set; }
        public int TotalRowCount { get; set; }
    }

    public class PurchaseOrderDetails
    {
        
        [Required(ErrorMessage = "Enter Document Type")]
        public int BILL_DOC_TYPE { get; set; }
        public DateTime? BILL_DATE { get; set; }
        public string? A_CY { get; set; }
        public double? BILL_RATE { get; set; }
        public double? STOCK_RATE { get; set; }
        public string? V_CODE { get; set; }
        public string? V_NAME { get; set; }
        public int? AC_DTL_TYP { get; set; }
        public string? A_CODE { get; set; }
        public int? W_CODE { get; set; }
        public string? A_DESC { get; set; }
        public int? BRN_NO { get; set; }
        public bool? BILL_HUNG { get; set; }
        public decimal? SOURC_BILL_NO { get; set; }
        public int? SOURC_BILL_TYP { get; set; }
        public double? BILL_CASH { get; set; }
        public int? CASH_NO { get; set; }
        public double? BILL_BANK { get; set; }
        public int? BANK_NO { get; set; }
        public double? BILL_DR_ACCOUNT { get; set; }
        public double? BILL_RT_AMT { get; set; }
        public int? PRNT_NO { get; set; }
        public decimal? OLD_DOC_SER { get; set; }
        public int? AR_TYPE { get; set; }
        public int? CERTIFIED { get; set; }
        public int? CERTIFIED_U_ID { get; set; }
        public DateTime? CERTIFIED_DATE { get; set; }
        public string? CERTIFIED_NOTES { get; set; }
        public int? CERTIFIED_USED { get; set; }
        public int? AD_U_ID { get; set; }
        public DateTime? AD_DATE { get; set; }
        public int? UP_U_ID { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string? AD_TRMNL_NM { get; set; }
        public string? UP_TRMNL_NM { get; set; }
        public int? AP_TYPE { get; set; }
        public List<PurchaseOrderItemDetails> PurchaseOrderItemList { get; set; }

    }
    public class PurchaseOrderItemDetails
    {
       
        [Required(ErrorMessage = "Enter I_CODE")]
        public string I_CODE { get; set; }
        [Required(ErrorMessage = "Enter I_QTY")]
        public double I_QTY { get; set; }
        public string? ITM_UNT { get; set; }
        public double? P_SIZE { get; set; }
        public double? I_PRICE { get; set; }
        public double? STK_COST { get; set; }
        public int? W_CODE { get; set; }
        public double? VAT_PER { get; set; }
        public double? DIS_PER { get; set; }
        public double? DIS_AMT { get; set; }
        public double FREE_QTY { get; set; } = 0;
        public string? BARCODE { get; set; }


    }

    public class EditPODetails
    {
        [Required(ErrorMessage = "Enter Bill Guid")]
        public Guid BILL_GUID { get; set; } = Guid.Empty;
        [Required(ErrorMessage = "Enter Document Type")]
        public int BILL_DOC_TYPE { get; set; }
        public DateTime? BILL_DATE { get; set; }
        public string? A_CY { get; set; }
        public double? BILL_RATE { get; set; }
        public double? STOCK_RATE { get; set; }
        public string? V_CODE { get; set; }
        public string? V_NAME { get; set; }
        public int? AC_DTL_TYP { get; set; }
        public string? A_CODE { get; set; }
        public int? W_CODE { get; set; }
        public string? A_DESC { get; set; }
        public int? BRN_NO { get; set; }
        public bool? BILL_HUNG { get; set; }
        public decimal? SOURC_BILL_NO { get; set; }
        public int? SOURC_BILL_TYP { get; set; }
        public double? BILL_CASH { get; set; }
        public int? CASH_NO { get; set; }
        public double? BILL_BANK { get; set; }
        public int? BANK_NO { get; set; }
        public double? BILL_DR_ACCOUNT { get; set; }
        public double? BILL_RT_AMT { get; set; }
        public int? PRNT_NO { get; set; }
        public decimal? OLD_DOC_SER { get; set; }
        public int? AR_TYPE { get; set; }
        public int? CERTIFIED { get; set; }
        public int? CERTIFIED_U_ID { get; set; }
        public DateTime? CERTIFIED_DATE { get; set; }
        public string? CERTIFIED_NOTES { get; set; }
        public int? CERTIFIED_USED { get; set; }
        public int? AD_U_ID { get; set; }
        public DateTime? AD_DATE { get; set; }
        public int? UP_U_ID { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string? AD_TRMNL_NM { get; set; }
        public string? UP_TRMNL_NM { get; set; }
        public int? AP_TYPE { get; set; }
        public List<EditPOItemDetails> PurchaseOrderItemList { get; set; }

    }

    public class EditPOItemDetails
    {
        //[Required(ErrorMessage = "Enter BillDetail Guid")]
        public Guid? BILL_DTL_GUID { get; set; }
        [Required(ErrorMessage = "Enter Bill Guid")]
        public Guid BILL_GUID { get; set; }
        [Required(ErrorMessage = "Enter I_CODE")]
        public string I_CODE { get; set; }
        [Required(ErrorMessage = "Enter I_QTY")]
        public double I_QTY { get; set; }
        public string? ITM_UNT { get; set; }
        public double? P_SIZE { get; set; }
        public double? I_PRICE { get; set; }
        public double? STK_COST { get; set; }
        public int? W_CODE { get; set; }
        public double? VAT_PER { get; set; }
        public double? DIS_PER { get; set; }
        public double? DIS_AMT { get; set; }
        public double FREE_QTY { get; set; } = 0;
        public string? BARCODE { get; set; }


    }
    public class PurchaseOrderViewDetails
    {
        public Guid? BILL_GUID { get; set; }
        public int BILL_DOC_TYPE { get; set; }
        public decimal? BILL_SER { get; set; }
        public decimal? BILL_NO { get; set; }
        public DateTime? BILL_DATE { get; set; }
        public string? A_CY { get; set; }
        public double? BILL_RATE { get; set; }
        public double? STOCK_RATE { get; set; }
        public string? V_CODE { get; set; }
        public double? VAT_AMT { get; set; }
        public double? BILL_AMT { get; set; }
        public double? DISC_AMT { get; set; }
        public double? DISC_AMT_MST { get; set; }
        public double? DISC_AMT_DTL { get; set; }
        public string? V_NAME { get; set; }
        public int? AC_DTL_TYP { get; set; }
        public string? A_CODE { get; set; }
        public int? W_CODE { get; set; }
        public string? A_DESC { get; set; }
        public int? BRN_NO { get; set; }
        public bool? BILL_HUNG { get; set; }
        public decimal? SOURC_BILL_NO { get; set; }
        public int? SOURC_BILL_TYP { get; set; }
        public double? BILL_CASH { get; set; }
        public int? CASH_NO { get; set; }
        public double? BILL_BANK { get; set; }
        public int? BANK_NO { get; set; }
        public double? BILL_DR_ACCOUNT { get; set; }
        public double? BILL_RT_AMT { get; set; }
        public int? PRNT_NO { get; set; }
        public decimal? OLD_DOC_SER { get; set; }
        public int? AR_TYPE { get; set; }
        public int? CERTIFIED { get; set; }
        public int? CERTIFIED_U_ID { get; set; }
        public DateTime? CERTIFIED_DATE { get; set; }
        public string? CERTIFIED_NOTES { get; set; }
        public int? CERTIFIED_USED { get; set; }
        public int? AD_U_ID { get; set; }
        public DateTime? AD_DATE { get; set; }
        public int? UP_U_ID { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string? AD_TRMNL_NM { get; set; }
        public string? UP_TRMNL_NM { get; set; }
        public int? AP_TYPE { get; set; }
     
        public List<PurchaseOrderItemViewDetails> PurchaseOrderItemViewList { get; set; }

    }

    public class PurchaseOrderItemViewDetails
    {
        public Guid BILL_DTL_GUID { get; set; }
        public Guid BILL_GUID { get; set; }
        public decimal BILL_SER { get; set; }
        public decimal BILL_NO { get; set; }
        public string I_CODE { get; set; }
        public double I_QTY { get; set; }
        public string? ITM_UNT { get; set; }
        public double P_QTY { get; set; }
        public double? P_SIZE { get; set; }
        public double? I_PRICE { get; set; }
        public double? STK_COST { get; set; }
        public int? W_CODE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public double? VAT_PER { get; set; }
        public double? VAT_AMT { get; set; }
        public double? DIS_PER { get; set; }
        public double? DIS_AMT { get; set; }
        public double FREE_QTY { get; set; } = 0;
        public string? BARCODE { get; set; }
        public int? RCRD_NO { get; set; }
        public int? AP_TYPE { get; set; }
    }

    public class GetPOItemDetails
    {
        public Guid BILL_DTL_GUID { get; set; }
        public Guid BILL_GUID { get; set; }
        public decimal BILL_SER { get; set; }
        public decimal BILL_NO { get; set; }
        public string I_CODE { get; set; }
        public double I_QTY { get; set; }
        public string? ITM_UNT { get; set; }
        public double P_QTY { get; set; }
        public double? P_SIZE { get; set; }
        public double? I_PRICE { get; set; }
        public double? STK_COST { get; set; }
        public int? W_CODE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public double? VAT_PER { get; set; }
        public double? VAT_AMT { get; set; }
        public double? DIS_PER { get; set; }
        public double? DIS_AMT { get; set; }
        public double FREE_QTY { get; set; } = 0;
        public string? BARCODE { get; set; }
        public int? RCRD_NO { get; set; }
        public int? AP_TYPE { get; set; }
    }

    public class PurchaseOrderGetReq
    {
        public Guid? BILL_GUID { get; set; } = Guid.Empty;
        public int? BILL_DOC_TYPE { get; set; }
        public decimal? BILL_SER { get; set; }
        public int? BILL_NO { get; set; }
        public int? RecordFrom { get; set; }
        public int? RecordTo { get; set; }
    }

    public class PurchaseOrderGetRes
    {
        public int TotalRowCount { get; set; }
        public Guid? BILL_GUID { get; set; }
        public int BILL_DOC_TYPE { get; set; }
        public decimal? BILL_SER { get; set; }
        public decimal? BILL_NO { get; set; }
        public DateTime? BILL_DATE { get; set; }
        public string? A_CY { get; set; }
        public double? BILL_RATE { get; set; }
        public double? STOCK_RATE { get; set; }
        public string? V_CODE { get; set; }
        public double? VAT_AMT { get; set; }
        public double? BILL_AMT { get; set; }
        public double? DISC_AMT { get; set; }
        public double? DISC_AMT_MST { get; set; }
        public double? DISC_AMT_DTL { get; set; }
        public string? V_NAME { get; set; }
        public int? AC_DTL_TYP { get; set; }
        public string? A_CODE { get; set; }
        public int? W_CODE { get; set; }
        public string? A_DESC { get; set; }
        public int? BRN_NO { get; set; }
        public bool? BILL_HUNG { get; set; }
        public decimal? SOURC_BILL_NO { get; set; }
        public int? SOURC_BILL_TYP { get; set; }
        public double? BILL_CASH { get; set; }
        public int? CASH_NO { get; set; }
        public double? BILL_BANK { get; set; }
        public int? BANK_NO { get; set; }
        public double? BILL_DR_ACCOUNT { get; set; }
        public double? BILL_RT_AMT { get; set; }
        public int? PRNT_NO { get; set; }
        public decimal? OLD_DOC_SER { get; set; }
        public int? AR_TYPE { get; set; }
        public int? CERTIFIED { get; set; }
        public int? CERTIFIED_U_ID { get; set; }
        public DateTime? CERTIFIED_DATE { get; set; }
        public string? CERTIFIED_NOTES { get; set; }
        public int? CERTIFIED_USED { get; set; }
        public int? AD_U_ID { get; set; }
        public DateTime? AD_DATE { get; set; }
        public int? UP_U_ID { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string? AD_TRMNL_NM { get; set; }
        public string? UP_TRMNL_NM { get; set; }
        public int? AP_TYPE { get; set; }
    }
}
