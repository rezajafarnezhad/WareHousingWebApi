using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Supplier : BaseEntity
{
    [Key]
    public int SupplierId { get; set; }
    [Required]
    public string SupplierName { get; set;}
    public string SupplierDescription { get; set;}
    public string SupplierTel { get; set;}
    public string SupplierSite { get; set;}


}
