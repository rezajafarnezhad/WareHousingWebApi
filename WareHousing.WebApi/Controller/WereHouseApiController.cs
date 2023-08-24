using AutoMapper;
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
    public async Task<ApiResponse<IEnumerable<WareHouse>>> GetAll()
    {
      var data = await _unitOfWork.wareHouseUw.Get();
      return new ApiResponse<IEnumerable<WareHouse>>()
      {
          flag = true,
          Data = data,
          StatusCode = ApiStatusCode.Success,
          Message = ApiStatusCode.Success.GetEnumDisplayName(),

      };
    }

    [HttpGet("{Id}")]

    public async Task<ApiResponse> GetById([FromRoute]int Id)
    {
        var WareHouse = await _unitOfWork.wareHouseUw.GetById(Id);
        return WareHouse != null
            ? new ApiResponse<WareHouse>
            {
                flag = true,
                Data = WareHouse,
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
    public async Task<ApiResponse> Create([FromForm] CreateWareHouse model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

            };

        var wareHouses = await _unitOfWork.wareHouseUw.Get();
        if(wareHouses.Any(c=>c.Name == model.Name))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),

            };

        try
        {
            model.CreateDateTime = DateTime.Now.ToString();
            var _wareHouseForAdd = _mapper.Map(model,new WareHouse());
            await _unitOfWork.wareHouseUw.Create(_wareHouseForAdd);
            await _unitOfWork.SaveAsync();
            return new ApiResponse<WareHouse>()
            {
                flag = true,
                Data = _wareHouseForAdd,
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
    public async Task<ApiResponse> Update([FromForm]EditWareHouse model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

            };

        var _wareHouse = await _unitOfWork.wareHouseUw.GetById(model.Id);
        if(_wareHouse is null) return new ApiResponse()
        {
            flag = false,
            StatusCode = ApiStatusCode.NotFound,
            Message = ApiStatusCode.NotFound.GetEnumDisplayName(),

        };

        var _wareHouses = await _unitOfWork.wareHouseUw.Get();
        if(_wareHouses.Any(c=>c.Name == model.Name && c.Id != model.Id))
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.DuplicateInformation,
                Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),

            };

        try
        {
            var _wareHouseForEdit = _mapper.Map(model, _wareHouse);
            _unitOfWork.wareHouseUw.Update(_wareHouseForEdit);
            await _unitOfWork.SaveAsync();
            return new ApiResponse<WareHouse>()
            {
                flag = true,
                Data=_wareHouseForEdit,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),

            };
        }
        catch (Exception e)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName(),

            };
        }
    }

    [HttpGet("GetWareHouseForDropDown")]
    public async Task<ApiResponse> GetWareHouseForDropDown()
    {

        var data = await _unitOfWork.wareHouseUw.GetEn.ToDictionaryAsync(c => c.Id, c => c.Name);
        return new ApiResponse<Dictionary<int,string>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),

        };
    }

}