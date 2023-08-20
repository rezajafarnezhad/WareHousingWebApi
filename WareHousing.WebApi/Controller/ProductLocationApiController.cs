using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductLocationApiController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductLocationApiController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductLocation model)
    {
        if (model.ProductLocationAddress == "") return BadRequest(ModelState);

        var _location =
            await _unitOfWork.productLocationUW.Get();

        if (_location.Any(c => c.ProductLocationAddress == model.ProductLocationAddress))
            return StatusCode(550);

        try
        {
            var _createLocation = _mapper.Map(model, new ProductLocation());
            _createLocation.CreateDateTime = DateTime.Now;
            await _unitOfWork.productLocationUW.Create(_createLocation);
            await _unitOfWork.SaveAsync();
            return Ok(200);
        }
        catch (Exception)
        {
            return Ok(500);

        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute]int Id)
    {
        var _Location = await _unitOfWork.productLocationUW.GetById(Id);
        return _Location == null ? NotFound() : Ok(_Location);

    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] EditProductLocation model)
    {

        if (model.Id == 0) return BadRequest(ModelState);

        if (!ModelState.IsValid) { return BadRequest(); }

        try
        {
            var _location = await _unitOfWork.productLocationUW.GetById(model.Id);
            //تکراری نبودن
            var countries = await _unitOfWork.productLocationUW.Get(c => c.ProductLocationAddress == model.ProductLocationAddress && c.Id != model.Id);
            if (countries.Count() > 0)
                return StatusCode(550);

            if (_location != null)
            {
                var eLocation = _mapper.Map(model,_location);
                _unitOfWork.productLocationUW.Update(eLocation);
                await _unitOfWork.SaveAsync();
                return Ok(200);
            }
        }
        catch (Exception )
        {
            return StatusCode(500);

        }
        return StatusCode(500);

    }

    [HttpGet("GetProductLocationByWareHouseId")]
    public async Task<IEnumerable<ProductLocation>> GetListProductLocationByWareHouseId([FromBody] int Id) // WareHouseId
    {
        return await _unitOfWork.productLocationUW.Get(c => c.WareHouseId == Id);
    }

}