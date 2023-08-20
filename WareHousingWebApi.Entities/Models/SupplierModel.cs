using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class SupplierModel
{
       
    [Required(AllowEmptyStrings =false,ErrorMessage = "نام را وارد کنید")]
    public string SupplierName { get; set; }
    [Required(AllowEmptyStrings = false,ErrorMessage = "تلفن را وارد کنید")]
    public string SupplierTel { get; set; }
    public string SupplierDescription { get; set; }
    public string SupplierSite { get; set; }
}


public class SupplierEditModel : SupplierModel 
{
    public int SupplierId { get; set; }

}