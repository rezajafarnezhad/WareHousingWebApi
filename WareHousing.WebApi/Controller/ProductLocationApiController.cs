using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

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
    public async Task<ApiResponse> Create([FromForm] CreateProductLocation model)
    {
        if (model.ProductLocationAddress == "")
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        var _location =
            await _unitOfWork.productLocationUW.Get();

        if (_location.Any(c => c.ProductLocationAddress == model.ProductLocationAddress))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
            };

        try
        {
            var _createLocation = _mapper.Map(model, new ProductLocation());
            _createLocation.CreateDateTime = DateTime.Now.ToString();
            await _unitOfWork.productLocationUW.Create(_createLocation);
            await _unitOfWork.SaveAsync();
            return new ApiResponse<ProductLocation>()
            {
                flag = true,
                Data = _createLocation,
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

        var _productLocation = await _unitOfWork.productLocationUW.GetById(id);

        return _productLocation != null
            ? new ApiResponse<ProductLocation>
            {
                flag = true,
                Data = _productLocation,
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
    public async Task<ApiResponse> Edit([FromForm] EditProductLocation model)
    {

        if (model.Id == 0) 
            return new ApiResponse()
        {
            flag = false,
            StatusCode = ApiStatusCode.NotFound,
            Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
        };

        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),
            };

        try
        {
            var _location = await _unitOfWork.productLocationUW.GetById(model.Id);
            //تکراری نبودن
            var countries = await _unitOfWork.productLocationUW.Get(c => c.ProductLocationAddress == model.ProductLocationAddress && c.Id != model.Id);
            if (countries.Count() > 0)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.DuplicateInformation,
                    Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),
                };


            if (_location != null)
            {
                var eLocation = _mapper.Map(model, _location);
                _unitOfWork.productLocationUW.Update(eLocation);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<ProductLocation>()
                {
                    flag = true,
                    Data = eLocation,
                    StatusCode = ApiStatusCode.Success,
                    Message = ApiStatusCode.Success.GetEnumDisplayName(),
                };
            }
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
        return new ApiResponse()
        {
            flag = false,
            StatusCode = ApiStatusCode.ServerError,
            Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
        };

    }

    [HttpGet("GetProductLocationByWareHouseId")]
    public async Task<ApiResponse> GetListProductLocationByWareHouseId([FromBody] int Id) // WareHouseId
    {
        var data = await _unitOfWork.productLocationUW.Get(c => c.WareHouseId == Id);
        return new ApiResponse<IEnumerable<ProductLocation>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

    [HttpGet("GetProductLocationDropDown/{warehouseId}")]
    public async Task<ApiResponse> GetProductLocationDropDown([FromRoute] int warehouseId)
    {
        var data = await _unitOfWork.productLocationUW.GetEnNoTraking
            .Where(c=>c.WareHouseId == warehouseId)
            .ToDictionaryAsync(c => c.Id, c => c.ProductLocationAddress);
        return new ApiResponse<Dictionary<int,string>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

}