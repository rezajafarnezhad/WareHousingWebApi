using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousingWebApi.Data.Services.Repository;

public class InventoryRepo : IInventoryRepo
{
    private readonly ApplicationDbContext _context;

    public InventoryRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventoryStockModel>> GetProductStock(InventoryQueryMaker model)
    {
        var lstProductStock = _context.Products_tbl.Select(c => new InventoryStockModel()
        {
            ProductId = c.ProductId,
            ProductCode = c.ProductCode,
            ProductName = c.ProductName,
            TotalProductCount = _context.Inventories_tbl.Where(x => x.ProductId == c.ProductId
                                                                    && x.FiscalYearId == model.FiscalYearId
                                                                    && x.WareHouseId == model.WareHouseId
                                                                    && x.ProductCountMain > 0
                                                                    && (x.OperationType == 1 ||
                                                                        x.OperationType == 2 ||
                                                                        x.OperationType == 5 ||
                                                                        x.OperationType == 6))
                    .Sum(x => x.ProductCountMain)

                ,

            TotalProductWaste = _context.Inventories_tbl.Where(x => x.ProductId == c.ProductId
                                                                    && x.FiscalYearId == model.FiscalYearId
                                                                    && x.WareHouseId == model.WareHouseId
                                                                    && x.ProductWastage > 0
                                                                    && (x.OperationType == 4 || x.OperationType == 3))
                    .Sum(x => x.ProductWastage)

        }).ToListAsync();

        return await lstProductStock;
    }
}