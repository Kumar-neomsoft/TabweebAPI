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
    public class SalesRepController : CommonController
    {
        #region "Declarations"
        private readonly ISalesRepRepository _salesrepRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Branch";
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public SalesRepController(IConfiguration iconfig)
        {
            _salesrepRepository = new SalesRepRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
        }
        #endregion
        [HttpGet("GetSalesRep")]
        public async Task<IActionResult> GetSalesRep()
        {
            try
            {
                var Result = await _salesrepRepository.GetSalesRep();

                return _commonController.ProcessGetResponse<SalesRep>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBranch Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
