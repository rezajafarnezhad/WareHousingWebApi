using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Migrations;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Repository;

public class InvoiceRepo : UnitOfWork, IInvoiceRepo
{
    private readonly IMapper _mapper;
   
    public InvoiceRepo(ApplicationDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<ProductItemsDto> GetProductItemInfo(int productCode, int wareHouseId, int fiscalYearId)
    {
        var _productInfo =
            await
            this.productsUw
                .GetEnNoTraking
            .Where(c => c.ProductCode == productCode)
            .Select(c => new ProductItemsDto()
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
            }).SingleOrDefaultAsync();

        var ProductPrice = GetPrices(_productInfo.ProductId);
        _productInfo.PurchasePrice = ProductPrice.PurchasePrice;
        _productInfo.CoverPrice = ProductPrice.CoverPrice;
        _productInfo.SalesPrice = ProductPrice.SalesPrice;
        _productInfo.ProductStock = GetProductStock(_productInfo.ProductId, wareHouseId, fiscalYearId);


        return _productInfo;
    }

    //محاسبه موجودی کالا
    private int GetProductStock(int productId, int wareHouseId, int fiscalYearId)
    {
        var ProductStock = this.inventoryUw
            .GetEnNoTraking
            .Where(c => c.ProductId == productId)
            .Where(c => c.FiscalYearId == fiscalYearId)
            .Where(c => c.WareHouseId == wareHouseId)
            .Sum(x =>
                x.OperationType == 1 ? x.ProductCountMain :
                x.OperationType == 2 ? -x.ProductCountMain :
                x.OperationType == 3 ? -x.ProductWastage :
                x.OperationType == 4 ? x.ProductWastage :
                x.OperationType == 6 ? x.ProductCountMain :
                x.OperationType == 5 ? -x.ProductCountMain :
                x.OperationType == 7 ? x.ProductCountMain :
                x.OperationType == 8 ? -x.ProductCountMain :
                x.OperationType == 9 ? x.ProductCountMain :
                0);
        return ProductStock;
    }

    /// <summary>
    /// به دست اودن فیمت های یک محصول
    /// </summary>
    /// <param name="productId"></param>
    /// 
    /// <returns></returns>
    public GetPrice GetPrices(int productId)
    {
        var _productsPrice = this.productPriceUW
            .GetEnNoTraking
            .Where(c => c.ProductId == productId)
            .Where(c => c.ActionDate <= DateTime.Now)
            .OrderByDescending(c => c.ActionDate)
            .Take(1)
            .AsEnumerable().Select(c => new GetPrice()
            {
                CoverPrice = c.CoverPrice,
                PurchasePrice = c.PurchasePrice,
                SalesPrice = c.SalesPrice,
            }).SingleOrDefault();
        return _productsPrice;
    }

    public async Task<int> CalculateInvoicePrice(int[] productid, int[] count, int fiscalYaerId)
    {
        int totalPrice = 0;

        for (int i = 0; i < productid.Count(); i++)
        {
            var _SalesPrice = GetPrices(productid[i]).SalesPrice;
            totalPrice += _SalesPrice * count[i];
        }
        return totalPrice;
    }


    public async Task<List<InvoiceList>> GetInvoiceList(InvoiceListDto model)
    {

        if (string.IsNullOrWhiteSpace(model.FromDate))
            model.FromDate = "1300/01/01";


        if (string.IsNullOrWhiteSpace(model.ToDate))
            model.ToDate = "1900/01/01";

        var Fromdata = model.FromDate.ConvertShamsiToMiladi();
        var Todata = model.ToDate.ConvertShamsiToMiladi();
        var _data =
            await _mapper.ProjectTo<InvoiceList>(
            this.invoiceUW.GetEnNoTraking
                .Where(c => c.WareHouseId == model.WareHouseId)
                .Where(c => c.fiscalYearId == model.FiscalYearId)
                .Where(c => c.Date >= Fromdata && c.Date <= Todata)

            ).ToListAsync();


        return _data;

    }


    


    public async Task<List<InvoiceItemList>> GetInvoiceList(int Id)
    {

        var _data = await _mapper.ProjectTo<InvoiceItemList>(
            
            this.invoiceItemsUW.GetEnNoTraking
                .Where(c=>c.InvoiceId == Id)).ToListAsync();

        return _data;
    }


    public async Task<InvoiceDetailsPrint> GetInvoiceForPrint(int id)
    {
        var _data =await _mapper
            .ProjectTo<InvoiceDetailsPrint>(this.invoiceUW.GetEnNoTraking
                .Include(c=>c.Customer)
                .Where(c=>c.Id == id))
            .SingleOrDefaultAsync();
        return _data;
    }


    public async Task<List<AllProductForInvoice>> GetProductsInvoice(IndeedParameterForAllProduct model)
    {
        if (string.IsNullOrWhiteSpace(model.FromDate))
            model.FromDate = "1300/01/01";


        if (string.IsNullOrWhiteSpace(model.ToDate))
            model.ToDate = "1900/01/01";

        var Fromdata = model.FromDate.ConvertShamsiToMiladi();
        var Todata = model.ToDate.ConvertShamsiToMiladi();


        var _data =
            await 
                this.inventoryUw
                .GetEnNoTraking
                .Where(c => c.OperationType == 5)
                .Where(c => c.WareHouseId == model.wareHouseId)
                .Where(c => c.FiscalYearId == model.fiscalYearId)
                .Where(c => c.OperationDate.Date >= Fromdata.Date && c.OperationDate <= Todata.Date)
                .Include(c => c.Product)
                .GroupBy(g => new { g.ProductId, g.Product.ProductName, g.ExpireData, g.Product.ProductCode })
                .Select(c => new AllProductForInvoice()
                {
                    ExpireDate = c.Key.ExpireData,
                    ProductCode = c.Key.ProductCode,
                    ProductId = c.Key.ProductId,
                    ProductName = c.Key.ProductName,
                    ProductCount=c.Sum(c=>c.ProductCountMain)
                }).ToListAsync();


        return _data;
    }


    public async Task<List<GroupInvoiceList>> GroupInvoice(GroupInvoiceDto model)
    {
        if (string.IsNullOrWhiteSpace(model.FromDate))
            model.FromDate = "1300/01/01";


        if (string.IsNullOrWhiteSpace(model.ToDate))
            model.ToDate = "1900/01/01";

        var Fromdata = model.FromDate.ConvertShamsiToMiladi();
        var Todata = model.ToDate.ConvertShamsiToMiladi();


        var _data =
            await
                this.invoiceItemsUW
                    .GetEnNoTraking
                    .Where(c => c.Invoice.WareHouseId == model.wareHouseId)
                    .Where(c => c.Invoice.fiscalYearId == model.fiscalYearId)
                    .Where(c => c.Date.Date >= Fromdata.Date && c.Date.Date <= Todata.Date)
                    .Include(c => c.Product)
                    .Include(c => c.Invoice)
                    .Select(v => new GroupInvoiceList()
                    {
                        InvoiceId = v.InvoiceId,
                        InvoiceNumber=v.Invoice.InvoiceNumber,
                        ProductCode=v.Product.ProductCode,
                        ProductId=v.Product.ProductId,
                        ProductName = v.Product.ProductName,
                        ProductCount= v.Count

                    }).ToListAsync();

        return _data;
    }

}