using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CreateWareHouse
{
    [Required()]

    public string Name { get; set; }
    [Required()]

    public string Address { get; set; }
    [Required()]

    public string Tell { get; set; }
    [Required()]

    public string UserId { get; set; }
    public string CreateDateTime { get; set; }

}

public class EditWareHouse : CreateWareHouse
{
    public int Id { get; set; }
}