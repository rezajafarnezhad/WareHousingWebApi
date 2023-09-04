using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IWastageRialStockRepo
{
    Task<List<WastageRialStockDto>> GetWastageRialStock(int fiscalYearId, int wareHouseId);
}