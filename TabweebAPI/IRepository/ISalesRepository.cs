using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;
using Tabweeb_Model;
using TabweebAPI.Common;
using Newtonsoft.Json.Linq;

namespace TabweebAPI.IRepository
{
    interface ISalesRepository
    {
        Task<MethodResult<List<BillType>>> GetBillType(int LangId,int DocType);
        Task<MethodResult<List<CCodeRes>>> GetCCode(int BranchId);
        Task<MethodResult<saveStatus>> InsertInvoice(string InvData, string InvItemData);
        Task<MethodResult<List<InvoiceDetails>>> EditInvoice(Guid BILL_GUID);
    }
}
