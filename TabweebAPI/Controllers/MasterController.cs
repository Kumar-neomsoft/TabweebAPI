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
using TabweebAPI.DBHelper;

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : CommonController
    {
        #region "Declarations"
        private readonly IMasterRepository _masterRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "MasterData";

        #endregion

        #region "Constructor"
        public MasterController(IConfiguration iconfig)
        {
            _masterRepository = new MasterRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
        }
        #endregion
        [HttpGet("GetLangList")]
        public async Task<IActionResult> GetLangList()
        {
            try
            {
                //Get the result from repository
                var Result = await _masterRepository.GetLangList();

                return _commonController.ProcessGetResponse<Language>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("GetAccountingYear")]
        public async Task<IActionResult> GetAccountingYear()
        {
            try
            {
                //Get the result from repository
                var Result = await _masterRepository.GetAccountingYear();

                return _commonController.ProcessGetResponse<AccountingYear>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetCompany()
        {
            try
            {
                //Get the result from repository
                var Result = await _masterRepository.GetCompany();

                return _commonController.ProcessGetResponse<Company>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("GetBranchById")]
        public async Task<IActionResult> GetBranchById(Int32 CompanyId)
        {
            try
            {

                if (CompanyId == 0)
                {
                    return StatusCode(500, "CompanyId cannot be null");
                }
                var Result = await _masterRepository.GetBranchById(CompanyId);

                return _commonController.ProcessGetResponse<BranchRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetBranch")]
        public async Task<IActionResult> GetBranch()
        {
            try
            {
                var Result = await _masterRepository.GetBranch();

                return _commonController.ProcessGetResponse<BranchRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
