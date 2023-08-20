using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Entities;

public class Supplier
{
    [Key]
    public int SupplierId { get; set; }
    [Required]
    public string SupplierName { get; set;}
    public string SupplierDescription { get; set;}
    public string SupplierTel { get; set;}
    public string SupplierSite { get; set;}

}
