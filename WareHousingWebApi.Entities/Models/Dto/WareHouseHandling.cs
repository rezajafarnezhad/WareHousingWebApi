namespace WareHousingWebApi.Entities.Models.Dto;

public class WareHouseHandling
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    public int TotalProductCount { get; set; }
    public DateTime ExpiryDate { get; set; }
}
public class WareHouseHandling2
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    public int InventoryId { get; set; }
    public int TotalProductCount { get; set; }
    public DateTime ExpiryDate { get; set; }
}