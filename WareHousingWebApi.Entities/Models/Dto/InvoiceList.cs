using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models.Dto;

public class InvoiceList
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int WareHouseId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCustomerTell { get; set; }
    public string CustomerCustomerFullName { get; set; }
    public string CustomerCustomerAddress { get; set; }
    public DateTime Date { get; set; }
    public byte InvoiceType { get; set; }
    public int InvoiceTotalPrice { get; set; } 
    public byte InvoiceStatus { get; set; }
    public DateTime? ReturnDate { get; set; }
}