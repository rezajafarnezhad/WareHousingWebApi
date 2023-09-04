using WareHousingWebApi.Entities.Models.Dto;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IRialStockRepo
{
    Task<List<RialStockDto>> GetRialStock(int fiscalYearId, int wareHouseId);
}