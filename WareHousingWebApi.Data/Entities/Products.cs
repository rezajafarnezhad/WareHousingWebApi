using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Data.Contract;

namespace WareHousingWebApi.Data.Entities;

public class Products
{
    [Key]
    public string ProductId { get; set; }
    public int CountryId { get; set; }
    public int SupplierId { get; set; }
    public string ProductName { get; set; }
    public PackingType PackingType { get; set; }
    public string CountInPacking { get; set; }
    public string ProductWeight { get; set; }
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
