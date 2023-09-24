using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.Data.Services.Interface;

public interface IFiscalYearRepo
{
    Task<bool> CheckDatesForFiscalYear(DateTime startDate, DateTime endDate);
    Task<bool> IsExistDates(string startDate, string endDate);
    Task<FiscalYear> GetNextFiscalYear();
}

