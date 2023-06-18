using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHousingWebApi.Data.Entities;

public class WareHouse
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Tell { get; set; }
    public string UserId { get; set; }
    public DateTime CreateDate { get; set; }

    [ForeignKey("UserId")]
    public Users Users { get; set; }
}