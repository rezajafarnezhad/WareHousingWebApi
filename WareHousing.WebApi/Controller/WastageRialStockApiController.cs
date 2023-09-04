using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models.Dto;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class WastageRialStockApiController : ControllerBase
{

    private readonly IWastageRialStockRepo _wastageRialStockRepo;
    public WastageRialStockApiController(IWastageRialStockRepo wastageRialStockRepo)
    {
        _wastageRialStockRepo = wastageRialStockRepo;
    }

    [HttpGet]
    public async Task<ApiResponse> GetWastageRialStock([FromBody] RialStockInput input)
    {
        var data =  await _wastageRialStockRepo.GetWastageRialStock(input.FiscalYearId, input.WareHouseId);
        return new ApiResponse<IEnumerable<WastageRialStockDto>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()

        };
    }
}