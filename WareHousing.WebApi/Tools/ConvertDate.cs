﻿using MD.PersianDateTime.Standard;

namespace WareHousing.WebApi.Tools;

public static class ConvertDate
{
    public static DateTime ConvertShamsiToMiladi(this string Shamsidate)
    {
        //2025/03/12
        PersianDateTime pdate = PersianDateTime.Parse(Shamsidate);
        return pdate.ToDateTime();
    } 
    
    public static string ConvertMiladiToShamsi(DateTime Miladidate,string format)
    {
        //1403/02/12

        var pdate = new PersianDateTime(Miladidate);
        return pdate.ToString(format);
    }
}
