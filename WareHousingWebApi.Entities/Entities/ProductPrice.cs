using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class ProductPrice : BaseEntity
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
    public int FiscalYearId { get; set; }



    //تاریخ اعمال قیمت
    public DateTime ActionDate { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public FiscalYear FiscalYear { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Products Product { get; set; }



}


