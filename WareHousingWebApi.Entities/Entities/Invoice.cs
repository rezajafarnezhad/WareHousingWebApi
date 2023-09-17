using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Invoice :BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int WareHouseId { get; set; }
    public int CustomerId { get; set; }
    public int fiscalYearId { get; set; }

    
    /// <summary>
    ///  یک فروش
    ///دو مرجوغی  
    /// 
    /// 
    /// 
    /// </summary>
    public byte InvoiceType { get; set; }

    public int InvoiceTotalPrice { get; set; }

    /// <summary>
    /// 1=  بسته شده
    /// 
    /// 
    /// </summary>
    public byte InvoiceStatus { get; set; }

    public DateTime Date { get; set; }
    public DateTime? ReturnDate { get; set; }

    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }


    [ForeignKey(nameof(fiscalYearId))]
    public FiscalYear FiscalYear { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }

    public ICollection<InvoiceItems> InvoiceItems { get; set; }

}