using System.ComponentModel.DataAnnotations;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Setting : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string MySetting { get; set; }

}