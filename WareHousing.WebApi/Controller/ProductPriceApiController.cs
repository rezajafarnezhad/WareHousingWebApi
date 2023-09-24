using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using WareHousingWebApi.Common.PublicTools;

using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Data.Services.Repository;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;


namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]

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


    [HttpGet("GetList")]
    public async Task<ApiResponse> Get()
    {
        var _data = await _productPriceRepo.GetProductsPrice();

        return _data != null
            ? new ApiResponse<IEnumerable<ProductsPriceInput>>()
            {
                flag = true,
                Data = _data,
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


    [HttpGet("{Id}")]
    public async Task<ApiResponse> GetById([FromRoute] int Id)
    {
        var _data = await _context.productPriceUW.GetById(Id);

        return _data != null
            ? new ApiResponse<ProductPrice>()
            {
                flag = true,
                Data = _data,
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
    public async Task<ApiResponse> Create([FromForm] CreateProductPrice model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        var getProductPrice =  _context.productPriceUW.Get(
            c => c.ProductId == model.ProductId
                 && c.ActionDate >= model.ActionDate.ConvertShamsiToMiladi());

        if (getProductPrice.Count() > 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };

        model.CreateDateTime = DateTime.Now;
        var _productPrice = _mapper.Map<ProductPrice>(model);

        await _context.productPriceUW.Create(_productPrice);
        await _context.SaveAsync();
        return new ApiResponse<ProductPrice>()
        {
            flag = true,
            Data = _productPrice,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };

    }

    [HttpGet("GetProductPriceHistory/{productId}")]
    public async Task<ApiResponse> GetProductPriceHistory([FromRoute] int productId)
    {
        var _data =  _context.productPriceUW.Get(c => c.ProductId == productId);

        if (_data is null)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };
        }
        else
        {
            return new ApiResponse<IEnumerable<WareHousingWebApi.Entities.Entities.ProductPrice>>()
            {
                flag = true,
                Data = _data,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };
        }
    }
   

    [HttpDelete("{Id}")]
    public async Task<ApiResponse> DeletePrice([FromRoute] int Id)
    {
        if (Id == 0)
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        var _productPrice = await _context.productPriceUW.GetById(Id);
        if (_productPrice.ActionDate > DateTime.Now.Date)
        {
            _context.productPriceUW.DeleteById(Id);
            await _context.SaveAsync();
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };
        }
        else
        {
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.DateTimeError,
                Message = ApiStatusCode.DateTimeError.GetEnumDisplayName(),
            };

        }

    }
}