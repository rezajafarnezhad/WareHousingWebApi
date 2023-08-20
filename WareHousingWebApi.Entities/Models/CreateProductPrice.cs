using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CreateProductPrice
{
    [Required]
    public int PurchasePrice { get; set; }

    [Required]

    //قیمت فروش به عمده فروش با فروشگاه
    public int SalesPrice { get; set; }

    [Required]

    //قیمت مصرف کننده
    public int CoverPrice { get; set; }

    [Required]

    public DateTime CreateDateTime { get; set; }

    [Required]

    public int ProductId { get; set; }
    
    public string UserId { get; set; }
    
    public int FiscalYearId { get; set; }


    [Required]
    //تاریخ اعمال قیمت
    public string ActionDate { get; set; }
}



public class ProductsPrice
{
    public int ProductPriceId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    public string? UserId { get; set; }

    public int FiscalYearId { get; set; }
    public DateTime ActionDate { get; set; }
    public int PurchasePrice { get; set; }
    public int SalesPrice { get; set; }

    public int CoverPrice { get; set; }

}