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
using Tabweeb_Model.Sales;

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
        [HttpGet("GetCurrencyDetails")]
        public async Task<IActionResult> GetCurrencyDetails()
        {
            try
            {
                //Get the result from repository
                var Result = await _salesRepository.GetCurrencyDetails();

                return _commonController.ProcessGetResponse<Currency>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetLangList Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetWareHouseDetails")]
        public async Task<IActionResult> GetWareHouseDetails()
        {
            try
            {
                //Get the result from repository
                var Result = await _salesRepository.GetWareHouseDetails();

                return _commonController.ProcessGetResponse<WareHouse>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetWareHouseDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetBillType")]
        public async Task<IActionResult> GetBillType(int LangNo, int DocType)
        {
            try
            {
                //Get the result from repository
                var Result = await _salesRepository.GetBillType(LangNo, DocType);

                return _commonController.ProcessGetResponse<BillType>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetWareHouseDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
