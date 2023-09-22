using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Repository;

public class InventoryRepo : UnitOfWork, IInventoryRepo
{
    private readonly IMapper _mapper;
    public InventoryRepo(ApplicationDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<List<InventoryStockModel>> GetProductStock(InventoryQueryMaker model)
    {
        var lstProductStock = this.productsUw.GetEn.Select(c => new InventoryStockModel()
        {
            ProductId = c.ProductId,
            ProductCode = c.ProductCode,
            ProductName = c.ProductName,
            TotalProductCount = this.inventoryUw.GetEn.Where(x => x.ProductId == c.ProductId
                                                                    && x.FiscalYearId == model.FiscalYearId
                                                                    && x.WareHouseId == model.WareHouseId)

                .Sum(x =>
                    x.OperationType == 1 ? x.ProductCountMain :
                    x.OperationType == 2 ? -x.ProductCountMain :
                    x.OperationType == 3 ? -x.ProductWastage :
                    x.OperationType == 4 ? x.ProductWastage :
                    x.OperationType == 6 ? x.ProductCountMain :
                    x.OperationType == 5 ? -x.ProductCountMain :
                    x.OperationType == 7 ? x.ProductCountMain :
                    x.OperationType == 8 ? -x.ProductCountMain :
                    0)

                ,

            TotalProductWaste = this.inventoryUw.GetEn.Where(x => x.ProductId == c.ProductId
                                                                  && x.FiscalYearId == model.FiscalYearId
                                                                  && x.WareHouseId == model.WareHouseId
                                                                  && x.ProductWastage > 0)
                .Sum(x => x.OperationType == 3 ? x.ProductWastage :
                                            x.OperationType == 4 ? -x.ProductWastage : 0)

        }).ToListAsync();

        return await lstProductStock;
    }


    /// <summary>
    /// موجودی یک کالا
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="fiscalYearId"></param>
    /// <param name="wareHouseId"></param>
    /// <returns></returns>
    public async Task<int> GetProductStock(int productId, int fiscalYearId, int wareHouseId)
    {
        var _count = this.inventoryUw
            .GetEn
            .Where(c => c.ProductId == productId)
            .Where(c => c.WareHouseId == wareHouseId)
            .Where(c => c.FiscalYearId == fiscalYearId)
            .Sum(x =>
                x.OperationType == 1 ? x.ProductCountMain :
                x.OperationType == 2 ? -x.ProductCountMain :
                x.OperationType == 3 ? -x.ProductWastage :
                x.OperationType == 4 ? x.ProductWastage :
                x.OperationType == 6 ? x.ProductCountMain :
                x.OperationType == 5 ? -x.ProductCountMain :
                x.OperationType == 7 ? x.ProductCountMain :
                x.OperationType == 8 ? -x.ProductCountMain :
                0);

        return _count;

    }



    /// <summary>
    /// دریافت موجوذی هر سری ساخت
    /// </summary>
    /// <param name="inventoryId"></param>
    /// <returns></returns>
    public async Task<int> GetPhysicalStockForBranch(int inventoryId)
    {
        var _physicalStock = this
            .inventoryUw
            .GetEn
            .Where(c => c.Id == inventoryId || c.ReferenceId == inventoryId)
            .Select (c => c.Id).ToList();

            int _physicalStock2 = this.inventoryUw.GetEn.
            Where(s=> _physicalStock.Contains(s.ReferenceId) || s.Id == inventoryId).ToList()
            .Sum(x =>
                x.OperationType == 1 ? x.ProductCountMain :
                x.OperationType == 2 ? -x.ProductCountMain :
                x.OperationType == 3 ? -x.ProductWastage :
                x.OperationType == 4 ? x.ProductWastage :
                x.OperationType == 6 ? x.ProductCountMain :
                x.OperationType == 5 ? -x.ProductCountMain :
                x.OperationType == 7 ? x.ProductCountMain :
                x.OperationType == 8 ? -x.ProductCountMain :
                0);
        
        return _physicalStock2;
    }

    public int GetPhysicalStockForBranch2(int inventoryId)
    {
        var _physicalStock = this
            .inventoryUw
            .GetEn
            .Where(c => c.Id == inventoryId || c.ReferenceId == inventoryId)
            .Select(c => c.Id).ToList();

        int _physicalStock2 = this.inventoryUw.GetEn.
            Where(s => _physicalStock.Contains(s.ReferenceId) || s.Id == inventoryId).ToList()
            .Sum(x =>
                x.OperationType == 1 ? x.ProductCountMain :
                x.OperationType == 2 ? -x.ProductCountMain :
                x.OperationType == 3 ? -x.ProductWastage :
                x.OperationType == 4 ? x.ProductWastage :
                x.OperationType == 6 ? x.ProductCountMain :
                x.OperationType == 5 ? -x.ProductCountMain :
                x.OperationType == 7 ? x.ProductCountMain :
                x.OperationType == 8 ? -x.ProductCountMain :
                0);

        return _physicalStock2;
    }



    /// <summary>
    /// دریافت موجوذی هر سری ساخت
    /// </summary>
    /// <param name="inventoryId"></param>
    /// <returns></returns>
    public async Task<int> GetPhysicalWastageStockForBranch(int inventoryId)
    {
        var _physicalStock = this.inventoryUw.GetEnNoTraking
            .Where(c => c.Id == inventoryId || c.ReferenceId == inventoryId)
            .Sum(x =>
                x.OperationType == 3 ? x.ProductWastage
                : x.OperationType == 4 ? -x.ProductWastage : 0);


        return _physicalStock;
    }

    public async Task<List<ProductFlowReplyDto>> GetProductFlow(ProductFlowDto model)
    {


        if (string.IsNullOrWhiteSpace(model.FromDate))
            model.FromDate = "1300/01/01";


        if (string.IsNullOrWhiteSpace(model.ToDate))
            model.ToDate = "1900/01/01";

        var Fromdata = model.FromDate.ConvertShamsiToMiladi();
        var Todata = model.ToDate.ConvertShamsiToMiladi();

        var _data =await _mapper.ProjectTo<ProductFlowReplyDto>
            (this.inventoryUw.GetEnNoTraking
                .Include(c => c.Users)
                .Where(c => c.ProductId == model.ProductId)
                .Where(c => c.FiscalYearId == model.FiscalYearId)
                .Where(c => c.WareHouseId == model.WareHouseId)
                .Where(c => c.OperationDate >= Fromdata && c.OperationDate <= Todata)).ToListAsync();


            return _data;
    }

    public async Task GetProductFromBranch(int productId, int productCount, int wareHouseId, int fisclaYearId, int invoiceId, string userId)
    {
        //دریافت همه سری انقضا هاُ
        var _productRefrence =
            await this.inventoryUw
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
            if (await GetPhysicalStockForBranch(_productRefrence[i].Id) == 0)
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
            int getBranchStock = await GetPhysicalStockForBranch(expireDateWithStock[j].Id);
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
                    ProductLocationId = this.inventoryUw.Get(c => c.Id == expireDateWithStock[j].Id).Select(c => c.ProductLocationId).Single(),
                    ReferenceId = expireDateWithStock[j].Id,
                    ExpireData = expireDateWithStock[j].ExpireData,
                };
                await this.inventoryUw.Create(_inventory);
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
                    ProductLocationId = this.inventoryUw.Get(c => c.Id == expireDateWithStock[j].Id).Select(c => c.ProductLocationId).Single(),
                    ReferenceId = expireDateWithStock[j].Id,
                    ExpireData = expireDateWithStock[j].ExpireData,
                };
                await this.inventoryUw.Create(_inventory);
            }

            await this.SaveAsync();

        }
    }

    public List<WareHouseHandling> GetWareHouseHanding(InventoryQueryMaker model)
    {
        IList<WareHouseHandling2> _data = this.inventoryUw
            .GetEn
            .Where(c => c.WareHouseId == model.WareHouseId)
            .Where(c => c.FiscalYearId == model.FiscalYearId)
            .Where(c => c.OperationType == 1)
            .Include(c => c.Product)
            .OrderBy(c => c.ExpireData)
            .GroupBy(c => new { c.ProductId, c.Product.ProductCode, c.Product.ProductName, c.ExpireData, c.Id })
            .Select(c => new WareHouseHandling2()
            {
                InventoryId=c.Key.Id,
                ProductCode = c.Key.ProductCode,
                ProductId = c.Key.ProductId,
                ProductName = c.Key.ProductName,
                ExpiryDate = c.Key.ExpireData,
                TotalProductCount = 0,
            }).ToList();

        foreach (var item1 in _data)
        {
            item1.TotalProductCount = GetPhysicalStockForBranch2(item1.InventoryId);
        }
        //حدف سری های موجودی صفر
        List<int> zeroStack = new List<int>();
        foreach (var item in _data)
        {
            if (GetPhysicalStockForBranch2(item.InventoryId) == 0)
            {
                zeroStack.Add(item.InventoryId);

            }
        }

        //حدف موحودی صفرا

        return
            _data.Where(c => !zeroStack.Contains(c.InventoryId))
                .GroupBy(c => new { c.ProductId, c.ProductCode, c.ProductName, c.ExpiryDate })
                .Select(s => new WareHouseHandling()
                {
                    ProductCode = s.Key.ProductCode,
                    ProductId = s.Key.ProductId,
                    ProductName = s.Key.ProductName,
                    ExpiryDate = s.Key.ExpiryDate,
                    TotalProductCount = s.Sum(c => c.TotalProductCount),

                }).ToList();
    }



}