using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
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

                    .Sum(x => x.OperationType == 1 ? x.ProductCountMain :
                                               x.OperationType == 2 ? -x.ProductCountMain :
                                               x.OperationType == 3 ? -x.ProductWastage :
                                               x.OperationType == 4 ? x.ProductWastage :
                                               x.OperationType == 6 ? x.ProductCountMain :
                                               x.OperationType == 5 ? -x.ProductCountMain : 0)

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
    /// دریافت موجوذی هر سری ساخت
    /// </summary>
    /// <param name="inventoryId"></param>
    /// <returns></returns>
    public async Task<int> GetPhysicalStockForBranch(int inventoryId)
    {
        var _physicalStock = this.inventoryUw.GetEnNoTraking
            .Where(c => c.Id == inventoryId || c.ReferenceId == inventoryId)
            .Sum(x =>
                x.OperationType == 1 ? x.ProductCountMain :
                x.OperationType == 2 ? -x.ProductCountMain :
                x.OperationType == 3 ? x.ProductWastage :
                x.OperationType == 4 ? -x.ProductWastage :
                x.OperationType == 6 ? x.ProductCountMain :
                x.OperationType == 5 ? -x.ProductCountMain : 0);
        return _physicalStock;
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
}