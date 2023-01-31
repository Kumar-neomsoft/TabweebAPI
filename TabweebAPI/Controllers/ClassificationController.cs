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
    public class ClassificationController : CommonController
    {
        #region "Declarations"
        private readonly IClassificationRepository _classificationRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Classification";
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public ClassificationController(IConfiguration iconfig)
        {
            _classificationRepository = new ClassificationRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
        }
        #endregion
        [HttpGet("GetARType")]
        public async Task<IActionResult> GetARType(Int32 ArTypeNo)
        {
            try
            {

                if (ArTypeNo == 0)
                {
                    return StatusCode(500, "ArTypeNo cannot be null");
                }
                var Result = await _classificationRepository.GetARType(ArTypeNo);

                return _commonController.ProcessGetResponse<ARType>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetARType Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
