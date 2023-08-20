using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHousingWebApi.Data.Entities;

public class ProductLocation
{
    [Key]
    public int Id { get; set; }
    public int WareHouseId { get; set; }
    public string UserId { get; set; }
    public string ProductLocationAddress { get; set; }
    public DateTime CreateDateTime { get; set; }


    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }

    [ForeignKey(nameof(UserId))]
    public Users Users { get; set; }
}