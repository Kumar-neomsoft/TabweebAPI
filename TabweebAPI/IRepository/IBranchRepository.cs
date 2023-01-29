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
    public interface IBranchRepository
    {
        Task<MethodResult<List<BranchRes>>> GetBranchById(int CompanyId);
        Task<MethodResult<List<BranchRes>>> GetBranch();
    }
}
