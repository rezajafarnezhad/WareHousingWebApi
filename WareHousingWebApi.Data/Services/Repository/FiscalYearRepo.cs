using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.Data.Services.Repository;

public class FiscalYearRepo : UnitOfWork ,IFiscalYearRepo
{
    public FiscalYearRepo(ApplicationDbContext context) : base(context)
    {
       
    }

    public async Task<bool> CheckDatesForFiscalYear(DateTime startDate, DateTime endDate)
    {

        if (endDate <= startDate)
            return false;

        //هم پوشانی نداشته باشد
        if (startDate.Date <= this.fiscalYearUw.GetEn.Where(c=>c.StartDate.Date !=startDate.Date).Max(c => c.EndDate))
            return false;

        return true;

    }

    public async Task<bool> IsExistDates(string startDate, string endDate)
    {
        var sd = startDate.ConvertShamsiToMiladi();
        var ed = endDate.ConvertShamsiToMiladi();
        var _Dates = this.fiscalYearUw.GetEn.Where(c => c.StartDate == sd && c.EndDate == ed).ToList();

        if (_Dates.Count == 1)
            return true;
      
        return false;
     

    }

    public async Task<FiscalYear> GetNextFiscalYear()
    {
        var CurrentEndDate = await this.fiscalYearUw.GetEn.Where(c => c.FiscalFlag).Select(c => c.EndDate).SingleAsync();
        return await this.fiscalYearUw.GetEn.OrderByDescending(c => c.EndDate).Where(c => !c.FiscalFlag && c.StartDate.Date > CurrentEndDate.Date).SingleAsync();
    }
}