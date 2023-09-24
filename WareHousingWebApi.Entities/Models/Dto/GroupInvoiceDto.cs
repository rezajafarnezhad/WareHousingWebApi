using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models.Dto;

public class GroupInvoiceDto
{

    [Required]
    public int wareHouseId { get; set; }
    public int fiscalYearId { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}