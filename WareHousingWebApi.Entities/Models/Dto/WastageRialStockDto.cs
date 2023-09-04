namespace WareHousingWebApi.Entities.Models.Dto;

public class WastageRialStockDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    //تعداد موجودی
    /// <summary>
    /// تعداد موجودی
    /// </summary>
    public int TotalWastageProductCount { get; set; }

    //
    /// <summary>
    /// موجودی ریالی کل کالا - خرید
    /// </summary>
    public int TotalWastagePurchasePrice { get; set; }

    //
    /// <summary>
    /// موجودی ریالی کل کالا - فروش
    /// </summary>
    public int TotalWastageSalePrice { get; set; }

    /// <summary>
    /// موجودی ریالی کل کالا - مصرف کننده
    /// </summary>
    public int TotalWastageCoverPrice { get; set; }

}