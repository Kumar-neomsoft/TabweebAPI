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
    public interface ISalesRepRepository
    {
       // Task<MethodResult<List<SalesRep>>> GetSalesRepById(int CompanyId);
        Task<MethodResult<List<SalesRep>>> GetSalesRep();
    }
}
