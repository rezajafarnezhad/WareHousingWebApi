using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Entities.Models.Dto;
using WareHousingWebApi.WebFramework.ApiResult;

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

                    var _productPrice = _invoiceRepo.GetPrices(model.productIds[i]);

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
                        Date=DateTime.Now
                    };

                    await _context.invoiceItemsUW.Create(_InvoiceItem);
                    if (model.InvoiceStatus == 1)
                    {
                        //کسر از موجودی
                        await _inventoryRepo.GetProductFromBranch(model.productIds[i], model.productcount[i], model.wareHouseId, model.fiscalYearId
                            , _invoice.Id, model.userId);
                    }
                   
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


    [HttpPost("SetInvoiceToClose")]
    public async Task<ApiResponse> SetInvoiceToClose([FromBody] CloseInvoice model)
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
            var _invoice =await _context.invoiceUW.GetById(model.invoiceId);
            _invoice.InvoiceStatus = 1;
            _context.invoiceUW.Update(_invoice);

            //کسر از موجودی
            var _invoiceItems = _context.invoiceItemsUW.Get(c => c.InvoiceId == model.invoiceId).ToList();

            foreach (var items in _invoiceItems)
            {
                await _inventoryRepo.GetProductFromBranch(items.ProductId, items.Count, _invoice.WareHouseId, model.fiscalYearId,
                    _invoice.Id, model.userid);
            }

            await _context.SaveAsync();

            return new ApiResponse()
            {
                flag = true,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };

        }
        catch (Exception )
        {
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };
        }


    }

    [HttpDelete("DeleteTemporaryInvoice")]
    public async Task<ApiResponse> DeleteTemporaryInvoice([FromQuery] int id)
    {
        if(id <= 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };

        try
        {
            var _invoice = await _context.invoiceUW.GetById(id);
            _context.invoiceUW.Delete(_invoice);

            //Delete Items
            var _InvoiceItems = _context.invoiceItemsUW.Get(c => c.InvoiceId == _invoice.Id).ToList();
            _context.invoiceItemsUW.DeleteByRange(_InvoiceItems);
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
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };
        }
        

    }

    /// <summary>
    /// For Print
    /// </summary>
    [HttpGet("PrintInvoice")]
    public async Task<ApiResponse> GetInvoiceDetails([FromQuery] int id)
    {

        if (id <= 0)
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
            };


        var _data = await _invoiceRepo.GetInvoiceForPrint(id);
        return new ApiResponse<InvoiceDetailsPrint>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }


    [HttpGet("AllProductInvoice")]
    public async Task<ApiResponse> AllProductInvoice([FromBody] IndeedParameterForAllProduct model)
    {

        var _data = await _invoiceRepo.GetProductsInvoice(model);
        return new ApiResponse<List<AllProductForInvoice>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };

    }

    [HttpGet("GroupInvoice")]
    public async Task<ApiResponse> GroupSeparationInvoice([FromBody] GroupInvoiceDto model)
    {
        var _data = await _invoiceRepo.GroupInvoice(model);
        return new ApiResponse<List<GroupInvoiceList>>()
        {
            flag = true,
            Data = _data,
            StatusCode = ApiStatusCode.Success,
            Message = ApiStatusCode.Success.GetEnumDisplayName(),
        };
    }

}