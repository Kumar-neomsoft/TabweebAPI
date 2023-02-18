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
    public interface IQuotationRepository
    {
        Task<MethodResult<saveStatus>> InsertQuotation(string JsonQDetails, string JsonQItemData);
        Task<MethodResult<saveStatus>> DeleteQuotation(Guid BILL_GUID); 
        Task<MethodResult<List<QuotationGetRes>>> GetQuotationDetails(QuotationGetReq obj);
        Task<MethodResult<saveStatus>> UpdateQuotation(string JsonQuoData, string jsonQuoItemDatta);
        Task<MethodResult<List<ViewQuotationDetails>>> GetQuotationDetails(Guid BILL_GUID);
        Task<MethodResult<List<ViewQuotationDetails>>> GetQuotationItemDetails(Guid BILL_GUID);
    }
}
