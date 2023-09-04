using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models.Dto;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductsFlowApiController : ControllerBase
{

    private readonly IInventoryRepo _inventoryRepo;

    public ProductsFlowApiController(IInventoryRepo inventoryRepo)
    {
        _inventoryRepo = inventoryRepo;
    }


    [HttpGet]
    public async Task<ApiResponse> GetProductFlow([FromBody] ProductFlowDto model)
    {

        var _request = await _inventoryRepo.GetProductFlow(model);
        return new ApiResponse<IEnumerable<ProductFlowReplyDto>>()
        {
            flag = true,
            Data = _request,
            StatusCode=ApiStatusCode.Success,
            Message=ApiStatusCode.Success.GetEnumDisplayName(),

        };
    }

}