using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class InvoiceItems : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public int PurchasePrice { get; set; }
    public int SalesPrice { get; set; }
    public int CoverPrice { get; set; }


    [ForeignKey(nameof(ProductId))]
    public Products Product { get; set; }
    
    [ForeignKey(nameof(InvoiceId))]
    public Invoice Invoice { get; set; }

}