using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CloseFiscalYear
{
    [Required]
    public int wareHouseId { get; set; }
    [Required]

    public string userId { get; set; }
    [Required]

    public int fiscalYearId { get; set; }
}