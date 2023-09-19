using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models.Dto;

public class IndeedParameterForAllProduct
{
    [Required]
    public int fiscalYearId { get; set; }

    [Required]
    public int wareHouseId { get; set; }

    public string FromDate { get; set; }
    public string ToDate { get; set; }
}



public class AllProductForInvoice
{
    public int ProductId { get; set; }
    public int ProductCount { get; set; }
    public int ProductCode { get; set; }
    public string ProductName { get; set; }
    public DateTime ExpireDate { get; set; }

}