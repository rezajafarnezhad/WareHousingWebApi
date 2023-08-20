using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Common.PublicTools;

using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FiscalYearApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IFiscalYearRepo _fiscalYearRepo;
    private readonly IMapper _mapper;
    public FiscalYearApiController(IUnitOfWork context, IMapper mapper, IFiscalYearRepo fiscalYearRepo)
    {
        _context = context;
        _mapper = mapper;
        _fiscalYearRepo = fiscalYearRepo;
    }

    [HttpGet]
    public async Task<IEnumerable<FiscalYear>> Get()
    {
        return await _context.fiscalYear.Get();
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var fiscalYear = await _context.fiscalYear.GetById(Id);
        if (fiscalYear is null)
            return NotFound();

        return Ok(fiscalYear);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateFiscalYear model)
    {
        if (!ModelState.IsValid) return BadRequest(model);

        //کنترل تکراری نبودن
        var fiscalYear = await _context.fiscalYear.Get();
        if (fiscalYear.Any(c => c.FiscalYearDescription == model.FiscalYearDescription))
            return StatusCode(550);

        if (!await _fiscalYearRepo.CheckDatesForFiscalYear(model.StartDate.ConvertShamsiToMiladi(),
                model.EndDate.ConvertShamsiToMiladi()))
        {
            return StatusCode(610);
        }

        try
        {
            model.CreateDateTime = DateTime.Now.ToString();
            var _fiscalYear = _mapper.Map(model, new FiscalYear());
            await _context.fiscalYear.Create(_fiscalYear);
            await _context.SaveAsync();
            return Ok(_fiscalYear);
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }

    }




    [HttpPut]
    public async Task<IActionResult> Update([FromForm] EditFiscalYear model)
    {
        if (!ModelState.IsValid) return BadRequest(model);

        var _fiscalYear = await _context.fiscalYear.GetById(model.Id);
        if (_fiscalYear == null)
            return BadRequest(model);


        //کنترل تکراری نبودن
        var fiscalYear = await _context.fiscalYear.Get();
        if (fiscalYear.Any(c => c.FiscalYearDescription == model.FiscalYearDescription && c.Id != model.Id))
            return StatusCode(550);


        if (!await _fiscalYearRepo.IsExistDates(model.StartDate, model.EndDate))
        {
            if (!await _fiscalYearRepo.CheckDatesForFiscalYear(model.StartDate.ConvertShamsiToMiladi(), model.EndDate.ConvertShamsiToMiladi()))
            {
                return StatusCode(610);
            }

        }

        try
        {
            var _fiscalYearToUpDate = _mapper.Map(model, _fiscalYear);
            _context.fiscalYear.Update(_fiscalYearToUpDate);
            await _context.SaveAsync();
            return Ok(_fiscalYearToUpDate);
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }


    [HttpGet("GetFiscalYearForDropDown")]
    public async Task<IActionResult> FiscalYearForDropDown()
    {
        var _fisclaYear = await _context.fiscalYear.GetEn.ToDictionaryAsync(c => c.Id, c => c.FiscalYearDescription);
        return Ok(JsonConvert.SerializeObject(_fisclaYear));
    }
}