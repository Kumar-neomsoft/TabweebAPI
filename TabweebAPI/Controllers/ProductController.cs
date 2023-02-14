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
    public class ProductController : CommonController
    {
        #region "Declarations"
        private readonly IProductRepository _productRepository;
        private readonly CommonRepository _commonRepository;
        private readonly CommonController _commonController;
        private readonly string PageName = "Product";
        private readonly JwtMiddleware _jwtmiddleware;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region "Constructor"
        public ProductController(IConfiguration iconfig)
        {
            _productRepository = new ProductRepository(iconfig);
            _commonController = new CommonController();
            _commonRepository = new CommonRepository();
            _jwtmiddleware = new JwtMiddleware(iconfig);
        }
        #endregion

        [HttpPost("SearchProduct")]
        public async Task<IActionResult> SearchProduct([FromForm] ProductSearchReq obj)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (obj == null)
                {
                    return StatusCode(500, "ProductSearch cannot be null");
                }
                var Result = await _productRepository.SearchProduct(obj);

                return _commonController.ProcessGetResponse<ProductSearch>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside SearchProduct Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpPost("GetProductCode")]
        public async Task<IActionResult> GetProductCode([FromForm] ProductGetReq obj)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (obj == null)
                {
                    return StatusCode(500, "ProductGetReq cannot be null");
                }
                var Result = await _productRepository.GetProductCode(obj);

                return _commonController.ProcessGetResponse<ProductGetRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetProductCode Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpPost("GetBarCode")]
        public async Task<IActionResult> GetBarCode([FromBody] BarcodeGetReq obj)
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }
                if (obj == null)
                {
                    return StatusCode(500, "BarcodeGetReq cannot be null");
                }
                var Result = await _productRepository.GetBarCode(obj);

                return _commonController.ProcessGetResponse<BarcodeGetRes>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetBarCode Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                //Validate JWT token validation
                var returnValue = _jwtmiddleware.ValidateJWTToken(HttpContext.Request.Headers.ToList());

                if (returnValue.Equals("unauthorized"))
                {
                    return StatusCode(401);
                }

                var Result = await _productRepository.GetAllProduct();

                return _commonController.ProcessGetResponse<ProductSearch>(Result.ResultObject.ToList(), PageName, CRUDAction.Select);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured inside GetAllProduct Action: {ex.Message}");
                return StatusCode(500, "Internal server error");

            }
        }
    }
}
