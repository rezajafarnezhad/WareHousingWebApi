using System.ComponentModel.DataAnnotations;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Country : BaseEntity
{
    [Key]
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}