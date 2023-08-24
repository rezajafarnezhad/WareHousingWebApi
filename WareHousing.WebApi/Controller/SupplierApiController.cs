using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Data.Services.Repository;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public SupplierApiController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ApiResponse> Get()
        {
            var _data =  await _context.SupplierUw.Get();
            return new ApiResponse<IEnumerable<Supplier>>()
            {
                flag = true,
                Data = _data,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),

            };
        }

        [HttpPost]
        public async Task<ApiResponse> Craate(SupplierModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.BadRequest,
                    Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

                };

            //کنترل تکراری نبودن
            var supplier = await _context.SupplierUw.Get(c => c.SupplierName == model.SupplierName || c.SupplierTel == model.SupplierTel);
            if (supplier.Count() > 0)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.DuplicateInformation,
                    Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),

                };

            try
            {
                var _supplier = new Supplier()
                {
                   SupplierName = model.SupplierName,
                   SupplierTel = model.SupplierTel,
                   SupplierDescription = model.SupplierDescription,
                   SupplierSite = model.SupplierSite,
                };
                await _context.SupplierUw.Create(_supplier);
                await _context.SaveAsync();
                return new ApiResponse<Supplier>()
                {
                    flag = true,
                    Data = _supplier,
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

        [HttpGet("{Id}")]
        public async Task<ApiResponse> GetbyId(int Id)
        {
            var _supplier = await _context.SupplierUw.GetById(Id);

            return _supplier != null
                ? new ApiResponse<Supplier>
                {
                    flag = true,
                    Data = _supplier,
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
        public async Task<ApiResponse> Edit(SupplierEditModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.BadRequest,
                    Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

                };

            //کنترل تکراری نبودن
            var supplier = await _context.SupplierUw.Get(c => c.SupplierName == model.SupplierName  && c.SupplierId != model.SupplierId);
          

            if (supplier.Count() > 0)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.DuplicateInformation,
                    Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),

                };

            try
            {
                var _supplier = await _context.SupplierUw.GetById(model.SupplierId);
                if (_supplier == null) return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),

                };

                _supplier.SupplierName = model.SupplierName.Trim();
                _supplier.SupplierTel = model.SupplierTel.Trim();
                _supplier.SupplierDescription = model.SupplierDescription;
                _supplier.SupplierSite = model.SupplierSite;
                
                _context.SupplierUw.Update(_supplier);
                await _context.SaveAsync();
                return new ApiResponse<Supplier>()
                {
                    flag = true,
                    Data = _supplier,
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

        [HttpGet("GetSupplierForDropDown")]
        public async Task<ApiResponse> GetSupplierForDropDown() 
        {

            var data = await _context.SupplierUw.GetEn.ToDictionaryAsync(c => c.SupplierId , c=>c.SupplierName);
            return new ApiResponse<Dictionary<int,string>>()
            {
                flag = true,
                Data = data,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),

            };
        }

    }

}
