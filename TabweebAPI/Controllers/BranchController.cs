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
    public class BranchController : CommonController
    {
        #region "Declarations"
        private readonly IBranchRepository _branchRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Branch";
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public BranchController(IConfiguration iconfig)
        {
            _branchRepository = new BranchRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
        }
        #endregion
        [HttpGet("GetBranchById")]
        public async Task<IActionResult> GetBranchById(Int32 CompanyId)
        {
            try
            {

                if (CompanyId == 0)
                {
                    return StatusCode(500, "CompanyId cannot be null");
                }
                var Result = await _branchRepository.GetBranchById(CompanyId);

                return _commonController.ProcessGetResponse<BranchRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBranchById Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetBranch")]
        public async Task<IActionResult> GetBranch()
        {
            try
            {
                var Result = await _branchRepository.GetBranch();

                return _commonController.ProcessGetResponse<BranchRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBranch Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
