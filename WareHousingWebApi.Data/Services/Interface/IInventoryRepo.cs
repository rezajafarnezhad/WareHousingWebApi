using WareHousingWebApi.Entities.Models;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IInventoryRepo
{
    Task<List<InventoryStockModel>> GetProductStock(InventoryQueryMaker model);

    /// <summary>
    /// دریافت موجوذی هر سری ساخت
    /// </summary>
    /// <param name="inventoryId"></param>
    /// <returns></returns>
    Task<int> GetPhysicalStockForBranch(int inventoryId);

    /// <summary>
    /// دریافت موجوذی هر سری ساخت
    /// </summary>
    /// <param name="inventoryId"></param>
    /// <returns></returns>
    Task<int> GetPhysicalWastageStockForBranch(int inventoryId);
}