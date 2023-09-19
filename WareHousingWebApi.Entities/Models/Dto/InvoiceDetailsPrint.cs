namespace WareHousingWebApi.Entities.Models.Dto;

public class InvoiceDetailsPrint
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int WareHouseId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCustomerTell { get; set; }
    public string CustomerCustomerFullName { get; set; }
    public string CustomerCustomerAddress { get; set; }
    public DateTime Date { get; set; }
    public int InvoiceTotalPrice { get; set; }
    public List<InvoiceItemForDetails> InvoiceItems { get; set; }
}
public class InvoiceItemForDetails
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public string ProductProductName { get; set; }
    public int ProductProductCode { get; set; }
    public int SalesPrice { get; set; }
    public int Count { get; set; }

}