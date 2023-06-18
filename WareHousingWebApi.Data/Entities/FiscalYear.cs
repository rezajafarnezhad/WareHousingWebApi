using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Data.Entities;

public class FiscalYear
{
    [Key]
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string FiscalYearDescription { get; set; }
    //true => open
    //false => close
    public bool FiscalFlag { get; set; } = false;

    public string UserId { get; set; }
    public string CreateDateTime { get; set; } 

}