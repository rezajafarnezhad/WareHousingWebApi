using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class ProductLocation :BaseEntity
{
    [Key]
    public int Id { get; set; }
    public int WareHouseId { get; set; }
 
    public string ProductLocationAddress { get; set; }


    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }

}