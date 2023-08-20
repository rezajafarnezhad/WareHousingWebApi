using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}