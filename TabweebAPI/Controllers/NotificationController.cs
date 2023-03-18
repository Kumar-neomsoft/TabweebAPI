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
using Microsoft.Extensions.Hosting.Internal;

namespace TabweebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        #region "Declarations"
        private readonly INotificationRepository _NotificationRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Notification";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region "Constructor"
        public NotificationController(IConfiguration iconfig, IWebHostEnvironment webHostEnvironment)
        {
            _NotificationRepository = new NotificationRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion
        [HttpPost("SendQuotation")]
        public async Task<IActionResult> SendQuotationDetails([FromBody] QuotationMailSend obj)
        {
            try
            {
                ////Validate JWT token validation
                //var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                //if (returnValue.Equals("unauthorized"))
                //{
                //    return StatusCode(401);
                //}
                if (obj == null)
                    return BadRequest("SendQuotationDetails request cannot be null");
                string AppPath = _webHostEnvironment.ContentRootPath;

                var Result = await _NotificationRepository.SendNotification(obj, AppPath);
                return _commonController.ProcessResponse<saveStatus>(Result.ResultObject, PageName, CRUDAction.Notification);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside SendQuotationDetails Action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
