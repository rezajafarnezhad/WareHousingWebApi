using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class InventoryApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IInventoryRepo _inventoryRepo;
    private readonly IMapper _mapper;

    public InventoryApiController(IUnitOfWork context, IMapper mapper, IInventoryRepo inventoryRepo)
    {
        _context = context;
        _mapper = mapper;
        _inventoryRepo = inventoryRepo;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<Inventory>>> Get()
    {
        var data = await _context.inventoryUw.Get();
        return new ApiResponse<IEnumerable<Inventory>>()
        {
            flag = true,
            Data = data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };

    }

    [HttpGet("GetProductStock")]
    public async Task<ApiResponse<IEnumerable<InventoryStockModel>>> GetProductStock([FromBody] InventoryQueryMaker model)
    {
        var _data = await _inventoryRepo.GetProductStock(model);
        return new ApiResponse<IEnumerable<InventoryStockModel>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }


    [HttpPost]
    public async Task<ApiResponse> AddProductStock([FromForm] AddProductStock model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        try
        {
            var _addProductStock = _mapper.Map(model, new Inventory());
            _addProductStock.CreateDateTime = DateTime.Now.ToString();
            _addProductStock.OperationType = 1; //ورود به انبار
            _addProductStock.ProductWastage = 0;
            _addProductStock.ReferenceId = 0;
            await _context.inventoryUw.Create(_addProductStock);
            await _context.SaveAsync();
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
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


    [HttpGet("GetExpireDatesProductForDropDown/{productid}/{wareHouseId}/{fiscalYearId}")]
    public async Task<ApiResponse> GetExpireDatesForDropDown([FromRoute] int productid, int wareHouseId, int fiscalYearId)
    {
        if (productid < 0 || wareHouseId < 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };

        var data =
            await _context.inventoryUw.GetEn
            .Where(c => c.ProductId == productid && c.WareHouseId == wareHouseId)
            .Where(c => c.FiscalYearId == fiscalYearId)
            .Where(c => c.OperationType == 1)
            .OrderByDescending(c => c.ExpireData)
            .Select(c => new DropDownDto()
            {
                DrId = c.Id,
                DrName = c.ExpireData.ConvertMiladiToShamsi("yyyy/MM/dd")

            }).ToListAsync();

        //حدف سری ساخت های بدون موجودی
        List<int> ZeroStock = new List<int>();

        for (int i = 0; i < data.Count(); i++)
        {
            if (await _inventoryRepo.GetPhysicalStockForBranch(data[i].DrId) == 0)
            {
                ZeroStock.Add(data[i].DrId);
            }
        }

        return new ApiResponse<List<DropDownDto>>()
        {
            flag = true,
            Data = data.Where(c => !ZeroStock.Contains(c.DrId)).ToList(),
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()
        };
    }
    
    //سری ساخت های کالا های ضایعاتی
    [HttpGet("GetExpireDatesProductForBWDropDown/{productid}/{wareHouseId}/{fiscalYearId}")]
    public async Task<ApiResponse> GetExpireDatesForBackWastageDropDown([FromRoute] int productid, int wareHouseId, int fiscalYearId)
    {
        if (productid < 0 || wareHouseId < 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };

        var data =
            await _context.inventoryUw.GetEnNoTraking
            .Where(c => c.ProductId == productid && c.WareHouseId == wareHouseId)
            .Where(c => c.FiscalYearId == fiscalYearId)
            .Where(c => c.OperationType == 3)
            .OrderByDescending(c => c.ExpireData)
            .Select(c => new DropDownDto()
            {
                DrId = c.Id,
                DrName = c.ExpireData.ConvertMiladiToShamsi("yyyy/MM/dd")

            }).ToListAsync();

        //حدف سری ساخت های بدون موجودی
        List<int> ZeroStock = new List<int>();

        for (int i = 0; i < data.Count(); i++)
        {
            if (await _inventoryRepo.GetPhysicalWastageStockForBranch(data[i].DrId) == 0)
            {
                ZeroStock.Add(data[i].DrId);
            }
        }

        return new ApiResponse<List<DropDownDto>>()
        {
            flag = true,
            Data = data.Where(c => !ZeroStock.Contains(c.DrId)).ToList(),
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName()
        };
    }

    [HttpPost("ExitProductStock")]
    public async Task<ApiResponse> ExitProductStock([FromForm] ExitStockModel model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        if (model.ProductCountMain > await _inventoryRepo.GetPhysicalStockForBranch(model.ReferenceId))
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ListEmpty,
                Message = "تعداد کالای وارد شده بیشتر از موجودی فیزیکی کالا جهت خروج میباشد"
            };
        }

        try
        {

            var _ExitProductStock = _mapper.Map(model, new Inventory());

            var getinfo = await _context.inventoryUw.GetEn.Where(c => c.Id == model.ReferenceId).SingleAsync();
            _ExitProductStock.CreateDateTime = DateTime.Now.ToString();
            _ExitProductStock.OperationType = 2; //خروج از انبار
            _ExitProductStock.ProductWastage = 0;
            _ExitProductStock.ProductLocationId = getinfo.ProductLocationId;
            _ExitProductStock.ExpireData = getinfo.ExpireData;
            await _context.inventoryUw.Create(_ExitProductStock);
            await _context.SaveAsync();
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
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

    [HttpGet("GetStockEachBranck/{id}")]
    public async Task<ApiResponse> GetStockEachBranch([FromRoute] int id)
    {
        if (id < 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };

        var results = await _inventoryRepo.GetPhysicalStockForBranch(id);

        var _LocationAddress =await _context.inventoryUw.GetEn.Include(c => c.ProductLocation).Where(c => c.Id == id)
            .Select(c => c.ProductLocation.ProductLocationAddress).SingleAsync();

        return new ApiResponse<int>()
        {
            flag = true,
            Data = results,
            StatusCode = ApiStatusCode.Success,
            Message = _LocationAddress
        };
    } 
    
    [HttpGet("GetWastageEachBranch/{id}")]
    public async Task<ApiResponse> GetWastageEachBranch([FromRoute] int id)
    {
        if (id < 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName()
            };

        var results = await _inventoryRepo.GetPhysicalWastageStockForBranch(id);

        var _LocationAddress =await _context.inventoryUw.GetEn.Include(c => c.ProductLocation).Where(c => c.Id == id)
            .Select(c => c.ProductLocation.ProductLocationAddress).SingleAsync();

        return new ApiResponse<int>()
        {
            flag = true,
            Data = results,
            StatusCode = ApiStatusCode.Success,
            Message = _LocationAddress
        };
    }

    [HttpPost("WastageProductStock")]
    public async Task<ApiResponse> WastageProductStock([FromForm] WastageStockModel model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        if (model.ProductWastage > await _inventoryRepo.GetPhysicalStockForBranch(model.ReferenceId))
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ListEmpty,
                Message = "تعداد کالای وارد شده بیشتر از موجودی فیزیکی کالا جهت ضایعات میباشد"
            };
        }

        try
        {
            var getParent = await _context.inventoryUw.GetEnNoTraking.Where(c=>c.Id == model.ReferenceId).SingleOrDefaultAsync();   
            var _WastageProductStock = _mapper.Map(model, new Inventory());
            _WastageProductStock.CreateDateTime = DateTime.Now.ToString();
            _WastageProductStock.OperationType = 3; //ورود به انبار ضایعات
            _WastageProductStock.ProductCountMain = 0;
            _WastageProductStock.ProductLocationId = getParent.ProductLocationId;
            _WastageProductStock.ExpireData = getParent.ExpireData;
           
            await _context.inventoryUw.Create(_WastageProductStock);
            await _context.SaveAsync();
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
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
    
    [HttpPost("BackWastageProductStock")]
    public async Task<ApiResponse> BackWastageProductStock([FromForm] BackWastageStockModel model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName()
            };

        if (model.ProductWastage > await _inventoryRepo.GetPhysicalWastageStockForBranch(model.ReferenceId))
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ListEmpty,
                Message = "تعداد کالای وارد شده بیشتر از موجودی ضایعات  میباشد"
            };
        }

        try
        {
            var getParent = await _context.inventoryUw.GetEnNoTraking.Where(c=>c.Id == model.ReferenceId).SingleOrDefaultAsync();   
           
            var _BackWastageProductStock = _mapper.Map(model, new Inventory());
            _BackWastageProductStock.CreateDateTime = DateTime.Now.ToString();
            _BackWastageProductStock.OperationType = 4; //برگشت از انبار ضایعات
            _BackWastageProductStock.ProductCountMain = 0;
            _BackWastageProductStock.ProductLocationId = getParent.ProductLocationId;
            _BackWastageProductStock.ExpireData = getParent.ExpireData;
           
            await _context.inventoryUw.Create(_BackWastageProductStock);
            await _context.SaveAsync();
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
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