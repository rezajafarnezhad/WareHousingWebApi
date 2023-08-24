using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Models;

namespace WareHousingWebApi.Data.Services.Repository;

public class ProductPriceRepo :UnitOfWork , IProductPriceRepo
{
    public ProductPriceRepo(ApplicationDbContext context) : base(context)
    {
       
    }

    public async Task<IEnumerable<ProductsPrice>> GetProductsPrice(int fiscalYearId)
    {

        var _productsPrice = this.productPriceUW.GetEn.Where(c => c.FiscalYearId == fiscalYearId).AsEnumerable();


        var data =await this.productsUw.GetEn.Select(c => new ProductsPrice()
        {

            ProductId = c.ProductId,
            ProductName = c.ProductName,
            ProductCode = c.ProductCode,
            FiscalYearId = fiscalYearId,

            ProductPriceId = _productsPrice.Where(w => w.ProductId == c.ProductId
                                                       && w.ActionDate <= DateTime.Now)
                .OrderByDescending(w => w.ActionDate).Take(1).Select(w => w.Id).DefaultIfEmpty().Single(),

            PurchasePrice = _productsPrice.Where(p => p.ProductId == c.ProductId
                                                      && p.ActionDate <= DateTime.Now)
                .OrderByDescending(p => p.ActionDate).Take(1).Select(c => c.PurchasePrice).DefaultIfEmpty().Single(),

            SalesPrice = _productsPrice.Where(s => s.ProductId == c.ProductId
                                                   && s.ActionDate <= DateTime.Now)
                .OrderByDescending(s => s.ActionDate).Take(1).Select(s => s.SalesPrice).DefaultIfEmpty().Single(),

            CoverPrice = _productsPrice.Where(v => v.ProductId == c.ProductId
                                                   && v.ActionDate <= DateTime.Now).
                OrderByDescending(v => v.ActionDate).Take(1).Select(v => v.CoverPrice).DefaultIfEmpty().Single(),


            ActionDate= _productsPrice.Where(d=>d.ProductId == c.ProductId 
                                                && d.ActionDate <=DateTime.Now)
                .OrderByDescending(d=>d.ActionDate).Take(1).Select(d=>d.ActionDate).DefaultIfEmpty().Single(),

            UserId = _productsPrice.Where(u => u.ProductId == c.ProductId
                                               && u.ActionDate <= DateTime.Now)
                .OrderByDescending(u => u.ActionDate).Take(1).Select(u => u.UserId).DefaultIfEmpty().Single(),

        }).ToListAsync();

        return data;
    }
}