using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.PublicTools;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductPriceApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;
    private readonly IProductPriceRepo _productPriceRepo;
    public ProductPriceApiController(IUnitOfWork context, IMapper mapper, IProductPriceRepo productPriceRepo)
    {
        _context = context;
        _mapper = mapper;
        _productPriceRepo = productPriceRepo;
    }


    [HttpGet("GetList/{fiscalYearId}")]
    public async Task<IEnumerable<ProductsPrice>> Get([FromRoute] int fiscalYearId)
    {
        return await _productPriceRepo.GetProductsPrice(fiscalYearId);
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var productPrice = await _context.productPriceUW.GetById(Id);
        return productPrice == null ? NotFound() : Ok(productPrice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductPrice model)
    {
        if (!ModelState.IsValid)
            return BadRequest(model);

        var getProductPrice = await _context.productPriceUW.Get(
            c => c.ProductId == model.ProductId
                        && c.FiscalYearId == model.FiscalYearId
                        && c.ActionDate >= model.ActionDate.ConvertShamsiToMiladi());

        if (getProductPrice.Count() > 0)
            return StatusCode(550); //

        model.CreateDateTime = DateTime.Now;
        var _productPrice = _mapper.Map<ProductPrice>(model);

        await _context.productPriceUW.Create(_productPrice);
        await _context.SaveAsync();
        return Ok(_productPrice);

    }
}