using WareHousingWebApi.Entities.Models;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IInventoryRepo
{
    Task<List<InventoryStockModel>> GetProductStock(InventoryQueryMaker model);
}