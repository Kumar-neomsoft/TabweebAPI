﻿using Microsoft.AspNetCore.Http;
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
using NLog;

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
        private Logger _logger = LogManager.GetCurrentClassLogger();
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
                if (!ModelState.IsValid)
                {
                    return BadRequest("Parameter is missing");
                }
                List<LoginResponse> loginResponse = new List<LoginResponse>();
                loginResponse = await _jwtmiddleware.AuthenticateUser(LoginReq);
                MethodResult<List<LoginResponse>> responseObject = new MethodResult<List<LoginResponse>>();
                responseObject.ResultObject = loginResponse;
                return _commonController.ProcessGetResponse<LoginResponse>(responseObject.ResultObject.ToList(), PageName, CRUDAction.Select);
              
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside AuthenticateUser Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
           
        }

        
    }
}
