using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CountriesApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    public CountriesApiController(IUnitOfWork context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<Country>>> Get()
    {
        var countryList =  _context.CountryUw.Get();

        return new ApiResponse<IEnumerable<Country>>()
        {
            flag = true,
            Data = countryList,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

    [HttpPost]
    public async Task<ApiResponse> Craate([FromForm] string countryName, [FromForm] string userId)
    {
        if (string.IsNullOrWhiteSpace(countryName))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        //کنترل تکراری نبودن
        var country =  _context.CountryUw.Get(c => c.CountryName == countryName);
        if (country.Count() > 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };

        try
        {
            var _country = new Country()
            {
                CountryName = countryName,
                UserId = userId,
                CreateDateTime = DateTime.Now.ToString()
            };
            await _context.CountryUw.Create(_country);
            await _context.SaveAsync();
            return new ApiResponse<Country>()
            {
                flag = true,
                Data = _country,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };
        }
        catch (Exception)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
            };
        }


    }

    [HttpGet("{id}")]
    public async Task<ApiResponse> GetById([FromRoute] int id)
    {
        var _country = await _context.CountryUw.GetById(id);

        return _country != null
            ? new ApiResponse<Country>
            {
                flag = true,
                Data = _country,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
            }
            : new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };
    }

    [HttpPut]
    public async Task<ApiResponse> Edit([FromForm] CountryEditModel model)
    {
        if (string.IsNullOrWhiteSpace(model.CountryName))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        //تکراری نبودن
        var countries = _context.CountryUw.Get(c => c.CountryName == model.CountryName && c.CountryId != model.CountryId);
        if (countries.Count() > 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };
        try
        {
            var _country = await _context.CountryUw.GetById(model.CountryId);

            if (_country == null)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
                };

            _country.CountryName = model.CountryName;
            _context.CountryUw.Update(_country);
            await _context.SaveAsync();
            return new ApiResponse<Country>()
            {
                flag = true,
                Data = _country,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };
        }
        catch (Exception)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
            };

        }


    }

    [HttpGet("CountriesListForDropDown")]
    public async Task<ApiResponse> CountriesListForDropDown()
    {
        var data = await _context.CountryUw.GetEn.ToDictionaryAsync(c => c.CountryId, c => c.CountryName);
        return new ApiResponse<Dictionary<int, string>>()
        {
            flag = true,
            Data = data,
            StatusCode= ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }


}