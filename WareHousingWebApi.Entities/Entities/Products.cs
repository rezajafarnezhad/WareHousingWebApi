using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Products : BaseEntity
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    public int ProductCode { get; set; }
    public int CountryId { get; set; }
    public int SupplierId { get; set; }
    public string ProductName { get; set; }
    public PackingType PackingType { get; set; }
    public int CountInPacking { get; set; }
    public int ProductWeight { get; set; }
    public string ProductImage { get; set; }
    [MaxLength(500,ErrorMessage ="حداکثر 500 کاراکتر")]
    public string ProductDescription { get; set; }
    
    /// <summary>
    /// 1 یخچالی
    /// 2 عاذی
    /// </summary>
    public byte IsRefregerator { get; set; }

   

  

    #region Rel

    [ForeignKey("CountryId")]
    public Country Country { get; set; }

    [ForeignKey("SupplierId")]
    public Supplier Supplier{ get; set; }

    
    
    #endregion

}
public enum PackingType
{
    Shell = 1,
    Cartoon = 2,
    Box = 3,
    BreakingPacking = 4,

}