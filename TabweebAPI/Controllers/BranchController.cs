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
using TabweebAPI.Middleware;

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
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public BranchController(IConfiguration iconfig)
        {
            _branchRepository = new BranchRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion
        [HttpGet("GetBranchById")]
        public async Task<IActionResult> GetBranchById(Int32 CompanyId)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }

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
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
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
