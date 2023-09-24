using WareHousingWebApi.Data.Services.Repository;
using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IInvoiceRepo
{
    Task<ProductItemsDto> GetProductItemInfo(int productCode, int wareHouseId, int fiscalYearId);

    /// <summary>
    /// به دست اودن فیمت های یک محصول
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="fiscalYearId"></param>
    /// <returns></returns>
    GetPrice GetPrices(int productId);

    Task<int> CalculateInvoicePrice(int[] productid, int[] count, int fiscalYaerId);
    Task<List<InvoiceList>> GetInvoiceList(InvoiceListDto model);
    Task<List<InvoiceItemList>> GetInvoiceList(int Id);
    Task<InvoiceDetailsPrint> GetInvoiceForPrint(int id);
    Task<List<AllProductForInvoice>> GetProductsInvoice(IndeedParameterForAllProduct model);
    Task<List<GroupInvoiceList>> GroupInvoice(GroupInvoiceDto model);
}