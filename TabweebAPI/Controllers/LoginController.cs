using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tabweeb_Model;
using static Tabweeb_Model.Common.commonclass;
using TabweebAPI.Repository;
using TabweebAPI.IRepository;
using TabweebAPI.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TabweebAPI.Middleware;

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : CommonController
    {
        #region "Declarations"
        private readonly ILoginRepository _loginRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Login";
        private readonly JwtMiddleware _jwtmiddleware;
        #endregion

        #region "Constructor"
        public LoginController(IConfiguration iconfig)
        {
            _loginRepository = new LoginRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequest LoginReq)
        {
            try
            {
                List<LoginResponse> loginResponse = new List<LoginResponse>();
                loginResponse = await _jwtmiddleware.AuthenticateUser(LoginReq);
                MethodResult<List<LoginResponse>> responseObject = new MethodResult<List<LoginResponse>>();
                responseObject.ResultObject = loginResponse;
                //Get the result from repository
                //var Result = await _loginRepository.AuthenticateUser(LoginReq);
                return _commonController.ProcessGetResponse<LoginResponse>(responseObject.ResultObject.ToList(), PageName, CRUDAction.Select);
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }

        
    }
}
