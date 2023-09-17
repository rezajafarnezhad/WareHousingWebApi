using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CreateInvoice
{

    [Required]
    public int wareHouseId { get; set; }
    [Required]

    public int fiscalYearId { get; set; }
    [Required]
    public int customerId { get; set; }
    [Required]
    public List<int> productIds { get; set; }
    [Required]
    public List<int> productcount { get; set; }
    [Required]
    public string userId { get; set; }



}