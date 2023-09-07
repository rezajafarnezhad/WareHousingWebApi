using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomerApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;
    public CustomerApiController(IUnitOfWork context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("GetCustomer")]
    public async Task<ApiResponse> Get([FromQuery] string userId)
    {
        var customerList = _context.customerUW.Get(c=>c.UserId == userId, "WareHouse");

        return new ApiResponse<IEnumerable<Customer>>()
        {
            flag = true,
            Data = customerList,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse> GetById([FromRoute] int id)
    {
        var _customer = await _context.customerUW.GetById(id);

        return _customer != null
            ? new ApiResponse<Customer>
            {
                flag = true,
                Data = _customer,
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


    [HttpPost]
    public async Task<ApiResponse> Craate([FromForm] CreateCustomerModel model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        //کنترل تکراری نبودن
        var customer = _context.customerUW.Get(c => c.CustomerFullName == model.CustomerFullName);
        if (customer.Count() > 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };

        try
        {
            var _customer = _mapper.Map(model, new Customer());
            await _context.customerUW.Create(_customer);
            await _context.SaveAsync();
            return new ApiResponse<Customer>()
            {
                flag = true,
                Data = _customer,
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


    [HttpPut]
    public async Task<ApiResponse> Edit([FromForm] EditCustomerModel model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        //تکراری نبودن
        var customers = _context.customerUW.Get(c => c.CustomerFullName == model.CustomerFullName && c.Id != model.Id);
        if (customers.Count() > 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };
        try
        {
            var _customer = await _context.customerUW.GetById(model.Id);

            if (_customer == null)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
                };

            var editcustomer = _mapper.Map(model, _customer);

            _context.customerUW.Update(editcustomer);
            await _context.SaveAsync();
            return new ApiResponse<Customer>()
            {
                flag = true,
                Data = editcustomer,
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

    [HttpGet("CustomerForDropDown/{wareHouseId}")]
    public async Task<ApiResponse> CustomerForDropDown([FromRoute]int wareHouseId)
    {
        var _data = await _context.customerUW
            .GetEn
            .Where(c=>c.WareHouseId == wareHouseId)
            .ToDictionaryAsync(c => c.Id, c => c.CustomerFullName);

        return new ApiResponse<Dictionary<int, string>>()
        {
            flag=true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message= ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }
}