using Tabweeb_Model;
using TabweebAPI.Common;

namespace TabweebAPI.IRepository
{
    interface IWareHouseRepository
    {
        Task<MethodResult<List<WareHouse>>> GetWareHouseDetails();
        Task<MethodResult<List<WareHouse>>> GetWareHouseDetails(int BranchId);
    }
}
