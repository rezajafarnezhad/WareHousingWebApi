using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.Entities.Base;



public interface IEntityObject
{

}

public abstract class BaseEntity : IEntityObject
{

    public string UserId { get; set; }
    public string CreateDateTime { get; set; }

    [ForeignKey("UserId")]
    public Users Users { get; set; }
}

