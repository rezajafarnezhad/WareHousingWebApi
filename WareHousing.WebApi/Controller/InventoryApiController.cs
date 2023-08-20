using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;

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
    public async Task<IEnumerable<Inventory>> Get()
    {
        return await _context.inventory.Get();
    }

    [HttpGet("GetProductStock")]
    public async Task<IEnumerable<InventoryStockModel>> GetProductStock([FromBody]InventoryQueryMaker model)
    {
        var _data = await _inventoryRepo.GetProductStock(model);
        return _data;
    }

}