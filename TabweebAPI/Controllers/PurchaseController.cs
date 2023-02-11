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
    public class PurchaseController : Controller
    {
        #region "Declarations"
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "PurchaseOrder";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public PurchaseController(IConfiguration iconfig)
        {
            _purchaseRepository = new PurchaseRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpPost("GetVendor")]
        public async Task<IActionResult> GetVendor([FromForm] VendorReq obj)
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
                {
                    return StatusCode(500, "VendorReq cannot be null");
                }
                var Result = await _purchaseRepository.GetVendor(obj);

                return _commonController.ProcessGetResponse<VendorRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetVendor Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }
        [HttpPost("GetCashBankNo")]
        public async Task<IActionResult> GetCashBankNo([FromForm] CashBankReq obj)
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
                {
                    return StatusCode(500, "CashBankReq cannot be null");
                }
                var Result = await _purchaseRepository.GetCashBankNo(obj);

                return _commonController.ProcessGetResponse<CashBankRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetCashBankNo Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }

        
    }
}
