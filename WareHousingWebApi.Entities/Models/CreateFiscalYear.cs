using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CreateFiscalYear
{
    [Required()]
    public string StartDate { get; set; }
    [Required()]

    public string EndDate { get; set; }
    [Required()]
    public string FiscalYearDescription { get; set; }
    
    public bool FiscalFlag { get; set; }

    public string UserId { get; set; }
    public string CreateDateTime { get; set; }
}

public class EditFiscalYear : CreateFiscalYear
{
    public int Id { get; set; }

}