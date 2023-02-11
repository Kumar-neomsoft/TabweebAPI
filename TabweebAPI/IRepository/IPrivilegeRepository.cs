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
    public interface IPrivilegeRepository
    {
        Task<MethodResult<List<PrivilegeRes>>> GetPrivilege(PrivilegeReq obj);
    }
}
