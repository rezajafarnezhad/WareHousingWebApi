using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Entities.Models.Dto;

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

    Task<List<ProductFlowReplyDto>> GetProductFlow(ProductFlowDto model);

    /// <summary>
    /// موجودی یک کالا
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="fiscalYearId"></param>
    /// <param name="wareHouseId"></param>
    /// <returns></returns>
    Task<int> GetProductStock(int productId, int fiscalYearId, int wareHouseId);

    Task GetProductFromBranch(int productId, int productCount, int wareHouseId, int fisclaYearId, int invoiceId, string userId);
}