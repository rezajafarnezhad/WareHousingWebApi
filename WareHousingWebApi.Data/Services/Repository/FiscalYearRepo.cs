using Microsoft.EntityFrameworkCore;
using System;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousingWebApi.Data.Services.Repository;

public class FiscalYearRepo : IFiscalYearRepo
{
    private readonly ApplicationDbContext _context;

    public FiscalYearRepo(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<bool> CheckDatesForFiscalYear(DateTime startDate, DateTime endDate)
    {



        if (endDate < startDate)
            return false;

        //هم پوشانی نداشته باشد
        if (startDate <= _context.FiscalYears_tbl.Max(c => c.EndDate))
            return false;

        return true;

    }

    public async Task<bool> IsExistDates(string startDate, string endDate)
    {
        var sd = startDate.ConvertShamsiToMiladi();
        var ed = endDate.ConvertShamsiToMiladi();
        var _Dates = _context.FiscalYears_tbl.Where(c => c.StartDate == sd && c.EndDate == ed).ToList();

        if (_Dates.Count == 1)
            return true;
      
        return false;
     

    }
}