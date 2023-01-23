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
    interface ILoginRepository
    {
        //Task<MethodResult<saveStatus>> AuthenticateUser(LoginRequest LoginReq);
        Task<MethodResult<List<LoginResponse>>> AuthenticateUser(LoginRequest LoginReq);
        public List<LoginResponse> ValidateUser(LoginRequest LoginReq);
        public long UpdateLoginSession(int UserId, string token);
        public SessionResponse SessionResponse(int vUserID);
        public long RefreshLoginSession(int UserId);
    }
}
