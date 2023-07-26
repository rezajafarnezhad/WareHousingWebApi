public class InventoryStockModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    public int TotalProductCount { get; set; }
    public int TotalProductWaste{ get; set; }
}

public class InventoryQueryMaker
{
    public int FiscalYearId { get; set; }
    public int WareHouseId { get; set; }

}