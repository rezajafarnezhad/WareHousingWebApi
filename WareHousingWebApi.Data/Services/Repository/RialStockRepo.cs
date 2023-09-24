using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Repository;

public class RialStockRepo : UnitOfWork, IRialStockRepo
{
    public RialStockRepo(ApplicationDbContext context) : base(context)
    { }


    public async Task<List<RialStockDto>> GetRialStock(int fiscalYearId, int wareHouseId)
    {
        //لیست همه قیمت ها
        var lstPriceList = this.productPriceUW.GetEn.AsEnumerable();

        //لیست همه تراکنش ها
        var StockList = this.inventoryUw.GetEn.Where(c => c.FiscalYearId == fiscalYearId && c.WareHouseId == wareHouseId).AsEnumerable();

        var lstProductRialStock = await this.productsUw.GetEn
            .Select(c => new RialStockDto()
        {

            ProductId = c.ProductId,
            ProductName = c.ProductName,
            ProductCode = c.ProductCode,
            TotalProductCount=
                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i => 
                        i.OperationType == 1 ? i.ProductCountMain :
                        i.OperationType == 2 ? -i.ProductCountMain :
                        i.OperationType == 3 ? -i.ProductWastage :
                        i.OperationType == 4 ? i.ProductWastage :
                        i.OperationType == 6 ? i.ProductCountMain :
                        i.OperationType == 5 ? -i.ProductCountMain :
                        i.OperationType == 7 ? i.ProductCountMain :
                        i.OperationType == 8 ? -i.ProductCountMain :
                        i.OperationType == 9 ? i.ProductCountMain :
                        0)
                    ,

            //به دست آوردن آخرین قیمت خرید محصول ایکس * موجودی
            TotalPurchasePrice =

                lstPriceList.Where(p => p.ActionDate <= DateTime.Now && p.ProductId == c.ProductId)
                    .OrderByDescending(p => p.ActionDate)
                    .Take(1)
                    .Select(p => p.PurchasePrice)
                    .DefaultIfEmpty()
                    .Single()
                *

                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 1 ? i.ProductCountMain :
                        i.OperationType == 2 ? -i.ProductCountMain :
                        i.OperationType == 3 ? -i.ProductWastage :
                        i.OperationType == 4 ? i.ProductWastage :
                        i.OperationType == 6 ? i.ProductCountMain :
                        i.OperationType == 5 ? -i.ProductCountMain :
                        i.OperationType == 7 ? i.ProductCountMain :
                        i.OperationType == 8 ? -i.ProductCountMain :
                        i.OperationType == 9 ? i.ProductCountMain :
                        
                        0)
            ,

            //به دست آوردن آخرین قیمت مصرف کننده محصول ایکس * موجودی
            TotalCoverPrice =

                 lstPriceList.Where(v => v.ActionDate <= DateTime.Now && v.ProductId == c.ProductId)
                    .OrderByDescending(v => v.ActionDate)
                    .Take(1)
                    .Select(v => v.CoverPrice)
                    .DefaultIfEmpty()
                    .Single()
                    *
                    StockList.Where(i => i.ProductId == c.ProductId)
                        .Sum(i =>
                            i.OperationType == 1 ? i.ProductCountMain :
                            i.OperationType == 2 ? -i.ProductCountMain :
                            i.OperationType == 3 ? -i.ProductWastage :
                            i.OperationType == 4 ? i.ProductWastage :
                            i.OperationType == 6 ? i.ProductCountMain :
                            i.OperationType == 5 ? -i.ProductCountMain :
                            i.OperationType == 7 ? i.ProductCountMain :
                            i.OperationType == 8 ? -i.ProductCountMain :
                            i.OperationType == 9 ? i.ProductCountMain :
                            0)

            ,

            //به دست آوردن آخرین قیمت فروش محصول ایکس * موجودی
            TotalSalePrice =

                  lstPriceList.Where(s => s.ActionDate <= DateTime.Now && s.ProductId == c.ProductId)
                    .OrderByDescending(s => s.ActionDate)
                    .Take(1)
                    .Select(s => s.SalesPrice)
                    .DefaultIfEmpty()
                    .Single()
                *
                StockList.Where(i => i.ProductId == c.ProductId)
                    .Sum(i =>
                        i.OperationType == 1 ? i.ProductCountMain :
                        i.OperationType == 2 ? -i.ProductCountMain :
                        i.OperationType == 3 ? -i.ProductWastage :
                        i.OperationType == 4 ? i.ProductWastage :
                        i.OperationType == 6 ? i.ProductCountMain :
                        i.OperationType == 5 ? -i.ProductCountMain :
                        i.OperationType == 7 ? i.ProductCountMain :
                        i.OperationType == 8 ? -i.ProductCountMain :
                        i.OperationType == 9 ? i.ProductCountMain :
                        0)
                    ,

            

        }).ToListAsync();


        return lstProductRialStock;

    }

}