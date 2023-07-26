using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using WareHousingWebApi.Data.Migrations;

namespace WareHousingWebApi.Data.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}