using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Repository;

public class WastageRialStockRepo : UnitOfWork, IWastageRialStockRepo
{
    public WastageRialStockRepo(ApplicationDbContext context) : base(context)
    { }

    /// <summary>
    /// فقط ضایعاتی ها
    /// </summary>
    /// <param name="fiscalYearId"></param>
    /// <param name="wareHouseId"></param>
    /// <returns></returns>
    public async Task<List<WastageRialStockDto>> GetWastageRialStock(int fiscalYearId, int wareHouseId)
    {
        //لیست همه قیمت ها
        var lstPriceList = this.productPriceUW.GetEn.Where(c => c.FiscalYearId == fiscalYearId).AsEnumerable();

        //لیست همه تراکنش ها
        var StockList = this.inventoryUw.GetEn.Where(c => c.FiscalYearId == fiscalYearId && c.WareHouseId == wareHouseId).Where(c=>c.OperationType==3 || c.OperationType==4).AsEnumerable();

        var lstProductRialStock = await this.productsUw.GetEn.Select(c => new WastageRialStockDto()
        {

            ProductId = c.ProductId,
            ProductName = c.ProductName,
            ProductCode = c.ProductCode,
          
            TotalWastageProductCount =
                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 3 ? i.ProductWastage :
                        i.OperationType == 4 ? -i.ProductWastage : 0)
            ,

            TotalWastagePurchasePrice =

                lstPriceList.Where(p => p.ActionDate <= DateTime.Now && p.ProductId == c.ProductId)
                    .OrderByDescending(p => p.ActionDate)
                    .Take(1)
                    .Select(p => p.PurchasePrice)
                    .DefaultIfEmpty()
                    .Single()
                *
            StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 3 ? i.ProductWastage :
                        i.OperationType == 4 ? -i.ProductWastage : 0)
            ,

            TotalWastageCoverPrice =

                lstPriceList.Where(v => v.ActionDate <= DateTime.Now && v.ProductId == c.ProductId)
                    .OrderByDescending(v => v.ActionDate)
                    .Take(1)
                    .Select(v => v.CoverPrice)
                    .DefaultIfEmpty()
                    .Single()
                *
                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 3 ? i.ProductWastage :
                        i.OperationType == 4 ? -i.ProductWastage : 0)
            ,

            TotalWastageSalePrice =

                lstPriceList.Where(s => s.ActionDate <= DateTime.Now && s.ProductId == c.ProductId)
                    .OrderByDescending(s => s.ActionDate)
                    .Take(1)
                    .Select(s => s.SalesPrice)
                    .DefaultIfEmpty()
                    .Single()
                *
                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 3 ? i.ProductWastage :
                        i.OperationType == 4 ? -i.ProductWastage : 0)
            ,



        }).ToListAsync();


        return lstProductRialStock;

    }

}