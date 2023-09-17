namespace WareHousingWebApi.Entities.Models.Dto;

public class InvoiceItemList
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public string ProductProductName { get; set; }
    public int ProductProductCode { get; set; }
    public int Count { get; set; }
    public int PurchasePrice { get; set; }
    public int SalesPrice { get; set; }
    public int CoverPrice { get; set; }
}