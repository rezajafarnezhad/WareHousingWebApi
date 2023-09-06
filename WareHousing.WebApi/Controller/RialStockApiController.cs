using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models.Dto;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class RialStockApiController : ControllerBase
{
    private readonly IRialStockRepo _rialstockRepo;

    public RialStockApiController(IRialStockRepo rialstockRepo)
    {
        _rialstockRepo = rialstockRepo;
    }

    [HttpGet]
    public async Task<ApiResponse> GetRialStock([FromBody]RialStockInput input)
    {

        var data = await _rialstockRepo.GetRialStock(input.FiscalYearId,input.WareHouseId);

        return new ApiResponse<IEnumerable<RialStockDto>>()
        {
            flag = true,
            Data = data,
            StatusCode=ApiStatusCode.Success,
            Message= ApiStatusCode.Success.GetEnumDisplayName()

        };
    }


}