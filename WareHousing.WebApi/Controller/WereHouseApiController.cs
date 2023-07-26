using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class WereHouseApiController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WereHouseApiController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<WareHouse>> GetAll()
    {
      return await _unitOfWork.wareHouse.Get();
    }

    [HttpGet("{Id}")]

    public async Task<IActionResult> GetById([FromRoute]int Id)
    {
        var WareHouse = await _unitOfWork.wareHouse.GetById(Id);
        if(WareHouse == null) { return NotFound();}
        return Ok(WareHouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateWareHouse model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var wareHouses = await _unitOfWork.wareHouse.Get();
        if(wareHouses.Any(c=>c.Name == model.Name))
            return StatusCode(550);

        try
        {
            model.CreateDateTime = DateTime.Now.ToString();
            var _wareHouseForAdd = _mapper.Map(model,new WareHouse());
            await _unitOfWork.wareHouse.Create(_wareHouseForAdd);
            await _unitOfWork.SaveAsync();
            return Ok(_wareHouseForAdd);

        }
        catch (Exception)
        {
            return StatusCode(500);

        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm]EditWareHouse model)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        var _wareHouse = await _unitOfWork.wareHouse.GetById(model.Id);
        if(_wareHouse is null) { return BadRequest(); }

        var _wareHouses = await _unitOfWork.wareHouse.Get();
        if(_wareHouses.Any(c=>c.Name == model.Name && c.Id != model.Id))
            return StatusCode(550);

        try
        {
            var _wareHouseForEdit = _mapper.Map(model, _wareHouse);
            _unitOfWork.wareHouse.Update(_wareHouseForEdit);
            await _unitOfWork.SaveAsync();
            return Ok(_wareHouseForEdit);
        }
        catch (Exception e)
        {
            return StatusCode(500);

        }
    }

    [HttpGet("GetWareHouseForDropDown")]
    public async Task<IActionResult> GetWareHouseForDropDown()
    {

        var data = await _unitOfWork.wareHouse.GetEn.ToDictionaryAsync(c => c.Id, c => c.Name);
        return Ok(JsonConvert.SerializeObject(data));

    }
}