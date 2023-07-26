using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHousingWebApi.Data.Entities;

public class ProductPrice
{
    [Key]
    public int Id { get; set; }

    //قیمت خرید از تولید کننده
    public int PurchasePrice { get; set; }

    //قیمت فروش به عمده فروش با فروشگاه
    public int SalesPrice { get; set; }

    //قیمت مصرف کننده
    public int CoverPrice { get; set; }

    public DateTime CreateDateTime { get; set; }


    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int FiscalYearId { get; set; }



    //تاریخ اعمال قیمت
    public DateTime ActionDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public Users Users { get; set; }
    [ForeignKey(nameof(FiscalYearId))]
    public FiscalYear FiscalYear { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Products Product { get; set; }



}


