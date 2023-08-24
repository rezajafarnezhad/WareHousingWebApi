using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class InventoryApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IInventoryRepo _inventoryRepo;
    private readonly IMapper _mapper;

    public InventoryApiController(IUnitOfWork context, IMapper mapper, IInventoryRepo inventoryRepo)
    {
        _context = context;
        _mapper = mapper;
        _inventoryRepo = inventoryRepo;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<Inventory>>> Get()
    {
        var data = await _context.inventoryUw.Get();
        return new ApiResponse<IEnumerable<Inventory>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };

    }

    [HttpGet("GetProductStock")]
    public async Task<ApiResponse<IEnumerable<InventoryStockModel>>> GetProductStock([FromBody]InventoryQueryMaker model)
    {
        var _data = await _inventoryRepo.GetProductStock(model);
        return new ApiResponse<IEnumerable<InventoryStockModel>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

}