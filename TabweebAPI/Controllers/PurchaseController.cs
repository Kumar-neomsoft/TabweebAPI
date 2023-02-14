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
                ////Validate JWT token validation
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
                ////Validate JWT token validation
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

        [HttpPost("InsertPurchaseOrder")]
        public async Task<IActionResult> InsertPurchaseOrder([FromBody] PurchaseOrderDetails PODetails)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }

                if (PODetails == null)
                    return BadRequest("Purchase Order Details is null");
                if (PODetails.PurchaseOrderItemList.Count == 0)
                    return BadRequest("PurchaseOrder Item Details is null");

                string JsonPOData = JsonConvert.SerializeObject(PODetails);
                string jsonPOItemData = JsonConvert.SerializeObject(PODetails.PurchaseOrderItemList);
                var Result = await _purchaseRepository.InsertPurchaseOrder(JsonPOData, jsonPOItemData);
                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Insert);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside Insert PurchaseOrder Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("UpdatePurchaseOrder")]
        public async Task<IActionResult> UpdatePurchaseOrder([FromBody] EditPODetails PODetails)
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
                if (PODetails == null)
                    return BadRequest("Purchase Order Details is null");
                if (PODetails.PurchaseOrderItemList.Count == 0)
                    return BadRequest("PurchaseOrder Item Details is null");

                string JsonPOData = JsonConvert.SerializeObject(PODetails);
                string jsonPOItemData = JsonConvert.SerializeObject(PODetails.PurchaseOrderItemList);
                var Result = await _purchaseRepository.UpdatePurchaseOrder(JsonPOData, jsonPOItemData);

                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Update);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside UpdatePurchaseOrder Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("DeletePurchaseOrder")]
        public async Task<IActionResult> DeletePurchaseOrder(Guid BILL_GUID)
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
                    return BadRequest("Enter Bill Guid");
                var Result = await _purchaseRepository.DeletePurchaseOrder(BILL_GUID);
                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Delete);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside DeletePurchaseOrder Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetPurchaseOrdertemDetails")]
        public async Task<IActionResult> GetPOItemDetails(Guid BILL_GUID)
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

                var Result = await _purchaseRepository.GetPOItemDetails(BILL_GUID);
                return _commonController.ProcessGetRes<GetPOItemDetails>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetPOItemDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetAllPurchaseOrderDetails")]
        public async Task<IActionResult> GetAllPODetails([FromForm]PurchaseOrderGetReq obj)
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
                    return BadRequest("Purchare order reuest cannot be null");

                var Result = await _purchaseRepository.GetAllPurchaseOrderDetails(obj);
                return _commonController.ProcessGetRes<PurchaseOrderGetRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetPOItemDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPurchaseOrderDetails")]
        public async Task<IActionResult> EditPurchaseOrder(Guid BILL_GUID)
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

                var Result = await _purchaseRepository.EditPurchaseOrder(BILL_GUID);
                return _commonController.ProcessGetResponse<PurchaseOrderViewDetails>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside EditPurchaseOrder Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
