using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;
using Tabweeb_Model;
using TabweebAPI.Common;
using Microsoft.AspNetCore.Mvc;
namespace TabweebAPI.IRepository
{
    public interface IProductRepository
    {
        Task<MethodResult<List<ProductSearch>>> SearchProduct(ProductSearchReq obj);
        Task<MethodResult<List<ProductGetRes>>> GetProductCode(ProductGetReq obj);
        Task<MethodResult<List<ProductSearch>>> GetAllProduct();
        Task<MethodResult<List<BarcodeGetRes>>> GetBarCode(BarcodeGetReq obj);
    }
}
