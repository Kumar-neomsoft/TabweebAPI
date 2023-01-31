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
    public class SalesController : CommonController
    {
        #region "Declarations"
        private readonly ISalesRepository _salesRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Sales";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public SalesController(IConfiguration iconfig)
        {
            _salesRepository = new SalesRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
            

        [HttpGet("GetBillType")]
        public async Task<IActionResult> GetBillType(int LangNo, int DocType)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                   // throw new JwtMiddleware.HttpException(401, "Unauthorized access");
                    return  StatusCode(401);
                }
                //Get the result from repository
                var Result = await _salesRepository.GetBillType(LangNo, DocType);

                return _commonController.ProcessGetResponse<BillType>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBillType Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetBillSource")]
        public async Task<IActionResult> GetBillSource(int LangNo)
        {
            try
            {
                //Validate JWT token validation
                //var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                //if (returnValue.Equals("unauthorized"))
                //{
                //    // throw new JwtMiddleware.HttpException(401, "Unauthorized access");
                //    return StatusCode(401);
                //}
                //Get the result from repository
                var Result = await _salesRepository.GetBillSource(LangNo);

                return _commonController.ProcessGetResponse<BillSource>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBillSource Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetCCode")]
        public async Task<IActionResult> GetCCode(int BranchNo)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                //Get the result from repository
                var Result = await _salesRepository.GetCCode(BranchNo);

                return _commonController.ProcessGetResponse<CCodeRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetCCode Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("EditInvoice")]
        public async Task<IActionResult> EditInvoice(Guid BILL_GUID)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                var Result = await _salesRepository.EditInvoice(BILL_GUID);
                return _commonController.ProcessGetResponse<InvoiceDetails>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside EditInvoice Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("InsertInvoice")]
        public async Task<IActionResult> InsertInvoice(InvoiceDetails invDetails)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (invDetails == null)
                    return BadRequest("Invoice Details is null");
                if (invDetails.InvoiceItemList.Count == 0)
                    return BadRequest("Invoice Item Details is null");

                string JsonInvData = JsonConvert.SerializeObject(invDetails);
                string jsonInvItemData = JsonConvert.SerializeObject(invDetails.InvoiceItemList);
                var Result = await _salesRepository.InsertInvoice(JsonInvData, jsonInvItemData);
                return  _commonController.ProcessResponse <saveStatus > (Result.ResultObject, "Invoice", CRUDAction.Insert);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside InsertInvoice Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
