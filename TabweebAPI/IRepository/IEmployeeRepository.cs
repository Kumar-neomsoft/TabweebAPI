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
    interface IEmployeeRepository
    {
        Task<MethodResult<List<Employee>>> GetEmployee(int BranchId);
    }
}
