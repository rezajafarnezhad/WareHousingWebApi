using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models.Dto;


public class ReturnInvoice
{
    [Required]
    public int invoiceId { get; set; }
    [Required]

    public string userid { get; set; }
    [Required]

    public int fiscalYearId { get; set; }
}