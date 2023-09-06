using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class UserInWareHouse :BaseEntity
{
    [Key]
    public int UserInWareHouseId { get; set; }

    /// <summary>
    /// آیدی کاربری که در انار کار میکند
    /// </summary>
    public string UserIdInWareHouse { get; set; }
    public int WareHouseId { get; set; }

    [ForeignKey(nameof(UserIdInWareHouse))]
    public Users Users_WareHous { get; set; }

    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }

}