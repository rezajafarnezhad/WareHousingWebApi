using WareHousingWebApi.Entities.Models;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IProductPriceRepo
{
    Task<IEnumerable<ProductsPriceInput>> GetProductsPrice();
}