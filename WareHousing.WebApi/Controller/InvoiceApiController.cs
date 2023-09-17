using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Abstractions;
using System.Transactions;
using System.Xml.Linq;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Migrations;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Entities.Models.Dto;
using WareHousingWebApi.WebFramework.ApiResult;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InvoiceApiController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;
    private readonly IInvoiceRepo _invoiceRepo;
    private readonly IInventoryRepo _inventoryRepo;
    public InvoiceApiController(IUnitOfWork context, IMapper mapper, IInvoiceRepo invoiceRepo, IInventoryRepo inventoryRepo)
    {
        _context = context;
        _mapper = mapper;
        _invoiceRepo = invoiceRepo;
        _inventoryRepo = inventoryRepo;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<ApiResponse> GetProductItemInfo([FromQuery] int productCode, int wareHouseId, int fiscalYearId)
    {
        try
        {
            var _data = await _invoiceRepo.GetProductItemInfo(productCode, wareHouseId, fiscalYearId);
            return new ApiResponse<ProductItemsDto>()
            {
                flag = true,
                Data = _data,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName()
            };
        }
        catch (Exception)
        {
            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.BadRequest,
                Message = "کالایی یافت نشد!"
            };
        }
    }


    [HttpPost]
    public async Task<ApiResponse> CreateInvoice([FromBody] CreateInvoice model)
    {
        if (!ModelState.IsValid)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ListEmpty,
                Message = ApiStatusCode.ListEmpty.GetEnumDisplayName(),
            };
        }


        using (var transaction = _context.BeginTransaction())
        {
            try
            {
                //create Invoice

                var _invoice = _mapper.Map(model, new Invoice());
                _invoice.InvoiceTotalPrice = await _invoiceRepo.CalculateInvoicePrice(model.productIds.ToArray(), model.productcount.ToArray(), model.fiscalYearId);


                await _context.invoiceUW.Create(_invoice);
                _context.Save();



                //Insert InvoiceItems
                for (int i = 0; i < model.productcount.Count(); i++)
                {
                    //چک کردن موجودی
                    if (model.productcount[i] >
                        await _inventoryRepo.GetProductStock(model.productIds[i], model.fiscalYearId,
                            model.wareHouseId))
                    {
                        return new ApiResponse()
                        {
                            flag = false,
                            StatusCode = ApiStatusCode.UserMistake,
                            Message = _context.productsUw.GetById(model.productIds[i]).Result.ProductName,
                        };
                    }

                    var _productPrice = _invoiceRepo.GetPrices(model.productIds[i], model.fiscalYearId);

                    var _InvoiceItem = new InvoiceItems()
                    {
                        UserId = model.userId,
                        CreateDateTime = DateTime.Now.ToString(),
                        InvoiceId = _invoice.Id,
                        ProductId = model.productIds[i],
                        Count = model.productcount[i],
                        CoverPrice = _productPrice.CoverPrice,
                        PurchasePrice = _productPrice.PurchasePrice,
                        SalesPrice = _productPrice.SalesPrice,
                    };

                    await _context.invoiceItemsUW.Create(_InvoiceItem);
                    await GetProductFromBranch(model.productIds[i], model.productcount[i], model.wareHouseId, model.fiscalYearId
                     , _invoice.Id, model.userId);
                }

                await _context.SaveAsync();
                transaction.Commit();
                return new ApiResponse()
                {
                    flag = true,
                    StatusCode = ApiStatusCode.Success,
                    Message = ApiStatusCode.Success.GetEnumDisplayName(),
                };

            }
            catch (Exception e)
            {
                transaction.RollBack();
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
                };
            }
        };
    }


    [HttpGet("ListInvoice")]
    public async Task<ApiResponse> InvoiceForList([FromBody] InvoiceListDto model)
    {
        if (!ModelState.IsValid)
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };
        }
        var _data = await _invoiceRepo.GetInvoiceList(model);
        return new ApiResponse<List<InvoiceList>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

    [HttpGet("GetInvoiceItems")]
    public async Task<ApiResponse> InvoiceItemList([FromQuery] int id)
    {
        if (id < 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };


        var _data = await _invoiceRepo.GetInvoiceList(id);
        return new ApiResponse<List<InvoiceItemList>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };


    }

    [HttpPost("returnInvoice")]
    public async Task<ApiResponse> ReturnInvoice([FromBody] ReturnInvoice model)
    {
        if (!ModelState.IsValid)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };

        try
        {
            var _Invoice = await _context.invoiceUW.GetById(model.invoiceId);
            _Invoice.InvoiceType = 2; //مرجوعی
            _Invoice.ReturnDate = DateTime.Now;
            _context.invoiceUW.Update(_Invoice);

            //2 inventory operation
            var _inventory = await _context.inventoryUw.GetEn.Where(c => c.InvoiceId == model.invoiceId).ToListAsync();
            foreach (var inventory in _inventory)
            {
                var inventoryAdded = new Inventory()
                {
                    InvoiceId = model.invoiceId,
                    CreateDateTime = DateTime.Now.ToString(),
                    OperationDate = DateTime.Now,
                    OperationType = 6, //مرجوعی
                    Description = "مرجوعی",
                    FiscalYearId = model.fiscalYearId,
                    UserId = model.userid,
                    WareHouseId = inventory.WareHouseId,
                    ExpireData = inventory.ExpireData,
                    ProductLocationId = inventory.ProductLocationId,
                    ProductCountMain = inventory.ProductCountMain,
                    ProductWastage = 0,
                    ProductId = inventory.ProductId,
                    ReferenceId = inventory.ReferenceId,
                    
                };
                await _context.inventoryUw.Create(inventoryAdded);
            }

            await _context.SaveAsync();

            return new ApiResponse()
            {
                flag = true,
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


    public async Task GetProductFromBranch(int productId, int productCount, int wareHouseId, int fisclaYearId, int invoiceId, string userId)
    {
        //دریافت همه سری انقضا هاُ
        var _productRefrence =
            await _context
            .inventoryUw
            .GetEn
            .AsNoTracking()
            .Where(c => c.ProductId == productId)
            .Where(c => c.WareHouseId == wareHouseId)
            .Where(c => c.FiscalYearId == fisclaYearId)
            .Where(c => c.OperationType == 1)
            .OrderByDescending(c => c.ExpireData)
            .ToListAsync();

        //حدف سری ساخت های بدون موجودی
        List<int> ZeroStock = new List<int>();

        for (int i = 0; i < _productRefrence.Count(); i++)
        {
            if (await _inventoryRepo.GetPhysicalStockForBranch(_productRefrence[i].Id) == 0)
            {
                ZeroStock.Add(_productRefrence[i].Id);
            }
        }

        var expireDateWithStock = _productRefrence
            .Where(c => !ZeroStock.Contains(c.Id))
            .OrderBy(c => c.ExpireData)
            .ToList();

        //برداشت ار هر سری انقضا

        int savedStock = productCount;
        for (int j = 0; j < expireDateWithStock.Count(); j++)
        {
            //بدست اوردن موجودی هر سری
            int getBranchStock = await _inventoryRepo.GetPhysicalStockForBranch(expireDateWithStock[j].Id);
            if (savedStock <= getBranchStock)
            {
                var _inventory = new Inventory()
                {
                    OperationDate = DateTime.Now,
                    CreateDateTime = DateTime.Now.ToString(),
                    OperationType = 5,
                    Description = "فروش",
                    FiscalYearId = fisclaYearId,
                    WareHouseId = wareHouseId,
                    ProductId = productId,
                    InvoiceId = invoiceId,
                    UserId = userId,
                    ProductWastage = 0,
                    ProductCountMain = savedStock,
                    ProductLocationId = _context.inventoryUw.Get(c => c.Id == expireDateWithStock[j].Id).Select(c => c.ProductLocationId).Single(),
                    ReferenceId = expireDateWithStock[j].Id,
                    ExpireData = expireDateWithStock[j].ExpireData,
                };
                await _context.inventoryUw.Create(_inventory);
                break;
            }
            else if (savedStock > getBranchStock)
            {
                savedStock -= getBranchStock;
                var _inventory = new Inventory()
                {
                    OperationDate = DateTime.Now,
                    CreateDateTime = DateTime.Now.ToString(),
                    OperationType = 5,
                    Description = "فروش",
                    FiscalYearId = fisclaYearId,
                    WareHouseId = wareHouseId,
                    ProductId = productId,
                    InvoiceId = invoiceId,
                    UserId = userId,
                    ProductWastage = 0,
                    ProductCountMain = getBranchStock,
                    ProductLocationId = _context.inventoryUw.Get(c => c.Id == expireDateWithStock[j].Id).Select(c => c.ProductLocationId).Single(),
                    ReferenceId = expireDateWithStock[j].Id,
                    ExpireData = expireDateWithStock[j].ExpireData,
                };
                await _context.inventoryUw.Create(_inventory);
            }

            await _context.SaveAsync();

        }


    }
}