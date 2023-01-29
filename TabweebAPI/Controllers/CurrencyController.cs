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
using TabweebAPI.DBHelper;
using NLog;

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : CommonController
    {
        #region "Declarations"
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Branch";
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public CurrencyController(IConfiguration iconfig)
        {
            _currencyRepository = new CurrencyRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
        }
        #endregion
        [HttpGet("GetCurrencyDetails")]
        public async Task<IActionResult> GetCurrencyDetails()
        {
            try
            {
                //Get the result from repository
                var Result = await _currencyRepository.GetCurrencyDetails();

                return _commonController.ProcessGetResponse<Currency>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetCurrencyDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
