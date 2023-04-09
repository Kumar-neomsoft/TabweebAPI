using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;
using Tabweeb_Model;
using TabweebAPI.Common;
using Microsoft.AspNetCore.Mvc;
namespace TabweebAPI.IRepository
{
    public interface IPurchaseRepository
    {
        Task<MethodResult<List<VendorRes>>> GetVendor(VendorReq obj);
        Task<MethodResult<List<AllVendorRes>>> GetAllVendor();
        Task<MethodResult<List<CashBankRes>>> GetCashBankNo(CashBankReq obj);
        //Task<MethodResult<saveStatus>> InsertPurchaseOrder(string PODetails, string POItemData);
        Task<MethodResult<List<PurchaseOrderViewDetails>>> EditPurchaseOrder(Guid BILL_GUID);
        Task<MethodResult<saveStatus>> DeletePurchaseOrder(Guid BILL_GUID);
        Task<MethodResult<saveStatus>> UpdatePurchaseOrder(string PODetails, string POItemData);

        Task<MethodResult<saveStatus>> InsertPurchaseOrder(string PODetails, string POItemData);

        Task<MethodResult<List<GetPOItemDetails>>> GetPOItemDetails(Guid BILL_GUID);
        Task<MethodResult<List<PurchaseOrderGetRes>>> GetAllPurchaseOrderDetails(PurchaseOrderGetReq obj);
    }
}
