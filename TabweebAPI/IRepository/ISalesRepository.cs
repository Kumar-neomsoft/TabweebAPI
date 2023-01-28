using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;
using Tabweeb_Model;
using TabweebAPI.Common;
using Newtonsoft.Json.Linq;
using Tabweeb_Model.Sales;

namespace TabweebAPI.IRepository
{
    interface ISalesRepository
    {
        Task<MethodResult<List<Currency>>> GetCurrencyDetails();
        Task<MethodResult<List<WareHouse>>> GetWareHouseDetails();
        Task<MethodResult<List<BillType>>> GetBillType(int LangId,int DocType);
    }
}
