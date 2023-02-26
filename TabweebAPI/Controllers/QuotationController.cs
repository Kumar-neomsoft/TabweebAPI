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
    public class QuotationController : Controller
    {
        #region "Declarations"
        private readonly IQuotationRepository _quotationRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Quotation";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public QuotationController(IConfiguration iconfig)
        {
            _quotationRepository = new QuotationRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpPost("InsertQuotation")]
        public async Task<IActionResult> InsertQuotation([FromBody] QuotationDetails QuotationDetails)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }

                if (QuotationDetails == null)
                    return BadRequest("Quotation Details is null");
                if (QuotationDetails.QuotationItemList.Count == 0)
                    return BadRequest("Quotation Item Details is null");

                string JsonQDetails = JsonConvert.SerializeObject(QuotationDetails);
                string JsonQItemData = JsonConvert.SerializeObject(QuotationDetails.QuotationItemList);
                var Result = await _quotationRepository.InsertQuotation(JsonQDetails, JsonQItemData);
                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Insert);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside Insert Quotation Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("UpdateQuotation")]
        public async Task<IActionResult> UpdateQuotation([FromBody] UpdateQuotationDetails QuotationDetails)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }

                //Get the result from repository
                if (QuotationDetails == null)
                    return BadRequest("QuotationDetails is null");
                if (QuotationDetails.QuotationItemDetailsList.Count == 0)
                    return BadRequest("Quotation Item Details is null");

                string JsonQuoData = JsonConvert.SerializeObject(QuotationDetails);
                string jsonQuoItemData = JsonConvert.SerializeObject(QuotationDetails.QuotationItemDetailsList);
                var Result = await _quotationRepository.UpdateQuotation(JsonQuoData, jsonQuoItemData);

                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Update);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside UpdateQuotation Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("DeleteQuotation")]
        public async Task<IActionResult> DeleteQuotation(Guid BILL_GUID)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (BILL_GUID == Guid.Empty)
                    return BadRequest("Enter Quotation Guid");
                var Result = await _quotationRepository.DeleteQuotation(BILL_GUID);
                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Delete);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside DeleteQuotation Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetQuotationDetails")]
        public async Task<IActionResult> GetQuotationDetails(Guid BILL_GUID)
        {
            try
            {
                ////Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (BILL_GUID == Guid.Empty)
                    return BadRequest("Bill Guid cannot be null");

                var Result = await _quotationRepository.GetQuotationDetails(BILL_GUID);
                return _commonController.ProcessGetResponse<ViewQuotationDetails>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetQuotationDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetQuotationItemDetails")]
        public async Task<IActionResult> GetQuotationItemDetails(Guid BILL_GUID)
        {
            try
            {
                ////Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (BILL_GUID == Guid.Empty)
                    return BadRequest("Bill Guid cannot be null");

                var Result = await _quotationRepository.GetQuotationItemDetails(BILL_GUID);
                return _commonController.ProcessGetRes<ViewQuotationDetails>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetQuotationItemDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("SearchQuotationDetails")]
        public async Task<IActionResult> GetQuotationDetails([FromBody] QuotationGetReq obj)
        {
            try
            {
                ////Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (obj == null)
                    return BadRequest("GetQuotation request cannot be null");

                var Result = await _quotationRepository.GetQuotationDetails(obj);
                return _commonController.ProcessGetRes<QuotationGetRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetQuotationDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
