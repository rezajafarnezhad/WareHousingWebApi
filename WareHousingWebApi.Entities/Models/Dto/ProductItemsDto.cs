namespace WareHousingWebApi.Entities.Models.Dto;

public class ProductItemsDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    //قیمت خرید از تولید کننده
    public int PurchasePrice { get; set; }

    //قیمت فروش به عمده فروش با فروشگاه
    public int SalesPrice { get; set; }

    //قیمت مصرف کننده
    public int CoverPrice { get; set; }
    public int ProductStock { get; set; }
}