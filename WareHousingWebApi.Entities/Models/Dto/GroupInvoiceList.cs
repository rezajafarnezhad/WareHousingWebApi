namespace WareHousingWebApi.Entities.Models.Dto;

public class GroupInvoiceList
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    public int ProductCount { get; set; }
}