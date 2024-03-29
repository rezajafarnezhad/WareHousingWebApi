﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Common.PublicTools;

using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class FiscalYearApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IFiscalYearRepo _fiscalYearRepo;
    private readonly IMapper _mapper;
    private readonly IInventoryRepo _inventoryRepo;
    public FiscalYearApiController(IUnitOfWork context, IMapper mapper, IFiscalYearRepo fiscalYearRepo, IInventoryRepo inventoryRepo)
    {
        _context = context;
        _mapper = mapper;
        _fiscalYearRepo = fiscalYearRepo;
        _inventoryRepo = inventoryRepo;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<FiscalYear>>> Get()
    {
        var data = _context.fiscalYearUw.Get();
        return new ApiResponse<IEnumerable<FiscalYear>>()
        {
            flag = true,
            Data = data.OrderByDescending(c => c.Id),
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.ServerError.GetEnumDisplayName()
        };
    }

    [HttpGet("{Id}")]
    public async Task<ApiResponse> GetById([FromRoute] int Id)
    {
        var fiscalYear = await _context.fiscalYearUw.GetById(Id);
        if (fiscalYear is null)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };

        return new ApiResponse<FiscalYear>()
        {
            flag = true,
            Data = fiscalYear,
            StatusCode = ApiStatusCode.NotFound,
            Message = ApiStatusCode.NotFound.GetEnumDisplayName()
        };
    }

    [HttpPost]
    public async Task<ApiResponse> Create([FromForm] CreateFiscalYear model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        //کنترل تکراری نبودن
        var fiscalYear = _context.fiscalYearUw.Get();
        if (fiscalYear.Any(c => c.FiscalYearDescription == model.FiscalYearDescription))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName()
            };

        if (!await _fiscalYearRepo.CheckDatesForFiscalYear(model.StartDate.ConvertShamsiToMiladi(),
                model.EndDate.ConvertShamsiToMiladi()))
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DateTimeError,
                Message = ApiStatusCode.DateTimeError.GetEnumDisplayName()
            };
        }

        try
        {
            model.CreateDateTime = DateTime.Now.ToString();
            var _fiscalYear = _mapper.Map(model, new FiscalYear());
            await _context.fiscalYearUw.Create(_fiscalYear);
            await _context.SaveAsync();
            return new ApiResponse<FiscalYear>()
            {
                flag = true,
                Data = _fiscalYear,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
            };
        }
        catch (Exception e)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName()
            };
        }

    }




    [HttpPut]
    public async Task<ApiResponse> Update([FromForm] EditFiscalYear model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        var _fiscalYear = await _context.fiscalYearUw.GetById(model.Id);
        if (_fiscalYear == null)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };



        //کنترل تکراری نبودن
        var fiscalYear = _context.fiscalYearUw.Get();
        if (fiscalYear.Any(c => c.FiscalYearDescription == model.FiscalYearDescription && c.Id != model.Id))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName()
            };


        if (!await _fiscalYearRepo.IsExistDates(model.StartDate, model.EndDate))
        {
            if (!await _fiscalYearRepo.CheckDatesForFiscalYear(model.StartDate.ConvertShamsiToMiladi(), model.EndDate.ConvertShamsiToMiladi()))
            {
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.DateTimeError,
                    Message = ApiStatusCode.DateTimeError.GetEnumDisplayName()
                };
            }

        }

        try
        {
            var _fiscalYearToUpDate = _mapper.Map(model, _fiscalYear);
            _context.fiscalYearUw.Update(_fiscalYearToUpDate);
            await _context.SaveAsync();
            return new ApiResponse<FiscalYear>()
            {
                flag = true,
                Data = _fiscalYearToUpDate,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
            };
        }
        catch (Exception e)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName()
            };
        }
    }


    [HttpGet("GetFiscalYearForDropDown")]
    [AllowAnonymous]
    public async Task<ApiResponse> FiscalYearForDropDown()
    {
        var _fisclaYear = await _context.fiscalYearUw.GetEn.OrderByDescending(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.FiscalYearDescription);

        return new ApiResponse<Dictionary<int, string>>()
        {
            flag = true,
            Data = _fisclaYear,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()
        };
    }


    /// <summary>
    /// دریافت سال مالی کنونی و باز
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetCurrentFiscalYear")]
    public async Task<ApiResponse> GetCurrentFiscalYear()
    {
        var _data = await _context.fiscalYearUw.GetEn.Where(c => c.FiscalFlag).SingleAsync();
        return new ApiResponse<FiscalYear>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()
        };
    }

    /// <summary>
    /// دریافت سال مالی جدید
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetNewFiscalYear")]
    public async Task<ApiResponse> GetNewFiscalYear()
    {
        var LastFiscalYear = await _fiscalYearRepo.GetNextFiscalYear();
        return new ApiResponse<FiscalYear>()
        {
            flag = true,
            Data = LastFiscalYear,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()
        };
    }


    [HttpPost("TransferToNewFiscalYear")]
    public ApiResponse TransferToNewFiscalYear([FromBody] CloseFiscalYear model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName()
            };

        try
        {
            var result = _inventoryRepo.TransferToNewFiscalYear(model);
            if(result)
                return new ApiResponse()
                {
                    flag = true,
                    StatusCode = ApiStatusCode.Success,
                    Message = ApiStatusCode.Success.GetEnumDisplayName()
                };

            else
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName()
                };
        }
        catch (Exception)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName()
            };
        }


    }

}