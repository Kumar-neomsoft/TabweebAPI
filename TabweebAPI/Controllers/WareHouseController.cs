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

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : CommonController
    {
        #region "Declarations"
        private readonly IWareHouseRepository _warehouseRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "WareHouse";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public WareHouseController(IConfiguration iconfig)
        {
            _warehouseRepository = new WareHouseRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpGet("GetWareHouseDetails")]
        public async Task<IActionResult> GetWareHouseDetails()
        {
            try
            {
                //Get the result from repository
                var Result = await _warehouseRepository.GetWareHouseDetails();

                return _commonController.ProcessGetResponse<WareHouse>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetWareHouseDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetWareHousebyBranchId")]
        public async Task<IActionResult> GetWareHousebyBranchId(int BranchNo)
        {
            try
            {
                //Get the result from repository
                var Result = await _warehouseRepository.GetWareHouseDetails(BranchNo);

                return _commonController.ProcessGetResponse<WareHouse>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetWareHouseDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
