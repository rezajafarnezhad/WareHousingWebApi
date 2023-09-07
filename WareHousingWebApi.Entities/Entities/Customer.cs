using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Customer : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string CustomerFullName { get; set; }
    public string EconomicCode { get; set; }
    public string CustomerTell { get; set; }
    public string CustomerAddress { get; set; }
    public int WareHouseId { get; set; }
    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }



}