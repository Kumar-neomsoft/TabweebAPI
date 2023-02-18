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
using NLog;
using System.Collections;

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        #region "Declarations"
        private readonly ICustomerRepository _customerRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Customer";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public CustomerController(IConfiguration iconfig)
        {
            _customerRepository = new CustomerRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpPost("GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromBody] GetCustomerReq obj)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (obj == null)
                    return BadRequest("GetCustomer request cannot be null");

                var Result = await _customerRepository.GetCustomer(obj);
                return _commonController.ProcessGetRes<GetCustomerRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetCustomer Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
