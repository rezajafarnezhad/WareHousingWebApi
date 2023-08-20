using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models;

public class CreateProductLocation
{
    public int WareHouseId { get; set; }
    public string UserId { get; set; }
    [Required(ErrorMessage ="نام محل استقرار خالی نباشد ")]
    public string ProductLocationAddress { get; set; }
}

public class EditProductLocation : CreateProductLocation
{
    public int Id { get; set; }
}